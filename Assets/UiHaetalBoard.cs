using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiHaetalBoard : SingletonMono<UiHaetalBoard>
{
    [SerializeField] private TextMeshProUGUI totalAbilityText;
    [SerializeField] private TextMeshProUGUI requireText;
    [SerializeField] private TextMeshProUGUI currentAbilityText;
    [SerializeField] private TextMeshProUGUI currentGradeText;
    [SerializeField] private TextMeshProUGUI maxGradeText;

    [SerializeField] private UiHaetalLevelCell prefab;

    [SerializeField] private Transform parent;

    private HaetalTableData currentData;

    private void Start()
    {
        var grade = Mathf.Max((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.haetalGradeIdx).Value, 0);
        
        SetUi(TableManager.Instance.HaetalTable.dataArray[grade]);
        
        Initialize();
        
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex)
            .AsObservable()
            .Subscribe(e =>
            {
                SetMaxGrade();
            })
            .AddTo(this);
    }

    private void OnEnable()
    {
        PlayerStats.ResetHaetal();
    }

    private void MakeCell()
    {
        var tableData = TableManager.Instance.HaetalTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate(prefab, parent);
            cell.Initialize(tableData[i]);
        }
    }

    public void SetUi(HaetalTableData tableData)
    {
        currentData = tableData;
        requireText.SetText($"명상 단계 {currentData.Unlockscore} 이상일 때 입장 가능");
        currentGradeText.SetText($"{currentData.Description}");

        currentAbilityText.SetText($"{CommonString.GetStatusName(currentData.Abiltype)}{Utils.ConvertNum(currentData.Abilvalue * 100, 2)}");
        
    }

    private void SetMaxGrade()
    {
        var tableData = TableManager.Instance.HaetalTable.dataArray;

        var grade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value;

        var idx = -1;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Unlockscore > grade) break;
            idx = i;
        }

        if (idx < 0)
        {
            maxGradeText.SetText($"[입장 불가]");
        }
        else
        {
            maxGradeText.SetText($"[{tableData[idx].Description} 입장 가능]");
        }
        
    }

    private void Initialize()
    {
        totalAbilityText.SetText(
            $"{CommonString.GetStatusName(currentData.Abiltype)} {Utils.ConvertNum(PlayerStats.GetHaetalValue((StatusType)currentData.Abiltype) * 100, 2)}");
        
        MakeCell();
    }

    private bool CanEnter()
    {
        return ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value >= currentData.Unlockscore;
    }
    
    public void OnClickEnterButton()
    {
        if (CanEnter() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("명상 단계가 부족하여 입장할 수 없습니다.");
            return;
        }
        
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{currentData.Description}\n도전 할까요?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.Haetal);
            GameManager.Instance.SetBossId(currentData.Id);
        }, null);
    }
}
