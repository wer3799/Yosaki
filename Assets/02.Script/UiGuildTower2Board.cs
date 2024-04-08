using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiGuildTower2Board : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI totalAbilityText;
    [SerializeField]
    private TextMeshProUGUI stageDescription;
    [SerializeField]
    private TextMeshProUGUI requireDamgeText;
    [SerializeField]
    private TextMeshProUGUI currentAbilityText;
    [SerializeField]
    private TextMeshProUGUI guildCurrentScoreText;
    [SerializeField]
    private TextMeshProUGUI guildTotalScoreText;
    [SerializeField]
    private TextMeshProUGUI currentStageText;
    
    [SerializeField]
    private Button leftButton;
    [SerializeField]
    private Button rightButton;


    
    private int currentId;

    private int score = 0;
    private void OnEnable()
    {
        Initialize();
    }

    public void UpdateRewardView(int idx)
    {
        currentId = idx;

        currentId = Mathf.Min(TableManager.Instance.GuildTowerTable2.dataArray.Length - 1, currentId);
        
        var towerTableData = TableManager.Instance.GuildTowerTable2.dataArray[currentId];
        
        stageDescription.SetText($"{towerTableData.Description}");

        requireDamgeText.SetText($"{Utils.ConvertNum(towerTableData.Hp)}");
        
        currentAbilityText.SetText($"{CommonString.GetStatusName((StatusType)towerTableData.Abiltype)} {Utils.ConvertNum(towerTableData.Abilvalue*100)}");

        guildCurrentScoreText.SetText($"문파 총점 : {towerTableData.Unlockscore}점 이상 시 입장가능");
        
        UpdateButtonState();
    }

    private void Initialize()
    {
        var currentScore = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value;

        UpdateRewardView(currentScore);
        
        score = UiGuildMemberList.Instance.GetGuildTowerTotalScore();
        
        GameManager.Instance.SetTowerScore(score);

        totalAbilityText.SetText($"{CommonString.GetStatusName(StatusType.DokChimHasValueUpgrade)} {Utils.ConvertNum(PlayerStats.GetGuildTower2TotalAbility(StatusType.DokChimHasValueUpgrade)*100,2)}");

        guildTotalScoreText.SetText($"문파 총점 : {score}점");

        if (TableManager.Instance.GuildTowerTable2.dataArray.Length - 1<currentScore+1)
        {
            currentStageText.SetText($"현재 {currentScore}층");
        }
        else
        {
            currentStageText.SetText($"현재 {currentScore+1}층");
        }
    }

    public void OnClickLeftButton()
    {
        currentId--;

        currentId = Mathf.Max(currentId, 0);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }
    public void OnClickRightButton()
    {
        currentId++;

        currentId = Mathf.Min(currentId, TableManager.Instance.GuildTowerTable2.dataArray.Length - 1);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        leftButton.interactable = currentId != 0;
        rightButton.interactable = currentId != TableManager.Instance.GuildTowerTable2.dataArray.Length - 1;
    }
    
    
     

    public void OnClickEnterButton()
    {
        var currentIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value;
        
        if (TableManager.Instance.GuildTowerTable2.dataArray.Length - 1 < currentIdx)
        {
            PopupManager.Instance.ShowAlarmMessage("모두 클리어하셨습니다.");
            return;
        }
        
        var unlockScore = TableManager.Instance.GuildTowerTable2
            .dataArray[currentIdx]
            .Unlockscore;
        if (unlockScore > score)
        {
            PopupManager.Instance.ShowAlarmMessage("문파 총점이 부족하여 입장할 수 없습니다.");
            return;
        }


        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.GuildTower2);
        }, () => { });
    }
}
