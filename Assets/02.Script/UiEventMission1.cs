using System;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.Serialization;

public class UiEventMission1 : MonoBehaviour
{
    [SerializeField]
    private UiEventMission1Cell missionCell;
    [SerializeField]
    private Transform cellParent;
    [SerializeField]
    private Transform cellParent2;

    private Dictionary<int, UiEventMission1Cell> cellContainer = new Dictionary<int, UiEventMission1Cell>();

    [SerializeField] private GameObject getCostumeButton;
    
    string costumeKey = "costume167";

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.AsObservable().Subscribe(e =>
        {
            getCostumeButton.SetActive(!e);
        }).AddTo(this);
    }
    private void OnEnable()
    {
                 if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION5].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }    
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION6].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        } 
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION1].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION1].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION2].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.dailySleepRewardReceiveCount).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION3].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getGumGi).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION4].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getFlower).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION5].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION6].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION7].Stringid;
            ServerData.eventMissionTable.UpdateMissionClearToCount(key, 1);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value > 0)
        {
            string key = TableManager.Instance.EventMissionDatas[(int)EventMissionKey.FMISSION8].Stringid;
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
            if (tableData[i].EVENTMISSIONTYPE != EventMissionType.FOURTH) continue;
            var cell = Instantiate<UiEventMission1Cell>(missionCell, cellParent);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].EVENTMISSIONTYPE != EventMissionType.THIRD) continue;
            var cell = Instantiate<UiEventMission1Cell>(missionCell, cellParent2);

            cell.Initialize(tableData[i]);

            cellContainer.Add(tableData[i].Id, cell);
        }
    }

    public void OnClickReceiveCostume()
    {
        if (ServerData.iapServerTable.TableDatas[UiEventPassBuyButton.productKey].buyCount.Value < 1)
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
            
            if (tableData[i].EVENTMISSIONTYPE == EventMissionType.THIRD ||
                tableData[i].EVENTMISSIONTYPE == EventMissionType.FOURTH)
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
                //패스사면 두배
                if (ServerData.iapServerTable.TableDatas[UiEventPassBuyButton.productKey].buyCount.Value > 0)
                {
                    ServerData.AddLocalValue((Item_Type)(int)tableData[i].Rewardtype, tableData[i].Rewardvalue * 2);
                }
                else
                {
                    ServerData.AddLocalValue((Item_Type)(int)tableData[i].Rewardtype, tableData[i].Rewardvalue);
                    //삿을떄 보너스 추가
                    ServerData.goodsTable.AddLocalData(GoodsTable.Event_Mission1_All, tableData[i].Rewardvalue);
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
            goodsParam.Add(GoodsTable.Event_Mission1_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission1_All).Value);;
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
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }
}
