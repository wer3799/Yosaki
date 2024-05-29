using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class UiBingoEventPopup : MonoBehaviour
{
    [SerializeField] private UiBingoEventRewardCell prefab;

    [SerializeField] private UiBingoEventRewardCell finishCell;

    [SerializeField] private Transform normalParent;
    [SerializeField] private Transform horizontalParent;
    [SerializeField] private Transform verticalParent;
    
    // Start is called before the first frame update
    void Start()
    {
        MakeCell();   
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GT).Value += 10;
        }    
    }
#endif
    private void MakeCell()
    {
        var tableData = TableManager.Instance.BingoEvent.dataArray;


        var horiziontalBingoIdx = 0;
        var verticalBingoIdx = 0;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            switch (tableData[i].BINGOEVENTREWARDTYPE)
            {
                case BingoEventRewardType.Normal:
                    var cell0 = Instantiate(prefab, normalParent);
                    cell0.Initialize(tableData[i]);
                    break;
                case BingoEventRewardType.HorizontalBingo:
                    var cell1 = Instantiate(prefab, horizontalParent);
                    cell1.Initialize(tableData[i],horiziontalBingoIdx);
                    horiziontalBingoIdx++;
                    break;
                case BingoEventRewardType.DiagonalBingo:
                    var cell2 = Instantiate(prefab, verticalParent);
                    cell2.Initialize(tableData[i]);
                    break;
                case BingoEventRewardType.VerticalBingo:
                    var cell3 = Instantiate(prefab, verticalParent);
                    cell3.Initialize(tableData[i],verticalBingoIdx);
                    verticalBingoIdx++;
                    break;
                case BingoEventRewardType.Finish:
                    finishCell.Initialize(tableData[i]);
                    break;
            }
        }
    }

    public void OnClickButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.GT).Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName(Item_Type.GT),JongsungType.Type_IGA)} 부족합니다.");
            return;
        }
        
        var rewardedList = ServerData.etcServerTable.GetGachaEventRewardedList();
        
        var tableData = TableManager.Instance.GachaEvent.dataArray;
        
        int randomIdx = -1;

        int normalReward = 25;
        
        if (ServerData.etcServerTable.GetGachaEventRewardedList().Count >= normalReward)
        {
            PopupManager.Instance.ShowAlarmMessage($"보상을 모두 수령해 주세요.");
            return;        
        }
        
        while (randomIdx<0)
        {
            randomIdx = UnityEngine.Random.Range(0, normalReward);
            //받앗던경우 리롤
            if (rewardedList.Contains(randomIdx))
            {
                randomIdx = -1;
            }   
        }

        ServerData.goodsTable.GetTableData(GoodsTable.GT).Value--;
        
        var type = (Item_Type)tableData[randomIdx].Itemtype;
        var value = tableData[randomIdx].Itemvalue;
        ServerData.AddLocalValue(type, value);
        
        ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].Value += $"{BossServerTable.rewardSplit}{randomIdx}";

        
        if (SyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutine);
        }
        else
        {
            UiRewardResultPopUp.Instance.Clear();
        }

        UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)tableData[randomIdx].Itemtype, value);
        
        SyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutine());
        
        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(type)} {Utils.ConvertNum(value)}개 획득!");

    }
    private Coroutine SyncRoutine;
    private WaitForSeconds syncWaitTime = new WaitForSeconds(0.8f);
    private IEnumerator SyncDataRoutine()
    {
        yield return syncWaitTime;
        

        List<TransactionValue> transactionList = new List<TransactionValue>();
        
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.GT, ServerData.goodsTable.GetTableData(GoodsTable.GT).Value);


        using var e = UiRewardResultPopUp.Instance.RewardList.GetEnumerator();
        while (e.MoveNext())
        {
            var rewardString = ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType);
            goodsParam.Add(rewardString, ServerData.goodsTable.GetTableData(rewardString).Value);
        }

        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        
        Param etcParam = new Param();

        etcParam.Add(EtcServerTable.gachaEventReward, ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].Value);
        transactionList.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        
        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            UiRewardResultPopUp.Instance.Clear();
        });

        SyncRoutine = null;
    }
}
