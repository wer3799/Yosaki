using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;
public class UiMunhaAwakeBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI gradeText;
    [SerializeField]
    private TextMeshProUGUI abilDescription;

    private int currentIdx;

    private void Start()
    {
        currentIdx = PlayerStats.GetMunhaTowerGrade();

        SetScoreText();

        Initialize(currentIdx);
    }

    private void SetScoreText()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.munhaScore].Value * GameBalance.BossScoreConvertToOrigin)}");
    }
    public void Initialize(int idx)
    {
        if (idx == -1) idx = 0;
        
        var tableData = TableManager.Instance.StudentAwakeTable.dataArray[idx];
        
        string str = "";
        
        str += $"<color=red>{Utils.ConvertBigNumForRewardCell(tableData.Score)}</color>";
        str += $"\n{idx + 1}단계";
        str += idx == PlayerStats.GetMunhaTowerGrade() ? "\n<color=yellow>적용됨</color>" : "";
        
        gradeText.SetText(str);

        abilDescription.SetText($"{CommonString.GetStatusName(StatusType.BigiDamPer)} {Utils.ConvertBigNum(tableData.Awakevalue*100)}");
    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.StudentAwakeTable.dataArray.Length - 1);

        Initialize(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.StudentAwakeTable.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.StudentAwakeTable.dataArray.Length - 1);

        Initialize(currentIdx);
    }

     

    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.MunhaTower);
        }, () => { });
    }
  
}
