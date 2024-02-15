using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using CodeStage.AntiCheat.ObscuredTypes;
using static UiRewardView;
using BackEnd;
using System.Linq;

public class OnlineTowerManager3 : ContentsManagerBase
{
    [Header("BossInfo")]
    [SerializeField]
    private BossEnemyBase finalBoss;

    [SerializeField]
    private BossEnemyBase middleBoss;

    [SerializeField]
    private AgentHpController finalBossHpController;

    [SerializeField]
    private AgentHpController middleBossHpController;

    private BossTableData bossTableData;
    private ReactiveProperty<ObscuredDouble> damageAmount = new ReactiveProperty<ObscuredDouble>();

    private TwoCaveData twoCaveData;

    private double bossMaxHp = double.MaxValue;

    private double currentTotalDamamge = 0;

    private BossLevel bossLevel = BossLevel.firstBoss;

    [SerializeField]
    private ObscuredInt PlayTime_LastBoss;

    [SerializeField]
    private GameObject bossSpawnEffectObject;

    [SerializeField] private GameObject guildHelpButton;
    
    private enum BossLevel
    {
        firstBoss, wait, final
    }

    public override Transform GetMainEnemyObjectTransform()
    {
        return null;
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
    private GameObject statusUi;

    [SerializeField]
    private GameObject directionUi;

    [SerializeField]
    private GameObject portalObject;

    [SerializeField]
    private Transform bossSpawnParent;

    private Coroutine sendScoreRoutine;

    [SerializeField] private GameObject loadingMask;
    
    public bool allPlayerEnd { get; private set; } = false;


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
        middleBossHpController.SetHp(double.MaxValue);

        twoCaveData = TableManager.Instance.twoCave.dataArray[PartyRaidManager.Instance.NetworkManager.partyRaidTargetFloor2];

        SoundManager.Instance.PlaySound("BossAppear");

        SetBossHp();

    }

    private void Subscribe()
    {
        finalBossHpController.whenEnemyDamaged.AsObservable().Subscribe(WhenBossDamaged).AddTo(this);
        middleBossHpController.whenEnemyDamaged.AsObservable().Subscribe(WhenBossDamaged).AddTo(this);

        PlayerStatusController.Instance.whenPlayerDead.Subscribe(e => { WhenPlayerDead(); }).AddTo(this);

        damageAmount.AsObservable().Subscribe(whenDamageAmountChanged).AddTo(this);



        contentsState.AsObservable().Subscribe(WhenBossModeStateChanged).AddTo(this);

        PartyRaidManager.Instance.NetworkManager.middleBossClearCount.AsObservable().Subscribe(e =>
        {
            if (PartyRaidManager.Instance.NetworkManager.RoomPlayerDatas.Count==1)
            {
                guildHelpButton.SetActive(e == 1);
            }
            //최종보스 시작
            else if (e == 2)
            {
                StartCoroutine(StartFinalBoss());
            }
            //
            //
            //
        }).AddTo(this);

        PartyRaidManager.Instance.NetworkManager.whenMiddleRetireReceived.AsObservable().Subscribe(e =>
        {

            contentsState.Value = (int)ContentsState.TimerEnd;

        }).AddTo(this);

        PartyRaidManager.Instance.NetworkManager.whenPlayerLeaved.AsObservable().Subscribe(e =>
        {
            //대기중에 누가 나가면
            if (bossLevel == BossLevel.wait)
            {
                PopupManager.Instance.ShowConfirmPopup("알림", "다른 유저가 도망갔습니다.", null);
                PartyRaidManager.Instance.NetworkManager.SendPartyTower2MiddleBossRetired();
            }

        }).AddTo(this);
    }

    private IEnumerator StartFinalBoss()
    {
        bossSpawnEffectObject.gameObject.SetActive(true);

        int bossStartCount = 3;

        int count = 0;

        while (count < bossStartCount)
        {
            PopupManager.Instance.ShowAlarmMessage($"{bossStartCount - count}초 후에 보스가 등장합니다.");
            count++;
            yield return new WaitForSeconds(1.0f);
        }

        bossSpawnEffectObject.gameObject.SetActive(false);

        PopupManager.Instance.ShowAlarmMessage($"보스등장");

        bossLevel = BossLevel.final;


        currentTotalDamamge = 0;

#if UNITY_EDITOR
        Debug.LogError("최종보스 시작");
#endif
        damageAmount.Value = 0;

        finalBoss.gameObject.SetActive(true);

        StartCoroutine(BossRandomActiveRoutine_finalBoss());

        //새 타이머 시작
        StartCoroutine(FinalBossTimerRoutine());
    }



