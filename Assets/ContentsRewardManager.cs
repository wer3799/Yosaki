using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class ContentsRewardManager : SingletonMono<ContentsRewardManager>
{
    private List<ObscuredInt> bonusDefenseEnemyCut = new List<ObscuredInt>() { 1, 2, 3, 4, 5 };

    private void Start()
    {
        StartCoroutine(RandomizeRoutine());
    }

    public int GetDefenseReward_BlueStone(int enemyNum)
    {
        return enemyNum * GameBalance.bonusDungeonGemPerEnemy * (GameBalance.bandiPlusStageJadeValue * (int)Mathf.Floor(Mathf.Max(1000f, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value)+2) / GameBalance.bandiPlusStageDevideValue);
    }

    public int GetDefenseReward_Marble(int enemyNum)
    {
        return enemyNum * GameBalance.bonusDungeonMarblePerEnemy * (GameBalance.bandiPlusStageMarbleValue * (int)Mathf.Floor(Mathf.Max(1000f, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value)+2) / GameBalance.bandiPlusStageDevideValue);
    }
    private IEnumerator RandomizeRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(2.0f);

        while (true)
        {
            Randomize();
            yield return delay;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        Randomize();
    }

    private void Randomize()
    {
        bonusDefenseEnemyCut.ForEach(e => e.RandomizeCryptoKey());
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

}
