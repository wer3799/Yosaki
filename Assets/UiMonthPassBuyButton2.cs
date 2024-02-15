using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UiMonthPassBuyButton2 : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI descText;

    private CompositeDisposable disposable = new CompositeDisposable();

    public static readonly string monthPassKey = "monthpass30";

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

        ServerData.iapServerTable.TableDatas[monthPassKey].buyCount.AsObservable().Subscribe(e =>
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
        if (ServerData.iapServerTable.TableDatas[monthPassKey].buyCount.Value >= 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 구매 했습니다.");
            return;
        }

#if UNITY_EDITOR|| TEST
        PopupManager.Instance.ShowYesNoPopup($"{CommonString.Notice}",$"구매시 {ServerData.userInfoTable.currentServerTime.Day}일분의 패스 소탕권을 획득 합니다.",()=>
        {
            GetPackageItem(monthPassKey);    
        },null);
        return;
#endif
        PopupManager.Instance.ShowYesNoPopup($"{CommonString.Notice}",$"구매시 {ServerData.userInfoTable.currentServerTime.Day}일분의 패스 소탕권을 획득 합니다.",()=>
        {
            IAPManager.Instance.BuyProduct(monthPassKey);
        },null);
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

        if (tableData.Productid != monthPassKey) return;
        

        
        
        //홀수월+패스구매시
        var rewardData = TableManager.Instance.MonthReward.dataArray;
        List<TransactionValue> transactionList = new List<TransactionValue>();

        string str = "";
        int ticketCount = 0;
        var dayRefund = ServerData.userInfoTable.currentServerTime.Day;
        Param goodsParam = new Param();
        for (int i = 0; i < rewardData.Length; i++)
        {
            if(rewardData[i].Monthsort!=true) continue;
            var itemValue = rewardData[i].Itemvalue * dayRefund;
            ServerData.goodsTable.GetTableData((Item_Type)rewardData[i].Itemtype).Value += itemValue;
            str += rewardData[i].Description+",";
            if (ticketCount <1)
            {
                ticketCount = (int)itemValue;
            }
            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)rewardData[i].Itemtype), ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString((Item_Type)rewardData[i].Itemtype)).Value);
        }

        str += $"# 소탕권\n{ticketCount}일분 획득!";
        str = str.Replace(",#", "");
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));


        Param iapParam = new Param();
        
        ServerData.iapServerTable.TableDatas[tableData.Productid].buyCount.Value++;
        
        iapParam.Add(tableData.Productid, ServerData.iapServerTable.TableDatas[tableData.Productid].ConvertToString());
        transactionList.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, str, null);
        });
    }
}
