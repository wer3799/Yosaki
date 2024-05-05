using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MunHaRefund : MonoBehaviour
{
    private void Start()
    {
        Check();
    }

    private void Check()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.munhaRefund].Value != 0)
        {
            return;
        }

        int currentIdx = PlayerStats.GetMunhaDispatchGrade();

        //소급대상 제외
        if (currentIdx < 20)
        {
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaRefund).Value = 1;
            ServerData.userInfoTable_2.UpData(UserInfoTable_2.munhaRefund, false);
            return;
        }

        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaRefund).Value = 1;

        List<TransactionValue> tr = new List<TransactionValue>();

        var tableData = TableManager.Instance.StudentDispatch.dataArray;

        var currentData = tableData[currentIdx];

        var sweepValue = currentData.Rewardvalue[0] * 5;

        ServerData.goodsTable.TableDatas[GoodsTable.SB].Value += sweepValue;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SB, ServerData.goodsTable.TableDatas[GoodsTable.SB].Value);
        tr.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable_2.munhaRefund, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaRefund).Value);
        tr.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfoParam));

        ServerData.SendTransaction(tr, successCallBack: () => { });
    }
}