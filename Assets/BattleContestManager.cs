using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UiRewardView;

public class BattleContestManager : ContentsManagerBase
{

    [SerializeField]
    private Transform enemySpawnPos;
    [SerializeField]
    private TextMeshProUGUI enemyNickName;
    
    private ReactiveProperty<ObscuredInt> contentsState = new ReactiveProperty<ObscuredInt>((int)ContentsState.Fight);

    private List<Enemy> spawnedEnemyList = new List<Enemy>();

    //null 일때 클리어 못한거
    private List<RewardData> rewardDatas;

    [SerializeField]
    private FoxMaskResultPopup uiInfinityTowerResult;

    public UnityEvent retryFunc;

    public static string poolName;

    private List<RankManager.RankInfo> rankData;

    private new void Start()
    {
        base.Start();

        Subscribe();
        
        rankData = BattleContestData.MakeRankData();

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

        GuideMissionManager.UpdateGuideMissionClear(GuideMissionKey.ClearFoxMask);

        //보상팝업
        ShowResultPopup();
    }



    private void ShowResultPopup()
    {
        //클리어 팝업출력
        uiInfinityTowerResult.gameObject.SetActive(true);
        uiInfinityTowerResult.Initialize((ContentsState)(int)contentsState.Value);
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
    
       var tableData = TableManager.Instance.BattleContestTable.dataArray[GameManager.Instance.bossId];

       var maxCount = tableData.Minrank-tableData.Maxrank; 
       
       var randomIdx = Random.Range(0, maxCount);
    
       enemyNickName.SetText($"VS {rankData[randomIdx].NickName}<color=red>({rankData[randomIdx].Rank}위)</color>");
    
        int stageId = (int)rankData[randomIdx].Score;

        
        EnemyTableData spawnEnemyData = GetSpawnedEnemy(stageId);

        poolName = $"Enemy/{spawnEnemyData.Prefabname}{GameManager.Instance.bossId}";

        var enemyObject = BattleObjectManager.Instance.GetItem(poolName) as Enemy;
        Vector3 spawnPos = enemySpawnPos.position + Random.Range(-2f, 2f) * Vector3.right;

        enemyObject.transform.position = spawnPos + (Vector3)Random.insideUnitCircle;

        enemyObject.transform.localScale = Vector3.one * 1.3f;

        enemyObject.SetEnemyDeadCallBack(EnemyDeadCallBack);

        enemyObject.Initialize(spawnEnemyData, updateSubHpBar: true);

        spawnedEnemyList.Add(enemyObject);
        var enemyObjectView = enemyObject.gameObject.GetComponent<BattleContestViewController>();

        enemyObjectView.Initialize(rankData[randomIdx]);

    }
    private EnemyTableData GetSpawnedEnemy(int stageId)
    {
        var tableData = TableManager.Instance.EnemyTable.dataArray[stageId];

        EnemyTableData enemy = new EnemyTableData();

        enemy.Prefabname = "BattleContest";
        enemy.Attackpower = (float)tableData.Attackpower* tableData.Bossattackratio;
        enemy.Movespeed = tableData.Movespeed;
        enemy.Hp = tableData.Hp* tableData.Bosshpratio;
        enemy.Knockbackpower = 0;
        enemy.Defense = (int)tableData.Defense;

#if UNITY_EDITOR
        Debug.LogError($"체력 : {Utils.ConvertBigNum(tableData.Hp)} 방어력 : {tableData.Defense}");
#endif

        return enemy;
    }


    private void SetClear()
    {
        var data = TableManager.Instance.BattleContestTable.dataArray[GameManager.Instance.bossId];
        
        List<UiRewardView.RewardData> winData = new List<UiRewardView.RewardData>();

        for (int i = 0; i < data.Winvalue.Length; i++)
        {
            //패배시 재화를 빼야함
            var win = new UiRewardView.RewardData((Item_Type)data.Rewardtype[i],data.Winvalue[i]-data.Losevalue[i]);
            winData.Add(win);
        }
        Param goodsParam = new Param();

        for (int i = 0; i < winData.Count; i++)
        {
            ServerData.goodsTable.GetTableData((Item_Type)winData[i].itemType).Value += winData[i].amount;
            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(winData[i].itemType),
                ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString(winData[i].itemType))
                    .Value);
        }

        //승리

        string winString = ServerData.etcServerTable.TableDatas[EtcServerTable.battleWinScore].Value;
        
        var scoreList = winString.Split(BossServerTable.rewardSplit).Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();


        scoreList[GameManager.Instance.bossId]++;

        string newString = "";
        for (int i = 0; i < scoreList.Count; i++)
        {
            newString += $"{BossServerTable.rewardSplit}{scoreList[i]}";
        }

        ServerData.etcServerTable.TableDatas[EtcServerTable.battleWinScore].Value = newString;

        Param etcParam = new Param();
        
        etcParam.Add(EtcServerTable.battleWinScore,ServerData.etcServerTable.TableDatas[EtcServerTable.battleWinScore].Value);


        List<TransactionValue> transactionList = new List<TransactionValue>();

    
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactionList.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            //  StartCoroutine(AutoPlayRoutine());
        });
    }


    //private IEnumerator AutoPlayRoutine()
    //{
    //    yield return new WaitForSeconds(0.5f);

    //    if (UiLastContentsFunc.AutoInfiniteTower2)
    //    {
    //        retryFunc?.Invoke();
    //    }
    //}

}
