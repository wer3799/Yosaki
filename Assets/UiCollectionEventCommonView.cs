using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiCollectionEventCommonView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private TextMeshProUGUI buyCountDesc;

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
    private string goodsName;

    [SerializeField]
    private Item_Type goodsType;

    [SerializeField]
    private GameObject goldButton;

    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private GameObject allExchangeLockMask;

    private CommonCollectionEventData tableData;


    private void Start()
    {
        //Initialize();
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

        // if (tableData.Lastexchange &&
        //    tableData.COMMONTABLEEVENTTYPE == CommonTableEventType.SnowMan &&
        //    allExchangeLockMask != null)
        // {
        //     ServerData.userInfoTable.SnowCollectionComplete.AsObservable().Subscribe(e =>
        //     {
        //
        //         allExchangeLockMask.SetActive(e == false);
        //
        //     }).AddTo(this);
        // }

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
                if (goldButton != null)
                {
                    goldButton.SetActive(false);
                }
            }

        }).AddTo(this);


    }

    private void Initialize()
    {
        tableData = TableManager.Instance.commoncollectionEvent.dataArray[tableId];

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
    public void Initialize(int index)
    {
        tableData = TableManager.Instance.commoncollectionEvent.dataArray[index];

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

    public void OnClickExchangeGoldButton()
    {
        if (goldButton.gameObject.activeSelf == false)
            return;

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

        if (IsCostumeItem())
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            if (ServerData.costumeServerTable.TableDatas[itemKey].hasCostume.Value)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
                return;
            }
        }


        int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value;

        if (currentEventItemNum < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Event_Fall_Gold)}이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("교환 완료");
        var log = string.Empty;

        log += $"before = {ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value}/";
        //로컬
        ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value -= 1;
        log += $"after = {ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value}/";

        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            ServerData.userInfoTable.TableDatas[tableData.Exchangekey].Value++;
        }

        ServerData.AddLocalValue((Item_Type)tableData.Itemtype, tableData.Itemvalue);

         List<TransactionValue> transactions = new List<TransactionValue>();

        if (IsCostumeItem())
        {
            Param costumeParam = new Param();

            string costumeKey = ((Item_Type)tableData.Itemtype).ToString();

            costumeParam.Add(costumeKey.ToString(), ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());
            log += $"costume = {costumeKey}";

            Param goodsParam = new Param();

            goodsParam.Add(goodsName, ServerData.goodsTable.GetTableData(goodsName).Value);

            goodsParam.Add(GoodsTable.Event_Fall_Gold, ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        else
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            if ((Item_Type)tableData.Itemtype == Item_Type.NewGachaEnergy)
            {
                itemKey = GoodsTable.NewGachaEnergy;
            }


            Param goodsParam = new Param();

            goodsParam.Add(goodsName, ServerData.goodsTable.GetTableData(goodsName).Value);

            goodsParam.Add(GoodsTable.Event_Fall_Gold, ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value);

            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype), ServerData.goodsTable.GetTableData((Item_Type)tableData.Itemtype).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }



        Param userInfoParam = new Param();

        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            userInfoParam.Add(tableData.Exchangekey, ServerData.userInfoTable.TableDatas[tableData.Exchangekey].Value);

        }

        userInfoParam.Add(UserInfoTable.usedSnowManCollectionCount, ServerData.userInfoTable.TableDatas[UserInfoTable.usedSnowManCollectionCount].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            if (IsCostumeItem())
            {
                LogManager.Instance.SendLogType("황금 곶감","이벤트",log);

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!!", null);
            }
            else
            {

            }

            //   LogManager.Instance.SendLogType("chuseokExchange", "Costume", ((Item_Type)tableData.Itemtype).ToString());
        });

        if (tableData.COMMONTABLEEVENTTYPE == CommonTableEventType.SnowMan)
        {
            ServerData.userInfoTable.UpdateSnowCollectionComplete();
        }

    }
    public void OnClickExchangeButton()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
            return;
        }

        // if (tableData.COMMONTABLEEVENTTYPE == CommonTableEventType.SnowMan &&
        //     tableData.Lastexchange == true &&
        //     ServerData.userInfoTable.SnowCollectionComplete.Value == false
        //     )
        // {
        //     PopupManager.Instance.ShowAlarmMessage("다른 재화 상품을 전부 교환해야 합니다.");
        //     return;
        // }


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


        int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(goodsName).Value;

        if (currentEventItemNum < tableData.Price)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(goodsType)}이(가) 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("교환 완료");

        ServerData.userInfoTable.GetTableData(UserInfoTable.usedSnowManCollectionCount).Value += tableData.Price;

        //로컬
        ServerData.goodsTable.GetTableData(goodsName).Value -= tableData.Price;

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

            goodsParam.Add(goodsName, ServerData.goodsTable.GetTableData(goodsName).Value);

            goodsParam.Add(GoodsTable.Event_Fall_Gold, ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        else
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            if ((Item_Type)tableData.Itemtype == Item_Type.NewGachaEnergy)
            {
                itemKey = GoodsTable.NewGachaEnergy;
            }


            Param goodsParam = new Param();

            goodsParam.Add(goodsName, ServerData.goodsTable.GetTableData(goodsName).Value);

            goodsParam.Add(GoodsTable.Event_Fall_Gold, ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value);

            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype), ServerData.goodsTable.GetTableData((Item_Type)tableData.Itemtype).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }



        Param userInfoParam = new Param();

        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            userInfoParam.Add(tableData.Exchangekey, ServerData.userInfoTable.TableDatas[tableData.Exchangekey].Value);

        }

        userInfoParam.Add(UserInfoTable.usedSnowManCollectionCount, ServerData.userInfoTable.TableDatas[UserInfoTable.usedSnowManCollectionCount].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

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

        if (tableData.COMMONTABLEEVENTTYPE == CommonTableEventType.SnowMan)
        {
            ServerData.userInfoTable.UpdateSnowCollectionComplete();
        }

    }
}
