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
        bestScoreText.SetText($"최고점수:{Utils.ConvertBigNum(ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.blackFoxScore].Value* GameBalance.BossScoreConvertToOrigin)}");


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

        var currentKillCount = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.blackFoxScore].Value;

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
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfoParam));

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
             int grade = (int)PlayerStats.GetBlackFoxGrade();

             if (grade < 0)
             {
                 PopupManager.Instance.ShowAlarmMessage("초기화할 데이터가 없습니다.");
                 return;
             }
             
             var tableDatas = TableManager.Instance.BlackFoxAbil.dataArray;

             var tableData2 = TableManager.Instance.BlackFoxTable.dataArray;

             float newTotal = tableData2[grade].Rewardvalue * (float)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.usedblackFoxClearNum).Value + AddPassReward();

             ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value = newTotal;

             List<TransactionValue> transactions = new List<TransactionValue>();

             Param relicParam = new Param();
             
             //구미호 레벨
             foreach (var t in tableDatas)
             {
                 ServerData.blackFoxServerTable.TableDatas[t.Stringid].level.Value = 0;
                 
                 relicParam.Add(t.Stringid, ServerData.blackFoxServerTable.TableDatas[t.Stringid].ConvertToString());
             }
             transactions.Add(TransactionValue.SetUpdate(BlackFoxServerTable.tableName, BlackFoxServerTable.Indate, relicParam));
             //
             //재화
             Param goodsParam = new Param();
             goodsParam.Add(GoodsTable.BlackFoxGoods, ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value);
             transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                //
             ServerData.SendTransaction(transactions, successCallBack: () =>
               {
                   PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
                   LogManager.Instance.SendLogType("BlackFox", "초기화", $"{newTotal}개");
               });

         }, () => { });
    }

    public void RenewalAbil()
    {
        
        int grade = (int)PlayerStats.GetBlackFoxGrade();

        if (grade < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("갱신할 데이터가 없습니다.");
            return;
        }
        
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"현재 점수에 맞게 {CommonString.GetItemName(Item_Type.BlackFoxGoods)}을 갱신 하시겠습니까?", () =>
        {
            var tableDatas = TableManager.Instance.BlackFoxAbil.dataArray;

            var tableData2 = TableManager.Instance.BlackFoxTable.dataArray;
            
            float prefTotal = ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value;

            for (int i = 0; i < tableDatas.Length; i++)
            {
                prefTotal += ServerData.blackFoxServerTable.TableDatas[tableDatas[i].Stringid].level.Value;
            }

            float newTotal = tableData2[grade].Rewardvalue * (float)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.usedblackFoxClearNum).Value + AddPassReward();

            float interval = newTotal - prefTotal;

            if (interval <= 0f)
            {
                PopupManager.Instance.ShowAlarmMessage("갱신할 데이터가 없습니다.");
            }
            else
            {
                ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value += interval;
                ServerData.goodsTable.UpDataV2(GoodsTable.BlackFoxGoods, false);
            }

        }, () => { });
    }

    private float AddPassReward()
    {
        var tableData3 = TableManager.Instance.BlackFoxPass.dataArray;
        var passIdx = int.Parse(ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.blackFoxFree].Value);

        var passAddSum = 0f;
        for (int i = 0; i <= passIdx; i++)
        {
            passAddSum += tableData3[i].Reward1_Value;
        }

        return passAddSum;
    }
}
