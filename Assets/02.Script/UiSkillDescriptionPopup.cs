using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using System.Linq;

public class UiSkillDescriptionPopup : MonoBehaviour
{
    [SerializeField] private Image skillIcon;

    [SerializeField] private TextMeshProUGUI skillName;

    [SerializeField] private TextMeshProUGUI skillDesc;

    [SerializeField] private TextMeshProUGUI skillAbility;

    private SkillTableData skillTableData;

    [SerializeField] private Button levelUpButton;

    [SerializeField] private Image awakeButton;

    [SerializeField] private TextMeshProUGUI awakeButtonDescription;

    [SerializeField] private WeaponView weaponView;

    [SerializeField] private TextMeshProUGUI levelDescription;

    [SerializeField] private TextMeshProUGUI awakeDescription;

    [SerializeField] private TextMeshProUGUI levelupButtonDescription;

    [SerializeField] private TextMeshProUGUI awakeText;

    private string lvTextFormat = "LV : {0}/{1}<color=yellow>(+{2} 강화됨)</color>";

    private CompositeDisposable disposables = new CompositeDisposable();

    [SerializeField] private UiSkillSlotSettingBoard uiSkillSlotSettingBoard;

    [SerializeField] private Button equipButton;

    [SerializeField] private TextMeshProUGUI hasEffectDesc;

    [SerializeField] private GameObject tutorialObject;

    public void OnClickEquipButton()
    {
        int skillAwakeNum = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id]
            .Value;

        if (skillAwakeNum == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("스킬을 배워야 등록할 수 있습니다.");
            return;
        }

