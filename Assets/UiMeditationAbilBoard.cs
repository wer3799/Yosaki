using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiMeditationAbilBoard : MonoBehaviour
{
    [FormerlySerializedAs("leftText")] [SerializeField] private TextMeshProUGUI leftAbilText;
    [FormerlySerializedAs("rightText")] [SerializeField] private TextMeshProUGUI rightAbilText;
    [SerializeField] private TextMeshProUGUI rightTitleText;
    
    [SerializeField] private TextMeshProUGUI gradeText;
    

    [SerializeField] private GameObject ScrollBar;
    
    private int currentIdx=-1; 
    
    private void OnEnable()
    {
        currentIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value;
        
        PlayerStats.ResetMeditationDictionary();
        PlayerStats.CreateMeditationDictionary();
        
        UpdateByCurrentId();
    }

    private void UpdateByCurrentId()
    {
        gradeText.SetText($"{currentIdx + 1}단계");
        
        rightTitleText.SetText($"{currentIdx + 2}단계 획득 심득 효과");
        
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

        var tableData = TableManager.Instance.Meditation.dataArray;

        if (currentIdx + 1 >= tableData.Length)
        {
            rightAbilText.SetText("추후 업데이트 예정입니다!");
            return;
        }

        if (tableData[currentIdx+1].Abiltype.Length == tableData[currentIdx+1].Abilvalue.Length)
        {
            for (int i = 0; i < tableData[currentIdx+1].Abiltype.Length; i++)
            {
                if (tableData[currentIdx + 1].Abiltype[i] == -1) continue;
                description += $"{CommonString.GetStatusName((StatusType)tableData[currentIdx+1].Abiltype[i])} {(tableData[currentIdx+1].Abilvalue[i]*100).ToString()} 증가\n";
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
}
