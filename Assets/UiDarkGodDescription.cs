﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiDarkGodDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI unlockDesc;

    [SerializeField]
    private GameObject equipFrame;


    private int currentIdx;

    private void Start()
    {
        currentIdx = PlayerStats.GetDarkGodGrade();

        Initialize(currentIdx);
    }

    public void Initialize(int idx)
    {
        if (idx == -1) idx = 0;

        var tableData = TableManager.Instance.TestDark.dataArray[idx];

        unlockDesc.SetText($"{Utils.ConvertBigNumForRewardCell(tableData.Score)}");

        equipFrame.gameObject.SetActive(idx == PlayerStats.GetDarkGodGrade());

        gradeText.SetText($"{idx + 1}단계");


        abilDescription.SetText($"{CommonString.GetItemName(Item_Type.DarkTreasure)} 능력치 효과 {Utils.ConvertBigNum(tableData.Abilvalue0 * 100f)}% 증가");
        
    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.TestDark.dataArray.Length - 1);

        Initialize(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.TestDark.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.TestDark.dataArray.Length - 1);

        Initialize(currentIdx);

    }
}
