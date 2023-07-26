using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;

public class UiSecondAnniversaryMissionCell : MonoBehaviour
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


    private int getAmountFactor;
    public void Initialize(EventMissionData tableData)
    {
        if (tableData.Enable == false)
        {
            this.gameObject.SetActive(false);
            return;
        }

        this.tableData = tableData;
        exchangeNum.SetText($"매일 교환 : {ServerData.eventMissionTable.TableDatas[tableData.Stringid].rewardCount}/{TableManager.Instance.EventMission.dataArray[tableData.Id].Dailymaxclear}");

        title.SetText(tableData.Title);

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.eventMissionTable.TableDatas[tableData.Stringid].clearCount.AsObservable().Subscribe(WhenMissionCountChanged).AddTo(this);
        ServerData.eventMissionTable.TableDatas[tableData.Stringid].rewardCount.AsObservable().Subscribe(e=>
        {
            exchangeNum.SetText($"매일 교환 : {ServerData.eventMissionTable.TableDatas[tableData.Stringid].rewardCount}/{TableManager.Instance.EventMission.dataArray[tableData.Id].Dailymaxclear}");
             
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

        rewardNum.SetText($"{Mathf.Max(getAmountFactor,1) * tableData.Rewardvalue   }개");
        //if (getButton.interactable)
        //{
        //    if (!lockMask.activeSelf)
        //    {
        //        this.transform.SetAsFirstSibling();
        //    }
        //}
    }

    private Coroutine SyncRoutine;
    private WaitForSeconds syncWaitTime = new WaitForSeconds(2.0f);

    public void OnClickGetButton()
    {
        

        int amountFactor = getAmountFactor;
        int rewardGemNum = tableData.Rewardvalue * amountFactor;

        //로컬 갱신
        EventMissionManager.UpdateEventMissionClear((EventMissionKey)(tableData.Id), -tableData.Rewardrequire * amountFactor);
        EventMissionManager.UpdateEventMissionReward((EventMissionKey)(tableData.Id), amountFactor);
        
        ServerData.goodsTable.AddLocalData(GoodsTable.Event_SA, rewardGemNum);

        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Event_SA)} {rewardGemNum}개 획득!!");
        SoundManager.Instance.PlaySound("GoldUse");

        if (SyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutine);
        }

        SyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutine());
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
        goodsParam.Add(GoodsTable.Event_SA, ServerData.goodsTable.GetTableData(GoodsTable.Event_SA).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactionList);

        SyncRoutine = null;
    }
}
