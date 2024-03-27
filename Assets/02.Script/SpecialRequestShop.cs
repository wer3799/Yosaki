using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpecialRequestShop : MonoBehaviour
{
    [SerializeField] private UiGoodsIndicatorV2 prefab;
    [SerializeField] private Transform normalParent;
    [SerializeField] private Transform seasonParent;

    [SerializeField] private UiShopCell shopPrefab;
    [SerializeField] private Transform normalShopParent;
    [SerializeField] private Transform seasonShopParent;


    private void Start()
    {
        MakeGoodsIndicator();

        MakeShopCell();
    }

    private void MakeGoodsIndicator()
    {
        var tableData = TableManager.Instance.SpecialRequestExchangeTable.dataArray;

        var currentSeasonIdx =  (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.currentSeasonIdx).Value;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            //현재 시즌이 끝나는 시즌보다 높으면 넘김.
            if (tableData[i].Endseasonid <= currentSeasonIdx) continue;
            //현재시즌이  시작하는시즌보다 낮으면 넘김
            if (tableData[i].Startseasonid > currentSeasonIdx) continue;
            switch (tableData[i].Taptype)
            {
                case 0 :
                    var cell2 = Instantiate(prefab, seasonParent);
                    cell2.Initialize(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData[i].Itemtype));
                    break;
                case 1 :
                    var cell = Instantiate(prefab, normalParent);
                    cell.Initialize(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData[i].Itemtype));
                    break;
     
            }
        }
    }

    private void MakeShopCell()
    {
        var tableData = TableManager.Instance.SpecialRequestExchangeTable.dataArray;

        var currentSeasonIdx =  (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.currentSeasonIdx).Value;

        var costItem = Item_Type.SRG;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            //현재 시즌이 끝나는 시즌보다 높으면 넘김.
            if (tableData[i].Endseasonid <= currentSeasonIdx) continue;
            //현재시즌이  시작하는시즌보다 낮으면 넘김
            if (tableData[i].Startseasonid > currentSeasonIdx) continue;
            switch (tableData[i].Taptype)
            {
                case 0 :
                    var cell2 = Instantiate(shopPrefab, seasonShopParent);
                    var data2 = new ExchangeData();
                    data2.Initialize(new RewardItem((Item_Type)tableData[i].Itemtype, tableData[i].Itemvalue),
                        new RewardItem(costItem, tableData[i].Price), tableData[i].Exchangemaxcount,
                        tableData[i].Exchangekey);
                    cell2.Initialize(data2);                   
                    break;
                case 1 :
                    var cell = Instantiate(shopPrefab, normalShopParent);
                    var data = new ExchangeData();
                    data.Initialize(new RewardItem((Item_Type)tableData[i].Itemtype, tableData[i].Itemvalue),
                        new RewardItem(costItem, tableData[i].Price), tableData[i].Exchangemaxcount,
                        tableData[i].Exchangekey);
                    cell.Initialize(data);
                    break;
            }
        }
    }
}
