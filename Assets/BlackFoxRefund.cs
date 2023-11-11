using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BlackFoxRefund : MonoBehaviour
{
    private void Start()
    {
        Check();
    }

    private void Check()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.blackFoxRefund).Value == 1)
        {
            return;
        }

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.blackFoxScore].Value *
                    GameBalance.BossScoreConvertToOrigin;


        int grade = (int)PlayerStats.GetBlackFoxGrade();

        if (grade == 0 || grade == -1 ||score == 0d)
        {
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.blackFoxRefund].Value = 1;
            ServerData.userInfoTable_2.UpData(UserInfoTable_2.blackFoxRefund, false);
            return;
        }

        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.blackFoxRefund].Value = 1;

        var tableDatas = TableManager.Instance.BlackFoxAbil.dataArray;

        float usedPoint = ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            usedPoint += ServerData.blackFoxServerTable.TableDatas[tableDatas[i].Stringid].level.Value;
        }

        //10등급정도 올랐다고침
        int usedTicketNum = (int)((usedPoint + 10) / grade);

        //usedTicketNum -= (int)ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxClear).Value;

        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.usedblackFoxClearNum].Value = usedTicketNum;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfo2Param = new Param();
        userInfo2Param.Add(UserInfoTable_2.blackFoxRefund, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.blackFoxRefund).Value);
        userInfo2Param.Add(UserInfoTable_2.usedblackFoxClearNum, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.usedblackFoxClearNum).Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));

        ServerData.SendTransaction(transactions, successCallBack: () => { });
    }
}