    private void WhenBossModeStateChanged(ObscuredInt state)
    {
        if (state != (int)ContentsState.Fight)
        {
            PartyRaidManager.Instance.NetworkManager.playerState.Value = NetworkManager.PlayerState.End;
        }

        if (state == (int)ContentsState.TimerEnd)
        {
            SendLastScore();
            StopTimer();
            DisableBoss();
            ShowResultPopup();

            PartyRaidResultPopup.Instance.UpdateScoreBoard();
        }
        else if (state == (int)ContentsState.Dead)
        {
            SendLastScore();
            DisableBoss();
            ShowResultPopup();

            PartyRaidResultPopup.Instance.UpdateScoreBoard();
        }
        else if (state == (int)ContentsState.AllPlayerEnd)
        {
            SendLastScore();
            StopTimer();
            DisableBoss();
            ShowResultPopup();

            allPlayerEnd = true;

            if (PartyRaidResultPopup.Instance != null)
            {
                PartyRaidResultPopup.Instance.ExitButtonActive();
            }

            PartyRaidResultPopup.Instance.UpdateScoreBoard();
        }
        else if (contentsState.Value == (int)ContentsState.Clear)
        {
            SendLastScore();
            StopTimer();
            DisableBoss();
            ShowResultPopup();


            allPlayerEnd = true;

            if (PartyRaidResultPopup.Instance != null)
            {
                PartyRaidResultPopup.Instance.ExitButtonActive();
            }

            PartyRaidResultPopup.Instance.UpdateScoreBoard();
        }
    }

    private bool SendAutoRecommend = false;

    private void DisableBoss()
    {
        middleBoss.gameObject.SetActive(false);
        finalBoss.gameObject.SetActive(false);
    }



    private void SetBossHp()
    {
        finalBossHpController.SetRaidEnemy();
    }

    private void whenDamageAmountChanged(ObscuredDouble hp)
    {
        damageIndicator.SetText(Utils.ConvertBigNum(hp));
        damagedAnim.SetTrigger(DamageAnimName);
    }

    private void WhenBossDamaged(ObscuredDouble hp)
    {

    }

    private void WhenBossDamaged(double damage)
    {
        damageAmount.Value -= damage;

        //중간보스 클리어 했을때
        if (damageAmount.Value >= twoCaveData.Firstbosshp && bossLevel == BossLevel.firstBoss)
        {
            bossLevel = BossLevel.wait;

            damageAmount.Value = 0f;

            middleBoss.gameObject.SetActive(false);

            if (PartyRaidManager.Instance.NetworkManager.middleBossClearCount.Value == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("문을 클리어 했습니다 다른 유저를 기다립니다.");
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("문을 클리어 했습니다 곧 최종 보스가 등장 합니다.");
            }


            PartyRaidManager.Instance.NetworkManager.SendPartyTower2MiddleClear();


            timerText.SetText($"대기중");


            //기존 타이머 종료
            StopTimer();
        }
    }

    #region EndConditions
    //클리어조건1 플레이어 사망
    private void WhenPlayerDead()
    {
#if UNITY_EDITOR
        Debug.LogError("플레이어 사망");
#endif

        if (contentsState.Value != (int)ContentsState.Fight) return;

        contentsState.Value = (int)ContentsState.Dead;
    }

    //클리어조건1 보스 처치 성공
    private void WhenBossDead()
    {
        //클리어 체크
        contentsState.Value = (int)ContentsState.Clear;

        //SendClearInfo();
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

    private void AllPlayerEnd()
    {
        if (contentsState.Value == (int)ContentsState.AllPlayerEnd || contentsState.Value == (int)ContentsState.Clear) return;


        contentsState.Value = (int)ContentsState.AllPlayerEnd;
    }

    private void SendLastScore()
    {
        if (sendScoreRoutine != null)
        {
            StopCoroutine(sendScoreRoutine);
        }

        Debug.LogError("SendLastScore");

        //end
        if (bossLevel == BossLevel.firstBoss)
        {
            PartyRaidManager.Instance.NetworkManager.SendScoreInfo(damageAmount.Value, true);

            PartyRaidManager.Instance.NetworkManager.SendPartyTower2MiddleBossRetired();
        }
        else if (bossLevel == BossLevel.final)
        {
            PartyRaidManager.Instance.NetworkManager.SendScoreInfo2(damageAmount.Value, true);
        }
    }

    private void ShowResultPopup()
    {

        PartyRaidManager.Instance.NetworkManager.ShowPartyRaidResultPopup();

        // PartyRaidManager.Instance.NetworkManager.scoreBoardPanel.SetActive(false);

    }

    private IEnumerator EndCheckRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(1.0f);
        while (true)
        {
            CheckEndGame();
            yield return delay;
        }
    }

