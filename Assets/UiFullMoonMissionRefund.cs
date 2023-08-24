using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class UiFullMoonMissionRefund : MonoBehaviour
{
    private string iapKey = "fullmoonpass";

    void Start()
    {
#if UNITY_IOS
        return;
#endif

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
        bool showMessage = false;

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

        //미션깬거 초기화 되어서 있어서 1개씩 클리어 된걸로 처리
        ServerData.eventMissionTable.TableDatas["AMission1"].clearCount.Value++;
        ServerData.eventMissionTable.TableDatas["AMission2"].clearCount.Value++;


        if (minusUser)
        {
            //재화차감
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission2).Value -= retakeCount;

            //미션 클리어처리
            ServerData.eventMissionTable.TableDatas["AMission3"].clearCount.Value = 0;
            ServerData.eventMissionTable.TableDatas["AMission3"].rewardCount.Value = 1;
            
            showMessage = true;
        }
        //이전에 보상 받은적이 있고,두번 클리어는 했는데 받지는 않은유저
        else
        {
            if (hasFullMoonPass)
            {
                if (moonCount >= 1000)
                {
                    if (ServerData.eventMissionTable.TableDatas["AMission3"].clearCount.Value >= 5)
                    {
                        ServerData.eventMissionTable.TableDatas["AMission3"].clearCount.Value = 0;
                        ServerData.eventMissionTable.TableDatas["AMission3"].rewardCount.Value = 1;
                        minusUser = true;
                    }
                }
            }
            else
            {
                if (moonCount >= 500)
                {
                    if (ServerData.eventMissionTable.TableDatas["AMission3"].clearCount.Value >= 5)
                    {
                        ServerData.eventMissionTable.TableDatas["AMission3"].clearCount.Value = 0;
                        ServerData.eventMissionTable.TableDatas["AMission3"].rewardCount.Value = 1;
                        minusUser = true;
                    }
                }
            }
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
            if (showMessage)
            {
                PopupManager.Instance.ShowConfirmPopup("중복 보상회수", $"십만대산미션 중복보상 보름달 {retakeCount}개가 회수 되었습니다.", null);
            }
        }
    }
}