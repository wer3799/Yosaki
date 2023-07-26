using System;
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
public class MonthlyPassData_Fancy
{
    public PassInfo passInfo { get; private set; }
    public MonthlyPassData_Fancy(PassInfo passData)
    {
        this.passInfo = passData;
    }
}
public class UiMonthPassSystem : FancyScrollView<MonthlyPassData_Fancy>
{
    [SerializeField]
    private UiMonthlyPassCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiMonthlyPassCell> uiPassCellContainer = new List<UiMonthlyPassCell>();

    private ObscuredString passShopId;
    [SerializeField]
    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();
    
    [SerializeField] private UiRewardResultView _uiRewardResultView;
    private List<UiLevelPassBoard.Reward> rewardList = new List<UiLevelPassBoard.Reward>();
    
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.evenMonthKillCount).Value += 1000000;
        }
    }
#endif

    private void AddOrUpdateReward(Item_Type itemType, float itemValue)
    {
        int existingRewardIndex = rewardList.FindIndex(r => r.ItemType == itemType);

        if (existingRewardIndex >= 0)
        {
            UiLevelPassBoard.Reward existingReward = rewardList[existingRewardIndex];
            existingReward.ItemValue += itemValue;
            rewardList[existingRewardIndex] = existingReward;
        }
        else
        {
            rewardList.Add(new UiLevelPassBoard.Reward(itemType, itemValue));
        }
    }
    
    private void Initialize()
    {
        var tableData = TableManager.Instance.MonthlyPass.dataArray;
        
        int interval = tableData.Length - uiPassCellContainer.Count;
        
        for (int i = 0; i < interval; i++)
        {
            var prefab = Instantiate<UiMonthlyPassCell>(uiPassCellPrefab, cellParent);
            uiPassCellContainer.Add(prefab);
        }
        
        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            if (i < tableData.Length)
            {
                var passInfo = new PassInfo();
        
                passInfo.require = tableData[i].Unlockamount;
                passInfo.id = tableData[i].Id;
        
                passInfo.rewardType_Free = tableData[i].Reward1;
                passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
                passInfo.rewardType_Free_Key = MonthlyPassServerTable.MonthlypassFreeReward;
        
                passInfo.rewardType_IAP = tableData[i].Reward2;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = MonthlyPassServerTable.MonthlypassAdReward;
        
                uiPassCellContainer[i].gameObject.SetActive(true);
                uiPassCellContainer[i].Initialize(passInfo);
            }
            else
            {
                uiPassCellContainer[i].gameObject.SetActive(false);
            }
        }

        cellParent.transform.localPosition = new Vector3(0f, cellParent.transform.localPosition.y, cellParent.transform.localPosition.z);
        
    }

    private void SetMonthText()
    {
        textList[0].SetText($"월간 훈련({ServerData.userInfoTable.currentServerTime.Month}월)");
        textList[1].SetText($"월간 출석({ServerData.userInfoTable.currentServerTime.Month}월)");
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
    }
    public void OnClickAllReceiveButton()
    {
        string freeKey = MonthlyPassServerTable.MonthlypassFreeReward;
        string adKey = MonthlyPassServerTable.MonthlypassAdReward;

        List<int> splitData_Free = GetSplitData(MonthlyPassServerTable.MonthlypassFreeReward);
        List<int> splitData_Ad = GetSplitData(MonthlyPassServerTable.MonthlypassAdReward);

        List<int> rewardTypeList = new List<int>();

        var tableData = TableManager.Instance.MonthlyPass.dataArray;

        int rewardedNum = 0;

        string free = ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassFreeReward].Value;
        string ad = ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAdReward].Value;

        bool hasCostumeItem = false;
        bool hasPassItem = false;

        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockamount);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward1)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }
                if (((Item_Type)(tableData[i].Reward1)).IsPassNorigaeItem())
                {
                    hasPassItem = true;
                    break;
                }
            }

            //유료보상
            if (HasPassItem() && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward2)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }
                if (((Item_Type)(tableData[i].Reward2)).IsPassNorigaeItem())
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

        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockamount);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward1)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }
                if (((Item_Type)(tableData[i].Reward1)).IsPassNorigaeItem())
                {
                    hasPassItem = true;
                    break;
                }

                free += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                if(rewardTypeList.Contains(tableData[i].Reward1)==false)
                {
                    rewardTypeList.Add(tableData[i].Reward1);
                }
                

                rewardedNum++;
            }

            //유료보상
            if (HasPassItem() && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward2)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }
                if (((Item_Type)(tableData[i].Reward2)).IsPassNorigaeItem())
                {
                    hasPassItem = true;
                    break;
                }

                ad += $",{tableData[i].Id}";
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
            ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassFreeReward].Value = free;
            ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAdReward].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while(e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }

            //goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
            //goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
            //goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
            //goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
            //goodsParam.Add(GoodsTable.SmithFire, ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);
            //goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);
            //goodsParam.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
            //goodsParam.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);
            //goodsParam.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);
            //goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);
            //goodsParam.Add(GoodsTable.SumiFire, ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(MonthlyPassServerTable.MonthlypassFreeReward, ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassFreeReward].Value);
            passParam.Add(MonthlyPassServerTable.MonthlypassAdReward, ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAdReward].Value);

            transactions.Add(TransactionValue.SetUpdate(MonthlyPassServerTable.tableName, MonthlyPassServerTable.Indate, passParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
              //  LogManager.Instance.SendLogType("MonthPass", "A", "A");//
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }
public void OnClickAllReceiveButtonV2()
    {
        string freeKey = MonthlyPassServerTable.MonthlypassFreeReward;
        string adKey = MonthlyPassServerTable.MonthlypassAdReward;

        List<int> splitData_Free = GetSplitData(MonthlyPassServerTable.MonthlypassFreeReward);
        List<int> splitData_Ad = GetSplitData(MonthlyPassServerTable.MonthlypassAdReward);

        List<int> rewardTypeList = new List<int>();

        var tableData = TableManager.Instance.MonthlyPass.dataArray;

        int rewardedNum = 0;

        string free = ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassFreeReward].Value;
        string ad = ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAdReward].Value;

        bool hasCostumeItem = false;
        bool hasPassItem = false;

        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockamount);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward1)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }
                if (((Item_Type)(tableData[i].Reward1)).IsPassNorigaeItem())
                {
                    hasPassItem = true;
                    break;
                }
            }

            //유료보상
            if (HasPassItem() && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward2)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }
                if (((Item_Type)(tableData[i].Reward2)).IsPassNorigaeItem())
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

        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockamount);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward1)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }
                if (((Item_Type)(tableData[i].Reward1)).IsPassNorigaeItem())
                {
                    hasPassItem = true;
                    break;
                }

                free += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                AddOrUpdateReward((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                if(rewardTypeList.Contains(tableData[i].Reward1)==false)
                {
                    rewardTypeList.Add(tableData[i].Reward1);
                }
                

                rewardedNum++;
            }

            //유료보상
            if (HasPassItem() && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward2)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }
                if (((Item_Type)(tableData[i].Reward2)).IsPassNorigaeItem())
                {
                    hasPassItem = true;
                    break;
                }

                ad += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);
                AddOrUpdateReward((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);

                if (rewardTypeList.Contains(tableData[i].Reward2) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward2);
                }

                rewardedNum++;
            }
        }

        

        if (rewardedNum > 0)
        {
            ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassFreeReward].Value = free;
            ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAdReward].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while(e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }


            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(MonthlyPassServerTable.MonthlypassFreeReward, ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassFreeReward].Value);
            passParam.Add(MonthlyPassServerTable.MonthlypassAdReward, ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAdReward].Value);

            transactions.Add(TransactionValue.SetUpdate(MonthlyPassServerTable.tableName, MonthlyPassServerTable.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                //PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
              //  LogManager.Instance.SendLogType("MonthPass", "A", "A");//
              List<UiRewardView.RewardData> rewardData = new List<UiRewardView.RewardData>();
              var e = rewardList.GetEnumerator();
              for (int i = 0 ;  i < rewardList.Count;i++)
              {
                  if (e.MoveNext())
                  {
                      rewardData.Add(new UiRewardView.RewardData(e.Current.ItemType,e.Current.ItemValue));
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
        int killCountTotal = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.evenMonthKillCount).Value;
        return killCountTotal >= require;
    }
    public bool HasReward(List<int> splitData, int id)
    {
        return splitData.Contains(id);
    }

    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton.monthPassKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.monthlyPassServerTable.TableDatas[key].Value.Split(',');

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
        SetMonthText();
        
        scroller.Initialize(TypeScroll.MonthPass);
        
        scroller.OnValueChanged(UpdatePosition);
    
        
        var tableData = TableManager.Instance.MonthlyPass.dataArray;
    
        List<MonthlyPassData_Fancy> passInfos = new List<MonthlyPassData_Fancy>();
    
        for (int i = 0; i < tableData.Length; i++)
        {
            var passInfo = new PassInfo();
    
            passInfo.require = tableData[i].Unlockamount;
            passInfo.id = tableData[i].Id;
    
            passInfo.rewardType_Free = tableData[i].Reward1;
            passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
            passInfo.rewardType_Free_Key = MonthlyPassServerTable.MonthlypassFreeReward;
    
            passInfo.rewardType_IAP = tableData[i].Reward2;
            passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
            passInfo.rewardType_IAP_Key = MonthlyPassServerTable.MonthlypassAdReward;
            passInfos.Add(new MonthlyPassData_Fancy(passInfo));
    
        }
    
    
        this.UpdateContents(passInfos.ToArray());
        scroller.SetTotalCount(passInfos.Count);
    }
}