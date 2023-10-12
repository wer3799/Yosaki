                                                         using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiEventMission1Shop : MonoBehaviour
{
    
    [SerializeField]
    private Transform cellParent;
    [SerializeField]
    private UiEventMission1ShopCell shopCell;


    private void Awake()
    {
        Initialize();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission1).Value += 100;
        }

    }
#endif

    private void Initialize()
    {
        var tableData = TableManager.Instance.xMasCollection.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].COMMONTABLEEVENTTYPE != CommonTableEventType.Flower) continue;
            if (tableData[i].Active == false) continue;
            var cell = Instantiate<UiEventMission1ShopCell>(shopCell, cellParent);

            cell.Initialize(i);
        }
    }
}
