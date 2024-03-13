using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiVisionTowerDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI unlockDesc;

    [SerializeField]
    private GameObject equipFrame;

    [SerializeField] private GameObject transBeforeObject;
    [SerializeField] private GameObject transAfterObject;

    [SerializeField]
    private TextMeshProUGUI awakeDescText;

    private int currentIdx;

    private void Start()
    {
        currentIdx = PlayerStats.GetVisionTowerGrade();

        Initialize(currentIdx);
        
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateVisionTower).AsObservable().Subscribe(e =>
        {
            transBeforeObject.SetActive(e < 1);
            transAfterObject.SetActive(e >= 1);
            Initialize(PlayerStats.GetVisionTowerGrade());
        }).AddTo(this);
    }
    public void Initialize(int idx)
    {
        if (idx == -1) idx = 0;

        var tableData = TableManager.Instance.visionTowerTable.dataArray[idx];
        unlockDesc.SetText($"{Utils.ConvertBigNumForRewardCell(tableData.Score)}");

        equipFrame.gameObject.SetActive(idx == PlayerStats.GetVisionTowerGrade());

        gradeText.SetText($"비전 {idx + 1}급");
        
        abilDescription.SetText($"{CommonString.GetStatusName(StatusType.EnhanceVisionSkill)} {Utils.ConvertNum(tableData.Abilvalue0 * 100f)}");

        awakeDescText.SetText($"각성 효과로 강화됩니다.\n" +
                              $"능력치 {GameBalance.VisionTowerGraduatePlusValue}배 증가");
    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.visionTowerTable.dataArray.Length - 1);

        Initialize(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.visionTowerTable.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.visionTowerTable.dataArray.Length - 1);

        Initialize(currentIdx);

    }
}
