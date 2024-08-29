using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiGuildRewardCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;

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

    private GuildRewardTableData tableData;

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
            }

        }).AddTo(this);
    }

    private void Initialize()
    {
        tableData = TableManager.Instance.GuildRewardTable.dataArray[tableId];

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
        if (IsCostumeItem())
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            if (ServerData.costumeServerTable.TableDatas[itemKey].hasCostume.Value)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
                return;
            }
        }

        int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value;

        if (currentEventItemNum < tableData.Price)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GuildReward)}이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("교환 완료");

        //로컬
        ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value -= tableData.Price;

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

            goodsParam.Add(GoodsTable.GuildReward, ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        else
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.GuildReward, ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value);

            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype), ServerData.goodsTable.GetTableData((Item_Type)tableData.Itemtype).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
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

            //    LogManager.Instance.SendLogType("guildExchange", "Costume", ((Item_Type)tableData.Itemtype).ToString());
        });
    }

    public void OnClickExchangeButton_All()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "전부 교환 합니까?", () =>
        {
            if (IsCostumeItem())
            {
                string itemKey = ((Item_Type)tableData.Itemtype).ToString();

                if (ServerData.costumeServerTable.TableDatas[itemKey].hasCostume.Value)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
                    return;
                }
            }

            int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value;

            if (currentEventItemNum < tableData.Price)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GuildReward)}이 부족합니다.");
                return;
            }

            PopupManager.Instance.ShowAlarmMessage("교환 완료");

            float exchangeNum = (int)(ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value / tableData.Price);

            //로컬
            ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value -= tableData.Price * exchangeNum;

            ServerData.AddLocalValue((Item_Type)tableData.Itemtype, tableData.Itemvalue * exchangeNum);

            if (syncRoutine != null)
            {
                CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
            }

            syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

        }, () => { });


    }
}
