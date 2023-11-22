using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiByeolHoBoard : MonoBehaviour
{
    [SerializeField] private UiByeolHoAbilCell prefab;
    [SerializeField] private Transform transform;


    [SerializeField] private TextMeshProUGUI curGradeText;
    [SerializeField] private TextMeshProUGUI curAbilityText;

    [SerializeField] private TextMeshProUGUI gaugeText;
    [SerializeField] private Image gaugeImage;

    [SerializeField] private Button levelUpButton;
    

    private bool initialized = false;
    private void Start()
    {
        MakeCell();
        
        Subscribe();
        
        if (PlayerPrefs.HasKey(SettingKey.showByeolhotitle) == false)
            PlayerPrefs.SetInt(SettingKey.showByeolhotitle, 1);     
        
        
        initialized = true;
    }
#if UNITY_EDITOR
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            var data = TableManager.Instance.Byeolho.dataArray[
                (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoLevelIdx).Value];
            var str = data.Title_Name;
            var color =CommonUiContainer.Instance.itemGradeColor[data.Grade];

            str = Utils.ColorToHexString(color, str);

            PopupManager.Instance.ShowAlarmMessage($"{str} {CommonString.ChatSplitChar} test");
        }  
        
    }
#endif
    private void OnEnable()
    {
        int limitLv = 1000000;
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < limitLv)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage($"레벨 100만 이상일 때 개방됩니다!");
        }
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.ByeolhoGoods).AsObservable().Subscribe(e =>
        {
            UpdateUi();
        }).AddTo(this);
        
    }

    private void UpdateUi()
    {
        var grade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoLevelIdx).Value;
        var tableData = TableManager.Instance.Byeolho.dataArray;
        if (grade < 0)
        {
            curGradeText.SetText("획득 별호 없음");
            curAbilityText.SetText("피해량 0%");
        }
        else
        {
            var currentData = tableData[grade];
        
            curGradeText.SetText($"{Utils.ColorToHexString(CommonUiContainer.Instance.itemGradeColor[currentData.Grade],currentData.Title_Name)}");
            curAbilityText.SetText(
                $"{CommonString.GetStatusName((StatusType)currentData.Abil_Type)} {Utils.ConvertNum(currentData.Abil_Value*100, 2)}%");
        }


        if (grade + 1 >= tableData.Length)
        {
            gaugeText.SetText("Max");
            gaugeImage.fillAmount = 1;
            levelUpButton.interactable = false;
        }
        else
        {
            var nextData = tableData[grade + 1];

            var currentGoods = ServerData.goodsTable.GetTableData(Item_Type.ByeolhoGoods).Value;
            
            gaugeText.SetText($"{Utils.ConvertNum(currentGoods)} / {Utils.ConvertNum(nextData.Condition_Value)}");
            gaugeImage.fillAmount = currentGoods/nextData.Condition_Value;

        }

        levelUpButton.interactable = CanLevelUp();
    }
    
    private void MakeCell()
    {
        var tableData = TableManager.Instance.Byeolho.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            //tableData[i].
            var cell = Instantiate<UiByeolHoAbilCell>(prefab, transform);
            cell.Initialize(tableData[i]);
        }
    }

    private bool CanLevelUp()
    {
        var currentIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoLevelIdx).Value;
        var tableData = TableManager.Instance.Byeolho.dataArray;
        if (currentIdx + 1 >= tableData.Length)
        {
            //PopupManager.Instance.ShowAlarmMessage("최대 레벨입니다.");
            return false;
        }

        var nextData = tableData[currentIdx + 1];

        if (ServerData.goodsTable.GetTableData(GoodsTable.ByeolhoGoods).Value < nextData.Condition_Value)
        {
            //PopupManager.Instance.ShowAlarmMessage("재화가 부족합니다.");
            return false;
        }

        return true;
    }
    
    public void OnClickLevelUpButton()
    {
        levelUpButton.interactable = false;

        var currentIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoLevelIdx).Value;
        var tableData = TableManager.Instance.Byeolho.dataArray;
        if (currentIdx + 1 >= tableData.Length)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 레벨입니다.");
            return;
        }

        var nextData = tableData[currentIdx + 1];

        if (ServerData.goodsTable.GetTableData(GoodsTable.ByeolhoGoods).Value < nextData.Condition_Value)
        {
            PopupManager.Instance.ShowAlarmMessage("재화가 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.ByeolhoGoods).Value -= nextData.Condition_Value;
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoLevelIdx).Value++;

        List<TransactionValue> transaction = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.ByeolhoGoods,ServerData.goodsTable.GetTableData(GoodsTable.ByeolhoGoods).Value);
        
        
        Param userinfo2Param = new Param();
        userinfo2Param.Add(UserInfoTable_2.byeolhoLevelIdx,ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoLevelIdx).Value);

        transaction.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transaction.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));
        
        ServerData.SendTransactionV2(transaction,successCallBack:(() =>
        {
            levelUpButton.interactable = true;

            if (currentIdx < 0)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,$"단계가 상승하셨습니다!\n" + $"[없음]->[{tableData[currentIdx+1].Title_Name}]",null);
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,$"단계가 상승하셨습니다!\n" + $"[{tableData[currentIdx].Title_Name}]->[{tableData[currentIdx+1].Title_Name}]",null);
            }
            
            UpdateUi();
        }));
        
    }
    
    public void ShowByeolhoTitleOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showByeolhotitle.Value = on ? 1 : 0;
    }
}
