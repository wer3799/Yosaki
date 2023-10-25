using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class UiGyungRockFloorInfoBoard5 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilDescriptionBoard;

    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.gyungRockTowerTable5.dataArray;

        string description = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            description += $"{tableData[i].Id+1}단계 {tableData[i].Fruitname}({tableData[i].Chimname}):{CommonString.GetStatusName((StatusType.SuperCritical13DamPer))} {tableData[i].Rewardvalue*100f}%";

            if (i != tableData.Length - 1)
            {
                description += "\n";
            }
        }
        
        abilDescriptionBoard.SetText(description);
    }
}
