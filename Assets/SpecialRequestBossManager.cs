using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using TMPro;
using CodeStage.AntiCheat.ObscuredTypes;
using static UiRewardView;
using BackEnd;


public class SpecialRequestBossManager : ContentsManagerBase
{
    [Header("BossInfo")]
    private BossEnemyBase singleRaidEnemy;
    private AgentHpController bossHpController;

    
    private double hpCut=0d;
    private ReactiveProperty<ObscuredDouble> damageAmount = new ReactiveProperty<ObscuredDouble>();


    [SerializeField]
    bool Reverse = false;

    public override Transform GetMainEnemyObjectTransform()
    {
        return singleRaidEnemy.transform;
    }
 


    public override double GetDamagedAmount()
    {
        return damageAmount.Value;
    }

    [Header("Ui")]
    [SerializeField]
    private TextMeshProUGUI damageIndicator;
    [SerializeField]
    private Animator damagedAnim;
    private string DamageAnimName = "Play";

    [Header("State")]
    private ReactiveProperty<ObscuredInt> contentsState = new ReactiveProperty<ObscuredInt>((int)ContentsState.Fight);

    [SerializeField]
    private UiTwelveBossResultPopup uiBossResultPopup;

    [SerializeField]
    private GameObject statusUi;

    [SerializeField]
    private GameObject directionUi;

    [SerializeField]
    private GameObject portalObject;

    [SerializeField]
    private Transform bossSpawnParent;

    private SpecialRequestBossTableData bossTableData;


    [SerializeField]
    private List<GameObject> mapObjects;

