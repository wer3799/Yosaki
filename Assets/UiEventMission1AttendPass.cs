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

public class UiEventMission1AttendPass : FancyScrollView<PassData_Fancy>
{
    private ObscuredString passShopId;
    [SerializeField]
    private TextMeshProUGUI attendCount;
    [SerializeField] private UiRewardResultView _uiRewardResultView;

    
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventMission1AttendCount).Value += 1;
        }
    }
#endif

    private void OnEnable()
    {
        if (ServerData.userInfoTable.IsMissionEventPeriod() == false)
        {
            this.gameObject.SetActive(false);
            return;
        }
    }

    public void OnClickAllReceiveButton()
    {
        string freeKey = OneYearPassServerTable.event1AttendFree;
        string adKey = OneYearPassServerTable.event1AttendAd;

        List<int> rewardTypeList = new List<int>();
        
        var tableData = TableManager.Instance.CommonEventAttend.dataArray;
        
        var passValue = int.Parse(ServerData.oneYearPassServerTable.TableDatas[freeKey].Value) + 1;
        var adValue = int.Parse(ServerData.oneYearPassServerTable.TableDatas[adKey].Value) + 1;

        List<RewardItem> rewards = new List<RewardItem>();


        int rewardedNum = 0;

        string free = ServerData.oneYearPassServerTable.TableDatas[freeKey]
            .Value;
        string ad = ServerData.oneYearPassServerTable.TableDatas[adKey].Value;

        bool hasCostumeItem = false;
        bool hasPassItem = false;

        //받아야할 곳 부터 체크
        for (int i = passValue; i < tableData.Length; i++)
        {
            //요구 조건 안되면 break.
            if (CanGetReward(tableData[i].Unlockamount) == false) break;

            //받은적 있는지 체크
            if (HasReward(freeKey, tableData[i].Id) == false)
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
            if (HasReward(adKey, tableData[i].Id) == false)
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
        rewards.Clear();
        
        //받기
        for (int i = passValue; i < tableData.Length; i++)
        {
            //요구 조건 안되면 break.
            if (CanGetReward(tableData[i].Unlockamount) == false) break;

            //무료보상
            if (HasReward(freeKey, tableData[i].Id) == false)
            {
                free = $"{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                Utils.AddOrUpdateReward(ref rewards, (Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);

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
            if (HasReward(adKey, tableData[i].Id) == false)
            {
                ad = $"{tableData[i].Id}";
                Utils.AddOrUpdateReward(ref rewards,(Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);

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
            ServerData.oneYearPassServerTable.TableDatas[freeKey].Value = free;
            ServerData.oneYearPassServerTable.TableDatas[adKey].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            using var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();

            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }


            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(freeKey, ServerData.oneYearPassServerTable.TableDatas[freeKey].Value);
            passParam.Add(adKey, ServerData.oneYearPassServerTable.TableDatas[adKey].Value);

            transactions.Add(TransactionValue.SetUpdate(OneYearPassServerTable.tableName, OneYearPassServerTable.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                List<RewardData> rewardData = new List<RewardData>();
                using var e = rewards.GetEnumerator();
                for (int i = 0 ;  i < rewards.Count;i++)
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

    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiEventPassBuyButton.productKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    private bool CanGetReward(int require)
    {
        int killCountTotal = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventMission1AttendCount).Value;
        return killCountTotal >= require;
    }
    public bool HasReward(string key, int data)
    {
        return int.Parse(ServerData.oneYearPassServerTable.TableDatas[key].Value) >= data;
    }



    [SerializeField]
    private Scroller scroller;
    
    
    [SerializeField] GameObject cellPrefab = default;

    protected override GameObject CellPrefab => cellPrefab;
    
    private void Start()
    {
        scroller.Initialize(TypeScroll.SnowManPass);
            
        scroller.OnValueChanged(UpdatePosition);
    
        var tableData = TableManager.Instance.CommonEventAttend.dataArray;
    
        List<PassData_Fancy> passInfos = new List<PassData_Fancy>();
    
        for (int i = 0; i < tableData.Length; i++)
        {
            var passInfo = new PassInfo();
    
            passInfo.require = tableData[i].Unlockamount;
            passInfo.id = tableData[i].Id;
    
            passInfo.rewardType_Free = tableData[i].Reward1;
            passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
            passInfo.rewardType_Free_Key = OneYearPassServerTable.event1AttendFree;

            passInfo.rewardType_IAP = tableData[i].Reward2;
            passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
            passInfo.rewardType_IAP_Key = OneYearPassServerTable.event1AttendAd;
            passInfos.Add(new PassData_Fancy(passInfo));
        }
    
    
        this.UpdateContents(passInfos.ToArray());
        scroller.SetTotalCount(passInfos.Count);
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventMission1AttendCount).AsObservable().Subscribe(e =>
        {
            attendCount.SetText($"출석일 : {e} 일");
        }).AddTo(this);
    }
}
