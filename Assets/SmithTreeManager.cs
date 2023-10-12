using Cinemachine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class SmithTreeManager : SingletonMono<SmithTreeManager>
{
    [SerializeField]
    private PolygonCollider2D cameracollider;

    [SerializeField]
    private Transform spawnMin;

    [SerializeField]
    private Transform spawnMax;

    [SerializeField]
    private UiSogulResultPopup uiSogulResultPopup;

    [SerializeField]
    private Image timerGauge;

    private bool deadFlag = false;

    [SerializeField]
    private Animator currentWaveAnim;

    [SerializeField]
    private Animator remainTextAnim;

    private ObscuredInt enemyMaxCount = 200;

    [SerializeField]
    private TextMeshProUGUI endText;

    [SerializeField]
    private TextMeshProUGUI enemyKillCount;

    private ReactiveProperty<int> enemyDeadCount = new ReactiveProperty<int>();

    public enum EnemyType
    {
        Normal
    }

    public enum ModeState
    {
        Wait, Playing, End
    }

    private ReactiveProperty<ModeState> modeState = new ReactiveProperty<ModeState>(ModeState.Wait);

    private void Start()
    {
        SetFirstStage();

        Subscribe();

        SetCameraCollider();

        DisableUi();

        SetEndText();
    }

    private void SetEndText()
    {
        endText.SetText($"{enemyMaxCount}초과시 종료");
    }
    private void SetFirstStage()
    {
        int lastStage = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.smithTreeClear].Value - (enemyMaxCount * 2);

        lastStage = Mathf.Max(0, lastStage);

        enemyDeadCount.Value = lastStage;

        enemyKillCount.color = Color.red;

        enemyDeadCount.AsObservable().Subscribe(e =>
        {
            enemyKillCount.SetText($"{e} 획득!");
        }).AddTo(this);
    }

    #region ETC
    private void DisableUi()
    {
        UiSubMenues.Instance.gameObject.SetActive(false);
    }

    private void SetCameraCollider()
    {
        var cameraConfiner = GameObject.FindObjectOfType<CinemachineConfiner>();
        cameraConfiner.m_BoundingShape2D = cameracollider;
    }
    #endregion

    private void Subscribe()
    {
        PlayerStatusController.Instance.whenPlayerDead.AsObservable().Subscribe(e =>
        {
            deadFlag = true;
            modeState.Value = ModeState.End;
        }).AddTo(this);

        modeState.AsObservable().Subscribe(state =>
        {
            switch (state)
            {
                case ModeState.Wait:
                    {
                        StartCoroutine(StartTimer());
                    }
                    break;
                case ModeState.Playing:
                    {
                        mainGameRoutine = StartCoroutine(MainGameRoutine());
                    }
                    break;
                case ModeState.End:
                    {
                        EndGame();
                    }
                    break;
            }
        }).AddTo(this);
    }


    #region StartTimer
    [SerializeField]
    private GameObject startTimerObject;
    [SerializeField]
    private TextMeshProUGUI startTimerText;
    [SerializeField]
    private TextMeshProUGUI remainEnemyText;

    private IEnumerator StartTimer()
    {
        //초반 딜레이
        //초반 딜레이 알림 시작
        remainTimeObject.SetActive(false);
        startTimerObject.gameObject.SetActive(true);

        float tick = 0f;

        float startWaitTime = 3f;

        while (tick < 3f)
        {
            tick += Time.deltaTime;

            startTimerText.SetText($"{(int)startWaitTime - (int)tick}초 뒤에 시작");

            yield return null;
        }

        remainTimeObject.SetActive(true);

        PopupManager.Instance.ShowAlarmMessage("살아있는 장작들이 나타납니다.");

        startTimerObject.gameObject.SetActive(false);

        modeState.Value = ModeState.Playing;
    }
    #endregion

    private Coroutine mainGameRoutine;

    [SerializeField]
    private TextMeshProUGUI currentWaveText;

    //private WaitForSeconds spawnDelay = new WaitForSeconds(0.4f);
    private WaitForSeconds spawnDelay = new WaitForSeconds(0.13f);

    private IEnumerator MainGameRoutine()
    {
        while (true)
        {
            //웨이브 올리기 
            SpawnEnemies();

            yield return spawnDelay;

        }

        yield return null;

    }


    private void CheckEnd()
    {
        if (spawnedEnemyList.Count >= enemyMaxCount)
        {
            modeState.Value = ModeState.End;
        }
    }


    public EnemyTableData GetEnemyTableData()
    {
        EnemyTableData enemyData = new EnemyTableData();

        int spawnCount = enemyDeadCount.Value;

        int enemyTableIdx = spawnCount * 2 + 8000 /*+ (randIdx * 200)*/;

        enemyTableIdx = Mathf.Clamp(enemyTableIdx, 100, 12400);

        bool isMax = enemyTableIdx == 12400;

        double addValue = 0d;

        if (isMax)
        {
            addValue = (spawnCount) - enemyTableIdx + 9000;
        }

        if (enemyTableIdx >= 12400)
        {
            enemyData.Hp = TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Hp * (System.Math.Pow(1.003d, addValue));
        }
        else
        {
            enemyData.Hp = TableManager.Instance.EnemyTable.dataArray[enemyTableIdx].Hp * 1000;
        }

        enemyData.Hp *= 100000000000000000f;

        Debug.LogError(enemyData.Hp);

        enemyData.Attackpower = 0;

        enemyData.Defense = (int)enemyDeadCount.Value;

        return enemyData;
    }

    private List<Enemy> spawnedEnemyList = new List<Enemy>();
    private void SpawnEnemies()
    {
        int spawnCount = 10;

        for (int i = 0; i < spawnCount; i++)
        {
            var enemyObject = BattleObjectManager.Instance.GetItem($"GumGiSoul/SmithTree") as Enemy;

            enemyObject.transform.position = new Vector3(Random.Range(spawnMin.position.x, spawnMax.position.x), Random.Range(spawnMin.position.y, spawnMax.position.y));

            enemyObject.SetReturnCallBack(EnemyRemoveCallBack);

            EnemyTableData enemyData = GetEnemyTableData();

            enemyObject.Initialize(enemyData, false, 0);

            spawnedEnemyList.Add(enemyObject);

            UpdateRemainEnemy();
        }

        CheckEnd();
    }

    private void EnemyRemoveCallBack(Enemy enemy)
    {
        spawnedEnemyList.Remove(enemy);

        if (modeState.Value != ModeState.End)
        {
            enemyDeadCount.Value++;
            currentWaveAnim.SetTrigger(PlayStr);
        }


        UpdateRemainEnemy();
    }

    private static string PlayStr = "Play";
    private void UpdateRemainEnemy()
    {
        remainEnemyText.SetText($"남은 장작:{spawnedEnemyList.Count}/{enemyMaxCount}");

        if (remainTextAnim != null)
        {
            remainTextAnim.SetTrigger(PlayStr);
        }
    }


    [SerializeField]
    private GameObject remainTimeObject;

    [SerializeField]
    private TextMeshProUGUI remainTimeText;

    private Coroutine waveTimerRoutine;



    private bool IsEnemyAllDead()
    {
        return spawnedEnemyList.Count == 0;
    }

    private void StopGameRoutines()
    {
        if (mainGameRoutine != null)
        {
            StopCoroutine(mainGameRoutine);
        }

        if (waveTimerRoutine != null)
        {
            StopCoroutine(waveTimerRoutine);
        }
    }
    private void EndGame()
    {
        SoundManager.Instance.PlaySound("Reward");

        StopGameRoutines();

        int pref = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.smithTreeClear].Value;

        int updateValue = enemyDeadCount.Value;

        if (updateValue > pref)
        {
            ServerData.userInfoTable.UpData(UserInfoTable.smithTreeClear, updateValue, false);
        }

        uiSogulResultPopup.Initialize(updateValue, false, deadFlag, defix: "개 달성!!");
    }
}