    #region Security
    private void OnEnable()
    {
        StartCoroutine(RandomizeRoutine());
    }
    #if UNITY_EDITOR
    private void OnDisable()
    {
        DamageCalculateManager.Instance.StopCalculate();
    }
    #endif
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
        damageAmount.Value.RandomizeCryptoKey();
        contentsState.Value.RandomizeCryptoKey();
    }
    #endregion

    protected new void Start()
    {
        base.Start();
        Initialize();
        Subscribe();


    }
    private void Initialize()
    {
        SoundManager.Instance.PlaySound("BossAppear");

        SetBossHp();

        SetBossMap();
    }

    private void SetBossMap()
    {
        int id = GameManager.Instance.bossId;


#if UNITY_EDITOR
        Debug.LogError($"Map id {id}");
#endif

        for (int i = 0; i < mapObjects.Count; i++)
        {

            mapObjects[i].gameObject.SetActive(i == id);

            if (i == id)
            {
                break;
            }
        }
    }

    private void Subscribe()
    {
        bossHpController.whenEnemyDamaged.AsObservable().Subscribe(WhenBossDamaged).AddTo(this);
        PlayerStatusController.Instance.whenPlayerDead.Subscribe(e => { WhenPlayerDead(); }).AddTo(this);

        damageAmount.AsObservable().Subscribe(whenDamageAmountChanged).AddTo(this);


        contentsState.AsObservable().Subscribe(WhenBossModeStateChanged).AddTo(this);
    }

    private void WhenBossModeStateChanged(ObscuredInt state)
    {
        if (state != (int)ContentsState.Fight)
        {
            EndBossMode();
        }
    }

    private void SetBossHp()
    {
        var data = Utils.GetCurrentSeasonSpecialRequestData();
        
        bossTableData = TableManager.Instance.SpecialRequestBossTable.dataArray[GameManager.Instance.bossId];
  
        hpCut = data.Specialrequestbosshp[GameManager.Instance.specialRequestBossId];
        
        var prefab = Resources.Load<BossEnemyBase>($"SpecialRequestBoss/{bossTableData.Id}");

        singleRaidEnemy = Instantiate<BossEnemyBase>(prefab, bossSpawnParent);
        

        singleRaidEnemy.transform.localPosition = Vector3.zero;
        singleRaidEnemy.gameObject.SetActive(false);
        bossHpController = singleRaidEnemy.GetComponent<AgentHpController>();
        bossHpController.SetRaidEnemy();
        bossHpController.InitializeSpecialRequestBoss();
    }

    private void whenDamageAmountChanged(ObscuredDouble hp)
    {
        damageIndicator.SetText(Utils.ConvertBigNum(hp));
        damagedAnim.SetTrigger(DamageAnimName);

        if (hp >= hpCut)
        {
            SendScore();
            
            TimerEnd();
        }
    }



    private void WhenBossDamaged(double damage)
    {
        damageAmount.Value -= damage;
            
        if (damageAmount.Value < 0)
        {
            damageAmount.Value = 0; 
        }

    }
    #region EndConditions
    //클리어조건1 플레이어 사망
    private void WhenPlayerDead()
    {
        if (contentsState.Value != (int)ContentsState.Fight) return;

        contentsState.Value = (int)ContentsState.Dead;

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "플레이어가 사망했습니다.", null);
    }

    //클리어조건1 보스 처치 성공
    private void WhenBossDead()
    {
        //클리어 체크
        contentsState.Value = (int)ContentsState.Clear;

        //  SendClearInfo();
    }

    //private void SendClearInfo()
    //{
    //    var serverData = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

    //    if (serverData.clear.Value != 1)
    //    {
    //        serverData.clear.Value = 1;

    //        ServerData.bossServerTable.UpdateData(bossTableData.Stringid);
    //    }
    //}

    //클리어조건1 타이머 종료
    protected override void TimerEnd()
    {
        base.TimerEnd();
        contentsState.Value = (int)ContentsState.TimerEnd;
    }
    #endregion

    private void EndBossMode()
    {
        //공격루틴 제거 + 클리어면 이펙트 켜주던지.?
        singleRaidEnemy.gameObject.SetActive(false);

        //타이머 종료
        if (contentsState.Value != (int)ContentsState.TimerEnd)
        {
            StopTimer();
        }
        //점수 전송
        //SendScore();

        //보상팝업
        RewardItem();
    }

    //깻을때만 쏨
    private void SendScore()
    {
        var currentData = Utils.GetCurrentSeasonSpecialRequestData();

        
        var score = (int)(remainSec / 10);

        score = Mathf.Min(score, 2);
        
        List<TransactionValue> transactionList = new List<TransactionValue>();
         Param bossParam = new Param();
        //

         var idx = GameManager.Instance.specialRequestBossId;
         
         for (int i = 0; i < idx; i++)
         {
             //이미 2점이면 스킵
             if (ServerData.specialRequestBossServerTable.TableDatas[currentData.Stringid[i]].score.Value >= 2) continue;
             ServerData.specialRequestBossServerTable.TableDatas[currentData.Stringid[i]].score.Value = 2;
            bossParam.Add(currentData.Stringid[i], ServerData.specialRequestBossServerTable.TableDatas[currentData.Stringid[i]].ConvertToString());
         }
         
        
        var stringId = currentData.Stringid[idx];
        var serverData = ServerData.specialRequestBossServerTable.TableDatas[stringId];
        
       if (serverData.score.Value >= score)
       {
           //데이터 없으면
       }
       else
       {
           serverData.score.Value = score;
           bossParam.Add(stringId, ServerData.specialRequestBossServerTable.TableDatas[stringId].ConvertToString());
           transactionList.Add(TransactionValue.SetUpdate(SpecialRequestBossServerTable.tableName, SpecialRequestBossServerTable.Indate, bossParam));
       }

       if (transactionList.Count < 1)
           return;
        
        ServerData.SendTransactionV2(transactionList);

    }

    private void RewardItem()
    {
        //결과 UI
        uiBossResultPopup.gameObject.SetActive(true);
        statusUi.SetActive(false);
        uiBossResultPopup.Initialize(damageAmount.Value, damageAmount.Value / hpCut);
    

    }

    protected override IEnumerator ModeTimer()
    {
        while (direciontEnd == false)
        {
            yield return null;
        }
        directionUi.SetActive(false);
        singleRaidEnemy.gameObject.SetActive(true);

        AutoManager.Instance.StartAutoWithDelay();

        portalObject.gameObject.SetActive(false);

        float remainSec = playTime;

        while (remainSec >= 0)
        {
            timerText.SetText($"남은시간 : {(int)remainSec}");
            yield return null;
            remainSec -= Time.deltaTime;
            this.remainSec = remainSec;
        }

        TimerEnd();
    }

    private ObscuredBool direciontEnd = false;

    public void WhenDirectionAnimEnd()
    {
        direciontEnd = true;
    }
}
