using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class MeditationRefund : MonoBehaviour
{
    void Start()
    {
        CheckRefund();
    }

    private void CheckRefund()
    {
        #if UNITY_IOS
        return;
        #endif

        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationRefund].Value == 1) return;
        
        int currentRewardIdx = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationTowerRewardIndex].Value;
        
        //시작X 플레이기록X 
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationStart].Value == 0 ||
            currentRewardIdx == 0||
            currentRewardIdx == -1
            )
        {
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationRefund].Value = 1;
            ServerData.userInfoTable_2.UpData(UserInfoTable_2.meditationRefund,false);
            return;
        }
        
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationRefund].Value = 1;

        //2일치
        int refundTicketNum = 4;

        bool hasMeditationpension = ServerData.iapServerTable.TableDatas["meditationpension"].buyCount.Value > 0;

        if (hasMeditationpension)
        {
            //연금구매 5 + 2일치
            refundTicketNum += 7;
        }

        float prefClearValue = TableManager.Instance.MeditationTower.dataArray[currentRewardIdx-1].Sweepvalue;
        
        float currentClearValue = TableManager.Instance.MeditationTower.dataArray[currentRewardIdx].Sweepvalue;

        float refundNum = (currentClearValue - prefClearValue) * refundTicketNum;

        ServerData.goodsTable.TableDatas[GoodsTable.MeditationGoods].Value += refundNum;
        
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable_2.meditationRefund,ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationRefund].Value);

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MeditationGoods,ServerData.goodsTable.TableDatas[GoodsTable.MeditationGoods].Value);
        
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName,UserInfoTable_2.Indate,userInfoParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName,GoodsTable.Indate,goodsParam));
        
        ServerData.SendTransaction(transactions);
        
        PopupManager.Instance.ShowConfirmPopup("알림",$"{CommonString.GetItemName(Item_Type.MeditationGoods)} {Utils.ConvertBigNum(refundNum)}개 소급 완료!",null);
    }

}
