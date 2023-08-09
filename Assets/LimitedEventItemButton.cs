using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitedEventItemButton : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private string shopId;
    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.InAppPurchase.dataArray;
        bool isEvent = false;
        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Productid.Equals(shopId))
            {
                isEvent = IsSellPeriod(tableData[i]);
                break;
            }
        }

        // if (isEvent)
        // {
        //     if (_image != null)
        //     {
        //         _image.color = Color.red;
        //     }
        // }
    }
    private bool IsSellPeriod(InAppPurchaseData inAppPurchaseData)
    {
        var splitData = inAppPurchaseData.Sellperiod.Split('-');

        DateTime sellPeriod =
            new DateTime(int.Parse(splitData[0]), int.Parse(splitData[1]), int.Parse(splitData[2]));
        sellPeriod = sellPeriod.AddDays(1);//5월5일을 넣으면 5월6일00시에끝나야함.
        var result = DateTime.Compare(ServerData.userInfoTable.currentServerTime, sellPeriod);

        
        switch (result)
        {
            //아직 안지남
            case -1 :
            case 0:
                return true;
            //지남
            case 1:
                return false;
            default:
                return false;
        }
    }
}
