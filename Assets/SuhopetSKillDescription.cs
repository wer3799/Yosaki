using System;
using System.Collections;
using System.Collections.Generic;
//using System.Management.Instrumentation;
using UnityEngine;

public class SuhopetSKillDescription : MonoBehaviour
{
    [SerializeField]
    private UiSuhoSkillCell suhoSkillCell;

    [SerializeField]
    private Transform cellParent;

    private void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        var tableData = TableManager.Instance.suhoPetTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if(tableData[i].SUHOPETTYPE==SuhoPetType.Special)continue;
            var cell = Instantiate<UiSuhoSkillCell>(suhoSkillCell, cellParent);
            cell.Initialize(tableData[i]);
        }
    }
}
