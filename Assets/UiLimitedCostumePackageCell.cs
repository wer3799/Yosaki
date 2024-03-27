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
    private PetTableData petTableData;

    private REWARDTYPE rewardType = REWARDTYPE.Costume;
    enum REWARDTYPE
    {
        Costume,
        Pet,
    }

    public void Initialize(CostumeData costumeData)
    {
        rewardType = REWARDTYPE.Costume;
        
        this.costumeData = costumeData;

        SetTexts();

        Subscribe();

        SetIcon();

    }
    public void Initialize(PetTableData petTableData)
    {
        rewardType = REWARDTYPE.Pet;
        
        this.petTableData = petTableData;

        SetTexts();

        Subscribe();

        SetIcon();

    }
    private void SetIcon()
    {
        if (packageIcon != null)
        {
            switch (rewardType)
            {
                case REWARDTYPE.Costume:
                    packageIcon.sprite = CommonUiContainer.Instance.GetCostumeThumbnail(costumeData.Id);
                    break;
                case REWARDTYPE.Pet:
                    packageIcon.sprite = CommonUiContainer.Instance.GetItemIcon(ServerData.goodsTable.ServerStringToItemType(petTableData.Stringid));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void SetTexts()
    {
        if (itemDetailText != null)
        {
            switch (rewardType)
            {
                case REWARDTYPE.Costume:
                    itemDetailText.SetText($"{costumeData.Name} 1개");
                    break;
                case REWARDTYPE.Pet:
                    itemDetailText.SetText($"{petTableData.Name} 1개");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
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

        switch (rewardType)
        {
            case REWARDTYPE.Costume:
                if (ServerData.costumeServerTable.TableDatas[costumeData.Stringid].hasCostume.Value == true)
                {
                    PopupManager.Instance.ShowAlarmMessage($"보유중입니다.");

                    return;
                }
                break;
            case REWARDTYPE.Pet:
                if (ServerData.petTable.TableDatas[petTableData.Stringid].hasItem.Value > 0)
                {
                    PopupManager.Instance.ShowAlarmMessage($"보유중입니다.");

                    return;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }        


        var log = string.Empty;
        
        List<TransactionValue> transactions = new List<TransactionValue>();
        
        Param goodsParam = new Param();
        Param costumeParam = new Param();
        Param petParam = new Param();

        switch (rewardType)
        {
            case REWARDTYPE.Costume:
                log += $"before = {ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value}/";
                ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value -= costumeData.Price;
                log += $"after = {ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value}/";
        
                ServerData.costumeServerTable.TableDatas[costumeData.Stringid].hasCostume.Value = true;
                log += $"costume = {costumeData.Stringid}";
                costumeParam.Add(costumeData.Stringid, ServerData.costumeServerTable.TableDatas[costumeData.Stringid].ConvertToString());
                transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
                break;
            case REWARDTYPE.Pet:
                log += $"before = {ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value}/";
                ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value -= petTableData.Shopprice;
                log += $"after = {ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value}/";
        
                ServerData.petTable.TableDatas[petTableData.Stringid].hasItem.Value = 1;
                log += $"pet = {petTableData.Stringid}";
                petParam.Add(petTableData.Stringid, ServerData.petTable.TableDatas[petTableData.Stringid].ConvertToString());
                transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        goodsParam.Add(GoodsTable.Event_Fall_Gold, ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        


        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            LogManager.Instance.SendLogType("황금 곶감","상점",log);
            switch (rewardType)
            {
                case REWARDTYPE.Costume:
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{costumeData.Name} 획득!!", null);
                    break;
                case REWARDTYPE.Pet:
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{petTableData.Name} 획득!!", null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });


    }

    private bool CanBuyProduct()
    {
        switch (rewardType)
        {
            case REWARDTYPE.Costume:
                return costumeData.Price <= ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value;
            case REWARDTYPE.Pet:
                return petTableData.Shopprice <= ServerData.goodsTable.GetTableData(GoodsTable.Event_Fall_Gold).Value;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Subscribe()
    {
        disposable.Clear();
        switch (rewardType)
        {
            case REWARDTYPE.Costume:
                ServerData.costumeServerTable.TableDatas[costumeData.Stringid].hasCostume.AsObservable().Subscribe(e =>
                {
                    buyCountText.SetText(e == true ? "1회만 구매가능\n(1/1)" : "1회만 구매가능\n(0/1)");

                    buyButton.interactable = !e;

                    priceText.SetText(e == false ? $"{costumeData.Price}" : "보유중");

                }).AddTo(disposable);
                break;
            case REWARDTYPE.Pet:
                ServerData.petTable.TableDatas[petTableData.Stringid].hasItem.AsObservable().Subscribe(e =>
                {
                    buyCountText.SetText(e >0 ? "1회만 구매가능\n(1/1)" : "1회만 구매가능\n(0/1)");

                    buyButton.interactable = e < 1;

                    priceText.SetText(e <1 ? $"{petTableData.Shopprice}" : "보유중");

                }).AddTo(disposable);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }
}
