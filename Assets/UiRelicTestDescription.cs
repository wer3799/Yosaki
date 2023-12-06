using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiRelicTestDescription : MonoBehaviour
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
        currentIdx = PlayerStats.GetRelicTestGrade();

        Initialize(currentIdx);
        
        Subscribe();
    }
    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.relicTestGraduate).AsObservable().Subscribe(e =>
        {
            Initialize(PlayerStats.GetRelicTestGrade());
        }).AddTo(this);
        
    }
    public void Initialize(int idx)
    {
        if (idx == -1) idx = 0;

        var tableData = TableManager.Instance.RelicTest.dataArray[idx];
        unlockDesc.SetText($"{Utils.ConvertBigNumForRewardCell(tableData.Score)}");

        equipFrame.gameObject.SetActive(idx == PlayerStats.GetRelicTestGrade());

        gradeText.SetText($"{idx + 1}단계");

        string description = string.Empty;
        
        description += $"영혼의숲 능력치\n{tableData.Abilvalue}배 상승\n<color=red>(공격 능력치) 증가\n";

        abilDescription.SetText(description);
    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.RelicTest.dataArray.Length - 1);

        Initialize(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.RelicTest.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.RelicTest.dataArray.Length - 1);

        Initialize(currentIdx);

    }
}
