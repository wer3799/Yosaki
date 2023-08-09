using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UniRx;
using UnityEngine;
public class UiMeditationStartObject : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;
    
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationStart].AsObservable().Subscribe(e =>
        {
            rootObject.SetActive(e == 0);
        }).AddTo(this);
    }

    
    public void OnClickStartButton()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationStart].Value != 0) return;

        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 1000000)
        {
            PopupManager.Instance.ShowAlarmMessage("레벨이 부족합니다!");
            return;
        }
        
        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.TableDatas[GoodsTable.MeditationClearTicket].Value += GameBalance.MeditationTicketDailyGetAmount;
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationStart].Value = 1;
        
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MeditationClearTicket, ServerData.goodsTable.TableDatas[GoodsTable.MeditationClearTicket].Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userinfoParam = new Param();
        userinfoParam.Add(UserInfoTable_2.meditationStart, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationStart].Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfoParam));
        
        ServerData.SendTransactionV2(transactions,
            successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                    $"명상이 시작됐습니다!\n매일 자동으로 {CommonString.GetItemName(Item_Type.MeditationClearTicket)}를 {GameBalance.MeditationTicketDailyGetAmount}개씩 획득 합니다!", null);
            });
    }
}
