using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UniRx;
using UnityEngine;

public class UiMunHaLockMask : MonoBehaviour
{
    [SerializeField] private GameObject rootObject;
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.munhaLevel].AsObservable().Subscribe(e =>
        {
            rootObject.gameObject.SetActive(e < 0);
        }).AddTo(this);
    }

    public void OnClickStartButton()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.munhaLevel].Value >= 0) return;

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.munhaLevel].Value = 0;
        
        Param userinfo2Param = new Param();
        userinfo2Param.Add(UserInfoTable_2.munhaLevel,ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.munhaLevel].Value);
        
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName,UserInfoTable_2.Indate,userinfo2Param));
        
        ServerData.SendTransactionV2(transactions,
            successCallBack: () =>
            {
                // PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                //     $"제자  시작되었습니다!", null);
            });
    }
    
    
    
}