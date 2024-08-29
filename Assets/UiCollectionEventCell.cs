using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiCollectionEventCell : MonoBehaviour
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

    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    private ChuseokEventData tableData;

    [SerializeField]
    private bool isSnowEvent = true;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            ServerData.userInfoTable.TableDatas[tableData.Exchangekey].AsObservable().Subscribe(e =>
            {

                buyCountDesc.SetText($"교환 가능 : {e}/{tableData.Exchangemaxcount}");

            }).AddTo(this);
        }

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
            }

        }).AddTo(this);
    }

    private void Initialize()
    {
        tableData = TableManager.Instance.ChuseokEventTable.dataArray[tableId];

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
            skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(idx);
            skeletonGraphic.Initialize(true);
            skeletonGraphic.SetMaterialDirty();
        }

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Itemtype);

        itemAmount.SetText(Utils.ConvertBigNum(tableData.Itemvalue) + "개");

        itemName.SetText(CommonString.GetItemName((Item_Type)tableData.Itemtype));
    }

    public void OnClickExchangeButton()
    {

        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            if (ServerData.userInfoTable.TableDatas[tableData.Exchangekey].Value >= tableData.Exchangemaxcount)
            {
                PopupManager.Instance.ShowAlarmMessage("더이상 교환하실 수 없습니다.");
                return;
            }
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

        if (isSnowEvent)
        {
            int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value;

            if (currentEventItemNum < tableData.Price)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Event_Item_0)}가 부족합니다.");
                return;
            }

            PopupManager.Instance.ShowAlarmMessage("교환 완료");

            //로컬
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value -= tableData.Price;

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
        else
        {
            int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value;

            if (currentEventItemNum < tableData.Price)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Event_Item_1)}가 부족합니다.");
                return;
            }

            PopupManager.Instance.ShowAlarmMessage("교환 완료");

            //로컬
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value -= tableData.Price;

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

            if (isSnowEvent)
            {
                goodsParam.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);
            }
            else
            {
                goodsParam.Add(GoodsTable.Event_Item_1, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value);
            }

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        else
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            Param goodsParam = new Param();



            if (isSnowEvent)
            {
                goodsParam.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);
            }
            else
            {
                goodsParam.Add(GoodsTable.Event_Item_1, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value);
            }

            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype), ServerData.goodsTable.GetTableData((Item_Type)tableData.Itemtype).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }

        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            Param userInfoParam = new Param();
            userInfoParam.Add(tableData.Exchangekey, ServerData.userInfoTable.TableDatas[tableData.Exchangekey].Value);
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        }

        ServerData.SendTransaction(transactions, successCallBack: () =>
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
