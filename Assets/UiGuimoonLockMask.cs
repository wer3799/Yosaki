using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UniRx;
using UnityEngine;

public class UiGuimoonLockMask : MonoBehaviour
{
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.guimoonRelicStart].AsObservable().Subscribe(e =>
        {
            
            this.gameObject.SetActive(e == 0);
            
        }).AddTo(this);
    }

    public void OnClickStartButton()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.guimoonRelicStart].Value != 0) return;

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.TableDatas[GoodsTable.GuimoonRelicClearTicket].Value += GameBalance.GuimoonTicketDailyGetAmount;
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.guimoonRelicStart].Value = 1;
        
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.GuimoonRelicClearTicket,ServerData.goodsTable.TableDatas[GoodsTable.GuimoonRelicClearTicket].Value);

        Param userinfo2Param = new Param();
        userinfo2Param.Add(UserInfoTable_2.guimoonRelicStart,ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.guimoonRelicStart].Value);
        
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName,GoodsTable.Indate,goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName,UserInfoTable_2.Indate,userinfo2Param));
        
        ServerData.SendTransactionV2(transactions,
            successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                    $"귀문개방이 시작되었습니다!\n매일 자동으로 {CommonString.GetItemName(Item_Type.GuimoonRelicClearTicket)}을 {GameBalance.GuimoonTicketDailyGetAmount}개씩 획득 합니다!", null);
            });
    }
    
    
    
}