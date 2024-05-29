using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;
using BackEnd;
using Random = UnityEngine.Random;

public class MapInfo : SingletonMono<MapInfo>
{
    public List<EnemySpawnPlatform> spawnPlatforms;
    private List<EnemyTableData> spawnEnemyData;
    private List<Enemy> spawnedEnemyList = new List<Enemy>();
    public List<Enemy> SpawnedEnemyList => spawnedEnemyList;
    private int maxEnemy = 0;

    [SerializeField]
    private PolygonCollider2D cameracollider;

    public ReactiveCommand whenSpawnedEnemyCountChanged = new ReactiveCommand();
    public ReactiveProperty<float> spawnGaugeValue = new ReactiveProperty<float>();

    public Dictionary<int, int> spawnedEnemyPlatforms = new Dictionary<int, int>();

    public ReactiveProperty<bool> canSpawnEnemy;

    public void SetCanSpawnEnemy(bool canSpawnEnemy)
    {
        this.canSpawnEnemy.Value = canSpawnEnemy;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        spawnPlatforms = GetComponentsInChildren<EnemySpawnPlatform>().ToList();
    }
#endif


    private new void Awake()
    {
        m_DontDestroy = false;
        Initialize();
        base.Awake();
        SetCameraCollider();
    }

    private bool IsEnemyEmpty()
    {
        using var e = spawnedEnemyPlatforms.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value > 0) return false;
        }

        return true;
    }

    private void SetCameraCollider()
    {
        var cameraConfiner = GameObject.FindObjectOfType<CinemachineConfiner>();
        cameraConfiner.m_BoundingShape2D = cameracollider;
    }
    private void Initialize()
    {
        SetEnemyData();
    }
    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());

        Subscribe();

        
        // UpdateStageRank();
    }

    private CompositeDisposable disposable = new CompositeDisposable();



    private Coroutine resetCoroutine;
    public void ResetEnemy()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(ResetStage());
    }
    private IEnumerator ResetStage()
    {
        yield return new WaitForSeconds(5f);
        //아직도 맥스면 초기화.
        if (IsEnemyMax())
        {
            //소환된 몹 전부 꺼버림
            var spawnedEnemies = new List<Enemy>(spawnedEnemyList);

            spawnedEnemies.ForEach(e => e.gameObject.SetActive(false));
        
            //몹생성 막음
            SetCanSpawnEnemy(false);

            yield return new WaitForSeconds(1f);
        
            Debug.LogError("재생성");
            PlayerStats.ResetAbilDic();
            SetCanSpawnEnemy(true);
        }
        else
        {
            yield return null;
        }


    }

    private void Subscribe()
    {
        //플레이어 사망시
        PlayerStatusController.Instance.whenPlayerDead.AsObservable().Subscribe(e =>
        {
            //자동사냥 종료
            AutoManager.Instance.SetAuto(false);

            //몹 스폰 중단
            StopAllCoroutines();

            //경험치 절반 감소
            GrowthManager.Instance.WhenPlayerDeadInNormalField();

            PopupManager.Instance.ShowDeadConfirmPopup(CommonString.Notice, "사망했습니다.", () =>
            {
                GameManager.Instance.LoadNormalField();
            });

        }).AddTo(this);
        
        //5분마다
        Observable.Interval(TimeSpan.FromSeconds(300f)).Subscribe(_ =>
        {
            //몬스터가 꽉차있으니까 리셋
            if (IsEnemyMax())
            {
                //Debug.LogError("리셋O");
                ResetEnemy();
            }
            else
            {
                //Debug.LogError("리셋X");
            }
        }).AddTo(this);

        UiStageNameIndicater.Instance.whenFieldBossTimerEnd.AsObservable().Subscribe(WhenFieldBossTimerEnd).AddTo(this);
    }

    private void WhenFieldBossTimerEnd(Unit unit)
    {
        UiStageNameIndicater.Instance.StopFieldBossTimer();
        DisableFieldBoss();
        PopupManager.Instance.ShowAlarmMessage("보스가 도망갔습니다!");
        UiAutoBoss.Instance.WhenToggleChanged(false);
    }

    private void SetEnemyData()
    {
        spawnEnemyData = GameManager.Instance.GetEnemyTableData();

        maxEnemy = spawnPlatforms.Count * GameManager.Instance.CurrentStageData.Spawnamountperplatform;
    }

    public Vector3 GetRandomPos()
    {
        return Vector3.zero;
        //return new Vector2(Random.Range(min.position.x, max.position.x), Random.Range(min.position.y, max.position.y));
    }

    private bool IsEnemyMax()
    {
        return spawnedEnemyList.Count >= maxEnemy;
    }


    private IEnumerator EnemySpawnRoutine()
    {
        if (spawnPlatforms == null || spawnPlatforms.Count == 0) yield break;

        //초기딜레이
        yield return new WaitForSeconds(2.0f);

        if (UiAutoRevive.autoRevive)
        {
            UiAutoRevive.autoRevive = false;
            AutoManager.Instance.SetAuto(true);
        }

        WaitForSeconds spawnDelay = new WaitForSeconds(GameManager.Instance.CurrentStageData.Spawndelay);

        //스폰 간격
        WaitForSeconds spawnInterval = new WaitForSeconds(GameBalance.spawnIntervalTime);
        while (true)
        {
            if (gaugeRoutine != null)
            {
                StopCoroutine(gaugeRoutine);
            }

            gaugeRoutine = StartCoroutine(UpdateGauge(GameManager.Instance.CurrentStageData.Spawndelay + GameBalance.spawnIntervalTime * GameBalance.spawnDivideNum));

            bool isEnemyEmpty = IsEnemyEmpty();


            int spawnNum = maxEnemy - spawnedEnemyList.Count + PlayerStats.GetAddSummonYogui();

            while (canSpawnEnemy.Value == false)
            {
                yield return null;
            }

            for (int i = 0; i < spawnNum; i++)
            {
                while (canSpawnEnemy.Value == false)
                {
                    yield return null;
                }

                SpawnEnemy(false, isEnemyEmpty);

                whenSpawnedEnemyCountChanged.Execute();

                if (i == spawnNum / GameBalance.spawnDivideNum)
                {
                    yield return spawnInterval;
                }
            }


            yield return spawnDelay;
        }
    }

    private int GetMinimalSpawnedPlatformIdx()
    {
        int platformId = 0;
        int currentEnemyCount = int.MaxValue;

        using var e = spawnedEnemyPlatforms.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value < currentEnemyCount)
            {
                currentEnemyCount = e.Current.Value;
                platformId = e.Current.Key;
            }

        }

        return platformId;
    }

    private void AddEnemyCountInPlatform(int platformId)
    {
        if (spawnedEnemyPlatforms.ContainsKey(platformId) == false)
        {
            spawnedEnemyPlatforms.Add(platformId, 0);
        }

        spawnedEnemyPlatforms[platformId]++;
    }

    private void RemoveEnemyCountInPlatform(int platformId)
    {
        if (spawnedEnemyPlatforms.ContainsKey(platformId))
        {
            spawnedEnemyPlatforms[platformId]--;
        }
    }

    //제일 하단 발판
    private int GetBossSpawnPlatformIdx()
    {
        int ret = 0;
        float yPos = float.MaxValue;

        for (int i = 0; i < spawnPlatforms.Count; i++)
        {
            if (spawnPlatforms[i].transform.position.y < yPos)
            {
                yPos = spawnPlatforms[i].transform.position.y;
                ret = i;
            }
        }

        return ret;
    }

    private void SpawnEnemy(bool isBossEnemy, bool isRandomTurn)
    {
        if (spawnEnemyData.Count == 0)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "데이터 없음", null);
            return;
        }

        var enemyObject = BattleObjectManager.Instance.GetItem($"Enemy/{spawnEnemyData[0].Prefabname}") as Enemy;
        int spawnedIdx = 0;

        if (isBossEnemy == false)
        {
            if (isRandomTurn)
            {
                spawnedIdx = Random.Range(0, spawnPlatforms.Count);
            }
            else
            {
                spawnedIdx = GetMinimalSpawnedPlatformIdx();
            }

            AddEnemyCountInPlatform(spawnedIdx);

            Vector3 spawnPos = spawnPlatforms[spawnedIdx].GetRandomSpawnPos();

            enemyObject.transform.position = spawnPos;
        }
        //보스소환
        else
        {
            UiStageNameIndicater.Instance.SerFieldBossTimerDefault();
            //UiStageNameIndicater.Instance.StartFieldBossTimer(15);

            PopupManager.Instance.ShowAlarmMessage("필드보스 출현!");

            //첫번째 발판에 소환
            Vector3 spawnPosition = spawnPlatforms[GetBossSpawnPlatformIdx()].GetRandomSpawnPos();
            enemyObject.transform.position = spawnPosition;

            //플레이어 이동기능
            PlayerSkillCaster.Instance.PlayerMoveController.transform.position = spawnPosition;
            
            
            EffectManager.SpawnEffectAllTime("FieldBossSpawn", enemyObject.transform.position);

            EffectManager.SpawnEffectAllTime("Circle1", enemyObject.transform.position);
            SoundManager.Instance.PlaySound("4-1");
        }


        enemyObject.SetReturnCallBack(EnemyRemoveCallBack);

        //10000스테이지 이상은 그냥 한방처리
        bool setHpOne = isBossEnemy == false && spawnEnemyData[0].Id >= 10000;
        
        enemyObject.Initialize(spawnEnemyData[0], isBossEnemy, spawnedIdx,setHpOne:setHpOne);

        spawnedEnemyList.Add(enemyObject);
    }

    private Coroutine gaugeRoutine;

    private IEnumerator UpdateGauge(float spawnDelay)
    {
        spawnGaugeValue.Value = 0f;

        while (spawnGaugeValue.Value < spawnDelay)
        {
            while (canSpawnEnemy.Value == false)
            {
                yield return null;
            }

            spawnGaugeValue.Value += Time.deltaTime;

            yield return null;
        }

        spawnGaugeValue.Value = spawnDelay;
    }

