using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class DokebiEnterView : MonoBehaviour
{
    [SerializeField]
    private GameObject enterButton;

    [SerializeField]
    private TextMeshProUGUI enterCountText;

    [SerializeField]
    private RectTransform popupBg;

    [SerializeField]
    private List<Button> dokebiEnterButtons;

    [SerializeField]
    private TextMeshProUGUI oldDokebiScore;


    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).AsObservable().Subscribe(e =>
        {
            enterCountText.SetText(e < 1 ? "입장가능" : "소탕완료");
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.oldDokebi2LastClear).AsObservable().Subscribe(e => 
        {
            oldDokebiScore.SetText($"현재 클리어 층 : {e}");
        }).AddTo(this);

    }

    public void OnClickEnterButton(int idx)
    {
        // int currentEnterCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value;
        //
        // if (currentEnterCount > 0)
        // {
        //     PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 입장할 수 없습니다.");
        //     return;
        // }

        // dokebiEnterButtons.ForEach(e => e.interactable = false);
        //
        // ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value++;
        //
        // List<TransactionValue> transactionList = new List<TransactionValue>();
        //
        // Param userInfoParam = new Param();
        // userInfoParam.Add(UserInfoTable.dokebiNewEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value);
        // transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
      GameManager.Instance.SetDokebiId(idx);                                
      GameManager.Instance.LoadContents(GameManager.ContentsType.Dokebi);   

    }
    public void OnClickEnterOldDokebi2Button(int idx)
    {
        dokebiEnterButtons.ForEach(e => e.interactable = false);

        GameManager.Instance.LoadContents(GameManager.ContentsType.OldDokebi2);
    }

    public void OnClickOldDokebi2RewardButton()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiBundle).Value > 0)
        {
            //이미 받음
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"오늘 {CommonString.GetItemName(Item_Type.DokebiBundle)}를 이미 수령하였습니다!", null);
            return;
        }

        ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiBundle).Value = 1;

        int score = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.oldDokebi2LastClear).Value;

        ServerData.goodsTable.GetTableData(GoodsTable.DokebiBundle).Value += score;

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.getDokebiBundle, ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiBundle).Value);

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.DokebiBundle, ServerData.goodsTable.GetTableData(GoodsTable.DokebiBundle).Value);

        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));


        ServerData.SendTransaction(transactionList,
          successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.DokebiBundle)} {score} 개 획득!!", null);
          });
    }
    private void OnEnable()
    {
        enterButton.SetActive(false);

    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value = 0;
        }
    }
#endif

    public void OnClickInstantClearButton(int idx)
    {
        int currentEnterCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value;

        if (currentEnterCount > 0)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
            return;
        }


        int dokebiClear = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount3).Value;

        if (dokebiClear == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("플레이 데이터가 없습니다.");
            return;
        }
        
        var stageData = GameManager.Instance.CurrentStageData;
        var enemyTableData = TableManager.Instance.EnemyData[stageData.Monsterid1];
        var expAmount = dokebiClear * enemyTableData.Exp * GameBalance.dokebiExpPlusValue;
        expAmount += expAmount * PlayerStats.GetBaseExpPlusValue_BuffAllIgnored() * 1f;

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"도깨비전 <color=yellow>{dokebiClear}</color>단계로 소탕 합니까?\n경험치 획득량 : {Utils.ConvertBigNumForRewardCell(expAmount)}\n현재 스테이지에 비례해 경험치를 획득 합니다.\n모든 시간제 버프 효과는 적용되지 않습니다.", () =>
         {
             if ((int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value > 0)
             {
                 PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
                 return;
             }

             GuideMissionManager.UpdateGuideMissionClear(GuideMissionKey.ClearOni);
             
             GrowthManager.Instance.GetExpBySleep(expAmount);
             
             ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value ++;

             List<TransactionValue> transactions = new List<TransactionValue>();

             Param userInfoParam = new Param();

             userInfoParam.Add(UserInfoTable.dokebiNewEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value);

             transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
             
             EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION7, 1);
             if (ServerData.userInfoTable.IsMonthlyPass2() == false)
             {
                 EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearOni, 1);
             }
             else
             {
                 EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearOni, 1);
             }
             ServerData.SendTransaction(transactions, successCallBack: () =>
             {
                 PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"경험치{Utils.ConvertBigNumForRewardCell(expAmount)} 획득!", null);
                 //사운드
                 SoundManager.Instance.PlaySound("Reward");
                 //LogManager.Instance.SendLog("DokClear", $"{rewardNum}개 획득 {ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value}");
             });
         }, null);
    }

}
