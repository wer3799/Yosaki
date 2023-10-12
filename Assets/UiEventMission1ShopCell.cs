using System;
using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiEventMission1ShopCell : MonoBehaviour
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
    ContinueOpenButton button;
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;


    private XMasCollectionData tableData;

    private void Start()
    {
        //Initialize();
        //Subscribe();
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission1).Value += 10;
        }
    }
#endif
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
        //   tableData.COMMONTABLEEVENTTYPE == CommonTableEventType.DdukGuk &&
        //   allExchangeLockMask != null)
        // {
        //     ServerData.userInfoTable.DDukGukCollectionComplete.AsObservable().Subscribe(e =>
        //     {
        //
        //         allExchangeLockMask.SetActive(e == false);
        //
        //     }).AddTo(this);
        // }


        if (IsCostumeItem() == false && IsPassWeaponItem() == false) return;

        string itemKey = ((Item_Type)tableData.Itemtype).ToString();

        if (IsCostumeItem())
        {
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
        else if (IsPassWeaponItem())
        {
            ServerData.weaponTable.TableDatas[itemKey].hasItem.AsObservable().Subscribe(e =>
            {
                if (e == 0)
                {
                    price.SetText(Utils.ConvertBigNum(tableData.Price));
                }
                else
                {
                    price.SetText("보유중!");
                }
            }).AddTo(this);
        }
    }

    public void Initialize(int _tableId)
    {
        if (button != null)
        {
            button.onEvent.AddListener(OnClickExchangeButton);
        }

        tableId = _tableId;
        tableData = TableManager.Instance.xMasCollection.dataArray[tableId];

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
        
        Subscribe();
    }

    public void OnClickExchangeButton()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
            return;
        }

        if (tableData.COMMONTABLEEVENTTYPE == CommonTableEventType.DdukGuk &&
         tableData.Lastexchange == true &&
         ServerData.userInfoTable.DDukGukCollectionComplete.Value == false
         )
        {
            PopupManager.Instance.ShowAlarmMessage("다른 재화 상품을 전부 교환해야 합니다.");
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
        if (IsPassWeaponItem())
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            //무기
            if (ServerData.weaponTable.TableDatas[itemKey].hasItem.Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
                return;
            }
        }


        int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission1).Value;

        if (currentEventItemNum < tableData.Price)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Event_Mission1)}이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowAlarmMessage("교환 완료");

        //로컬
        ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission1).Value -= tableData.Price;


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
    private bool IsPassWeaponItem()
    {
        return ((Item_Type)tableData.Itemtype).IsPassWeaponItem();
    }

    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.6f);

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

            goodsParam.Add(GoodsTable.Event_Mission1, ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission1).Value);



            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        else if (IsPassWeaponItem())
        {
            Param weaponParam = new Param();

            string weaponKey = ((Item_Type)tableData.Itemtype).ToString();

            weaponParam.Add(weaponKey.ToString(), ServerData.weaponTable.TableDatas[weaponKey].ConvertToString());

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.Event_Mission1, ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission1).Value);


            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));
        }
        else
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            if ((Item_Type)tableData.Itemtype == Item_Type.NewGachaEnergy)
            {
                itemKey = GoodsTable.NewGachaEnergy;
            }

            Param goodsParam = new Param();


            goodsParam.Add(GoodsTable.Event_Mission1, ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission1).Value);


            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype), ServerData.goodsTable.GetTableData((Item_Type)tableData.Itemtype).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }

        Param userInfoParam = new Param();

        if (string.IsNullOrEmpty(tableData.Exchangekey) == false)
        {
            userInfoParam.Add(tableData.Exchangekey, ServerData.userInfoTable.TableDatas[tableData.Exchangekey].Value);
        }
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));


        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            if (IsCostumeItem())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!!", null);
            }
            else if (IsPassWeaponItem())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "패스 무기 획득!!", null);
            }
            else
            {

            }

            //   LogManager.Instance.SendLogType("chuseokExchange", "Costume", ((Item_Type)tableData.Itemtype).ToString());
        });

        if (tableData.COMMONTABLEEVENTTYPE == CommonTableEventType.DdukGuk)
        {
            ServerData.userInfoTable.UpdateDdukGukCollectionComplete();
        }
    }
}