    protected override IEnumerator ModeTimer()
    {
        while (direciontEnd == false)
        {
            yield return null;
        }

        StartCoroutine(EndCheckRoutine());

        PartyRaidManager.Instance.NetworkManager.playerState.Value = NetworkManager.PlayerState.Playing;

        directionUi.SetActive(false);

        //  PartyRaidManager.Instance.NetworkManager.scoreBoardPanel.SetActive(true);

        PartyRaidManager.Instance.NetworkManager.whenScoreInfoReceived.AsObservable().Subscribe(e =>
        {

            UpdateFinalBossHp();

        }).AddTo(this);

        SpawnBoss();

        AutoManager.Instance.StartAutoWithDelay();

        sendScoreRoutine = StartCoroutine(SendScoreRoutine());

        portalObject.gameObject.SetActive(false);

        float remainSec = playTime;

        while (remainSec >= 0)
        {

            timerText.SetText($"남은시간 : {(int)remainSec}");

            yield return null;

            remainSec -= Time.deltaTime;
            this.remainSec = remainSec;
        }

        PartyRaidManager.Instance.NetworkManager.SendPartyTower2MiddleBossRetired();

        TimerEnd();

    }

    private IEnumerator FinalBossTimerRoutine()
    {
        float remainSec = PlayTime_LastBoss;

        while (remainSec >= 0)
        {

            timerText.SetText($"남은시간 : {(int)remainSec}");

            yield return null;

            remainSec -= Time.deltaTime;
            this.remainSec = remainSec;
        }

        TimerEnd();
    }

    //


    private IEnumerator BossRandomActiveRoutine_middleBoss()
    {
        middleBoss.GetComponent<HellWarModeEnemy>().StartAttackRoutine_PartyRaid();

        yield return null;
    }

    private IEnumerator BossRandomActiveRoutine_finalBoss()
    {
        finalBoss.GetComponent<HellWarModeEnemy>().StartAttackRoutine_PartyRaid();

        yield return null;
    }
    //

    private IEnumerator SendScoreRoutine()
    {
        var delay = new WaitForSeconds(0.03f);

        while (true)
        {
            yield return delay;

            if (bossLevel == BossLevel.firstBoss)
            {
                PartyRaidManager.Instance.NetworkManager.SendScoreInfo(damageAmount.Value);
            }
            else if (bossLevel == BossLevel.final)
            {
                PartyRaidManager.Instance.NetworkManager.SendScoreInfo2(damageAmount.Value);
            }
        }

    }
    private void SpawnBoss()
    {
        middleBoss.gameObject.SetActive(true);

        UpdateFinalBossHp();

        StartCoroutine(BossRandomActiveRoutine_middleBoss());

    }

    private void UpdateFinalBossHp()
    {
        if (bossLevel == BossLevel.firstBoss)
        {
            int stageId = PartyRaidManager.Instance.NetworkManager.partyRaidTargetFloor2;

            var TowerTableData4 = TableManager.Instance.twoCave.dataArray[stageId];

            bossMaxHp = TowerTableData4.Firstbosshp;
        }
        else if (bossLevel == BossLevel.final)
        {
            int stageId = PartyRaidManager.Instance.NetworkManager.partyRaidTargetFloor2;

            var TowerTableData4 = TableManager.Instance.twoCave.dataArray[stageId];

            bossMaxHp = TowerTableData4.Lastbosshp;
        }

        if (bossLevel == BossLevel.firstBoss)
        {
            currentTotalDamamge = damageAmount.Value;
        }
        else if (bossLevel == BossLevel.final)
        {
            currentTotalDamamge = PartyRaidManager.Instance.NetworkManager.GetTotalScore2();
        }

        if (UiOnlineTowerHpBar.Instance != null)
        {
            UiOnlineTowerHpBar.Instance.UpdateGauge(bossMaxHp - currentTotalDamamge, bossMaxHp);
        }

        CheckBossClear();

        CheckEndGame();
    }

    private void CheckEndGame()
    {
        //보상팝업
        if (PartyRaidManager.Instance.NetworkManager.IsAllPlayerEnd())
        {
            AllPlayerEnd();

            ShowResultPopup();

            StopCoroutine(EndCheckRoutine());
        }
    }

    private void CheckBossClear()
    {
        if (currentTotalDamamge >= bossMaxHp &&
            contentsState.Value != (int)ContentsState.Clear &&
            contentsState.Value != (int)ContentsState.AllPlayerEnd &&
            contentsState.Value != (int)ContentsState.TimerEnd &&
            bossLevel == BossLevel.final)
        {
            contentsState.Value = (int)ContentsState.Clear;

            SetClear();

        }
    }
    //null 일때 클리어 못한거
    private List<RewardData> rewardDatas;

