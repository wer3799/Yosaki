                                                         using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiEventMission2Shop : MonoBehaviour
{
    //추석
    [SerializeField]
    private Transform cellParent;
    [SerializeField]
    private UiEventMission2ShopCell shopCell;


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
            var cell = Instantiate<UiEventMission2ShopCell>(shopCell, cellParent);

            cell.Initialize(i);
        }
    }
}
