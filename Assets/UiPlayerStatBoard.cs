using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiPlayerStatBoard : SingletonMono<UiPlayerStatBoard>
{
    [SerializeField] private TextMeshProUGUI descriptionBoard1;

    [SerializeField] private TextMeshProUGUI descriptionBoard2;

    [FormerlySerializedAs("slashDescriptionBoard")] [SerializeField] private TextMeshProUGUI slashDescriptionBoard1;
    [SerializeField] private TextMeshProUGUI slashDescriptionBoard2;

    [SerializeField] private TextMeshProUGUI fightPoint;

    [SerializeField] private RectTransform contentsTr;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        string description1 = $"<color=red>공격능력</color>\n";
        string description2 = $"<color=yellow>기타능력</color>\n";
        string description3 = string.Empty;
        string description4 = string.Empty;

        //전투력
        fightPoint.SetText($"무력 : {Utils.ConvertBigNum(PlayerStats.GetTotalPower())}");

        //공격력
        description1 +=
            $"{CommonString.GetStatusName(StatusType.AttackAdd)} : {Utils.ConvertBigNum(PlayerStats.GetBaseAttackPower())}\n";
        //공격력증가(%)
        description1 +=
            $"{CommonString.GetStatusName(StatusType.AttackAddPer)} : {Utils.ConvertBigNum(PlayerStats.GetBaseAttackAddPercentValue() * 100f)}\n";

        //크리티컬 확률(%)
        description1 +=
            $"{CommonString.GetStatusName(StatusType.CriticalProb)} : {Utils.ConvertBigNum(PlayerStats.GetCriticalProb() * 100f)}\n";
        //크리티컬 데미지(%)
        description1 +=
            $"{CommonString.GetStatusName(StatusType.CriticalDam)} : {Utils.ConvertBigNum(PlayerStats.CriticalDam() * 100f)}\n";
        //--
        //스킬 쿨타임 감소
        description1 +=
            $"{CommonString.GetStatusName(StatusType.SkillCoolTime)} : {Utils.ConvertBigNum(PlayerStats.GetSkillCoolTimeDecreaseValue() * 100f)}\n";

        //스킬 데미지 증가
        description1 +=
            $"{CommonString.GetStatusName(StatusType.SkillDamage)} : {Utils.ConvertBigNum(PlayerStats.GetSkillDamagePercentValue() * 100f)}\n";

        ////최소데미지 보정
        //description1 += $"{CommonString.GetStatusName(StatusType.DamBalance)} : +{PlayerStats.GetDamBalanceAddValue() * 100f}%\n";
        //description1 += $"데미지 범위(%) : {(DamageBalance.baseMinDamage + PlayerStats.GetDamBalanceAddValue()) * 100f}%~{DamageBalance.baseMaxDamage * 100}%\n";

        //체력
        description2 +=
            $"{CommonString.GetStatusName(StatusType.Hp)} : {Utils.ConvertBigNum(PlayerStats.GetOriginHp())}\n";
        //체력 증가
        description2 +=
            $"{CommonString.GetStatusName(StatusType.HpAddPer)} : {Utils.ConvertBigNum(PlayerStats.GetMaxHpPercentAddValue() * 100f)}\n";

        //초당체력회복
        description2 += $"{CommonString.GetStatusName(StatusType.HpRecover)} : {PlayerStats.GetHpRecover() * 100f}\n";
        description2 +=
            $"{CommonString.GetStatusName(StatusType.MagicStoneAddPer)} : {PlayerStats.GetMagicStonePlusValue() * 100f}\n";

        ////마력
        //description1 += $"{CommonString.GetStatusName(StatusType.Mp)} : {PlayerStats.GetOriginMp()}\n";

        ////마력 증가
        //description1 += $"{CommonString.GetStatusName(StatusType.MpAddPer)} : {PlayerStats.GetMaxMpPercentAddValue() * 100f}\n";

        ////초당마력회복
        //description1 += $"{CommonString.GetStatusName(StatusType.MpRecover)} : {PlayerStats.GetMpRecover() * 100f}\n";

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateGold).Value < 1)
        {
            //골드 추가 획득
            description2 +=
                $"{CommonString.GetStatusName(StatusType.GoldGainPer)} : {Utils.ConvertBigNum(PlayerStats.GetGoldPlusValue() * 100f)}\n";
            //금괴 추가 획득
        }
        else
        {
            description2 +=
                $"{CommonString.GetStatusName(StatusType.GoldBarGainPer)} : {Utils.ConvertBigNum(PlayerStats.GetGoldBarPlusValue() * 100f)}\n";
        }

        //경험치 추가 획득
        description2 +=
            $"{CommonString.GetStatusName(StatusType.ExpGainPer)} : {Utils.ConvertNum(PlayerStats.GetExpPlusValue_WithAllBuff() * 100f)}\n";

        //아이템 획득량
        description2 +=
            $"{CommonString.GetStatusName(StatusType.DropAmountAddPer)} : {PlayerStats.GetDropAmountAddValue()}\n";

        //이동속도
        description2 += $"{CommonString.GetStatusName(StatusType.MoveSpeed)} : {PlayerStats.GetMoveSpeedValue()}\n";

        //피해 감소
        description2 +=
            $"{CommonString.GetStatusName(StatusType.Damdecrease)} : {PlayerStats.GetDamDecreaseValue() * 100f}\n";

        //보스피해
        description1 +=
            $"{CommonString.GetStatusName(StatusType.BossDamAddPer)} : {PlayerStats.GetBossDamAddValue() * 100f}\n";

        //방어도 무시
        description1 +=
            $"{CommonString.GetStatusName(StatusType.IgnoreDefense)} : {Utils.ConvertBigNum(PlayerStats.GetIgnoreDefenseValue())}\n";

        //관통
        description1 +=
            $"{CommonString.GetStatusName(StatusType.PenetrateDefense)} : {(PlayerStats.GetPenetrateDefense() * 100f).ToString("F3")}\n";

        //타격수
        description1 +=
            $"{CommonString.GetStatusName(StatusType.SkillAttackCount)} : {PlayerStats.GetSkillHitAddValue()}\n";
        //타격수
        description1 +=
            $"요도 시전 속도 강화: {Utils.ConvertNum((PlayerSkillCaster.Instance.sealChargeCount.Value + PlayerSkillCaster.Instance.sealChargeCount2.Value + PlayerSkillCaster.Instance.sealChargeCount3.Value- 1) * 100)}%\n";
        //타격수
        description1 +=
            $"{CommonString.GetStatusName(StatusType.ReduceSealSwordSkillRequireCount)}: {Utils.ConvertNum(PlayerStats.GetReduceSealSwordSkillRequireCount())}\n";
        //방무 GetIgnoreDefenseValue
        
        //천공베기 확률
        description3 +=
            $"{CommonString.GetStatusName(StatusType.SuperCritical1Prob)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCriticalProb() * 100f)}\n";

    description3 +=
            $"크리티컬 1단계 {CommonString.GetStatusName(StatusType.SuperCritical16DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical16DamPer() * 100f)}\n";
        //타격수
        description3 +=
            $"크리티컬 2단계 {CommonString.GetStatusName(StatusType.SuperCritical1DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCriticalDamPer() * 100f)}\n";

        description2 +=
            $"{CommonString.GetStatusName(StatusType.MarbleAddPer)} : {PlayerStats.GetMarblePlusValue() * 100f}\n";

        description3 +=
            $"크리티컬 3단계 {CommonString.GetStatusName(StatusType.SuperCritical2DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical2DamPer() * 100f)}\n";
        
        //단전베기
        description3 +=
            $"크리티컬 4단계 {CommonString.GetStatusName(StatusType.SuperCritical8DamPer)} : {PlayerStats.GetSuperCritical8DamPer() * 100f}\n";
        
        description3 +=
            $"크리티컬 5단계 {CommonString.GetStatusName(StatusType.SuperCritical13DamPer)} : {PlayerStats.GetSuperCritical13DamPer() * 100f}\n";
         description3 +=
            $"크리티컬 6단계 {CommonString.GetStatusName(StatusType.SuperCritical18DamPer)} : {PlayerStats.GetSuperCritical18DamPer() * 100f}\n";

        
        description2 +=
            $"{CommonString.GetStatusName(StatusType.YoPowerGoodsGainPer)} : {PlayerStats.GetYoPowerGoodsGainValue() * 100f}\n";
        description2 +=
            $"{CommonString.GetStatusName(StatusType.TaegeukGoodsGainPer)} : {PlayerStats.GetTaegeukGoodsGainValue() * 100f}\n";
        
        description2 +=
            $"{CommonString.GetStatusName(StatusType.DecreaseBossHp)} : {PlayerStats.DecreaseBossHp() * 100f}\n";

       //int hellPlusSpawnNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.du).Value;

        int chunPlusSpawnNum = 0;



