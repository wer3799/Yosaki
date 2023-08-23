using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiDanjeonDescription : MonoBehaviour
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
        currentIdx = PlayerStats.GetDanjeonGrade();

        Initialize(currentIdx);
    }

    public void Initialize(int idx)
    {
        if (idx == -1) idx = 0;

        var tableData = TableManager.Instance.DanjeonTable.dataArray[idx];
        unlockDesc.SetText($"{Utils.ConvertBigNumForRewardCell(tableData.Score)}");

        equipFrame.gameObject.SetActive(idx == PlayerStats.GetDanjeonGrade());

        gradeText.SetText($"{idx + 1}단계");

        string description = string.Empty;

        if (tableData.Abiltype.Length == tableData.Abilvalue.Length)
        {
            for (int i = 0; i < tableData.Abiltype.Length; i++)
            {
                if (tableData.Abiltype[i] == -1) continue;
                description += $"{CommonString.GetStatusName((StatusType)tableData.Abiltype[i])} {(tableData.Abilvalue[i]*100).ToString()} 증가\n";
            }
        }

        abilDescription.SetText(description);
    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.DanjeonTable.dataArray.Length - 1);

        Initialize(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.DanjeonTable.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.DanjeonTable.dataArray.Length - 1);

        Initialize(currentIdx);

    }
}
