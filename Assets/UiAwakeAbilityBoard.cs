using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using BackEnd;
using LitJson;
using UnityEngine.UI;

public class UiAwakeAbilityBoard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> gradeTexts;
    [SerializeField]
    private List<TextMeshProUGUI> abilityTexts;
    [SerializeField]
    private List<TextMeshProUGUI> requireTexts;

    private List<int> currentIdx = new List<int>() { 0, 0, 0 };

    private List<int> maxList = new List<int>() { 0, 0, 0 };

    private void Start()
    {
        Initialize();
        
        UpdateUi();

    }

    private void OnEnable()
    {
        SetMinMax();
    }

    private void Initialize()
    {
        currentIdx[(int)AbilAwakeType.Vision] = Mathf.Max((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeVisionSkill).Value, 0);
        currentIdx[(int)AbilAwakeType.SealSword] = Mathf.Max((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeSealSword).Value, 0);
        currentIdx[(int)AbilAwakeType.Dosul] = Mathf.Max((int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeDosulSkill).Value, 0);

        SetMinMax();
    }

    private void SetMinMax()
    {
        var tableData = TableManager.Instance.AbilAwakeTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            switch (tableData[i].ABILAWAKETYPE)
            {
                case AbilAwakeType.Vision:
                    maxList[(int)AbilAwakeType.Vision] = tableData[i].Grade;
                    break;
                case AbilAwakeType.SealSword:
                    maxList[(int)AbilAwakeType.SealSword] = tableData[i].Grade;
                    break;
                case AbilAwakeType.Dosul:
                    maxList[(int)AbilAwakeType.Dosul] =  tableData[i].Grade;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
    private void UpdateUi()
    {
        SetGrade();
        SetAbility();
        SetRequire();
    }
    
    private void SetGrade()
    {
        var visionGrade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeVisionSkill).Value;
        var sealSwordGrade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeSealSword).Value;
        var dosulGrade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeDosulSkill).Value;

        string desc0 = $"{currentIdx[(int)AbilAwakeType.Vision] + 1}단계";
        string desc1 = $"{currentIdx[(int)AbilAwakeType.SealSword] + 1}단계";
        string desc2 = $"{currentIdx[(int)AbilAwakeType.Dosul] + 1}단계";

        if (currentIdx[(int)AbilAwakeType.Vision] == visionGrade)
        {
            desc0 += $"\n<color=yellow>(적용중)</color>";
        }
        if (currentIdx[(int)AbilAwakeType.SealSword] == sealSwordGrade)
        {
            desc1+= $"\n<color=yellow>(적용중)</color>";
        }
        if (currentIdx[(int)AbilAwakeType.Dosul] == dosulGrade)
        {
            desc2 += $"\n<color=yellow>(적용중)</color>";
        }
        
        gradeTexts[(int)AbilAwakeType.Vision].SetText(desc0);
        gradeTexts[(int)AbilAwakeType.SealSword].SetText(desc1);
        gradeTexts[(int)AbilAwakeType.Dosul].SetText(desc2);
    }

    private void SetAbility()
    {
        var tableData = TableManager.Instance.AbilAwakeTable.dataArray;
        for (int i = 0; i < tableData.Length; i++)
        {
            switch (tableData[i].ABILAWAKETYPE)
            {
                case AbilAwakeType.Vision:
                    if (tableData[i].Grade == currentIdx[(int)AbilAwakeType.Vision])
                    {
                        string desc = "";
                        for (int j = 0 ; j < tableData[i].Abiltype.Length;j++)
                        {
                            if(tableData[i].Abiltype[j]==-1) continue;

                            if (Utils.IsPercentStat((StatusType)tableData[i].Abiltype[j]))
                            {
                                desc += $"{CommonString.GetStatusName((StatusType)tableData[i].Abiltype[j])} : {Utils.ConvertNum(tableData[i].Abilvalue[j] * 100)}\n";
                            }
                            else
                            {
                                desc += $"{CommonString.GetStatusName((StatusType)tableData[i].Abiltype[j])} : {Utils.ConvertNum(tableData[i].Abilvalue[j],1)}\n";
                            }
                        }
                        
                        abilityTexts[(int)AbilAwakeType.Vision].SetText(desc);
                    }
                    break;
                case AbilAwakeType.SealSword:
                    if (tableData[i].Grade == currentIdx[(int)AbilAwakeType.SealSword])
                    {
                        string desc = "";
                        for (int j = 0 ; j < tableData[i].Abiltype.Length;j++)
                        {
                            if(tableData[i].Abiltype[j]==-1) continue;
                            if (Utils.IsPercentStat((StatusType)tableData[i].Abiltype[j]))
                            {
                                desc += $"{CommonString.GetStatusName((StatusType)tableData[i].Abiltype[j])} : {Utils.ConvertNum(tableData[i].Abilvalue[j] * 100)}\n";
                            }
                            else
                            {
                                desc += $"{CommonString.GetStatusName((StatusType)tableData[i].Abiltype[j])} : {Utils.ConvertNum(tableData[i].Abilvalue[j],1)}\n";
                            }                        }
                        
                        abilityTexts[(int)AbilAwakeType.SealSword].SetText(desc);
                    }
                    break;
                case AbilAwakeType.Dosul:
                    if (tableData[i].Grade == currentIdx[(int)AbilAwakeType.Dosul])
                    {
                        string desc = "";
                        for (int j = 0 ; j < tableData[i].Abiltype.Length;j++)
                        {
                            if(tableData[i].Abiltype[j]==-1) continue;
                            if (Utils.IsPercentStat((StatusType)tableData[i].Abiltype[j]))
                            {
                                desc += $"{CommonString.GetStatusName((StatusType)tableData[i].Abiltype[j])} : {Utils.ConvertNum(tableData[i].Abilvalue[j] * 100)}\n";
                            }
                            else
                            {
                                desc += $"{CommonString.GetStatusName((StatusType)tableData[i].Abiltype[j])} : {Utils.ConvertNum(tableData[i].Abilvalue[j],1)}\n";
                            }                        }
                        
                        abilityTexts[(int)AbilAwakeType.Dosul].SetText(desc);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }


    private void SetRequire()
    {
         var tableData = TableManager.Instance.AbilAwakeTable.dataArray;
        for (int i = 0; i < tableData.Length; i++)
        {
            switch (tableData[i].ABILAWAKETYPE)
            {
                case AbilAwakeType.Vision:
                    if (tableData[i].Grade == currentIdx[(int)AbilAwakeType.Vision])
                    {
                        requireTexts[(int)AbilAwakeType.Vision].SetText($"폐관수련 {tableData[i].Unlockclosedtraining+1}단계 이상\n비전 급수 {tableData[i].Unlockcontents+1} 급 이상");
                    }
                    break;
                case AbilAwakeType.SealSword:
                    if (tableData[i].Grade == currentIdx[(int)AbilAwakeType.SealSword])
                    {
                        requireTexts[(int)AbilAwakeType.SealSword].SetText($"폐관수련 {tableData[i].Unlockclosedtraining+1}단계 이상\n요도 해방 {tableData[i].Unlockcontents+1} 층 이상");
                    }
                    break;
                case AbilAwakeType.Dosul:
                    if (tableData[i].Grade == currentIdx[(int)AbilAwakeType.Dosul])
                    {
                        requireTexts[(int)AbilAwakeType.Dosul].SetText($"폐관수련 {tableData[i].Unlockclosedtraining+1}단계 이상\n도술 레벨 {tableData[i].Unlockcontents+1} 이상");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public void OnClickRightButton(int idx)
    {
        var tableData = TableManager.Instance.AbilAwakeTable.dataArray;

        currentIdx[idx]++;

        if (currentIdx[idx] == tableData.Length)
        {
            PopupManager.Instance.ShowAlarmMessage("최고단계 입니다.");
        }

        
        currentIdx[idx] = Mathf.Min(currentIdx[idx], maxList[idx]);

        UpdateUi();
    }

    public void OnClickLeftButton(int idx)
    {
        currentIdx[idx]--;

        if (currentIdx[idx] <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("처음단계 입니다.");
        }

        currentIdx[idx] = Mathf.Max(currentIdx[idx], 0);

        UpdateUi();
    }
    public void OnClickAwakeButton(int idx)
    {
        switch ((AbilAwakeType)idx)
        {
            case AbilAwakeType.Vision:
                if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeVisionSkill).Value >= maxList[idx])
                {
                    PopupManager.Instance.ShowAlarmMessage("최대 레벨입니다!");
                    return;
                }
                if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeVisionSkill).Value >= PlayerStats.CanAbilAwakeGrade((AbilAwakeType)idx))
                {
                    PopupManager.Instance.ShowAlarmMessage("요구 조건을 충족해야합니다!");
                    return;
                }

                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeVisionSkill).Value++;
                ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.awakeVisionSkill,false);
                currentIdx[idx] = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeVisionSkill).Value;
                break;
            case AbilAwakeType.SealSword:
                if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeSealSword).Value >= maxList[idx])
                {
                    PopupManager.Instance.ShowAlarmMessage("최대 레벨입니다!");
                    return;
                }
                if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeSealSword).Value >= PlayerStats.CanAbilAwakeGrade((AbilAwakeType)idx))
                {
                    PopupManager.Instance.ShowAlarmMessage("요구 조건을 충족해야합니다!");
                    return;
                }

                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeSealSword).Value++;
                ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.awakeSealSword,false);
                currentIdx[idx] = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeSealSword).Value;
                break;
            case AbilAwakeType.Dosul:
                if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeDosulSkill).Value >= maxList[idx])
                {
                    PopupManager.Instance.ShowAlarmMessage("최대 레벨입니다!");
                    return;
                }
                if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeDosulSkill).Value >= PlayerStats.CanAbilAwakeGrade((AbilAwakeType)idx))
                {
                    PopupManager.Instance.ShowAlarmMessage("요구 조건을 충족해야합니다!");
                    return;
                }

                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeDosulSkill).Value++;
                ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.awakeDosulSkill,false);
                currentIdx[idx] = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeDosulSkill).Value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(idx), idx, null);
        }
        PopupManager.Instance.ShowAlarmMessage("각성 완료!");
        UpdateUi();
    }
    
    public void OnClickEnterButton()

    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.ClosedTraining);
        }, () => { });
    }
}
