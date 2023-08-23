                                                         using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiEvent2Shop : MonoBehaviour
{
    
    [SerializeField]
    private Transform cellParent;
    [SerializeField]
    private UiEventMission3ShopCell shopCell;


    private void Awake()
    {
        Initialize();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission2).Value += 100;
        }

    }
#endif

    private void Initialize()
    {
        var tableData = TableManager.Instance.xMasCollection.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].COMMONTABLEEVENTTYPE != CommonTableEventType.FullMoon) continue;
            if (tableData[i].Active == false) continue;
            var cell = Instantiate<UiEventMission3ShopCell>(shopCell, cellParent);

            cell.Initialize(i);
        }
    }
}
