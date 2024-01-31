using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManduRefund : MonoBehaviour
{
    private void Start()
    {
        Check();
    }

    private void Check()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.monthreset).Value == 1) return;

        var tableData = TableManager.Instance.oneYearAtten.dataArray;

        int killCountTotal = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.usedCollectionCount).Value;

        int canGetId = 0;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (killCountTotal >= tableData[i].Unlockamount)
            {
                canGetId = i;
            }
        }

        //어느정도 뒤로
        canGetId -= 55;

        if (canGetId <= 0)
        {
            canGetId = -1;
        }

        string freeKey = OneYearPassServerTable.childFree;
        string adKey = OneYearPassServerTable.childAd;

        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiCollectionPass0BuyButton.PassKey].buyCount.Value > 0;

        Param passParam = new Param();
        Param userInfoParam = new Param();

        if (hasIapProduct)
        {
            ServerData.oneYearPassServerTable.TableDatas[freeKey].Value = canGetId.ToString();
            ServerData.oneYearPassServerTable.TableDatas[adKey].Value = canGetId.ToString();
            passParam.Add(freeKey, ServerData.oneYearPassServerTable.TableDatas[freeKey].Value);
            passParam.Add(adKey, ServerData.oneYearPassServerTable.TableDatas[adKey].Value);
        }
        else
        {
            ServerData.oneYearPassServerTable.TableDatas[freeKey].Value = canGetId.ToString();
            passParam.Add(freeKey, ServerData.oneYearPassServerTable.TableDatas[freeKey].Value);
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.userInfoTable.TableDatas[UserInfoTable.monthreset].Value = 1;
        userInfoParam.Add(UserInfoTable.monthreset, ServerData.userInfoTable.TableDatas[UserInfoTable.monthreset].Value);

        transactions.Add(TransactionValue.SetUpdate(OneYearPassServerTable.tableName, OneYearPassServerTable.Indate, passParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            //
        });
    }
}