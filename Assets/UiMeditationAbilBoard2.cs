using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiMeditationAbilBoard2 : MonoBehaviour
{
    [FormerlySerializedAs("leftText")] [SerializeField] private TextMeshProUGUI leftAbilText;
    [FormerlySerializedAs("rightText")] [SerializeField] private TextMeshProUGUI rightAbilText;
    [SerializeField] private TextMeshProUGUI rightTitleText;
    
    [SerializeField] private TextMeshProUGUI gradeText;
    
    [SerializeField] private GameObject currentGrade;

    [SerializeField] private GameObject ScrollBar;
    
    private int currentIdx=-1; 
    
    private void OnEnable()
    {
        currentIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value;
        
        PlayerStats.ResetMeditationDictionary();
        
        UpdateByCurrentId();
    }

    private void UpdateByCurrentId()
    {
        currentIdx = Mathf.Max(currentIdx, 0);

        var meditationIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value;
        currentGrade.SetActive(currentIdx == meditationIdx);

        gradeText.SetText($"{currentIdx + 1}단계");
        
        rightTitleText.SetText($"{currentIdx + 1}단계 획득 심득 효과");
        
        //왼쪽
        string description = string.Empty;
        
        var dictionary = PlayerStats.GetMeditationDictionary();
        using var e = dictionary.GetEnumerator();

        while (e.MoveNext())
        {
            if ((int)e.Current.Key == -1) continue;
            description += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertNum(e.Current.Value*100)}\n";
        }

        ScrollBar.SetActive(dictionary.Count >= 13);

        leftAbilText.SetText(description);

        RightAbilDescription();


    }

    private void RightAbilDescription()
    {
        //오른쪽
        string description = string.Empty;

        var tableData = TableManager.Instance.Meditation.dataArray[currentIdx];

        if (tableData.Abiltype.Length == tableData.Abilvalue.Length)
        {
            for (int i = 0; i < tableData.Abiltype.Length; i++)
            {
                if (tableData.Abiltype[i] == -1) continue;
                description += $"{CommonString.GetStatusName((StatusType)tableData.Abiltype[i])} {(tableData.Abilvalue[i]*100).ToString()} 증가\n";
            }
        }

        rightAbilText.SetText(description);
    }

    private int GetNextGrade()
    {
        var tableData = TableManager.Instance.Meditation.dataArray;
        var nextGrade =
            Mathf.Min((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value + 1, tableData.Length - 1);

        return nextGrade;
    }
    public void OnClickRightButton()
    {
        currentIdx++;
        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.Meditation.dataArray.Length - 1);
        UpdateByCurrentId();
    }
    public void OnClickLeftButton()
    {
        currentIdx--;
        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.Meditation.dataArray.Length - 1);
        UpdateByCurrentId();
    }
}