        uiSkillSlotSettingBoard.gameObject.SetActive(true);
        uiSkillSlotSettingBoard.SetSkillIdx(skillTableData.Id);
    }

    public void Initialize(SkillTableData skillTableData)
    {
        if(skillTableData.Id>=15&&skillTableData.SKILLCASTTYPE==SkillCastType.Player)
        // //신수스킬용
        // if (
        //     skillTableData.Id == 15 ||
        //     skillTableData.Id == 16 ||
        //     skillTableData.Id == 17 ||
        //     skillTableData.Id == 20 ||
        //     skillTableData.Id == 21 ||
        //     skillTableData.Id == 22 ||
        //     skillTableData.Id == 23 ||
        //     skillTableData.Id == 24 ||
        //     skillTableData.Id == 25 ||
        //     skillTableData.Id == 26 ||
        //     skillTableData.Id == 27 ||
        //     skillTableData.Id == 28 ||
        //     skillTableData.Id == 29 ||
        //     skillTableData.Id == 30 ||
        //     skillTableData.Id == 31 ||
        //     skillTableData.Id == 32 ||
        //     skillTableData.Id == 33 ||
        //     skillTableData.Id == 34 ||
        //     skillTableData.Id == 35 ||
        //     skillTableData.Id == 36 ||
        //     skillTableData.Id == 41 ||
        //     skillTableData.Id == 42 ||
        //     skillTableData.Id == 43 ||
        //     skillTableData.Id == 44 ||
        //     skillTableData.Id == 45
        // )
        {
            if (skillTableData.Id == 15)
            {
                if (ServerData.goodsTable.GetTableData(GoodsTable.SinSkill2).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 16)
            {
                if (ServerData.goodsTable.GetTableData(GoodsTable.SinSkill3).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 17)
            {
                //흑룡 있을때
                if (ServerData.costumeServerTable.TableDatas["costume27"].hasCostume.Value == true)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 20)
            {
                //흑룡 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.NataSkill).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 21)
            {
                //흑룡 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.OrochiSkill).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            //
            //

            else if (skillTableData.Id == 22)
            {
                //흑룡 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.Sun0).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 23)
            {
                //흑룡 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.Sun1).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 24)
            {
                //흑룡 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.Sun2).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 25)
            {
                //흑룡 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.Sun3).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 26)
            {
                //흑룡 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.Sun4).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 27)
            {
                //천계검 1형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.Chun0).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 28)
            {
                //천계검 2형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.Chun1).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 29)
            {
                //천계검 3형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.Chun2).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 30)
            {
                //천계검 4형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.Chun3).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 31)
            {
                //천계검 5형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.Chun4).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 32)
            {
                //도깨비검 1형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill0).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 33)
            {
                //도깨비검 2형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill1).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 34)
            {
                //도깨비검 3형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill2).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 35)
            {
                //도깨비검 4형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill3).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else if (skillTableData.Id == 36)
            {
                //도깨비검 5형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.DokebiSkill4).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            //

            else if (skillTableData.Id == 41)
            {
                //도깨비검 5형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill4).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }

            else if (skillTableData.Id == 42)
            {
                //도깨비검 5형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill5).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }

            else if (skillTableData.Id == 43)
            {
                //도깨비검 5형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill6).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }

            else if (skillTableData.Id == 44)
            {
                //도깨비검 5형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill7).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }

            else if (skillTableData.Id == 45)
            {
                //도깨비검 5형 있을때
                if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill8).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
            else
            {
                if (ServerData.goodsTable.GetTableData(skillTableData.Skillclassname).Value != 0)
                {
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value =
                        1;
                }
            }
        }

        //

        this.gameObject.SetActive(true);

        this.skillTableData = skillTableData;

        skillIcon.sprite = CommonResourceContainer.GetSkillIconSprite(skillTableData.Id);

        skillName.SetText(skillTableData.Skillname);

        skillName.color = CommonUiContainer.Instance.itemGradeColor[skillTableData.Skillgrade];

        skillDesc.SetText(skillTableData.Skilldesc);

        UpdateSkillAwakeText();

        weaponView.Initialize(null, null, skillTableData);

        Subscribe();

        RefreshUpgradeButton();

        UpdateUi();
    }

    private void UpdateSkillAwakeText()
    {
        awakeText.SetText(ServerData.skillServerTable.GetSkillMaxLevel(skillTableData.Id) == 0 ? "배우기" : "각성");
    }

    private void UpdateUi()
    {
        string desc = string.Empty;
        desc +=
            $"데미지 : {(ServerData.skillServerTable.GetSkillDamagePer(skillTableData.Id, applySkillDamAbility: false) * 100f).ToString("F1")}% -> {(ServerData.skillServerTable.GetSkillDamagePer(skillTableData.Id, 1, applySkillDamAbility: false) * 100f).ToString("F1")}%\n";
        desc += $"기술 시전 속도 : {skillTableData.Cooltime}\n";
        desc += $"타겟수 : {skillTableData.Targetcount}\n";
        desc += $"히트수 : {skillTableData.Hitcount}\n";
        desc += $"범위 : {skillTableData.Targetrange}\n";
        //  desc += $"마나 소모 : {skillTableData.Usecost}\n";
        desc += $"각성 최대 : {skillTableData.Awakemaxnum}";
        //  desc += $"기술 타입 : {skillTableData.Skilltype}";
        skillAbility.SetText(desc);
    }

    private void Subscribe()
    {
        disposables.Clear();

        //스킬 각성시
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id].AsObservable()
            .Subscribe(WhenSkillAwake).AddTo(disposables);

        //스킬 레벨업시
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillLevel][skillTableData.Id].AsObservable()
            .Subscribe(WhenSkillUpgraded).AddTo(disposables);

        if (skillTableData.Skilltype == 0 || skillTableData.Skilltype == 1 || skillTableData.Skilltype == 2 ||
            skillTableData.Skilltype == 4)
        {
            ServerData.statusTable.GetTableData(StatusTable.Skill0_AddValue).AsObservable().Subscribe(e =>
            {
                WhenSkillUpgraded(
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillLevel][skillTableData.Id].Value);
            }).AddTo(disposables);

            ServerData.statusTable.GetTableData(StatusTable.Skill1_AddValue).AsObservable().Subscribe(e =>
            {
                WhenSkillUpgraded(
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillLevel][skillTableData.Id].Value);
            }).AddTo(disposables);

            ServerData.statusTable.GetTableData(StatusTable.Skill2_AddValue).AsObservable().Subscribe(e =>
            {
                WhenSkillUpgraded(
                    ServerData.skillServerTable.TableDatas[SkillServerTable.SkillLevel][skillTableData.Id].Value);
            }).AddTo(disposables);
        }

        var weaponData = TableManager.Instance.WeaponData[skillTableData.Awakeweaponidx];

        // DatabaseManager.weaponTable.TableDatas[weaponData.Stringid].amount.AsObservable().Subscribe(WhenAwakeWeaponAmountChanged).AddTo(disposables);
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].AsObservable()
            .Subscribe(WhenAwakeWeaponAmountChanged).AddTo(disposables);


        int hasCount = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value;
        int awakeNum = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id].Value;
        tutorialObject.gameObject.SetActive(hasCount > 0 && awakeNum == 0);
    }

    private void WhenAwakeWeaponAmountChanged(int amount)
    {
        int skillAwakeNum = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id]
            .Value;

        //최초에는 1개로 스킬 배울수있음.
        if (skillAwakeNum == 0)
        {
            awakeButtonDescription.SetText($"{amount}/{GameBalance.firstSkillAwakeNum}");
        }
        else
        {
            awakeButtonDescription.SetText($"{amount}/{skillTableData.Awakeweaponreqcount}");
        }
    }

    private void WhenSkillAwake(int awakeNum)
    {
        awakeDescription.SetText($"현재 각성 {awakeNum}회 기술 최대레벨 {(awakeNum) * GameBalance.SkillAwakePlusNum}");

        //var weaponData = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id];

        //WhenAwakeWeaponAmountChanged(ServerData.weaponTable.TableDatas[weaponData.Stringid].amount.Value);

        RefreshSkillLvText();

        equipButton.interactable = awakeNum != 0;

        if (((StatusType)skillTableData.Haseffecttype).IsPercentStat())
        {
            hasEffectDesc.SetText(
                $"보유효과 : {CommonString.GetStatusName((StatusType)skillTableData.Haseffecttype)} {Utils.SmallFloatToDecimalFloat(awakeNum * skillTableData.Haseffectvalue * 100f, 2)}%\n(기술 각성시 보유 효과가 증가합니다))");
        }
        else
        {
            hasEffectDesc.SetText(
                $"보유효과 : {CommonString.GetStatusName((StatusType)skillTableData.Haseffecttype)} {awakeNum * skillTableData.Haseffectvalue}\n(기술 각성시 보유 효과가 증가합니다))");
        }
    }

    private void WhenSkillUpgraded(int skillLevel)
    {
        RefreshSkillLvText();
    }

    private void RefreshSkillLvText()
    {
        var skillLevel = ServerData.skillServerTable.GetSkillCurrentLevel(skillTableData.Id);
        int maxLevel = ServerData.skillServerTable.GetSkillMaxLevel(skillTableData.Id);
        var addValue = 0f;

        if (skillTableData.SKILLCASTTYPE == SkillCastType.Player)
        {
            addValue = ServerData.statusTable.GetTableData(StatusTable.Skill0_AddValue).Value;
            addValue += ServerData.statusTable.GetTableData(StatusTable.Skill1_AddValue).Value;
            addValue += ServerData.statusTable.GetTableData(StatusTable.Skill2_AddValue).Value;
        }

        levelDescription.SetText(string.Format(lvTextFormat, skillLevel, maxLevel, addValue));
    }

    private void RefreshUpgradeButton()
    {
        var skillLevel = ServerData.skillServerTable.GetSkillCurrentLevel(skillTableData.Id);
        int maxLevel = ServerData.skillServerTable.GetSkillMaxLevel(skillTableData.Id);
        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.SkillPoint);
        int awakeNum = ServerData.skillServerTable.GetSkillAwakeNum(skillTableData.Id);

        levelUpButton.interactable = skillPoint.Value >= 0 && skillLevel < maxLevel;

        if (awakeNum == 0)
        {
            levelupButtonDescription.SetText("미습득");
        }
        else
        {
            if (levelUpButton.interactable)
            {
                levelupButtonDescription.SetText("레벨업");
            }
            else
            {
                levelupButtonDescription.SetText("최고레벨");
            }
        }
    }

    private Coroutine SyncRoutine;
    private WaitForSeconds syncWaitTime = new WaitForSeconds(2.0f);

    private void SyncServerRoutine()
    {
        if (SyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutine);
        }

        SyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutine());
    }

    public void OnClickSkillUpgradeButton()
    {
        int maxLevel = ServerData.skillServerTable.GetSkillMaxLevel(skillTableData.Id);
        int skillLevel = (int)ServerData.skillServerTable.GetSkillCurrentLevel(skillTableData.Id);

        if (skillLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("기술이 최대 레벨 입니다.");
            return;
        }

        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.SkillPoint);

        int currentSkillPoint = (int)skillPoint.Value;
        if (currentSkillPoint <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }

        SkillLevelUp(skillPoint);
    }

    public void OnClickSkillAllUpgradeButton()
    {
        int maxLevel = ServerData.skillServerTable.GetSkillMaxLevel(skillTableData.Id);
        int skillLevel = (int)ServerData.skillServerTable.GetSkillCurrentLevel(skillTableData.Id);

        if (skillLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("기술이 최대 레벨 입니다.");
            return;
        }

        var skillPoint = ServerData.statusTable.GetTableData(StatusTable.SkillPoint);


        var currentSkillPoint = skillPoint.Value;
        if (currentSkillPoint <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("기술포인트가 부족합니다.");
            return;
        }

        float upgradableNum = maxLevel - skillLevel;

        upgradableNum = Mathf.Min(currentSkillPoint, upgradableNum);

        for (int i = 0; i < upgradableNum; i++)
        {
            SkillLevelUp(skillPoint, i == upgradableNum - 1);
        }
    }

    private void SkillLevelUp(ReactiveProperty<float> skillPoint, bool updateUi = true)
    {
        //스킬포인트 감소
        skillPoint.Value--;

        //스킬 레벨 증가
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillLevel][skillTableData.Id].Value++;

        //서버 업데이트 요청
        SyncServerRoutine();

        //Ui갱신
        if (updateUi)
        {
            Initialize(skillTableData);
        }

        //   UiTutorialManager.Instance.SetClear(TutorialStep._5_SkillLevelUp);
    }

    private IEnumerator SyncDataRoutine()
    {
        yield return syncWaitTime;

        //데이터 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param statusParam = new Param();
        Param skillParam = new Param();

        //스킬포인트
        statusParam.Add(StatusTable.SkillPoint, ServerData.statusTable.GetTableData(StatusTable.SkillPoint).Value);
        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        //스킬레벨
        skillParam.Add(SkillServerTable.SkillLevel,
            ServerData.skillServerTable.TableDatas[SkillServerTable.SkillLevel].Select(e => e.Value).ToList());
        transactionList.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate,
            skillParam));

        ServerData.SendTransaction(transactionList);

        SyncRoutine = null;
    }

    public void OnClickAwakeButton()
    {
        // UiTutorialManager.Instance.SetClear(TutorialStep._10_GetSkill);

        int currentAwakeNum = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id]
            .Value;
        int maxAwakeNum = skillTableData.Awakemaxnum;

        if (currentAwakeNum >= maxAwakeNum)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 각성 입니다.");
            return;
        }

        int skillAmount = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id]
            .Value;
        //로컬 데이터 갱신
        if (currentAwakeNum != 0 && skillAmount < skillTableData.Awakeweaponreqcount)
        {
            PopupManager.Instance.ShowAlarmMessage("기술이 부족 합니다.");
            //
            return;
        }
        else if (currentAwakeNum == 0)
        {
            if (skillAmount <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage("기술이 부족 합니다.");
                return;
            }
        }

        //스킬북 차감 맨처음에는 1개만 차감
        if (currentAwakeNum == 0)
        {
            ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value -= 1;
        }
        else
        {
            ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][skillTableData.Id].Value -=
                skillTableData.Awakeweaponreqcount;
        }

        UiTutorialManager.Instance.SetClear(TutorialStep.LearnSkill);

        //각성 +1
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][skillTableData.Id].Value++;

        Initialize(skillTableData);

        //서버 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        //스킬 각성 횟수 증가
        Param skillParam = new Param();
        List<int> skillAWakeData = new List<int>();
        var currentAwakeData = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum];
        skillParam.Add(SkillServerTable.SkillAwakeNum, currentAwakeData.Select(e => e.Value).ToList());

        //스킬북 차감
        List<int> skillAmountSyncData = new List<int>();
        for (int i = 0; i < ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount].Count; i++)
        {
            skillAmountSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][i].Value);
        }

        skillParam.Add(SkillServerTable.SkillHasAmount, skillAmountSyncData);

        transactionList.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate,
            skillParam));


        ServerData.SendTransaction(transactionList);

        UpdateSkillAwakeText();

        //일일미션
        DailyMissionManager.UpdateDailyMission(DailyMissionKey.SkillAwake, 1);
    }

    private void OnDisable()
    {
        disposables.Dispose();
    }
}