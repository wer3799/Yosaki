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
        GyungRock7,
        GyungRock8,
        GyungRock9,
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
            case gyungRockIdx.GyungRock7:
                var tableData3 = TableManager.Instance.gyungRockTowerTable7.dataArray;
                for (int i = 0; i < tableData3.Length; i++)
                {
                    description += $"{tableData3[i].Id+1}단계 {tableData3[i].Fruitname}({tableData3[i].Chimname}):{CommonString.GetStatusName((StatusType)(tableData3[i].Rewardtype))} {tableData3[i].Rewardvalue*100f}%";

                    if (i != tableData3.Length - 1)
                    {
                        description += "\n";
                    }
                }
                break;
            case gyungRockIdx.GyungRock8:
                var tableData4 = TableManager.Instance.gyungRockTowerTable8.dataArray;
                for (int i = 0; i < tableData4.Length; i++)
                {
                    description += $"{tableData4[i].Id+1}단계 {tableData4[i].Fruitname}({tableData4[i].Chimname}):{CommonString.GetStatusName((StatusType)(tableData4[i].Rewardtype))} {tableData4[i].Rewardvalue*100f}%";

                    if (i != tableData4.Length - 1)
                    {
                        description += "\n";
                    }
                }
                break;
            case gyungRockIdx.GyungRock9:
                var tableData5 = TableManager.Instance.gyungRockTowerTable9.dataArray;
                for (int i = 0; i < tableData5.Length; i++)
                {
                    description += $"{tableData5[i].Id+1}단계 {tableData5[i].Fruitname}({tableData5[i].Chimname}):{CommonString.GetStatusName((StatusType)(tableData5[i].Rewardtype))} {tableData5[i].Rewardvalue*100f}%";

                    if (i != tableData5.Length - 1)
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
