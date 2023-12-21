using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiAttendEventBoard : MonoBehaviour
{
    [SerializeField] private UiAttendEventCell prefab;
    [SerializeField] private Transform parent;
    [SerializeField] private TextMeshProUGUI attendText;
    [SerializeField] private UiRewardResultView resultView;


    private void Start()
    {
        Initialize();
        
        Subscribe();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.SecondAttend.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var obj = Instantiate(prefab, parent);
            obj.Initialize(tableData[i], resultView);
        }

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventAttendCount).Value < 1)
        {
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventAttendCount).Value = 1;
            ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.eventAttendCount,false);
        }
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventAttendCount).AsObservable().Subscribe(e =>
        {
            attendText.SetText($"출석일 : {e}일");
        }).AddTo(this);
    }
}