#if UNITY_EDITOR
    IEnumerator SpawnTimerRoutine_Text(float delay)
    {
        WaitForSeconds oneSecond = new WaitForSeconds(1);
        while (delay > 0)
        {
            Debug.LogError($"Enemy Spawn remain {delay}");
            yield return oneSecond;
            delay -= 1f;
        }
    }
#endif

    private void EnemyRemoveCallBack(Enemy enemy)
    {
        RemoveEnemyCountInPlatform(enemy.spawnedPlatformIdx);

        spawnedEnemyList.Remove(enemy);
        whenSpawnedEnemyCountChanged.Execute();
    }


    public void SpawnBossEnemy()
    {
        if (GameManager.contentsType != GameManager.ContentsType.NormalField)
        {
            PopupManager.Instance.ShowAlarmMessage("이곳에서는 소환할 수 없습니다.");
            return;
        }

        //소환된 몹 전부 꺼버림
        var spawnedEnemies = new List<Enemy>(spawnedEnemyList);

        spawnedEnemies.ForEach(e => e.gameObject.SetActive(false));
        //
        //몹생성 막음
        SetCanSpawnEnemy(false);
        SpawnEnemy(true, false);
    }

    // //jumping
    // private int bossIdx=0;
    // public void SpawnBossEnemy(int idx)
    // {
    //     bossIdx = idx;
    //     if (GameManager.contentsType != GameManager.ContentsType.NormalField)
    //     {
    //         PopupManager.Instance.ShowAlarmMessage("이곳에서는 소환할 수 없습니다.");
    //         return;
    //     }
    //
    //     //소환된 몹 전부 꺼버림
    //     var spawnedEnemies = new List<Enemy>(spawnedEnemyList);
    //
    //     spawnedEnemies.ForEach(e => e.gameObject.SetActive(false));
    //     //
    //     //몹생성 막음
    //     SetCanSpawnEnemy(false);
    //     SpawnEnemy(true, false, bossIdx);
    // }

    public bool HasSpawnedBossEnemy()
    {
        for (int i = 0; i < spawnedEnemyList.Count; i++)
        {
            if (spawnedEnemyList[i].isFieldBossEnemy == true)
            {
                return true;
            }
        }
        return false;
    }
    private void DisableFieldBoss()
    {
        for (int i = 0; i < spawnedEnemyList.Count; i++)
        {
            if (spawnedEnemyList[i].isFieldBossEnemy == true)
            {
                spawnedEnemyList[i].gameObject.SetActive(false);
                return;
            }
        }
    }

    public void SetFieldClear()
    {
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();

        if (GameManager.Instance.IsJumpBoss)
        {
            GameManager.Instance.IsJumpBoss = false;
            
            //보상지급(합산지급해야함)
            var preStage = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;
            float rewardSum = 0f;
            for (var i = preStage; i <= GameManager.Instance.CurrentStageData.Id; i++)
            {
                rewardSum += TableManager.Instance.StageMapData[i].Bossrewardvalue;
            }
        
#if UNITY_EDITOR
            Debug.LogError($"옥 합계 : {rewardSum}");
#endif
            ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardSum;
            goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
    
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
    
            Param stageParam = new Param();
            ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value = GameManager.Instance.CurrentStageData.Id;
            stageParam.Add(UserInfoTable.topClearStageId, ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value);
    
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, stageParam));
    
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                // LogManager.Instance.SendLog("스테이지클리어", GameManager.Instance.CurrentStageData.Id.ToString());
                //결과표시
                UiFieldBossRewardView.Instance.Initialize(rewardSum);
            });
    
            if (UiTutorialManager.Instance.HasClearFlag(TutorialStep.ClearBoss1) == false)
            {
                UiTutorialManager.Instance.SetClear(TutorialStep.ClearBoss1);
            }
            else
            {
                UiTutorialManager.Instance.SetClear(TutorialStep.ClearBoss2);
            }

        }
        else
        {
            //보상지급
            int rewardValue = GameManager.Instance.CurrentStageData.Bossrewardvalue;
            ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardValue;
            goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param stageParam = new Param();
            ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value = GameManager.Instance.CurrentStageData.Id;
            stageParam.Add(UserInfoTable.topClearStageId, ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, stageParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                // LogManager.Instance.SendLog("스테이지클리어", GameManager.Instance.CurrentStageData.Id.ToString());
                //결과표시
                UiFieldBossRewardView.Instance.Initialize(rewardValue);
            });

            if (UiTutorialManager.Instance.HasClearFlag(TutorialStep.ClearBoss1) == false)
            {
                UiTutorialManager.Instance.SetClear(TutorialStep.ClearBoss1);
            }
            else
            {
                UiTutorialManager.Instance.SetClear(TutorialStep.ClearBoss2);
            }
        }

        GameManager.Instance.ResetLastContents();
        PlayerStats.ResetAbilDic();
    }
    // public void SetFieldJumpClear()
    // {
    //     List<TransactionValue> transactions = new List<TransactionValue>();
    //
    //     Param goodsParam = new Param();
    //
    //     //보상지급(합산지급해야함)
    //     //int rewardCurrentValue = GameManager.Instance.CurrentStageData.Bossrewardvalue;
    //     float rewardSum = 0f;
    //     for (int i = GameManager.Instance.CurrentStageData.Id; i <= bossIdx; i++)
    //     {
    //         rewardSum += TableManager.Instance.StageMapData[i].Bossrewardvalue;
    //     }
    //     //var rewardValue = TableManager.Instance.StageMapData[bossIdx].Bossrewardvalue;
    //     
    //     #if UNITY_EDITOR
    //     Debug.LogError($"옥 합계 : {rewardSum}");
    //     #endif
    //     ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardSum;
    //     goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
    //
    //     transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
    //
    //     Param stageParam = new Param();
    //     ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value = bossIdx;
    //     stageParam.Add(UserInfoTable.topClearStageId, ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value);
    //
    //     transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, stageParam));
    //
    //     ServerData.SendTransaction(transactions, successCallBack: () =>
    //       {
    //           // LogManager.Instance.SendLog("스테이지클리어", GameManager.Instance.CurrentStageData.Id.ToString());
    //           //결과표시
    //           UiFieldBossRewardView.Instance.Initialize(rewardSum, true, bossIdx);
    //       });
    //
    //     if (UiTutorialManager.Instance.HasClearFlag(TutorialStep.ClearBoss1) == false)
    //     {
    //         UiTutorialManager.Instance.SetClear(TutorialStep.ClearBoss1);
    //     }
    //     else
    //     {
    //         UiTutorialManager.Instance.SetClear(TutorialStep.ClearBoss2);
    //     }
    //
    //
    //     GameManager.Instance.ResetLastContents();
    //     PlayerStats.ResetAbilDic();
    // }
}