    private bool rewarded = false;
    private string bossKey = "b91";

    private void SetClear()
    {
        PopupManager.Instance.ShowAlarmMessage("클리어!!");
        Debug.LogError("클리어!!");


        var serverData = ServerData.bossServerTable.TableDatas[bossKey];

        int currentStage = PartyRaidManager.Instance.NetworkManager.partyRaidTargetFloor2;

        currentStage++;

        if (string.IsNullOrEmpty(serverData.score.Value) == false)
        {
            if (currentStage < double.Parse(serverData.score.Value))
            {
                //본인원래 점수보다 낮은 스테이지일 경우
                return;
            }
            else
            {
                serverData.score.Value = currentStage.ToString();

                ServerData.bossServerTable.UpdateData(bossKey);
            }
        }
        else
        {
            serverData.score.Value = currentStage.ToString();

            ServerData.bossServerTable.UpdateData(bossKey);
        }
    }

    private ObscuredBool direciontEnd = false;

    public void WhenDirectionAnimEnd()
    {
        direciontEnd = true;
    }

    public void OnClickGuildHelpButton()
    {
        //길드가없으면
        if (GuildManager.Instance.hasGuild.Value == false) 
        {
            PopupManager.Instance.ShowAlarmMessage("혼자 입장 시 문파가 없으면 입장할 수 없습니다.");
            
            return;
        }
        
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,"<size=40>문파 - 대산군에 등록된 점수에 따라 단계를 클리어할 수 있습니다.\n단계를 클리어 하시겠습니까?</size>",YesCallBack,NoCallBack);
    }

    private void YesCallBack()
    {


        
        Backend.Social.Guild.GetMyGuildGoodsV3((guildInfoBro) =>
        {
            if (guildInfoBro.IsSuccess())
            {
                var returnValue = guildInfoBro.GetReturnValuetoJSON();

                int currentScore = int.Parse(returnValue["goods"]["totalGoods7Amount"]["N"].ToString());

                //
                var sangoonData = ServerData.bossServerTable.TableDatas["b73"];
                var shadowData = ServerData.bossServerTable.TableDatas[bossKey];

                sangoonData.score.Value = currentScore.ToString();

                ServerData.bossServerTable.UpdateData("b73");

                var tableData = TableManager.Instance.TwelveBossTable.dataArray[73];
                var scoreData = TableManager.Instance.TwelveBossTable.dataArray[74];

                var idx = -1;
                for (int i = 0; i < tableData.Rewardcut.Length; i++)
                {
                    if ((int)tableData.Rewardcut[i] == int.Parse(sangoonData.score.Value))
                    {
                        idx = i;
                        break;
                    }
                }

                if (idx < 0)
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "등록된 대산군 점수가 없습니다. ", LoadNormalField);
                    return;
                }

                var guildDamage = scoreData.Rewardcut[idx] / GameBalance.shadowCaveGuildScoreDevideValue;
                
                int currentStage = PartyRaidManager.Instance.NetworkManager.partyRaidTargetFloor2;

                var cave = TableManager.Instance.twoCave.dataArray[currentStage];
                Debug.LogError($"Require : {cave.Lastbosshp}\ndamage : {guildDamage}");
                //클리어
                if (cave.Lastbosshp<=guildDamage)
                {
                    currentStage++;
                    
                    if (string.IsNullOrEmpty(shadowData.score.Value) == false)
                    {
                        if (currentStage <= int.Parse(shadowData.score.Value))
                        {
                            //본인 원래 점수가 스테이지보다 큰 경우
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"이미 클리어한 스테이지입니다!", LoadNormalField);
                        }
                        else
                        {
                            shadowData.score.Value = currentStage.ToString();

                            ServerData.bossServerTable.UpdateData(bossKey);
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"점수 : { currentStage.ToString()} 등록 완료", LoadNormalField);
                        }
                    }
                    else
                    {
                        shadowData.score.Value = currentStage.ToString();

                        ServerData.bossServerTable.UpdateData(bossKey);
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"점수 : { currentStage.ToString()} 등록 완료", LoadNormalField);
                    }
                }
                else
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,"등록된 대산군 점수가 부족합니다.", LoadNormalField);
                }
                    
                //
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "점수 갱신 실패", null);
            }

            loadingMask.SetActive(false);
            // 이후 처리
        });   
    }
    
    private void NoCallBack()
    {
        
    }

    private void LoadNormalField()
    {
        PartyRaidManager.Instance.OnClickCloseButton();

        GameManager.Instance.LoadContents(GameManager.ContentsType.NormalField);    
    }
}
