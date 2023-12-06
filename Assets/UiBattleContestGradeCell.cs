using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiBattleContestGradeCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI abilityText;
    [SerializeField]
    private TextMeshProUGUI lockMaskText;
    [SerializeField]
    private GameObject lockMask;
    [SerializeField] 
    private Image buttonImage;

    private BattleContestGradeData _tableData;

    [SerializeField] private Sprite Image_Lock;
    [SerializeField] private Sprite Image_CanReward;
    
    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.battleContestGradeLevel).AsObservable().Subscribe(e =>
        {
            lockMask.SetActive(e < _tableData.Id);
        }).AddTo(this);
        ServerData.goodsTable.GetTableData(GoodsTable.BattleScore).AsObservable().Subscribe(e =>
        {
            buttonImage.sprite = e >= _tableData.Condition_Value ? Image_CanReward : Image_Lock;
        }).AddTo(this);
    }

    public void Initialize(BattleContestGradeData tableData)
    {
        _tableData = tableData;
        
        titleText.SetText($"{Utils.ColorToHexString(CommonUiContainer.Instance.itemGradeColor[_tableData.Grade],_tableData.Title_Name)}");

        abilityText.SetText($"{CommonString.GetStatusName((StatusType)_tableData.Abil_Type)} {Utils.ConvertNum(_tableData.Abil_Value*100, 2)}%");
        
        lockMaskText.SetText($"{CommonString.GetItemName(Item_Type.BattleScore)} {_tableData.Condition_Value}점 필요");
        
        Subscribe();
    }

    public void OnClickUpgrade()
    {
        var currentGoods = ServerData.goodsTable.GetTableData(GoodsTable.BattleScore).Value;

        var require = _tableData.Condition_Value;

        if (require > currentGoods)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.BattleScore)}가 부족합니다.");
            return;
        }

        var beforeIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.battleContestGradeLevel).Value;
        

        //업그레이드 가능
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.battleContestGradeLevel).Value = _tableData.Id;

        ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.battleContestGradeLevel, false);


        if (beforeIdx < 0)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등급이 상승하셨습니다.\n" + $"[등급 없음] -> [{_tableData.Title_Name}]", null);
        }
        else
        {
            var beforeData = TableManager.Instance.BattleContestGrade.dataArray[beforeIdx];
            
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등급이 상승하셨습니다.\n" + $"[{beforeData.Title_Name}] -> [{_tableData.Title_Name}]", null);            
        }


    }
    
    // private Coroutine serverSyncRoutine;
    //
    // private void SyncData()
    // {
    //     if (serverSyncRoutine != null)
    //     {
    //         CoroutineExecuter.Instance.StopCoroutine(serverSyncRoutine);
    //     }
    //     serverSyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncServerData());
    //     
    // }
    //
    // private IEnumerator SyncServerData()
    // {
    //     yield return new WaitForSeconds(1.0f);
    //
    //     List<TransactionValue> transactions = new List<TransactionValue>();
    //
    //     Param userinfo2Param = new Param();
    //     userinfo2Param.Add(UserInfoTable_2.battleContestGradeLevel, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.battleContestGradeLevel).Value);
    //     transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));
    //
    //     ServerData.SendTransactionV2(transactions);
    // }

}
