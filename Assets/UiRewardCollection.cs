using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiRewardCollection : MonoBehaviour
{
    [SerializeField]
    private Button oniButton;

    [SerializeField]
    private Button oniButton2;

    [SerializeField]
    private Button baekGuiButton;

    [SerializeField]
    private Button sonButton;

    [SerializeField]
    private Button gangChulButton;

    [SerializeField]
    private Button gumGiButton;

    [SerializeField]
    private Button hellFireButton;

    [SerializeField]
    private Button chunFlowerButton;

    [SerializeField]
    private Button hellRelicButton;

    [SerializeField]
    private Button dokebiClearButton;

    [SerializeField]
    private Button sumiClearButton;

    [SerializeField]
    private Button newGachaButton;

    [SerializeField]
    private GameObject Bandi0;
    [SerializeField]
    private GameObject Bandi1;
    
    [SerializeField]
    private GameObject SonObject;
    [SerializeField]
    private GameObject HelObject;
    [SerializeField]
    private GameObject ChunObject;
    [SerializeField]
    private GameObject DokebiObject;
    [SerializeField]
    private GameObject SumiObject;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(e =>
        {
            oniButton.interactable = e >= 300;
            baekGuiButton.interactable = e >= 3000;
            sonButton.interactable = e >= 5000;
            //gangChulButton.interactable = e >= 30000;
            gumGiButton.interactable = e >= 50000;
            hellFireButton.interactable = e >= 50000;
            chunFlowerButton.interactable = e >= 200000;
            hellRelicButton.interactable = e >= 50000;
            dokebiClearButton.interactable = e >= 500000;
            sumiClearButton.interactable = e >= 1000000;
            oniButton2.interactable = e >= 500000;


            if (e >= GameBalance.banditUpgradeLevel)
            {
                Bandi0.SetActive(false);
            }
            else
            {
                Bandi1.SetActive(false);
            }
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).AsObservable().Subscribe(e =>
        {
            newGachaButton.interactable = e >= 25000;
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).AsObservable().Subscribe(e =>
        {
            SonObject.SetActive(e == 0);
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).AsObservable().Subscribe(e =>
        {
            HelObject.SetActive(e == 0);
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).AsObservable().Subscribe(e =>
        {
            ChunObject.SetActive(e == 0);
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).AsObservable().Subscribe(e =>
        {
            DokebiObject.SetActive(e == 0);
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSumiFire).AsObservable().Subscribe(e =>
        {
            SumiObject.SetActive(e == 0);
        }).AddTo(this);
    }

    public void OnClickBanditReward(bool isPopup = true)
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value >=
            GameBalance.bonusDungeonEnterCount)
        {
            if (isPopup)
            {
                PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
            }

            return;
        }

        int killCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonMaxKillCount).Value;

        if (killCount == 0)
        {
            if (isPopup)
            {
                PopupManager.Instance.ShowAlarmMessage("기록이 없습니다.");
            }

            return;
        }

        int clearCount = GameBalance.bonusDungeonEnterCount -
                         (int)ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value;

        if (isPopup)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"처치 <color=yellow>{killCount}</color>로 <color=yellow>{clearCount}회</color> 소탕 합니까?\n{CommonString.GetItemName(Item_Type.Jade)} {killCount * GameBalance.bonusDungeonGemPerEnemy * (GameBalance.bandiPlusStageJadeValue * (int)Mathf.Floor(Mathf.Max(1000f, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value) + 2) / GameBalance.bandiPlusStageDevideValue) * clearCount}개\n{CommonString.GetItemName(Item_Type.Marble)} {killCount * GameBalance.bonusDungeonMarblePerEnemy * (GameBalance.bandiPlusStageMarbleValue * (int)Mathf.Floor(Mathf.Max(1000f, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value) + 2) / GameBalance.bandiPlusStageDevideValue) * clearCount}개",
                () =>
                {
                    // enterButton.interactable = false;
                    if (ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value >= GameBalance.bonusDungeonEnterCount)
                    {
                        PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
                        return;
                    }

                    int rewardNumJade = (killCount * GameBalance.bonusDungeonGemPerEnemy) *
                                        (GameBalance.bandiPlusStageJadeValue *
                                         (int)Mathf.Floor(Mathf.Max(1000f,
                                             (float)ServerData.userInfoTable
                                                 .GetTableData(UserInfoTable.topClearStageId).Value) + 2) /
                                         GameBalance.bandiPlusStageDevideValue) * clearCount;
                    int rewardNumMarble = killCount * GameBalance.bonusDungeonMarblePerEnemy *
                                          (GameBalance.bandiPlusStageMarbleValue *
                                           (int)Mathf.Floor(Mathf.Max(1000f,
                                               (float)ServerData.userInfoTable
                                                   .GetTableData(UserInfoTable.topClearStageId).Value) + 2) /
                                           GameBalance.bandiPlusStageDevideValue) * clearCount;
                    ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value += clearCount;

                    ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardNumJade;
                    ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += rewardNumMarble;

                    //데이터 싱크
                    List<TransactionValue> transactionList = new List<TransactionValue>();

                    Param goodsParam = new Param();
                    goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
                    goodsParam.Add(GoodsTable.MarbleKey,
                        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
                    transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate,
                        goodsParam));

                    Param userInfoParam = new Param();
                    userInfoParam.Add(UserInfoTable.bonusDungeonEnterCount,
                        ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value);
                    transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate,
                        userInfoParam));

                    ServerData.SendTransaction(transactionList,
                        successCallBack: () =>
                        {
                            DailyMissionManager.UpdateDailyMission(DailyMissionKey.ClearBonusDungeon, 1);
                        },
                        completeCallBack: () =>
                        {
                            // enterButton.interactable = true;
                        });

                    EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION1, clearCount);

                    if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                    {
                        EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearBandit, clearCount);
                    }
                    else
                    {
                        EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearBandit, clearCount);
                    }
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                        $"{clearCount}회 소탕 완료!\n{CommonString.GetItemName(Item_Type.Jade)} {rewardNumJade}개\n{CommonString.GetItemName(Item_Type.Marble)} {rewardNumMarble}개 획득!",
                        null);
                    SoundManager.Instance.PlaySound("GoldUse");


                }, null);
        }
        else
        {
                
                    if (ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value >= GameBalance.bonusDungeonEnterCount)
                    {
                        return;
                    }

                    int rewardNumJade = (killCount * GameBalance.bonusDungeonGemPerEnemy) *
                                        (GameBalance.bandiPlusStageJadeValue *
                                         (int)Mathf.Floor(Mathf.Max(1000f,
                                             (float)ServerData.userInfoTable
                                                 .GetTableData(UserInfoTable.topClearStageId).Value) + 2) /
                                         GameBalance.bandiPlusStageDevideValue) * clearCount;
                    int rewardNumMarble = killCount * GameBalance.bonusDungeonMarblePerEnemy *
                                          (GameBalance.bandiPlusStageMarbleValue *
                                           (int)Mathf.Floor(Mathf.Max(1000f,
                                               (float)ServerData.userInfoTable
                                                   .GetTableData(UserInfoTable.topClearStageId).Value) + 2) /
                                           GameBalance.bandiPlusStageDevideValue) * clearCount;
                    ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value += clearCount;

                    ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardNumJade;
                    ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += rewardNumMarble;

                    //데이터 싱크
                    List<TransactionValue> transactionList = new List<TransactionValue>();

                    Param goodsParam = new Param();
                    goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
                    goodsParam.Add(GoodsTable.MarbleKey,
                        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
                    transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate,
                        goodsParam));

                    Param userInfoParam = new Param();
                    userInfoParam.Add(UserInfoTable.bonusDungeonEnterCount,
                        ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value);
                    transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate,
                        userInfoParam));

                    ServerData.SendTransaction(transactionList,
                        successCallBack: () =>
                        {
                            DailyMissionManager.UpdateDailyMission(DailyMissionKey.ClearBonusDungeon, 1);
                        },
                        completeCallBack: () =>
                        {
                            // enterButton.interactable = true;
                        });

                    EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION1, clearCount);

                    if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                    {
                        EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearBandit, clearCount);
                    }
                    else
                    {
                        EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearBandit, clearCount);
                    }
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                        $"{clearCount}회 소탕 완료!\n{CommonString.GetItemName(Item_Type.Jade)} {rewardNumJade}개\n{CommonString.GetItemName(Item_Type.Marble)} {rewardNumMarble}개 획득!",
                        null);
                    SoundManager.Instance.PlaySound("GoldUse");

 
        }
    
}//★

    public void OnClickDayofWeekReward(bool isPopUp=true)
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value == 1)
        {
            if(isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage($"요일 보상은 하루에 한번만 획득 가능합니다!");
            }
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.DayOfWeekClear].Value;

        if (score == 0)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            }
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

        if (isPopUp)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score * multipleValue}개 획득 합니까?", () =>
            {
                if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value == 1)
                {
                    if (isPopUp)
                    {
                        PopupManager.Instance.ShowAlarmMessage($"요일 보상은 하루에 한번만 획득 가능합니다!");
                    }

                    return;
                }

                ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value = 1;


                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.getDayOfWeek,
                    ServerData.userInfoTable.TableDatas[UserInfoTable.getDayOfWeek].Value);



                ServerData.goodsTable.GetTableData(tabledata[GetDayOfweek()].Rewardstring).Value +=
                    score * multipleValue;
                Param goodsParam = new Param();
                goodsParam.Add(tabledata[GetDayOfweek()].Rewardstring,
                    ServerData.goodsTable.GetTableData(tabledata[GetDayOfweek()].Rewardstring).Value);


                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate,
                    userInfoParam));
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                DailyMissionManager.UpdateDailyMission(DailyMissionKey.ClearBonusDungeon, 10);

                EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION1, 1);

                if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearBandit, 1);
                }
                else
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearBandit, 1);
                }
                ServerData.SendTransaction(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"{CommonString.GetItemName((Item_Type)tabledata[GetDayOfweek()].Rewardtype)} {score * multipleValue}개 획득!",
                            null);
                    });
            }, null);
        }
        else
        {
                if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value == 1)
                {
                    return;
                }

                ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value = 1;


                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.getDayOfWeek,
                    ServerData.userInfoTable.TableDatas[UserInfoTable.getDayOfWeek].Value);



                ServerData.goodsTable.GetTableData(tabledata[GetDayOfweek()].Rewardstring).Value +=
                    score * multipleValue;
                Param goodsParam = new Param();
                goodsParam.Add(tabledata[GetDayOfweek()].Rewardstring,
                    ServerData.goodsTable.GetTableData(tabledata[GetDayOfweek()].Rewardstring).Value);


                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate,
                    userInfoParam));
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                DailyMissionManager.UpdateDailyMission(DailyMissionKey.ClearBonusDungeon, 10);

                EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION1, 1);

                if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearBandit, 1);
                }
                else
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearBandit, 1);

                }
                ServerData.SendTransaction(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"{CommonString.GetItemName((Item_Type)tabledata[GetDayOfweek()].Rewardtype)} {score * multipleValue}개 획득!(반딧불전)",
                            null);
                    });
        }
    }//★

    public void OnClickOniReward(bool isPopUp=true)
    {
        if (isPopUp)
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
                 if (currentEnterCount > 0)
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
                 EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION2, 1);
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
        else
        {
            int currentEnterCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value;

            if (currentEnterCount > 0)
            {
                //PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
                return;
            }


            int dokebiClear = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiKillCount3).Value;

            if (dokebiClear == 0)
            {
                //PopupManager.Instance.ShowAlarmMessage("플레이 데이터가 없습니다.");
                return;
            }

           
            if (currentEnterCount > 0)
            {
                 PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
                 return;
            }

            GuideMissionManager.UpdateGuideMissionClear(GuideMissionKey.ClearOni);

            var stageData = GameManager.Instance.CurrentStageData;
            var enemyTableData = TableManager.Instance.EnemyData[stageData.Monsterid1];
             
            var expAmount = dokebiClear * enemyTableData.Exp * GameBalance.dokebiExpPlusValue;
            expAmount += expAmount * PlayerStats.GetBaseExpPlusValue_BuffAllIgnored() * 1f;
            GrowthManager.Instance.GetExpBySleep(expAmount);

            ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value ++;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();

            userInfoParam.Add(UserInfoTable.dokebiNewEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION2, 1);
            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearOni, 1);
            }
            else{
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearOni, 1);
            }
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"경험치{Utils.ConvertBigNumForRewardCell(expAmount)} 획득!", null);
                //사운드
             SoundManager.Instance.PlaySound("Reward");
             //LogManager.Instance.SendLog("DokClear", $"{rewardNum}개 획득 {ServerData.goodsTable.GetTableData(GoodsTable.DokebiKey).Value}");
            });
        }
    }//★

    public void OnClickGumgiReward(bool isPopup=true)
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value == 1)
        {
            if (isPopup)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SP)}은 하루에 한번만 획득 가능합니다!");
            }
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value;

        if (score == 0)
        {
            if (isPopup)
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            }
            return;
        }

        if (isPopup)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>\n{CommonString.GetItemName(Item_Type.DokebiTreasure)}로 추가획득 : {Utils.GetDokebiTreasureAddValue()}", () =>
            {
                if (ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value == 1)
                {
                    
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SP)}은 하루에 한번만 획득 가능합니다!");
                    return;
                }

                ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value = 1;
                ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += score + Utils.GetDokebiTreasureAddValue();

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.getGumGi, ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value);

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));


                EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION4, 1);
                if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearSwordPartial, 1);
                }
                else
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearSwordPartial, 1);
                }
                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    LogManager.Instance.SendLogType("GumGi", "_", score.ToString());
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SP)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!", null);
                });
            }, null);
        }
        else
        {
            
                if (ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value == 1)
                {
                    return;
                }

                ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value = 1;
                ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value += score + Utils.GetDokebiTreasureAddValue();

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.getGumGi, ServerData.userInfoTable.TableDatas[UserInfoTable.getGumGi].Value);

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION4, 1);
                if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearSwordPartial, 1);
                }
                else
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearSwordPartial, 1);
                }
                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    LogManager.Instance.SendLogType("GumGi", "_", score.ToString());
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SP)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!", null);
                });
        }
    }

    public void OnClickHellFireReward()
    {

    }

    public void OnClickChunFlowerReward(bool isPopUp=true)
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).Value > 0)
        {
            //졸업했으면 못받게
            return;
        }
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value == 1)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Cw)}은 하루에 한번만 획득 가능합니다!");
            }
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.flowerClear].Value;

        if (score == 0)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            }
            return;
        }

        if (isPopUp)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>\n{CommonString.GetItemName(Item_Type.DokebiTreasure)}로 추가획득 : {Utils.GetDokebiTreasureAddValue()}", () =>
            {
                if (ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value == 1)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Cw)}은 하루에 한번만 획득 가능합니다!");
                    return;
                }

                ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value = 1;
                ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value += score + Utils.GetDokebiTreasureAddValue();

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.getFlower, ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value);

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION6, 1);
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearChunFlower, 1);
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearChunFlower, 1);

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Cw)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!", null);
                });
            }, null);
        }
        else
        {
            
                if (ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value == 1)
                {
                    return;
                }

                ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value = 1;
                ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value += score + Utils.GetDokebiTreasureAddValue();

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.getFlower, ServerData.userInfoTable.TableDatas[UserInfoTable.getFlower].Value);

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION6, 1);
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearChunFlower, 1);
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearChunFlower, 1);
                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.Cw)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!", null);
                });
        }
    }

    public void OnClickSmithReward(bool isPopUp=true)
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value == 1)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SmithFire)}은 하루에 한번만 획득 가능합니다!");
            }
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.smithClear].Value;

        if (score == 0)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            }
            return;
        }

        if (isPopUp)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"{score}개 획득 합니까?\n<color=red>(하루 한번만 획득 가능)</color>", () =>
                {
                    if (ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value == 1)
                    {
                        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SmithFire)}은 하루에 한번만 획득 가능합니다!");
                        return;
                    }

                    ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value = 1;
                    ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value += score;

                    List<TransactionValue> transactions = new List<TransactionValue>();

                    Param userInfoParam = new Param();
                    userInfoParam.Add(UserInfoTable.getSmith,
                        ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value);

                    Param goodsParam = new Param();
                    goodsParam.Add(GoodsTable.SmithFire,
                        ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);

                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate,
                        userInfoParam));
                    transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                    ServerData.SendTransaction(transactions, successCallBack: () =>
                    {
                        LogManager.Instance.SendLogType("Smith", "_", score.ToString());
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"{CommonString.GetItemName(Item_Type.SmithFire)} {score}개 획득!", null);
                    });
                }, null);
        }
        else
        {

                    if (ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value == 1)
                    {
                        if (isPopUp)
                        {
                            PopupManager.Instance.ShowAlarmMessage(
                                $"{CommonString.GetItemName(Item_Type.SmithFire)}은 하루에 한번만 획득 가능합니다!");
                        }

                        return;
                    }

                    ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value = 1;
                    ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value += score;

                    List<TransactionValue> transactions = new List<TransactionValue>();

                    Param userInfoParam = new Param();
                    userInfoParam.Add(UserInfoTable.getSmith,
                        ServerData.userInfoTable.TableDatas[UserInfoTable.getSmith].Value);

                    Param goodsParam = new Param();
                    goodsParam.Add(GoodsTable.SmithFire,
                        ServerData.goodsTable.GetTableData(GoodsTable.SmithFire).Value);

                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate,
                        userInfoParam));
                    transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                    ServerData.SendTransaction(transactions, successCallBack: () =>
                    {
                        LogManager.Instance.SendLogType("Smith", "_", score.ToString());
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"{CommonString.GetItemName(Item_Type.SmithFire)} {score}개 획득!", null);
                    });
        }
    }

    public void OnClickGuildGumihoReward()
    {

    }

    public void OnClickTrainingReward()
    {
        RewardPopupManager.Instance.OnclickButton();
    }

    public void OnClickDokebiReward(bool isPopUp=true)
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).Value > 0)
        {
            //졸업했으면 못받게
            return;
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value == 1)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DokebiFire)}은 하루에 한번만 획득 가능합니다!");
            }
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.DokebiFireClear].Value;

        if (score == 0)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            }

            return;
        }

        if (isPopUp)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"{score}개 획득 합니까?\n{CommonString.GetItemName(Item_Type.DokebiTreasure)}로 추가획득 : {Utils.GetDokebiTreasureAddValue()}",
                () =>
                {
                    if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value == 1)
                    {
                        PopupManager.Instance.ShowAlarmMessage(
                            $"{CommonString.GetItemName(Item_Type.DokebiFire)}은 하루에 한번만 획득 가능합니다!");
                        return;
                    }

                    ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value = 1;
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value +=
                        score + Utils.GetDokebiTreasureAddValue();

                    List<TransactionValue> transactions = new List<TransactionValue>();

                    Param userInfoParam = new Param();
                    userInfoParam.Add(UserInfoTable.getDokebiFire,
                        ServerData.userInfoTable.TableDatas[UserInfoTable.getDokebiFire].Value);

                    Param goodsParam = new Param();
                    goodsParam.Add(GoodsTable.DokebiFire,
                        ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);

                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate,
                        userInfoParam));
                    transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));


                    EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION7, 1);
                    if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                    {
                        EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearDokebiFire, 1);
                    }
                    else
                    {
                        EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearDokebiFire, 1);
                    }
                    ServerData.SendTransaction(transactions,
                        successCallBack: () =>
                        {
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                                $"{CommonString.GetItemName(Item_Type.DokebiFire)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!",
                                null);
                        });
                }, null);
        }
        else
        {
                    
                if (ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value == 1)
                {
                    return;
                }

                ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value = 1;
                ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value +=
                    score + Utils.GetDokebiTreasureAddValue();

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.getDokebiFire,
                    ServerData.userInfoTable.TableDatas[UserInfoTable.getDokebiFire].Value);

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.DokebiFire,
                    ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate,
                    userInfoParam));
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                //EventMissionManager.UpdateEventMissionClear(EventMissionKey.ClearDokebiFire, 1);
                EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION7, 1);
                if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearDokebiFire, 1);
                }
                else
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearDokebiFire, 1);
                }
                ServerData.SendTransaction(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"{CommonString.GetItemName(Item_Type.DokebiFire)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!",
                            null);
                    });
        }
    }

    public void OnClickSumiReward(bool isPopUp=true)
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSumiFire).Value > 0)
        {
            //졸업했으면 못받게
            return;
        }
        
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value == 1)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SumiFire)}은 하루에 한번만 획득 가능합니다!");
            }
            return;
        }

        int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.sumiFireClear].Value;

        if (score == 0)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            }
            return;
        }

        if (isPopUp)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score}개 획득 합니까?\n{CommonString.GetItemName(Item_Type.DokebiTreasure)}로 추가획득 : {Utils.GetDokebiTreasureAddValue()}", () =>
            {
                if (ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value == 1)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SumiFire)}은 하루에 한번만 획득 가능합니다!");
                    return;
                }

                ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value = 1;
                ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value += score + Utils.GetDokebiTreasureAddValue();

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.getSumiFire, ServerData.userInfoTable.TableDatas[UserInfoTable.getSumiFire].Value);

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.SumiFire, ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION8, 1);
                if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearSumiFire, 1);
                }
                else
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearSumiFire, 1);
                    
                }
                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SumiFire)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!", null);
                });
            }, null);
        }
        else
        {
                if (ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value == 1)
                {
                    return;
                }

                ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value = 1;
                ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value += score + Utils.GetDokebiTreasureAddValue();

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.getSumiFire, ServerData.userInfoTable.TableDatas[UserInfoTable.getSumiFire].Value);

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.SumiFire, ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION8, 1);

                if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearSumiFire, 1);
                }
                else
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearSumiFire, 1);
                }
                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.SumiFire)} {score + Utils.GetDokebiTreasureAddValue()}개 획득!", null);
                });
        }

    }

    private int GetDayOfweek()
    {
        var serverTime = ServerData.userInfoTable.currentServerTime;
        return (int)serverTime.DayOfWeek;
    }
    public void OnClickGetNewGachaButton(bool isPopUp=true)
    {

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).Value < 25000)
        {
            return;
        }
        
        
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value == 1)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.NewGachaEnergy)}은 하루에 한번만 획득 가능합니다!");
            }
            return;
        }

        //int amount = GameBalance.getRingGoodsAmount;
        int amount = GameBalance.getRingGoodsAmount * (int)Mathf.Floor(Mathf.Max(1, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).Value));

        if (amount == 0)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            }
            return;
        }

        if (isPopUp)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{amount}개 획득 합니까?", () =>
            {
            if (ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.NewGachaEnergy)}은 하루에 한번만 획득 가능합니다!");
                return;
            }

            ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += amount;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getRingGoods, ServerData.userInfoTable.TableDatas[UserInfoTable.getRingGoods].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION9, 1);
            
            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearSoulStone, 1);
            }
            else
            {
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearSoulStone, 1);
            }
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.NewGachaEnergy)} {amount}개 획득!", null);
            });
        }, null);     
        }
        else
        {
                if (ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value == 1)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.NewGachaEnergy)}은 하루에 한번만 획득 가능합니다!");
                    return;
                }
                ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value = 1;
                ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += amount;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.getRingGoods, ServerData.userInfoTable.TableDatas[UserInfoTable.getRingGoods].Value);

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);

                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION9, 1);
                
                if (ServerData.userInfoTable.IsMonthlyPass2() == false)
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearSoulStone, 1);
                }
                else
                {
                    EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearSoulStone, 1);
                }
                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.NewGachaEnergy)} {amount}개 획득!", null);
                });
        }
    }
    public void OnClickSonReceiveButton(bool isPopUp=true)
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).Value > 0)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("손오공을 각성하여 받을 수 없습니다.");
            }
            return;
        }
        double score = ServerData.userInfoTable.TableDatas[UserInfoTable.sonScore].Value * GameBalance.BossScoreConvertToOrigin;

        var tableData = TableManager.Instance.SonReward.dataArray;

        var sonRewardedIdxList = ServerData.etcServerTable.GetSonRewardedIdxList();

        int rewardCount = 0;

        string addStringValue = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score < tableData[i].Score)
            {
                break;
            }
            else
            {
                if (sonRewardedIdxList.Contains(tableData[i].Id) == false)
                {
                    float amount = tableData[i].Rewardvalue;

                    addStringValue += $"{BossServerTable.rewardSplit}{tableData[i].Id}";

                    ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value += (int)amount;

                    rewardCount++;
                }
            }
        }

        if (rewardCount > 0)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value += addStringValue;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            rewardParam.Add(EtcServerTable.sonReward, ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                // LogManager.Instance.SendLogType("Son", "all", "");
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상이 없습니다.");
            }
        }

    }

    public void OnClickBackGuiReceiveButton(bool isPopUp=true)
    {
        GuideMissionManager.UpdateGuideMissionClear(GuideMissionKey.ClearBackgui);

        int lastClearStageId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value;

        var tableDatas = TableManager.Instance.YoguisogulTable.dataArray;

        var GetYoguiSoguilRewardedList = ServerData.etcServerTable.GetYoguiSoguilRewardedList();

        int rewardReceiveCount = 0;

        string addStringValue = string.Empty;


        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Rewardtype == -1) continue;

            if (lastClearStageId < tableDatas[i].Stage) break;

            if (GetYoguiSoguilRewardedList.Contains(tableDatas[i].Stage) == true) continue;

            ServerData.AddLocalValue((Item_Type)tableDatas[i].Rewardtype, tableDatas[i].Rewardvalue);

            addStringValue += $"{BossServerTable.rewardSplit}{tableDatas[i].Stage}";

            rewardReceiveCount++;
        }


        if (rewardReceiveCount > 0)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value += addStringValue;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            rewardParam.Add(EtcServerTable.yoguiSogulReward, ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
            goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                // LogManager.Instance.SendLogType("Son", "all", "");
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상이 없습니다.");
            }
        }
    }
  public void OnClickOldDokebi2ReceiveButton(bool isPopUp=true)
    {

        int lastClearStageId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.oldDokebi2LastClear].Value;

        var tableDatas = TableManager.Instance.OldDokebiTable.dataArray;

        var GetOldDokebi2RewardedList = ServerData.etcServerTable.GetOldDokebi2RewardedList();

        int rewardReceiveCount = 0;

        string addStringValue = string.Empty;


        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Rewardtype == -1) continue;

            if (lastClearStageId < tableDatas[i].Stage) break;

            if (GetOldDokebi2RewardedList.Contains(tableDatas[i].Stage) == true) continue;

            ServerData.AddLocalValue((Item_Type)tableDatas[i].Rewardtype, tableDatas[i].Rewardvalue);

            addStringValue += $"{BossServerTable.rewardSplit}{tableDatas[i].Stage}";

            rewardReceiveCount++;
        }


        if (rewardReceiveCount > 0)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.oldDokebi2Reward].Value += addStringValue;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            rewardParam.Add(EtcServerTable.oldDokebi2Reward, ServerData.etcServerTable.TableDatas[EtcServerTable.oldDokebi2Reward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.DokebiBundle, ServerData.goodsTable.GetTableData(GoodsTable.DokebiBundle).Value);
            

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상이 없습니다.");
            }
        }
    }
    public void OnClickGangChulReceiveButton(bool isPopUp=true)
    {
        if (double.TryParse(ServerData.bossServerTable.TableDatas["boss20"].score.Value, out double score) == false)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("점수를 등록해주세요!");
            }
            return;
        }
        
        List<int> rewardTypeList = new List<int>();
        
        var gangchulRewardedIdxList = ServerData.bossServerTable.GetGangChulBossRewardedIdxList();

        int rewardCount = 0;

        string addStringValue = string.Empty;
        var twelveTableData = TableManager.Instance.TwelveBossTable.dataArray[20];
        for (int i = 0; i < twelveTableData.Rewardcut.Length; i++)
        {
            if(score< twelveTableData.Rewardcut[i])
            {
                break;
            }
            else
            {
                if(gangchulRewardedIdxList.Contains(i) ==false)
                {
                    
                    float amount = twelveTableData.Rewardvalue[i];

                    addStringValue += $"{BossServerTable.rewardSplit}{i}";

                    ServerData.goodsTable.GetTableData((Item_Type)twelveTableData.Rewardtype[i]).Value += (int)amount;
                    if(rewardTypeList.Contains(twelveTableData.Rewardtype[i])==false)
                    {
                        rewardTypeList.Add(twelveTableData.Rewardtype[i]);
                    }
                    rewardCount++;
                }
            }
        }

        if (rewardCount != 0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();
            ServerData.bossServerTable.TableDatas["boss20"].rewardedId.Value += addStringValue;

            Param bossParam = new Param();
            bossParam.Add("boss20", ServerData.bossServerTable.TableDatas["boss20"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

            var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while(e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("받을수 있는 보상이 없습니다.");
            }
        }
    }
    public void OnClickHelRelicReceiveButton(bool isPopUp=true)
    {
        if (double.TryParse(ServerData.bossServerTable.TableDatas["b53"].score.Value, out double score) == false)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("점수를 등록해주세요!");
            }
            return;
        }
        
        List<int> rewardTypeList = new List<int>();
        
        var hellRelicRewardedIdxList = ServerData.bossServerTable.GetHellRelicRewardedIdxList();

        int rewardCount = 0;

        string addStringValue = string.Empty;
        var twelveTableData = TableManager.Instance.TwelveBossTable.dataArray[53];
        for (int i = 0; i < twelveTableData.Rewardcut.Length; i++)
        {
            if(score< twelveTableData.Rewardcut[i])
            {
                break;
            }
            else
            {
                if(hellRelicRewardedIdxList.Contains(i) ==false)
                {
                    
                    float amount = twelveTableData.Rewardvalue[i];

                    addStringValue += $"{BossServerTable.rewardSplit}{i}";

                    ServerData.goodsTable.GetTableData((Item_Type)twelveTableData.Rewardtype[i]).Value += (int)amount;
                    if(rewardTypeList.Contains(twelveTableData.Rewardtype[i])==false)
                    {
                        rewardTypeList.Add(twelveTableData.Rewardtype[i]);
                    }
                    rewardCount++;
                }
            }
        }

        if (rewardCount != 0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();
            ServerData.bossServerTable.TableDatas["b53"].rewardedId.Value += addStringValue;

            Param bossParam = new Param();
            bossParam.Add("b53", ServerData.bossServerTable.TableDatas["b53"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

            var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while(e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("받을수 있는 보상이 없습니다.");
            }
        }
    }
     public void OnClickHellReceiveButton(bool isPopUp=true)
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).Value > 0)
        {
            //졸업했으면 못받게
            return;
        }
        double score = ServerData.userInfoTable.TableDatas[UserInfoTable.hellScore].Value *
                       GameBalance.BossScoreConvertToOrigin;

        var tableData = TableManager.Instance.hellReward.dataArray;

        var sonRewardedIdxList = ServerData.etcServerTable.GetHellRewardedIdxList();

        int rewardCount = 0;

        string addStringValue = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score < tableData[i].Score)
            {
                break;
            }
            else
            {
                if (sonRewardedIdxList.Contains(tableData[i].Id) == false)
                {
                    float amount = tableData[i].Rewardvalue;

                    addStringValue += $"{BossServerTable.rewardSplit}{tableData[i].Id}";

                    ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value += (int)amount;

                    rewardCount++;
                }
            }
        }

        if (rewardCount > 0)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.hellReward].Value += addStringValue;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            rewardParam.Add(EtcServerTable.hellReward,
                ServerData.etcServerTable.TableDatas[EtcServerTable.hellReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));


            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearHell, 1);
            }
            else
            {
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearHell, 1);
            }
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상이 없습니다.");
            }
        }

    }
    
    public void OnClickAllRewardReceive()
    {
        var lv = ServerData.statusTable.GetTableData(StatusTable.Level).Value;
        if (lv >= GameBalance.banditUpgradeLevel)
        {
            //대왕반딧불전 받기
            OnClickDayofWeekReward(false);
        }
        else
        {
            //반딧불전 받기
            OnClickBanditReward(false);
        }
        OnClickOniReward(false);
        OnClickBackGuiReceiveButton(false);
        OnClickSonReceiveButton(false);
        OnClickSmithReward(false);
        //OnClickGangChulReceiveButton(false);
        OnClickGetNewGachaButton(false);
        
        //오른쪽
        OnClickGumgiReward(false);
        OnClickHellReceiveButton(false);
        OnClickChunFlowerReward(false);
        OnClickHelRelicReceiveButton(false);
        OnClickDokebiReward(false);
        OnClickSumiReward(false);
        //OnClickOldDokebi2ReceiveButton(false);
        
        YorinSpecialMissionManager.UpdateMissionClear(YorinSpecialMissionKey.YSMission2_2, 1);

        PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상을 전부 받았습니다!");
    }
}
