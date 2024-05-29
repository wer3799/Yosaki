using System;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.Serialization;

public class UiHalloweenEventMission : MonoBehaviour
{
    [FormerlySerializedAs("missionCell")] [SerializeField]
    private UiEventMission3Cell mission3Cell;
    [SerializeField]
    private Transform cellParent;
    [FormerlySerializedAs("missionCell2")] [SerializeField]
    private UiEventMission3Cell mission3Cell2;
    [SerializeField]
    private Transform cellParent2;

    private Dictionary<int, UiEventMission3Cell> cellContainer = new Dictionary<int, UiEventMission3Cell>();

    private void OnEnable()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION5].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }    
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION6].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        } 
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSumiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION7].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        } 
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION1].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION1].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION2].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.dailySleepRewardReceiveCount).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION3].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getGumGi).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION4].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getFlower).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION5].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION6].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION7].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.BMISSION8].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
    }


    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.EventMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].EVENTMISSIONTYPE != EventMissionType.AFIRST) continue;
            var cell = Instantiate<UiEventMission3Cell>(mission3Cell, cellParent2);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].EVENTMISSIONTYPE != EventMissionType.BSECOND) continue;
            var cell = Instantiate<UiEventMission3Cell>(mission3Cell2, cellParent);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
    }

    public void OnClickAllReceive()
    {
        var tableData = TableManager.Instance.EventMission.dataArray;
        int rewardedNum = 0;
        List<int> rewardTypeList = new List<int>();
        
        List<string> stringIdList = new List<string>();
        for (int i = 0; i < tableData.Length; i++)
        {
            
            if (tableData[i].EVENTMISSIONTYPE == EventMissionType.AFIRST ||
                tableData[i].EVENTMISSIONTYPE == EventMissionType.BSECOND)
            {
                
            }
            else
            {
                //FIRST나SECOND가 아니라면
                continue;
            }
            //Enable을 껐다면
            if (tableData[i].Enable == false) continue;
            //보상을 받았다면
            if (ServerData.eventMissionTable.CheckMissionRewardCount(tableData[i].Stringid) > 0) continue;
            //깨지 않았다면
            if (ServerData.eventMissionTable.CheckMissionClearCount(tableData[i].Stringid) <
                tableData[i].Rewardrequire) continue;

            //보상
            ServerData.eventMissionTable.TableDatas[tableData[i].Stringid].rewardCount.Value++;
            //패스사면 두배
            if (ServerData.iapServerTable.TableDatas[UiEvent2PassBuyButton.productKey].buyCount.Value > 0)
            {
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Rewardtype, tableData[i].Rewardvalue * 2);
            }
            else
            {
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Rewardtype, tableData[i].Rewardvalue);
                //삿을떄 보너스 추가
                ServerData.goodsTable.AddLocalData(GoodsTable.Event_Mission3_All, tableData[i].Rewardvalue);
            }

            if (rewardTypeList.Contains(tableData[i].Rewardtype) == false)
            {
                rewardTypeList.Add(tableData[i].Rewardtype);
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
            
            using var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            goodsParam.Add(GoodsTable.Event_Mission3_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission3_All).Value);;
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        
            using var stringName = stringIdList.GetEnumerator();
        
            Param eventMissionParam = new Param();
            while(stringName.MoveNext())
            {
                string updateValue = ServerData.eventMissionTable.TableDatas[stringName.Current].ConvertToString();
                eventMissionParam.Add(stringName.Current, updateValue);
            }
            transactions.Add(TransactionValue.SetUpdate(EventMissionTable.tableName, EventMissionTable.Indate, eventMissionParam));
    
            ServerData.SendTransactionV2(transactions, successCallBack: () =>
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
