using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Globalization;
using System.Threading;
using Photon.Pun;

//
public class GameManager : SingletonMono<GameManager>
{
    public enum InitPlayerPortalPosit
    {
        Left,
        Right
    }

    public enum ContentsType
    {
        NormalField,
        FireFly,
        Boss,
        InfiniteTower,
        Dokebi,
        TwelveDungeon,
        YoguiSoGul,
        RelicDungeon,
        Son,
        Smith,
        InfiniteTower2,
        GumGi,
        FoxMask,
        Susano,
        Hell,
        HellRelic,
        GumGiSoul,
        HellWarMode,
        PartyRaid,
        ChunFlower,
        PartyRaid_Guild,
        DokebiFire,
        DokebiTower,
        Yum,
        Ok,
        Online_Tower,
        GradeTest,
        Do,
        Sasinsu,
        SumiFire,
        SumisanTower,
        SmithTree,
        SonClone,
        DayOfWeekDungeon,
        Online_Tower2,
        OldDokebi2,
        Sumi,
        GyungRockTower,
        NorigaeSoul,
        RoyalTombTower,
        SuhoAnimal,
        SinsuTower,
        GuildTower,
        Thief,
        GyungRockTower2,
        DarkTower,
        FoxTower,
        SealSwordTower,
        TestMonkey,
        TestSword,
        TestHell,
        TestChun,
        SealAwake,
        TaeguekTower,
        SinsunTower,
        Dark,
        GyungRockTower3,
        TestDo,
        RelicTest,
        DosulBoss,
        TestSumi,
        VisionTower,
        Sinsun,
        TestThief,
        HyunSangTower,
        TransTower,
        GyungRockTower4,
        MeditationTower,
        TestDark,
        TestSin,
        DosulAwake,
        Danjeon,
        ClosedTraining,
        DragonTower,
        GyungRockTower5,
        BlackFox,
        ByeolhoTower,
        BattleContest,
        GyungRockTower6,
        DragonPalaceTower,
        WeeklyBoss,
        MunhaTower,
        MunhaTower2,
        MurimTower,
        GyungRockTower7,
        OffLine_Tower,//십만동굴 솔플
        SpecialRequestBoss,
        GuildTower2, //황금전갈굴
        GyungRockTower8, //황금전갈굴
        
        YeonOkTower, //연옥타워
        ChunguTower, 
        Haetal, 
        Dimension, 
        TransJewelTower,
        ChunSangTower,
        GyungRockTower9,
    }
    
    public bool SpawnMagicStone => IsNormalField;
    public bool IsNormalField => contentsType == ContentsType.NormalField;
    public bool IsJumpBoss = false;

    public StageMapData CurrentStageData { get; private set; }
    public MapThemaInfo MapThemaInfo { get; private set; }

    private ReactiveProperty<int> currentMapIdx = new ReactiveProperty<int>();

    public static ContentsType contentsType { get; private set; }

    public ReactiveCommand whenSceneChanged = new ReactiveCommand();

    public ObscuredInt bossId { get; private set; }
    public ObscuredInt specialRequestBossId { get; private set; }
    public ObscuredInt suhoAnimalId { get; private set; }
    public ObscuredInt currentTowerScore { get; private set; }

    public ObscuredInt dokebiIdx { get; private set; }

    public ContentsType lastContentsType { get; private set; } = ContentsType.NormalField;
    public ContentsType lastContentsType2 { get; private set; } = ContentsType.NormalField;

    private bool firstInit = true;
    

    
    public void ResetLastContents()
    {
        lastContentsType = ContentsType.NormalField;
    }

    
    public void ResetLastContents2()
    {
        lastContentsType2 = ContentsType.NormalField;
    }

    public void SetBossId(int bossId)
    {
        this.bossId = bossId;

        RandomizeKey();
    }
    public void SetSpecialRequestBossId(int bossId)
    {
        this.specialRequestBossId = bossId;

        RandomizeKey();
    }

    public void SetSuhoAnimalBossId(int bossId)
    {
        this.suhoAnimalId = bossId;

        RandomizeKey();
    }

    public void SetDokebiId(int dokebiId)
    {
        this.dokebiIdx = dokebiId;
    }
    public void SetTowerScore(int count)
    {
        this.currentTowerScore = count;
    }

    private new void Awake()
    {
        base.Awake();
        SettingData.InitFirst();
        SetCultureInfo();
    }

    private void SetCultureInfo()
    {
        CultureInfo customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        Thread.CurrentThread.CurrentCulture = customCulture;
        Thread.CurrentThread.CurrentUICulture = customCulture;
        CultureInfo.DefaultThreadCurrentCulture = customCulture;
        CultureInfo.DefaultThreadCurrentUICulture = customCulture;
    }
    private void RandomizeKey()
    {
        this.bossId.RandomizeCryptoKey();
    }

    public void Initialize()
    {
        Subscribe();
        InitGame();
    }

