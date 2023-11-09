using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiBlackFoxBoard : MonoBehaviour
{
    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private UiBlackFoxCell uiBlackFoxCell;

    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    [SerializeField]
    private TextMeshProUGUI enterButtonText;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI unlockDesc;
    [SerializeField]
    private GameObject equipFrame;
    private int currentIdx;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.BlackFoxAbil.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var cell = Instantiate<UiBlackFoxCell>(uiBlackFoxCell, cellParents);

            cell.Initialize(tableDatas[i]);
        }
        bestScoreText.SetText($"최고점수:{Utils.ConvertBigNum(ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.blackFoxScore].Value* GameBalance.BossScoreConvertToOrigin)}");


        currentIdx = PlayerStats.GetBlackFoxGrade();
        
        SetText(currentIdx);
    }

    private void SetText(int idx)
    {
        if (idx == -1) idx = 0;

        var tableData = TableManager.Instance.BlackFoxTable.dataArray[idx];
        unlockDesc.SetText($"{Utils.ConvertBigNumForRewardCell(tableData.Score)}");

        equipFrame.gameObject.SetActive(idx == PlayerStats.GetBlackFoxGrade());

        gradeText.SetText($"{idx + 1}단계");
        

    }
    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.BlackFoxTable.dataArray.Length - 1);

        SetText(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.BlackFoxTable.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.BlackFoxTable.dataArray.Length - 1);

        SetText(currentIdx);

    }
    public void OnClickEnterButton()
    {

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.BlackFox);
        }, () => { });   
    

    }
#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value += 100000;
        }
    }

#endif

    //소탕

    [SerializeField]
    private Button clearButton;


    public void OnClickInstantClearButton()
    {
        int currentTicketNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxClear).Value;

        if (currentTicketNum < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.BlackFoxClear)}이(가) 부족합니다.");
            return;
        }


        int clearAmount = currentTicketNum;

        var currentKillCount = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.blackFoxScore].Value;

        if (currentKillCount == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup("소탕", $"{PlayerStats.GetBlackFoxGrade()+1}단계로 {clearAmount}번 소탕합니까?", () =>
            {
                InstantClearReceive(PlayerStats.GetBlackFoxGrade(), clearAmount);
            }, null);
        }
    }

    private void InstantClearReceive(int grade, int clearAmount)
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxClear).Value < clearAmount)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.BlackFoxClear)}이(가) 부족합니다.");
            return;
        }

        clearButton.interactable = false;


        var data = TableManager.Instance.BlackFoxTable.dataArray[grade];
        //티켓차감
        ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxClear).Value -= clearAmount;
        ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value += (data.Rewardvalue * clearAmount);

        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.usedblackFoxClearNum).Value += clearAmount;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.BlackFoxClear, ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxClear).Value);
        goodsParam.Add(GoodsTable.BlackFoxGoods, ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value);

        Param userinfoParam = new Param();
        userinfoParam.Add(UserInfoTable_2.usedblackFoxClearNum, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.usedblackFoxClearNum).Value);

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

            LogManager.Instance.SendLogType("BlackFox", "c", $"grade:{grade+1} clear{clearAmount}");

            PopupManager.Instance.ShowConfirmPopup($"소탕", $"단계 : {grade+1} 소탕 : {clearAmount}\n보상 {CommonString.GetItemName(Item_Type.BlackFoxGoods)} {data.Rewardvalue * clearAmount}개", null);
        });
    }

    public void OnClickAllResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "모든 능력치를 초기화 합니까?", () =>
         {
             float refundCount = 0;

             var tableDatas = TableManager.Instance.BlackFoxAbil.dataArray;

             List<TransactionValue> transactions = new List<TransactionValue>();

             Param relicParam = new Param();

             for (int i = 0; i < tableDatas.Length; i++)
             {
                 refundCount += ServerData.blackFoxServerTable.TableDatas[tableDatas[i].Stringid].level.Value;
                 ServerData.blackFoxServerTable.TableDatas[tableDatas[i].Stringid].level.Value = 0;

                 relicParam.Add(tableDatas[i].Stringid, ServerData.blackFoxServerTable.TableDatas[tableDatas[i].Stringid].ConvertToString());
             }

             if (refundCount == 0)
             {
                 PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
                 return;
             }

             transactions.Add(TransactionValue.SetUpdate(BlackFoxServerTable.tableName, BlackFoxServerTable.Indate, relicParam));


             ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value += refundCount;

             Param goodsParam = new Param();
             goodsParam.Add(GoodsTable.BlackFoxGoods, ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value);

             transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

             ServerData.SendTransaction(transactions, successCallBack: () =>
               {
                   PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
                   LogManager.Instance.SendLogType("BlackFox", "초기화", $"{refundCount}개");
               });

         }, () => { });
    }

    public void ExChangeAbilityToKey()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"모든 능력치를 초기화 하고\n사용한 {CommonString.GetItemName(Item_Type.BlackFoxClear)}를 반환 합니까?", () =>
        {
            var tableDatas = TableManager.Instance.BlackFoxAbil.dataArray;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param relicParam = new Param();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                ServerData.blackFoxServerTable.TableDatas[tableDatas[i].Stringid].level.Value = 0;

                relicParam.Add(tableDatas[i].Stringid, ServerData.blackFoxServerTable.TableDatas[tableDatas[i].Stringid].ConvertToString());
            }

            transactions.Add(TransactionValue.SetUpdate(BlackFoxServerTable.tableName, BlackFoxServerTable.Indate, relicParam));

            ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value = 0;

            int usedTicketNum = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.usedblackFoxClearNum).Value;
            int prefticketNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxClear).Value;

            ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxClear).Value += usedTicketNum;

            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.usedblackFoxClearNum).Value = 0;

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.BlackFoxGoods, ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value);
            goodsParam.Add(GoodsTable.BlackFoxClear, ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxClear).Value);

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable_2.usedblackFoxClearNum, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.usedblackFoxClearNum).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage($"초기화 성공!\n{CommonString.GetItemName(Item_Type.Ticket)} {usedTicketNum}개 획득!");
                LogManager.Instance.SendLogType("BlackFox", "반환", $"pref {prefticketNum} get {usedTicketNum}개");
            });

        }, () => { });
    }


    public void RenewalAbil()
    {
        
        var currentKillCount = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.blackFoxScore].Value;

        if (currentKillCount == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }
        
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"현재 점수에 맞게 {CommonString.GetItemName(Item_Type.BlackFoxGoods)}을 갱신 하시겠습니까?", () =>
        {
            int grade = (int)PlayerStats.GetBlackFoxGrade();

            var tableDatas = TableManager.Instance.BlackFoxAbil.dataArray;

            var tableData2 = TableManager.Instance.BlackFoxTable.dataArray;
            float prefTotal = ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value;

            for (int i = 0; i < tableDatas.Length; i++)
            {
                prefTotal += ServerData.blackFoxServerTable.TableDatas[tableDatas[i].Stringid].level.Value;
            }

            float newTotal = tableData2[grade].Rewardvalue * (float)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.usedblackFoxClearNum).Value;

            float interval = newTotal - prefTotal;

            if (interval <= 0f)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "갱신할 데이터가 없습니다.", null);
            }
            else
            {
                ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value += interval;
                ServerData.goodsTable.UpDataV2(GoodsTable.BlackFoxGoods, false);
            }

        }, () => { });
    }
}
