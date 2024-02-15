using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class AgentHpController : MonoBehaviour
{
    private ReactiveProperty<double> currentHp = new ReactiveProperty<double>();
    public double CurrentHp => currentHp.Value;
    public ReactiveCommand whenEnemyDead { get; private set; } = new ReactiveCommand();
    public ReactiveCommand<double> whenEnemyDamaged { get; private set; } = new ReactiveCommand<double>();
    public double maxHp { get; private set; }

    private EnemyTableData enemyTableData;
    private float gainGoldFromStage;

    [SerializeField]
    private EnemyHpBar enemyHpBar;

    [SerializeField]
    private bool ReverseDamage = false;

    private EnemyMoveController enemyMoveController;

    private bool isEnemyDead = false;

    public ReactiveCommand<double> WhenAgentDamaged = new ReactiveCommand<double>();

    private Transform playerPos;

    private ObscuredFloat defense;
    public ObscuredFloat Defense => defense;

    [SerializeField]
    private Transform damTextSpawnPos;

    private bool updateSubHpBar = false;

    private bool isFieldBossEnemy = false;
    private bool fieldBossTimerStarted = false;
    private bool initialized = false;
    private bool isRaidEnemy = false;

    public void SetRaidEnemy()
    {
        isRaidEnemy = true;
    }

    private void Awake()
    {
        GetRequireComponents();
    }

    private void GetRequireComponents()
    {
        enemyMoveController = GetComponent<EnemyMoveController>();
    }

    private void Start()
    {
        Subscribe();
        SetPlayerTr();
    }

    private void SetPlayerTr()
    {
        if (initialized == false)
        {
            playerPos = PlayerMoveController.Instance.transform;
            initialized = true;
        }
    }

    private void Subscribe()
    {
        currentHp.AsObservable().Subscribe(e =>
        {
            enemyHpBar.UpdateGauge(e, maxHp);

            if (updateSubHpBar)
            {
                UiSubHpBar.Instance.UpdateGauge(e, maxHp);
            }
        }).AddTo(this);
    }

    private void ResetEnemy()
    {
        currentHp.Value = maxHp;
        isEnemyDead = false;
    }

    public void SetDefense(float defense)
    {
        this.defense = defense;
    }

    public void Initialize(EnemyTableData enemyTableData, bool isFieldBossEnemy = false, bool updateSubHpBar = false, bool setHpOne = false)
    {
        fieldBossTimerStarted = false;

        this.isFieldBossEnemy = isFieldBossEnemy;

        this.enemyTableData = enemyTableData;

        gainGoldFromStage = TableManager.Instance.StageMapTable.dataArray[enemyTableData.Id].Goldbar;

        this.updateSubHpBar = isFieldBossEnemy || updateSubHpBar;

        SetDefense(enemyTableData.Defense);

        if (isFieldBossEnemy == false)
        {
            if (setHpOne == false)
            {
                SetHp(enemyTableData.Hp);
            }
            else
            {
                SetHp(1);
            }
        }
        else
        {
            double bossHp = enemyTableData.Hp * enemyTableData.Bosshpratio;

            if (GameManager.contentsType == GameManager.ContentsType.BattleContest)
            {
                bossHp *= Random.Range(0.8f, 1f);
            }

            double decreaseValue = PlayerStats.DecreaseBossHp();

            bossHp -= bossHp * decreaseValue;

            SetHp(bossHp);
        }
    }

    public void SetHp(double hp)
    {
        this.maxHp = hp;
        currentHp.Value = maxHp;
    }

    public void SetKnockBack()
    {
        if (enemyMoveController == null) return;
        enemyMoveController.SetKnockBack();
    }

    private static string hitSfxName = "EnemyHitted";
    private static string deadSfxName = "EnemyDead";

    public void ApplyPlusDamage(ref double value, bool isCritical, bool isSuperCritical)
    {
        SoundManager.Instance.PlaySound(hitSfxName);

        //크리티컬
        if (isCritical)
        {
            value += value * PlayerStats.CriticalDam();
        }

        //보너스던전등 특수몹
        if (enemyTableData != null && enemyTableData.Useonedamage)
        {
            isCritical = false;
            value = 1f;
        }

        value += value * PlayerStats.GetBossDamAddValue();

        //방어력 초과데미지
        //방어력 차이
        float gapDefense = (float)(defense - ignoreDefenseValue);
        if (gapDefense < 0)
        {
            double penetrateValue = PlayerStats.GetPenetrateDefense();
            value += Mathf.Abs(gapDefense) * value * penetrateValue;
        }

        //태극베기
        value += value * PlayerStats.GetSuperCritical16DamPer();
        //슈퍼크리티컬

        if (isSuperCritical)
        {
            //30% 고정
            value += value * PlayerStats.GetSuperCriticalDamPer();
        }

        //필멸 데미지
        value += value * PlayerStats.GetSuperCritical2DamPer();

        //단전베기
        value += value * PlayerStats.GetSuperCritical8DamPer();

        //중단전베기
        value += value * PlayerStats.GetSuperCritical13DamPer();

        //상단전베기
        value += value * PlayerStats.GetSuperCritical18DamPer();

        //지옥 데미지
        value += value * PlayerStats.GetSuperCritical3DamPer();

        //천상베기 데미지
        value += value * PlayerStats.GetSuperCritical4DamPer();

        //도깨비참수 데미지
        value += value * PlayerStats.GetSuperCritical5DamPer();

        //수호베기
        value += value * PlayerStats.GetSuperCritical11DamPer();

        //여우베기
        value += value * PlayerStats.GetSuperCritical14DamPer();

        //신수베기 데미지
        value += value * PlayerStats.GetSuperCritical6DamPer();

        //사흉베기
        value += value * PlayerStats.GetSuperCritical9DamPer();

        //수미베기
        value += value * PlayerStats.GetSuperCritical7DamPer();

        //도적베기
        value += value * PlayerStats.GetSuperCritical10DamPer();
        //심연베기
        value += value * PlayerStats.GetSuperCritical12DamPer();
        //신선베기
        value += value * PlayerStats.GetSuperCritical15DamPer();
        //영혼베기
        value += value * PlayerStats.GetSuperCritical17DamPer();
        //귀살베기
        value += value * PlayerStats.GetSuperCritical19DamPer();
        //천구베기
        value += value * PlayerStats.GetSuperCritical20DamPer();
        //초월베기
        value += value * PlayerStats.GetSuperCritical21DamPer();
        //진귀살베기
        value += value * PlayerStats.GetSuperCritical22DamPer();
        //심상베기
        value += value * PlayerStats.GetSuperCritical23DamPer();
        //용살베기
        value += value * PlayerStats.GetSuperCritical24DamPer();
        //요력베기
        value += value * PlayerStats.GetSuperCritical25DamPer();
        //진 요도 피해
        value += value * PlayerStats.GetSuperCritical26DamPer();
        //무공 피해
        value += value * PlayerStats.GetSuperCritical27DamPer();
        //파도베기
        value += value * PlayerStats.GetSuperCritical28DamPer();
        //비무 피해
        value += value * PlayerStats.GetSuperCritical29DamPer();
        //신력피해
        value += value * PlayerStats.GetSuperCritical30DamPer();
        //협동피해
        value += value * PlayerStats.GetSuperCritical31DamPer();
        //무림피해
        value += value * PlayerStats.GetSuperCritical32DamPer();
        //극혈피해
        value += value * PlayerStats.GetSuperCritical33DamPer();
        //??피해
        value += value * PlayerStats.GetSuperCritical34DamPer();
    }

    private Vector3 damTextspawnPos;
    private Coroutine damTextRoutine;
    private int attackCount = 0;
    private int attackCountMax = 16;
    private double attackResetCount = 0f;
    private double damTextMergeTime = 0.5f;
    private const float damTextCountAddValue = 0.1f;
    private readonly WaitForSeconds DamTextDelay = new WaitForSeconds(damTextCountAddValue);
    private float rightValue = 0f;
    private float upValue = 2f;

    public void SpawnDamText(bool isCritical, bool isSuperCritical, double value)
    {
        Vector3 spawnPos = Vector3.zero;

        if (damTextSpawnPos != null)
        {
            spawnPos = damTextSpawnPos.position;
        }
        else
        {
            spawnPos = this.transform.position;
        }

        if (damTextRoutine == null && isEnemyDead == false)
        {
            damTextRoutine = StartCoroutine(DamTextRoutine());
        }

        damTextspawnPos = this.transform.position + Vector3.up * attackCount * 1f + Vector3.right * rightValue + Vector3.up * upValue;

        attackCount++;

        if (attackCount == attackCountMax)
        {
            attackCount = 0;
            rightValue = UnityEngine.Random.Range(-3f, 3f);
            upValue = UnityEngine.Random.Range(2f, 4.5f);
        }

        attackResetCount = 0f;

        SetPlayerTr();

        if (Vector3.Distance(playerPos.position, this.transform.position) < GameBalance.effectActiveDistance)
        {
            DamTextType damType = DamTextType.Normal;

            if (isCritical)
            {
                damType = DamTextType.Critical;
            }

            if (isSuperCritical)
            {
                damType = DamTextType.SuperCritical;
            }

            BattleObjectManagerAllTime.Instance.SpawnDamageText(value, damTextspawnPos, damType);
        }
    }

    private IEnumerator DamTextRoutine()
    {
        while (attackResetCount < damTextMergeTime)
        {
            yield return DamTextDelay;
            attackResetCount += 0.1f;
        }

        ResetDamTextValue();
    }

    private void ResetDamTextValue()
    {
        attackCount = 0;
        attackResetCount = 0f;
        damTextRoutine = null;
    }

    //안씀, 골드 드랍으로 바꿈
    private void GetHitGold(float value)
    {
        ////데미지는 -값임
        //if (value < 0)
        //{
        //    if (currentHp.Value + value > 0)
        //    {
        //        GetGoldByEnemy(-value);
        //    }
        //    else
        //    {
        //        GetGoldByEnemy(-(value - currentHp.Value));
        //    }
        //}
    }

    public void UpdateHp(double value)
    {
        if (value < 0 && isFieldBossEnemy && fieldBossTimerStarted == false)
        {
            fieldBossTimerStarted = true;
            UiStageNameIndicater.Instance.StartFieldBossTimer(15);
        }

        //방어력 적용
        //ApplyDefense(ref value);

        //ApplyPlusDamage(ref value);

        if (isEnemyDead == true) return;

        if (ReverseDamage)
        {
            value *= -1f;
        }

        if (value < 0f)
        {
            WhenAgentDamaged.Execute(-value);
        }

        //GetHitGold(value);
        if (value < 0)
        {
            currentHp.Value += value;
        }


        whenEnemyDamaged.Execute(value);

        if (currentHp.Value <= 0 && isRaidEnemy == false)
        {
            EnemyDead();

            return;
        }
    }

    private float ignoreDefenseValue;

    public void ApplyDefense(ref double value)
    {
        ignoreDefenseValue = PlayerStats.GetIgnoreDefenseValue();

        float enemyDefense = Mathf.Max(0f, defense - ignoreDefenseValue);

        value -= value * enemyDefense * 0.01f;
    }

    private void EnemyDead()
    {
        whenEnemyDead.Execute();

        isEnemyDead = true;

        AddEnemyDeadCount();

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateGold).Value < 1)
        {
            GetGoldByEnemy(enemyTableData.Gold);
        }
        else
        {
            GetGoldBarByEnemy(gainGoldFromStage);
        }

        this.gameObject.SetActive(false);

        SoundManager.Instance.PlaySound(deadSfxName);
    }

    private void AddEnemyDeadCount()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).Value += GameManager.Instance.CurrentStageData.Marbleamount;
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailybooty).Value += GameManager.Instance.CurrentStageData.Dailyitemgetamount * GameManager.Instance.CurrentStageData.Marbleamount;
        ServerData.userInfoTable.GetKillCountTotal();
        ServerData.userInfoTable_2.GetKillCountTotal();
    }

    private void OnEnable()
    {
        ResetEnemy();
    }

    private void GetGoldByEnemy(float gold)
    {
        gold += gold * PlayerStats.GetGoldPlusValue();

        ServerData.goodsTable.GetGold(gold);
    }

    private void GetGoldBarByEnemy(float goldbar)
    {
        goldbar += goldbar *= PlayerStats.GetGoldBarPlusValue();

        ServerData.goodsTable.GetGoldBar(goldbar);
    }

    private void OnDisable()
    {
        updateSubHpBar = false;
    }
}