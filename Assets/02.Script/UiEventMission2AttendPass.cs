﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using UnityEngine.Serialization;

public class UiEventMission2AttendPass : MonoBehaviour
{
    [SerializeField] private UiEventMission2AttendCell cell;

    [SerializeField] private Transform cellParentTransform;

    [SerializeField] private TextMeshProUGUI day;
    
    [SerializeField] private UiRewardResultView _uiRewardResultView;

    
    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventAttendCount).AsObservable().Subscribe(e =>
        {
            day.SetText($"출석일 : {(int)e}일");
        });
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.SecondAttend.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var prefab = Instantiate<UiEventMission2AttendCell>(cell, cellParentTransform);
            prefab.Initialize(tableData[i],_uiRewardResultView);
        }
    }
}