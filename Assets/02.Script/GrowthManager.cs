using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GrowthManager : SingletonMono<GrowthManager>
{
    public ReactiveProperty<float> maxExp = new ReactiveProperty<float>();

    private int levelUpSyncCount = 100;
    private static int currentSyncCount = 0;
    float expValue = 0f;

    private new void Awake()
    {
        base.Awake();
        Initialize();
    }

    private WaitForSeconds buffUpdateDelay = new WaitForSeconds(30.0f);

    private IEnumerator Start()
    {
        while (true)
        {
            SetBuffExpValue();
            yield return buffUpdateDelay;
        }
    }

    private void Initialize()
    {
        maxExp.Value = GameDataCalculator.GetMaxExp(ServerData.statusTable.GetTableData(StatusTable.Level).Value);
    }

    private void SetBuffExpValue()
    {
        expValue = PlayerStats.GetExpPlusValue_WithAllBuff();
    }


    private bool useEffect = true;

    public void GetExpBySleep(float exp)
    {
        ServerData.growthTable.GetTableData(GrowthTable.Exp).Value += exp;

        var maxLevelUpCount = 100000;

        this.useEffect = false;

        for (int i = 0; i < maxLevelUpCount; i++)
        {
            if (CanLevelUp())
            {
                accumLevel++;
                ServerData.growthTable.GetTableData(GrowthTable.Exp).Value -= maxExp.Value;
                maxExp.Value = GameDataCalculator.GetMaxExp(ServerData.statusTable.GetTableData(StatusTable.Level).Value + accumLevel);
            }
            else
            {
                break;
            }
        }

        UpdateLocalByAccum();

        SyncLevelUpDatas();

        UiExpGauge.Instance.WhenGrowthValueChanged();
    }

    public void GetExp(float exp, bool useEffect = true, bool syncToServer = true, bool isSleep = false)
    {
        if (isSleep) 
        {
            if (accumLevel > 50000)
            {
                return;
            }
        }
        else 
        {
            if (accumLevel > 1234)
            {
                return;
            }
        }

      
            exp += exp * expValue;



        this.useEffect = useEffect;

        if (syncToServer)
        {
            //SystemMessage.Instance.SetMessage($"경험치 획득 ({(int)exp})");
        }

        ServerData.growthTable.GetTableData(GrowthTable.Exp).Value += exp;

        if (CanLevelUp())
        {
            ServerData.growthTable.GetTableData(GrowthTable.Exp).Value -= maxExp.Value;

            levelUp();

            //추가레벨업 가능?
            if (CanLevelUp())
            {
                GetExp(0, useEffect: false, syncToServer: syncToServer, isSleep: isSleep);
            }
            else
            {
                if (isSleep)
                {
                    SyncLevelUpDatas();
                }
                else
                {
                    if (syncToServer)
                    {
                    
                            currentSyncCount++;

                            if (currentSyncCount >= levelUpSyncCount)
                            {
                                SyncLevelUpDatas();
                                currentSyncCount = 0;
                            }    
                        
                    }
                }
            }
        }
  
        if (syncToServer)
        {
            UiExpGauge.Instance.WhenGrowthValueChanged();
        }

    }

    private bool CanLevelUp()
    {
        return ServerData.growthTable.GetTableData(GrowthTable.Exp).Value >= maxExp.Value;
    }

    int accumLevel = 0;
    private WaitForSeconds delay = new WaitForSeconds(0.1f);
    private Coroutine syncRoutine;
    private IEnumerator SyncRoutine()
    {
        accumLevel++;

        ShowContentsUnlockAlarm(accumLevel);

        yield return delay;

        UpdateLocalByAccum();
    }

    public void UpdateLocalByAccum()
    {
        //레벨 증가
        ServerData.statusTable.GetTableData(StatusTable.Level).Value += accumLevel;

        //스킬포인트 증가
        ServerData.statusTable.GetTableData(StatusTable.SkillPoint).Value += accumLevel * GameBalance.SkillPointGet;

        //스탯포인트 증가
        ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value += accumLevel * GameBalance.StatPoint;

        //스핀포인트 증가
        ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value += accumLevel * GameBalance.levelUpSpinGet;

        //일일미션
        DailyMissionManager.UpdateDailyMission(DailyMissionKey.LevelUp, accumLevel);

        accumLevel = 0;
    }
    public void levelUp()
    {
        UpdateLocalData();

        //최대경험치 갱신
        maxExp.Value = GameDataCalculator.GetMaxExp(ServerData.statusTable.GetTableData(StatusTable.Level).Value + accumLevel);

        //레벨업 이벤트

        if (useEffect)
        {
            EffectManager.SpawnEffectAllTime("LevelUp", PlayerMoveController.Instance.transform.position, PlayerMoveController.Instance.transform);
            SoundManager.Instance.PlaySound("Reward");
        }
    }

    private void UpdateLocalData()
    {
        if (syncRoutine != null)
        {
            StopCoroutine(syncRoutine);
        }

        syncRoutine = StartCoroutine(SyncRoutine());
    }

    private void ShowContentsUnlockAlarm(int accumLevel)
    {

        var currentLevel = ServerData.statusTable.GetTableData(StatusTable.Level).Value + accumLevel;

        if (currentLevel == GameBalance.bonusDungeonUnlockLevel)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"({CommonString.ContentsName_FireFly}) 해금됐습니다!\n전투 탭에서 도전 가능 합니다.", null);
        }
        else if (currentLevel == GameBalance.InfinityDungeonUnlockLevel)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"({CommonString.ContentsName_InfinityTower}) 해금됐습니다!\n전투 탭에서 도전 가능 합니다.", null);
        }
        else if (currentLevel == GameBalance.bossUnlockLevel)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"({CommonString.ContentsName_Boss}) 해금됐습니다!\n전투 탭에서 도전 가능 합니다.", null);
        }
    }

    public void SyncLevelUpDatas()
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param statusParam = new Param();
        //레벨
        statusParam.Add(StatusTable.Level, ServerData.statusTable.GetTableData(StatusTable.Level).Value);

        //스킬포인트
        statusParam.Add(StatusTable.SkillPoint, ServerData.statusTable.GetTableData(StatusTable.SkillPoint).Value);

        //스탯포인트
        statusParam.Add(StatusTable.StatPoint, ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value);

        Param growthParam = new Param();
        growthParam.Add(GrowthTable.Exp, ServerData.growthTable.GetTableData(GrowthTable.Exp).Value);

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.BonusSpinKey, ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value);

        Param userinfoParam = new Param();
        userinfoParam.Add(UserInfoTable.LastLogin, Math.Truncate(ServerData.userInfoTable.TableDatas[UserInfoTable.LastLogin].Value));

        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        transactionList.Add(TransactionValue.SetUpdate(GrowthTable.tableName, GrowthTable.Indate, growthParam));
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
          {
#if UNITY_EDITOR
              Debug.Log("levelUp");
#endif
              ServerData.userInfoTable.UpdateLastLoginTime();

          });
    }

    public void WhenPlayerDeadInNormalField()
    {
        //경험치 절반 감소 안시킴
        return;
        ServerData.growthTable.GetTableData(GrowthTable.Exp).Value = ServerData.growthTable.GetTableData(GrowthTable.Exp).Value * 0.5f;
        ServerData.growthTable.UpData(GrowthTable.Exp, false);
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ServerData.statusTable.GetTableData(StatusTable.Level).Value += 1000;
        }
    }
#endif
}
