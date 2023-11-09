using System;
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIrelicBoard : MonoBehaviour
{
    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private UiRelicCell uiRelicCell;

    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    [SerializeField]
    private TextMeshProUGUI enterButtonText;

    [SerializeField] private GameObject relicTestBoard; 
    
    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.relicKillCount].AsObservable().Subscribe(e =>
        {
            enterButtonText.SetText(e >= GameBalance.RelicDungeonGraduateScore ? "영혼사냥" : "입장");
        }).AddTo(this);
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.RelicTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var cell = Instantiate<UiRelicCell>(uiRelicCell, cellParents);

            cell.Initialize(tableDatas[i]);
        }

        bestScoreText.SetText($"최고점수:{(int)ServerData.userInfoTable.TableDatas[UserInfoTable.relicKillCount].Value}");
    }

    public void OnClickEnterButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.relicKillCount].Value < 80000)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
            {
                GameManager.Instance.LoadContents(GameManager.ContentsType.RelicDungeon);
            }, () => { });   
        }
        else
        {
            relicTestBoard.SetActive(true);
        }
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value += 100000;
        }
    }

#endif

    //소탕

    [SerializeField]
    private Button clearButton;


    public void OnClickInstantClearButton()
    {
        int currentTicketNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value;

        if (currentTicketNum < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.RelicTicket)}가 부족합니다.");
            return;
        }


        int clearAmount = currentTicketNum;

        int currentKillCount = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.relicKillCount].Value;

        if (currentKillCount == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup("소탕", $"{currentKillCount}점으로 {clearAmount}번 소탕합니까?", () =>
            {
                InstantClearReceive(currentKillCount, clearAmount);
            }, null);
        }
    }

    private void InstantClearReceive(float score, int clearAmount)
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value < clearAmount)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.RelicTicket)}가 부족합니다.");
            return;
        }

        clearButton.interactable = false;

        //티켓차감
        ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value -= clearAmount;
        ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value += (score * clearAmount);

        ServerData.userInfoTable.GetTableData(UserInfoTable.usedRelicTicketNum).Value += clearAmount;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
        goodsParam.Add(GoodsTable.Relic, ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value);

        Param userinfoParam = new Param();
        userinfoParam.Add(UserInfoTable.usedRelicTicketNum, ServerData.userInfoTable.GetTableData(UserInfoTable.usedRelicTicketNum).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));

        ServerData.SendTransaction(transactions,
         completeCallBack: () =>
         {
             clearButton.interactable = true;
         }
        , successCallBack: () =>
        {
            clearButton.interactable = true;

            LogManager.Instance.SendLogType("Relic", "c", $"score:{score} clear{clearAmount}");

            PopupManager.Instance.ShowConfirmPopup($"소탕", $"점수 : {score} 소탕 : {clearAmount}\n보상 {CommonString.GetItemName(Item_Type.Relic)} {score * clearAmount}개", null);
        });
    }

    public void OnClickAllResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "모든 능력치를 초기화 합니까?", () =>
         {
             float refundCount = 0;

             var tableDatas = TableManager.Instance.RelicTable.dataArray;

             List<TransactionValue> transactions = new List<TransactionValue>();

             Param relicParam = new Param();

             for (int i = 0; i < tableDatas.Length; i++)
             {
                 refundCount += ServerData.relicServerTable.TableDatas[tableDatas[i].Stringid].level.Value;
                 ServerData.relicServerTable.TableDatas[tableDatas[i].Stringid].level.Value = 0;

                 relicParam.Add(tableDatas[i].Stringid, ServerData.relicServerTable.TableDatas[tableDatas[i].Stringid].ConvertToString());
             }

             if (refundCount == 0)
             {
                 PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
                 return;
             }

             transactions.Add(TransactionValue.SetUpdate(RelicServerTable.tableName, RelicServerTable.Indate, relicParam));


             ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value += refundCount;

             Param goodsParam = new Param();
             goodsParam.Add(GoodsTable.Relic, ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value);

             transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

             ServerData.SendTransaction(transactions, successCallBack: () =>
               {
                   PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
                   LogManager.Instance.SendLogType("Relic", "초기화", $"{refundCount}개");
               });

         }, () => { });
    }

    public void ExChangeAbilityToKey()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"모든 능력치를 초기화 하고\n사용한 {CommonString.GetItemName(Item_Type.RelicTicket)}를 반환 합니까?", () =>
        {
            var tableDatas = TableManager.Instance.RelicTable.dataArray;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param relicParam = new Param();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                ServerData.relicServerTable.TableDatas[tableDatas[i].Stringid].level.Value = 0;

                relicParam.Add(tableDatas[i].Stringid, ServerData.relicServerTable.TableDatas[tableDatas[i].Stringid].ConvertToString());
            }

            transactions.Add(TransactionValue.SetUpdate(RelicServerTable.tableName, RelicServerTable.Indate, relicParam));

            ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value = 0;

            int usedTicketNum = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.usedRelicTicketNum).Value;
            int prefticketNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value;

            ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += usedTicketNum;

            ServerData.userInfoTable.GetTableData(UserInfoTable.usedRelicTicketNum).Value = 0;

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Relic, ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value);
            goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.usedRelicTicketNum, ServerData.userInfoTable.GetTableData(UserInfoTable.usedRelicTicketNum).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage($"초기화 성공!\n{CommonString.GetItemName(Item_Type.Ticket)} {usedTicketNum}개 획득!");
                LogManager.Instance.SendLogType("Relic", "반환", $"pref {prefticketNum} get {usedTicketNum}개");
            });

        }, () => { });
    }


    public void RenewalAbil()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"현재 점수에 맞게 영혼 조각을 갱신 하시겠습니까?", () =>
        {
            int currentKillCount = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.relicKillCount].Value;

            var tableDatas = TableManager.Instance.RelicTable.dataArray;

            float prefTotal = ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value;

            for (int i = 0; i < tableDatas.Length; i++)
            {
                prefTotal += ServerData.relicServerTable.TableDatas[tableDatas[i].Stringid].level.Value;
            }

            float newTotal = currentKillCount * (float)ServerData.userInfoTable.GetTableData(UserInfoTable.usedRelicTicketNum).Value;

            float interval = newTotal - prefTotal;

            if (interval <= 0f)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "갱신할 데이터가 없습니다.", null);
            }
            else
            {
                ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value += interval;
                ServerData.goodsTable.UpData(GoodsTable.Relic, false);
            }

        }, () => { });
    }

    private void OnDisable()
    {
        PlayerStats.ResetAbilDic();
    }
}
