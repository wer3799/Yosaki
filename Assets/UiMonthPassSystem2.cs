using System;
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
public class MonthlyPass2Data_Fancy
{
    public PassInfo passInfo { get; private set; }
    public MonthlyPass2Data_Fancy(PassInfo passData)
    {
        this.passInfo = passData;
    }
}
public class UiMonthPassSystem2 : FancyScrollView<MonthlyPass2Data_Fancy>
{

    private List<UiMonthlyPassCell2> uiPassCellContainer = new List<UiMonthlyPassCell2>();

    private ObscuredString passShopId;
    
    [SerializeField]
    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();
    
    [SerializeField] private UiRewardResultView _uiRewardResultView;
    [SerializeField]
    private SkeletonGraphic costumeGraphic;
    private Item_Type costumeType;
    private List<RewardItem> rewardList = new List<RewardItem>();

    [SerializeField] private List<MonthlyTrainingCell> _monthlyTrainingCells;
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.oddMonthKillCount).Value += 1000000;
        }
    }
#endif
    
    private void AddOrUpdateReward(Item_Type itemType, float itemValue)
    {
        int existingRewardIndex = rewardList.FindIndex(r => r.ItemType == itemType);

        if (existingRewardIndex >= 0)
        {
            RewardItem existingReward = rewardList[existingRewardIndex];
            existingReward.ItemValue += itemValue;
            rewardList[existingRewardIndex] = existingReward;
        }
        else
        {
            rewardList.Add(new RewardItem(itemType, itemValue));
        }
    }
    
    private void FindMonthCostume()
    {
        var tableData = TableManager.Instance.MonthlyPass2.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var item = (Item_Type)tableData[i].Reward2;
            if (item.IsCostumeItem())
            {
                costumeType = item;
                return;
            }
        }
    }
    private void SetMonthText()
    {
        textList[0].SetText($"월간 훈련({ServerData.userInfoTable.currentServerTime.Month}월)");
        textList[1].SetText($"월간 훈련 패스");
        textList[2].SetText($"월간 미션({ServerData.userInfoTable.currentServerTime.Month}월)");
        if (ServerData.userInfoTable.currentServerTime.Day < 23)
        {
            textList[3].SetText($"{ServerData.userInfoTable.currentServerTime.Month}월 23일부터 구매 가능");
        }
        else
        {
            textList[3].SetText($"요괴 처치수만 증가합니다.");
        }
        textList[4].SetText($"{ServerData.userInfoTable.currentServerTime.Month}월 훈련권 필요");
        textList[5].SetText($"종료 : {ServerData.userInfoTable.currentServerTime.Month}월 {DateTime.DaysInMonth(ServerData.userInfoTable.currentServerTime.Year,ServerData.userInfoTable.currentServerTime.Month)}일\n(100단위로 갱신됩니다!)");
        textList[6].SetText($"월간 훈련({ServerData.userInfoTable.currentServerTime.Month}월)");
        textList[7].SetText($"({ServerData.userInfoTable.currentServerTime.Month}월 01일 ~ {ServerData.userInfoTable.currentServerTime.Month}월 {DateTime.DaysInMonth(ServerData.userInfoTable.currentServerTime.Year,ServerData.userInfoTable.currentServerTime.Month)}일)");
        textList[8].SetText($"월간 훈련 구매 시, <color=#849e72ff>{CommonString.GetItemName(costumeType)}</color> 획득 가능 !\n월간 구매 혜택 및 월간 버프 사용 가능 !\n월간 훈련 패스로 다양한 보상 획득 가능!");
        textList[9].SetText($"{CommonString.GetItemName(costumeType)}");
        textList[10].SetText($"{ServerData.userInfoTable.currentServerTime.Month}월 한정 외형");

        var rewardData = TableManager.Instance.MonthReward.dataArray;
        var str0 = "[";
        for (int i = 0; i < rewardData.Length; i++)
        {
            if(rewardData[i].Monthsort!=true) continue;
            str0 += $"{rewardData[i].Description},";
        }

        if (str0.EndsWith(","))
        {
            str0 = str0.Substring(0, str0.Length - 1);
        }

        str0 += "]";
        _monthlyTrainingCells[0].SetText($"매일 소탕권 추가 지급(+{rewardData[0].Itemvalue}개)",str0);

        var buffData = TableManager.Instance.MonthBuff.dataArray;
        var str1 = "[";
        for (int i = 0; i < buffData.Length; i++)
        {
            if(buffData[i].Monthsort!=true) continue;
            str1 += $"{buffData[i].Description},";
        }
        if (str1.EndsWith(","))
        {
            str1 = str1.Substring(0, str1.Length - 1);
        }
        str1 += "]";
        _monthlyTrainingCells[1].SetText($"컨텐츠 소탕량 증가 +{buffData[0].Statusvalue*100}%",str1);
        
        _monthlyTrainingCells[2].SetText($"{CommonString.GetStatusName(StatusType.ExpGainPer)} +{GameBalance.MonthPass_Exp*100}",null);
        _monthlyTrainingCells[3].SetText($"{CommonString.GetStatusName(StatusType.GoldGainPer)} +{GameBalance.MonthPass_Gold*100}",null);
        _monthlyTrainingCells[4].SetText($"{CommonString.GetStatusName(StatusType.MagicStoneAddPer)} +{GameBalance.MonthPass_GrowthStone*100}",null);
        
        
        
    }

    private void SetCostume()
    {
        var idx = ServerData.costumeServerTable.TableDatas[costumeType.ToString()].idx;
        
        costumeGraphic.Clear();

        costumeGraphic.gameObject.SetActive(true);
        costumeGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[idx];
        costumeGraphic.Initialize(true);
        costumeGraphic.SetMaterialDirty();
    }

    public void OnClickAllReceiveButton()
    {
        var tableData = TableManager.Instance.MonthlyPass2.dataArray;
        //현재 index가 -1이라면 0번째 보상을 받아야하기 때문에 +1
        var passValue = int.Parse(ServerData.monthlyPassServerTable2
            .TableDatas[MonthlyPassServerTable2.MonthlypassFreeReward].Value) + 1;
        var adValue = int.Parse(ServerData.monthlyPassServerTable2
            .TableDatas[MonthlyPassServerTable2.MonthlypassAdReward].Value) + 1;

        List<int> rewardTypeList = new List<int>();


        int rewardedNum = 0;

        string free = ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassFreeReward]
            .Value;
        string ad = ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAdReward].Value;

        bool hasCostumeItem = false;
        bool hasPassItem = false;

        //받아야할 곳 부터 체크
        for (int i = passValue; i < tableData.Length; i++)
        {
            //요구 조건 안되면 break.
            if (CanGetReward(tableData[i].Unlockamount) == false) break;

            //받은적 있는지 체크
            if (HasReward(MonthlyPassServerTable2.MonthlypassFreeReward, tableData[i].Id) == false)
            {
                //코스튬이나 노리개가 있다면?
                if (((Item_Type)(tableData[i].Reward1)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                if (((Item_Type)(tableData[i].Reward1)).IsNorigaeItem())
                {
                    hasPassItem = true;
                    break;
                }
            }
        }

        //받은적 있는지 체크
        for (int i = adValue; i < tableData.Length; i++)
        {
            if (HasReward(MonthlyPassServerTable2.MonthlypassAdReward, tableData[i].Id) == false)
            {
                //코스튬이나 노리개가 있다면?
                if (((Item_Type)(tableData[i].Reward2)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                if (((Item_Type)(tableData[i].Reward2)).IsNorigaeItem())
                {
                    hasPassItem = true;
                    break;
                }
            }
        }

        if (hasCostumeItem)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 아이템은 직접 수령해야 합니다.", null);
            return;
        }

        if (hasPassItem)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "패스 보상 장비는 직접 수령해야 합니다.", null);
            return;
        }

        //받기
        for (int i = passValue; i < tableData.Length; i++)
        {
            //요구 조건 안되면 break.
            if (CanGetReward(tableData[i].Unlockamount) == false) break;

            //무료보상
            if (HasReward(MonthlyPassServerTable2.MonthlypassFreeReward, tableData[i].Id) == false)
            {
                free = $"{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                if (rewardTypeList.Contains(tableData[i].Reward1) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward1);
                }

                rewardedNum++;
            }
        }

        for (int i = adValue; i < tableData.Length; i++)
        {
            //요구 조건 안되면 break.
            if (CanGetReward(tableData[i].Unlockamount) == false) break;
            if (HasPassItem() == false) break;
            
            //유료보상
            if (HasReward(MonthlyPassServerTable2.MonthlypassAdReward, tableData[i].Id) == false)
            {
                ad = $"{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);
                if (rewardTypeList.Contains(tableData[i].Reward2) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward2);
                }

                rewardedNum++;
            }
        }
    


        if (rewardedNum > 0)
        {
            ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassFreeReward].Value = free;
            ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAdReward].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();

            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }


            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(MonthlyPassServerTable2.MonthlypassFreeReward, ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassFreeReward].Value);
            passParam.Add(MonthlyPassServerTable2.MonthlypassAdReward, ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAdReward].Value);

            transactions.Add(TransactionValue.SetUpdate(MonthlyPassServerTable2.tableName, MonthlyPassServerTable2.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
              //  LogManager.Instance.SendLogType("MonthPass", "A", "A");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }

    public void OnClickAllReceiveButtonV2()
    {
        var tableData = TableManager.Instance.MonthlyPass2.dataArray;
        //현재 index가 -1이라면 0번째 보상을 받아야하기 때문에 +1
        var passValue = int.Parse(ServerData.monthlyPassServerTable2
            .TableDatas[MonthlyPassServerTable2.MonthlypassFreeReward].Value) + 1;
        var adValue = int.Parse(ServerData.monthlyPassServerTable2
            .TableDatas[MonthlyPassServerTable2.MonthlypassAdReward].Value) + 1;

        List<int> rewardTypeList = new List<int>();


        int rewardedNum = 0;

        string free = ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassFreeReward]
            .Value;
        string ad = ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAdReward].Value;

        bool hasCostumeItem = false;
        bool hasPassItem = false;

        //받아야할 곳 부터 체크
        for (int i = passValue; i < tableData.Length; i++)
        {
            //요구 조건 안되면 break.
            if (CanGetReward(tableData[i].Unlockamount) == false) break;

            //받은적 있는지 체크
            if (HasReward(MonthlyPassServerTable2.MonthlypassFreeReward, tableData[i].Id) == false)
            {
                //코스튬이나 노리개가 있다면?
                if (((Item_Type)(tableData[i].Reward1)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                if (((Item_Type)(tableData[i].Reward1)).IsNorigaeItem())
                {
                    hasPassItem = true;
                    break;
                }
            }
        }

        //받은적 있는지 체크
        for (int i = adValue; i < tableData.Length; i++)
        {
            if (HasPassItem() == false) break;
            if (HasReward(MonthlyPassServerTable2.MonthlypassAdReward, tableData[i].Id) == false)
            {
                //코스튬이나 노리개가 있다면?
                if (((Item_Type)(tableData[i].Reward2)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                if (((Item_Type)(tableData[i].Reward2)).IsNorigaeItem())
                {
                    hasPassItem = true;
                    break;
                }
            }
        }

        if (hasCostumeItem)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 아이템은 직접 수령해야 합니다.", null);
            return;
        }

        if (hasPassItem)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "패스 보상 장비는 직접 수령해야 합니다.", null);
            return;
        }
        //보상모음 클리어
        rewardList.Clear();
        
        //받기
        for (int i = passValue; i < tableData.Length; i++)
        {
            //요구 조건 안되면 break.
            if (CanGetReward(tableData[i].Unlockamount) == false) break;

            //무료보상
            if (HasReward(MonthlyPassServerTable2.MonthlypassFreeReward, tableData[i].Id) == false)
            {
                free = $"{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                AddOrUpdateReward((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);

                if (rewardTypeList.Contains(tableData[i].Reward1) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward1);
                }

                rewardedNum++;
            }
        }

        for (int i = adValue; i < tableData.Length; i++)
        {
            //요구 조건 안되면 break.
            if (CanGetReward(tableData[i].Unlockamount) == false) break;
            if (HasPassItem() == false) break;
            
            //유료보상
            if (HasReward(MonthlyPassServerTable2.MonthlypassAdReward, tableData[i].Id) == false)
            {
                ad = $"{tableData[i].Id}";
                AddOrUpdateReward((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);

                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);
                if (rewardTypeList.Contains(tableData[i].Reward2) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward2);
                }

                rewardedNum++;
            }
        }
    


        if (rewardedNum > 0)
        {
            ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassFreeReward].Value = free;
            ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAdReward].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();

            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }


            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(MonthlyPassServerTable2.MonthlypassFreeReward, ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassFreeReward].Value);
            passParam.Add(MonthlyPassServerTable2.MonthlypassAdReward, ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAdReward].Value);

            transactions.Add(TransactionValue.SetUpdate(MonthlyPassServerTable2.tableName, MonthlyPassServerTable2.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                List<RewardData> rewardData = new List<RewardData>();
                var e = rewardList.GetEnumerator();
                for (int i = 0 ;  i < rewardList.Count;i++)
                {
                    if (e.MoveNext())
                    {
                        rewardData.Add(new RewardData(e.Current.ItemType,e.Current.ItemValue));
                    }                    
                }
                if (rewardData.Count > 0)
                {
                    _uiRewardResultView.gameObject.SetActive(true);
                    _uiRewardResultView.Initialize(rewardData);
                }
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
        
        
    }


    private bool CanGetReward(int require)
    {
        int killCountTotal = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.oddMonthKillCount).Value;
        return killCountTotal >= require;
    }
    public bool HasReward(string key, int data)
    {
        return int.Parse(ServerData.monthlyPassServerTable2.TableDatas[key].Value) >= data;
    }

    private bool HasPassItem()
    {
        return ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton2.monthPassKey].buyCount.Value > 0;
    }

    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.monthlyPassServerTable2.TableDatas[key].Value.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            if (int.TryParse(splits[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;
    }

    public void RefreshCells()
    {
        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            uiPassCellContainer[i].RefreshParent();
        }
    }
        
    //
    [SerializeField]
    private Scroller scroller;
    
    
    [SerializeField] GameObject cellPrefab = default;

    protected override GameObject CellPrefab => cellPrefab;
    private void Start()
    {
        FindMonthCostume();

        SetCostume();
        
        SetMonthText();
        
        scroller.Initialize(TypeScroll.MonthPass2);
        
        scroller.OnValueChanged(UpdatePosition);
    
        
        var tableData = TableManager.Instance.MonthlyPass2.dataArray;
    
        List<MonthlyPass2Data_Fancy> passInfos = new List<MonthlyPass2Data_Fancy>();
    
        for (int i = 0; i < tableData.Length; i++)
        {
            var passInfo = new PassInfo();
    
            passInfo.require = tableData[i].Unlockamount;
            passInfo.id = tableData[i].Id;
    
            passInfo.rewardType_Free = tableData[i].Reward1;
            passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
            passInfo.rewardType_Free_Key = MonthlyPassServerTable2.MonthlypassFreeReward;
    
            passInfo.rewardType_IAP = tableData[i].Reward2;
            passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
            passInfo.rewardType_IAP_Key = MonthlyPassServerTable2.MonthlypassAdReward;
            passInfos.Add(new MonthlyPass2Data_Fancy(passInfo));
    
        }
    
    
        this.UpdateContents(passInfos.ToArray());
        scroller.SetTotalCount(passInfos.Count);
    }
}
