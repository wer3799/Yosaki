using System;
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiEnhanceRelicBoard : MonoBehaviour
{  
    [SerializeField]
    private TextMeshProUGUI grade;
    
    [SerializeField]
    private TextMeshProUGUI cost;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI sumLevel;

    private void OnEnable()
    {
        SetUi();
    }

    private void SetUi()
    {
        SetGrade();

        SetCost();
        
        SetDescription();
    }
    
    private int GetMaxGrade()
    {
        var suho = PlayerStats.GetSpecialSuhoRelicGrade()+1;
        var fox = PlayerStats.GetSpecialFoxRelicGrade()+1;
        var dosul = PlayerStats.GetSpecialDosulRelicGrade()+1;
        var meditation = PlayerStats.GetSpecialMeditationRelicGrade()+1;

        return suho + fox + dosul + meditation;
    }
    
    private void SetGrade()
    {
        grade.SetText($"{ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.enhanceRelicIdx).Value + 1}단계");
    }

    private void SetCost()
    {
        var tabledata = TableManager.Instance.RelicEnhance.dataArray;

        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.enhanceRelicIdx).Value;

        if (idx >= tabledata.Length-1)
        {
            cost.SetText("최고단계");
        }
        else
        {
            cost.SetText(Utils.ConvertNum(tabledata[idx+1].Require));
        }
    }

    private void SetDescription()
    {
        var tabledata = TableManager.Instance.RelicEnhance.dataArray;

        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.enhanceRelicIdx).Value;

        if (idx == -1)
        {
            abilDescription.SetText($"{CommonString.GetStatusName((StatusType)tabledata[0].Abiltype)} : 0배 추가 증가 ");
        }
        else
        {
            abilDescription.SetText($"{CommonString.GetStatusName((StatusType)tabledata[idx].Abiltype)} : {tabledata[idx].Abilvalue}배 추가 증가 ");
        }
        
        sumLevel.SetText($"특별유물 레벨 총합 : {GetMaxGrade()}");
    }

    public void OnClickUpgrade()
    {
        var tabledata = TableManager.Instance.RelicEnhance.dataArray;

        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.enhanceRelicIdx).Value;

        if (idx >= tabledata.Length-1)
        {
            PopupManager.Instance.ShowAlarmMessage("최고단계입니다!");
            return;
        }

        if (idx+1 >= GetMaxGrade())
        {
            PopupManager.Instance.ShowAlarmMessage("한계레벨입니다!");
            return;
        }
        
        var goods = ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value;
        
        var require = tabledata[idx+1].Require;

        if (goods < require)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.StageRelic)}이 부족합니다!");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value -= require;
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.enhanceRelicIdx).Value++;
        
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.StageRelic, ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userInfo_2Param = new Param();
        userInfo_2Param.Add(UserInfoTable_2.enhanceRelicIdx, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.enhanceRelicIdx).Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo_2Param));
        
        
        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            SetUi();
            //PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "유물 강화 성공!", null);
            PopupManager.Instance.ShowAlarmMessage("유물 강화 성공!");
        });
    }
}
