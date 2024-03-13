using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.UI;

public class UiLimitedCostumePackageCell : MonoBehaviour
{
    
    [SerializeField]
    private TextMeshProUGUI itemDetailText;

    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private TextMeshProUGUI buyCountText;
    [SerializeField]
    private Image packageIcon;

    private CompositeDisposable disposable = new CompositeDisposable();

    [SerializeField]
    private Button buyButton;

    private CostumeData costumeData;


    public void Initialize(CostumeData costumeData)
    {
        this.costumeData = costumeData;

        SetTexts();

        Subscribe();

        SetIcon();

    }
    private void SetIcon()
    {
        if (packageIcon != null)
        {
            packageIcon.sprite = CommonUiContainer.Instance.GetCostumeThumbnail(costumeData.Id);
        }
    }

    private void SetTexts()
    {
        if (itemDetailText != null)
            itemDetailText.SetText($"{costumeData.Name} 1개");
    }

    public void OnClickBuyButton()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
            return;
        }
        if (CanBuyProduct() == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Event_Fall_Gold)}이 부족합니다.");

            return;
        }

        if (ServerData.costumeServerTable.TableDatas[costumeData.Stringid].hasCostume.Value == true)
        {
            PopupManager.Instance.ShowAlarmMessage($"보유중입니다.");

            return;
        }

        var log = string.Empty;

        log += $"before = {ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value}/";
        ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value -= costumeData.Price;
        log += $"after = {ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value}/";
        
        ServerData.costumeServerTable.TableDatas[costumeData.Stringid].hasCostume.Value = true;
        log += $"costume = {costumeData.Stringid}";
        
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();

        goodsParam.Add(GoodsTable.Event_Fall_Gold, ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        
        Param costumeParam = new Param();

        costumeParam.Add(costumeData.Stringid, ServerData.costumeServerTable.TableDatas[costumeData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));


        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            LogManager.Instance.SendLogType("황금 곶감","상점",log);
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{costumeData.Name} 획득!!", null);
        });


    }

    private bool CanBuyProduct()
    {
        return costumeData.Price <= ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value;
    }

    private void Subscribe()
    {
        disposable.Clear();            
        
        ServerData.costumeServerTable.TableDatas[costumeData.Stringid].hasCostume.AsObservable().Subscribe(e =>
        {
            buyCountText.SetText(e == true ? "1회만 구매가능\n(1/1)" : "1회만 구매가능\n(0/1)");

            buyButton.interactable = !e;

            priceText.SetText(e == false ? $"{costumeData.Price}" : "보유중");

        }).AddTo(disposable);
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }
}
