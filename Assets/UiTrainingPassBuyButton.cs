using System.Collections;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UiTrainingPassBuyButton : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI descText;

    private CompositeDisposable disposable = new CompositeDisposable();

    public static readonly string productKey = "trainingpass4";
    public static readonly string costumeKey = "costume250";

    private Button buyButton;

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

        ServerData.iapServerTable.TableDatas[productKey].buyCount.AsObservable().Subscribe(e =>
        {
            descText.SetText(e >= 1 ? "구매완료" : "훈련권 구매");
            this.gameObject.SetActive(e <= 0);
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
        
        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"구매 성공!", null);

        List<TransactionValue> transactions = new List<TransactionValue>();
        Param iapParam = new Param();
        Param costumeParam = new Param();


        ServerData.iapServerTable.TableDatas[tableData.Productid].buyCount.Value++;

        if (productKey == "trainingpass4")
        {
            ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.Value = true;
            costumeParam.Add(costumeKey, ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        iapParam.Add(tableData.Productid, ServerData.iapServerTable.TableDatas[tableData.Productid].ConvertToString());
        transactions.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));

        //ServerData.iapServerTable.UpData(tableData.Productid);
        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            //PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
            //LogManager.Instance.SendLogType("ChildPass", "A", "A");
        });
    }
}
