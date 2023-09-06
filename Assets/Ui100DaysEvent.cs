using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui100DaysEvent : MonoBehaviour
{
    [SerializeField]
    private List<UiAttendanceCell_100Days> attendanceCellList;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        
    }


    private void Initialize()
    {
        this.gameObject.SetActive(ServerData.attendanceServerTable.Attendance100AllReceived() == false);
        
        var tableDatas = TableManager.Instance.AttendanceReward_100.dataArray;

        int currentAttendance = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_100Day).Value;

        for (int i = 0; i < attendanceCellList.Count; i++)
        {
            attendanceCellList[i].Initialize(tableDatas[i]);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_100Day).Value++;
        }
    }
#endif
}
