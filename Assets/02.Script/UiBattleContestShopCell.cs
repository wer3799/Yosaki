using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiBattleContestShopCell : MonoBehaviour
{
    [SerializeField] private Image itemIcon;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI requireText;
    [SerializeField] private TextMeshProUGUI limitText;

    private XMasCollectionData tableData;

    
    private void Subscribe()
    {
        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            ServerData.userInfoTable.TableDatas[tableData.Exchangekey].AsObservable().Subscribe(e =>
            {
                limitText.SetText($"교환 가능 : {e}/{tableData.Exchangemaxcount}");
            }).AddTo(this);
        }
    }
    
    public void Initialize(XMasCollectionData data)
    {
        tableData = data;

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Itemtype);
        
        titleText.SetText($"{CommonString.GetItemName((Item_Type)tableData.Itemtype)}");
        
        requireText.SetText($"{CommonString.GetItemName(Item_Type.BattleGoods)} {tableData.Price}개");

        Subscribe();
    }
    
    public void OnClickExchangeButton()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
            return;
        }

        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            if (ServerData.userInfoTable.TableDatas[tableData.Exchangekey].Value >= tableData.Exchangemaxcount)
            {
                PopupManager.Instance.ShowAlarmMessage("더이상 교환하실 수 없습니다.");
                return;
            }
        }

        int itemNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.BattleGoods).Value;

        if (itemNum < tableData.Price)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.BattleGoods)}이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("교환 완료");

        //로컬
        ServerData.goodsTable.GetTableData(GoodsTable.BattleGoods).Value -= tableData.Price;


        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            ServerData.userInfoTable.TableDatas[tableData.Exchangekey].Value++;
        }

        ServerData.AddLocalValue((Item_Type)tableData.Itemtype, tableData.Itemvalue);

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }
 
   private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.6f);

    public IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();
        

        Param goodsParam = new Param();

        goodsParam.Add(GoodsTable.BattleGoods, ServerData.goodsTable.GetTableData(GoodsTable.BattleGoods).Value);

        goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype), ServerData.goodsTable.GetTableData((Item_Type)tableData.Itemtype).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        

        Param userInfoParam = new Param();

        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            userInfoParam.Add(tableData.Exchangekey, ServerData.userInfoTable.TableDatas[tableData.Exchangekey].Value);
        }
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));


        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            //   LogManager.Instance.SendLogType("chuseokExchange", "Costume", ((Item_Type)tableData.Itemtype).ToString());
        });
    }
}
