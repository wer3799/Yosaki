                                                         using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiSAEventShop : MonoBehaviour
{
    
    [SerializeField]
    private Transform cellParent;
    [SerializeField]
    private UiSAShopCell shopCell;


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.xMasCollection.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].COMMONTABLEEVENTTYPE != CommonTableEventType.SecondAnniversary) continue;
            if (tableData[i].Active == false) continue;
            var cell = Instantiate<UiSAShopCell>(shopCell, cellParent);

            cell.Initialize(i);
        }
    }
}
