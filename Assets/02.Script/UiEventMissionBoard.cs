using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiEventMissionBoard : MonoBehaviour
{
    [SerializeField]
    private UiEventMissionCell missionCell;

    [SerializeField]
    private Transform cellParent;

    private Dictionary<int, UiEventMissionCell> cellContainer = new Dictionary<int, UiEventMissionCell>();


    private void OnEnable()
    {
        CheckEventEnd();
    }

    private void CheckEventEnd()
    {
        var severTime = ServerData.userInfoTable.currentServerTime;
#if UNITY_EDITOR
#else
        if (severTime.Month == 1 && severTime.Day < 20)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage("1월 20일 부터 이벤트 시작!");
            return;
        }
#endif
        if (severTime.Month >= 3)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage("이벤트가 종료됐습니다.");
            return;
        }

    }

    private void Awake()
    {
        Initialize();
       // CheckChrisEvent();
    }
    //private void CheckChrisEvent()
    //{
    //    if (ServerData.goodsTable.TableDatas[GoodsTable.Hel].Value == 0)
    //    {
    //        ServerData.userInfoTable.TableDatas[UserInfoTable.chrisRefund].Value = 1;

    //        ServerData.userInfoTable.UpData(UserInfoTable.chrisRefund, false);

    //        return;
    //    }

    //    if (ServerData.userInfoTable.TableDatas[UserInfoTable.chrisRefund].Value == 1) return;

    //    ServerData.userInfoTable.TableDatas[UserInfoTable.chrisRefund].Value = 1;

    //    ServerData.eventMissionTable.TableDatas["Mission5"].clearCount.Value = 1;

    //    List<TransactionValue> transactions = new List<TransactionValue>();

    //    Param userinfoParam = new Param();

    //    userinfoParam.Add(UserInfoTable.chrisRefund, ServerData.userInfoTable.TableDatas[UserInfoTable.chrisRefund].Value);

    //    Param eventParam = new Param();

    //    eventParam.Add("Mission5", ServerData.eventMissionTable.TableDatas["Mission5"].ConvertToString());

    //    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));
    //    transactions.Add(TransactionValue.SetUpdate(EventMissionTable.tableName, EventMissionTable.Indate, eventParam));

    //    ServerData.SendTransaction(transactions);
    //}


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_NewYear).Value += 100;
        }

    }
#endif

    private void Initialize()
    {
    //     ServerData.userInfoTable.UpdateDdukGukCollectionComplete();
    //
    //     var tableData = TableManager.Instance.EventMission.dataArray;
    //
    //     for (int i = 0; i < tableData.Length; i++)
    //     {
    //         if (tableData[i].EVENTMISSIONTYPE != EventMissionType.FIRST) continue;
    //         var cell = Instantiate<UiEventMissionCell>(missionCell, cellParent);
    //
    //         cell.Initialize(tableData[i]);
    //
    //         cellContainer.Add(tableData[i].Id, cell);
    //     }
     }
}
