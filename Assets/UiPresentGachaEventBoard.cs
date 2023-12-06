using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using WebSocketSharp;

public class UiPresentGachaEventBoard : MonoBehaviour
{
    [SerializeField] private UiPresentEventCell prefab;

    [SerializeField] private Transform content;
    
    private List<float> probs = new List<float>();

    
    private void Start()
    {
        MakeCell();
    }

    private void MakeCell()
    {
        var tableData = TableManager.Instance.GachaEvent.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {

            var cell = Instantiate(prefab, content);
            cell.Initialize(tableData[i]);
        }
       
        probs.Clear();
        
        for (int i = 0; i < tableData.Length; i++)
        {
            probs.Add(tableData[i].Gacha);
        }
    }


    public void OnClickGachaButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.GT).Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GT)}이 부족합니다.");
            return;
        }
        
        var rewardedList = ServerData.etcServerTable.GetGachaEventRewardedList();
        
        var tableData = TableManager.Instance.GachaEvent.dataArray;
        
        int randomIdx = -1;

        while (randomIdx<0)
        {
            randomIdx = Utils.GetRandomIdx(probs);
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
        //초기화
        if (ServerData.etcServerTable.GetGachaEventRewardedList().Count >= tableData.Length)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].Value = string.Empty;
        }
        
        List<TransactionValue> transactionList = new List<TransactionValue>();
        
        

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.GT, ServerData.goodsTable.GetTableData(GoodsTable.GT).Value);
        
        var rewardString = ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData[randomIdx].Itemtype);
        goodsParam.Add(rewardString, ServerData.goodsTable.GetTableData(rewardString).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        
        Param etcParam = new Param();

        etcParam.Add(EtcServerTable.gachaEventReward, ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].Value);
        transactionList.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        
        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
           PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(type)} {Utils.ConvertNum(value)}개 획득!");
        });
    }

    public void OnClickInitializeButton()
    {
        if (ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].Value.IsNullOrEmpty())
        {
            PopupManager.Instance.ShowAlarmMessage("뽑기 한번 이상을 진행해야 합니다!");
            return;
        }
        else
        {
            
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,"정말 뽑기판을 초기화하시겠습니까?", () =>
            {
                ServerData.etcServerTable.TableDatas[EtcServerTable.gachaEventReward].Value = string.Empty;
                
                ServerData.etcServerTable.UpdateData(EtcServerTable.gachaEventReward);
                
            },null);
        }
    }
}
