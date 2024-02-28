                                                         using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiMonthMissionBoard : MonoBehaviour
{
    [SerializeField]
    private UiMonthMissionCell missionCell;

    [SerializeField]
    private Transform cellParent;

    private Dictionary<int, UiMonthMissionCell> cellContainer = new Dictionary<int, UiMonthMissionCell>();


    private void OnEnable()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).Value > 0)
        {
            string key = TableManager.Instance.MonthMissionDatas[(int)MonthMissionKey.ClearChunFlower].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 10);
        }    
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).Value > 0)
        {
            string key = TableManager.Instance.MonthMissionDatas[(int)MonthMissionKey.ClearDokebiFire].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 10);
        }    
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSumiFire).Value > 0)
        {
            string key = TableManager.Instance.MonthMissionDatas[(int)MonthMissionKey.ClearSumiFire].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        } 
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).Value > 0)
        {
            string key = TableManager.Instance.MonthMissionDatas[(int)MonthMissionKey.ClearHell].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }    
    }

    private void Awake()
    {
        Initialize();
    }


    private void Initialize()
    {
        var tableData = TableManager.Instance.MonthMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiMonthMissionCell>(missionCell, cellParent);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
    }
    public void OnClickAllReceive()
    {
        var tableData = TableManager.Instance.MonthMission.dataArray;
        int rewardedNum = 0;
        List<int> rewardTypeList = new List<int>();
        
        List<string> stringIdList = new List<string>();
        for (int i = 0; i < tableData.Length; i++)
        {
            //Enable을 껐다면
            if (tableData[i].Enable == false) continue;
            //보상을 받았다면
            if (ServerData.eventMissionTable.CheckMissionRewardCount(tableData[i].Stringid) > 0) continue;
            //깨지 않았다면
            if (ServerData.eventMissionTable.CheckMissionClearCount(tableData[i].Stringid) <
                tableData[i].Rewardrequire) continue;

            //보상
            ServerData.eventMissionTable.TableDatas[tableData[i].Stringid].rewardCount.Value++;
            
            ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);

            if (rewardTypeList.Contains(tableData[i].Reward1) == false)
            {
                rewardTypeList.Add(tableData[i].Reward1);
            }
            if (stringIdList.Contains(tableData[i].Stringid) == false)
            {
                stringIdList.Add(tableData[i].Stringid);
            }
            rewardedNum++;
        }

        if (rewardedNum > 0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();
            
            var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        
            using var stringName = stringIdList.GetEnumerator();
        
            Param eventMissionParam = new Param();
            while(stringName.MoveNext())
            {
                string updateValue = ServerData.eventMissionTable.TableDatas[stringName.Current].ConvertToString();
                eventMissionParam.Add(stringName.Current, updateValue);
            }
            transactions.Add(TransactionValue.SetUpdate(EventMissionTable.tableName, EventMissionTable.Indate, eventMissionParam));
    
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                //LogManager.Instance.SendLogType("ChildPass", "A", "A");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }
}
