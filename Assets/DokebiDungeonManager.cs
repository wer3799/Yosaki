using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;
using UniRx;

public class DokebiDungeonManager : ContentsManagerBase
{
    [SerializeField]
    private List<Transform> spawnPoints;

    private ReactiveProperty<ObscuredInt> enemyDeadCount = new ReactiveProperty<ObscuredInt>();

    [SerializeField]
    private TextMeshProUGUI enemyKillCount;

    [SerializeField]
    private UiDokebiResultPopup resultPopup;

    private Coroutine spawnRoutine;

    private ObscuredInt currentSpawnedNum = 0;

    public static string poolName = "Enemy/Dokebi0";

    private ObscuredInt maxEnemySpawnNum = 5;

    private ObscuredInt spawnNum = 1;

    protected new void Start()
    {
        base.Start();

        spawnRoutine = StartCoroutine(EnemySpawnRoutine());

        Subscribe();

        UiTutorialManager.Instance.SetClear(TutorialStep.PlayFireFly);
    }

    private void Subscribe()
    {
        enemyDeadCount.AsObservable().Subscribe(e => { enemyKillCount.SetText($"{e}처치"); }).AddTo(this);
    }

    protected override void TimerEnd()
    {
        EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION7, 1);
        EventMissionManager.UpdateEventMissionClear(EventMissionKey.ANMISSION5, 1);

        if (ServerData.userInfoTable.IsMonthlyPass2() == false)
        {
            EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearOni, 1);
        }
        else
        {
            EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearOni, 1);
        }
        base.TimerEnd();

        StopCoroutine(spawnRoutine);

        GuideMissionManager.UpdateGuideMissionClear(GuideMissionKey.ClearOni);

        //580미만은 600으로 고정 ->기기별 데미지 차이
        if (enemyDeadCount.Value >= 500)
        {
            enemyDeadCount.Value = 600;
        }

        resultPopup.Initialize(enemyDeadCount.Value);

        resultPopup.gameObject.SetActive(true);

        BattleObjectManager.Instance.PoolContainer[poolName].DisableAllObject();
    }

    private IEnumerator EnemySpawnRoutine()
    {
        // WaitForSeconds interval = new WaitForSeconds(0.5f);
        //
        // yield return new WaitForSeconds(1.5f);
        WaitForSeconds interval = new WaitForSeconds(0.08f);

        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            int enemySpawnNum = maxEnemySpawnNum - currentSpawnedNum;

            for (int i = 0; i < enemySpawnNum; i++)
            {
                SpawnEnemy();
            }

            yield return interval;
        }
    }

    private void SpawnEnemy()
    {
        currentSpawnedNum++;
        spawnNum++;

        int directionRand = Random.Range(0, 2);
        Vector3 moveDir = Vector3.zero;
        Vector3 spawnPos = Vector3.zero;

        int randIdx = Random.Range(0, spawnPoints.Count);
        spawnPos = spawnPoints[randIdx].transform.position;

        var enemyPrefab = BattleObjectManager.Instance.GetItem(poolName);

        var enemy = enemyPrefab.GetComponent<DokebiEnemy>();

        enemy.Initialize(GetEnemyHp(), GetMoveSpeed(), GetEnemyDefense(), WhenEnemyDead);

        enemy.transform.position = spawnPos;
    }

    public double GetEnemyHp()
    {
        int enemyTableIdx = spawnNum * 22;

        enemyTableIdx = Mathf.Clamp(enemyTableIdx, 100, TableManager.Instance.EnemyTable.dataArray.Length - 1);

        //최대층
        if (enemyTableIdx == TableManager.Instance.EnemyTable.dataArray.Length - 1)
        {
            return TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Hp * TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Bosshpratio * Mathf.Abs(spawnNum - 300);
        }
        else
        {
            return TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Hp * TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Bosshpratio;
        }
    }

    public int GetEnemyDefense()
    {
        int enemyTableIdx = spawnNum * 22;

        enemyTableIdx = Mathf.Clamp(enemyTableIdx, 100, TableManager.Instance.EnemyTable.dataArray.Length - 1);

        //최대층
        if (enemyTableIdx == TableManager.Instance.EnemyTable.dataArray.Length - 1)
        {
            return TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Defense + enemyDeadCount.Value;
        }
        else
        {
            return TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Defense;
        }
    }

    public float GetMoveSpeed()
    {
        return Random.Range(5, 15);
    }

    private void WhenEnemyDead()
    {
        enemyDeadCount.Value++;
        currentSpawnedNum--;
    }
}