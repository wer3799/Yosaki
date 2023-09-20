using System;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.Serialization;

public class UiEventMission : MonoBehaviour
{
    [SerializeField]
    private UiEventMission2Cell missionCell;
    [SerializeField]
    private Transform cellParent;
    [SerializeField]
    private UiEventMission2Cell missionCell2;
    [SerializeField]
    private Transform cellParent2;

    private Dictionary<int, UiEventMission2Cell> cellContainer = new Dictionary<int, UiEventMission2Cell>();

    string costumeKey = "costume137";
    private void OnEnable()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.SMISSION6].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }    
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.SMISSION7].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }    
        
    }


    private void Awake()
    {
        Initialize();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission).Value += 100;
        }

    }
#endif

    private void Initialize()
    {
        var tableData = TableManager.Instance.EventMission.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].EVENTMISSIONTYPE != EventMissionType.SECOND) continue;
            var cell = Instantiate<UiEventMission2Cell>(missionCell, cellParent);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].EVENTMISSIONTYPE != EventMissionType.FIRST) continue;
            var cell = Instantiate<UiEventMission2Cell>(missionCell, cellParent2);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
    }

    public void OnClickReceiveCostume()
    {
        if (ServerData.iapServerTable.TableDatas["vacationpass"].buyCount.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("패스권이 필요합니다!");
            return;
        }
        
 
        
        if(ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.Value==true)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유중입니다!");
            return;
        };
        ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.Value = true;
        
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param costumeParam = new Param();

        costumeParam.Add(costumeKey.ToString(), ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));


        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!!", null);
        });
    }
    
    public void OnClickAllReceive()
    {
        var tableData = TableManager.Instance.EventMission.dataArray;
        int rewardedNum = 0;
        List<int> rewardTypeList = new List<int>();
        
        List<string> stringIdList = new List<string>();
        for (int i = 0; i < tableData.Length; i++)
        {
            
            if (tableData[i].EVENTMISSIONTYPE == EventMissionType.FIRST ||
                tableData[i].EVENTMISSIONTYPE == EventMissionType.SECOND)
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
            if (ServerData.iapServerTable.TableDatas[UiEventPassBuyButton.productKey].buyCount.Value > 0)
            {
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Rewardtype, tableData[i].Rewardvalue * 2);
            }
            else
            {
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Rewardtype, tableData[i].Rewardvalue);
                //삿을떄 보너스 추가
                ServerData.goodsTable.AddLocalData(GoodsTable.Event_Mission_All, tableData[i].Rewardvalue);
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
            
            var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            goodsParam.Add(GoodsTable.Event_Mission_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission_All).Value);;
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
