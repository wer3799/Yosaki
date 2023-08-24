using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class UiFullMoonMissionRefund : MonoBehaviour
{
    private string iapKey = "fullmoonpass";

    void Start()
    {
        CheckRefund();
    }

    private void CheckRefund()
    {
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.fullMoonRefund].Value == 1) return;

        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.fullMoonRefund].Value = 1;

        bool hasFullMoonPass = ServerData.iapServerTable.TableDatas["fullmoonpass"].buyCount.Value > 0;
        int retakeCount = hasFullMoonPass ? 1000 : 500;


        int totalExchangeCount = 0;
        int remainMoonHasCount = (int)ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission2).Value;

        var rewardTableData = TableManager.Instance.xMasCollection.dataArray;

        for (int i = 0; i < rewardTableData.Length; i++)
        {
            if (rewardTableData[i].COMMONTABLEEVENTTYPE != CommonTableEventType.FullMoon) continue;

            int exchangeNum = (int)ServerData.userInfoTable.TableDatas[rewardTableData[i].Exchangekey].Value;

            totalExchangeCount += exchangeNum * (int)rewardTableData[i].Price;
        }


        bool minusUser = false;

        int moonCount = totalExchangeCount + remainMoonHasCount;

        if (hasFullMoonPass)
        {
            if (moonCount >= 2000)
            {
                minusUser = true;
            }
        }
        else
        {
            if (moonCount >= 1000)
            {
                minusUser = true;
            }
        }


        if (minusUser)
        {
            //재화차감
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission2).Value -= retakeCount;

            //미션 클리어처리
            ServerData.eventMissionTable.TableDatas["AMission3"].clearCount.Value = 0;
            ServerData.eventMissionTable.TableDatas["AMission3"].rewardCount.Value = 1;
            
            //미션깬거 초기화 됐을수 있어서 1개씩 클리어 된걸로 처리
            ServerData.eventMissionTable.TableDatas["AMission1"].clearCount.Value++;
            ServerData.eventMissionTable.TableDatas["AMission2"].clearCount.Value++;
        }


        List<TransactionValue> transactions = new List<TransactionValue>();

        Param eventMissionParam = new Param();
        Param goodsParam = new Param();
        Param userInfo2Param = new Param();

        eventMissionParam.Add("AMission3", ServerData.eventMissionTable.TableDatas["AMission3"].ConvertToString());
        eventMissionParam.Add("AMission1", ServerData.eventMissionTable.TableDatas["AMission1"].ConvertToString());
        eventMissionParam.Add("AMission2", ServerData.eventMissionTable.TableDatas["AMission2"].ConvertToString());
        
        goodsParam.Add(GoodsTable.Event_Mission2, ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission2).Value);
        userInfo2Param.Add(UserInfoTable_2.fullMoonRefund, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.fullMoonRefund].Value);


        transactions.Add(TransactionValue.SetUpdate(EventMissionTable.tableName, EventMissionTable.Indate, eventMissionParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));


        ServerData.SendTransaction(transactions);

        if (minusUser)
        {
            PopupManager.Instance.ShowConfirmPopup("중복 보상회수", $"십만대산미션 중복보상 보름달 {retakeCount}개가 회수 되었습니다.", null);
        }
    }
}