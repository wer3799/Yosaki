using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
public class UiMonthPassAttendSystem : MonoBehaviour
{
    [SerializeField]
    private UiMonthlyPassAttendCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiMonthlyPassAttendCell> uiPassCellContainer = new List<UiMonthlyPassAttendCell>();

    private ObscuredString passShopId;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.monthAttendCount).Value += 1;
        }
    }
#endif

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.MonthlyPassAttend.dataArray;

        int interval = tableData.Length - uiPassCellContainer.Count;

        for (int i = 0; i < interval; i++)
        {
            var prefab = Instantiate<UiMonthlyPassAttendCell>(uiPassCellPrefab, cellParent);
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
                passInfo.rewardType_Free_Key = MonthlyPassServerTable.MonthlypassAttendFreeReward;

                passInfo.rewardType_IAP = tableData[i].Reward2;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = MonthlyPassServerTable.MonthlypassAttendAdReward;

                uiPassCellContainer[i].gameObject.SetActive(true);
                uiPassCellContainer[i].Initialize(passInfo);
            }
            else
            {
                uiPassCellContainer[i].gameObject.SetActive(false);
            }
        }

        // cellParent.transform.localPosition = new Vector3(0f, cellParent.transform.localPosition.y, cellParent.transform.localPosition.z);
    }

    public void OnClickAllReceiveButton()
    {
        string freeKey = MonthlyPassServerTable.MonthlypassAttendFreeReward;
        string adKey = MonthlyPassServerTable.MonthlypassAttendAdReward;


        var tableData = TableManager.Instance.MonthlyPassAttend.dataArray;

        int rewardedNum = 0;

        var free = int.Parse(ServerData.monthlyPassServerTable.TableDatas[freeKey].Value);
        var ad = int.Parse(ServerData.monthlyPassServerTable.TableDatas[adKey].Value);
 
        UiRewardResultPopUp.Instance.Clear();

        for (int i = free + 1; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockamount);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(freeKey, tableData[i].Id) == false)
            {
                free = tableData[i].Id;
                UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
            }
        }

        if (HasPassItem())
        {
            for (int i = ad + 1; i < tableData.Length; i++)
            {
                bool canGetReward = CanGetReward(tableData[i].Unlockamount);

                if (canGetReward == false) break;

                //무료보상
                if (HasReward(adKey, tableData[i].Id) == false)
                {
                    ad = tableData[i].Id;
                    UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);
                }
            }
        }


        if ( UiRewardResultPopUp.Instance.RewardList.Count> 0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            Param passParam = new Param();
            
            ServerData.monthlyPassServerTable.TableDatas[freeKey].Value = $"{free}";
            ServerData.monthlyPassServerTable.TableDatas[adKey].Value = $"{ad}";
            
            passParam.Add(freeKey, ServerData.monthlyPassServerTable.TableDatas[freeKey].Value);
            passParam.Add(adKey, ServerData.monthlyPassServerTable.TableDatas[adKey].Value);
            transactions.Add(TransactionValue.SetUpdate(MonthlyPassServerTable.tableName, MonthlyPassServerTable.Indate, passParam));

            using var e = UiRewardResultPopUp.Instance.RewardList.GetEnumerator();

            while (e.MoveNext())
            {
                var goodsName = ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType);
                ServerData.AddLocalValue(e.Current.itemType,e.Current.amount);

                goodsParam.Add(goodsName, ServerData.goodsTable.GetTableData(goodsName).Value);
            }

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));


            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                UiRewardResultPopUp.Instance.Show().Clear();
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }


    private bool CanGetReward(int require)
    {
        return (int)ServerData.userInfoTable.GetTableData(UserInfoTable.monthAttendCount).Value >= require;
    }
    public bool HasReward(string key, int id)
    {
        return int.Parse(ServerData.monthlyPassServerTable.TableDatas[key].Value) >= id;
    }

    private bool HasPassItem()
    {
        return  ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton.monthPassKey].buyCount.Value > 0;
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
}
