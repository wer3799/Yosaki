using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiClearTicketCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;


    [SerializeField]
    private TextMeshProUGUI itemAmount;

    [SerializeField]
    private TextMeshProUGUI itemAmount_Costume;

    [SerializeField]
    private TextMeshProUGUI price;


    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private int tableId;

    [SerializeField]
    ContinueOpenButton button;
    [SerializeField]
    ContinueOpenButton goldButton;
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    private ClearTicketData tableData;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        if (IsCostumeItem() == false) return;

        string itemKey = ((Item_Type)tableData.Itemtype).ToString();

        ServerData.costumeServerTable.TableDatas[itemKey].hasCostume.AsObservable().Subscribe(e =>
        {
            if (e == false)
            {
                price.SetText(Utils.ConvertBigNum(tableData.Price));
            }
            else
            {
                price.SetText("보유중!");
                goldButton.gameObject.SetActive(false);
            }

        }).AddTo(this);
    }

    public void Initialize(int _tableId=-1)
    {
        if (_tableId != -1)
        {
            tableId = _tableId;
        }
        if (button != null)
        { 
            button.onEvent.AddListener(OnClickExchangeButton);
        }
        tableData = TableManager.Instance.ClearTicket.dataArray[tableId];

        itemIcon.gameObject.SetActive(IsCostumeItem() == false);
        skeletonGraphic.gameObject.SetActive(IsCostumeItem());

        if (IsCostumeItem() == false)
        {
            price.SetText(Utils.ConvertBigNum(tableData.Price));
        }

        //스파인
        if (IsCostumeItem())
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();
            var idx = ServerData.costumeServerTable.TableDatas[itemKey].idx;
            skeletonGraphic.Clear();
            skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[idx];
            skeletonGraphic.Initialize(true);
            skeletonGraphic.SetMaterialDirty();

            var costumeTable = TableManager.Instance.Costume.dataArray[idx];

            if (itemAmount_Costume != null)
            {
                itemAmount_Costume.SetText($"(능력치 슬롯{costumeTable.Slotnum}개)");
            }
        }

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Itemtype);

        itemAmount.SetText(Utils.ConvertBigNum(tableData.Itemvalue) + "개");

        itemName.SetText(CommonString.GetItemName((Item_Type)tableData.Itemtype));
    }

    public void OnClickExchangeButton()
    {
  
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
            return;
        }

        
        if (IsCostumeItem())
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            if (ServerData.costumeServerTable.TableDatas[itemKey].hasCostume.Value)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
                return;
            }
        }


        int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.ClearTicket).Value;

        if (currentEventItemNum < tableData.Price)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.ClearTicket)}이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("교환 완료");

        //로컬
        ServerData.goodsTable.GetTableData(GoodsTable.ClearTicket).Value -= tableData.Price;
        

        ServerData.AddLocalValue((Item_Type)tableData.Itemtype, tableData.Itemvalue);

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }
    private bool IsCostumeItem()
    {
        return ((Item_Type)tableData.Itemtype).IsCostumeItem();
    }

    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);

    public IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        if (IsCostumeItem())
        {
            Param costumeParam = new Param();

            string costumeKey = ((Item_Type)tableData.Itemtype).ToString();

            costumeParam.Add(costumeKey.ToString(), ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.ClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.ClearTicket).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        else
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            Param goodsParam = new Param();


            goodsParam.Add(GoodsTable.ClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.ClearTicket).Value);

            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype), ServerData.goodsTable.GetTableData((Item_Type)tableData.Itemtype).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }

        Param userInfoParam = new Param();

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));


        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            if (IsCostumeItem())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!!", null);
            }
            else
            {

            }

            //   LogManager.Instance.SendLogType("chuseokExchange", "Costume", ((Item_Type)tableData.Itemtype).ToString());
        });
    }
}
