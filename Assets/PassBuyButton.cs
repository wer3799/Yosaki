using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;

public class PassBuyButton : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI mileageText;
    
    protected InAppPurchaseData tableData = new InAppPurchaseData();
    
    protected string seasonPassKey;
    public static readonly string sasinsuPassKey = "sasinsupass0";
    public static readonly string suhoPassKey = "suhopass";
    public static readonly string foxfirePassKey = "foxfirepass";
    public static readonly string sealswordPassKey = "sealswordpass";
    public static readonly string dosulPassKey = "dosulpass";
    public static readonly string blackfoxPassKey = "blackfoxpass0";
    public static readonly string dosullevelPassKey = "dosullevelpass0";
    public static readonly string bimuPassKey = "bimupass0";
    public static readonly string studentPassKey = "studentpass0";
    public static readonly string studentSpotPassKey = "studentspotpass0";
    public static readonly string haetalPassKey = "haetalpass0";
    public static readonly string yorinsealswordpass0key = "yorinsealswordpass0";
    public static readonly string yorinsealswordpass1key = "yorinsealswordpass1";
    public static readonly string yorinsealswordpass2key = "yorinsealswordpass2";
    public static readonly string yorinsealswordpass3key = "yorinsealswordpass3";
    public static readonly string yorinsealswordpass4key = "yorinsealswordpass4";

    public static readonly string yorinringpass0key = "yorinringpass0";
    public static readonly string yorinringpass1key = "yorinringpass1";
    public static readonly string yorinringpass2key = "yorinringpass2";
    public static readonly string yorinringpass3key = "yorinringpass3";
    public static readonly string yorinringpass4key = "yorinringpass4";

    
    private enum PassKey 
    {
        None=-1,
        Sasinsu,
        Suho,
        FoxFire,
        SealSword,
        Dosul,
        BlackFox,
        DosulLevel,
        Bimu,
        Student,
        StudentSpot,
        Haetal,
        YorinSealSword0,
        YorinSealSword1,
        YorinSealSword2,
        YorinSealSword3,
        YorinSealSword4,
        YorinRing0,
        YorinRing1,
        YorinRing2,
        YorinRing3,
        YorinRing4,
    }

    [SerializeField] private PassKey passKey = PassKey.None;

    protected void Start()
    {
        if (passKey != PassKey.None)
        {
            var key = passKey switch
            {
                PassKey.None => sasinsuPassKey,
                PassKey.Sasinsu => sasinsuPassKey,
                PassKey.Suho => suhoPassKey,
                PassKey.FoxFire => foxfirePassKey,
                PassKey.SealSword => sealswordPassKey,
                PassKey.Dosul => dosulPassKey,
                PassKey.BlackFox => blackfoxPassKey,
                PassKey.DosulLevel => dosullevelPassKey,
                PassKey.Bimu => bimuPassKey,
                PassKey.Student => studentPassKey,
                PassKey.StudentSpot => studentSpotPassKey,
                PassKey.Haetal => haetalPassKey,
                PassKey.YorinSealSword0 => yorinsealswordpass0key,
                PassKey.YorinSealSword1 => yorinsealswordpass1key,
                PassKey.YorinSealSword2 => yorinsealswordpass2key,
                PassKey.YorinSealSword3 => yorinsealswordpass3key,
                PassKey.YorinSealSword4 => yorinsealswordpass4key,
                PassKey.YorinRing0 => yorinringpass0key,
                PassKey.YorinRing1 => yorinringpass1key,
                PassKey.YorinRing2 => yorinringpass2key,
                PassKey.YorinRing3 => yorinringpass3key,
                PassKey.YorinRing4 => yorinringpass4key,
                _ => throw new ArgumentOutOfRangeException()
            };

            SetPassKey(key);
        }
    }

    protected void GetTableData()
    {
        if (TableManager.Instance.InAppPurchaseData.TryGetValue(seasonPassKey, out tableData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 상품 id {seasonPassKey}", null);
        }
    }
    public void SetPassKey(string key)
    {
        seasonPassKey = key;
        GetTableData();
        SetMileageText();
    }
    public string GetPassKey()
    {
        return seasonPassKey;
    }
    protected void SetMileageText()
    {
        var str = "";
        for (int i = 0; i < tableData.Rewardtypes.Length; i++)
        {
            if ((Item_Type)tableData.Rewardtypes[i] == Item_Type.Mileage)
            {
                str += $"패스 구매 시 {CommonString.GetItemName((Item_Type)tableData.Rewardtypes[i])} {tableData.Rewardvalues[i]}개 추가 획득!";
            }
        }
        mileageText.SetText(str);
    }
    protected void GetPackageItem(string productId)
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

        if (tableData.Productid != seasonPassKey) return;

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"구매 성공!", null);

        ServerData.iapServerTable.TableDatas[tableData.Productid].buyCount.Value++;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param iapParam = new Param();
        
        iapParam.Add(tableData.Productid, ServerData.iapServerTable.TableDatas[tableData.Productid].ConvertToString());
        transactionList.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));
        
        Param goodsParam = new Param();
        for (int i = 0; i < tableData.Rewardtypes.Length; i++)
        {
            if (tableData.Rewardvalues[i] < 1) continue;
            
            ServerData.goodsTable.GetTableData((Item_Type)tableData.Rewardtypes[i]).Value += tableData.Rewardvalues[i];
            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Rewardtypes[i]), ServerData.goodsTable.GetTableData((Item_Type)tableData.Rewardtypes[i]).Value);
        }
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            
        });
    }
}
