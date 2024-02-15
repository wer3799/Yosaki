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
        Student
    }

    [SerializeField] private PassKey passKey = PassKey.None;

    protected void Start()
    {
        if (passKey != PassKey.None)
        {
            var key = string.Empty; 
            switch (passKey)
            {
                case PassKey.None:
                    key = sasinsuPassKey;
                    break;
                case PassKey.Sasinsu:
                    key = sasinsuPassKey;
                    break;
                case PassKey.Suho:
                    key = suhoPassKey;
                    break;
                case PassKey.FoxFire:
                    key = foxfirePassKey;
                    break;
                case PassKey.SealSword:
                    key = sealswordPassKey;
                    break;
                case PassKey.Dosul:
                    key = dosulPassKey;
                    break;
                case PassKey.BlackFox:
                    key = blackfoxPassKey;
                    break;
                case PassKey.DosulLevel:
                    key = dosullevelPassKey;
                    break;
                case PassKey.Bimu:
                    key = bimuPassKey;
                    break;
                case PassKey.Student:
                    key = studentPassKey;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

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