    private void InitGame()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
    }

    private void SetFrameRate(int option)
    {
        Application.targetFrameRate = 30 + 15 * option;
#if UNITY_EDITOR
        Debug.LogError($"Frame changed {Application.targetFrameRate}");
#endif
    }

    private void Subscribe()
    {
        AutoManager.Instance.Subscribe();

        currentMapIdx.AsObservable().Subscribe(e =>
        {
            if (!firstInit)
            {
                ServerData.userInfoTable.UpData(UserInfoTable.LastMap, e, true);
            }
            else
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.LastMap).Value = ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value + 1;
                firstInit = false;
            }
        }).AddTo(this);

        SettingData.FrameRateOption.AsObservable().Subscribe(SetFrameRate).AddTo(this);

        if (ServerData.userInfoTable.TableDatas[UserInfoTable.hellWarScore].Value != 0)
        {
            RankManager.Instance.UpdateBoss_Score(ServerData.userInfoTable.TableDatas[UserInfoTable.hellWarScore].Value);
        }
    }

    private void ClearStage()
    {
        int lastIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.LastMap).Value;

        currentMapIdx.Value = Mathf.Max(lastIdx, 0);

        CurrentStageData = TableManager.Instance.StageMapData[currentMapIdx.Value];
        MapThemaInfo = Resources.Load<MapThemaInfo>($"MapThema/{CurrentStageData.Mapthema}");
        
        PopupManager.Instance.DestroyAllPopup();
    }

    public List<EnemyTableData> GetEnemyTableData()
    {
        List<EnemyTableData> enemyDatas = new List<EnemyTableData>();

        enemyDatas.Add(TableManager.Instance.EnemyData[CurrentStageData.Monsterid1]);
        enemyDatas.Add(TableManager.Instance.EnemyData[CurrentStageData.Monsterid2]);

        return enemyDatas;
    }

    public void LoadBackScene()
    {
        if (IsFirstScene() == false)
        {
            currentMapIdx.Value--;

            if (currentMapIdx.Value < 0)
            {
                currentMapIdx.Value = 0;
            }

            LoadNormalField();
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("첫번째 스테이지 입니다.");
        }
    }

    private bool CanMoveStage = true;

    public void LoadNextScene()
    {
        UiTutorialManager.Instance.SetClear(TutorialStep.GoNextStage);

        if (IsLastScene() == false && CanMoveStage)
        {
            CanMoveStage = false;

            currentMapIdx.Value++;

            LoadNormalField();
        }
    }

    public void JumpNextScene(int bossIdx)
    {
        if (IsLastScene() == false && CanMoveStage)
        {
            CanMoveStage = false;

            currentMapIdx.Value = bossIdx;

            LoadNormalField();
        }
    }

    public void MoveMapByIdx(int idx)
    {
        currentMapIdx.Value = idx;
        LoadNormalField();
    }

    private Coroutine internetConnectCheckRoutine;

    private bool guildInfoLoadComplete = false;

    public void LoadNormalField()
    {
        if (guildInfoLoadComplete == false)
        {
            GuildManager.Instance.LoadGuildInfo(false);
            guildInfoLoadComplete = true;
        }

        if (internetConnectCheckRoutine != null)
        {
            StopCoroutine(internetConnectCheckRoutine);
        }

        internetConnectCheckRoutine = StartCoroutine(checkInternetConnection((isConnected) =>
        {
            if (isConnected)
            {
                contentsType = ContentsType.NormalField;

                ClearStage();

                ChangeScene();
            }
            else
            {
                currentMapIdx.Value--;

                if (currentMapIdx.Value < 0)
                {
                    currentMapIdx.Value = 0;
                }

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "네트워크가 불안정 합니다.\n잠시 후에 다시 시도해주세요.", null);
            }

            CanMoveStage = true;
        }));
    }

    IEnumerator checkInternetConnection(Action<bool> action)
    {
        action(true);
        yield break;

        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    public void LoadContents(ContentsType type)
    {
        if (type == ContentsType.FireFly)
        {
            if (ServerData.userInfoTable.TableDatas[UserInfoTable.bonusDungeonEnterCount].Value >= GameBalance.bonusDungeonEnterCount)
            {
                PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 입장하실수 없습니다!");
                return;
            }

            DailyMissionManager.UpdateDailyMission(DailyMissionKey.ClearBonusDungeon, 1);

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
        }

        if (type != ContentsType.NormalField)
        {
            lastContentsType = type;
            SetLastContentsType2(type);
        }

        contentsType = type;


        ChangeScene();
    }

    public void SetLastContentsType2(ContentsType type)
    {
        lastContentsType2 = type;
    }

    private static bool firstLoad = true;

    private void ChangeScene()
    {
        IAPManager.Instance.ResetDisableCallbacks();

        if (UiDeadConfirmPopup.Instance != null)
        {
            Destroy(UiDeadConfirmPopup.Instance.gameObject);
        }

        PopupManager.Instance.SetChatBoardPopupManager();

        if (firstLoad)
        {
            firstLoad = false;
            PostManager.Instance.RefreshPost();
        }

        if (contentsType == ContentsType.NormalField)
        {
            if (currentMapIdx.Value != 0 && UiTutorialManager.Instance != null)
            {
                //   UiTutorialManager.Instance.SetClear(TutorialStep._1_MoveField);
            }

            if (lastContentsType == ContentsType.OffLine_Tower)
            {
                PhotonNetwork.Disconnect();
            }

        }

        // SaveManager.Instance.SyncDatasInQueue();

        MainTabButtons.ResetPopups();
        
        whenSceneChanged.Execute();

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public bool IsLastScene()
    {
        return currentMapIdx.Value == TableManager.Instance.StageMapData.Count - 1;
    }

    public bool IsFirstScene()
    {
        return currentMapIdx.Value == 0;
    }

    private void Start()
    {
        StartCoroutine(CheckNetworkState());
    }

    private WaitForSeconds delay = new WaitForSeconds(60f);

    private IEnumerator CheckNetworkState()
    {
        while (true)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                AutoManager.Instance.SetAuto(false);

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "네트워크 연결이 끊겼습니다.\n앱을 종료합니다.", confirmCallBack: () => { Application.Quit(); });
            }

            yield return delay;
        }
    }
}