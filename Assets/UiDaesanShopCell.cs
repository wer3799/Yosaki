using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiDaesanShopCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private TextMeshProUGUI buyCountDesc;

    [SerializeField]
    private TextMeshProUGUI itemAmount;


    [SerializeField]
    private TextMeshProUGUI price;


    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private int tableId;

    private DaesanExchangeData tableData;

    [SerializeField] private GameObject allExchangeButton; 
    
    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            ServerData.userInfoTable_2.TableDatas[tableData.Exchangekey].AsObservable().Subscribe(e =>
            {

                buyCountDesc.SetText($"교환 가능 : {e}/{tableData.Exchangemaxcount}");
                
            }).AddTo(this);
        }
        else
        {
            buyCountDesc.SetText("");
        }
    }

    private void Initialize()
    {

        tableData = TableManager.Instance.DaesanExchange.dataArray[tableId];

        price.SetText(Utils.ConvertBigNum(tableData.Price));


        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Itemtype);

        itemAmount.SetText(Utils.ConvertBigNum(tableData.Itemvalue) + "개");

        itemName.SetText(CommonString.GetItemName((Item_Type)tableData.Itemtype));
    }    
    public void Initialize(int tableIdx)
    {
        tableId = tableIdx;
        tableData = TableManager.Instance.DaesanExchange.dataArray[tableId];

        price.SetText(Utils.ConvertBigNum(tableData.Price));

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Itemtype);

        itemAmount.SetText(Utils.ConvertBigNum(tableData.Itemvalue) + "개");

        itemName.SetText(CommonString.GetItemName((Item_Type)tableData.Itemtype));

        allExchangeButton.SetActive(string.IsNullOrEmpty(tableData.Exchangekey) == true);
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
            if (ServerData.userInfoTable_2.TableDatas[tableData.Exchangekey].Value >= tableData.Exchangemaxcount)
            {
                PopupManager.Instance.ShowAlarmMessage("더이상 교환하실 수 없습니다.");
                return;
            }
        }

        int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.DaesanGoods).Value;

        if (currentEventItemNum < tableData.Price)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DaesanGoods)}가 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("교환 완료");

        //로컬
        ServerData.goodsTable.GetTableData(GoodsTable.DaesanGoods).Value -= tableData.Price;


        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            ServerData.userInfoTable_2.TableDatas[tableData.Exchangekey].Value++;
        }

        ServerData.AddLocalValue((Item_Type)tableData.Itemtype, tableData.Itemvalue);

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }
    public void OnClickExchangeAllButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "전부 교환 합니까?", () =>
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
                return;
            }


            if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
            {
                //주간상품은 해당 버튼이 없어야 함.
                return;
            }

            int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.DaesanGoods).Value;

            if (currentEventItemNum < tableData.Price)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DaesanGoods)}가 부족합니다.");
                return;
            }

        
            PopupManager.Instance.ShowAlarmMessage("교환 완료");
        
            var exchangeCount = (int)(currentEventItemNum / tableData.Price);

            //로컬
            ServerData.goodsTable.GetTableData(GoodsTable.DaesanGoods).Value -= tableData.Price * exchangeCount;


            ServerData.AddLocalValue((Item_Type)tableData.Itemtype, tableData.Itemvalue * exchangeCount);

            if (syncRoutine != null)
            {
                CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
            }

            syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
        }, null);
    }
   

    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);

    public IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();


        goodsParam.Add(GoodsTable.DaesanGoods, ServerData.goodsTable.GetTableData(GoodsTable.DaesanGoods).Value);


        goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype), ServerData.goodsTable.GetTableData((Item_Type)tableData.Itemtype).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        

        Param userInfo2Param = new Param();

        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            userInfo2Param.Add(tableData.Exchangekey, ServerData.userInfoTable_2.TableDatas[tableData.Exchangekey].Value);
        }

        if (userInfo2Param.Count > 0)
        {
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));
        }

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            //   LogManager.Instance.SendLogType("chuseokExchange", "Costume", ((Item_Type)tableData.Itemtype).ToString());
        });
    }
}
