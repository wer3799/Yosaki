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


public class TwelveDungeonManager : ContentsManagerBase
{
    [Header("BossInfo")]
    private BossEnemyBase singleRaidEnemy;
    private AgentHpController bossHpController;

    private TwelveBossTableData twelveBossTable;
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

    [SerializeField]
    private Transform bossSpawnParent_Sin;

    [SerializeField]
    private Transform bossSpawnParent_Sin2;


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
        twelveBossTable = TableManager.Instance.TwelveBossTable.dataArray[GameManager.Instance.bossId];

        if (twelveBossTable.Skipboss == true)
        {
            hpCut = twelveBossTable.Rewardcut.Last();
        }
        else
        {
            hpCut = 1E+308;
        }
        var prefab = Resources.Load<BossEnemyBase>($"TwelveBoss/{GameManager.Instance.bossId}");

        //아수라,인드라,구미호
        if (GameManager.Instance.bossId == 13 || GameManager.Instance.bossId == 24 || GameManager.Instance.bossId == 38 || GameManager.Instance.bossId == 50)
        {
            singleRaidEnemy = Instantiate<BossEnemyBase>(prefab, bossSpawnParent_Sin);
        }
        else if (GameManager.Instance.bossId == 14)
        {
            singleRaidEnemy = Instantiate<BossEnemyBase>(prefab, bossSpawnParent_Sin2);

        }
        else
        {
            singleRaidEnemy = Instantiate<BossEnemyBase>(prefab, bossSpawnParent);
        }

        singleRaidEnemy.transform.localPosition = Vector3.zero;
        singleRaidEnemy.gameObject.SetActive(false);
        bossHpController = singleRaidEnemy.GetComponent<AgentHpController>();
        bossHpController.SetRaidEnemy();

