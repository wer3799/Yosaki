using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UniRx;
using UnityEngine;
public class UiRelicUpgradeStartObject : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;
    
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.getRelicUpgrade].AsObservable().Subscribe(e =>
        {
            
            rootObject.SetActive(e == 0);
            
        }).AddTo(this);
    }

    
    public void OnClickStartButton()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.getRelicUpgrade].Value != 0) return;

        var tabledata = TableManager.Instance.NewGachaTable.dataArray;

        float rewardSum = 0f;
        
        List<TransactionValue> transactions = new List<TransactionValue>();
        
        Param ringParam = new Param();
        
        for (int i = 0; i < tabledata.Length; i++)
        {
            //if (string.Equals(tabledata[i].Stringid,"Ring27")) continue;
            
            var amount = ServerData.newGachaServerTable.TableDatas[tabledata[i].Stringid].amount.Value;

            rewardSum += tabledata[i].Exchage * amount;
            ServerData.newGachaServerTable.TableDatas[tabledata[i].Stringid].amount.Value = 0;
            ringParam.Add(tabledata[i].Stringid, ServerData.newGachaServerTable.TableDatas[tabledata[i].Stringid].ConvertToString());
        }
        transactions.Add(TransactionValue.SetUpdate(NewGachaServerTable.tableName, NewGachaServerTable.Indate, ringParam));

        ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += rewardSum;
        
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.TableDatas[GoodsTable.NewGachaEnergy].Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.getRelicUpgrade].Value = 1;
        
        Param userinfo2Param = new Param();
        userinfo2Param.Add(UserInfoTable_2.getRelicUpgrade,ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.getRelicUpgrade].Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName,UserInfoTable_2.Indate,userinfo2Param));

        
        ServerData.SendTransactionV2(transactions,
            successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                    $"영혼석 {rewardSum}개 지급!", null);
            });
    }

}
