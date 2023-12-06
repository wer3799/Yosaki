using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class UiGyungRockFloorInfoBoard5 : MonoBehaviour
{

    private enum gyungRockIdx
    {
        GyungRock5,
        GyungRock6,
    }
    [SerializeField] private TextMeshProUGUI abilDescriptionBoard;

    [SerializeField] private gyungRockIdx GyungRockIdx= gyungRockIdx.GyungRock5;
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {

        string description = string.Empty;

        switch(GyungRockIdx)
        {
            case gyungRockIdx.GyungRock5:
                var tableData = TableManager.Instance.gyungRockTowerTable5.dataArray;
                for (int i = 0; i < tableData.Length; i++)
                {
                    description += $"{tableData[i].Id+1}단계 {tableData[i].Fruitname}({tableData[i].Chimname}):{CommonString.GetStatusName((StatusType.SuperCritical13DamPer))} {tableData[i].Rewardvalue*100f}%";

                    if (i != tableData.Length - 1)
                    {
                        description += "\n";
                    }
                }
                break;
            case gyungRockIdx.GyungRock6:
                var tableData2 = TableManager.Instance.gyungRockTowerTable6.dataArray;
                for (int i = 0; i < tableData2.Length; i++)
                {
                    description += $"{tableData2[i].Id+1}단계 {tableData2[i].Fruitname}({tableData2[i].Chimname}):{CommonString.GetStatusName((StatusType.SuperCritical18DamPer))} {tableData2[i].Rewardvalue*100f}%";

                    if (i != tableData2.Length - 1)
                    {
                        description += "\n";
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }



        
        abilDescriptionBoard.SetText(description);
    }
}
