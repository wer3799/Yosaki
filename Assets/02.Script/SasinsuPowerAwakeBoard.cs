using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UnityEngine;

public class SasinsuPowerAwakeBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentGradeText;
    [SerializeField] private TextMeshProUGUI totalGradeText;

    [SerializeField] private List<TextMeshProUGUI> nameTexts = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> levelTexts = new List<TextMeshProUGUI>();


    private int maxLv = 0;
    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.PetAwakeTable.dataArray;
        for (int i = 0; i < nameTexts.Count; i++)
        {
            nameTexts[i].SetText(tableData[i].Description);
        }
        
        SetUi();
    }

    private void SetUi()
    {
        SetLevel();
        SetGrade();
    }

    private void SetMaxLevel()
    {
        var minLv = ServerData.etcServerTable.GetSasinsuPowerLowestLevel();

        if (TableManager.Instance.PetAwakeLevel.dataArray.Length <= minLv)
        {
            maxLv = TableManager.Instance.PetAwakeLevel.dataArray.Last().Maxlevel;
        }
        else
        {
            if (minLv < 0)
            {
                maxLv = TableManager.Instance.PetAwakeLevel.dataArray[minLv+1].Maxlevel;
            }
            else
            {
                if (TableManager.Instance.PetAwakeLevel.dataArray[minLv].Grade == (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sasinsuAwakeGrade).Value)
                {
                    maxLv = TableManager.Instance.PetAwakeLevel.dataArray[minLv].Maxlevel;
                }
                else
                {
                    maxLv = TableManager.Instance.PetAwakeLevel.dataArray[minLv+1].Maxlevel;
                }  
            }
              
        }

    }
    private void SetLevel()
    {
        SetMaxLevel();
        
        for (int i = 0; i < levelTexts.Count; i++)
        {
            var level = ServerData.etcServerTable.GetSasinsuPowerLevel(i);

            levelTexts[i].SetText($"({level+1}/{maxLv+1})");
        }
        
        totalGradeText.SetText($"신수 총합 레벨 ({ServerData.etcServerTable.GetSasinsuPowerTotalLevel()+4}/{(maxLv+1)*4})");
    }

    private void SetGrade()
    {
        var str = $"각성 {ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sasinsuAwakeGrade).Value + 1}단계";

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sasinsuAwakeGrade).Value > 0)
        {
            str +=
                $"\n<size=25>{CommonString.GetStatusName(StatusType.SuperCritical30DamPer)} {Utils.ConvertNum(PlayerStats.GetSasinsuAwakePowerAbility(StatusType.SuperCritical30DamPer)*100)}</size>";        }
        else
        {
            str +=
                $"\n<size=25>{CommonString.GetStatusName(StatusType.SuperCritical30DamPer)} 0</size>";        
        }
        currentGradeText.SetText(str);
    }

    public void OnClickAwakeButton()
    {
        if (ServerData.etcServerTable.GetSasinsuPowerTotalLevel() < maxLv * 4)
        {
            PopupManager.Instance.ShowAlarmMessage("레벨이 부족합니다.");
            return;
        }

        if (maxLv >= TableManager.Instance.PetAwakeLevel.dataArray.Last().Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 각성 단계에 도달하였습니다.");
            return;
        }

        //각성조건 
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sasinsuAwakeGrade).Value++;
        ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.sasinsuAwakeGrade,false);
        
        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,"각성 완료!",null);
        SetUi();
    }
}
