using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;

public class UiYoSaKiMarbleMissionCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI gaugeText;

    [SerializeField]
    private Button getButton;

    [SerializeField]
    private TextMeshProUGUI rewardNum;

    [SerializeField]
    private TextMeshProUGUI exchangeNum;

    private EventMissionData tableData;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField] private Image ItemIcon;

    private int getAmountFactor;
    public void Initialize(EventMissionData tableData)
    {
        if (tableData.Enable == false)
        {
            this.gameObject.SetActive(false);
            return;
        }

        this.tableData = tableData;

        if (tableData.EVENTMISSIONTYPE == EventMissionType.NORMALMARBLE)
        {
            exchangeNum.SetText($"매일 교환 : {ServerData.eventMissionTable.TableDatas[tableData.Stringid].rewardCount}/{TableManager.Instance.EventMission.dataArray[tableData.Id].Dailymaxclear}");
        }
        else if(tableData.EVENTMISSIONTYPE == EventMissionType.FINISHMARBLE)
        {
            exchangeNum.SetText($"이벤트 기간 내 1회 획득 : {ServerData.eventMissionTable.TableDatas[tableData.Stringid].rewardCount}/{TableManager.Instance.EventMission.dataArray[tableData.Id].Dailymaxclear}");
        }

        title.SetText(tableData.Title);

        ItemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Rewardtype);
        
        Subscribe();
    }

    
    private void Subscribe()
    {
        ServerData.eventMissionTable.TableDatas[tableData.Stringid].clearCount.AsObservable().Subscribe(WhenMissionCountChanged).AddTo(this);
        ServerData.eventMissionTable.TableDatas[tableData.Stringid].rewardCount.AsObservable().Subscribe(e=>
        {
            if (tableData.EVENTMISSIONTYPE == EventMissionType.NORMALMARBLE)
            {
                exchangeNum.SetText($"매일 교환 : {ServerData.eventMissionTable.TableDatas[tableData.Stringid].rewardCount}/{TableManager.Instance.EventMission.dataArray[tableData.Id].Dailymaxclear}");
            }
            else if(tableData.EVENTMISSIONTYPE == EventMissionType.FINISHMARBLE)
            {
                exchangeNum.SetText($"이벤트 기간 내 1회 획득 : {ServerData.eventMissionTable.TableDatas[tableData.Stringid].rewardCount}/{TableManager.Instance.EventMission.dataArray[tableData.Id].Dailymaxclear}");
            }
             
            if(e>=TableManager.Instance.EventMission.dataArray[tableData.Id].Dailymaxclear)
            {
                lockMask.SetActive(true);
            }
            else
            {
                lockMask.SetActive(false);
            }
        }).AddTo(this);
    }

    private void OnEnable()
    {
        if (tableData != null)
        {
            WhenMissionCountChanged(ServerData.eventMissionTable.TableDatas[tableData.Stringid].clearCount.Value);
        }

    }

    private void WhenMissionCountChanged(int count)
    {
        if (this.gameObject.activeInHierarchy == false) return;


        gaugeText.SetText($"{count}/{tableData.Rewardrequire}");

        getButton.interactable = count >= tableData.Rewardrequire;


        if ((count / tableData.Rewardrequire) > (TableManager.Instance.EventMissionDatas[tableData.Id].Dailymaxclear - ServerData.eventMissionTable.CheckMissionRewardCount(tableData.Stringid)))
        {
            getAmountFactor = TableManager.Instance.EventMissionDatas[tableData.Id].Dailymaxclear - ServerData.eventMissionTable.CheckMissionRewardCount(tableData.Stringid);
        }
        else
        {
            getAmountFactor = count / tableData.Rewardrequire;
        }


        rewardNum.SetText($"{Mathf.Max(getAmountFactor,1) * tableData.Rewardvalue  }개");
    }

    private Coroutine SyncRoutine;
    private WaitForSeconds syncWaitTime = new WaitForSeconds(2.0f);

    public void OnClickGetButton()
    {
        
        //로컬 갱신
        //EventMissionManager.UpdateEventMissionClear((EventMissionKey)(tableData.Id), -tableData.Rewardrequire );
        EventMissionManager.UpdateEventMissionReward((EventMissionKey)(tableData.Id), 1);
        if(Utils.IsCostumeItem((Item_Type)tableData.Rewardtype))
        {
            if ((Item_Type)tableData.Rewardtype == Item_Type.costume161)
            {
                List<TransactionValue> transactions = new List<TransactionValue>();

                var costumeServerData = ServerData.costumeServerTable.TableDatas["costume161"];

                costumeServerData.hasCostume.Value = true;

                Param costumeParam = new Param();
                costumeParam.Add("costume161", costumeServerData.ConvertToString());
                transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
                
                Param eventMissionParam = new Param();
                eventMissionParam.Add(tableData.Stringid, ServerData.eventMissionTable.TableDatas[tableData.Stringid].ConvertToString());
                transactions.Add(TransactionValue.SetUpdate(EventMissionTable.tableName, EventMissionTable.Indate, eventMissionParam));

                ServerData.SendTransactionV2(transactions, successCallBack: () =>
                {
                    SoundManager.Instance.PlaySound("Reward");
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.costume161)} 획득!!", null);
                    // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
                });
            }        }
        else
        {
            ServerData.goodsTable.AddLocalData(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Rewardtype), tableData.Rewardvalue);

            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName((Item_Type)tableData.Rewardtype)} {tableData.Rewardvalue}개 획득!!");
            
            SoundManager.Instance.PlaySound("GoldUse");

            if (SyncRoutine != null)
            {
                CoroutineExecuter.Instance.StopCoroutine(SyncRoutine);
            }

            SyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutine());
        }


    }

    private IEnumerator SyncDataRoutine()
    {
        yield return syncWaitTime;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param eventMissionParam = new Param();
        Param goodsParam = new Param();

        //미션 카운트 차감
        eventMissionParam.Add(tableData.Stringid, ServerData.eventMissionTable.TableDatas[tableData.Stringid].ConvertToString());
        transactionList.Add(TransactionValue.SetUpdate(EventMissionTable.tableName, EventMissionTable.Indate, eventMissionParam));

        //재화 추가
        goodsParam.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactionList);

        SyncRoutine = null;
    }
}
