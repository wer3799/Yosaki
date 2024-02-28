using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class CurrentStageInfoPopup : SingletonMono<CurrentStageInfoPopup>
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private GameObject rootObject;

    public void ShowInfoPopup(bool show)
    {
        rootObject.SetActive(show);
    }

    private void Start()
    {
        ShowInfoPopup(false);

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).AsObservable().Subscribe(e =>
        {
            SetDescription();
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).AsObservable().Subscribe(e =>
        {
            SetDescription();
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).AsObservable().Subscribe(e =>
        {
            SetDescription();
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).AsObservable().Subscribe(e =>
        {
            SetDescription();
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSumiFire).AsObservable().Subscribe(e =>
        {
            SetDescription();
        }).AddTo(this);
    }

    private void SetDescription()
    {
        var stageData = GameManager.Instance.CurrentStageData;
        var enemyTableData = TableManager.Instance.EnemyData[stageData.Monsterid1];

        string desc = string.Empty;

        desc += $"체력 : {Utils.ConvertBigNum(enemyTableData.Hp)}\n";
        desc += $"방어력 : {Utils.ConvertBigNum(enemyTableData.Defense)}\n";
        desc += $"공격력 : {Utils.ConvertBigNum(enemyTableData.Attackpower)}\n";
        desc += $"보스체력 : {Utils.ConvertBigNum(enemyTableData.Hp * enemyTableData.Bosshpratio)}\n";
        desc += $"보스공격력 : {Utils.ConvertBigNum(enemyTableData.Attackpower * enemyTableData.Bossattackratio)}\n\n";
        desc += $"경험치 : {Utils.ConvertBigNum(enemyTableData.Exp)}\n";
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateGold).Value < 1)
        {
            desc += $"{CommonString.GetItemName(Item_Type.Gold)} : {Utils.ConvertBigNum(enemyTableData.Gold)}\n";
        }
        else
        {
            //수정필요부분(500마리당 이면)
            desc += $"요괴 500마리당 {CommonString.GetItemName(Item_Type.GoldBar)} 획득량 : {stageData.Goldbar * 1000}\n";
        }
        desc += $"{CommonString.GetItemName(Item_Type.GrowthStone)} : {Utils.ConvertBigNum(stageData.Magicstoneamount)}\n";
        desc += $"{CommonString.GetItemName(Item_Type.Marble)} : {Utils.ConvertBigNum(stageData.Marbleamount)}\n";
        desc += $"{CommonString.GetItemName(Item_Type.StageRelic)} : {Utils.ConvertBigNum(stageData.Relicspawnamount)}\n";

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).Value > 0)
        {
            desc += $"\n요괴 500마리당 {CommonString.GetItemName(Item_Type.PeachReal)} 획득량 : {stageData.Peachamount * 1000}";
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).Value > 0)
        {
            desc += $"\n요괴 500마리당 {CommonString.GetItemName(Item_Type.Hel)} 획득량 : {stageData.Helamount * 1000}";
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).Value > 0)
        {
            desc += $"\n요괴 500마리당 {CommonString.GetItemName(Item_Type.Cw)} 획득량 : {stageData.Chunfloweramount * 1000}";
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).Value > 0)
        {
            desc += $"\n요괴 500마리당 {CommonString.GetItemName(Item_Type.DokebiFire)} 획득량 : {stageData.Dokebifireamount * 1000}";
        }
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSumiFire).Value > 0)
        {
            desc += $"\n요괴 500마리당 {CommonString.GetItemName(Item_Type.SumiFire)} 획득량 : {stageData.Sumifloweramount * 1000}";
        }

        if (stageData.Yokaiessence > 0)
        {
            desc += $"\n요괴 500마리당 {CommonString.GetItemName(Item_Type.YoPowerGoods)} 획득량 : {stageData.Yokaiessence * 1000}";
        }

        if (stageData.Taegeuk > 0)
        {
            desc += $"\n요괴 500마리당 {CommonString.GetItemName(Item_Type.TaeguekGoods)} 획득량 : {stageData.Taegeuk * 1000}";
        }
        if (stageData.Sinsu > 0)
        {
            desc += $"\n요괴 500마리당 {CommonString.GetItemName(Item_Type.SG)} 획득량 : {stageData.Sinsu * 1000}";
        }
        
        description.SetText(desc);
    }
}
