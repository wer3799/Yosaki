using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using WebSocketSharp;

public class UiSasinsuPowerBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentAbilityText;
    [SerializeField] private TextMeshProUGUI nextAbilityText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI abilityDescriptionText;
    [SerializeField] private TextMeshProUGUI sasainsuAwakeGradeText;

    [SerializeField] private List<TextMeshProUGUI> levelTexts = new List<TextMeshProUGUI>();
    [SerializeField] private List<GameObject> masks = new List<GameObject>();
    
    private int currentIdx = 0;

    
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sasinsuAwakeGrade).AsObservable().Subscribe(e =>
        {
            UpdateUi();
        }).AddTo(this);
    }
    public void OnSelectIcon(int idx)
    {
        currentIdx = idx;

        UpdateUi();
    }
    private void UpdateUi()
    {
        SetLevelText();
        SetAbilityText(currentIdx);
        SetAbilityDescription();
    }
    private void SetLevelText()
    {
        for (int i = 0; i < levelTexts.Count; i++)
        {
            levelTexts[i].SetText($"{ServerData.etcServerTable.GetSasinsuPowerLevel(i) + 1}레벨");
        }

        var str = $"신수 각성\n{ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sasinsuAwakeGrade).Value + 1}단계";

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sasinsuAwakeGrade).Value > 0)
        {
            str +=
                $"\n<size=20>{CommonString.GetStatusName(StatusType.SuperCritical30DamPer)} {Utils.ConvertNum(PlayerStats.GetSasinsuAwakePowerAbility(StatusType.SuperCritical30DamPer)*100)}</size>";
        }
        sasainsuAwakeGradeText.SetText(str);
    }

    //0현무 1청룡 2주작 3백호 
    private void SetAbilityText(int idx)
    {
        var tableData = TableManager.Instance.PetAwakeTable.dataArray;
        var levelData = TableManager.Instance.PetAwakeLevel.dataArray;
        var level = ServerData.etcServerTable.GetSasinsuPowerLevel(idx);

        for (int i = 0; i < masks.Count; i++)
        {
            masks[i].SetActive(idx != i);
        }
        
        if (level < 0)
        {
            currentAbilityText.SetText($"{CommonString.GetStatusName((StatusType)tableData[idx].Abiltype)} 0");
        }
        else
        {
            currentAbilityText.SetText($"{CommonString.GetStatusName((StatusType)tableData[idx].Abiltype)} {Utils.ConvertNum(tableData[idx].Abilvalue[level] * 100)}");
        }

        if (tableData[idx].Abilvalue.Length - 1 <= level)
        {
            nextAbilityText.SetText($"최고 단계");
            priceText.SetText("Max");
        }
        else
        {
            nextAbilityText.SetText($"{CommonString.GetStatusName((StatusType)tableData[idx].Abiltype)} {Utils.ConvertNum(tableData[idx].Abilvalue[level+1] * 100)}");
            priceText.SetText($"({Utils.ConvertNum(levelData[level+1].Conditionvalue)})");
        }
    }

    private void SetAbilityDescription()
    {
        string sum = "";
        for (int idx = 0; idx < 4; idx++)
        {
            var tableData = TableManager.Instance.PetAwakeTable.dataArray;

            var level = ServerData.etcServerTable.GetSasinsuPowerLevel(idx);
            if (level < 0)
            {
                continue;
            }
            
            sum +=
                $"{CommonString.GetStatusName((StatusType)tableData[idx].Abiltype)} {Utils.ConvertNum(tableData[idx].Abilvalue[level] * 100)}\n";
        }

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sasinsuAwakeGrade).Value > 0)
        {
            sum+= $"<color=yellow>신수 각성 : {CommonString.GetStatusName(StatusType.SuperCritical30DamPer)} {Utils.ConvertNum(PlayerStats.GetSasinsuAwakePowerAbility(StatusType.SuperCritical30DamPer)*100)}";
        }

        if (sum.IsNullOrEmpty())
        {
            abilityDescriptionText.SetText("획득한 능력치가 없습니다.");

        }
        else
        {
            abilityDescriptionText.SetText(sum);
        }
    }
    public void OnClickUpgradeButton()
    {
        
        var level = ServerData.etcServerTable.GetSasinsuPowerLevel(currentIdx);

        var levelData = TableManager.Instance.PetAwakeLevel.dataArray;

        if (levelData.Length-1 <= level)
        {
            PopupManager.Instance.ShowAlarmMessage("Max 레벨입니다!");
            return;
        }
        
        var nextLevel=level+1;

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sasinsuAwakeGrade).Value < levelData[nextLevel].Grade)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 레벨에 달성하였습니다!\n각성 단계를 올려주세요!");
            return;
        }
        
        var requireValue = levelData[nextLevel].Conditionvalue;
        if (ServerData.goodsTable.GetTableData(GoodsTable.SG).Value < requireValue)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SG)}이 부족합니다!");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.SG).Value -= requireValue;

        string winString = ServerData.etcServerTable.TableDatas[EtcServerTable.sasinsuPowerLevel].Value;
        
        var scoreList = winString.Split(BossServerTable.rewardSplit).Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();

        scoreList[currentIdx]++;

        string newString = "";
        for (int i = 0; i < scoreList.Count; i++)
        {
            newString += $"{BossServerTable.rewardSplit}{scoreList[i]}";
        }

        ServerData.etcServerTable.TableDatas[EtcServerTable.sasinsuPowerLevel].Value = newString;
        
        UpdateUi();
        
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    } 
    private WaitForSeconds syncDelay = new WaitForSeconds(0.6f);

    private Coroutine syncRoutine;
    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        Debug.LogError($"@@@@@@@@@@@@@@@SasinsuPowerOpen SyncComplete@@@@@@@@@@@@@@");

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SG, ServerData.goodsTable.GetTableData(GoodsTable.SG).Value);
        
        Param etcParam = new Param();
        etcParam.Add(EtcServerTable.sasinsuPowerLevel,ServerData.etcServerTable.TableDatas[EtcServerTable.sasinsuPowerLevel].Value);

        List<TransactionValue> transactionList = new List<TransactionValue>();
    
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactionList.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
        });
    }
}
