using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using UnityEngine.UI;

public class UiDayOfWeekDungeonBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI goodsText;


    [SerializeField]
    private Image rewardIcon0;
    [SerializeField]
    private Image rewardIcon1;

    public Button registerButton;


    public TextMeshProUGUI getButtonDesc;


    private void Start()
    {
        Subscribe();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private int GetDayOfweek()
    {
        var serverTime = ServerData.userInfoTable.currentServerTime;
        return (int)serverTime.DayOfWeek;
    }

    private void Subscribe()
    {
        string goodsKey = TableManager.Instance.dayOfWeekDungeon.dataArray[GetDayOfweek()].Rewardstring;
        if (ServerData.goodsTable.TableDatas.ContainsKey(goodsKey))
        {
            ServerData.goodsTable.GetTableData(goodsKey).AsObservable().Subscribe(goods =>
            {
                goodsText.SetText($"{Utils.ConvertBigNum(goods).ToString()}");
            }).AddTo(this);
        }

        ServerData.userInfoTable.TableDatas[UserInfoTable.getDayOfWeek].AsObservable().Subscribe(e =>
        {
            registerButton.interactable = e == 0;

            getButtonDesc.SetText(e == 0 ? "획득" : "오늘 획득함");
        }).AddTo(this);
    }


    private void Initialize()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.DayOfWeekClear].Value)}");

        var tabledata = TableManager.Instance.dayOfWeekDungeon.dataArray;

        rewardIcon0.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tabledata[GetDayOfweek()].Rewardtype);
        rewardIcon1.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tabledata[GetDayOfweek()].Rewardtype);

    }

    public void OnClickDokebiEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.DayOfWeekDungeon);
        }, () => { });
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value = 0;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.DayOfWeekClear).Value += 10;
        }
    }
#endif

    public void OnClickGetDayOfWeekButton()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"요일 보상은 하루에 한번만 획득 가능합니다!");
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.DayOfWeekClear].Value;

        if (score == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }


        var tabledata = TableManager.Instance.dayOfWeekDungeon.dataArray;


        float multipleValue = 0f;
        for (int i = 0; i < tabledata[GetDayOfweek()].Score.Length; i++)
        {
            //통과
            if (tabledata[GetDayOfweek()].Score[i] <= ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value)
            {
                multipleValue = tabledata[GetDayOfweek()].Rewardvalue[i];
            }
            //정지
            else
            {
                if (i == 0)
                {
                    multipleValue = 1f;
                }
                break;
            }
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score * multipleValue}개 획득 합니까?", () =>
        {
            if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage($"요일 보상은 하루에 한번만 획득 가능합니다!");
                return;
            }

            ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value = 1;
  

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getDayOfWeek, ServerData.userInfoTable.TableDatas[UserInfoTable.getDayOfWeek].Value);


            
            ServerData.goodsTable.GetTableData(tabledata[GetDayOfweek()].Rewardstring).Value += score * multipleValue;
            Param goodsParam = new Param();
            goodsParam.Add(tabledata[GetDayOfweek()].Rewardstring, ServerData.goodsTable.GetTableData(tabledata[GetDayOfweek()].Rewardstring).Value);
          

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            DailyMissionManager.UpdateDailyMission(DailyMissionKey.ClearBonusDungeon, 10);
            EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION1, 1);
            EventMissionManager.UpdateEventMissionClear(EventMissionKey.ANMISSION4, 1);

            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearBandit, 1);
            }
            else
            {
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearBandit, 1);
            }
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName((Item_Type)tabledata[GetDayOfweek()].Rewardtype)} {score * multipleValue}개 획득!", null);
            });
        }, null);
    }

}
