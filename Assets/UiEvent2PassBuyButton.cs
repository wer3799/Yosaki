using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UiEvent2PassBuyButton : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI descText;

    private CompositeDisposable disposable = new CompositeDisposable();

    public static readonly string productKey = "fullmoonpass";

    private Button buyButton;

    [SerializeField]
    private TextMeshProUGUI GetEventItemCount;


    [SerializeField]
    private GameObject rootObject;
    void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }

    private void Subscribe()
    {
        buyButton = GetComponent<Button>();

        disposable.Clear();

        ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission2_All).AsObservable().Subscribe(e =>
        {
            GetEventItemCount.SetText($"구매시 즉시 추가획득 : {ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission2_All).Value} 개");
        }).AddTo(disposable);

        ServerData.iapServerTable.TableDatas[productKey].buyCount.AsObservable().Subscribe(e =>
        {
            descText.SetText(e >= 1 ? "구매완료" : $"{CommonString.GetItemName(Item_Type.Event_Mission2)} 패스 구매");
            if (e >= 1)
            {
                GetEventItemCount.SetText(""); 
            }
            rootObject.SetActive(e < 1);
           // this.gameObject.SetActive(e <= 0);
        }).AddTo(disposable);

        IAPManager.Instance.WhenBuyComplete.AsObservable().Subscribe(e =>
        {
            SoundManager.Instance.PlaySound("GoldUse");
            GetPackageItem(e.purchasedProduct.definition.id);
        }).AddTo(disposable);

        IAPManager.Instance.disableBuyButton.AsObservable().Subscribe(e =>
        {
            buyButton.interactable = false;
        }).AddTo(disposable);

        IAPManager.Instance.activeBuyButton.AsObservable().Subscribe(e =>
        {
            buyButton.interactable = true;
        }).AddTo(disposable);
    }

    public void OnClickBuyButton()
    {
        if (ServerData.iapServerTable.TableDatas[productKey].buyCount.Value >= 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 구매 했습니다.");
            return;
        }

#if UNITY_EDITOR|| TEST
        GetPackageItem(productKey);
        return;
#endif

        IAPManager.Instance.BuyProduct(productKey);
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
        else
        {
            // PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{tableData.Title} 구매 성공!", null);
        }

        if (tableData.Productid != productKey) return;

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"구매 성공!\n {CommonString.GetItemName(Item_Type.Event_Mission2)} {ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission2_All).Value}개 획득!", null);

        //소급적용
        ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission2).Value += ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission2_All).Value;

        ServerData.goodsTable.UpData(GoodsTable.Event_Mission2, false);

        ServerData.iapServerTable.TableDatas[tableData.Productid].buyCount.Value++;

        ServerData.iapServerTable.UpData(tableData.Productid);
    }
}
