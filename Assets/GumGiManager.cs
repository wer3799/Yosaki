using Cinemachine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GumGiManager : SingletonMono<GumGiManager>
{
    [SerializeField]
    private PolygonCollider2D cameracollider;

    [SerializeField]
    private Transform start_point;

    [SerializeField]
    private Transform end_point;

    [SerializeField]
    private UiSogulResultPopup uiSogulResultPopup;

    [SerializeField]
    private Image timerGauge;

    private bool deadFlag = false;

    [SerializeField]
    private Animator currentWaveAnim;

    [SerializeField]
    private Animator remainTextAnim;

    private ObscuredInt enemyMaxCount = 100;

    [SerializeField]
    private TextMeshProUGUI endText;

    [SerializeField]
    private TextMeshProUGUI enemyKillCount;

    private ReactiveProperty<int> enemyDeadCount = new ReactiveProperty<int>();

    public enum EnemyType
    {
        _0, End
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
        endText.SetText($"요괴 {enemyMaxCount}초과시 종료");
    }
    private void SetFirstStage()
    {
        int lastStage = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value - (enemyMaxCount * 2);

        lastStage = Mathf.Max(0, lastStage);

        enemyDeadCount.Value = lastStage;

        enemyKillCount.color = Color.red;

        enemyDeadCount.AsObservable().Subscribe(e =>
        {
            enemyKillCount.SetText($"{e} 처치!");
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

        PopupManager.Instance.ShowAlarmMessage("검들이 나타납니다.");

        startTimerObject.gameObject.SetActive(false);

        modeState.Value = ModeState.Playing;
    }
    #endregion

    private Coroutine mainGameRoutine;

    [SerializeField]
    private TextMeshProUGUI currentWaveText;

    private WaitForSeconds spawnDelay = new WaitForSeconds(0.1f);

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


    public EnemyTableData GetEnemyTableData(EnemyType enemyType)
    {
        EnemyTableData enemyData = new EnemyTableData();

        int index = Mathf.Min(enemyDeadCount.Value, TableManager.Instance.EnemyTable.dataArray.Length - 1);

        var tableData = TableManager.Instance.EnemyTable.dataArray[index];

        enemyData.Hp = tableData.Hp * enemyDeadCount.Value * 1800000;

        enemyData.Attackpower = tableData.Attackpower * 0.0001f * enemyDeadCount.Value;

        enemyData.Defense = (int)tableData.Defense;

        enemyData.Movespeed = Random.Range(5, 8);

        return enemyData;
    }

    private List<Enemy> spawnedEnemyList = new List<Enemy>();
    private void SpawnEnemies()
    {
        int spawnCount = 12;

        for (int i = 0; i < spawnCount; i++)
        {
            int randValue = Random.Range(0, (int)EnemyType.End);

            EnemyType enemyType = (EnemyType)randValue;

            var enemyObject = BattleObjectManager.Instance.GetItem($"GumGi/{enemyType.ToString()}") as Enemy;

            enemyObject.transform.position = new Vector3(Random.Range(start_point.position.x, end_point.position.x), start_point.position.y, start_point.position.z);

            enemyObject.SetReturnCallBack(EnemyRemoveCallBack);

            EnemyTableData enemyData = GetEnemyTableData(enemyType);

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
            if(currentWaveAnim!=null)
            {currentWaveAnim.SetTrigger(PlayStr);}
            
        }


        UpdateRemainEnemy();
    }

    private static string PlayStr = "Play";
    private void UpdateRemainEnemy()
    {
        remainEnemyText.SetText($"남은 검:{spawnedEnemyList.Count}/{enemyMaxCount}");

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

        GuideMissionManager.UpdateGuideMissionClear(GuideMissionKey.ClearSwordPartial);

        int pref = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value;

        int updateValue = enemyDeadCount.Value;

        if (updateValue > pref)
        {
            ServerData.userInfoTable.UpData(UserInfoTable.gumGiClear, updateValue, false);
        }

        uiSogulResultPopup.Initialize(updateValue, false, deadFlag);


    }
}
