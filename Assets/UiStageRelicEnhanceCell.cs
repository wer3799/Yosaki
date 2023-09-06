using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiStageRelicEnhanceCell : MonoBehaviour
{
    public enum SpecialRelic
    {
        None,
        Suho,
        Fox,
        Dosul,
        Meditation,
    }

    [SerializeField] private Image icon;

    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private TextMeshProUGUI relicName;

    [SerializeField] private TextMeshProUGUI nextGradeDesc;

    [SerializeField] private TextMeshProUGUI relicDescription;

    [SerializeField] private SpecialRelic _specialRelic = SpecialRelic.None;
    private RelicSpecialData[] tableData;
    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        tableData = TableManager.Instance.RelicSpecial.dataArray;
        
        relicName.SetText(SetName());
        
        nextGradeDesc.SetText(SetNextGrade());
        
        levelText.SetText($"Lv : {SetLevel()}");
        
        relicDescription.SetText(SetAbilDescription());
    }

    private string SetName()
    {
        return _specialRelic switch
        {
            SpecialRelic.None => "미등록",
            SpecialRelic.Suho => "수호의 유물",
            SpecialRelic.Fox => "여우의 유물",
            SpecialRelic.Dosul => "도술의 유물",
            SpecialRelic.Meditation => "내면의 유물",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private string SetNextGrade()
    {
        switch (_specialRelic)
        {
            case SpecialRelic.None:
                return "미등록";
            case SpecialRelic.Suho:
                var grade1 = PlayerStats.GetSpecialSuhoRelicGrade();

                var nextGrade1 = Mathf.Min(grade1 + 1, tableData.Length - 1);

                if (tableData[nextGrade1].Suhorequire == -1 || grade1 + 1 >= tableData.Length)
                {
                    return "Max 레벨 달성";
                }
                else
                {
                    return $"다음 레벨 : 수호동물 {TableManager.Instance.suhoPetTable.dataArray[tableData[nextGrade1].Suhorequire].Name} 획득";
                }
            case SpecialRelic.Fox:
                var grade2 = PlayerStats.GetSpecialFoxRelicGrade();

                var nextGrade2 = Mathf.Min(grade2 + 1, tableData.Length - 1);

                if (tableData[nextGrade2].Foxrequire == -1 || grade2 + 1 >= tableData.Length)
                {
                    return "Max 레벨 달성";
                }
                else
                {
                    return $"다음 레벨 : 여우불 {tableData[nextGrade2].Foxrequire} 단계 달성";
                }
            case SpecialRelic.Dosul:
                var grade3 = PlayerStats.GetSpecialDosulRelicGrade();

                var nextGrade3 = Mathf.Min(grade3 + 1, tableData.Length - 1);

                if (tableData[nextGrade3].Dosulrequire == -1 || grade3 + 1 >= tableData.Length)
                {
                    return "Max 레벨 달성";
                }
                else
                {
                    return $"다음 레벨 : 도술 {tableData[nextGrade3].Dosulrequire+1} LV 달성";
                }            
            case SpecialRelic.Meditation:
                var grade4 = PlayerStats.GetSpecialMeditationRelicGrade();

                var nextGrade4 = Mathf.Min(grade4 + 1, tableData.Length - 1);

                if (tableData[nextGrade4].Meditationrequire == -1 || grade4 + 1 >= tableData.Length)
                {
                    return "Max 레벨 달성";
                }
                else
                {
                    return $"다음 레벨 : 명상 {tableData[nextGrade4].Meditationrequire+1} 단계 달성";
                }                     default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private int SetLevel()
    {
        switch (_specialRelic)
        {
            case SpecialRelic.None:
                return 0;
            case SpecialRelic.Suho:
                return PlayerStats.GetSpecialSuhoRelicGrade() + 1;
            case SpecialRelic.Fox:
                return PlayerStats.GetSpecialFoxRelicGrade() + 1;
            case SpecialRelic.Dosul:
                return PlayerStats.GetSpecialDosulRelicGrade() + 1;
            case SpecialRelic.Meditation:
                return PlayerStats.GetSpecialMeditationRelicGrade() + 1;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private string SetAbilDescription()
    {
        switch (_specialRelic)
        {
            case SpecialRelic.None:
                return "미등록";
            case SpecialRelic.Suho:
                var grade = PlayerStats.GetSpecialSuhoRelicGrade();
                if (grade == -1)
                {
                    return $"{CommonString.GetStatusName((StatusType)tableData[0].Suhoabiltype)} : {0}";
                }
                else
                {
                    return $"{CommonString.GetStatusName((StatusType)tableData[grade].Suhoabiltype)} : {tableData[grade].Suhoabilvalue*100f}";
                }
            case SpecialRelic.Fox:
                
                var grade1 = PlayerStats.GetSpecialFoxRelicGrade();

                if (grade1 == -1)
                {
                    return $"{CommonString.GetStatusName((StatusType)tableData[0].Foxabiltype)} : {0}";
                }
                else
                {
                    return $"{CommonString.GetStatusName((StatusType)tableData[grade1].Foxabiltype)} : {tableData[grade1].Foxabilvalue*100f}";
                }
            case SpecialRelic.Dosul:

                var grade2 = PlayerStats.GetSpecialDosulRelicGrade();

                if (grade2 == -1)
                {
                    return $"{CommonString.GetStatusName((StatusType)tableData[0].Dosulabiltype)} : {0}";
                }
                else
                {
                    return $"{CommonString.GetStatusName((StatusType)tableData[grade2].Dosulabiltype)} : {tableData[grade2].Dosulabilvalue*100f}";
                }            
            case SpecialRelic.Meditation:
          
                var grade3 = PlayerStats.GetSpecialMeditationRelicGrade();

                if (grade3 == -1)
                {
                    return $"{CommonString.GetStatusName((StatusType)tableData[0].Meditationabiltype)} : {0}";
                }
                else
                {
                    return $"{CommonString.GetStatusName((StatusType)tableData[grade3].Meditationabiltype)} : {tableData[grade3].Meditationabilvalue*100f}";
                }        
            default:
                
                throw new ArgumentOutOfRangeException();
        }
    }
}


