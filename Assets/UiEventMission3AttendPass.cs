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

public class UiEventMission3AttendPass : FancyScrollView<PassData_Fancy>
{
    private ObscuredString passShopId;
    [SerializeField]
    private TextMeshProUGUI attendCount;

    [SerializeField] private UiRewardResultView _uiRewardResultView;


    public void OnClickAllReceiveButton()
    {
        string freeKey = OneYearPassServerTable.event3AttendFree;
        string adKey = OneYearPassServerTable.event3AttendAd;

        List<int> splitData_Free = GetSplitData(OneYearPassServerTable.event3AttendFree);
        List<int> splitData_Ad = GetSplitData(OneYearPassServerTable.event3AttendAd);
        
        List<int> rewardTypeList = new List<int>();
        List<RewardItem> rewards = new List<RewardItem>();
        var tableData = TableManager.Instance.CommonEventAttend2.dataArray;

        int rewardedNum = 0;

        string free = ServerData.oneYearPassServerTable.TableDatas[OneYearPassServerTable.event3AttendFree].Value;
        string ad = ServerData.oneYearPassServerTable.TableDatas[OneYearPassServerTable.event3AttendAd].Value;

        bool hasCostumeItem = false;

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

                free += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                Utils.AddOrUpdateReward(ref rewards,(Item_Type)tableData[i].Reward1,tableData[i].Reward1_Value);
                if (rewardTypeList.Contains(tableData[i].Reward1) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward1);

                }
                rewardedNum++;
            }

            ////유료보상
            if (HasPassItem() && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward2)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                ad += $",{tableData[i].Id}";
                Utils.AddOrUpdateReward(ref rewards,(Item_Type)tableData[i].Reward2,tableData[i].Reward2_Value);
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);
                if (rewardTypeList.Contains(tableData[i].Reward2) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward2);
                }
                rewardedNum++;
            }
        }

        if (hasCostumeItem)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 아이템은 직접 수령해야 합니다.", null);
            return;
        }

        if (rewardedNum > 0)
        {
            ServerData.oneYearPassServerTable.TableDatas[OneYearPassServerTable.event3AttendFree].Value = free;
            ServerData.oneYearPassServerTable.TableDatas[OneYearPassServerTable.event3AttendAd].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();
            
            var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            Param passParam = new Param();

            passParam.Add(OneYearPassServerTable.event3AttendFree, ServerData.oneYearPassServerTable.TableDatas[OneYearPassServerTable.event3AttendFree].Value);
            passParam.Add(OneYearPassServerTable.event3AttendAd, ServerData.oneYearPassServerTable.TableDatas[OneYearPassServerTable.event3AttendAd].Value);

            transactions.Add(TransactionValue.SetUpdate(OneYearPassServerTable.tableName, OneYearPassServerTable.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                List<UiRewardView.RewardData> rewardData = new List<UiRewardView.RewardData>();
                var e2 = rewards.GetEnumerator();
                for (int i = 0 ;  i < rewards.Count;i++)
                {
                    if (e2.MoveNext())
                    {
                        rewardData.Add(new UiRewardView.RewardData(e2.Current.ItemType,e2.Current.ItemValue));
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
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiEvent2PassBuyButton.productKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    private bool CanGetReward(int require)
    {
        int killCountTotal = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.commonAttend2Count).Value;
        return killCountTotal >= require;
    }
    public bool HasReward(List<int> splitData, int id)
    {
        return splitData.Contains(id);
    }

    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.oneYearPassServerTable.TableDatas[key].Value.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            if (int.TryParse(splits[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;
    }


    [SerializeField]
    private Scroller scroller;
    
    
    [SerializeField] GameObject cellPrefab = default;

    protected override GameObject CellPrefab => cellPrefab;
    
    private void Start()
    {
        scroller.Initialize(TypeScroll.SnowManPass);
            
        scroller.OnValueChanged(UpdatePosition);
    
        var tableData = TableManager.Instance.CommonEventAttend2.dataArray;
    
        List<PassData_Fancy> passInfos = new List<PassData_Fancy>();
    
        for (int i = 0; i < tableData.Length; i++)
        {
            var passInfo = new PassInfo();
    
            passInfo.require = tableData[i].Unlockamount;
            passInfo.id = tableData[i].Id;
    
            passInfo.rewardType_Free = tableData[i].Reward1;
            passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
            passInfo.rewardType_Free_Key = OneYearPassServerTable.event3AttendFree;

            passInfo.rewardType_IAP = tableData[i].Reward2;
            passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
            passInfo.rewardType_IAP_Key = OneYearPassServerTable.event3AttendAd;
            passInfos.Add(new PassData_Fancy(passInfo));
        }
    
    
        this.UpdateContents(passInfos.ToArray());
        scroller.SetTotalCount(passInfos.Count);
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.commonAttend2Count).AsObservable().Subscribe(e =>
        {
            attendCount.SetText($"출석일 : {e} 일");
        }).AddTo(this);
    }
}
