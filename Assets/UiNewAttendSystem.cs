using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UiNewAttendSystem : MonoBehaviour
{
    [SerializeField]
    private UiNewAttendCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiNewAttendCell> uiPassCellContainer = new List<UiNewAttendCell>();

    private ObscuredString passShopId;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value += 1;
        }
    }
#endif

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.AttendanceReward.dataArray;

        for (int i = 0; i < 30; i++)
        {
            var prefab = Instantiate<UiNewAttendCell>(uiPassCellPrefab, cellParent);
            uiPassCellContainer.Add(prefab);
        }

        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            if (i < tableData.Length)
            {
                var passInfo = new PassInfo();
                int adjustCount = 0;
                if (ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value > 10)
                {
                    adjustCount += (int)ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value - 10;
                }
                if(i + adjustCount + 1>=tableData.Length)
                {
                    uiPassCellContainer[i].gameObject.SetActive(false);
                    continue;
                }
                passInfo.require = tableData[i + adjustCount + 1].Id;
                passInfo.id = tableData[i + adjustCount + 1].Id;

                passInfo.rewardType_Free = tableData[i + adjustCount + 1].Reward_Type;
                passInfo.rewardTypeValue_Free = tableData[i + adjustCount + 1].Reward_Value;
                passInfo.rewardType_Free_Key = AttendanceServerTable.attendFree;

                passInfo.rewardType_IAP = tableData[i + adjustCount + 1].Reward_Type1;
                passInfo.rewardTypeValue_IAP = tableData[i + adjustCount + 1].Reward_Value1;
                passInfo.rewardType_IAP_Key = AttendanceServerTable.attendAd;

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
        int attendIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value;
        
        int freeIdx = int.Parse(ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendFree].Value);
        
        int adIdx = int.Parse(ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendAd].Value);
        
        var tableData = TableManager.Instance.AttendanceReward.dataArray;

        int rewardedNum = 0;

        List<int> ItemTypeList = new List<int>();

        if (attendIdx > freeIdx)
        {
            for (int i = freeIdx+1; i <= attendIdx; i++)
            {
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward_Type, tableData[i].Reward_Value);
                rewardedNum++;
                if (ItemTypeList.Contains(tableData[i].Reward_Type) == false)
                {
                    ItemTypeList.Add(tableData[i].Reward_Type);
                }
            }

            ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendFree].Value= $"{attendIdx}";
        }

        if (HasPassItem())
        {
            if (attendIdx > adIdx)
            {
                for (int i = adIdx+1; i <= attendIdx; i++)
                {
                    ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward_Type1, tableData[i].Reward_Value1);
                    rewardedNum++;
                    if (ItemTypeList.Contains(tableData[i].Reward_Type1) == false)
                    {
                        ItemTypeList.Add(tableData[i].Reward_Type1);
                    }
                }

                ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendAd].Value= $"{attendIdx}";
            }
        }
        

        if (rewardedNum > 0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            using var e = ItemTypeList.GetEnumerator();
            Param goodsParam = new Param();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current)).Value);
            }

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(AttendanceServerTable.attendFree, ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendFree].Value);
            passParam.Add(AttendanceServerTable.attendAd, ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendAd].Value);

            transactions.Add(TransactionValue.SetUpdate(AttendanceServerTable.tableName, AttendanceServerTable.Indate, passParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }


    private bool CanGetReward(int require)
    {
        int killCountTotal = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value;
        return killCountTotal >= require;
    }
    public bool HasReward(List<int> splitData, int id)
    {
        return splitData.Contains(id);
    }

    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiNewAttendBuyButton.productKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.attendanceServerTable.TableDatas[key].Value.Split(',');

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
