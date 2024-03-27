using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYorinMissionInGameCanvas : MonoBehaviour
{
    private void OnEnable()
    {
        this.gameObject.SetActive(IsAllClear()==false);


    }

    private bool IsAllClear()
    {
        var tableData = TableManager.Instance.YorinMission.dataArray;

        //출석 7일 수령 x 
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yorinAttendRewarded).Value < 7) return false; 
        for (var i = 0; i < tableData.Length; i++)
        {
            //깬것
            if (ServerData.yorinMissionServerTable.TableDatas[tableData[i].Stringid].rewardCount.Value > 0) continue;
            //안깬것
            else
            {
                return false;
            }
        }

        var tableData2 = TableManager.Instance.YorinSpecialMission.dataArray;
        for (var i = 0; i < tableData2.Length; i++)
        {
            //깬것
            if (ServerData.yorinSpecialMissionServerTable.TableDatas[tableData2[i].Stringid].rewardCount.Value > 0) continue;
            //안깬것
            else
            {
                return false;
            }        
        }

        return true;
    }
}
