using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiGradeTestDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI unlockDesc;

    [SerializeField]
    private GameObject equipFrame;

    [SerializeField]
    private Image image;

    private int currentIdx;

    private void Start()
    {
        currentIdx = PlayerStats.GetGradeTestGrade();

        Initialize(currentIdx);
        
        Subscribe();
    }
    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gradeTestGraduate).AsObservable().Subscribe(e =>
        {
            Initialize(PlayerStats.GetGradeTestGrade());    
        }).AddTo(this);
        
    }
    public void Initialize(int idx)
    {
        if (idx == -1) idx = 0;

        var tableData = TableManager.Instance.gradeTestTable.dataArray[idx];
        unlockDesc.SetText($"{Utils.ConvertBigNumForRewardCell(tableData.Score)}");

        equipFrame.gameObject.SetActive(idx == PlayerStats.GetGradeTestGrade());

        gradeText.SetText($"{idx + 1}단계");

        string description = string.Empty;

        if (tableData.Abiltype.Length == tableData.Abilvalue.Length)
        {
            for (int i = 0; i < tableData.Abiltype.Length; i++)
            {
                description += $"{CommonString.GetStatusName((StatusType)tableData.Abiltype[i])} {(tableData.Abilvalue[i]*100).ToString()} 증가\n";
            }
        }

        abilDescription.SetText(description);
        if (idx < 17)
        {
            image.sprite = Resources.Load<Sprite>($"GradeTest/{idx}");
        }
        else
        {
            image.sprite = Resources.Load<Sprite>($"GradeTest/{17}");
        }
    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.gradeTestTable.dataArray.Length - 1);

        Initialize(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.gradeTestTable.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.gradeTestTable.dataArray.Length - 1);

        Initialize(currentIdx);

    }
}