// #if  UNITY_EDITOR
//         int plusSpawnNum = 71;
// #else
        int plusSpawnNum = //GuildManager.Instance.GetGuildSpawnEnemyNum(GuildManager.Instance.guildLevelExp.Value) +
                           //hellPlusSpawnNum + 
                           // chunPlusSpawnNum +
                           PlayerStats.GetAddSummonYogui();
//#endif
        //지옥베기
        description3 +=
            $"크리티컬 7단계 {CommonString.GetStatusName(StatusType.SuperCritical3DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical3DamPer() * 100f)}\n";

        description2 += $"요괴 추가소환 : {plusSpawnNum}\n";
        //획득
        description2 += $"{CommonString.GetStatusName(StatusType.PeachGainPer)}: {Utils.ConvertNum(PlayerStats.GetPeachGainValue()*100)}\n";
        //획득
        description2 += $"{CommonString.GetStatusName(StatusType.HellGainPer)}: {Utils.ConvertNum(PlayerStats.GetHellGainValue()*100)}\n";
        //획득
        description2 += $"{CommonString.GetStatusName(StatusType.ChunGainPer)}: {Utils.ConvertNum(PlayerStats.GetChunGainValue()*100)}\n";
        //획득
        description2 += $"{CommonString.GetStatusName(StatusType.DokebiFireGainPer)}: {Utils.ConvertNum(PlayerStats.GetDokebiFireGainValue()*100)}\n";
        //수호환획득
        description2 += $"{CommonString.GetStatusName(StatusType.SuhoGainPer)}: {Utils.ConvertNum(PlayerStats.GetSuhoGainValue()*100)}\n";
        //여우불획득
        description2 += $"{CommonString.GetStatusName(StatusType.FoxRelicGainPer)}: {Utils.ConvertNum(PlayerStats.GetFoxRelicGainValue()*100)}\n";
        //도술꽃획득
        description2 += $"{CommonString.GetStatusName(StatusType.DosulGainPer)}: {Utils.ConvertNum(PlayerStats.GetDosulGainValue()*100)}\n";
        //명상획득량
        description2 += $"{CommonString.GetStatusName(StatusType.MeditationGainPer)}: {Utils.ConvertNum(PlayerStats.GetMeditationGainValue()*100)}\n";
        
        //천상베기
        description3 +=
            $"크리티컬 8단계 {CommonString.GetStatusName(StatusType.SuperCritical4DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical4DamPer() * 100f)}\n";

        //도깨비참수
        description3 +=
            $"크리티컬 9단계 {CommonString.GetStatusName(StatusType.SuperCritical5DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical5DamPer() * 100f)}\n";

        
        
        //수호베기
        description3 +=
            $"크리티컬 10단계 {CommonString.GetStatusName(StatusType.SuperCritical11DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical11DamPer() * 100f)}\n";

            
        
        //수호베기
        description3 +=
            $"크리티컬 11단계 {CommonString.GetStatusName(StatusType.SuperCritical14DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical14DamPer() * 100f)}\n";

        
        //신수베기
        description3 +=
            $"크리티컬 12단계 {CommonString.GetStatusName(StatusType.SuperCritical6DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical6DamPer() * 100f)}\n";

        //사흉베기
        description3 +=
            $"크리티컬 13단계 {CommonString.GetStatusName(StatusType.SuperCritical9DamPer)} : {PlayerStats.GetSuperCritical9DamPer() * 100f}\n";
        //금강베기
        description3 +=
            $"크리티컬 14단계 {CommonString.GetStatusName(StatusType.SuperCritical7DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical7DamPer() * 100f)}\n";

        description3 +=
            $"크리티컬 15단계 {CommonString.GetStatusName(StatusType.SuperCritical10DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical10DamPer() * 100f)}\n";
        description3 +=
            $"크리티컬 16단계 {CommonString.GetStatusName(StatusType.SuperCritical12DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical12DamPer() * 100f)}\n";
        description3 +=
            $"크리티컬 17단계 {CommonString.GetStatusName(StatusType.SuperCritical15DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical15DamPer() * 100f)}\n";
        description3 +=
            $"크리티컬 18단계 {CommonString.GetStatusName(StatusType.SuperCritical17DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical17DamPer() * 100f,1)}\n";

        description3 +=
            $"크리티컬 19단계 {CommonString.GetStatusName(StatusType.SuperCritical19DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical19DamPer() * 100f,1)}\n";

        description3 +=
            $"크리티컬 20단계 {CommonString.GetStatusName(StatusType.SuperCritical20DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical20DamPer() * 100f,1)}\n";

        description4 +=
            $"크리티컬 21단계 {CommonString.GetStatusName(StatusType.SuperCritical21DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical21DamPer() * 100f,1)}\n";

        description4 +=
            $"크리티컬 22단계 {CommonString.GetStatusName(StatusType.SuperCritical22DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical22DamPer() * 100f,1)}\n";

        description4 +=
            $"크리티컬 23단계 {CommonString.GetStatusName(StatusType.SuperCritical23DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical23DamPer() * 100f,1)}\n";

        description4 +=
            $"크리티컬 24단계 {CommonString.GetStatusName(StatusType.SuperCritical24DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical24DamPer() * 100f,1)}\n";

        description4 +=
            $"크리티컬 25단계 {CommonString.GetStatusName(StatusType.SuperCritical25DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical25DamPer() * 100f,1)}\n";

        description4 +=
            $"크리티컬 26단계 {CommonString.GetStatusName(StatusType.SuperCritical26DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical26DamPer() * 100f,1)}\n";

        description4 +=
            $"크리티컬 27단계 {CommonString.GetStatusName(StatusType.SuperCritical27DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical27DamPer() * 100f,1)}\n";

        description4 +=
            $"크리티컬 28단계 {CommonString.GetStatusName(StatusType.SuperCritical28DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical28DamPer() * 100f,1)}\n";

        description4 +=
            $"크리티컬 29단계 {CommonString.GetStatusName(StatusType.SuperCritical29DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical29DamPer() * 100f,1)}\n";

        description4 +=
            $"크리티컬 30단계 {CommonString.GetStatusName(StatusType.SuperCritical30DamPer)} : {Utils.ConvertNum(PlayerStats.GetSuperCritical30DamPer() * 100f,1)}\n";

        
            //도술
            description1 +=
                $"{CommonString.GetStatusName(StatusType.DosulDamPer)} : {Utils.ConvertNum(PlayerStats.GetDosulDamPer() * 100f)}\n";
            //궁극기술타수
            description1 +=
                $"{CommonString.GetStatusName(StatusType.AddVisionSkillUseCount)} : {Utils.ConvertNum(PlayerStats.GetAddVisionSkillUseCount())}\n";
            //요도강화 타수
            description1 +=
                $"{CommonString.GetStatusName(StatusType.AddSealSwordSkillHitCount)} : {Utils.ConvertNum(PlayerStats.GetAddSealSwordSkillHitCount())}\n";
            //도술쿨감
            description1 +=
                $"{CommonString.GetStatusName(StatusType.ReduceDosulSkillCoolTime)} : {Utils.ConvertNum(PlayerStats.GetReduceDosulSkillCoolTime())}\n";
            //궁극기술
            description1 +=
                $"{CommonString.GetStatusName(StatusType.EnhanceVisionSkill)} : {Utils.ConvertNum(PlayerStats.GetEnhanceVisionSkill() * 100f)}\n";

            descriptionBoard1.SetText(description1);

        descriptionBoard2.SetText(description2);

        slashDescriptionBoard1.SetText(description3);
        slashDescriptionBoard2.SetText(description4);
    }
}