        bossHpController.InitializeTwelveBoss(twelveBossTable.Skipboss);
        

    }

    private void whenDamageAmountChanged(ObscuredDouble hp)
    {
        damageIndicator.SetText(Utils.ConvertBigNum(hp));
        damagedAnim.SetTrigger(DamageAnimName);


        if (hp >= hpCut)
        {
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
        if (GameManager.Instance.bossId == 20)
        {
            GuideMissionManager.UpdateGuideMissionClear(GuideMissionKey.ClearGangChul);
        }
        //점수 전송
        SendScore();

        //보상팝업
        RewardItem();

        UiTutorialManager.Instance.SetClear(TutorialStep.PlayCatContents);
    }

    private void SendScore()
    {
        // //인만 업데이트
        // if (GameManager.Instance.bossId == 11)
        // {
        //     // RankManager.Instance.UpdateRealBoss_Score(damageAmount.Value);
        // }
        // //강철이
        // else if (GameManager.Instance.bossId == 20)
        // {
        //     //RankManager.Instance.UpdateRealBoss_Score_GangChul(damageAmount.Value);
        // }

        var serverData = ServerData.bossServerTable.TableDatas[twelveBossTable.Stringid];
        //푸른강철이
        if (twelveBossTable.Stringid == "b204")
        {
            
            Param bossScoreParam = new Param();

            bool isRenewal = false;
            if (ServerData.etcServerTable.IsBlueGangChulUnlocked(0))
            {
                double reqValue = damageAmount.Value * GameBalance.BossScoreSmallizeValue;

                if (reqValue > ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.DosulAwakeScore].Value)
                {
                    ServerData.bossScoreTable.TableDatas[BossScoreTable.DosulAwakeScore].Value = reqValue.ToString();
                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.DosulAwakeScore,reqValue);
                    bossScoreParam.Add(BossScoreTable.DosulAwakeScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.DosulAwakeScore].Value);
                    isRenewal = true;
                }
            }
            if (ServerData.etcServerTable.IsBlueGangChulUnlocked(1))
            {
                double reqValue = damageAmount.Value * GameBalance.BossScoreSmallizeValue;

                if (reqValue > ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.SealSwordAwakeScore].Value)
                {
                    ServerData.bossScoreTable.TableDatas[BossScoreTable.SealSwordAwakeScore].Value = reqValue.ToString();

                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.SealSwordAwakeScore,reqValue);
                    bossScoreParam.Add(BossScoreTable.SealSwordAwakeScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.SealSwordAwakeScore].Value);
                    isRenewal = true;
                }
            }

            if (ServerData.etcServerTable.IsBlueGangChulUnlocked(2))
            {
                double reqValue = damageAmount.Value * GameBalance.BossScoreSmallizeValue;

                if (reqValue > ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.danjeonScore].Value)
                {
                    ServerData.bossScoreTable.TableDatas[BossScoreTable.danjeonScore].Value = reqValue.ToString();

                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.danjeonScore,reqValue);
                    bossScoreParam.Add(BossScoreTable.danjeonScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.danjeonScore].Value);
                    isRenewal = true;
                }
            }

            if (ServerData.etcServerTable.IsBlueGangChulUnlocked(3))
            {
                double reqValue = damageAmount.Value * GameBalance.BossScoreSmallizeValue;

                if (reqValue > ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.hyunsangTowerScore].Value)
                {
                    ServerData.bossScoreTable.TableDatas[BossScoreTable.hyunsangTowerScore].Value = reqValue.ToString();

                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.hyunsangTowerScore,reqValue);
                    bossScoreParam.Add(BossScoreTable.hyunsangTowerScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.hyunsangTowerScore].Value);
                    isRenewal = true;
                }
            }
            if (ServerData.etcServerTable.IsBlueGangChulUnlocked(4))
            {
                double reqValue = damageAmount.Value * GameBalance.BossScoreSmallizeValue;

                if (reqValue > ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.closedScore].Value)
                {
                    ServerData.bossScoreTable.TableDatas[BossScoreTable.closedScore].Value = reqValue.ToString();

                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.closedScore,reqValue);
                    bossScoreParam.Add(BossScoreTable.closedScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.closedScore].Value);
                    isRenewal = true;
                }
            }
            if (ServerData.etcServerTable.IsBlueGangChulUnlocked(5))
            {
                double reqValue = damageAmount.Value * GameBalance.BossScoreSmallizeValue;

                if (reqValue > ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.munhaScore].Value)
                {
                    ServerData.bossScoreTable.TableDatas[BossScoreTable.munhaScore].Value = reqValue.ToString();

                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.munhaScore,reqValue);
                    bossScoreParam.Add(BossScoreTable.munhaScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.munhaScore].Value);
                    isRenewal = true;
                }
            }
            if (ServerData.etcServerTable.IsBlueGangChulUnlocked(6))
            {
                double reqValue = damageAmount.Value * GameBalance.BossScoreSmallizeValue;

                if (reqValue > ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.blackFoxScore].Value)
                {
                    ServerData.bossScoreTable.TableDatas[BossScoreTable.blackFoxScore].Value = reqValue.ToString();

                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.blackFoxScore,reqValue);
                    bossScoreParam.Add(BossScoreTable.blackFoxScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.blackFoxScore].Value);
                    isRenewal = true;
                }
            }
            if (ServerData.etcServerTable.IsBlueGangChulUnlocked(7))
            {
                double reqValue = damageAmount.Value * GameBalance.BossScoreSmallizeValue;

                if (reqValue > ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.transJewelScore].Value)
                {
                    ServerData.bossScoreTable.TableDatas[BossScoreTable.transJewelScore].Value = reqValue.ToString();

                    ServerData.bossScoreTable.UpdateNumberValue(BossScoreTable.transJewelScore,reqValue);
                    bossScoreParam.Add(BossScoreTable.transJewelScore,ServerData.bossScoreTable.TableDatas[BossScoreTable.transJewelScore].Value);
                    isRenewal = true;
                }
            }

            List<TransactionValue> transactions = new List<TransactionValue>();
            
            if (isRenewal)
            {
                transactions.Add(TransactionValue.SetUpdate(BossScoreTable.tableName, BossScoreTable.Indate, bossScoreParam));
            }

            if (string.IsNullOrEmpty(serverData.score.Value) == false)
            {
                if (damageAmount.Value < double.Parse(serverData.score.Value))
                {
                    
                }
                else
                {
                    serverData.score.Value = damageAmount.Value.ToString();


                    Param bossParam = new Param();
                    bossParam.Add(twelveBossTable.Stringid, ServerData.bossServerTable.TableDatas[twelveBossTable.Stringid].ConvertToString());
                    transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));                
                }
            }
            else
            {
                serverData.score.Value = damageAmount.Value.ToString();

                Param bossParam = new Param();
                bossParam.Add(twelveBossTable.Stringid, ServerData.bossServerTable.TableDatas[twelveBossTable.Stringid].ConvertToString());
                transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            }

            //쏠거없음 안쏘기
            if (transactions.Count < 1)
                return;
            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                //PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
            });


        }
        else
        {
            if (string.IsNullOrEmpty(serverData.score.Value) == false)
            {
                if (damageAmount.Value < double.Parse(serverData.score.Value))
                {
                    return;
                }
                else
                {
                    serverData.score.Value = damageAmount.Value.ToString();

                    ServerData.bossServerTable.UpdateData(twelveBossTable.Stringid);
                }
            }
            else
            {
                serverData.score.Value = damageAmount.Value.ToString();

                ServerData.bossServerTable.UpdateData(twelveBossTable.Stringid);
            }
        }

        //여래 예외처리
        if (twelveBossTable.Stringid == "b51")
        {

            var yeoraeData = ServerData.bossServerTable.TableDatas["b50"];

            if (string.IsNullOrEmpty(yeoraeData.score.Value) == false)
            {
                if (damageAmount.Value < double.Parse(yeoraeData.score.Value))
                {
                    return;
                }
                else
                {
                    yeoraeData.score.Value = damageAmount.Value.ToString();

                    ServerData.bossServerTable.UpdateData("b50");
                }
            }
            else
            {
                yeoraeData.score.Value = damageAmount.Value.ToString();

                ServerData.bossServerTable.UpdateData("b50");
            }

        }


    }

    private void RewardItem()
    {
        // DailyMissionManager.UpdateDailyMission(DailyMissionKey.RewardedBossContents, 1);

        //double damagedHp = damageAmount.Value;

        //List<RewardData> rewardDatas = GetRewawrdData(twelveBossTable, damagedHp);

        ////데이터 적용(서버)
        //ServerData.SendTransaction(rewardDatas);

        //혈자리 타워
        if (GameManager.Instance.bossId == 296)
        {
            var cur = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.hyulTowerRewardIndex).Value;

            var damage = damageAmount.Value;
            var tableData = TableManager.Instance.StudentSpotTower.dataArray;

            UiRewardResultPopUp.Instance.Clear();
            var rewardIdx = -1;
            //현재 등급+1부터 끝까지 받을 것이 있는가.
            for (int i = cur+1; i < tableData.Length; i++)
            {
                if (tableData[i].Rewrardcut > damage)
                {
                    break;
                }
                else
                {
                    UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)(int)tableData[i].Rewardtype, (float)tableData[i].Rewardvalue);
                    rewardIdx = i;

                }
            }

            if (cur < rewardIdx)
            {            

                List<TransactionValue> transactionList = new List<TransactionValue>();
                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.hyulTowerRewardIndex).Value = rewardIdx;

                using var e = UiRewardResultPopUp.Instance.RewardList.GetEnumerator();
            
                Param goodsParam = new Param();
                while (e.MoveNext())
                {
                    ServerData.AddLocalValue(e.Current.itemType, e.Current.amount);
                    goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType), ServerData.goodsTable.GetTableData(e.Current.itemType).Value);
                }
                Param userinfo2Param = new Param();

                userinfo2Param.Add(UserInfoTable_2.hyulTowerRewardIndex, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.hyulTowerRewardIndex].Value);
               
                transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));


                ServerData.SendTransactionV2(transactionList, successCallBack: () =>
                {
                    UiRewardResultPopUp.Instance.Show().Clear();
                });            
            }

        }
        
            //결과 UI
            uiBossResultPopup.gameObject.SetActive(true);
            statusUi.SetActive(false);
            uiBossResultPopup.Initialize(damageAmount.Value, damageAmount.Value / twelveBossTable.Hp);
        

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

        if (twelveBossTable != null)
        {
            if (twelveBossTable.TIMETYPE == TimeType.Half)
            {
                remainSec *= 0.5f;
            }
            else if (twelveBossTable.TIMETYPE == TimeType.Quarter)
            {
                remainSec *= 0.25f;
            }
        }

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
