using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UiDosulGradeDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI abilDescription;
    [SerializeField]
    private TextMeshProUGUI abilDescription1;


    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI unlockDesc;

    [SerializeField]
    private GameObject equipFrame;

    private int currentIdx;

    private void Start()
    {
        currentIdx = PlayerStats.GetDosulGrade();

        Initialize(currentIdx);
    }

    public void Initialize(int idx)
    {
        if (idx == -1) idx = 0;

        var tableData = TableManager.Instance.dosulTowerTable.dataArray[idx];
        unlockDesc.SetText($"{Utils.ConvertBigNumForRewardCell(tableData.Rewrardcut)}");

        equipFrame.gameObject.SetActive(idx == PlayerStats.GetDosulGrade());

        gradeText.SetText($"{idx + 1}단계");

        var addDescription = "";

        //
        abilDescription.SetText($"달성 보상 : {Utils.ConvertBigNum(tableData.Rewardvalue)}");

        if (PlayerStats.GetDosulGainValue() > 0f)
        {
            abilDescription1.SetText($"소탕 보상 : {Utils.ConvertBigNum(tableData.Sweepvalue)}(+{Utils.ConvertBigNum(tableData.Sweepvalue*PlayerStats.GetDosulGainValue())})");
        }
        else
        {
            abilDescription1.SetText($"소탕 보상 : {Utils.ConvertBigNum(tableData.Sweepvalue)}");
        }
        
        
        //
    }
    
    

    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.dosulTowerTable.dataArray.Length - 1);

        Initialize(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.dosulTowerTable.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.dosulTowerTable.dataArray.Length - 1);

        Initialize(currentIdx);
    }
}
