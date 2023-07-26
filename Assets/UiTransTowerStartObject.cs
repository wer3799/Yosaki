using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UniRx;
using UnityEngine;
public class UiTransTowerStartObject : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;
    
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.transTowerStart].AsObservable().Subscribe(e =>
        {
            
            rootObject.SetActive(e == 0);
            
        }).AddTo(this);
    }

    
    public void OnClickStartButton()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.transTowerStart].Value != 0) return;

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.TableDatas[GoodsTable.TransClearTicket].Value += GameBalance.TransTicketWeeklyGetAmount;
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.transTowerStart].Value = 1;
        
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.TransClearTicket,ServerData.goodsTable.TableDatas[GoodsTable.TransClearTicket].Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName,GoodsTable.Indate,goodsParam));

        Param userinfoParam = new Param();
        userinfoParam.Add(UserInfoTable_2.transTowerStart,ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.transTowerStart].Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName,UserInfoTable_2.Indate,userinfoParam));
        
        ServerData.SendTransaction(transactions,
            successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                    $"매주 자동으로 {CommonString.GetItemName(Item_Type.TransClearTicket)}를 {GameBalance.TransTicketWeeklyGetAmount}개씩 획득 합니다!", null);
            });
    }
}
