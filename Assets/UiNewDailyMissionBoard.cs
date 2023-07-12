using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class UiNewDailyMissionBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentBootyAmount;

    [SerializeField]
    private TextMeshProUGUI exchangeDesciprtion;

    [SerializeField]
    private TextMeshProUGUI dailyGetNum;

    [SerializeField]
    private TextMeshProUGUI exChangeJadeNum;

    private void Start()
    {
        Subscribe();

        exchangeDesciprtion.SetText($"전리품 1개당 옥{Utils.ConvertBigNum(GameBalance.JadeExchangeValuePerBooty)}개로 교환하실 수 있습니다.");
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.dailybooty].AsObservable().Subscribe(e =>
        {
            currentBootyAmount.SetText($"{Utils.ConvertBigNum(e)}");

            exChangeJadeNum.SetText($"{Utils.ConvertBigNum(e * GameBalance.JadeExchangeValuePerBooty)}");
        }).AddTo(this);

        int currentStage = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.topClearStageId].Value;

        if (currentStage == -1)
        {
            dailyGetNum.SetText("현재 스테이지에서는 전리품을 획득하실 수 없습니다.");
        }
        else
        {
            var tableData = TableManager.Instance.StageMapData[currentStage];
            dailyGetNum.SetText($"현재 스테이지 {Utils.ConvertStage(currentStage + 1)} 요괴 처치당 전리품 획득량 : {tableData.Dailyitemgetamount * tableData.Marbleamount}개");
        }
    }

    public void OnClickExChangeButton()
    {
        float currentBootyNum = (float)ServerData.userInfoTable.TableDatas[UserInfoTable.dailybooty].Value;

        if (currentBootyNum <= 0f)
        {
            PopupManager.Instance.ShowAlarmMessage("전리품이 없습니다.");
            return;
        }

        float JadeGetAmount = currentBootyNum * GameBalance.JadeExchangeValuePerBooty;

        ServerData.userInfoTable.TableDatas[UserInfoTable.dailybooty].Value = 0f;
        ServerData.goodsTable.TableDatas[GoodsTable.Jade].Value += JadeGetAmount;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.dailybooty, ServerData.userInfoTable.TableDatas[UserInfoTable.dailybooty].Value);

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.TableDatas[GoodsTable.Jade].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        YorinMissionManager.UpdateYorinMissionClear(YorinMissionKey.YMission6_5, 1);
        ServerData.SendTransaction(transactions);

        PopupManager.Instance.ShowAlarmMessage($"옥 {Utils.ConvertBigNum(JadeGetAmount)}개 획득!");
    }
    
    
}