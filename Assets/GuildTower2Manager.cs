using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using static UiRewardView;

public class GuildTower2Manager : ContentsManagerBase
{
    [SerializeField]
    private Transform enemySpawnPos;

    private ReactiveProperty<ObscuredInt> contentsState = new ReactiveProperty<ObscuredInt>((int)ContentsState.Fight);

    private List<Enemy> spawnedEnemyList = new List<Enemy>();

    //null 일때 클리어 못한거
    private List<RewardData> rewardDatas;

    [SerializeField]
    private UiHellTowerResult uiInfinityTowerResult;

    public UnityEvent retryFunc;

    public static string poolName;

    private new void Start()
    {
        base.Start();

        Subscribe();

        StartCoroutine(ContentsRoutine());
    }

    #region Security

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
        contentsState.Value.RandomizeCryptoKey();
    }

    #endregion

    //종료조건

    #region EndConditions

    private void EnemyDeadCallBack(Enemy enemy)
    {
        spawnedEnemyList.Remove(enemy);

        //전부 처치함
        if (spawnedEnemyList.Count == 0)
        {
            contentsState.Value = (int)ContentsState.Clear;
        }
    }

    private void WhenPlayerDead()
    {
        //클리어 됐을때 죽지 않게
        if (contentsState.Value != (int)ContentsState.Fight) return;

        //  UiLastContentsFunc.AutoInfiniteTower2 = false;

        contentsState.Value = (int)ContentsState.Dead;
    }

    protected override void TimerEnd()
    {
        //  UiLastContentsFunc.AutoInfiniteTower2 = false;
        base.TimerEnd();
        contentsState.Value = (int)ContentsState.TimerEnd;
    }

    #endregion

    private void Subscribe()
    {
        contentsState.AsObservable().Subscribe(WhenTowerModeStateChanged).AddTo(this);

        PlayerStatusController.Instance.whenPlayerDead.Subscribe(e => { WhenPlayerDead(); }).AddTo(this);
    }


    private void WhenTowerModeStateChanged(ObscuredInt state)
    {
        if (state == (int)ContentsState.Clear)
        {

            SetClear();
        }

        if (state != (int)ContentsState.Fight)
        {
            EndInfiniteTower();
        }
    }


    private void EndInfiniteTower()
    {
        //몹 꺼줌
        spawnedEnemyList.ForEach(e => e.gameObject.SetActive(false));

        //타이머 종료
        if (contentsState.Value != (int)ContentsState.TimerEnd)
        {
            StopTimer();
        }

        //보상팝업
        ShowResultPopup();
    }


    private void ShowResultPopup()
    {
        //클리어 팝업출력
        uiInfinityTowerResult.gameObject.SetActive(true);
        uiInfinityTowerResult.Initialize((ContentsState)(int)contentsState.Value, rewardDatas);
        //
    }

    private IEnumerator ContentsRoutine()
    {
        yield return null;

        SpawnEnemy();

        AutoManager.Instance.StartAutoWithDelay();
    }


    private void SpawnEnemy()
    {
        int stageId = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value;

        EnemyTableData spawnEnemyData = GetSpawnedEnemy(stageId);

        poolName = $"Enemy/GuildTower2/{spawnEnemyData.Prefabname}";

        var enemyObject = BattleObjectManager.Instance.GetItem(poolName) as Enemy;

        enemyObject.transform.SetParent(enemySpawnPos.transform);

        enemyObject.transform.localPosition = Vector3.zero;

        enemyObject.transform.localScale = Vector3.one * 1.3f;

        enemyObject.SetEnemyDeadCallBack(EnemyDeadCallBack);

        enemyObject.Initialize(spawnEnemyData, updateSubHpBar: true);

        spawnedEnemyList.Add(enemyObject);
        
    }

    private EnemyTableData GetSpawnedEnemy(int stageId)
    {
        stageId = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value;

        var tableData = TableManager.Instance.GuildTowerTable2.dataArray[stageId];

        EnemyTableData enemy = new EnemyTableData();

        enemy.Prefabname = "1-1";
        enemy.Attackpower = 0;
        enemy.Movespeed = 0;
        enemy.Hp = tableData.Hp;
        enemy.Knockbackpower = 0;
        enemy.Defense = (int)tableData.Defense;

        return enemy;
    }


    private void SetClear()
    {
        Param userinfo2Param = new Param();

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //단계상승
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value++;

        userinfo2Param.Add(UserInfoTable_2.guildTower2ClearIndex, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value);
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            
        });
    }
}