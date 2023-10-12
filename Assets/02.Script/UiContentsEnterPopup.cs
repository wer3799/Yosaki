using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UiContentsEnterPopup : SingletonMono<UiContentsEnterPopup>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI ticketPrice;

    [SerializeField]
    private GameObject bonusDungeonObject;

    [SerializeField]
    private GameObject infinityTowerObject;

    [SerializeField]
    private GameObject infinityTowerObject2;

    [SerializeField]
    private GameObject dokebiObject;

    [SerializeField]
    private GameObject twelveDungeonObject;

    public static ContentsType contentsType;

    public ObscuredInt bossId { get; private set; }

    [SerializeField]
    private Button enterButton;

    private void OnEnable()
    {
        StartCoroutine(RandomizeRoutine());
    }

    private IEnumerator RandomizeRoutine()
    {
        var delay = new WaitForSeconds(1.0f);

        while (true)
        {
            RandomizeKey();
            yield return delay;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        RandomizeKey();
    }

    private void RandomizeKey()
    {
        bossId.RandomizeCryptoKey();
    }
    public void Initialize(ContentsType type, int bossId)
    {
        this.transform.SetAsLastSibling();

        this.bossId = bossId;

        rootObject.SetActive(true);

        contentsType = type;

        UpdateUi();

        ticketPrice.SetText($"X{GameBalance.contentsEnterprice}");

        bonusDungeonObject.SetActive(type == ContentsType.FireFly);

        infinityTowerObject.SetActive(type == ContentsType.InfiniteTower);

        infinityTowerObject2.SetActive(type == ContentsType.InfiniteTower2);

        dokebiObject.SetActive(type == ContentsType.Dokebi);

        twelveDungeonObject.SetActive(type == ContentsType.TwelveDungeon);

        if (type == ContentsType.InfiniteTower && UiLastContentsFunc.AutoInfiniteTower)
        {
            if (ServerData.userInfoTable.IsLastFloor())
            {
                UiLastContentsFunc.AutoInfiniteTower = false;
                return;
            }

            OnClickEnterButton();
        }
        else if (type == ContentsType.InfiniteTower2 && UiLastContentsFunc.AutoInfiniteTower2)
        {
            if (ServerData.userInfoTable.IsLastFloor2())
            {
                UiLastContentsFunc.AutoInfiniteTower2 = false;
                return;
            }

            OnClickEnterButton();
        }

        //if (type == ContentsType.InfiniteTower2 && UiLastContentsFunc.AutoInfiniteTower2)
        //{
        //    if (ServerData.userInfoTable.TableDatas[UserInfoTable.currentFloorIdx2].Value >= TableManager.Instance.TowerTableData2.Count+1)
        //    {
        //        UiLastContentsFunc.AutoInfiniteTower2 = false;
        //        return;
        //    }

        //    OnClickEnterButton();
        //}
    }

    private void UpdateUi()
    {
        title.SetText($"{CommonString.GetContentsName(contentsType)}");
    }
    public void ClosePopup()
    {
        rootObject.SetActive(false);
    }
    public void OnClickEnterButton()
    {
        switch (contentsType)
        {
            case ContentsType.FireFly:
                {
                    BonusDefenseEnterRoutine();
                }
                break;
            case ContentsType.InfiniteTower:
                {
                    InfiniteTowerEnterRoutine();
                }
                break;
            case ContentsType.InfiniteTower2:
                {
                    InfiniteTowerEnterRoutine2();
                }
                break;
        }
    }
    private void BonusDefenseEnterRoutine()
    {
        float currentBlueStone = ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value;

        if (currentBlueStone < GameBalance.contentsEnterprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
            return;
        }

        int currentEnterCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value;
        if (currentEnterCount >= GameBalance.bonusDungeonEnterCount)
        {
            PopupManager.Instance.ShowAlarmMessage($"오늘은 더이상 입장할 수 없습니다.");
            return;
        }

        enterButton.interactable = false;

        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= GameBalance.contentsEnterprice;
        ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value++;

        //데이터 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.bonusDungeonEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList,
            successCallBack: () =>
            {
                GameManager.Instance.LoadContents(contentsType);
            },
            completeCallBack: () =>
            {
                enterButton.interactable = true;
            });
    }

    private void InfiniteTowerEnterRoutine()
    {
        float currentBlueStone = ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value;

        if (currentBlueStone < GameBalance.contentsEnterprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
            return;
        }

        enterButton.interactable = false;

        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= GameBalance.contentsEnterprice;

        ServerData.goodsTable.SyncToServerEach(GoodsTable.Jade, () =>
        {
            GameManager.Instance.LoadContents(contentsType);
        },
       () =>
       {
           enterButton.interactable = true;
       },
       //실패
       () =>
       {
           ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += GameBalance.contentsEnterprice;
           enterButton.interactable = true;
           PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "서버가 불안정 합니다. 잠시후 다시 시도해 주세요.", null);
       });
    }

    private void InfiniteTowerEnterRoutine2()
    {
        enterButton.interactable = false;

        GameManager.Instance.LoadContents(contentsType);
    }

    public void BonusDungeonInstantClear()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value >= GameBalance.bonusDungeonEnterCount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
            return;
        }

        int killCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonMaxKillCount).Value;

        if (killCount == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기록이 없습니다.");
            return;
        }

        int clearCount = GameBalance.bonusDungeonEnterCount - (int)ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value;

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"처치 <color=yellow>{killCount}</color>로 <color=yellow>{clearCount}회</color> 소탕 합니까?\n{CommonString.GetItemName(Item_Type.Jade)} {killCount * GameBalance.bonusDungeonGemPerEnemy * (GameBalance.bandiPlusStageJadeValue * Mathf.Max(1000f, (int)Mathf.Floor((float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value+2)) / GameBalance.bandiPlusStageDevideValue) * clearCount}개\n{CommonString.GetItemName(Item_Type.Marble)} {killCount * GameBalance.bonusDungeonMarblePerEnemy * clearCount * (GameBalance.bandiPlusStageMarbleValue * (int)Mathf.Floor(Mathf.Max(1000f, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value+2)) / GameBalance.bandiPlusStageDevideValue)}개", () =>
          {
              if (ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value >= GameBalance.bonusDungeonEnterCount)
              {
                  PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 소탕할 수 없습니다.");
                  return;
              }

              enterButton.interactable = false;

              int stageValue = (GameBalance.bandiPlusStageMarbleValue * Mathf.Max(1000,(int)Mathf.Floor((float)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value)+2) / GameBalance.bandiPlusStageDevideValue);

              int rewardNumJade = killCount * GameBalance.bonusDungeonGemPerEnemy * clearCount * stageValue;
              int rewardNumMarble = killCount * GameBalance.bonusDungeonMarblePerEnemy * clearCount * stageValue;
              ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value += clearCount;
              ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardNumJade;
              ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += rewardNumMarble;

            //데이터 싱크
            List<TransactionValue> transactionList = new List<TransactionValue>();

              Param goodsParam = new Param();
              goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
              goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
              transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

              Param userInfoParam = new Param();
              userInfoParam.Add(UserInfoTable.bonusDungeonEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value);
              transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

              EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION1, clearCount);
              if (ServerData.userInfoTable.IsMonthlyPass2() == false)
              {
                  EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearBandit, clearCount);
              }
              else
              {
                  EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearBandit, clearCount);
              }
              ServerData.SendTransaction(transactionList,
                  successCallBack: () =>
                  {
                      DailyMissionManager.UpdateDailyMission(DailyMissionKey.ClearBonusDungeon, 1);
                  },
                  completeCallBack: () =>
                  {
                      enterButton.interactable = true;
                  });

              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{clearCount}회 소탕 완료!\n{CommonString.GetItemName(Item_Type.Jade)} {Utils.ConvertNum(rewardNumJade)}개\n{CommonString.GetItemName(Item_Type.Marble)} {rewardNumMarble}개 획득!", null);
              SoundManager.Instance.PlaySound("GoldUse");


          }, null);
    }
}
