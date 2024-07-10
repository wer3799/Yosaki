using System;
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiPensionObjectView : MonoBehaviour
{
    public ObscuredString pensionKey;

    public ObscuredInt instantReceiveValue;

    public ObscuredInt dailyReceiveValue;

    public ObscuredInt dayCountMax;

    [SerializeField]
    private UiPensionItemCell cellPrefab;

    private List<UiPensionItemCell> cellContainer = new List<UiPensionItemCell>();

    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private UIShiny uiShiny;

    [SerializeField]
    private TextMeshProUGUI buyButtonDesc;
    [SerializeField]
    private TextMeshProUGUI productDesc;

    [SerializeField]
    private GameObject buyButton;
    [SerializeField]
    private GameObject buyAfter;

    [SerializeField]
    private TextMeshProUGUI instantRewardDesc;

    [SerializeField]
    private TextMeshProUGUI dailyRewardDesc;

    [SerializeField]
    private TextMeshProUGUI attendanceCount;

    [SerializeField] private Image itemImage;
    
    void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Initialize()
    {
        if (itemImage != null)
        {
            itemImage.sprite = CommonUiContainer.Instance.GetItemIcon(SetItemType());
        }

        if (productDesc != null)
        {
            productDesc.SetText($"{CommonString.GetItemName(SetItemType())} 연금");
        }

        for (int i = 0; i < dayCountMax; i++)
        {
            var cell = Instantiate<UiPensionItemCell>(cellPrefab, cellParents);
            cell.gameObject.SetActive(true);
            cell.Initialize(pensionKey, i, dailyReceiveValue, dayCountMax);
            cellContainer.Add(cell);
        }

        instantRewardDesc.SetText(Utils.ConvertBigNum(instantReceiveValue));
        dailyRewardDesc.SetText(Utils.ConvertBigNum(dailyReceiveValue));
    }

    private void Subscribe()
    {
        ServerData.iapServerTable.TableDatas[pensionKey].buyCount.AsObservable().Subscribe(e =>
        {
            uiShiny.enabled = e > 0;

            string price = IAPManager.m_StoreController.products.WithID(pensionKey).metadata.localizedPrice.ToString("N0");

            buyButton.SetActive(e == 0);

            buyAfter.SetActive(e == 1);

            buyButtonDesc.SetText(e > 0 ? "구매함" : $"{price}");

            attendanceCount.gameObject.SetActive(true);
        }).AddTo(this);

        IAPManager.Instance.WhenBuyComplete.AsObservable().Subscribe(e =>
        {
            SoundManager.Instance.PlaySound("Reward");
            GetPackageItem(e.purchasedProduct.definition.id);
        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[pensionKey].AsObservable().Subscribe(e =>
        {
            attendanceCount.gameObject.SetActive(ServerData.iapServerTable.TableDatas[pensionKey].buyCount.Value > 0);
            attendanceCount.SetText($"{e + 1}일차");
        }).AddTo(this);
    }
    private Tuple<Item_Type, string> itemTuple;
    public void OnClickAllReceiveItem()
    {
        var e = cellContainer.GetEnumerator();

        int rewardCount = 0;

        while (e.MoveNext())
        {
            if (e.Current != null)
            {
                var tuple = e.Current.OnClickRewardButton_All();

                if (tuple != null)
                {
                    itemTuple = tuple;
                    rewardCount++;   
                }
            }
        }

        if (rewardCount > 0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(itemTuple.Item1),ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString(itemTuple.Item1)).Value);

            Param pensionParam = new Param();
            pensionParam.Add(itemTuple.Item2, ServerData.pensionServerTable.TableDatas[itemTuple.Item2].Value);
            
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            transactions.Add(TransactionValue.SetUpdate(PensionServerTable.tableName, PensionServerTable.Indate, pensionParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowAlarmMessage("보상 전부 수령 완료!");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("받을 보상이 없습니다!");
        }
    }

    private Item_Type SetItemType()
    {
        Item_Type itemType = Item_Type.Gold;
        
        if (pensionKey == "oakpension")
        {
            itemType = Item_Type.Jade;
        }
        else if (pensionKey == "marblepension")
        {
            itemType = Item_Type.Marble;
        }
        else if (pensionKey == "relicpension")
        {
            itemType = Item_Type.RelicTicket;
        }
        else if (pensionKey == "peachpension")
        {
            itemType = Item_Type.PeachReal;
        }
        else if (pensionKey == "smithpension")
        {
            itemType = Item_Type.SmithFire;
        }   
        else if (pensionKey == "weaponpension")
        {
            itemType = Item_Type.SP;
        }
        else if (pensionKey == "hellpension")
        {
            itemType = Item_Type.Hel;
        } 
        else if (pensionKey == "chunpension")
        {
            itemType = Item_Type.Cw;
        }  
        else if (pensionKey == "dokebipension")
        {
            itemType = Item_Type.DokebiFire;
        }
        else if (pensionKey == "sumipension")
        {
            itemType = Item_Type.SumiFire;
        }  
        else if (pensionKey == "sealWeaponpension")
        {
            itemType = Item_Type.SealWeaponClear;
        }    
        else if (pensionKey == "ringpension")
        {
            itemType = Item_Type.NewGachaEnergy;
        }
        else if (pensionKey == "suhopetfeedclearpension")
        {
            itemType = Item_Type.SuhoPetFeedClear;
        }  
        else if (pensionKey == "foxfirepension")
        {
            itemType = Item_Type.FoxRelicClearTicket;
        }  
        else if (pensionKey == "sealswordpension")
        {
            itemType = Item_Type.SealWeaponClear;
        }
        else if (pensionKey == "dosulpension")
        {
            itemType = Item_Type.DosulClear;
        }
        else if (pensionKey == "guimoonpension")
        {
            itemType = Item_Type.GuimoonRelicClearTicket;
        }
        else if (pensionKey == "meditationpension")
        {
            itemType = Item_Type.MeditationClearTicket;
        }
        else if (pensionKey == "taeguekpension")
        {
            itemType = Item_Type.TaeguekElixir;
        }
        else if (pensionKey == "blacksoulpension")
        {
            itemType = Item_Type.BlackFoxClear;
        }
        else if (pensionKey == "studentspotpension")
        {
            itemType = Item_Type.HYC;
        }
        else if (pensionKey == "transjeweltpension")
        {
            itemType = Item_Type.TJCT;
        }
        else if (pensionKey == "masterpension")
        {
            itemType = Item_Type.ClearTicket;
        }

        return itemType;
    }
    
    public void GetPackageItem(string productId)
    {
        if (productId.Equals("removeadios"))
        {
            productId = "removead";
        }

        if (TableManager.Instance.InAppPurchaseData.TryGetValue(productId, out var tableData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 상품 id {productId}", null);
            return;
        }

        if (tableData.Productid != pensionKey) return;

        ServerData.iapServerTable.TableDatas[tableData.Productid].buyCount.Value++;

        Item_Type itemType = SetItemType();

   

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param iapParam = new Param();
        iapParam.Add(tableData.Productid, ServerData.iapServerTable.TableDatas[tableData.Productid].ConvertToString());

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(itemType, instantReceiveValue));
        transactions.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"구매 성공!\n{CommonString.GetItemName(itemType)} {Utils.ConvertBigNum(instantReceiveValue)}개 획득!", null);
        });
    }

    public void OnClickBuyButton()
    {
        if (ServerData.iapServerTable.TableDatas[pensionKey].buyCount.Value > 0f)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 구입함");
            return;
        }

#if UNITY_EDITOR || TEST
        GetPackageItem(pensionKey);
        return;
#endif

        IAPManager.Instance.BuyProduct(pensionKey);
    }



}
