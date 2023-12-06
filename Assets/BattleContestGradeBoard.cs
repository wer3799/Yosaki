using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleContestGradeBoard : MonoBehaviour
{
    [SerializeField] private UiBattleContestGradeCell prefab;
    [SerializeField] private Transform transform;


    [SerializeField] private TextMeshProUGUI curGradeText;
    [SerializeField] private TextMeshProUGUI curAbilityText;
   

    private bool initialized = false;
    private void Start()
    {
        MakeCell();

        Subscribe();
        
        initialized = true;
    }
    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.battleContestGradeLevel).AsObservable().Subscribe(e =>
        {
            UpdateUi();
            
        }).AddTo(this);
    }
    private void UpdateUi()
    {
        var grade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.battleContestGradeLevel).Value;
        var tableData = TableManager.Instance.BattleContestGrade.dataArray;
        if (grade < 0)
        {
            curGradeText.SetText("획득 등급 없음");
            curAbilityText.SetText("피해량 0%");
        }
        else
        {
            var currentData = tableData[grade];
        
            curGradeText.SetText($"{Utils.ColorToHexString(CommonUiContainer.Instance.itemGradeColor[currentData.Grade],currentData.Title_Name)}");
            curAbilityText.SetText(
                $"{CommonString.GetStatusName((StatusType)currentData.Abil_Type)} {Utils.ConvertNum(currentData.Abil_Value*100, 2)}%");
        }
    }
    
    private void MakeCell()
    {
        var tableData = TableManager.Instance.BattleContestGrade.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            //tableData[i].
            var cell = Instantiate<UiBattleContestGradeCell>(prefab, transform);
            cell.Initialize(tableData[i]);
        }
    }
    
}
