using System;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//삭제하거나순서절대바꾸면안됨
public enum StatusType
{
    AttackAddPer, // icon
    CriticalProb, //icon
    CriticalDam, //
    SkillCoolTime, //icon
    SkillDamage, //icon
    MoveSpeed,
    DamBalance,
    HpAddPer, //icon
    MpAddPer, //icon
    GoldGainPer, //icon
    ExpGainPer, //icon
    AttackAdd, //icon
    Hp, //icon
    Mp, //icon
    HpRecover, //icon
    MpRecover, //icon
    MagicStoneAddPer, //icon
    Damdecrease,
    IgnoreDefense,
    DashCount,
    DropAmountAddPer,
    BossDamAddPer,
    SkillAttackCount,
    PenetrateDefense,
    SuperCritical1Prob,
    SuperCritical1DamPer,
    MarbleAddPer,
    SuperCritical2DamPer, //필멸

    //Smith
    growthStoneUp,
    WeaponHasUp,
    
    NorigaeHasUp,
    PetEquipHasUp,
    PetEquipProbUp,
    DecreaseBossHp,
    OneYearBuff,
    SuperCritical3DamPer, //지옥
    SuperCritical4DamPer, //천상
    MonthBuff,
    FlowerHasValueUpgrade,
    SuperCritical5DamPer, //도깨비
    DokebiFireHasValueUpgrade,
    
    HellHasValueUpgrade=41,
    SuperCritical6DamPer, //신수
    SuperCritical7DamPer, //금강베기
    SumiHasValueUpgrade,
    SuperCritical8DamPer, //단전(?)베기 ->경락마사지에 가`됨 
    SuperCritical9DamPer, //흉수베기 
    SuperCritical10DamPer, //도적베기 
    NorigaeGoldAbilUp, //노리개 장착효과중 기본무공 효과 버프 
    SuperCritical11DamPer, //수호동물베기 
    SuperCritical12DamPer, //심베기 S
    
    SuperCritical13DamPer, //중단전베기 
    TreasureHasValueUpgrade, // 보물당 섬광베기
    SuperCritical14DamPer, //여우베기 
    SuperCritical15DamPer, //신선베기 
    SuperCritical16DamPer, //태극베기 
    DarkHasValueUpgrade=56, //심연보물당 암흑베기
    GoldBarGainPer, //금괴 획득량 증가
    SuperCritical17DamPer, //영혼베기
    SuperCritical18DamPer, //상단전
    SuperCritical19DamPer, //귀살베기
    
    SuperCritical20DamPer, //천구베기
    SealSwordDam=62, //요도 피해량 증가
    DosulDamPer=63, //도술 피해량 증가
    PeachGainPer, //복숭아획득량 증가
    HellGainPer, //지옥석 획득량 증가
    ChunGainPer, //천계꽃 획득량 증가
    DokebiFireGainPer, //도꺠비불 획득량 증가
    SealAttackSpeed, //요도 기술시전속도증가
    SuhoGainPer, //수호환 소탕량증가
    FoxRelicGainPer, //여우불 소탕량 증가
    
    DosulGainPer, //도술꽃 소탕량 증가ㅋ
    AddSummonYogui, //요괴 추가소환
    PeachAbilUpgradePer, //복숭아 효과 강화
    SinsunHasValueUpgrade=74, //신선보옥당 신선베기
    HyunsangHasValueUpgrade=75, //귀멸증표당 귀멸베기
    
    SuperCritical21DamPer, //초월베기
    SuperCritical8AddDam,
    SuperCritical13AddDam,
    SuperCritical18AddDam,
    SuperCritical22DamPer, //진 귀살베기
    
    StageRelicUpgrade, //유물 복원 강화
    MeditationGainPer, //내면세계 소탕량 증가
    SuperCritical23DamPer, //심상베기
    AddVisionSkillUseCount, //비전스킬횟수증가
    AddSealSwordSkillHitCount, //요도 타수강화
    
    ReduceDosulSkillCoolTime, //도술재사용시간
    EnhanceVisionSkill=87, //비전스킬효과 강화
    SuperCritical24DamPer,  //용베기
    SuperCritical25DamPer, //요력개방
    EnhanceTaegeukCritical, //태극베기증폭

    SuperCritical26DamPer, //진 요도베기

    ReduceSealSwordSkillRequireCount, //요도 시전 타수 감소
    EnhanceSuhoCritical, //수호베기증폭
    EnhanceSinsuCritical, //신수베기증폭
    EnhanceHyungsuCritical, //흉수베기증폭
    EnhanceSoulCritical, //영혼베기증폭
    EnhanceChunguCritical, //천구베기증폭
    YoPowerGoodsGainPer, //요석 획득 증가
    DragonHasValueUpgrade,//용인비늘당 개수증가
    SuperCritical27DamPer,//무공피해
    SuperCritical28DamPer=101,//파도베기
    SuperCritical29DamPer,//비무 피해(%)
    TaegeukGoodsGainPer,//태극 획득 증가
    EnhanceTransCritical,//초월증폭
    SuperCritical30DamPer,//신력피해(%)
    BigiDamPer=106,//비기 추가 피해량 증가(%)
    DragonPalaceHasValueUpgrade,//용궁 재화당 개수증가
    SuperCritical31DamPer,//협동피해(%)
    EnhanceSP,//검기 능력치 효과(%)
    SuperCritical32DamPer,//무림피해(%)
    SasinsuGoodsGainPer,
    SuperCritical33DamPer=112,//극혈 피해(%)
    SuperCritical34DamPer,//보스 피해(%)
    MurimHasValueUpgrade,//극락 재화당 개수증가
    EnhanceGwisalCritical,//귀살증폭
    EnhanceSealSword,//요도증폭
    EnhanceDosul,//도술증폭
    EnhanceVision,//궁극증폭
    DBTHasValueUpgrade,//무림구슬
    DokChimHasValueUpgrade=120,
    SuperCritical35DamPer=121,
    SuperCritical36DamPer=122,//해탈피해
    SuperCritical37DamPer,//보옥
    SuperCritical38DamPer,//차원 신규 베기
    YOTHasValueUpgrade,//업화 개수
    EnhanceAttack,//공격력 증폭
    SuperCritical39DamPer=127,
    CSTHasValueUpgrade=128,
}

public enum DimensionStatusType
{
    None = -1,
    BaseAttackDam=0,
    AttackAddPer,
    BaseSkillDam,
    AddSkillDamPer,
    ReduceSkillCoolTimePer,
    AddHp,
    CubeGainPer,
    EssenceGainPer,
}

public static class PlayerStats
{
    public static double GetTotalPower()
    {
        double baseAttack = GetBaseAttackPower();
        double baseAttackPer = GetBaseAttackAddPercentValue();
        double enhanceBaseAttackPer = GetEnhanceAttack();
        double criProb = GetCriticalProb();
        double criDam = CriticalDam();
        double coolTIme = GetSkillCoolTimeDecreaseValue();
        double skillDam = GetSkillDamagePercentValue();
        double hpBase = GetMaxHp();
        double hpAddPer = GetMaxHpPercentAddValue();
        double mpBase = GetMaxMp();
        double mpAddPer = GetMaxMpPercentAddValue();

        double ignoreDefense = GetIgnoreDefenseValue();
        double decreaseDam = GetDamDecreaseValue();
        double skillAttackCount = GetSkillHitAddValue();
        double penetration = GetPenetrateDefense();
        double superCriticalProb = GetSuperCriticalProb();

        double taeguk = GetSuperCritical16DamPer();
        double feelMulDam = GetSuperCritical2DamPer();
        double gyungRock = GetSuperCritical8DamPer();
        double gyungRock2 = GetSuperCritical13DamPer();
        
        double gyungRock3 = GetSuperCritical18DamPer();
        double hellDam = GetSuperCritical3DamPer();
        double chunSangDam = GetSuperCritical4DamPer();
        double dokebiDam = GetSuperCritical5DamPer();
        double suho = GetSuperCritical11DamPer();
        
        double fox = GetSuperCritical14DamPer();
        double sinsuDam = GetSuperCritical6DamPer();
        double saHung = GetSuperCritical9DamPer();
        double sumiDam = GetSuperCritical7DamPer();
        double doJuk = GetSuperCritical10DamPer();

        double dark = GetSuperCritical12DamPer();
        double sinsun = GetSuperCritical15DamPer();
        double soul = GetSuperCritical17DamPer();
        double gwisal = GetSuperCritical19DamPer();
        double chungu = GetSuperCritical20DamPer();
        double trans = GetSuperCritical21DamPer();
        double jingwisal = GetSuperCritical22DamPer();
        double simsang = GetSuperCritical23DamPer();
        double dragon = GetSuperCritical24DamPer();
        double yoPower = GetSuperCritical25DamPer();
        double jinYodo = GetSuperCritical26DamPer();
        double mugong = GetSuperCritical27DamPer();
        double wave = GetSuperCritical28DamPer();
        double bimu = GetSuperCritical29DamPer();
        double sinpower = GetSuperCritical30DamPer();
        double munha = GetSuperCritical31DamPer();
        double murim = GetSuperCritical32DamPer();
        double hyul = GetSuperCritical33DamPer();
        double df = GetSuperCritical34DamPer();
        double yo = GetSuperCritical35DamPer();
        double haetal = GetSuperCritical36DamPer();
        double bo = GetSuperCritical37DamPer();
        double di = GetSuperCritical38DamPer();
        double cs = GetSuperCritical39DamPer();

        double totalPower =
            ((baseAttack + baseAttack * baseAttackPer*(1+enhanceBaseAttackPer))
             * (Mathf.Max((float)criProb, 0.01f) * 100f * Mathf.Max((float)criDam, 0.01f))
             * (Mathf.Max((float)skillDam, 0.01f) * 100f)
             * (Mathf.Max((float)coolTIme, 0.01f)) * 100f)
            + ((hpBase + hpBase * hpAddPer) + (mpBase + mpBase * mpAddPer))
            + ((baseAttack + baseAttack * baseAttackPer)
               * (Mathf.Max((float)ignoreDefense, 0.01f)) * 100f
               * (Mathf.Max((float)decreaseDam, 0.01f)) * 100f
               * (Mathf.Max((float)skillAttackCount, 0.01f)) * 100f
               * (Mathf.Max((float)penetration, 0.01f)) * 100f
            );

        totalPower += totalPower * GetSuperCriticalDamPer() * superCriticalProb;

        totalPower += totalPower * taeguk;
        totalPower += totalPower * feelMulDam;
        totalPower += totalPower * gyungRock;
        totalPower += totalPower * gyungRock2;
        totalPower += totalPower * gyungRock3;
        totalPower += totalPower * hellDam;
        totalPower += (totalPower * chunSangDam);
        totalPower += (totalPower * dokebiDam);
        totalPower += (totalPower * suho);
        totalPower += (totalPower * fox);
        totalPower += (totalPower * sinsuDam);
        totalPower += (totalPower * saHung);
        totalPower += (totalPower * sumiDam);
        totalPower += (totalPower * doJuk);
        totalPower += (totalPower * dark);
        totalPower += (totalPower * sinsun);
        totalPower += (totalPower * soul);
        totalPower += (totalPower * gwisal);
        totalPower += (totalPower * chungu);
        totalPower += (totalPower * trans);
        totalPower += (totalPower * jingwisal);
        totalPower += (totalPower * simsang);
        totalPower += (totalPower * dragon);
        totalPower += (totalPower * yoPower);
        totalPower += (totalPower * jinYodo);
        totalPower += (totalPower * mugong);
        totalPower += (totalPower * wave);
        totalPower += (totalPower * bimu);
        totalPower += (totalPower * sinpower);
        totalPower += (totalPower * munha);
        totalPower += (totalPower * murim);
        totalPower += (totalPower * hyul);
        totalPower += (totalPower * df);
        totalPower += (totalPower * yo);
        totalPower += (totalPower * haetal);
        totalPower += (totalPower * bo);
        totalPower += (totalPower * di);
        totalPower += (totalPower * cs);

        //     float totalPower =
        //((baseAttack + baseAttack * baseAttackPer)
        // * (Mathf.Max(criProb, 0.01f) * 100f * Mathf.Max(criDam, 0.01f))
        // * (Mathf.Max(skillDam, 0.01f) * 100f)
        // * (Mathf.Max(coolTIme, 0.01f)) * 100)
        // + ((hpBase + hpBase * hpAddPer) + (mpBase + mpBase * mpAddPer));

        return totalPower * 0.01f;
    }

    public static double GetDimensionTotalPower()
    {
        double baseAttack = GetDimensionBaseAttackDam();
        double baseAttackPer = GetDimensionAttackAddPer();
        double addSkillDamPer = GetDimensionAddSkillDamPer();
        double reduceSkillCoolTime = GetDimensionReduceSkillCoolTime();

        double totalPower = baseAttack * (1 + baseAttackPer) * (1 + addSkillDamPer) / (1 - reduceSkillCoolTime);


        return totalPower;
    }
    public static float GetMoveSpeedValue()
    {
        float ret = 0f;
        ret += GetMarbleValue(StatusType.MoveSpeed);

        return ret;
    }

    public static float GetDropAmountAddValue()
    {
        float ret = 0f;

        ret += GetMarbleValue(StatusType.DropAmountAddPer);

        return ret;
    }

    public static float GetDamDecreaseValue()
    {
        float ret = 0f;

        ret += GetMarbleValue(StatusType.Damdecrease);
        ret += GetMagicBookHasPercentValue(StatusType.Damdecrease);
        ret += GetSinsuEquipEffect(StatusType.Damdecrease);
        ret += GetRelicHasEffect(StatusType.Damdecrease);
        return ret;
    }

    public static float GetBossDamAddValue()
    {
        float ret = 0f;

        ret += GetMarbleValue(StatusType.BossDamAddPer);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.BossDamage_memory);
        ret += GetMagicBookHasPercentValue(StatusType.BossDamAddPer);

        return ret;
    }

    public static int GetSkillHitAddValue()
    {
        int ret = 0;

        ret += (int)GetMarbleValue(StatusType.SkillAttackCount);
        ret += (int)GetMagicBookHasPercentValue(StatusType.SkillAttackCount);
        ret += (int)GetSinsuEquipEffect(StatusType.SkillAttackCount);

        return ret;
    }

    public static float sogulGab = 20f;
    public static float sogulValuePerGab = 0.5f;
    public static float baekPassiveLock = 200;

    public static float GetPassiveSkillValue(StatusType statusType)
    {
        float ret = 0f;

        var tableData = TableManager.Instance.PassiveSkill.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Abilitytype != (int)statusType) continue;

            var serverData = ServerData.passiveServerTable.TableDatas[tableData[i].Stringid];

            int level = serverData.level.Value;


            if (level != 0)
            {
                ret += level * tableData[i].Abilityvalue;
            }
        }

        ret = ret + ret * GetPassiveAdvanceValue();


        return ret;
    }
    public static float GetPassiveSkillValue(StatusType statusType, int avoidDuplicateIdx)
    {
        float ret = 0f;

        var tableData = TableManager.Instance.PassiveSkill.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Abilitytype != (int)statusType) continue;

            if(tableData[i].Avoidduplicationidx != avoidDuplicateIdx) continue;
            
            var serverData = ServerData.passiveServerTable.TableDatas[tableData[i].Stringid];

            int level = serverData.level.Value;


            if (level != 0)
            {
                ret += level * tableData[i].Abilityvalue;
            }
        }

        ret = ret + ret * GetPassiveAdvanceValue();


        return ret;
    }

    public static float GetPassiveSkill2Value(StatusType statusType)
    {
        float ret = 0f;

        var tableData = TableManager.Instance.PassiveSkill2.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Abilitytype != (int)statusType) continue;

            var serverData = ServerData.passive2ServerTable.TableDatas[tableData[i].Stringid];

            int level = serverData.level.Value;


            if (level != 0)
            {
                ret += level * tableData[i].Abilityvalue;
            }
        }


        return ret;
    }
    public static float GetPassiveSkill3Value(StatusType statusType)
    {
        float ret = 0f;

        var tableData = TableManager.Instance.PassiveSkill3.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Abilitytype != (int)statusType) continue;

            var serverData = ServerData.passive3ServerTable.TableDatas[tableData[i].Stringid];

            int level = serverData.level.Value;


            if (level != 0)
            {
                ret += level * tableData[i].Abilityvalue;
            }
        }


        return ret;
    }
    public static float GetCostumeSpecialAbilityValue(StatusType statusType)
    {
        float ret = 0f;

        var tableData = TableManager.Instance.CostumeSpecialAbility.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Abilitytype != (int)statusType) continue;

            var serverData = ServerData.costumeSpecialAbilityServerTable.TableDatas[tableData[i].Stringid];

            int level = serverData.level.Value;


            if (level != 0)
            {
                ret += level * tableData[i].Abilityvalue;
            }
        }


        return ret;
    }

    public static float GetPassiveAdvanceValue()
    {
        int floor = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value;

        float gap = floor / sogulGab;

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateBackGui).Value > 0)
        {
            return gap * sogulValuePerGab * GameBalance.BackguiGraduatePlusValue;
        }
        else
        {
            return gap * sogulValuePerGab;
            
        }
        
    }

    #region AttackPower

    public static float GetBaseAttackPower()
    {
        float ret = 0f;

        ret += ServerData.statusTable.GetStatusValue(StatusTable.AttackLevel_Gold);
        ret += ServerData.petTable.GetStatusValue(StatusType.AttackAdd);
        ret += GetWeaponEquipPercentValue(StatusType.AttackAdd);
        ret += GetSkillCollectionValue(StatusType.AttackAdd);
        ret += GetPassiveSkillValue(StatusType.AttackAdd);
        ret += GetMarbleValue(StatusType.AttackAdd);
        ret += GetSkillHasValue(StatusType.AttackAdd);
        ret += GetYomulUpgradeValue(StatusType.AttackAdd);
        //ret += GetTitleAbilValue(StatusType.AttackAdd);
        ret += GetTitleLevelAbilValue(StatusType.AttackAdd);

        ret += GetBuffValue(StatusType.AttackAdd);
        ret += GetRelicHasEffect(StatusType.AttackAdd);

        ret += GetStageRelicHasEffect(StatusType.AttackAdd);
        ret += GetSonAbilHasEffect(StatusType.AttackAdd);
        ret += GetCaveBeltAttackAdd();

        ret += GetGumGiAttackValue();
        ret += GetWeaponCollectionHasValue(StatusType.AttackAdd);
        
        ret += GetGuimoonHasEffect1(StatusType.AttackAdd);
        
        return ret;
    }

    public static float GetGumGiAttackValue()
    {
        int idx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponEnhance].Value;

        float ret = (float)TableManager.Instance.gumGiTable.dataArray[idx].Abilvalue;


        if (ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value >=
            TableManager.Instance.gumGiTable.dataArray[200].Require)
        {
            int over200 = Mathf.Max(0,
                ((int)ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value -
                 TableManager.Instance.gumGiTable.dataArray[200].Require) / 50000);
            ret += over200 * GameBalance.gumgiAttackValue200;
        }

        return (ret + ret * GetGumgiAbilAddValue()) * (GetEnhanceSP());
    }

    public static float GetGumIgDefenseValue()
    {
        int idx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponEnhance].Value;

        float ret = (float)TableManager.Instance.gumGiTable.dataArray[idx].Abilvalue2;

        if (ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value >=
            TableManager.Instance.gumGiTable.dataArray[200].Require)
        {
            int over200 = Mathf.Max(0,
                ((int)ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value -
                 TableManager.Instance.gumGiTable.dataArray[200].Require) / 50000);
            ret += over200 * GameBalance.gumgiDefenseValue200;
        }

        return (ret + ret * GetGumgiAbilAddValue()) * (GetEnhanceSP());
    }

    public static float GetCollectionAbilValue(StatusType type)
    {
        var enemyTable = TableManager.Instance.EnemyTable.dataArray;
        float ret = 0f;

        for (int i = 0; i < enemyTable.Length; i++)
        {
            if ((StatusType)enemyTable[i].Collectionabiltype == type)
            {
                ret += ServerData.collectionTable.GetCollectionAbilValue(enemyTable[i]);
            }
        }

        return ret;
    }


    public static float GetBaseAttackAddPercentValue()
    {
        float ret = 0f;

        ret += ServerData.statusTable.GetStatusValue(StatusTable.IntLevelAddPer_StatPoint);
        ret += GetWeaponHasPercentValue(StatusType.AttackAddPer);
        ret += GetMagicBookHasPercentValue(StatusType.AttackAddPer);
        ret += GetCostumeAttackPowerValue();
        ret += GetSkillCollectionValue(StatusType.AttackAddPer);
        ret += GetPassiveSkillValue(StatusType.AttackAddPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.AttackAddPer);
        ret += GetMarbleValue(StatusType.AttackAddPer);
        ret += GetStageRelicHasEffect(StatusType.AttackAddPer);
        ret += GetSonAbilHasEffect(StatusType.AttackAddPer);
        ret += GetAsuraAbilValue(StatusType.AttackAddPer);
        ret += GetGuildPetEffect(StatusType.AttackAddPer);
        ret += GetMaskAttackAddPerDam();
        ret += GetRelicReleaseValue();
        ret += GetHellAbilHasEffect(StatusType.AttackAddPer);
        ret += GetChunAbilHasEffect(StatusType.AttackAddPer);
        ret += GetDokebiFireAbilHasEffect(StatusType.AttackAddPer);
        ret += GetMagicBookCollectionHasValue(StatusType.AttackAddPer);
        ret += GetGuimoonHasEffect1(StatusType.AttackAddPer);

        return ret;
    }

    public static ObscuredFloat relicReleaseValue = 30000f;

    public static float GetRelicReleaseValue()
    {
        int divideNum = (int)(ServerData.userInfoTable.TableDatas[UserInfoTable.usedRelicTicketNum].Value / 1000f);
        return relicReleaseValue * divideNum;
    }

    public static float GetCostumeAttackPowerValue()
    {
        float ret = ServerData.costumeServerTable.GetCostumeAbility(StatusType.AttackAddPer);
        return ret;
    }

    public static float GetCalculatedAttackPower()
    {
        float ret = 0f;

        float baseAttackPower = GetBaseAttackPower();

        float baseAttackAddPercentValue = GetBaseAttackAddPercentValue();
        
        float baseAttackEnhancePercentValue = GetEnhanceAttack();

        ret += baseAttackPower;

        ret += baseAttackPower * baseAttackAddPercentValue*(1+baseAttackEnhancePercentValue);

        return ret;
    }

    public static float GetWeaponEquipPercentValue(StatusType type)
    {
        int equipId = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;

        using var e = TableManager.Instance.WeaponData.GetEnumerator();

        float ret = 0f;
        while (e.MoveNext())
        {
            if (e.Current.Value.Id != equipId) continue;
            if (TableManager.Instance.WeaponEffectDatas.TryGetValue(e.Current.Value.Weaponeffectid,
                    out var effectData) == false) continue;

            int currentLevel = ServerData.weaponTable.GetWeaponData(e.Current.Value.Stringid).level.Value;

            if (effectData.Equipeffecttype1 == (int)type)
            {
                ret += effectData.Equipeffectbase1;
                ret += currentLevel * effectData.Equipeffectvalue1;
            }

            if (effectData.Equipeffecttype2 == (int)type)
            {
                ret += effectData.Equipeffectbase2;
                ret += currentLevel * effectData.Equipeffectvalue2;
            }

            break;
        }

        return ret;
    }

    public static float GetWeaponHasPercentValue(StatusType type)
    {
        using var e = TableManager.Instance.WeaponData.GetEnumerator();

        float ret = 0f;
        while (e.MoveNext())
        {
            if (TableManager.Instance.WeaponEffectDatas.TryGetValue(e.Current.Value.Weaponeffectid,
                    out var effectData) == false) continue;

            var weaponServertable = ServerData.weaponTable.TableDatas[e.Current.Value.Stringid];

            if (weaponServertable.hasItem.Value == 0) continue;

            int currentLevel = ServerData.weaponTable.GetWeaponData(e.Current.Value.Stringid).level.Value;

            if (effectData.Haseffecttype1 == (int)type)
            {
                ret += effectData.Haseffectbase1;
                ret += currentLevel * effectData.Haseffectvalue1;
            }

            if (effectData.Haseffecttype2 == (int)type)
            {
                ret += effectData.Haseffectbase2;
                ret += currentLevel * effectData.Haseffectvalue2;
            }
        }

        if (ActiveSmithValue(type))
        {
            ret = ret * (GetSmithValue(StatusType.WeaponHasUp)+GetCostumeSpecialAbilityValue(StatusType.WeaponHasUp));
        }
        else
        {
        }


        return ret;
    }

    public static float GetWeaponCollectionHasValue(StatusType type)
    {
        var tableData = TableManager.Instance.WeaponTable.dataArray;

        var serverData = ServerData.weaponTable.TableDatas;

        float ret = 0f;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].WEAPONTYPE == WeaponType.Basic) continue;
            if (tableData[i].WEAPONTYPE == WeaponType.View) continue;
            if (tableData[i].WEAPONTYPE == WeaponType.RecommendView) continue;
            if (serverData[tableData[i].Stringid].hasItem.Value == 0) continue;

            if ((StatusType)tableData[i].Collectioneffecttype == type)
            {
                ret += tableData[i].Collectioneffectvalue;
            }
        }

        return ret;
    }

    public static int GetTransEquipmentCount()
    {
        int ret = 0;

        var weaponData = TableManager.Instance.WeaponTable.dataArray;

        var weaponServerData = ServerData.weaponTable.TableDatas;
        
        for (int i = 0; i < weaponData.Length; i++)
        {
            if (weaponData[i].WEAPONTYPE != WeaponType.Normal) continue;
            if (weaponServerData[weaponData[i].Stringid].hasItem.Value == 0) continue;
            if (weaponServerData[weaponData[i].Stringid].trans.Value > 0)
            {
                ret ++;
            }
        }
        var magicBookData = TableManager.Instance.MagicBookTable.dataArray;

        var magicBookServerData = ServerData.magicBookTable.TableDatas;


        for (int i = 0; i < magicBookData.Length; i++)
        {
            if (magicBookData[i].MAGICBOOKTYPE != MagicBookType.Normal) continue;
            if (magicBookServerData[magicBookData[i].Stringid].hasItem.Value == 0) continue;

            
            
            if (magicBookServerData[magicBookData[i].Stringid].trans.Value > 0)
            {
                ret ++;
            }
        }
        return ret;
    }
    public static float GetWeaponTransHasValue(StatusType type)
    {
        var tableData = TableManager.Instance.WeaponTable.dataArray;

        var serverData = ServerData.weaponTable.TableDatas;

        float ret = 0f;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].WEAPONTYPE != WeaponType.Normal) continue;
            if (serverData[tableData[i].Stringid].hasItem.Value == 0) continue;

            
            
            if ((StatusType)tableData[i].Transeffecttype == type&&serverData[tableData[i].Stringid].trans.Value > 0)
            {
                ret += tableData[i].Transeffectvalue;
            }
        }

        return ret;
    }

    public static float GetMagicBookTransHasValue(StatusType type)
    {
        var tableData = TableManager.Instance.MagicBookTable.dataArray;

        var serverData = ServerData.magicBookTable.TableDatas;

        float ret = 0f;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].MAGICBOOKTYPE != MagicBookType.Normal) continue;
            if (serverData[tableData[i].Stringid].hasItem.Value == 0) continue;

            
            
            if ((StatusType)tableData[i].Transeffecttype == type&&serverData[tableData[i].Stringid].trans.Value > 0)
            {
                ret += tableData[i].Transeffectvalue;
            }
        }

        return ret;
    }

    public static float GetRingCollectionHasValue(StatusType type)
    {
        var tableData = TableManager.Instance.NewGachaTable.dataArray;

        var serverData = ServerData.newGachaServerTable.TableDatas;

        float ret = 0f;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].RINGTYPE == RingType.Basic) continue;
            if (tableData[i].RINGTYPE == RingType.View) continue;
            if (serverData[tableData[i].Stringid].hasItem.Value == 0) continue;

            if ((StatusType)tableData[i].Collectioneffecttype == type)
            {
                ret += tableData[i].Collectioneffectvalue;
            }
        }

        return ret;
    }

    public static float GetSealSwordCollectionHasValue(StatusType type)
    {
        var tableData = TableManager.Instance.sealSwordTable.dataArray;

        var serverData = ServerData.sealSwordServerTable.TableDatas;

        float ret = 0f;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (serverData[tableData[i].Stringid].hasItem.Value == 0) continue;

            if ((StatusType)tableData[i].Collectioneffecttype == type)
            {
                ret += tableData[i].Collectioneffectvalue;
            }
        }

        return ret;
    }

    public static float GetMagicBookCollectionHasValue(StatusType type)
    {
        var tableData = TableManager.Instance.MagicBookTable.dataArray;

        var serverData = ServerData.magicBookTable.TableDatas;

        float ret = 0f;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].MAGICBOOKTYPE == MagicBookType.Basic) continue;
            if (tableData[i].MAGICBOOKTYPE == MagicBookType.View) continue;
            if (serverData[tableData[i].Stringid].hasItem.Value == 0) continue;

            if ((StatusType)tableData[i].Collectioneffecttype == type)
            {
                ret += tableData[i].Collectioneffectvalue;
            }
        }

        return ret;
    }

    public static float GetMagicBookEquipPercentValue(StatusType type)
    {
        using var e = TableManager.Instance.MagicBoocDatas.GetEnumerator();

        int equipId = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;

        float ret = 0f;
        while (e.MoveNext())
        {
            if (e.Current.Value.Id != equipId) continue;
            if (TableManager.Instance.WeaponEffectDatas.TryGetValue(e.Current.Value.Magicbookeffectid,
                    out var effectData) == false) continue;

            int currentLevel = ServerData.magicBookTable.GetMagicBookData(e.Current.Value.Stringid).level.Value;

            if (effectData.Equipeffecttype1 == (int)type)
            {
                ret += effectData.Equipeffectbase1;
                ret += currentLevel * effectData.Equipeffectvalue1;
            }

            if (effectData.Equipeffecttype2 == (int)type)
            {
                ret += effectData.Equipeffectbase2;
                ret += currentLevel * effectData.Equipeffectvalue2;
            }

            break;
        }

        return ret;
    }


    private static void ResetMagicBookHas()
    {
        magicBookHasValue.Clear();
    }

    public static float GetMagicBookHasPercentValue(StatusType type)
    {
        float ret = 0f;

        if (magicBookHasValue.ContainsKey(type))
        {
            ret = magicBookHasValue[type];
        }
        else
        {
            using var e = TableManager.Instance.MagicBoocDatas.GetEnumerator();


            while (e.MoveNext())
            {
                if (TableManager.Instance.WeaponEffectDatas.TryGetValue(e.Current.Value.Magicbookeffectid,
                        out var effectData) == false) continue;

                var magieBookServerData = ServerData.magicBookTable.TableDatas[e.Current.Value.Stringid];

                if (magieBookServerData.hasItem.Value == 0) continue;

                int currentLevel = ServerData.magicBookTable.GetMagicBookData(e.Current.Value.Stringid).level.Value;

                if (effectData.Haseffecttype1 == (int)type)
                {
                    ret += effectData.Haseffectbase1;
                    ret += currentLevel * effectData.Haseffectvalue1;
                }

                if (effectData.Haseffecttype2 == (int)type)
                {
                    ret += effectData.Haseffectbase2;
                    ret += currentLevel * effectData.Haseffectvalue2;
                }

                if (effectData.Haseffecttype3 == (int)type)
                {
                    ret += effectData.Haseffectbase3;
                    ret += currentLevel * effectData.Haseffectvalue3;
                }
            }

            magicBookHasValue.Add(type, ret);
        }


        if (ActiveSmithValue(type))
        {
            ret = ret * (GetSmithValue(StatusType.NorigaeHasUp)+GetCostumeSpecialAbilityValue(StatusType.NorigaeHasUp));
        }
        else
        {
        }

        return ret;
    }

    public static float GetSkillCollectionValue(StatusType type)
    {
        using var e = TableManager.Instance.SkillData.GetEnumerator();

        float ret = 0f;
        while (e.MoveNext())
        {
            if ((StatusType)e.Current.Value.Collectionabiltype != type) continue;

            int skillCurrentLevel =
                ServerData.skillServerTable.TableDatas[SkillServerTable.SkillCollectionLevel][e.Current.Value.Id].Value;

            if (skillCurrentLevel != 0)
            {
                ret += skillCurrentLevel * e.Current.Value.Collectionvalue;
            }
        }

        return ret;
    }


    public static float GetSkillHasValue(StatusType type)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.SkillTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Haseffecttype != (int)type) continue;

            int awakeNum = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAwakeNum][tableDatas[i].Id]
                .Value;

            if (awakeNum == 0) continue;


            ret += awakeNum * tableDatas[i].Haseffectvalue;
        }

        return ret;
    }

    public static float GetMarbleValue(StatusType type)
    {
        float ret = 0f;

        bool isMarbleAwaked = ServerData.userInfoTable.TableDatas[UserInfoTable.marbleAwake].Value == 1;

        var tableDatas = TableManager.Instance.MarbleTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (ServerData.marbleServerTable.TableDatas[tableDatas[i].Stringid].hasItem.Value == 0) continue;

            for (int j = 0; j < tableDatas[i].Abilitytype.Length; j++)
            {
                if (tableDatas[i].Abilitytype[j] == (int)type)
                {
                    ret += isMarbleAwaked == false ? tableDatas[i].Abilityvalue[j] : tableDatas[i].Awakevalue[j];
                }
            }
        }

        return ret;
    }

    #endregion

    #region SkillDamage

    public static float GetSkillDamagePercentValue()
    {
        float ret = 0f;

        ret += GetWeaponEquipPercentValue(StatusType.SkillDamage);
        ret += GetMagicBookEquipPercentValue(StatusType.SkillDamage);
        ret += GetMagicBookHasPercentValue(StatusType.SkillDamage);


        ret += ServerData.statusTable.GetStatusValue(StatusTable.SkillDamage_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.SkillDamage);
        ret += GetSkillCollectionValue(StatusType.SkillDamage);
        ret += GetPassiveSkillValue(StatusType.SkillDamage);
        ret += ServerData.petTable.GetStatusValue(StatusType.SkillDamage);

        //ret += GetTitleAbilValue(StatusType.SkillDamage);
        ret += GetRelicHasEffect(StatusType.SkillDamage);

        ret += GetSinsuEquipEffect(StatusType.SkillDamage);
        ret += GetStageRelicHasEffect(StatusType.SkillDamage);
        ret += GetSonAbilHasEffect(StatusType.SkillDamage);
        ret += GetYachaSkillPercentValue();
        ret += GuildManager.abilValue;
        ret += GetTitleMagicBookAbilValue(StatusType.SkillDamage);
        
        ret += GetGuimoonHasEffect1(StatusType.SkillDamage);

        return ret;
    }

    private const string yachaKey = "weapon21";

    public static float GetYachaSkillPercentValue()
    {
        bool hasYacha = ServerData.weaponTable.TableDatas[yachaKey].hasItem.Value == 1;

        if (hasYacha == false) return 0f;

        return ServerData.statusTable.GetTableData(StatusTable.Level).Value * GameBalance.YachaSkillAddValuePerLevel;
    }

    public static float GetYachaIgnoreDefenseValue()
    {
        bool hasYacha = ServerData.weaponTable.TableDatas[yachaKey].hasItem.Value == 1;

        bool cockAwake = ServerData.userInfoTable.TableDatas[UserInfoTable.cockAwake].Value == 1;

        if (hasYacha == false || cockAwake == false) return 0f;

        return ServerData.statusTable.GetTableData(StatusTable.Level).Value *
               GameBalance.YachaIgnoreDefenseAddValuePerLevel;
    }

    public static float GetYachaChunSlashValue()
    {
        bool hasYacha = ServerData.weaponTable.TableDatas[yachaKey].hasItem.Value == 1;

        bool dogAwake = ServerData.userInfoTable.TableDatas[UserInfoTable.dogAwake].Value == 1;

        if (hasYacha == false || dogAwake == false) return 0f;

        return ServerData.statusTable.GetTableData(StatusTable.Level).Value *
               GameBalance.YachaChunSlashAddValuePerLevel;
    }

    #endregion

    #region SkillCoolTime

    public static float GetSkillCoolTimeDecreaseValue()
    {
        float ret = 0f;

        ret += GetWeaponEquipPercentValue(StatusType.SkillCoolTime);
        ret += GetMagicBookEquipPercentValue(StatusType.SkillCoolTime);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.SkillCoolTime_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.SkillCoolTime);
        ret += GetBuffValue(StatusType.SkillCoolTime);
        ret += GetYomulUpgradeValue(StatusType.SkillCoolTime);

        ret += IsChunAttackSpeedAwake() ? 0.05f : 0f;

        return ret;
    }

    #endregion

    #region DamBalance

    public static float GetDamBalanceAddValue()
    {
        float addValue1 = ServerData.statusTable.GetStatusValue(StatusTable.DamageBalance_memory);
        return addValue1;
    }

    #endregion

    #region Critical

    public static bool ActiveCritical()
    {
        return Random.value < GetCriticalProb();
    }

    public static bool ActiveSuperCritical()
    {
        return Random.value < GetSuperCriticalProb();
    }

    public static float GetCriticalProb()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalLevel_Gold);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalLevel_StatPoint);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalLevel_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.CriticalProb);
        ret += ServerData.petTable.GetStatusValue(StatusType.CriticalProb);
        ret += GetSkillCollectionValue(StatusType.CriticalProb);

        return ret;
    }

    public static float CriticalDam()
    {
        float ret = 0f;

        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalDamLevel_Gold);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalDamLevel_StatPoint);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.CriticalDamLevel_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.CriticalDam);
        ret += GetWeaponHasPercentValue(StatusType.CriticalDam);
        ret += GetMagicBookHasPercentValue(StatusType.CriticalDam);
        ret += GetSkillCollectionValue(StatusType.CriticalDam);
        ret += ServerData.petTable.GetStatusValue(StatusType.CriticalDam);
        ret += GetStageRelicHasEffect(StatusType.CriticalDam);
        ret += GetSonAbilHasEffect(StatusType.CriticalDam);
        ret += GetSinsuEquipEffect(StatusType.CriticalDam);
        ret += GetSusanoAbil(StatusType.CriticalDam);
        ret += GetRingCollectionHasValue(StatusType.CriticalDam);
        ret += GetGuimoonHasEffect1(StatusType.CriticalDam);

        return ret;
    }

    #endregion

    #region BuffEffect

    public static float GetGoldPlusValue()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.GoldGain_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.GoldGainPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.GoldGainPer);
        ret += GetBuffValue(StatusType.GoldGainPer);
        ret += GetMarbleValue(StatusType.GoldGainPer);
        ret += GetMagicBookHasPercentValue(StatusType.GoldGainPer);

        ret += GetHotTimeBuffEffect(StatusType.GoldGainPer);
        ret += GetHotTimeEventBuffEffect(StatusType.GoldGainPer);
        ret += GetMonthBuffEffect(StatusType.GoldGainPer);
        ret += GetMonth2BuffEffect(StatusType.GoldGainPer);
        ret += GetGuildPetEffect(StatusType.GoldGainPer);
        
        ret += GetGuimoonHasEffect2(StatusType.GoldGainPer);
        return ret;
    }

    public static float GetGoldPlusValueExclusiveBuff()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.GoldGain_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.GoldGainPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.GoldGainPer);
        ret += GetMarbleValue(StatusType.GoldGainPer);
        ret += GetMagicBookHasPercentValue(StatusType.GoldGainPer);

        ret += GetHotTimeBuffEffect(StatusType.GoldGainPer);
        ret += GetHotTimeEventBuffEffect(StatusType.GoldGainPer);
        ret += GetMonthBuffEffect(StatusType.GoldGainPer);
        ret += GetMonth2BuffEffect(StatusType.GoldGainPer);
        ret += GetGuildPetEffect(StatusType.GoldGainPer);

        ret += GetGuimoonHasEffect2(StatusType.GoldGainPer);
        return ret;
    }

    public static float GetGoldBarPlusValue()
    {
        float ret = 0;

        ret += GetGoldPlusValue() / 100000;

        ret += ServerData.petTable.GetStatusValue(StatusType.GoldBarGainPer);
        
        ret += GetGuimoonHasEffect2(StatusType.GoldBarGainPer);

        //ret += ServerData.statusTable.GetStatusValue(StatusTable.GoldBarGain_GoldBar);

        return ret;
    }

    public static float GetGoldBarPlusValueExclusiveBuff()
    {
        float ret = 0f;

        ret += GetGoldPlusValueExclusiveBuff() / 100000;

        ret += ServerData.petTable.GetStatusValue(StatusType.GoldBarGainPer);
        //ret += ServerData.statusTable.GetStatusValue(StatusTable.GoldBarGain_GoldBar);


        return ret;
    }

    public static float GetBaseExpPlusValue_BuffAllIgnored()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.ExpGain_memory);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.ExpGainPer);
        ret += GetMarbleValue(StatusType.ExpGainPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.ExpGainPer);

        ret += GetTitleStageAbilValue(StatusType.ExpGainPer);
        ret += GetGuildPetEffect(StatusType.ExpGainPer);
        ret += GetPetHomeAbilValue(StatusType.ExpGainPer);
        ret += GetMarkBuffAddValue();

        ret += GetWeaponHasPercentValue(StatusType.ExpGainPer);
        
        ret += GetGuimoonHasEffect2(StatusType.ExpGainPer);

        return ret;
    }

    public static float GetExpPlusValue_WithAllBuff()
    {
        float ret = 0f;

        ret += GetBaseExpPlusValue_BuffAllIgnored();
        //
        ret += GetBuffValue(StatusType.ExpGainPer);
        ret += GetHotTimeBuffEffect(StatusType.ExpGainPer);
        ret += GetHotTimeEventBuffEffect(StatusType.ExpGainPer);
       // ret += GetSAHotTimeEventBuffEffect(StatusType.ExpGainPer);
        ret += GetMonthBuffEffect(StatusType.ExpGainPer);
        ret += GetMonth2BuffEffect(StatusType.ExpGainPer);
        
        //ret += GetOneYearBuffValue(StatusType.ExpGainPer);
        //ret += GetChuSeokBuffValue(StatusType.ExpGainPer);
        //ret += GetChuSeokBuffValue2(StatusType.ExpGainPer);
        //

        return ret;
    }

    public static float GetExpPlusValueIncludeHotTimeBuffOnly()
    {
        float ret = 0f;

        ret += GetBaseExpPlusValue_BuffAllIgnored();

        ret += GetHotTimeEventBuffEffect(StatusType.ExpGainPer);
        
        ret += GetMonthBuffEffect(StatusType.ExpGainPer);
        
        ret += GetMonth2BuffEffect(StatusType.ExpGainPer);

        return ret;
    }

    public static float GetMarkBuffAddValue()
    {
        int idx = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.hellMark].Value;
        if (idx == 0) return 0f;
        if (idx >= GameBalance.warMarkAbils.Count) return 0f;

        return GameBalance.warMarkAbils[idx];
    }

    //1주년 버프 키값
    private static string ob = "ob";

    private static float GetOneYearBuffValue(StatusType status)
    {
        if (ServerData.buffServerTable.TableDatas[ob].remainSec.Value <= 0f) return 0f;

        switch (status)
        {
            case StatusType.ExpGainPer:
            {
                return 100f;
            }
                break;
            case StatusType.MagicStoneAddPer:
            {
                return 200f;
            }
                break;
            case StatusType.MarbleAddPer:
            {
                return 50f;
            }
                break;
        }

        return 0f;
    }


    //월간 패스 키값
    private static string mf11 = "mf11";

    private static float GetChuSeokBuffValue(StatusType status)
    {
        if (ServerData.buffServerTable.TableDatas[mf11].remainSec.Value <= 0f) return 0f;

        switch (status)
        {
            case StatusType.ExpGainPer:
            {
                return 150f;
            }
                break;
            case StatusType.MagicStoneAddPer:
            {
                return 400f;
            }
                break;
            case StatusType.MarbleAddPer:
            {
                return 100f;
            }
                break;
        }

        return 0f;
    }

    private static string ma11 = "ma11";

    private static float GetChuSeokBuffValue2(StatusType status)
    {
        if (ServerData.buffServerTable.TableDatas[ma11].remainSec.Value <= 0f) return 0f;

        switch (status)
        {
            case StatusType.ExpGainPer:
            {
                return 200f;
            }
                break;
            case StatusType.MagicStoneAddPer:
            {
                return 500f;
            }
                break;
            case StatusType.MarbleAddPer:
            {
                return 200f;
            }
                break;
        }

        return 0f;
    }


    //월간 패스 키값
    private static string mf12 = "mf12";

    private static float GetMonthlyFreeBuffValue(StatusType status)
    {
        if (ServerData.buffServerTable.TableDatas[mf12].remainSec.Value <= 0f) return 0f;

        switch (status)
        {
            case StatusType.ExpGainPer:
            {
                return 150f;
            }
                break;
            case StatusType.MagicStoneAddPer:
            {
                return 400f;
            }
                break;
            case StatusType.MarbleAddPer:
            {
                return 100f;
            }
                break;
        }

        return 0f;
    }

    private static string ma12 = "ma12";

    private static float GetMonthlyAdBuffValue(StatusType status)
    {
        if (ServerData.buffServerTable.TableDatas[ma12].remainSec.Value <= 0f) return 0f;

        switch (status)
        {
            case StatusType.ExpGainPer:
            {
                return 200f;
            }
                break;
            case StatusType.MagicStoneAddPer:
            {
                return 500f;
            }
                break;
            case StatusType.MarbleAddPer:
            {
                return 200f;
            }
                break;
        }

        return 0f;
    }


    public static float GetMagicStonePlusValue()
    {
        float ret = 0f;

        ret += GetHotTimeBuffEffect(StatusType.MagicStoneAddPer);
        ret += GetHotTimeEventBuffEffect(StatusType.MagicStoneAddPer);
        //ret += GetSAHotTimeEventBuffEffect(StatusType.MagicStoneAddPer);
        ret += GetMonthBuffEffect(StatusType.MagicStoneAddPer);
        ret += GetMonth2BuffEffect(StatusType.MagicStoneAddPer);
        ret += GetBuffValue(StatusType.MagicStoneAddPer);
        ret += GetGuildPetEffect(StatusType.MagicStoneAddPer);
        return ret;
    }

    public static float GetMarblePlusValue()
    {
        float ret = 0f;

        ret += GetHotTimeBuffEffect(StatusType.MarbleAddPer);
        ret += GetHotTimeEventBuffEffect(StatusType.MarbleAddPer);
        //ret += GetSAHotTimeEventBuffEffect(StatusType.MarbleAddPer);
        ret += GetBuffValue(StatusType.MarbleAddPer);

        //ret += GetOneYearBuffValue(StatusType.MarbleAddPer);
        //ret += GetChuSeokBuffValue(StatusType.MarbleAddPer);
        //ret += GetChuSeokBuffValue2(StatusType.MarbleAddPer);

        //  ret += GetMonthlyFreeBuffValue(StatusType.MarbleAddPer);
        // ret += GetMonthlyAdBuffValue(StatusType.MarbleAddPer);

        return ret;
    }

    public static float GetBuffValue(StatusType type)
    {
        float ret = 0f;

        var tableData = TableManager.Instance.BuffTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if ((int)type == tableData[i].Bufftype)
            {
                //-1은 무한

                if (tableData[i].Yomulid == -1)
                {
                    if (ServerData.buffServerTable.TableDatas[tableData[i].Stringid].remainSec.Value != 0f)
                    {
                        ret += tableData[i].Buffvalue;
                    }
                }
                //요물
                else
                {
                    if (ServerData.userInfoTable.TableDatas[UserInfoTable.buffAwake].Value == 0)
                    {
                        if (ServerData.buffServerTable.TableDatas[tableData[i].Stringid].remainSec.Value != 0f)
                        {
                            ret += tableData[i].Buffvalue;
                        }
                    }
                    //각성후
                    else
                    {
                        ret += tableData[i].Buffawakevalue;
                    }
                }
            }
        }

        return ret;
    }

    #endregion

    #region HP&MP

    public static float GetMaxHpDimension()
    {
        float hp = GameBalance.dimensionHp;

        hp += GetDimensionAddHp();
        
        return hp;
    }
    
    public static float GetMaxHp()
    {
        float originHp = GetOriginHp();

        float hpAddPerValue = GetMaxHpPercentAddValue();

        return originHp + originHp * hpAddPerValue;
    }

    public static float GetOriginHp()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.HpLevel_Gold);
        ret += ServerData.petTable.GetStatusValue(StatusType.Hp);
        ret += GetMagicBookEquipPercentValue(StatusType.Hp);

        ret += GetSinsuEquipEffect(StatusType.Hp);
        ret += GetRelicHasEffect(StatusType.Hp);
        
        ret += GetGuimoonHasEffect2(StatusType.Hp);
        return ret;
    }

    public static float GetMaxHpPercentAddValue()
    {
        float ret = 0f;
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.HpAddPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.HpAddPer);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.HpPer_StatPoint);
        ret += GetPassiveSkillValue(StatusType.HpAddPer);

        //ret += GetTitleAbilValue(StatusType.HpAddPer);
        ret += GetRelicHasEffect(StatusType.HpAddPer);

        ret += GetMagicBookEquipPercentValue(StatusType.HpAddPer);
        
        ret += GetGuimoonHasEffect2(StatusType.HpAddPer);
        return ret;
    }

    public static float GetMaxMp()
    {
        float originMp = GetOriginMp();

        float mpAddPerValue = GetMaxMpPercentAddValue();

        return originMp + originMp * mpAddPerValue;
    }

    public static float GetOriginMp()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.MpLevel_Gold);
        ret += ServerData.petTable.GetStatusValue(StatusType.Mp);
        return ret;
    }

    public static float GetMaxMpPercentAddValue()
    {
        float ret = 0f;
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.MpAddPer);
        ret += ServerData.petTable.GetStatusValue(StatusType.MpAddPer);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.MpPer_StatPoint);
        return ret;
    }

    public static float GetHpRecover()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.HpRecover_Gold);
        
        ret += GetGuimoonHasEffect2(StatusType.HpRecover);

        return ret;
    }

    public static float GetMpRecover()
    {
        float ret = 0f;
        ret += ServerData.statusTable.GetStatusValue(StatusTable.MpRecover_Gold);
        return ret;
    }

    #endregion

    public static float GetIgnoreDefenseValue()
    {
        float ret = 300f;

        ret += ServerData.statusTable.GetStatusValue(StatusTable.IgnoreDefense_memory);
        ret += GetWeaponEquipPercentValue(StatusType.IgnoreDefense);
        ret += GetWeaponHasPercentValue(StatusType.IgnoreDefense);
        ret += ServerData.costumeServerTable.GetCostumeAbility(StatusType.IgnoreDefense);
        ret += ServerData.petTable.GetStatusValue(StatusType.IgnoreDefense);

        //ret += GetTitleAbilValue(StatusType.IgnoreDefense);
        ret += GetYomulUpgradeValue(StatusType.IgnoreDefense);

        ret += GetBuffValue(StatusType.IgnoreDefense);

        ret += GetRelicHasEffect(StatusType.IgnoreDefense);
        ret += GetStageRelicHasEffect(StatusType.IgnoreDefense);

        ret += GetSinsuEquipEffect(StatusType.IgnoreDefense);

        ret += GetYachaIgnoreDefenseValue();

        ret += GetAsuraAbilValue(StatusType.IgnoreDefense);
        ret += GetIndraAbilValue(StatusType.IgnoreDefense);

        ret += GetPassiveSkillValue(StatusType.IgnoreDefense);

        //
        ret += GetGumIgDefenseValue();
        //
        ret += GetGumihoAbil();
        
        ret += GetGuimoonHasEffect1(StatusType.IgnoreDefense);

        return ret;
    }

    public static float GetPenetrateDefense()
    {
        float ret = 0f;

        ret += GetYomulUpgradeValue(StatusType.PenetrateDefense);

        ret += GetBuffValue(StatusType.PenetrateDefense);

        ret += GetStageRelicHasEffect(StatusType.PenetrateDefense);

        ret += GetPassiveSkillValue(StatusType.PenetrateDefense);

        ret += GetIndraAbilValue(StatusType.PenetrateDefense);

        ret += GetSusanoAbil(StatusType.PenetrateDefense);

        ret += GetOrochiAbilValue(StatusType.PenetrateDefense);

        return ret;
    }

    public static float GetSuperCriticalDamPer()
    {
        float ret = 0.5f;

        ret += GetSinsuEquipEffect(StatusType.SuperCritical1DamPer);
        ret += GetYomulUpgradeValue(StatusType.SuperCritical1DamPer);
        ret += GetStageRelicHasEffect(StatusType.SuperCritical1DamPer);
        ret += GetBuffValue(StatusType.SuperCritical1DamPer);
        ret += GetRelicHasEffect(StatusType.SuperCritical1DamPer);

        ret += ServerData.petTable.GetStatusValue(StatusType.SuperCritical1DamPer);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.ChunSlash_memory);

        ret += GetYachaChunSlashValue();

        ret += GetAsuraAbilValue(StatusType.SuperCritical1DamPer);

        ret += GetPassiveSkillValue(StatusType.SuperCritical1DamPer);

        ret += GetGuimoonHasEffect1(StatusType.SuperCritical1DamPer);

        
        return ret;
    }


    public static float GetSuperCriticalProb()
    {
        float ret = 0f;

        ret += GetYomulUpgradeValue(StatusType.SuperCritical1Prob);
        ret += GetBuffValue(StatusType.SuperCritical1Prob);
        ret += GetSinsuEquipEffect(StatusType.SuperCritical1Prob);

        return ret;
    }


    public static float GetSuperCritical2DamPer()
    {
        float ret = 0f;

        ret += GetWeaponHasPercentValue(StatusType.SuperCritical2DamPer);

        ret += GetFeelMulAddDam();

        ret += GetAsuraAbilValue(StatusType.SuperCritical2DamPer);

        ret += GetLeeMuGiAddDam();

        ret += GetRelicHasEffect(StatusType.SuperCritical2DamPer);

        //ret += GetTitleAbilValue(StatusType.SuperCritical2DamPer);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.FeelSlash_memory);

        ret += GetPetHomeAbilValue(StatusType.SuperCritical2DamPer);

        ret += GetGradeTestAbilValue(StatusType.SuperCritical2DamPer);

        ret += GetTitleWeaponAbilValue(StatusType.SuperCritical2DamPer);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical2DamPer);

        return ret;
    }

    public static float GetSuperCritical3DamPer()
    {
        float ret = 0f;

        ret += GetWeaponEquipPercentValue(StatusType.SuperCritical3DamPer);

        ret += GetMagicBookEquipPercentValue(StatusType.SuperCritical3DamPer);

        ret += GetRelicHasEffect(StatusType.SuperCritical3DamPer);

        ret += GetHellMarkValue();

        ret += GetHellAbilHasEffect(StatusType.SuperCritical3DamPer);

        ret += GetDragonBallAbil0Value();

        ret += GetHellRelicAbilValue();

        ret += GetPetHomeAbilValue(StatusType.SuperCritical3DamPer);

        ret += GetYumAbil(StatusType.SuperCritical3DamPer);

        ret += GetStageRelicHasEffect(StatusType.SuperCritical3DamPer);

        ret += GetSumiTowerEffect(StatusType.SuperCritical3DamPer);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.ZSlash_memory);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical3DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical3DamPer);
        
        ret += GetPassiveSkill3Value(StatusType.SuperCritical3DamPer);

        return ret;
    }
    //천상베기
    public static float GetSuperCritical4DamPer()
    {
        float ret = 0f;

        ret += GetChunMarkValue();

        ret += GetChunAbilHasEffect(StatusType.SuperCritical4DamPer);

        ret += GetRelicHasEffect(StatusType.SuperCritical4DamPer);

        ret += GetMagicBookEquipPercentValue(StatusType.SuperCritical4DamPer);

        ret += GetWeaponEquipPercentValue(StatusType.SuperCritical4DamPer);

        ret += GetDragonBallAbil1Value();

        ret += GetPetHomeAbilValue(StatusType.SuperCritical4DamPer);

        ret += GetOkAbil(StatusType.SuperCritical4DamPer);

        ret += GetStageRelicHasEffect(StatusType.SuperCritical4DamPer);

        ret += GetSumiTowerEffect(StatusType.SuperCritical4DamPer);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.Cslash_memory);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical4DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical4DamPer);
        
        ret += GetPassiveSkill3Value(StatusType.SuperCritical4DamPer);

        return ret;
    }

    //귀신베기
    public static float GetSuperCritical5DamPer()
    {
        float ret = 0f;

        ret += GetDokebiMarkValue();

        ret += GetDokebiHornCritical5Add();

        ret += GetDokebiFireAbilHasEffect(StatusType.SuperCritical5DamPer);

        ret += GetMagicBookEquipPercentValue(StatusType.SuperCritical5DamPer);

        ret += GetWeaponEquipPercentValue(StatusType.SuperCritical5DamPer);

        ret += GetDoAbil(StatusType.SuperCritical5DamPer);

        ret += GetStageRelicHasEffect(StatusType.SuperCritical5DamPer);

        ret += GetRelicHasEffect(StatusType.SuperCritical5DamPer);

        ret += GetSumiTowerEffect(StatusType.SuperCritical5DamPer);

        ret += GetFoxCupAbilValue(GetCurrentFoxCupIdx(), 0);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.GiSlash_memory);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical5DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical5DamPer);
        
        ret += GetPassiveSkill3Value(StatusType.SuperCritical5DamPer);

        return ret;
    }
    //신수베기
    public static float GetSuperCritical6DamPer()
    {
        float ret = 0f;

        ret += GetPetHomeAbilValue(StatusType.SuperCritical6DamPer);

        ret += GetSasinsuStarAddValue();

        ret += GetFoxCupAbilValue(GetCurrentFoxCupIdx(), 1);

        ret += GetStageRelicHasEffect(StatusType.SuperCritical6DamPer);

        ret += GetSinsuTreasureAbilPlusValue();

        ret += ServerData.statusTable.GetStatusValue(StatusTable.Sin_StatPoint);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical6DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical6DamPer);

        return ret * (1 + GetEnhanceSinsuCritical());
    }


    //수미베기
    public static float GetSuperCritical7DamPer()
    {
        float ret = 0f;

        ret += GetMagicBookEquipPercentValue(StatusType.SuperCritical7DamPer);

        ret += GetWeaponEquipPercentValue(StatusType.SuperCritical7DamPer);

        ret += GetSumiFireAbilHasEffect(StatusType.SuperCritical7DamPer);

        ret += GetSuAbil(StatusType.SuperCritical7DamPer);

        ret += GetStageRelicHasEffect(StatusType.SuperCritical7DamPer);

        ret += GetRelicHasEffect(StatusType.SuperCritical7DamPer);

        ret += GetSumiTowerEffect(StatusType.SuperCritical7DamPer);

        ret += GetFoxCupAbilValue(GetCurrentFoxCupIdx(), 2);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.Gum_memory);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical7DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical7DamPer);
        
        ret += GetPassiveSkill3Value(StatusType.SuperCritical7DamPer);

        return ret;
    }

    /// <summary>
    /// 혈자리베기
    /// </summary>
    /// <returns></returns>
    /// 하단전베기
    public static float GetSuperCritical8DamPer()
    {
        float ret = 0f;

        ret += GetGyungRockEffect(StatusType.SuperCritical8DamPer);

        ret += GetGyungRockEffect(StatusType.SuperCritical8DamPer) * GetGuildTowerChimUpgradeValue();
        
        ret += GetGyungRockEffect(StatusType.SuperCritical8DamPer) * SuperCritical8AddDam();
        
        ret += GetGyungRockEffect4(StatusType.SuperCritical8DamPer);

        ret += GetGyungRockEffect4(StatusType.SuperCritical8DamPer) * GetGuildTowerChimUpgradeValue();
        
        ret += GetGyungRockEffect4(StatusType.SuperCritical8DamPer) * SuperCritical8AddDam();

        ret += GetPassiveSkillValue(StatusType.SuperCritical8DamPer);
        
        
        ret += GetNewLowGyungRockAwakeAbilValue();
        

        return ret;
    }
    //흉수베기
    public static float GetSuperCritical9DamPer()
    {
        float ret = 0f;

        ret += GetSahyungTreasureAbilPlusValue();

        ret += GetPetHomeAbilValue(StatusType.SuperCritical9DamPer);

        ret += GetStageRelicHasEffect(StatusType.SuperCritical9DamPer);

        ret += GetWolfRingAbilValue(GetCurrentWolfRingIdx(), 0);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.Hyung_StatPoint);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical9DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical9DamPer);

        return ret * (1 + GetEnhanceHyungsuCritical());
    }

    //섬광베기
    public static float GetSuperCritical10DamPer()
    {
        float ret = 0f;

        ret += GetWeaponEquipPercentValue(StatusType.SuperCritical10DamPer);

        ret += GetMagicBookEquipPercentValue(StatusType.SuperCritical10DamPer);

        ret += GetRelicHasEffect(StatusType.SuperCritical10DamPer);

        ret += GetTresureAbilHasEffect(StatusType.SuperCritical10DamPer);

        ret += GetThiefAbil(StatusType.SuperCritical10DamPer);

        ret += GetStageRelicHasEffect(StatusType.SuperCritical10DamPer);

        ret += GetGuimoonHasEffect1(StatusType.SuperCritical10DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical10DamPer);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.Sum_memory);

        ret += GetPassiveSkill3Value(StatusType.SuperCritical10DamPer);

        
        return ret;
    }
    private static float superCritical11Value = -1;

    public static void ResetSuperCritical11CalculatedValue()
    {
        superCritical11Value = -1;
    }

    //수호베기
    public static float GetSuperCritical11DamPer()
    {
        if (superCritical11Value == -1)
        {
            superCritical11Value = 0;

            var tableData = TableManager.Instance.suhoPetTable.dataArray;

            for (int i = 0; i < tableData.Length; i++)
            {
                var serverData = ServerData.suhoAnimalServerTable.TableDatas[tableData[i].Stringid];

                if (serverData.hasItem.Value == 0) continue;

                int currentLevel = serverData.level.Value;

                superCritical11Value += tableData[i].Abilvalue[currentLevel];
            }
            superCritical11Value += GetGuimoonHasEffect1(StatusType.SuperCritical11DamPer);
            superCritical11Value += GetSuhoUpgradeAbilValue(GetCurrentSuhoUpgradeIdx());
        }

        //막아둠일단
        //return 0f;

        return superCritical11Value * (1 + GetEnhanceSuhoCritical());
    }

    //심연베기
    public static float GetSuperCritical12DamPer()
    {
        float ret = 0f;

        ret += GetWeaponEquipPercentValue(StatusType.SuperCritical12DamPer);

        ret += GetMagicBookEquipPercentValue(StatusType.SuperCritical12DamPer);

        ret += GetDarkTreasureAbilHasEffect(StatusType.SuperCritical12DamPer);

        ret += GetDarkMarkValue();

        ret += GetRelicHasEffect(StatusType.SuperCritical12DamPer);

        ret += GetDarkAbil(StatusType.SuperCritical12DamPer);

        ret += GetStageRelicHasEffect(StatusType.SuperCritical12DamPer);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical12DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical12DamPer);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.Sim_memory);

        ret += GetPassiveSkill3Value(StatusType.SuperCritical12DamPer);

        return ret;
    }

    //중단전베기
    public static float GetSuperCritical13DamPer()
    {
        float ret = 0f;

        ret += GetGyungRockEffect2(StatusType.SuperCritical13DamPer);

        ret += GetGyungRockEffect2(StatusType.SuperCritical13DamPer) * GetGuildTowerChimUpgradeValue();
        
        ret += GetGyungRockEffect2(StatusType.SuperCritical13DamPer) * SuperCritical13AddDam();

        ret += GetPassiveSkillValue(StatusType.SuperCritical13DamPer);
        
        ret += GetNewMiddleGyungRockAwakeAbilValue();
        
        ret += GetGyungRockEffect5(StatusType.SuperCritical13DamPer);

        ret += GetGyungRockEffect5(StatusType.SuperCritical13DamPer) * GetGuildTowerChimUpgradeValue();
        
        ret += GetGyungRockEffect5(StatusType.SuperCritical13DamPer) * SuperCritical13AddDam();

        return ret;
    }
    

    //여우
    public static float GetSuperCritical14DamPer()
    {
        float ret = 0f;

        ret += GetFoxFireEffect(StatusType.SuperCritical14DamPer);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical14DamPer);
        
        return ret;
    }

    //신선
    public static float GetSuperCritical15DamPer()
    {
        float ret = 0f;

        ret += GetWeaponEquipPercentValue(StatusType.SuperCritical15DamPer);

        ret += GetMagicBookEquipPercentValue(StatusType.SuperCritical15DamPer);

        ret += GetSinsunTreasureAbilHasEffect(StatusType.SuperCritical15DamPer);

        ret += GetRelicHasEffect(StatusType.SuperCritical15DamPer);

        ret += GetStageRelicHasEffect(StatusType.SuperCritical15DamPer);

        ret += GetSinsunAbil(StatusType.SuperCritical15DamPer);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical15DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical15DamPer);
        
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Sin_memory);
        
        ret += GetPassiveSkill3Value(StatusType.SuperCritical15DamPer);

        return ret;
    }

    //태극
    public static float GetSuperCritical16DamPer()
    {
        float ret = 0f;

        ret += GetTaeGukTowerValue();
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical16DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical16DamPer);

        return ret * (1 + GetEnhanceTaegeukCritical());
    }
    //영혼 베기
    public static float GetSuperCritical17DamPer()
    {
        float ret = 0f;

        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special0_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special1_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special2_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special3_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special4_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special5_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special6_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special7_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special8_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special9_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special10_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special11_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special12_GoldBar);
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Special13_GoldBar);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical17DamPer);

        return ret*(1+GetEnhanceSoulCritical());
    }

    //상단전베기
    public static float GetSuperCritical18DamPer()
    {
        float ret = 0f;

        ret += GetGyungRockEffect3(StatusType.SuperCritical18DamPer);

        ret += GetGyungRockEffect3(StatusType.SuperCritical18DamPer) * GetGuildTowerChimUpgradeValue();

        ret += GetGyungRockEffect3(StatusType.SuperCritical18DamPer) * SuperCritical18AddDam();

        ret += GetPassiveSkillValue(StatusType.SuperCritical18DamPer);

        ret += GetNewTopGyungRockAwakeAbilValue();
        
        ret += GetGyungRockEffect6(StatusType.SuperCritical18DamPer);

        ret += GetGyungRockEffect6(StatusType.SuperCritical18DamPer) * GetGuildTowerChimUpgradeValue();
        
        ret += GetGyungRockEffect6(StatusType.SuperCritical18DamPer) * SuperCritical18AddDam();
        
        return ret;
    }
    //귀살 베기
    public static float GetSuperCritical19DamPer()
    {
        float ret = 0f;

        ret += GetGwisalTreasureAbilHasEffect(StatusType.SuperCritical19DamPer);

        ret += GetWeaponEquipPercentValue(StatusType.SuperCritical19DamPer);

        ret += GetMagicBookEquipPercentValue(StatusType.SuperCritical19DamPer);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical19DamPer);
        
        ret += GetPassiveSkill3Value(StatusType.SuperCritical19DamPer);

        return ret;
    }

    //천구 베기
    public static float GetSuperCritical20DamPer()
    {
        float ret = 0f;

        ret += GetChunguTreasureAbilHasEffect();
        
        ret += GetPetHomeAbilValue(StatusType.SuperCritical20DamPer);
        
        ret += GetStageRelicHasEffect(StatusType.SuperCritical20DamPer);

        ret += GetDragonBraceletAbilValue(GetCurrentDragonBraceletIdx(), 0);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical20DamPer);
        
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Chungu_StatPoint);
        
        ret += GetMeditationAbilValue(StatusType.SuperCritical20DamPer);

        return ret *(1+GetEnhanceChunguCritical());
    }
    //초월 베기
    public static float GetSuperCritical21DamPer()
    {
        float ret = 0f;

        ret += GetWeaponTransHasValue(StatusType.SuperCritical21DamPer);

        ret += GetMagicBookTransHasValue(StatusType.SuperCritical21DamPer);

        ret += GetTransUpgradeAbilValue(GetCurrentTransUpgradeIdx());

        return ret * (1 + GetEnhanceTransCritical());
    }
    //진 귀살 베기
    public static float GetSuperCritical22DamPer()
    {
        float ret = 0f;

        ret += GetWeaponEquipPercentValue(StatusType.SuperCritical22DamPer);

        ret += GetMagicBookEquipPercentValue(StatusType.SuperCritical22DamPer);

        return ret * (1 + GetEnhanceGwisalCritical());
    }
    //심상베기
    public static float GetSuperCritical23DamPer()
    {
        float ret = 0f;

        
        ret += GetClosedTrainingValue();
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical23DamPer);

        return ret;
    }
    //용인베기
    public static float GetSuperCritical24DamPer()
    {
        float ret = 0f;
        
        ret += GetDragonScaleAbilHasEffect();
        
        ret += GetWeaponEquipPercentValue(StatusType.SuperCritical24DamPer);
        
        ret += GetWeaponHasPercentValue(StatusType.SuperCritical24DamPer);

        ret += GetMagicBookEquipPercentValue(StatusType.SuperCritical24DamPer);
        
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Dragon_memory);

        ret += GetStageRelicHasEffect(StatusType.SuperCritical24DamPer);

        ret += GetRelicHasEffect(StatusType.SuperCritical24DamPer);

        ret += GetGuimoonHasEffect1(StatusType.SuperCritical24DamPer);
        
        ret += GetPassiveSkill3Value(StatusType.SuperCritical24DamPer);

        return ret;
    }
    
    //요력베기
    public static float GetSuperCritical25DamPer()
    {
        float ret = 0f;
        
        ret += GetYoPowerEffect(StatusType.SuperCritical25DamPer);

        
        return ret;
    }

    //진 요도 피해
    public static float GetSuperCritical26DamPer()
    {
        float ret = 0f;

        ret += SealSwordEvolutionAbility(StatusType.SuperCritical26DamPer);
        
        return ret;
    }

    //무공 피해
    public static float GetSuperCritical27DamPer()
    {
        float ret = 0f;

        ret += ByeolhoHasAbility(StatusType.SuperCritical27DamPer);
        
        return ret;
    }

    //파도베기
    public static float GetSuperCritical28DamPer()
    {
        float ret = 0f;

        ret += GetDragonPalaceTreasureAbilHasEffect();
        
        ret += GetStageRelicHasEffect(StatusType.SuperCritical28DamPer);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical28DamPer);
        
        ret += GetRelicHasEffect(StatusType.SuperCritical28DamPer);
        
        ret += GetPassiveSkill3Value(StatusType.SuperCritical28DamPer);

        
        ret += ServerData.statusTable.GetStatusValue(StatusTable.DragonPlace_memory);

        return ret;
    }

    //비무피해
    public static float GetSuperCritical29DamPer()
    {
        float ret = 0f;
        
        ret += BattleContestGradeAbility(StatusType.SuperCritical29DamPer);
        
        return ret;
    }

    //신력피해
    public static float GetSuperCritical30DamPer()
    {
        float ret = 0f;
        
        ret += GetSasinsuPowerAbility(StatusType.SuperCritical30DamPer,1);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical30DamPer);
        
        ret += GetSasinsuAwakePowerAbility(StatusType.SuperCritical30DamPer);
        
        return ret;
    }

    //협동피해
    public static float GetSuperCritical31DamPer()
    {
        float ret = 0f;
        
        ret += GetMunhaTower2Ability(StatusType.SuperCritical31DamPer);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical31DamPer);
        
        ret += GetMunhaHyulAbilValue(StatusType.SuperCritical31DamPer);

        return ret;
    }

    //극락베기
    public static float GetSuperCritical32DamPer()
    {
        float ret = 0f;
        
        ret += GetMurimTreasureAbilHasEffect();
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical32DamPer);
        
        ret += GetStageRelicHasEffect(StatusType.SuperCritical32DamPer);

        ret += ServerData.statusTable.GetStatusValue(StatusTable.Murim_memory);

        ret += GetRelicHasEffect(StatusType.SuperCritical32DamPer);
        
        ret += GetPassiveSkill3Value(StatusType.SuperCritical32DamPer);

        return ret;
    }
    //극혈 피해
    public static float GetSuperCritical33DamPer()
    {
        float ret = 0f;
        
        ret += GetGeukGyungRockAwakeAbilValue();
        
        ret += GetGeukGyungRock2AwakeAbilValue();
        
        ret += GetGeukGyungRock3AwakeAbilValue();
        
        ret += GetGyungRockEffect7(StatusType.SuperCritical33DamPer);

        ret += GetGyungRockEffect7(StatusType.SuperCritical33DamPer) * GetGuildTowerChimUpgradeValue();
        
        ret += GetGyungRockEffect8(StatusType.SuperCritical33DamPer);

        ret += GetGyungRockEffect8(StatusType.SuperCritical33DamPer) * GetGuildTowerChimUpgradeValue();

        ret += GetGyungRockEffect9(StatusType.SuperCritical33DamPer);

        ret += GetGyungRockEffect9(StatusType.SuperCritical33DamPer) * GetGuildTowerChimUpgradeValue();


        ret += GetGuimoonHasEffect1(StatusType.SuperCritical33DamPer);

        return ret;
    }
    //무림 베기
    public static float GetSuperCritical34DamPer()
    {
        float ret = 0f;
        
        ret += GetDifficultyBossTreasureAbilHasEffect();
        
        ret += ServerData.statusTable.GetStatusValue(StatusTable.Difficulty_StatPoint);

        ret += GetWeaponHasPercentValue(StatusType.SuperCritical34DamPer);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical34DamPer);

        return ret;
    }
    
    //업화베기
    public static float GetSuperCritical35DamPer()
    {
        float ret = 0f;
        
        ret += GetYeonOkTreasureAbilHasEffect();
        
        ret += GetWeaponHasPercentValue(StatusType.SuperCritical35DamPer);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical35DamPer);
        
        ret += GetStageRelicHasEffect(StatusType.SuperCritical35DamPer);
        
        ret += GetRelicHasEffect(StatusType.SuperCritical35DamPer);
        
        ret += GetPassiveSkill3Value(StatusType.SuperCritical35DamPer);

        return ret;
    }
    
    //해탈베기
    public static float GetSuperCritical36DamPer()
    {
        float ret = 0f;
        
        ret += GetHaetalValue(StatusType.SuperCritical36DamPer);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical36DamPer);

        return ret;
    }
    //보옥베기
    public static float GetSuperCritical37DamPer()
    {
        float ret = 0f;

        ret += GetTransJewelAbility(StatusType.SuperCritical37DamPer, 0);
        
        ret += GetTransJewelAbility(StatusType.SuperCritical37DamPer, 1);
        
        ret += GetTransJewelAbility(StatusType.SuperCritical37DamPer, 2);
        
        ret += GetTransJewelAbility(StatusType.SuperCritical37DamPer, 3);
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical37DamPer);
        
        return ret;
    }
    
    //차원베기
    public static float GetSuperCritical38DamPer()
    {
        float ret = 0f;
                
        ret += ServerData.dimensionStatusTable.GetStatusValue(DimensionStatusTable.S0_DC);
        
        ret += ServerData.dimensionStatusTable.GetStatusValue(DimensionStatusTable.S1_DC);
        
        ret += ServerData.dimensionStatusTable.GetStatusValue(DimensionStatusTable.S2_DC);

        ret += ServerData.dimensionStatusTable.GetStatusValue(DimensionStatusTable.S3_DC);
        
        ret += ServerData.dimensionStatusTable.GetStatusValue(DimensionStatusTable.S4_DC);

        ret += ServerData.dimensionStatusTable.GetStatusValue(DimensionStatusTable.S5_DC);
        
        ret += ServerData.dimensionStatusTable.GetStatusValue(DimensionStatusTable.S6_DC);
        
        
        return ret;
    }
    //
    public static float GetSuperCritical39DamPer()
    {
        float ret = 0f;

        ret += GetChunsangTreasureAbilHasEffect();
        
        ret += GetGuimoonHasEffect1(StatusType.SuperCritical39DamPer);

        return ret;
    }
    

    //태극증폭
    public static float GetEnhanceTaegeukCritical()
    {
        float ret = 0f;

        ret += GetEnhanceTaegeukCriticalAbil(StatusType.EnhanceTaegeukCritical);

        return ret;
    }
    //수호증폭
    public static float GetEnhanceSuhoCritical()
    {
        float ret = 0f;

        ret += GetSuhoTreasureAbilHasEffect();
        
        ret += GetSpecialTypeAbility(StatusType.EnhanceSuhoCritical);

        return ret;
    }
    //신수증폭
    public static float GetEnhanceSinsuCritical()
    {
        float ret = 0f;

        ret += GetBlackFoxEffect(StatusType.EnhanceSinsuCritical);
        
        return ret;
    }    
    //흉수증폭
    public static float GetEnhanceHyungsuCritical()
    {
        float ret = 0f;
        
        ret += GetBlackFoxEffect(StatusType.EnhanceHyungsuCritical);

        return ret;
    }   
    //영혼증폭
    public static float GetEnhanceSoulCritical()
    {
        float ret = 0f;

        ret += GetBlackFoxEffect(StatusType.EnhanceSoulCritical);

        return ret;
    }    
    //천구증폭
    public static float GetEnhanceChunguCritical()
    {
        float ret = 0f;

        ret += GetBlackFoxEffect(StatusType.EnhanceChunguCritical);

        return ret;
    }
    //초월증폭
    public static float GetEnhanceTransCritical()
    {
        float ret = 0f;

        ret += GetBlackFoxEffect(StatusType.EnhanceTransCritical);

        return ret;
    }
    //귀살증폭
    public static float GetEnhanceGwisalCritical()
    {
        float ret = 0f;

        ret += GetBlackFoxEffect(StatusType.EnhanceGwisalCritical);

        return ret;
    }
    //요도 증폭
    public static float GetEnhanceSealSword()
    {
        float ret = 0f;

        ret += GetMunhaHyulAbilValue(StatusType.EnhanceSealSword);
        
        
        return ret;
    }
    //공격력 증폭
    public static float GetEnhanceAttack()
    {
        float ret = 0f;

        ret += GetCaveBeltEnhanceAttackAdd();
        
        
        return ret;
    }
    //도술 증폭
    public static float GetEnhanceDosul()
    {
        float ret = 0f;

        ret += GetMunhaHyulAbilValue(StatusType.EnhanceDosul);
        
        return ret;
    }
    //궁극기 증폭
    public static float GetEnhanceVision()
    {
        float ret = 0f;

        ret += GetMunhaHyulAbilValue(StatusType.EnhanceVision);
        
        return ret;
    }
    //검기 능력치 효과 증가(%)
    public static float GetEnhanceSP()
    {
        float ret = 0f;
        
        ret += GetSwordGodAbil0();

        ret += GetSpecialTypeAbility(StatusType.EnhanceSP);

        return ret;
    }

 

    //요도 추가피해량
    public static float GetSealSwordDam()
    {
        float ret = 0f;
      
        ret += GetGuimoonHasEffect1(StatusType.SealSwordDam);
        
        ret += GetSealSwordCollectionHasValue(StatusType.SealSwordDam);
        
        ret += GetAwakeAbilityValue(AbilAwakeType.SealSword, StatusType.SealSwordDam);
        
        ret += GetSasinsuPowerAbility(StatusType.SealSwordDam,2);

        
        return ret;
    }
    //비기 추가 피해량
    public static float GetBigiDamPer()
    {
        
        float ret = 0f;

        ret += GetMunhaAbility(StatusType.BigiDamPer);
        
        ret += GetMunhaTowerAbility();
        
        ret += GetGuimoonHasEffect1(StatusType.BigiDamPer);

        
        return ret;
    }
    //요도 충전속도증가
    public static float GetSealSwordAttackSpeed()
    {
        float ret = 0f;
      
        ret += GetGuimoonHasEffect2(StatusType.SealAttackSpeed);
        
        return ret;
    }

    //도술 추가피해량
    public static float GetDosulDamPer()
    {
        float ret = 0f;
      
        ret += GetGuimoonHasEffect1(StatusType.DosulDamPer);
        
        ret += GetCurrentDosulAddValue();
        
        ret += GetAwakeAbilityValue(AbilAwakeType.Dosul, StatusType.DosulDamPer);
        
        ret += GetSasinsuPowerAbility(StatusType.DosulDamPer,0);

        return ret;
    }

    private static float GetTaeGukTowerValue()
    {
        float ret = 0f;

        int idx = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.taeguekTower].Value - 1;

        if (idx < 0)
        {
            return 0f;
        }

        var TableData = TableManager.Instance.taegeukTitle.dataArray[idx];

        return TableData.Abilvalue;
    }

   
    public static float GetHellMarkValue()
    {
        float ret = 0f;
        if (ServerData.goodsTable.GetTableData(GoodsTable.h0).Value == 1)
        {
            ret += TableManager.Instance.hellAbil.dataArray[0].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.h1).Value == 1)
        {
            ret += TableManager.Instance.hellAbil.dataArray[1].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.h2).Value == 1)
        {
            ret += TableManager.Instance.hellAbil.dataArray[2].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.h3).Value == 1)
        {
            ret += TableManager.Instance.hellAbil.dataArray[3].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.h4).Value == 1)
        {
            ret += TableManager.Instance.hellAbil.dataArray[4].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.h5).Value == 1)
        {
            ret += TableManager.Instance.hellAbil.dataArray[5].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.h6).Value == 1)
        {
            ret += TableManager.Instance.hellAbil.dataArray[6].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.h7).Value == 1)
        {
            ret += TableManager.Instance.hellAbil.dataArray[7].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.h8).Value == 1)
        {
            ret += TableManager.Instance.hellAbil.dataArray[8].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.h9).Value == 1)
        {
            ret += TableManager.Instance.hellAbil.dataArray[9].Abilbasevalue;
        }

        return ret;
    }

    public static float GetDarkMarkValue()
    {
        float ret = 0f;
        if (ServerData.goodsTable.GetTableData(GoodsTable.d0).Value > 0)
        {
            ret += TableManager.Instance.DarkAbil.dataArray[0].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.d1).Value > 0)
        {
            ret += TableManager.Instance.DarkAbil.dataArray[1].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.d2).Value > 0)
        {
            ret += TableManager.Instance.DarkAbil.dataArray[2].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.d3).Value > 0)
        {
            ret += TableManager.Instance.DarkAbil.dataArray[3].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.d4).Value > 0)
        {
            ret += TableManager.Instance.DarkAbil.dataArray[4].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.d5).Value > 0)
        {
            ret += TableManager.Instance.DarkAbil.dataArray[5].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.d6).Value > 0)
        {
            ret += TableManager.Instance.DarkAbil.dataArray[6].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.d7).Value > 0)
        {
            ret += TableManager.Instance.DarkAbil.dataArray[7].Abilbasevalue;
        }


        return ret;
    }


    public static float GetChunMarkValue()
    {
        float ret = 0f;
        if (ServerData.goodsTable.GetTableData(GoodsTable.c0).Value == 1)
        {
            ret += TableManager.Instance.chunMarkAbil.dataArray[0].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.c1).Value == 1)
        {
            ret += TableManager.Instance.chunMarkAbil.dataArray[1].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.c2).Value == 1)
        {
            ret += TableManager.Instance.chunMarkAbil.dataArray[2].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.c3).Value == 1)
        {
            ret += TableManager.Instance.chunMarkAbil.dataArray[3].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.c4).Value == 1)
        {
            ret += TableManager.Instance.chunMarkAbil.dataArray[4].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.c5).Value == 1)
        {
            ret += TableManager.Instance.chunMarkAbil.dataArray[5].Abilbasevalue;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.c6).Value == 1)
        {
            ret += TableManager.Instance.chunMarkAbil.dataArray[6].Abilbasevalue;
        }

        return ret;
    }

    public static float GetDokebiMarkValue()
    {
        float ret = 0f;

        return ret;
    }

    public static float GetYomulUpgradeValue(StatusType type, bool onlyType1 = false, bool onlyType2 = false,
        int targetId = -1)
    {
        float ret = 0f;
        var tableDatas = TableManager.Instance.YomulAbilTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var serverData = ServerData.yomulServerTable.TableDatas[tableDatas[i].Stringid];
            if (serverData.hasAbil.Value == 0) continue;
            if (targetId != -1 && i != targetId) continue;

            if (tableDatas[i].Abiltype == (int)type && onlyType2 == false)
            {
                if (type == StatusType.PenetrateDefense)
                {
                    float addValue = serverData.level.Value < 80
                        ? tableDatas[i].Abiladdvalue
                        : tableDatas[i].Abiladdvalue * 2f;
                    ret += tableDatas[i].Abilvalue + (serverData.level.Value * addValue);
                }
                else
                {
                    ret += tableDatas[i].Abilvalue + (serverData.level.Value * tableDatas[i].Abiladdvalue);
                }
            }

            if (tableDatas[i].Abiltype2 == (int)type && onlyType1 == false)
            {
                if (type == StatusType.PenetrateDefense)
                {
                    float addValue = tableDatas[i].Abiladdvalue2;
                    ret += tableDatas[i].Abilvalue2 + (serverData.level.Value * addValue);
                }
                else
                {
                    ret += tableDatas[i].Abilvalue2 + (serverData.level.Value * tableDatas[i].Abiladdvalue2);
                }
            }
        }

        return ret;
    }


    public static void ResetTitleHas()
    {
        titleHasValue.Clear();
    }

    public static float GetTitleAbilValue(StatusType type)
    {
        float ret = 0f;

        if (titleHasValue.ContainsKey(type))
        {
            ret = titleHasValue[type];
        }
        else
        {
            var dicData = TableManager.Instance.TitleAbils[(int)type];

            for (int i = 0; i < dicData.Count; i++)
            {
                if (ServerData.titleServerTable.TableDatas[dicData[i].Stringid].clearFlag.Value == 0) continue;

                if (dicData[i].Id == ServerData.equipmentTable.TableDatas[EquipmentTable.TitleSelectId].Value)
                {
                    ret += dicData[i].Abilvalue1 * GameBalance.TitleEquipAddPer;
                }
                else
                {
                    ret += dicData[i].Abilvalue1;
                }
            }

            titleHasValue.Add(type, ret);
        }


        return ret;
    }

    public static float GetTitleLevelAbilValue(StatusType type)
    {
        float ret = 0f;

        if (titleHasValue.ContainsKey(type))
        {
            ret = titleHasValue[type];
        }
        else
        {
            var tableData = TableManager.Instance.titleLevel.dataArray;
            var currentLevel = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.titleLevel).Value;
            for (int i = 0; i < tableData.Length; i++)
            {
                //현재 레벨 초과시 break;
                if (currentLevel < i) break;

                ret += tableData[i].Abilvalue1;
            }

            titleHasValue.Add(type, ret);
        }


        return ret;
    }

    public static float GetTitleStageAbilValue(StatusType type)
    {
        float ret = 0f;

        if (titleHasValue.ContainsKey(type))
        {
            ret = titleHasValue[type];
        }
        else
        {
            var tableData = TableManager.Instance.titleStage.dataArray;
            var currentLevel = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.titleStage).Value;
            for (int i = 0; i < tableData.Length; i++)
            {
                //현재 레벨 초과시 break;
                if (currentLevel < i) break;

                ret += tableData[i].Abilvalue1;
            }

            titleHasValue.Add(type, ret);
        }


        return ret;
    }

    public static float GetTitleWeaponAbilValue(StatusType type)
    {
        float ret = 0f;

        if (titleHasValue.ContainsKey(type))
        {
            ret = titleHasValue[type];
        }
        else
        {
            var tableData = TableManager.Instance.titleWeapon.dataArray;
            var currentLevel = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.titleWeapon).Value;
            for (int i = 0; i < tableData.Length; i++)
            {
                //현재 레벨 초과시 break;
                if (currentLevel < i) break;

                ret += tableData[i].Abilvalue1;
            }

            titleHasValue.Add(type, ret);
        }


        return ret;
    }

    public static float GetTitleMagicBookAbilValue(StatusType type)
    {
        float ret = 0f;

        if (titleHasValue.ContainsKey(type))
        {
            ret = titleHasValue[type];
        }
        else
        {
            var tableData = TableManager.Instance.titleMagicBook.dataArray;
            var currentLevel = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.titleMagicBook).Value;
            for (int i = 0; i < tableData.Length; i++)
            {
                //현재 레벨 초과시 break;
                if (currentLevel < i) break;

                ret += tableData[i].Abilvalue1;
            }

            titleHasValue.Add(type, ret);
        }


        return ret;
    }

    public static float GetHotTimeBuffEffect(StatusType statusType)
    {
        if (ServerData.userInfoTable.IsWeekend() == false)
        {
            float ret = 0f;

            if (ServerData.userInfoTable.IsHotTime() == false) return 0f;

            if (statusType == StatusType.GoldGainPer)
            {
                ret = GameBalance.HotTime_Gold;
            }
            else if (statusType == StatusType.ExpGainPer)
            {
                ret = GameBalance.HotTime_Exp;
            }
            else if (statusType == StatusType.MagicStoneAddPer)
            {
                ret = GameBalance.HotTime_GrowthStone;
            }
            else if (statusType == StatusType.MarbleAddPer)
            {
                ret = GameBalance.HotTime_Marble;
            }

            return ret;
        }
        else
        {
            float ret = 0f;

            if (ServerData.userInfoTable.IsHotTime() == false) return 0f;

            if (statusType == StatusType.GoldGainPer)
            {
                ret = GameBalance.HotTime_Gold_Weekend;
            }
            else if (statusType == StatusType.ExpGainPer)
            {
                ret = GameBalance.HotTime_Exp_Weekend;
            }
            else if (statusType == StatusType.MagicStoneAddPer)
            {
                ret = GameBalance.HotTime_GrowthStone_Weekend;
            }
            else if (statusType == StatusType.MarbleAddPer)
            {
                ret = GameBalance.HotTime_Marble_Weekend;
            }

            return ret;
        }
    }
    //홀수 월간버프
    public static float GetMonth2BuffEffect(StatusType statusType)
    {
        float ret = 0f;
        //홀수가 아니면 0
        if (ServerData.userInfoTable.IsMonthlyPass2() != true) return 0f;
        //패스권없으면 0 
        if (ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton2.monthPassKey].buyCount.Value < 1) return 0f;

        if (statusType == StatusType.GoldGainPer)
        {
            return GameBalance.MonthPass_Gold;
        }
        else if (statusType == StatusType.ExpGainPer)
        {
            return  GameBalance.MonthPass_Exp;
        }
        else if (statusType == StatusType.MagicStoneAddPer)
        {
            return  GameBalance.MonthPass_GrowthStone;
        }

        var tableData =  TableManager.Instance.MonthBuff.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            //홀수가 아니면
            if (tableData[i].Monthsort != true) continue;
            //타입이 아니면
            if(statusType!=(StatusType)tableData[i].Statustype) continue;
            //타입이 같으면
            return tableData[i].Statusvalue;
        }
        
        return ret;
    }
    //짝수 월간버프
    public static float GetMonthBuffEffect(StatusType statusType)
    {
        float ret = 0f;
        //짝수가 아니면 0
        if (ServerData.userInfoTable.IsMonthlyPass2() != false) return 0f;
        //패스권없으면 0 
        if (ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton.monthPassKey].buyCount.Value < 1) return 0f;

        if (statusType == StatusType.GoldGainPer)
        {
            return GameBalance.MonthPass_Gold;
        }
        else if (statusType == StatusType.ExpGainPer)
        {
            return  GameBalance.MonthPass_Exp;
        }
        else if (statusType == StatusType.MagicStoneAddPer)
        {
            return  GameBalance.MonthPass_GrowthStone;
        }

        var tableData =  TableManager.Instance.MonthBuff.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            // 짝수가 아니면
            if (tableData[i].Monthsort != false) continue;
            //타입이 아니면
            if(statusType!=(StatusType)tableData[i].Statustype) continue;
            //타입이 같으면
            return tableData[i].Statusvalue;
        }
        
        return ret;
    }

    public static float GetHotTimeEventBuffEffect(StatusType statusType)
    {
        float ret = 0f;

        if (ServerData.userInfoTable.IsHotTimeEvent() == false) return 0f;

        if (statusType == StatusType.GoldGainPer)
        {
            ret = GameBalance.HotTimeEvent_Gold;
            if (Utils.HasHotTimeEventPass())
            {
                ret += GameBalance.HotTimeEvent_Ad_Gold;
            }
        }
        else if (statusType == StatusType.ExpGainPer)
        {
            ret = GameBalance.HotTimeEvent_Exp;
            if (Utils.HasHotTimeEventPass())
            {
                ret += GameBalance.HotTimeEvent_Ad_Exp;
            }
        }
        else if (statusType == StatusType.MagicStoneAddPer)
        {
            ret = GameBalance.HotTimeEvent_GrowthStone;
            if (Utils.HasHotTimeEventPass())
            {
                ret += GameBalance.HotTimeEvent_Ad_GrowthStone;
            }
        }
        else if (statusType == StatusType.MarbleAddPer)
        {
            ret = GameBalance.HotTimeEvent_Marble;
            if (Utils.HasHotTimeEventPass())
            {
                ret += GameBalance.HotTimeEvent_Ad_Marble;
            }
        }
        else if (statusType == StatusType.YoPowerGoodsGainPer)
        {
            ret = GameBalance.HotTimeEvent_YoPowerGoods;
            if (Utils.HasHotTimeEventPass())
            {
                ret += GameBalance.HotTimeEvent_Ad_YoPowerGoods;
            }
        }
        else if (statusType == StatusType.TaegeukGoodsGainPer)
        {
            ret = GameBalance.HotTimeEvent_TaegeukGoods;
            if (Utils.HasHotTimeEventPass())
            {
                ret += GameBalance.HotTimeEvent_Ad_TaegeukGoods;
            }
        }
        else if (statusType == StatusType.SasinsuGoodsGainPer)
        {
            ret = GameBalance.HotTimeEvent_SasinsuGoods;
            if (Utils.HasHotTimeEventPass())
            {
                ret += GameBalance.HotTimeEvent_Ad_SasinsuGoods;
            }
        }

        return ret;
    }



    private static void ResetSinsuHas()
    {
        sinsuHasValue.Clear();
    }

    public static float GetSinsuEquipEffect(StatusType statusType)
    {
        float ret = 0f;

        if (sinsuHasValue.ContainsKey(statusType))
        {
            ret = sinsuHasValue[statusType];
        }
        else
        {
            var tableDatas = TableManager.Instance.PetEquipment.dataArray;

            var petEquipLevel = ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value;

            for (int i = 0; i < tableDatas.Length; i++)
            {
                var serverData = ServerData.petEquipmentServerTable.TableDatas[tableDatas[i].Stringid];

                if (serverData.hasAbil.Value == 0) continue;

                if (tableDatas[i].Abiltype1 == (int)statusType)
                {
                    ret += (tableDatas[i].Abilvalue1 + serverData.level.Value * tableDatas[i].Abiladdvalue1 +
                            tableDatas[i].Leveladdvalue1 * petEquipLevel);
                }

                if (tableDatas[i].Abiltype2 == (int)statusType)
                {
                    ret += (tableDatas[i].Abilvalue2 + serverData.level.Value * tableDatas[i].Abiladdvalue2 +
                            tableDatas[i].Leveladdvalue2 * petEquipLevel);
                }
            }

            sinsuHasValue.Add(statusType, ret);
        }


        if (ActiveSmithValue(statusType))
        {
            ret = ret * GetSmithValue(StatusType.PetEquipHasUp);
        }
        else
        {
        }

        return ret;
    }

    private static bool ActiveSmithValue(StatusType statustype)
    {
        return statustype != StatusType.Damdecrease && statustype != StatusType.SuperCritical1Prob &&
               statustype != StatusType.ExpGainPer&&
               statustype != StatusType.SuperCritical24DamPer&&
               statustype != StatusType.SuperCritical34DamPer;
    }

    public static float GetRelicHasEffect(StatusType statusType)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.RelicTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var serverData = ServerData.relicServerTable.TableDatas[tableDatas[i].Stringid];

            if (serverData.level.Value == 0) continue;

            if (tableDatas[i].Abiltype != (int)statusType) continue;

            ret += serverData.level.Value * tableDatas[i].Abilvalue;
        }

        if (statusType == StatusType.Hp ||
            statusType == StatusType.HpAddPer ||
            statusType == StatusType.Damdecrease
           )
        {
            return ret;
        }
        else
        {
            return ret * (GetSpecialAbilRing() + GetRelicTestAbilRing()+GetRelicUpgradeAbilValue(GetCurrentRelicUpgradeIdx()));
        }
    }

    private static Dictionary<StatusType, float> stageRelicValue = new Dictionary<StatusType, float>();
    private static Dictionary<StatusType, float> guimoonValue = new Dictionary<StatusType, float>();
    private static Dictionary<StatusType, float> blackFoxValue = new Dictionary<StatusType, float>();
    private static Dictionary<StatusType, float> meditationDictionary = new Dictionary<StatusType, float>();
    private static Dictionary<StatusType, float> munhaHyulDictionary = new Dictionary<StatusType, float>();
    private static Dictionary<StatusType, float> magicBookHasValue = new Dictionary<StatusType, float>();
    private static Dictionary<StatusType, float> titleHasValue = new Dictionary<StatusType, float>();
    private static Dictionary<StatusType, float> sinsuHasValue = new Dictionary<StatusType, float>();
    private static Dictionary<StatusType, float> haetalValue = new Dictionary<StatusType, float>();

    private static bool meditationInitialize = false;
    private static bool munhaHyulInitialize = false;

    private static void ResetStageRelicHas()
    {
        stageRelicValue.Clear();
    }
    private static void ResetGuimoonRelicHas()
    {
        guimoonValue.Clear();
    }
    public static void ResetBlackFoxHas()
    {
        blackFoxValue.Clear();
    }
    public static void ResetHaetal()
    {
        haetalValue.Clear();
    }

    public const float divideNum = 500f;
    public const float divideAbilValue = 0.5f;

    //유물복원
    public static float GetStageRelicHasEffect(StatusType statusType)
    {
        float ret = 0f;

        if (stageRelicValue.ContainsKey(statusType))
        {
            ret = stageRelicValue[statusType];
        }
        else
        {
            float totalLevel = ServerData.stageRelicServerTable.GetTotalStageRelicLevel();

            var tableDatas = TableManager.Instance.StageRelic.dataArray;

            for (int i = 0; i < tableDatas.Length; i++)
            {
                var serverData = ServerData.stageRelicServerTable.TableDatas[tableDatas[i].Stringid];

                if (tableDatas[i].Abiltype != (int)statusType) continue;

                if (serverData.level.Value == 0 && tableDatas[i].Istotalskill == false) continue;

                if (tableDatas[i].Istotalskill)
                {
                    //언락 체크
                    if (ServerData.goodsTable.GetTableData(tableDatas[i].Requiregoods).Value >=
                        tableDatas[i].Requiregoodsvalue)
                    {
                        ret += totalLevel * tableDatas[i].Abilvalue;
                    }
                }
                else
                {
                    ret += serverData.level.Value * tableDatas[i].Abilvalue;
                }
            }

            stageRelicValue.Add(statusType, ret);
        }

        ret *= GetStageRelicUpgradeValue();

        return ret;
    }
    public static float GetGuimoonHasEffect1(StatusType statusType)
    {
        float ret = 0f;

        if (guimoonValue.ContainsKey(statusType))
        {
            ret = guimoonValue[statusType];
        }
        else
        {
            var tableDatas = TableManager.Instance.GuimoonTable.dataArray;

            for (int i = 0; i < tableDatas.Length; i++)
            {
                var serverData = ServerData.guimoonServerTable.TableDatas[tableDatas[i].Stringid];

                if (tableDatas[i].Abiltype1 != (int)statusType) continue;

                if (serverData.level1.Value == 0) continue;

                ret += serverData.level1.Value * tableDatas[i].Abilvalue1;
            }
            guimoonValue.Add(statusType, ret);
        }

        return ret;
    }
    public static float GetHaetalValue(StatusType statusType)
    {
        float ret = 0f;
        
        var grade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.haetalGradeIdx).Value;

        if (grade < 0)
        {
            return 0f;
        }
        
        if (haetalValue.ContainsKey(statusType))
        {
            ret = haetalValue[statusType];
        }
        else
        {
            var tableDatas = TableManager.Instance.HaetalTable.dataArray;

            for (int i = 0; i <= grade; i++)
            {
                if (tableDatas[i].Abiltype != (int)statusType) continue;

                ret += tableDatas[i].Abilvalue;
            }
            haetalValue.Add(statusType, ret);
        }

        return ret;
    }
    public static float GetBlackFoxEffect(StatusType statusType)
    {
        float ret = 0f;

        
        if (blackFoxValue.ContainsKey(statusType))
        {
            ret = blackFoxValue[statusType];
        }
        else
        {
            var tableDatas = TableManager.Instance.BlackFoxAbil.dataArray;

            for (int i = 0; i < tableDatas.Length; i++)
            {
                var serverData = ServerData.blackFoxServerTable.TableDatas[tableDatas[i].Stringid];

                if (tableDatas[i].Abiltype != (int)statusType) continue;

                if (serverData.level.Value == 0) continue;

                ret += serverData.level.Value * tableDatas[i].Abilvalue;
            }
            blackFoxValue.Add(statusType, ret);
        }

        return ret;
    }
    public static float GetGuimoonHasEffect2(StatusType statusType)
    {
        float ret = 0f;

        if (guimoonValue.ContainsKey(statusType))
        {
            ret = guimoonValue[statusType];
        }
        else
        {
            var tableDatas = TableManager.Instance.GuimoonTable.dataArray;

            for (int i = 0; i < tableDatas.Length; i++)
            {
                var serverData = ServerData.guimoonServerTable.TableDatas[tableDatas[i].Stringid];

                if (tableDatas[i].Abiltype2 != (int)statusType) continue;

                if (serverData.level2.Value == 0) continue;

                ret += serverData.level2.Value * tableDatas[i].Abilvalue2;
            }

            guimoonValue.Add(statusType, ret);
        }

        return ret;
    }

    public static float GetStageAddValue()
    {
        int currentStage = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.topClearStageId].Value;

        float divide = (int)(currentStage / divideNum);

        return (1 + divide * divideAbilValue);
    }

    public static float GetSonAbilHasEffect(StatusType statusType, float addLevel = 0f)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.SonAbil.dataArray;

        var currentLevel = ServerData.statusTable.GetTableData(StatusTable.Son_Level).Value + addLevel;

        currentLevel += (int)ServerData.userInfoTable.TableDatas[UserInfoTable.sonCloneClear].Value *
                        GameBalance.sonCloneAddValue;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (currentLevel < tableDatas[i].Unlocklevel) continue;
            if (statusType != (StatusType)tableDatas[i].Abiltype) continue;

            var calculatedLevel = currentLevel - tableDatas[i].Unlocklevel;

            ret += tableDatas[i].Abilvalue + calculatedLevel * tableDatas[i].Abiladdvalue;
        }

        ret = ret * (1 + GetSonAbilPlusValue() + GetSonTransPlusValue());

        return ret * (1 + GetPeachAbilValue());
    }

    public static float GetHellAbilHasEffect(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.hellAbilBase.dataArray;

        int currentLevel = (int)ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value + addLevel;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (currentLevel < tableDatas[i].Unlocklevel) continue;
            if (statusType != (StatusType)tableDatas[i].Abiltype) continue;

            int calculatedLevel = currentLevel - tableDatas[i].Unlocklevel;

            ret += tableDatas[i].Abilvalue + calculatedLevel * (tableDatas[i].Abiladdvalue + GetHellFireHasAddValue());
        }


        float hellPowerAddValue = GetHellPowerAddValue();
        float hellTransAddValue = GetHelTransPlusValue();
        ret = (ret * (1 + hellPowerAddValue)) * hellTransAddValue;

        return ret * GetHellGodAbil0();
    }

    public static float GetChunAbilHasEffect(StatusType statusType, int addLevel = 0)
    {
        var ret = 0f;

        var tableDatas = TableManager.Instance.chunAbilBase.dataArray;

        var currentLevel = (int)ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value + addLevel;

        for (var i = 0; i < tableDatas.Length; i++)
        {
            if (currentLevel < tableDatas[i].Unlocklevel) continue;
            if (statusType != (StatusType)tableDatas[i].Abiltype) continue;

            var calculatedLevel = currentLevel - tableDatas[i].Unlocklevel;

            ret += tableDatas[i].Abilvalue + calculatedLevel * tableDatas[i].Abiladdvalue;
        }

        if (statusType == StatusType.SuperCritical4DamPer)
        {
            if (IsChunFlowerDamageEnhance())
            {
                ret += 0.000015f * currentLevel;
            }

            ret += GetChunFlowerHasAddValue() * currentLevel;
        }

        var chunTransAddValue = GetChunTransPlusValue();
        ret = (ret * chunTransAddValue);

        return ret * GetChunGodAbil0();
    }

    public static float GetDokebiFireAbilHasEffect(StatusType statusType, int addLevel = 0)
    {
        //if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 500000) return 0f;
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < 11900-2) return 0f;
        
        float ret = 0f;

        var tableDatas = TableManager.Instance.dokebiAbilBase.dataArray;

        int currentLevel = (int)ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value + addLevel;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (currentLevel < tableDatas[i].Unlocklevel) continue;
            if (statusType != (StatusType)tableDatas[i].Abiltype) continue;

            int calculatedLevel = currentLevel - tableDatas[i].Unlocklevel;

            ret += tableDatas[i].Abilvalue + calculatedLevel * tableDatas[i].Abiladdvalue;
        }

        if (statusType == StatusType.SuperCritical5DamPer)
        {
            ret += GetDokebiFireHasAddValue() * currentLevel;
        }

        var dokebiTransAddValue = GetDokebiTransPlusValue();
        ret = (ret * dokebiTransAddValue);

        return ret * GetDoGodAbil0() + ret * GetDokebiFireEnhanceAbilPlusValue();
    }

    public static float GetSumiFireAbilHasEffect(StatusType statusType, int addLevel = 0)
    {
        //if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 1000000) return 0f;
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < 13900-2) return 0f;
        
        float ret = 0f;

        var tableDatas = TableManager.Instance.sumiAbilBase.dataArray;

        int currentLevel = (int)ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value + addLevel;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (currentLevel < tableDatas[i].Unlocklevel) continue;
            if (statusType != (StatusType)tableDatas[i].Abiltype) continue;

            int calculatedLevel = currentLevel - tableDatas[i].Unlocklevel;

            ret += tableDatas[i].Abilvalue + calculatedLevel * tableDatas[i].Abiladdvalue;
        }

        if (statusType == StatusType.SuperCritical7DamPer)
        {
            ret += GetSumiFireHasAddValue() * currentLevel;
        }
        var transAddValue = GetSumiTransPlusValue();
        
        ret = (ret * transAddValue);
        
        return ret * GetSumiGodAbil0();
    }

    public static float GetTresureAbilHasEffect(StatusType statusType, int addLevel = 0)
    {
        //if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 2000000) return 0f;
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < 14900-2) return 0f;

        float ret = 0f;

        var tableDatas = TableManager.Instance.tresureAbilBase.dataArray;

        int currentLevel = (int)ServerData.goodsTable.GetTableData(GoodsTable.Tresure).Value + addLevel;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (currentLevel < tableDatas[i].Unlocklevel) continue;
            if (statusType != (StatusType)tableDatas[i].Abiltype) continue;

            int calculatedLevel = currentLevel - tableDatas[i].Unlocklevel;

            ret += tableDatas[i].Abilvalue + calculatedLevel * tableDatas[i].Abiladdvalue;
        }

        if (statusType == StatusType.SuperCritical10DamPer)
        {
            ret += GetTreasureHasAddValue() * currentLevel;
        }

        return ret * GetThiefGodAbil0();
    }

    public static float GetDarkTreasureAbilHasEffect(StatusType statusType, int addLevel = 0)
    {
        //if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 2000000) return 0f;
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < 14900-2) return 0f;
        float ret = 0f;

        var tableDatas = TableManager.Instance.DarkTreasureAbilBase.dataArray;

        int currentLevel = (int)ServerData.goodsTable.GetTableData(GoodsTable.DarkTreasure).Value + addLevel;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (currentLevel < tableDatas[i].Unlocklevel) continue;
            if (statusType != (StatusType)tableDatas[i].Abiltype) continue;

            int calculatedLevel = currentLevel - tableDatas[i].Unlocklevel;

            ret += tableDatas[i].Abilvalue + calculatedLevel * tableDatas[i].Abiladdvalue;
        }

        if (statusType == StatusType.SuperCritical12DamPer)
        {
            ret += GetDarkTreasureHasAddValue() * currentLevel;
        }

        return ret * GetDarkGodAbil0();
    }

    public static float GetSinsunTreasureAbilHasEffect(StatusType statusType, int addLevel = 0)
    {
       // if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 3000000) return 0f;
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < 16900-2) return 0f;
        
        float ret = 0f;

        var tableDatas = TableManager.Instance.SinsunTreasureAbilBase.dataArray;

        int currentLevel = (int)ServerData.goodsTable.GetTableData(GoodsTable.SinsunTreasure).Value + addLevel;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (currentLevel < tableDatas[i].Unlocklevel) continue;
            if (statusType != (StatusType)tableDatas[i].Abiltype) continue;

            int calculatedLevel = currentLevel - tableDatas[i].Unlocklevel;

            ret += tableDatas[i].Abilvalue + calculatedLevel * tableDatas[i].Abiladdvalue;
        }

        if (statusType == StatusType.SuperCritical15DamPer)
        {
            ret += GetSinsunTreasureHasAddValue() * currentLevel;
        }
        
        return ret * GetSinsunGodAbil0();
    }
    public static float GetDragonScaleAbilHasEffect()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < 17900-2) return 0f;

        return ((int)ServerData.goodsTable.GetTableData(GoodsTable.DragonScale).Value * (GameBalance.dragonScaleAbilValue+GetDragonScaleHasValueUpgrade()));
    }

    public static float GetDragonPalaceTreasureAbilHasEffect()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < 17900-2) return 0f;

        return ((int)ServerData.goodsTable.GetTableData(GoodsTable.DragonPalaceTreasure).Value *
                (GameBalance.dragonPalaceTreasureAbilValue + GetDragonPalaceHasValueUpgrade()));
    }

    public static float GetMurimTreasureAbilHasEffect()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < 21399-2) return 0f;

        return ((int)ServerData.goodsTable.GetTableData(GoodsTable.MRT).Value * (GameBalance.murimTreasureAbilValue + GetMurimHasValueUpgrade()));
    }

    public static float GetDifficultyBossTreasureAbilHasEffect()
    {
        return ((int)ServerData.goodsTable.GetTableData(GoodsTable.DBT).Value *
                (GameBalance.difficultyBossTreasureAbilValue + GetDBTHasValueUpgrade()));
    }

    public static float GetYeonOkTreasureAbilHasEffect()
    {
        return ((int)ServerData.goodsTable.GetTableData(GoodsTable.YOT).Value *
                (GameBalance.yeonOkBossTreasureAbilValue + GetYOTHasValueUpgrade()));
    }
    public static float GetChunsangTreasureAbilHasEffect()
    {
        return ((int)ServerData.goodsTable.GetTableData(GoodsTable.CST).Value *
                (GameBalance.chunSangTreasureAbilValue + GetCSTHasValueUpgrade()));
    }

    public static float GetGwisalTreasureAbilHasEffect(StatusType statusType, int addLevel = 0)
    {
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 2000000) return 0f;

        float ret = 0f;

        var tableDatas = TableManager.Instance.GwisalTreasureAbilBase.dataArray;

        int currentLevel = (int)ServerData.goodsTable.GetTableData(GoodsTable.GwisalTreasure).Value + addLevel;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (currentLevel < tableDatas[i].Unlocklevel) continue;
            if (statusType != (StatusType)tableDatas[i].Abiltype) continue;

            int calculatedLevel = currentLevel - tableDatas[i].Unlocklevel;

            ret += tableDatas[i].Abilvalue + calculatedLevel * tableDatas[i].Abiladdvalue;
        }       
        if (statusType == StatusType.SuperCritical19DamPer)
        {
            ret += GetGwisalTreasureHasAddValue() * currentLevel;
        }

        return ret;
    }


    public static float GetChunguTreasureAbilHasEffect(int addLevel = 0)
    {
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 3000000) return 0f;

        float ret = 0f;

        int currentLevel = (int)ServerData.goodsTable.GetTableData(GoodsTable.ChunguTreasure).Value + addLevel;

        ret += currentLevel * GameBalance.chunguAbil;

        return ret;
    }
    public static float GetSuhoTreasureAbilHasEffect(int addLevel = 0)
    {
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 4000000) return 0f;

        float ret = 0f;

        int currentLevel = (int)ServerData.goodsTable.GetTableData(GoodsTable.SuhoTreasure).Value + addLevel;

        ret += currentLevel * GameBalance.suhoTreasureAbil;

        return ret;
    }

    public static float GetSumiTowerEffect(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.sumisanTowerTable.dataArray;

        int currentLevel = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx4).Value;

        if (currentLevel == 0)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            if ((StatusType)tableDatas[i].Rewardtype == statusType)
            {
                ret += tableDatas[i].Rewardvalue;
            }
        }

        return ret;
    }

    public static float GetGyungRockEffect(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.gyungRockTowerTable.dataArray;

        int currentLevel = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx5).Value;

        if (currentLevel == 0)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            if ((StatusType)tableDatas[i].Rewardtype == statusType)
            {
                ret += tableDatas[i].Rewardvalue;
            }
        }

        return ret;
    }

    public static float GetGyungRockEffect2(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.gyungRockTowerTable2.dataArray;

        int currentLevel = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx7).Value;

        if (currentLevel == 0)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            if ((StatusType)tableDatas[i].Rewardtype == statusType)
            {
                ret += tableDatas[i].Rewardvalue;
            }
        }

        return ret;
    }

    public static float GetFoxFireEffect(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.FoxFire.dataArray;

        int currentLevel = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxFireIdx).Value;

        if (currentLevel == -1)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel + 1; i++)
        {
            if ((StatusType)tableDatas[i].Abil_Type == statusType)
            {
                ret += tableDatas[i].Abil_Value;
            }
        }

        return ret;
    }
    public static float GetYoPowerEffect(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.YokaiPowerOpen.dataArray;

        int currentLevel = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.yoPowerIdx).Value;

        if (currentLevel == -1)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel + 1; i++)
        {
            if ((StatusType)tableDatas[i].Abil_Type == statusType)
            {
                ret += tableDatas[i].Abil_Value;
            }
        }

        return ret;
    }

    public static float GetSmithValue(StatusType statusType)
    {
        int currentExp = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.smithExp].Value;

        currentExp += (int)ServerData.userInfoTable.TableDatas[UserInfoTable.smithTreeClear].Value *
                      (int)GameBalance.smithTreeAddValue;

        if (statusType == StatusType.growthStoneUp)
        {
            if (currentExp < 1000)
            {
                return 0;
            }
            else
            {
                currentExp = Mathf.Min(currentExp, 60000);

                int divide = currentExp / 1000;

                return (1 + (divide)) * 10;
            }
        }
        else if (statusType == StatusType.WeaponHasUp ||
                 statusType == StatusType.NorigaeHasUp ||
                 statusType == StatusType.PetEquipHasUp)
        {
            return 1f + (currentExp / 2500) * 0.05f;
        }
        else if (statusType == StatusType.PetEquipProbUp)
        {
            currentExp = Mathf.Min(currentExp, 50000);

            int divide = currentExp / 10000;

            return divide * 10;
        }

        return 0f;
    }

    public static float GetFeelMulAddDam()
    {
        return ServerData.statusTable.GetTableData(StatusTable.FeelMul).Value * 0.1f;
    }

    public static float GetLeeMuGiAddDam()
    {
        return ServerData.statusTable.GetTableData(StatusTable.LeeMuGi).Value * 0.2f;
    }

    public static string asuraKey0 = "a0";
    public static string asuraKey1 = "a1";
    public static string asuraKey2 = "a2";
    public static string asuraKey3 = "a3";
    public static string asuraKey4 = "a4";
    public static string asuraKey5 = "a5";

    public static string indraKey0 = "i0";
    public static string indraKey1 = "i1";
    public static string indraKey2 = "i2";

    public static string orochi0 = "or0";
    public static string orochi1 = "or1";


    public static ObscuredFloat asura0Value = 15000f;
    public static ObscuredFloat asura1Value = 25000f;
    public static ObscuredFloat asura2Value = 300f;
    public static ObscuredFloat asura3Value = 0.5f;
    public static ObscuredFloat asura4Value = 0.8f;
    public static ObscuredFloat asura5Value = 1.0f;

    public static ObscuredFloat indra0Value = 50000;
    public static ObscuredFloat indra1Value = 70000;
    public static ObscuredFloat indra2Value = 0.001f;

    public static ObscuredFloat orochi0Value = 0.001f;
    public static ObscuredFloat orochi1Value = 0.002f;

    public static ObscuredFloat gumihoValue0 = 5000;
    public static ObscuredFloat gumihoValue1 = 10000;
    public static ObscuredFloat gumihoValue2 = 15000;
    public static ObscuredFloat gumihoValue3 = 30000;
    public static ObscuredFloat gumihoValue4 = 40000;
    public static ObscuredFloat gumihoValue5 = 50000;
    public static ObscuredFloat gumihoValue6 = 70000;
    public static ObscuredFloat gumihoValue7 = 90000;
    public static ObscuredFloat gumihoValue8 = 120000;


    public static float GetGumihoAbil()
    {
        float ret = 0f;

        ret += ServerData.goodsTable.GetTableData(GoodsTable.gumiho0).Value == 1 ? gumihoValue0 : 0;
        ret += ServerData.goodsTable.GetTableData(GoodsTable.gumiho1).Value == 1 ? gumihoValue1 : 0;
        ret += ServerData.goodsTable.GetTableData(GoodsTable.gumiho2).Value == 1 ? gumihoValue2 : 0;
        ret += ServerData.goodsTable.GetTableData(GoodsTable.gumiho3).Value == 1 ? gumihoValue3 : 0;
        ret += ServerData.goodsTable.GetTableData(GoodsTable.gumiho4).Value == 1 ? gumihoValue4 : 0;
        ret += ServerData.goodsTable.GetTableData(GoodsTable.gumiho5).Value == 1 ? gumihoValue5 : 0;
        ret += ServerData.goodsTable.GetTableData(GoodsTable.gumiho6).Value == 1 ? gumihoValue6 : 0;
        ret += ServerData.goodsTable.GetTableData(GoodsTable.gumiho7).Value == 1 ? gumihoValue7 : 0;
        ret += ServerData.goodsTable.GetTableData(GoodsTable.gumiho8).Value == 1 ? gumihoValue8 : 0;

        return ret;
    }

    public static float GetAsuraAbilValue(StatusType type)
    {
        switch (type)
        {
            case StatusType.AttackAddPer:
            {
                if (ServerData.goodsTable.GetTableData(asuraKey0).Value == 0)
                {
                    return 0f;
                }

                return asura0Value;
            }
                break;
            case StatusType.IgnoreDefense:
            {
                if (ServerData.goodsTable.GetTableData(asuraKey1).Value == 0)
                {
                    return 0f;
                }

                return asura1Value;
            }
                break;
            case StatusType.SuperCritical1DamPer:
            {
                if (ServerData.goodsTable.GetTableData(asuraKey2).Value == 0)
                {
                    return 0f;
                }

                return asura2Value;
            }
                break;
            case StatusType.SuperCritical2DamPer:
            {
                float ret = 0f;

                if (ServerData.goodsTable.GetTableData(asuraKey3).Value == 0)
                {
                }
                else
                {
                    ret += asura3Value;
                }

                if (ServerData.goodsTable.GetTableData(asuraKey4).Value == 0)
                {
                }
                else
                {
                    ret += asura4Value;
                }

                if (ServerData.goodsTable.GetTableData(asuraKey5).Value == 0)
                {
                }
                else
                {
                    ret += asura5Value;
                }

                return ret;
            }
                break;
        }

        return 0f;
    }

    public static float GetIndraAbilValue(StatusType type)
    {
        switch (type)
        {
            case StatusType.IgnoreDefense:
            {
                float ret = 0f;

                if (ServerData.goodsTable.GetTableData(indraKey0).Value != 0)
                {
                    ret += indra0Value;
                }

                if (ServerData.goodsTable.GetTableData(indraKey1).Value != 0)
                {
                    ret += indra1Value;
                }

                return ret;
            }
                break;

            case StatusType.PenetrateDefense:
            {
                if (ServerData.goodsTable.GetTableData(indraKey2).Value == 0)
                {
                    return 0f;
                }

                return indra2Value;
            }
                break;
        }

        return 0f;
    }

    public static float GetOrochiAbilValue(StatusType type)
    {
        switch (type)
        {
            case StatusType.PenetrateDefense:
            {
                float ret = 0f;

                if (ServerData.goodsTable.GetTableData(orochi0).Value != 0)
                {
                    ret += orochi0Value;
                }

                if (ServerData.goodsTable.GetTableData(orochi1).Value != 0)
                {
                    ret += orochi1Value;
                }

                return ret;
            }
                break;
        }

        return 0f;
    }

    public static float GetGuildPetEffect(StatusType type)
    {
        int petLevel = GuildManager.Instance.guildPetExp.Value;

        switch (type)
        {
            case StatusType.AttackAddPer:
            {
                return petLevel *GameBalance.guildPet_attack;
            }
                break;
            case StatusType.ExpGainPer:
            {
                return petLevel * GameBalance.guildPet_exp;
            }
                break;
            case StatusType.GoldGainPer:
            {
                return petLevel * GameBalance.guildPet_gold;
            }
                break;
            case StatusType.MagicStoneAddPer:
            {
                return petLevel * GameBalance.guildPet_getGrowthStone;
            }
                break;
        }

        return 0f;
    }

    private static string adukCostumeKey = "costume26";
    private static string leeMuGiCostumeKey = "costume27";
    private static string nataCostumeKey = "costume35";
    private static string foxCostumeKey = "costume40";

    private static string hellCostumeKey0 = "costume43";
    private static string hellCostumeKey1 = "costume44";

    public static float DecreaseBossHp()
    {
        float ret = 0f;

        if (ServerData.costumeServerTable.TableDatas[adukCostumeKey].hasCostume.Value == true)
        {
            ret += 0.05f;
        }

        if (ServerData.costumeServerTable.TableDatas[leeMuGiCostumeKey].hasCostume.Value == true)
        {
            ret += 0.05f;
        }

        if (ServerData.costumeServerTable.TableDatas[nataCostumeKey].hasCostume.Value == true)
        {
            ret += 0.1f;
        }

        if (ServerData.costumeServerTable.TableDatas[foxCostumeKey].hasCostume.Value == true)
        {
            ret += 0.1f;
        }

        //
        if (ServerData.costumeServerTable.TableDatas[hellCostumeKey0].hasCostume.Value == true)
        {
            ret += 0.05f;
        }

        if (ServerData.costumeServerTable.TableDatas[hellCostumeKey1].hasCostume.Value == true)
        {
            ret += 0.1f;
        }

        if (IsChunBossHpDec())
        {
            ret += 0.05f;
        }

        ret += GetGuimoonHasEffect2(StatusType.DecreaseBossHp);
        
        return ret;
    }

    public static float GetGoldAbilAddRatio()
    {
        float ret = 1f;

        if (ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value >= 0)
        {
            ret = (float)TableManager.Instance.MagicBookTable
                .dataArray[ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value].Goldabilratio;
        }
        else
        {
            ret = 1f;
        }

        return ret;
    }

    public static float GetNorigaeSoulGradeValue()
    {
        float ret = 1f;

        int norigaeSoulGrade = PlayerStats.GetNorigaeSoulGrade();

        if (norigaeSoulGrade != -1 && norigaeSoulGrade < TableManager.Instance.norigaeJewel.dataArray.Length)
        {
            float abilValue = TableManager.Instance.norigaeJewel.dataArray[norigaeSoulGrade].Abilvalue0;

            ret = abilValue;
        }

        return ret * GetNorigaeSoulTransPlusValue();
    }

    public static float GetSpecialAbilRatio()
    {
        if (ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value >= 0)
        {
            return (float)TableManager.Instance.WeaponTable
                .dataArray[ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value].Specialadd;
        }
        else
        {
            return 1f;
        }
    }

    //1 디폴트
    public static float GetSpecialAbilRing()
    {
        if (ServerData.equipmentTable.TableDatas[EquipmentTable.SoulRing].Value >= 0)
        {
            return (float)TableManager.Instance.NewGachaTable
                .dataArray[ServerData.equipmentTable.TableDatas[EquipmentTable.SoulRing].Value].Specialadd;
        }
        else
        {
            return 1f;
        }
    }

    //0디폴트
    public static float GetRelicTestAbilRing()
    {
        float ret = 0f;
        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.relicTestScore].Value *
                    GameBalance.BossScoreConvertToOrigin;
        var tabledata = TableManager.Instance.RelicTest.dataArray;
        for (int i = 0; i < tabledata.Length; i++)
        {
            if (tabledata[i].Score <= score)
            {
                ret = tabledata[i].Abilvalue;
            }
        }

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.relicTestGraduate).Value > 0)
        {
            ret *= GameBalance.relicTestGraduateValue;
        }

        return ret;
    }

    public static float GetMaskAttackAddPerDam()
    {
        int equipId = ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMask].Value;
        if (equipId == -1) return 0f;

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.foxMaskGraduate).Value > 0)
        {
            return (float)TableManager.Instance.FoxMask.dataArray[equipId].Abilvalue * (1 + GetFoxMaskAbilPlusValue())*GameBalance.foxMaskGraduateValue;
        }
        return (float)TableManager.Instance.FoxMask.dataArray[equipId].Abilvalue * (1 + GetFoxMaskAbilPlusValue());
    }

    public static float GetDokebiHornCritical5Add()
    {
        int equipId = ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHorn].Value;
        if (equipId == -1) return 0f;

        return (float)TableManager.Instance.DokebiHorn.dataArray[equipId].Abilvalue;
    }

    public static float GetCaveBeltAttackAdd()
    {
        int equipId = ServerData.equipmentTable.TableDatas[EquipmentTable.CaveBelt].Value;
        if (equipId == -1) return 0f;

        return (float)TableManager.Instance.CaveBelt.dataArray[equipId].Abilvalue * (1 + GetTwoCaveBeltAbilPlusValue());
    }
    public static float GetCaveBeltEnhanceAttackAdd()
    {
        int equipId = ServerData.equipmentTable.TableDatas[EquipmentTable.CaveBelt].Value;
        if (equipId == -1) return 0f;

        return (float)TableManager.Instance.CaveBelt.dataArray[equipId].Abilvalue2;
    }


    public static void ResetAbilDic()
    {
        PlayerStats.ResetMagicBookHas();
        PlayerStats.ResetSinsuHas();
        PetServerTable.ResetPetHas();
        PlayerStats.ResetTitleHas();
        PlayerStats.ResetStageRelicHas();
        PlayerStats.ResetGuimoonRelicHas();
        PlayerStats.ResetBlackFoxHas();
        PlayerStats.ResetHaetal();
        PlayerStats.ResetMeditationDictionary();
        PlayerStats.ResetSuperCritical11CalculatedValue();
        PlayerStats.ResetMunhaHyulDictionary();
    }

    public static int GetSusanoGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.susanoTable.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.susanoScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetBlackFoxGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.BlackFoxTable.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.blackFoxScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetAddVisionSkillUseCount()
    {
        int ret = 0;

        ret += (int)GetAwakeAbilityValue(AbilAwakeType.Vision, StatusType.AddVisionSkillUseCount);
        
        ret += GetDragonKingWeaponHasAbility();
        
        return ret;
    }
    public static int GetAddMunhaSkillUseCount()
    {
        int ret = 0;
        
        ret += GetMunhaAddAbility();
        
        return ret;
    }
    public static float GetEnhanceVisionSkill()
    {
        float ret = 0f;

        ret += GetVisionTowerAbil();

        ret += GameBalance.VisionTreasurePerDamage * ServerData.goodsTable.GetTableData(GoodsTable.VisionTreasure).Value;

        ret += GetAwakeAbilityValue(AbilAwakeType.Vision, StatusType.EnhanceVisionSkill);

        ret += GetSasinsuPowerAbility(StatusType.EnhanceVisionSkill, 3);
        
        return ret;
    }
    public static float GetReduceDosulSkillCoolTime()
    {
        float ret = 0f;
        
        ret += GetAwakeAbilityValue(AbilAwakeType.Dosul, StatusType.ReduceDosulSkillCoolTime);
        
        return ret;
    }
    
    public static int GetAddSealSwordSkillHitCount()
    {
        var ret = 0;
        
        ret += (int)GetAwakeAbilityValue(AbilAwakeType.SealSword, StatusType.AddSealSwordSkillHitCount);
        
        return ret;
    }
    //요도 시전요구최대치 감소
    public static float GetReduceSealSwordSkillRequireCount()
    {
        float ret = 0f;

        ret += SealSwordEvolutionAbility(StatusType.ReduceSealSwordSkillRequireCount);
        
        return ret;
    }
    

    public static float GetVisionTowerAbil()
    {
        var grade = PlayerStats.GetVisionTowerGrade();
        if (grade > -1)
        {
            var data = TableManager.Instance.visionTowerTable.dataArray[grade];

            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateVisionTower).Value > 0)
            {
                return data.Abilvalue0 * GameBalance.VisionTowerGraduatePlusValue;
            }
            return data.Abilvalue0;
        } 
        else
        {
            return 0f;
        }
    }
    public static int GetVisionTowerGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.visionTowerTable.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.visionTowerScore].Value;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Id)
            {
                grade = i;
            }
        }

        return grade - 1;
    }

    public static int GetTaeguekGrade()
    {
        int ret = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.taeguekTower].Value - 1;
        return ret;
    }
    //문하도장(특훈아님)
    public static int GetMunhaGrade()
    {
        int ret = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.munhaTower].Value - 1;
        return ret;
    }

    public static int GetSealSwordAwakeGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.SealSwordAwakeTable.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.SealSwordAwakeScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }
    public static int GetDosulAwakeGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.DosulAwakeTable.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.DosulAwakeScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetDosulGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.dosulTowerTable.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.dosulScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Rewrardcut)
            {
                grade = i;
            }
        }


        return grade;
    }

    public static int GetMeditationTowerGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.MeditationTower.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.meditationScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Rewrardcut)
            {
                grade = i;
            }
        }


        return grade;
    }
    public static int GetTransJewelTowerGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.TransJewelTower.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.transJewelScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        var equipmentCount = PlayerStats.GetTransEquipmentCount();
        
        for (int i = 0; i < tableData[0].Rewardcut.Length; i++)
        {
            //초월수가 부족하면 break
            if (tableData[0].Unlocktranscount[i] > equipmentCount) break;
            if (score >= tableData[0].Rewardcut[i])
            {
                grade = i;
            }
        }


        return grade;
    }
    //특훈
    public static int GetMunhaTowerGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.StudentAwakeTable.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.munhaScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }
        
        return grade;
    }
    //특훈
    public static int GetMunhaHyulTowerGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.StudentSpotTower.dataArray;
        var scoreValue = ServerData.bossServerTable.TableDatas["b296"].score.Value;
        var score = -1d;
        if (string.IsNullOrEmpty(scoreValue) == false)
        {
            score = double.Parse(scoreValue);
        }
        
        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Rewrardcut)
            {
                grade = i;
            }
        }
        
        return grade;
    }

    public static int GetByeolhoTowerGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.ByeolhoTower.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.byeolhoTowerScore].Value *
                    GameBalance.BossScoreConvertToOrigin;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Rewrardcut)
            {
                grade = i;
            }
        }


        return grade;
    }


    public static float GetCurrentDosulAddValue()
    {
        float ret = 0;

        int currentDosulLevel = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulLevel].Value;

        if (currentDosulLevel != -1 && currentDosulLevel < TableManager.Instance.dosulTable.dataArray.Length)
        {
            var dosulTableData = TableManager.Instance.dosulTable.dataArray[currentDosulLevel];

            float dosulValue = 0f;

            return dosulTableData.Abil_Value;
        }

        return ret;
    }

    public static int GetNorigaeSoulGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.norigaeJewel.dataArray;

        var score = ServerData.userInfoTable.TableDatas[UserInfoTable.norigaeScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetGradeTestGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.gradeTestTable.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.gradeScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }
    public static int GetWeeklyBossGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.WeeklyBoss.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.weeklyBossScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Hp)
            {
                grade = i;
            }
        }

        return grade;
    }
    public static int GetDanjeonGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.DanjeonTable.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.danjeonScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }
    public static int GetClosedTrainingGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.ClosedTrainingTable.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.closedScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }
    //type별 그레이드의 테이블 내 실제 id
    private static int GetAbilAwakeTableIdx(AbilAwakeType type, int idx)
    {
        var tableData = TableManager.Instance.AbilAwakeTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].ABILAWAKETYPE != type) continue;

            if (tableData[i].Grade == idx)
            {
                return i;
            }
        }

        return -1;
    }
    //type별 진행가능한 grade 단계
    public static int CanAbilAwakeGrade(AbilAwakeType type)
    {
        int grade0 = GetClosedTrainingGrade();
        int grade1 = -1;
        int abilGrade = -1;

        switch (type)
        {
            case AbilAwakeType.Vision:
                grade1 = GetVisionTowerGrade();
                break;
            case AbilAwakeType.SealSword:
                grade1 = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx9).Value;;
                break;
            case AbilAwakeType.Dosul:
                grade1 = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dosulLevel).Value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        var tableData = TableManager.Instance.AbilAwakeTable.dataArray;


        for (int i = 0; i < tableData.Length; i++)
        {
            if(tableData[i].ABILAWAKETYPE!=type) continue;

            if (tableData[i].Unlockclosedtraining <= grade0 && tableData[i].Unlockcontents <= grade1)
            {
                abilGrade = tableData[i].Grade;
            }
            else
            {
                break;
            }
        }

        return abilGrade;
    }
    
    public static float GetAwakeAbilityValue(AbilAwakeType type,StatusType statusType)
    {
        int currentGrade = -1;

        switch (type)
        {
            case AbilAwakeType.Vision:
                currentGrade= GetAbilAwakeTableIdx(type,(int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeVisionSkill).Value);
                break;
            case AbilAwakeType.SealSword:
                currentGrade= GetAbilAwakeTableIdx(type,(int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeSealSword).Value);
                break;
            case AbilAwakeType.Dosul:
                currentGrade= GetAbilAwakeTableIdx(type,(int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.awakeDosulSkill).Value);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }


        if (currentGrade == -1) return 0f;

        var tableData = TableManager.Instance.AbilAwakeTable.dataArray[currentGrade];

        if (tableData.Abiltype.Length != tableData.Abilvalue.Length) return 0f;

        float ret = 0f;

        for (int i = 0; i < tableData.Abiltype.Length; i++)
        {
            if (tableData.Abiltype[i] == (int)statusType)
            {
                ret += tableData.Abilvalue[i];
            }
        }

        return ret;
    }
    public static int GetDragonKingWeaponHasAbility()
    {
        return ServerData.weaponTable.GetWeaponData("weapon155").hasItem.Value > 0 ? 1 : 0;
    }
    public static int GetMunhaAddAbility()
    {
        return ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaTower).Value >= GameBalance.munhaSkillAddGrade ? 1 : 0;
    }

    public static float GetClosedTrainingValue()
    {
        int grade = -1;

        grade = GetClosedTrainingGrade();

        if (grade < 0)
        {
            return 0f;
        }

        var tableData = TableManager.Instance.ClosedTrainingTable.dataArray[grade];

        return tableData.Abilvalue;
    }
    public static int GetHyunsangTowerGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.HyunSangTowerTable.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.hyunsangTowerScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetRelicTestGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.RelicTest.dataArray;

        var score = ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.relicTestScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetRingGrade()
    {
        return ServerData.equipmentTable.TableDatas[EquipmentTable.SoulRing].Value;
    }

    public static int GetYumGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.yumTable.dataArray;

        var score = ServerData.userInfoTable.TableDatas[UserInfoTable.yumScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetGumgiGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.gumGiTable.dataArray;

        var value = ServerData.goodsTable.TableDatas[GoodsTable.SwordPartial].Value;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (value >= tableData[i].Require)
            {
                grade = i;
            }
        }

        if (grade >= 200)
        {
            var over200 = Mathf.Max(0, ((int)ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value - TableManager.Instance.gumGiTable.dataArray[200].Require) / 50000);
            grade += over200;
        }
        
        return grade;
    }

    public static int GetOkGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.okTable.dataArray;

        var score = ServerData.userInfoTable.TableDatas[UserInfoTable.okScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetDoGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.doTable.dataArray;

        var score = ServerData.userInfoTable.TableDatas[UserInfoTable.doScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetSumiGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.sumiTable.dataArray;

        var score = ServerData.userInfoTable.TableDatas[UserInfoTable.sumiScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetThiefKingGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.ThiefTable.dataArray;

        var score = ServerData.userInfoTable.TableDatas[UserInfoTable.thiefScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetDarkKingGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.DarkTable.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.darkScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }
    public static int GetSinsunKingGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.SinsunTable.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.sinsunScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetMonkeyGodGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.TestMonkey.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.monkeyGodScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetSwordGodGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.TestSword.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.swordGodScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetHellGodGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.TestHell.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.hellGodScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetChunGodGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.TestChun.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.chunGodScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetDoGodGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.TestDo.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.doGodScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetSumiGodGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.TestSumi.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.sumiGodScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }

    public static int GetThiefGodGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.TestThief.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.thiefGodScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }
    public static int GetDarkGodGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.TestDark.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.darkGodScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }
    public static int GetSinsunGodGrade()
    {
        int grade = -1;

        var tableData = TableManager.Instance.TestSin.dataArray;

        var score = ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.sinsunGodScore].Value *
                    GameBalance.BossScoreConvertToOrigin;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (score >= tableData[i].Score)
            {
                grade = i;
            }
        }

        return grade;
    }
    public static int GetSpecialSuhoRelicGrade()
    {
        var relicTable = TableManager.Instance.RelicSpecial.dataArray;
        
        var localData = TableManager.Instance.suhoPetTable.dataArray;
        var serverData = ServerData.suhoAnimalServerTable.TableDatas;

        int suhoIdx = -1;

        int grade = -1;
        
        for (int i = 0; i < localData.Length; i++)
        {
            if (localData[i].SUHOPETTYPE != SuhoPetType.Basic) continue;
            if (serverData[localData[i].Stringid].hasItem.Value < 1) break;
            else
            {
                suhoIdx = i;
            }
        }

        for (int i = 0; i < relicTable.Length; i++)
        {
            if (relicTable[i].Suhorequire < 0) break;
            if (suhoIdx >= relicTable[i].Suhorequire)
            {
                grade = i;
            }
            else
            {
                break;
            }
        }
        

        return grade;
    }
    public static int GetFoxFireGrade()
    {
        var value = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxFireIdx).Value;

        if (value < 0)
        {
            return 0;
        }
        else
        {
            var tableData = TableManager.Instance.FoxFire.dataArray;

            return tableData[value].Level;
        }
    }
    
    public static int GetSpecialFoxRelicGrade()
    {
        var relicTable = TableManager.Instance.RelicSpecial.dataArray;
        
        int foxGrade = GetFoxFireGrade();

        int grade = -1;

        for (int i = 0; i < relicTable.Length; i++)
        {
            if (relicTable[i].Foxrequire < 0) break;

            if (foxGrade >= relicTable[i].Foxrequire)
            {
                grade = i;
            }
            else
            {
                break;
            }
        }
        

        return grade;
    }
    public static int GetSpecialDosulRelicGrade()
    {
        var relicTable = TableManager.Instance.RelicSpecial.dataArray;
        
        int dosulGrade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dosulLevel).Value;

        int grade = -1;

        for (int i = 0; i < relicTable.Length; i++)
        {
            if (relicTable[i].Dosulrequire < 0) break;

            
            if (dosulGrade >= relicTable[i].Dosulrequire)
            {
                grade = i;
            }
            else
            {
                break;
            }
        }
        

        return grade;
    }
    public static int GetSpecialMeditationRelicGrade()
    {
        var relicTable = TableManager.Instance.RelicSpecial.dataArray;
        
        int meditationGrade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value;

        int grade = -1;

        for (int i = 0; i < relicTable.Length; i++)
        {
            if (relicTable[i].Meditationrequire < 0) break;

            if (meditationGrade >= relicTable[i].Meditationrequire)
            {
                grade = i;
            }
            else
            {
                break;
            }
        }
        

        return grade;
    }
    public static float GetSpecialSuhoRelicAbil(StatusType type)
    {
        var grade = GetSpecialSuhoRelicGrade();

        if (grade == -1)
        {
            return 0f;
        }
        var tableData =  TableManager.Instance.RelicSpecial.dataArray;

        return (StatusType)tableData[grade].Suhoabiltype != type ? 0f : tableData[grade].Suhoabilvalue;
    }

    public static float GetSpecialFoxRelicAbil(StatusType type)
    {
        var grade = GetSpecialFoxRelicGrade();
        if (grade == -1)
        {
            return 0f;
        }
        var tableData =  TableManager.Instance.RelicSpecial.dataArray;

        return (StatusType)tableData[grade].Foxabiltype != type ? 0f : tableData[grade].Foxabilvalue;
    }
    public static float GetSpecialDosulRelicAbil(StatusType type)
    {
        var grade = GetSpecialDosulRelicGrade();
        if (grade == -1)
        {
            return 0f;
        }
        var tableData =  TableManager.Instance.RelicSpecial.dataArray;

        return (StatusType)tableData[grade].Dosulabiltype != type ? 0f : tableData[grade].Dosulabilvalue;
    }
    public static float GetSpecialMeditationRelicAbil(StatusType type)
    {
        var grade = GetSpecialMeditationRelicGrade();
        if (grade == -1)
        {
            return 0f;
        }
        var tableData =  TableManager.Instance.RelicSpecial.dataArray;

        return (StatusType)tableData[grade].Meditationabiltype != type ? 0f : tableData[grade].Meditationabilvalue;
    }
    public static float GetEnhanceRelicAbil(StatusType type)
    {
        int grade = -1;
        
        grade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.enhanceRelicIdx).Value;

        if (grade < 0) return 0f;
        
        var tableData =  TableManager.Instance.RelicEnhance.dataArray;

        return (StatusType)tableData[grade].Abiltype != type ? 0f : tableData[grade].Abilvalue;
    }
    public static float GetEnhanceTaegeukCriticalAbil(StatusType type)
    {
        int grade = -1;
        
        grade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.taegeukSimbeopIdx).Value;

        if (grade < 0) return 0f;
        
        var tableData =  TableManager.Instance.TaegeukSimbeop.dataArray;

        return (StatusType)tableData[grade].Abiltype != type ? 0f : tableData[grade].Abilvalue;
    }
    public static float GetSusanoAbil(StatusType type)
    {
        int grade = GetSusanoGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.susanoTable.dataArray[grade];

        if (type == StatusType.CriticalDam)
        {
            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.susanoGraduate).Value > 0)
            {
                return (tableData.Abilvalue0 * (1 + GetSusanoUpgradeAbilPlusValue())*GameBalance.susanoGraduateValue);
            }

            return tableData.Abilvalue0 * (1 + GetSusanoUpgradeAbilPlusValue());
        }
        else if (type == StatusType.PenetrateDefense)
        {
            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.susanoGraduate).Value > 0)
            {
                return tableData.Abilvalue1 * GameBalance.susanoGraduateValue;
            }
            return tableData.Abilvalue1;
        }

        return 0f;
    }

    public static float GetYumAbil(StatusType type)
    {
        int grade = GetYumGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.yumTable.dataArray[grade];
        //지옥베기
        if (type == StatusType.SuperCritical3DamPer)
        {
            //각성시 추가
            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.KingTrialGraduateIdx).Value >= GameBalance.yumKingGraduate)
            {
                return tableData.Abilvalue0 * GameBalance.yumGraduateValue;
            }

            return tableData.Abilvalue0;
        }
        //else if (type == StatusType.PenetrateDefense)
        //{

        //    return tableData.Abilvalue1;

        //}

        return 0f;
    }

    public static float GetOkAbil(StatusType type)
    {
        int grade = GetOkGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.okTable.dataArray[grade];

        if (type == StatusType.SuperCritical4DamPer)
        {
            //각성시 추가
            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.KingTrialGraduateIdx).Value >= GameBalance.okKingGraduate)
            {
                return tableData.Abilvalue0 * GameBalance.okGraduateValue;
            }

            return tableData.Abilvalue0;
        }
        //else if (type == StatusType.PenetrateDefense)
        //{

        //    return tableData.Abilvalue1;

        //}

        return 0f;
    }

    public static float GetDoAbil(StatusType type)
    {
        int grade = GetDoGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.doTable.dataArray[grade];

        if (type == StatusType.SuperCritical5DamPer)
        {
            //각성시 추가
            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.KingTrialGraduateIdx).Value >= GameBalance.doKingGraduate)
            {
                return tableData.Abilvalue0 * GameBalance.doGraduateValue;
            }

            return tableData.Abilvalue0;
        }
        //else if (type == StatusType.PenetrateDefense)
        //{

        //    return tableData.Abilvalue1;

        //}

        return 0f;
    }

    public static float GetSuAbil(StatusType type)
    {
        int grade = GetSumiGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.sumiTable.dataArray[grade];

        if (type == StatusType.SuperCritical7DamPer)
        {
            //각성시 추가
            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.KingTrialGraduateIdx).Value >= GameBalance.sumiKingGraduate)
            {
                return tableData.Abilvalue0 * GameBalance.sumiGraduateValue;
            }

            return tableData.Abilvalue0;
        }

        return 0f;
    }

    public static float GetThiefAbil(StatusType type)
    {
        int grade = GetThiefKingGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.ThiefTable.dataArray[grade];

        if (type == StatusType.SuperCritical10DamPer)
        {
            //각성시 추가
            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.KingTrialGraduateIdx).Value >= GameBalance.thiefKingGraduate)
            {
                return tableData.Abilvalue0 * GameBalance.thiefGraduateValue;
            }

            return tableData.Abilvalue0;
        }

        return 0f;
    }

    public static float GetDarkAbil(StatusType type)
    {
        int grade = GetDarkKingGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.DarkTable.dataArray[grade];

        if (type == StatusType.SuperCritical12DamPer)
        {
            //각성시 추가
            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.KingTrialGraduateIdx).Value >= GameBalance.darkKingGraduate)
            {
                return tableData.Abilvalue0 * GameBalance.darkGraduateValue;
            }

            return tableData.Abilvalue0;
        }

        return 0f;
    }
    public static float GetSinsunAbil(StatusType type)
    {
        int grade = GetSinsunKingGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.SinsunTable.dataArray[grade];

        if (type == StatusType.SuperCritical15DamPer)
        {
            //각성시 추가
            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.KingTrialGraduateIdx).Value >= GameBalance.sinsunKingGraduate)
            {
                return tableData.Abilvalue0 * GameBalance.sinsunGraduateValue;
            }

            return tableData.Abilvalue0;
        }

        return 0f;
    }

    public static float GetSwordGodAbil0()
    {
        int grade = GetSwordGodGrade();

        if (grade == -1) return 1f;

        var tableData = TableManager.Instance.TestSword.dataArray[grade];

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).Value >= GameBalance.swordGodGraduate)
        {
            return tableData.Abilvalue0 * GameBalance.swordGodGraduateValue;
        }

        return tableData.Abilvalue0;
    }

    public static float GetMonkeyGodAbil0()
    {
        int grade = GetMonkeyGodGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.TestMonkey.dataArray[grade];

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).Value >= GameBalance.monkeyGodGraduate)
        {
            return tableData.Abilvalue0 * GameBalance.monkeyGodGraduateValue;
        }

        return tableData.Abilvalue0;
    }

    public static float GetMonkeyGodAbil1()
    {
        int grade = GetMonkeyGodGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.TestMonkey.dataArray[grade];

        return tableData.Abilvalue1;
    }

    public static float GetHellGodAbil0()
    {
        int grade = GetHellGodGrade();

        if (grade == -1) return 1f;

        var tableData = TableManager.Instance.TestHell.dataArray[grade];
        
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).Value >= GameBalance.hellGodGraduate)
        {
            return tableData.Abilvalue0 * GameBalance.hellGodGraduateValue;
        }
        
        return tableData.Abilvalue0;
    }

    public static float GetHellGodAbil1()
    {
        int grade = GetHellGodGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.TestHell.dataArray[grade];

        return tableData.Abilvalue1;
    }

    public static float GetChunGodAbil0()
    {
        int grade = GetChunGodGrade();

        if (grade == -1) return 1f;

        var tableData = TableManager.Instance.TestChun.dataArray[grade];
        
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).Value >= GameBalance.chunGodGraduate)
        {
            return tableData.Abilvalue0 * GameBalance.chunGodGraduateValue;
        }
        return tableData.Abilvalue0;
    }

    public static float GetChunGodAbil1()
    {
        int grade = GetChunGodGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.TestChun.dataArray[grade];

        return tableData.Abilvalue1;
    }

    public static float GetDoGodAbil0()
    {
        int grade = GetDoGodGrade();

        if (grade == -1) return 1f;

        var tableData = TableManager.Instance.TestDo.dataArray[grade];
        
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).Value >= GameBalance.doGodGraduate)
        {
            return tableData.Abilvalue0 * GameBalance.doGodGraduateValue;
        }
        
        return tableData.Abilvalue0;
    }

    public static float GetDoGodAbil1()
    {
        int grade = GetDoGodGrade();

        if (grade == -1) return 0f;

        var tableData = TableManager.Instance.TestDo.dataArray[grade];

        return tableData.Abilvalue1;
    }

    public static float GetSumiGodAbil0()
    {
        int grade = GetSumiGodGrade();

        if (grade == -1) return 1f;

        var tableData = TableManager.Instance.TestSumi.dataArray[grade];
        
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).Value >= GameBalance.sumiGodGraduate)
        {
            return tableData.Abilvalue0 * GameBalance.sumiGodGraduateValue;
        }
        return tableData.Abilvalue0;
    }
    public static float GetThiefGodAbil0()
    {
        int grade = GetThiefGodGrade();

        if (grade == -1) return 1f;

        var tableData = TableManager.Instance.TestThief.dataArray[grade];

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).Value >= GameBalance.thiefGodGraduate)
        {
            return tableData.Abilvalue0 * GameBalance.thiefGodGraduateValue;
        }
        return tableData.Abilvalue0;
    }

    public static float GetDarkGodAbil0()
    {
        int grade = GetDarkGodGrade();

        if (grade == -1) return 1f;

        var tableData = TableManager.Instance.TestDark.dataArray[grade];
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).Value >= GameBalance.darkGodGraduate)
        {
            return tableData.Abilvalue0 * GameBalance.darkGodGraduateValue;
        }
        return tableData.Abilvalue0;
    }
    public static float GetSinsunGodAbil0()
    {
        int grade = GetSinsunGodGrade();

        if (grade == -1) return 1f;

        var tableData = TableManager.Instance.TestSin.dataArray[grade];
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).Value >= GameBalance.sinsunGodGraduate)
        {
            return tableData.Abilvalue0 * GameBalance.sinsunGodGraduateValue;
        }
        return tableData.Abilvalue0;
    }


    public static float yeoRaeMarbleValue = 0.1f;

    public static float GetSonAbilPlusValue()
    {
        return yeoRaeMarbleValue * ServerData.goodsTable.GetTableData(GoodsTable.Ym).Value;
    }

    public static float SonTransAddValue = 10f;

    public static float GetSonTransPlusValue()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).Value > 0)
        {
            return SonTransAddValue;
        }
        else
        {
            return 0f;
        }
    }

    public static float HelTransAddValue = 2f;

    public static float GetHelTransPlusValue()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).Value > 0)
        {
            return HelTransAddValue;
        }
        else
        {
            return 1f;
        }
    }

    public static float ChunTransAddValue = 2f;

    private static float GetChunTransPlusValue()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).Value > 0)
        {
            return ChunTransAddValue;
        }

        return 1f;
    }

    private static float GetDokebiTransPlusValue()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).Value > 0)
        {
            return GameBalance.dokebiGraduatePlusValue;
        }

        return 1f;
    }
    private static float GetSumiTransPlusValue()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSumiFire).Value > 0)
        {
            return GameBalance.sumiGraduatePlusValue;
        }

        return 1f;
    }

    public static float GetGumSoulTransPlusValue()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateGumSoul).Value > 0)
        {
            return GameBalance.GumSoulGraduatePlusValue;
        }
        else
        {
            return 1f;
        }
    }

    public static float GetNorigaeSoulTransPlusValue()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateNorigaeSoul).Value > 0)
        {
            return GameBalance.NorigaeSoulGraduatePlusValue;
        }
        else
        {
            return 1f;
        }
    }

    public static float GetEvilSeedTransPlusValue()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateEvilSeed).Value > 0)
        {
            return GameBalance.EvilSeedGraduatePlusValue;
        }
        else
        {
            return 1f;
        }
    }

    public static float GetGhostTreeTransPlusValue()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateGhostTree).Value > 0)
        {
            return GameBalance.GhostTreeGraduatePlusValue;
        }
        else
        {
            return 1f;
        }
    }

    public static float foxMaskPartialValue = 0.02f;

    public static float GetFoxMaskAbilPlusValue()
    {
        return (foxMaskPartialValue * ServerData.goodsTable.GetTableData(GoodsTable.FoxMaskPartial).Value) * GetGhostTreeTransPlusValue();
    }

    public static float susanoUpgradelValue = 0.025f;

    public static float GetSusanoUpgradeAbilPlusValue()
    {
        return (susanoUpgradelValue * ServerData.goodsTable.GetTableData(GoodsTable.SusanoTreasure).Value) *
               GetEvilSeedTransPlusValue();
    }

    public static float dokebiUpgradeValue = 0.0025f;

    public static float GetDokebiFireEnhanceAbilPlusValue()
    {
        return dokebiUpgradeValue * ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireEnhance).Value;
    } //

    public static string caveBossKey = "b91";

    public static float GetTwoCaveBeltAbilPlusValue()
    {
        if (string.IsNullOrEmpty(ServerData.bossServerTable.TableDatas[caveBossKey].score.Value))
        {
            return 0f;
        }

        int.TryParse(ServerData.bossServerTable.TableDatas[caveBossKey].score.Value, out var score);
        return TableManager.Instance.twoCave.dataArray[score - 1].Beltaddvalue;
    }

    public static float sahyungUpgradeValue = GameBalance.sahyungUpgradeValue;
    public static float sinSuUpgradeValue = GameBalance.sinsuUpgradeValue;

    public static float GetSahyungTreasureAbilPlusValue()
    {
        return sahyungUpgradeValue * ServerData.goodsTable.GetTableData(GoodsTable.SahyungTreasure).Value;
    }

    public static float GetSinsuTreasureAbilPlusValue()
    {
        return sinSuUpgradeValue * ServerData.goodsTable.GetTableData(GoodsTable.SinsuMarble).Value;
    }


    //
    //public static float GetTwoCaveExpAbilPlusValue()
    //{
    //    string bossKey = "b91";
    //    if (string.IsNullOrEmpty(ServerData.bossServerTable.TableDatas[bossKey].score.Value))
    //    {
    //        return 0f;
    //    }
    //    int.TryParse(ServerData.bossServerTable.TableDatas[bossKey].score.Value, out var score);
    //    return TableManager.Instance.twoCave.dataArray[score-1].Expaddvalue;
    //}


    public static int GetCurrentFoxCupIdx()
    {
        var tableData = TableManager.Instance.foxCup.dataArray;
        var currentPetEquipGrade = ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value;
        int idx = -1;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (currentPetEquipGrade >= tableData[i].Require)
            {
                idx = i;
            }
        }

        return idx;
    }

    public static int GetCurrentWolfRingIdx()
    {
        var tableData = TableManager.Instance.BlackWolfRing.dataArray;
        var currentPetEquipGrade = ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value;
        int idx = -1;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (currentPetEquipGrade >= tableData[i].Require)
            {
                idx = i;
            }
        }

        return idx;
    }
    public static int GetCurrentRelicUpgradeIdx()
    {
        var tableData = TableManager.Instance.RelicUpgrade.dataArray;
        var currentPetEquipGrade = ServerData.statusTable.GetTableData(StatusTable.RingEnhance_Level).Value;
        int idx = -1;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (currentPetEquipGrade >= tableData[i].Require)
            {
                idx = i;
            }
        }

        return idx;
    }
    public static int GetCurrentTransUpgradeIdx()
    {
        var tableData = TableManager.Instance.TransUpgrade.dataArray;
        var grade = ServerData.statusTable.GetTableData(StatusTable.Trans_Level).Value;
        int idx = -1;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (grade >= tableData[i].Require)
            {
                idx = i;
            }
        }

        return idx;
    }
    public static int GetCurrentSuhoUpgradeIdx()
    {
        var tableData = TableManager.Instance.SuhoUpgrade.dataArray;
        var currentPetEquipGrade = ServerData.statusTable.GetTableData(StatusTable.SuhoEnhance_Level).Value;
        int idx = -1;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (currentPetEquipGrade >= tableData[i].Require)
            {
                idx = i;
            }
        }

        return idx;
    }
    public static int GetCurrentDragonBraceletIdx()
    {
        var tableData = TableManager.Instance.DragonBracelet.dataArray;
        var currentPetEquipGrade = ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value;
        int idx = -1;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (currentPetEquipGrade >= tableData[i].Require)
            {
                idx = i;
            }
        }

        return idx;
    }

    public static int GetCurrentDragonIdx()
    {
        var tableData = TableManager.Instance.dragonBall.dataArray;
        var currentPetEquipGrade = ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value;
        int idx = -1;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (currentPetEquipGrade >= tableData[i].Require)
            {
                idx = i;
            }
        }

        return idx;
    }

    public static int GetCurrentGumgiIdx()
    {
        int idx = ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponEnhance].Value;

        return idx;
    }

    public static float GetDragonBallAbil0Value()
    {
        int idx = GetCurrentDragonIdx();

        if (idx == -1) return 0f;

        return TableManager.Instance.dragonBall.dataArray[idx].Abilvalue0;
    }

    public static float GetDragonBallAbil1Value()
    {
        int idx = GetCurrentDragonIdx();

        if (idx == -1) return 0f;

        return TableManager.Instance.dragonBall.dataArray[idx].Abilvalue1;
    }

    //
    public static float GetFoxCupAbilValue(int idx, int abilIdx)
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getFoxCup).Value == 0) return 0f;
        if (idx == -1) return 0f;

        switch (abilIdx)
        {
            case 0:
            {
                return TableManager.Instance.foxCup.dataArray[idx].Abilvalue0;
            }
                break;
            case 1:
            {
                return TableManager.Instance.foxCup.dataArray[idx].Abilvalue1;
            }
                break;
            case 2:
            {
                return TableManager.Instance.foxCup.dataArray[idx].Abilvalue2;
            }
                break;
            case 3:
            {
                return TableManager.Instance.foxCup.dataArray[idx].Abilvalue3;
            }
                break;
            case 4:
            {
                return TableManager.Instance.foxCup.dataArray[idx].Abilvalue4;
            }
                break;
            case 5:
            {
                return TableManager.Instance.foxCup.dataArray[idx].Abilvalue5;
            }
                break;
        }

        return 0f;
    }

    public static float GetWolfRingAbilValue(int idx, int abilIdx)
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getWolfRing).Value == 0) return 0f;
        if (idx == -1) return 0f;

        switch (abilIdx)
        {
            case 0:
            {
                return TableManager.Instance.BlackWolfRing.dataArray[idx].Abilvalue0;
            }
                break;
            case 1:
            {
                return TableManager.Instance.BlackWolfRing.dataArray[idx].Abilvalue1;
            }
                break;
            case 2:
            {
                return TableManager.Instance.BlackWolfRing.dataArray[idx].Abilvalue2;
            }
                break;
            case 3:
            {
                return TableManager.Instance.BlackWolfRing.dataArray[idx].Abilvalue3;
            }
                break;
            case 4:
            {
                return TableManager.Instance.BlackWolfRing.dataArray[idx].Abilvalue4;
            }
                break;
            case 5:
            {
                return TableManager.Instance.BlackWolfRing.dataArray[idx].Abilvalue5;
            }
                break;
        }

        return 0f;
    }
    public static float GetRelicUpgradeAbilValue(int idx)
    {
        //if (ServerData.userInfoTable.GetTableData(UserInfoTable.getRelicUpgrade).Value == 0) return 0f;
        if (idx == -1) return 0f;

        return TableManager.Instance.RelicUpgrade.dataArray[idx].Abilvalue;
    }
    public static float GetTransUpgradeAbilValue(int idx)
    {
        if (idx == -1) return 0f;

        return TableManager.Instance.TransUpgrade.dataArray[idx].Abilvalue;
    }
    public static float GetSuhoUpgradeAbilValue(int idx)
    {
        //if (ServerData.userInfoTable.GetTableData(UserInfoTable.getRelicUpgrade).Value == 0) return 0f;
        if (idx == -1) return 0f;

        var multipleValue = ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.suhoUpgradeGraduateIdx).Value > 0
            ? GameBalance.suhoGraduateValue
            : 1;
 
        
        return TableManager.Instance.SuhoUpgrade.dataArray[idx].Abilvalue * multipleValue;
    }
    public static float GetDragonBraceletAbilValue(int idx, int abilIdx)
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.getDragonBracelet).Value == 0) return 0f;
        if (idx == -1) return 0f;

        switch (abilIdx)
        {
            case 0:
            {
                return TableManager.Instance.DragonBracelet.dataArray[idx].Abilvalue0;
            }
                break;
            case 1:
            {
                return TableManager.Instance.DragonBracelet.dataArray[idx].Abilvalue1;
            }
                break;
            case 2:
            {
                return TableManager.Instance.DragonBracelet.dataArray[idx].Abilvalue2;
            }
                break;
            case 3:
            {
                return TableManager.Instance.DragonBracelet.dataArray[idx].Abilvalue3;
            }
                break;
            case 4:
            {
                return TableManager.Instance.DragonBracelet.dataArray[idx].Abilvalue4;
            }
                break;
            case 5:
            {
                return TableManager.Instance.DragonBracelet.dataArray[idx].Abilvalue5;
            }
                break;
        }

        return 0f;
    }

    //

    public const float HellRelicAbilValue = 0.5f;
    public const int HellRelicAbilDivide = 100;

    public static float GetHellRelicAbilValue()
    {
        int kt =
            (int)(ServerData.userInfoTable.TableDatas[UserInfoTable.hellRelicKillCount].Value / HellRelicAbilDivide);
        return kt * HellRelicAbilValue;
    }

    public const float gumgiSoulAbilValue = 0.005f;
    public const int gumgiSoulDivideNum = 100;

    public static float GetGumgiAbilAddValue()
    {
        int kt = (int)(ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiSoulClear].Value / gumgiSoulDivideNum);

        float addValue = ServerData.goodsTable.GetTableData(GoodsTable.Fw).Value;

        //천상계 마크
        float addValue2 = 0;
        if (PlayerStats.IsChunFlowerGumgiEnhance())
        {
            addValue2 = TableManager.Instance.chunMarkAbil.dataArray[6].Abiladdvalue;
        }

        return (kt * gumgiSoulAbilValue + (addValue * 0.01f) + addValue2) * GetGumSoulTransPlusValue();
    }

    public static bool IsChunQuickMoveAwake()
    {
        var chunFlowerNum = ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value;
        var requireFlower = TableManager.Instance.chunMarkAbil.dataArray[0].Requirespeicalabilflower;
        return chunFlowerNum >= requireFlower;
    }

    public static bool IsChunAttackSpeedAwake()
    {
        var chunFlowerNum = ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value;
        var requireFlower = TableManager.Instance.chunMarkAbil.dataArray[1].Requirespeicalabilflower;
        return chunFlowerNum >= requireFlower;
    }

    public static bool IsChunBossHpDec()
    {
        var chunFlowerNum = ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value;
        var requireFlower = TableManager.Instance.chunMarkAbil.dataArray[2].Requirespeicalabilflower;
        return chunFlowerNum >= requireFlower;
    }

    public static bool IsChunMonsterSpawnAdd()
    {
        var chunFlowerNum = ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value;
        var requireFlower = TableManager.Instance.chunMarkAbil.dataArray[3].Requirespeicalabilflower;
        return chunFlowerNum >= requireFlower;
    }

    public static bool IsChunFlowerDamageEnhance()
    {
        var chunFlowerNum = ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value;
        var requireFlower = TableManager.Instance.chunMarkAbil.dataArray[4].Requirespeicalabilflower;
        return chunFlowerNum >= requireFlower;
    }

    public static bool IsChunFlowerCostumeEnhance()
    {
        var chunFlowerNum = ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value;
        var requireFlower = TableManager.Instance.chunMarkAbil.dataArray[5].Requirespeicalabilflower;
        return chunFlowerNum >= requireFlower;
    }

    public static float IsCostumeCollectionEnhance()
    {
        float ret = 1f;

        var costumeNum = ServerData.costumeServerTable.GetCostumeHasAmount();
        var tableData = TableManager.Instance.costumeCollection.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Require <= costumeNum)
            {
                ret = tableData[i].Plusvalue;
            }
            else
            {
                break;
            }
        }

        return ret;
    }

    public static bool IsChunFlowerGumgiEnhance()
    {
        var chunFlowerNum = ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value;
        var requireFlower = TableManager.Instance.chunMarkAbil.dataArray[6].Requirespeicalabilflower;
        return chunFlowerNum >= requireFlower;
    }

    public static int GetPetHomeHasCount()
    {
        int ret = 0;

        var tableData = TableManager.Instance.PetTable.dataArray;

        for (int i = 8; i < tableData.Length; i++)
        {
            if (ServerData.petTable.TableDatas[tableData[i].Stringid].hasItem.Value == 1)
            {
                ret++;
            }
        }

        return ret;
    }


    public static float GetPetHomeAbilValue(StatusType type)
    {
        float ret = 0f;

        int petHomeHasCount = GetPetHomeHasCount();

        var tableData = TableManager.Instance.petHome.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (petHomeHasCount <= i) break;

            if (tableData[i].Abiltype == (int)type)
            {
                ret += tableData[i].Abilvalue;
            }
        }

        return ret;
    }

    public const float hellPowerStoneAddPer = 0.007f;

    public static float GetHellPowerAddValue()
    {
        var goodsNum = ServerData.goodsTable.GetTableData(GoodsTable.HellPowerUp).Value;

        return goodsNum * hellPowerStoneAddPer;
    }

    public static float GetGradeTestAbilValue(StatusType statusType)
    {
        int currentGrade = GetGradeTestGrade();

        if (currentGrade == -1) return 0f;

        var tableData = TableManager.Instance.gradeTestTable.dataArray[currentGrade];

        if (tableData.Abiltype.Length != tableData.Abilvalue.Length) return 0f;

        float ret = 0f;

        for (int i = 0; i < tableData.Abiltype.Length; i++)
        {
            if (tableData.Abiltype[i] == (int)statusType)
            {
                ret += tableData.Abilvalue[i];
            }
        }
        
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gradeTestGraduate).Value > 0)
        {
            ret *= GameBalance.gradeTestGraduateValue;
        }

        return ret;
    }
    public static float GetDanjeonAbilValue(StatusType statusType)
    {
        int currentGrade = GetDanjeonGrade();

        if (currentGrade == -1) return 0f;

        var tableData = TableManager.Instance.DanjeonTable.dataArray[currentGrade];

        if (tableData.Abiltype.Length != tableData.Abilvalue.Length) return 0f;

        float ret = 0f;

        for (int i = 0; i < tableData.Abiltype.Length; i++)
        {
            if (tableData.Abiltype[i] == (int)statusType)
            {
                ret += tableData.Abilvalue[i];
            }
        }

        return ret;
    }
    public static Dictionary<StatusType, float> GetMeditationDictionary()
    {
        return meditationDictionary;
    }
    public static void ResetMeditationDictionary()
    {
        meditationInitialize = false;
    }
    public static void CreateMeditationDictionary()
    {
        if (meditationInitialize == true) return;
        
        meditationDictionary.Clear();
        var tableData = TableManager.Instance.Meditation.dataArray;

        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value;

        for (int i = 0; i <= idx; i++)
        {
            for (int j=0;j<tableData[i].Abiltype.Length;j++)
            {
                AddOrUpdateValue(meditationDictionary, (StatusType)tableData[i].Abiltype[j], tableData[i].Abilvalue[j]);
            }
        }

        meditationInitialize = true;

    }
    public static float GetSamchunAbility(StatusType type)
    {

        var tableData = TableManager.Instance.Title_Samcheon.dataArray;
        var ret = 0f;
        for (int i = 0; i < tableData.Length; i++)
        {
            if ((StatusType)tableData[i].Abiltype != type) continue;
            if (ServerData.samchunTitleServerTable.TableDatas[tableData[i].Stringid].hasReward.Value < 1) continue;
            ret += tableData[i].Abilvalue;
        }

        return ret;
    }
    public static float SealSwordEvolutionAbility(StatusType type)
    {
        Dictionary<StatusType, float> dictionary = new Dictionary<StatusType, float>();

        var tableData = TableManager.Instance.SealSwordEvolution.dataArray;

        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sealSwordEvolutionIdx).Value;

        for (int i = 0; i <= idx; i++)
        {
            AddOrUpdateValue(dictionary, (StatusType)tableData[i].Abiltype, tableData[i].Abilvalue);
        }
        if(dictionary.TryGetValue(type, out float value))
        {
            return value;
        }
        else
        {
            return 0f;
        }
    }
    public static float GetSealSwordEvolutionAbilityByIdx(StatusType type,int idx)
    {
        Dictionary<StatusType, float> dictionary = new Dictionary<StatusType, float>();

        var tableData = TableManager.Instance.SealSwordEvolution.dataArray;
        
        for (int i = 0; i <= idx; i++)
        {
            AddOrUpdateValue(dictionary, (StatusType)tableData[i].Abiltype, tableData[i].Abilvalue);
        }
        if(dictionary.TryGetValue(type, out float value))
        {
            return value;
        }
        else
        {
            return 0f;
        }
    }
    //해당 단계 하나만 갖고옴.
    public static float ByeolhoHasAbility(StatusType type)
    {
        var ret = 0f;
        
        var tableData = TableManager.Instance.Byeolho.dataArray;

        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoLevelIdx).Value;

        if (idx < 0)
        {
            return ret;
        }

        if (type == (StatusType)tableData[idx].Abil_Type)
        {
            ret = tableData[idx].Abil_Value;
        }
        
        return ret;
    }
    //해당 단계 하나만 갖고옴.
    public static float BattleContestGradeAbility(StatusType type)
    {
        var ret = 0f;
        
        var tableData = TableManager.Instance.BattleContestGrade.dataArray;

        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.battleContestGradeLevel).Value;

        if (idx < 0)
        {
            return ret;
        }

        if (type == (StatusType)tableData[idx].Abil_Type)
        {
            ret = tableData[idx].Abil_Value;
        }
        
        return ret;
    }
    public static void AddOrUpdateValue(Dictionary<StatusType, float> dictionary, StatusType key, float value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }
    public static float GetMeditationAbilValue(StatusType statusType)
    {
        int currentGrade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value;

        if (currentGrade == -1) return 0f;

        if (meditationInitialize == false)
        {
            CreateMeditationDictionary();
        }
        
        
        if(meditationDictionary.TryGetValue(statusType, out float value))
        {
            return value;
        }

        return 0f;
        
    }
    
    public static float GetHyunSangTowerAbilValue(StatusType statusType)
    {
        int currentGrade = GetHyunsangTowerGrade();

        if (currentGrade == -1) return 0f;

        var tableData = TableManager.Instance.HyunSangTowerTable.dataArray[currentGrade];

        float ret = 0f;

        ret += tableData.Abilvalue;

        return ret;
    }
  


    #region Treasure
    
    public static float GetPeachAbilValue()
    {
        float ret = 0f;

        ret += GetGuimoonHasEffect1(StatusType.PeachAbilUpgradePer);

        ret += GetMonkeyGodAbil0();
        
        ret += GetCostumeSpecialAbilityValue(StatusType.PeachAbilUpgradePer);

        ret += GetSpecialTypeAbility(StatusType.PeachAbilUpgradePer);

        return ret;
    }
 public static float GetHellFireHasAddValue()
    {
        float ret = 0f;

        ret += GetGradeTestAbilValue(StatusType.HellHasValueUpgrade);

        ret += GetPassiveSkill2Value(StatusType.HellHasValueUpgrade);

        ret += GetCostumeSpecialAbilityValue(StatusType.HellHasValueUpgrade);
        
        ret += GetSpecialTypeAbility(StatusType.HellHasValueUpgrade);

        ret += GetSamchunAbility(StatusType.HellHasValueUpgrade);

        return ret;
    }

    public static float GetChunFlowerHasAddValue()
    {
        float ret = 0f;

        ret += GetSkillHasValue(StatusType.FlowerHasValueUpgrade);

        ret += GetGradeTestAbilValue(StatusType.FlowerHasValueUpgrade);

        ret += GetPassiveSkill2Value(StatusType.FlowerHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.FlowerHasValueUpgrade);
        
        ret += GetSpecialTypeAbility(StatusType.FlowerHasValueUpgrade);

        ret += GetSamchunAbility(StatusType.FlowerHasValueUpgrade);

        return ret;
    }


    public static float GetDokebiFireHasAddValue()
    {
        float ret = 0f;

        ret += GetSkillHasValue(StatusType.DokebiFireHasValueUpgrade);

        ret += GetGradeTestAbilValue(StatusType.DokebiFireHasValueUpgrade);

        ret += GetPassiveSkill2Value(StatusType.DokebiFireHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.DokebiFireHasValueUpgrade);

        ret += GetSpecialTypeAbility(StatusType.DokebiFireHasValueUpgrade);
        
        ret += GetSamchunAbility(StatusType.DokebiFireHasValueUpgrade);

        
        return ret;
    }
    public static float GetSumiFireHasAddValue()
    {
        float ret = 0f;

        ret += GetSkillHasValue(StatusType.SumiHasValueUpgrade);

        ret += GetGradeTestAbilValue(StatusType.SumiHasValueUpgrade);

        ret += GetPassiveSkill2Value(StatusType.SumiHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.SumiHasValueUpgrade);

        ret += GetSpecialTypeAbility(StatusType.SumiHasValueUpgrade);

        ret += GetSamchunAbility(StatusType.SumiHasValueUpgrade);

        return ret;
    }
    public static float GetTreasureHasAddValue()
    {
        float ret = 0f;

        ret += GetSkillHasValue(StatusType.TreasureHasValueUpgrade);

        ret += GetGradeTestAbilValue(StatusType.TreasureHasValueUpgrade);

        ret += GetPassiveSkill2Value(StatusType.TreasureHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.TreasureHasValueUpgrade);

        ret += GetSpecialTypeAbility(StatusType.TreasureHasValueUpgrade);

        ret += GetSamchunAbility(StatusType.TreasureHasValueUpgrade);

        
        return ret;
    }
    public static float GetDarkTreasureHasAddValue()
    {
        float ret = 0f;

        ret += GetSkillHasValue(StatusType.DarkHasValueUpgrade);

        ret += GetGradeTestAbilValue(StatusType.DarkHasValueUpgrade);

        ret += GetPassiveSkill2Value(StatusType.DarkHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.DarkHasValueUpgrade);

        ret += GetSpecialTypeAbility(StatusType.DarkHasValueUpgrade);
        
        ret += GetSamchunAbility(StatusType.DarkHasValueUpgrade);

        return ret;
    }
    public static float GetSinsunTreasureHasAddValue()
    {
        float ret = 0f;

        ret += GetSkillHasValue(StatusType.SinsunHasValueUpgrade);

        ret += GetGradeTestAbilValue(StatusType.SinsunHasValueUpgrade);

        ret += GetPassiveSkill2Value(StatusType.SinsunHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.SinsunHasValueUpgrade);

        ret += GetSpecialTypeAbility(StatusType.SinsunHasValueUpgrade);

        ret += GetSamchunAbility(StatusType.SinsunHasValueUpgrade);

        return ret;
    }
    public static float GetGwisalTreasureHasAddValue()
    {
        float ret = 0f;

        //ret += GetSkillHasValue(StatusType.SinsunHasValueUpgrade);

        ret += GetHyunSangTowerAbilValue(StatusType.HyunsangHasValueUpgrade);
        
        ret += GetPassiveSkill2Value(StatusType.HyunsangHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.HyunsangHasValueUpgrade);

        ret += GetSpecialTypeAbility(StatusType.HyunsangHasValueUpgrade);

        return ret;
    }
    public static float GetDragonScaleHasValueUpgrade()
    {
        float ret = 0f;
        
        ret += GetPassiveSkill2Value(StatusType.DragonHasValueUpgrade);
        
        ret += GetSkillHasValue(StatusType.DragonHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.DragonHasValueUpgrade);
        
        ret += GetSpecialTypeAbility(StatusType.DragonHasValueUpgrade);
        
        ret += GetSamchunAbility(StatusType.DragonHasValueUpgrade);
        
        return ret;
    }
    public static float GetDragonPalaceHasValueUpgrade()
    {
        float ret = 0f;
        
        ret += GetPassiveSkill2Value(StatusType.DragonPalaceHasValueUpgrade);
        
        ret += GetSkillHasValue(StatusType.DragonPalaceHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.DragonPalaceHasValueUpgrade);

        ret += GetSpecialTypeAbility(StatusType.DragonPalaceHasValueUpgrade);
        
        ret += GetSamchunAbility(StatusType.DragonPalaceHasValueUpgrade);

        return ret;
    }
    //극락재화
    public static float GetMurimHasValueUpgrade()
    {
        float ret = 0f;
        
        ret += GetPassiveSkill2Value(StatusType.MurimHasValueUpgrade);
        
        ret += GetSkillHasValue(StatusType.MurimHasValueUpgrade);
        
        ret += GetSpecialTypeAbility(StatusType.MurimHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.MurimHasValueUpgrade);

        ret += GetSamchunAbility(StatusType.MurimHasValueUpgrade);

        return ret;
    }
    //난이도보스
    public static float GetDBTHasValueUpgrade()
    {
        float ret = 0f;
        
        ret += GetSpecialTypeAbility(StatusType.DBTHasValueUpgrade);
        
        return ret;
    }
    //연옥 업화
    public static float GetYOTHasValueUpgrade()
    {
        float ret = 0f;
        
        ret += GetPassiveSkill2Value(StatusType.YOTHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.YOTHasValueUpgrade);
        
        ret += GetSkillHasValue(StatusType.YOTHasValueUpgrade);

        return ret;
    }
    public static float GetCSTHasValueUpgrade()
    {
        float ret = 0f;
        
        ret += GetPassiveSkill2Value(StatusType.CSTHasValueUpgrade);
        
        ret += GetCostumeSpecialAbilityValue(StatusType.CSTHasValueUpgrade);
        
        ret += GetSkillHasValue(StatusType.CSTHasValueUpgrade);

        return ret;
    }

    

    #endregion

    #region gainValue

    public static float GetPeachGainValue()
    {
        float ret = 0f;

        ret += GetGuimoonHasEffect2(StatusType.PeachGainPer);
        
        ret += GetMonkeyGodAbil1();

        return ret;
    }
    public static float GetHellGainValue()
    {
        float ret = 0f;

        ret += GetGuimoonHasEffect2(StatusType.HellGainPer);

        ret += GetHellGodAbil1();

        return ret;
    }
    public static float GetChunGainValue()
    {
        float ret = 0f;

        ret += GetGuimoonHasEffect2(StatusType.ChunGainPer);

        ret += GetChunGodAbil1();
        
        return ret;
    }
    public static float GetDokebiFireGainValue()
    {
        float ret = 0f;

        ret += GetGuimoonHasEffect2(StatusType.DokebiFireGainPer);
        
        ret += GetDoGodAbil1();
        
        return ret;
    }
    public static float GetSumiFireGainValue()
    {
        float ret = 0f;

        
        return ret;
    }
    public static float GetSuhoGainValue()
    {
        float ret = 0f;

        ret += GetGuimoonHasEffect2(StatusType.SuhoGainPer);

        ret += GetSpecialSuhoRelicAbil(StatusType.SuhoGainPer);
        
        ret += ServerData.petTable.GetStatusValue(StatusType.SuhoGainPer);
        
        ret += GetMonthBuffEffect(StatusType.SuhoGainPer);
        
        ret += GetMonth2BuffEffect(StatusType.SuhoGainPer);

        return ret;
    }
    public static float GetFoxRelicGainValue()
    {
        float ret = 0f;

        ret += GetGuimoonHasEffect2(StatusType.FoxRelicGainPer);

        ret += GetSpecialFoxRelicAbil(StatusType.FoxRelicGainPer);

        ret += ServerData.petTable.GetStatusValue(StatusType.FoxRelicGainPer);
        
        ret += GetMonthBuffEffect(StatusType.FoxRelicGainPer);
        
        ret += GetMonth2BuffEffect(StatusType.FoxRelicGainPer);

        return ret;
    }
    public static float GetDosulGainValue()
    {
        float ret = 0f;

        ret += GetGuimoonHasEffect2(StatusType.DosulGainPer);
        
        ret += GetSpecialDosulRelicAbil(StatusType.DosulGainPer);
        
        ret += ServerData.petTable.GetStatusValue(StatusType.DosulGainPer);
        
        ret += GetMonthBuffEffect(StatusType.DosulGainPer);
        
        ret += GetMonth2BuffEffect(StatusType.DosulGainPer);

        return ret;
    }
    public static float GetMeditationGainValue()
    {
        float ret = 0f;

        ret += GetSpecialMeditationRelicAbil(StatusType.MeditationGainPer);
        
        ret += ServerData.petTable.GetStatusValue(StatusType.MeditationGainPer);
        
        ret += GetMonthBuffEffect(StatusType.MeditationGainPer);
        
        ret += GetMonth2BuffEffect(StatusType.MeditationGainPer);

        return ret;
    }
    public static float GetYoPowerGoodsGainValue()
    {
        float ret = 0f;

        ret += GetHotTimeEventBuffEffect(StatusType.YoPowerGoodsGainPer);
        
        ret += GetGuimoonHasEffect2(StatusType.YoPowerGoodsGainPer);
        
        return ret;
    }
    public static float GetTaegeukGoodsGainValue()
    {
        float ret = 0f;

        ret += GetHotTimeEventBuffEffect(StatusType.TaegeukGoodsGainPer);
        
        ret += GetGuimoonHasEffect2(StatusType.TaegeukGoodsGainPer);
        
        return ret;
    }
    public static float GetSasinsuGoodsGainValue()
    {
        float ret = 0f;

        ret += GetHotTimeEventBuffEffect(StatusType.SasinsuGoodsGainPer);
        
        return ret;
    }
    

    #endregion

    public static float GetStageRelicUpgradeValue()
    {
        float ret = 0f;

        ret += GetStageAddValue();

        ret += GetEnhanceRelicAbil(StatusType.StageRelicUpgrade);
        

        return ret;
    }
    public static int GetAddSummonYogui()
    {
        float ret = 0f;
        
        ret+= GuildManager.Instance.GetGuildSpawnEnemyNum(GuildManager.Instance.guildLevelExp.Value);
        
        ret += GetGuimoonHasEffect2(StatusType.AddSummonYogui);
        
        ret += (int)ServerData.goodsTable.GetTableData(GoodsTable.du).Value;

        ret += PlayerStats.IsChunMonsterSpawnAdd() ? 5 : 0;

        return (int)ret;
    }




    #region Hyul

    private static float GetNewLowGyungRockAwakeAbilValue()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower4).Value >= 10)
        {
            return GameBalance.newLowGyungRockAwakeValue;
        }
        else
        {
            return 0f;
        }
    }
    private static float GetNewMiddleGyungRockAwakeAbilValue()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower5).Value >= 10)
        {
            return GameBalance.newMiddleGyungRockAwakeValue;
        }
        else
        {
            return 0f;
        }
    }

    private static float GetNewTopGyungRockAwakeAbilValue()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower6).Value >= 10)
        {
            return GameBalance.newTopGyungRockAwakeValue;
        }
        else
        {
            return 0f;
        }
    }
    private static float GetGeukGyungRockAwakeAbilValue()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower7).Value >= 10)
        {
            return GameBalance.newGeukGyungRockAwakeValue;
        }
        else
        {
            return 0f;
        }
    }
    private static float GetGeukGyungRock2AwakeAbilValue()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower8).Value >= 10)
        {
            return GameBalance.newGeukGyungRock2AwakeValue;
        }
        else
        {
            return 0f;
        }
    }
    private static float GetGeukGyungRock3AwakeAbilValue()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower9).Value >= 10)
        {
            return GameBalance.newGeukGyungRock3AwakeValue;
        }
        else
        {
            return 0f;
        }
    }


    //독침
    public static float GetDokChimHasValueUpgrade()
    {
        float ret = 0f;
        
        ret += GetGuildTower2TotalAbility(StatusType.DokChimHasValueUpgrade);
        
        return ret;
    }
    public static float GetGuildTowerChimUpgradeValue()
    {
        return ServerData.goodsTable.TableDatas[GoodsTable.GuildTowerHorn].Value *
               GameBalance.GuildTowerChimAbilUpValue * (1 + GetDokChimHasValueUpgrade());
    }

    public static float GetGyungRockEffect3(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.gyungRockTowerTable3.dataArray;

        int currentLevel = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower3).Value;

        if (currentLevel == 0)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            if ((StatusType)tableDatas[i].Rewardtype == statusType)
            {
                ret += tableDatas[i].Rewardvalue;
            }
        }

        return ret;
    }
    
    public static float GetGyungRockEffect4(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.gyungRockTowerTable4.dataArray;

        int currentLevel = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower4).Value;

        if (currentLevel == 0)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            if ((StatusType)tableDatas[i].Rewardtype == statusType)
            {
                ret += tableDatas[i].Rewardvalue;
            }
        }

        return ret;
    }
    public static float GetGyungRockEffect5(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.gyungRockTowerTable5.dataArray;

        int currentLevel = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower5).Value;

        if (currentLevel == 0)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            if ((StatusType)tableDatas[i].Rewardtype == statusType)
            {
                ret += tableDatas[i].Rewardvalue;
            }
        }

        return ret;
    }
    
    public static float GetGyungRockEffect6(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.gyungRockTowerTable6.dataArray;

        int currentLevel = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower6).Value;

        if (currentLevel == 0)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            if ((StatusType)tableDatas[i].Rewardtype == statusType)
            {
                ret += tableDatas[i].Rewardvalue;
            }
        }

        return ret;
    }    
    public static float GetGyungRockEffect7(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.gyungRockTowerTable7.dataArray;

        int currentLevel = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower7).Value;

        if (currentLevel == 0)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            if ((StatusType)tableDatas[i].Rewardtype == statusType)
            {
                ret += tableDatas[i].Rewardvalue;
            }
        }

        return ret;
    }
        public static float GetGyungRockEffect8(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.gyungRockTowerTable8.dataArray;

        int currentLevel = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower8).Value;

        if (currentLevel == 0)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            if ((StatusType)tableDatas[i].Rewardtype == statusType)
            {
                ret += tableDatas[i].Rewardvalue;
            }
        }

        return ret;
    }
        public static float GetGyungRockEffect9(StatusType statusType, int addLevel = 0)
    {
        float ret = 0f;

        var tableDatas = TableManager.Instance.gyungRockTowerTable9.dataArray;

        int currentLevel = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower9).Value;

        if (currentLevel == 0)
        {
            return 0f;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            if ((StatusType)tableDatas[i].Rewardtype == statusType)
            {
                ret += tableDatas[i].Rewardvalue;
            }
        }

        return ret;
    }
    
    #endregion

    #region Guimoon
    public static bool HasGuimoon(int idx)
    {
        var tableData = TableManager.Instance.GuimoonTable.dataArray;

        return ServerData.guimoonServerTable.TableDatas[tableData[idx].Stringid].level2.Value > 0f;
    }
    

    #endregion
    

    public static int GetPetDispatchGrade()
    {
        var grade = -1;
        
        var score =  ServerData.petTable.GetPetDispatchScore();

        var tableData = TableManager.Instance.PetDispatch.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            //최소치보다 작으면 끝
            if (tableData[i].Minscore > score) break;
            grade = i;
        }
        return grade;
    }

    #region Sasinsu
    public static float GetSasinsuStarAddValue()
    {
        float ret = 0f;

        var tableData = TableManager.Instance.sasinsuTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            for (int j = 0; j < tableData[i].Score.Length; j++)
            {
                if (tableData[i].Score[j] < ServerData.sasinsuServerTable.TableDatas[$"b{i}"].score.Value)
                {
                    ret += tableData[i].Abilvalue0[j];
                }
            }
        }


        return ret;
    }
//0현무 1청룡 2주작 3백호
    public static float GetSasinsuPowerAbility(StatusType statusType,int idx)
    {
        var sum = 0f;
        
        var level = ServerData.etcServerTable.GetSasinsuPowerLevel(idx);

        if (level < 0) return 0f;
        
        var tableData = TableManager.Instance.PetAwakeTable.dataArray[idx];

        if ((StatusType)tableData.Abiltype != statusType) return 0f;

        sum = tableData.Abilvalue[level];

        return sum;
    }
//0현무 1청룡 2주작 3백호
    public static float GetTransJewelAbility(StatusType statusType,int idx)
    {
        var sum = 0f;
        
        var level = ServerData.etcServerTable.GetTransJewelLevel(idx);

        if (level < 0) return 0f;
        
        var tableData = TableManager.Instance.TransJewelAbil.dataArray[idx];

        if ((StatusType)tableData.Abiltype != statusType) return 0f;

        sum = tableData.Abiladdvalue * (level + 1);

        return sum;
    }
//신수 각성
    public static float GetSasinsuAwakePowerAbility(StatusType statusType)
    {
        var sum = 0f;

        var level = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.sasinsuAwakeGrade).Value;

        //0부터시작이라서 0도 0점
        if (level <= 0) return 0f;
        
        var tableData = TableManager.Instance.PetAwakeLevel.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Grade == level)
            {
                if ((StatusType)tableData[i].Abiltype != statusType) return 0f;
                return tableData[i].Abilvalue;
            }
        }

        return sum;
    }
    

    #endregion

    #region Munha
    public static Dictionary<StatusType, float> GetMunhaHyulDictionary()
    {
        ResetMunhaHyulDictionary();
        CreateMunhaHyulDictionary();
        
        return munhaHyulDictionary;
    }
    public static void ResetMunhaHyulDictionary()
    {
        munhaHyulInitialize = false;
    }
    public static void CreateMunhaHyulDictionary()
    {
        if (munhaHyulInitialize == true) return;
        
        munhaHyulDictionary.Clear();
        var tableData = TableManager.Instance.StudentSpot.dataArray;

        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.studentSpotGrade).Value;
        
        
        for (int i = 0; i <= idx; i++)
        {
            AddOrUpdateValue(munhaHyulDictionary, (StatusType)tableData[i].Abil_Type, tableData[i].Abil_Value);
        }


        munhaHyulInitialize = true;

    }
    public static float GetMunhaHyulAbilValue(StatusType statusType)
    {
        int currentGrade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.studentSpotGrade).Value;

        if (currentGrade == -1) return 0f;

        if (munhaHyulInitialize == false)
        {
            CreateMunhaHyulDictionary();
        }
        
        
        if(munhaHyulDictionary.TryGetValue(statusType, out float value))
        {
            return value;
        }

        return 0f;
        
    }
    public static int GetMunhaDispatchGrade()
    {
        var grade = 0;
        
        var tableData = TableManager.Instance.StudentDispatch.dataArray;
        
        var currentLv = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value;
        
        for (int i = 0; i<tableData.Length; i++)
        {
            if (tableData[i].Maxlevel >= currentLv && tableData[i].Minlevel <= currentLv)
            {
                grade = i;
                break;
            }
        }

        return grade;
    }
    //문하생
    public static float GetMunhaAbility(StatusType statusType)
    {
        var ret = 0f;

        var idx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.munhaLevel).Value;

        if (idx == -1) return 0f;
        
        var tableData = TableManager.Instance.StudentTable.dataArray[idx];
        
        if(statusType!=(StatusType)tableData.Abil_Type) return 0f;
        
        return tableData.Abil_Value;

    }//문하생
    public static float GetMunhaTowerAbility()
    {
        var ret = 0f;

        var idx = GetMunhaTowerGrade();

        if (idx == -1) return 0f;
        
        var tableData = TableManager.Instance.StudentAwakeTable.dataArray[idx];
        
        return tableData.Awakevalue;

    }

    public static float GetMunhaTower2Ability(StatusType type)
    {
        var ret = 0f;
        var idx = (int)PlayerStats.GetMunhaGrade();

        if (idx < 0)
        {
            return 0f;
        }

        var data = TableManager.Instance.StudentTower.dataArray[idx];

        if (type != (StatusType)data.Abil_Type) return 0f;

        return data.Abil_Value;

    }
    

    #endregion
   

    public static float GetSpecialTypeAbility(StatusType type)
    {
        var ret = 0f;

        var tableData = TableManager.Instance.Title_Special.dataArray;

        foreach (var t in tableData)
        {
            if((StatusType)t.Abiltype!=type) continue;
            if (ServerData.specialTitleServerTable.TableDatas[t.Stringid].hasItem.Value < 1) continue;
            ret += t.Abilvalue;
        }
        
        return ret;

    }
    

    
    public static float SuperCritical8AddDam()
    {
        float ret = 0f;

        
        ret += GetDanjeonAbilValue(StatusType.SuperCritical8AddDam);
        
        return ret;
    }
    public static float SuperCritical13AddDam()
    {
        float ret = 0f;

        
        ret += GetDanjeonAbilValue(StatusType.SuperCritical13AddDam);
        
        return ret;
    }
    public static float SuperCritical18AddDam()
    {
        float ret = 0f;

        
        ret += GetDanjeonAbilValue(StatusType.SuperCritical18AddDam);
        
        return ret;
    }

    public static int GetStudentSpotLevel(int grade)
    {

        if (grade < 0)
        {
            return 0;
        }
        else
        {
            return TableManager.Instance.StudentSpot.dataArray[grade].Level;
        }
    }
    public static float GetGuildTower2TotalAbility(StatusType type)
    {
        var grade = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildTower2ClearIndex).Value-1;

        var tableData = TableManager.Instance.GuildTowerTable2.dataArray;

        var sum = 0f;
        if (grade < 0)
        {
            return sum;
        }
        
        for (int i = 0; i < tableData.Length; i++)
        {
            if (grade < i) break;
            if ((StatusType)tableData[i].Abiltype != type) continue;
            sum += tableData[i].Abilvalue;
        }

        return sum;

    }

    #region Dimension

    
    public static float GetDimensionBaseAttackDam()
    {
        float ret = GameBalance.dimensionBaseAttackDamage;

        ret += ServerData.dimensionStatusTable.GetStatusValue(DimensionStatusTable.A_DS);
        
        ret += GetDimensionEquipmentAbility(DimensionStatusType.BaseAttackDam);

        
        return ret;
    }
    
    public static float GetDimensionAttackAddPer()
    {
        float ret = 0f;
        
        ret += ServerData.dimensionStatusTable.GetStatusValue(DimensionStatusTable.AP_DS);

        return ret;
    }
    public static float GetDimensionBaseSkillDam()
    {
        float ret = GameBalance.dimensionBaseSkillDamage;
        
        return ret;
    }
    
    public static float GetDimensionAddSkillDamPer()
    {
        float ret = 0f;
        
        ret += ServerData.dimensionStatusTable.GetStatusValue(DimensionStatusTable.SD_DS);

        
        return ret;
    }
    
    public static float GetDimensionReduceSkillCoolTime()
    {
        float ret = 0f;
        
        ret += GetDimensionEquipmentAbility(DimensionStatusType.ReduceSkillCoolTimePer);
        
        return ret;
    }
    public static float GetDimensionAddHp()
    {
        float ret = 0f;
        
        ret += GetDimensionEquipmentAbility(DimensionStatusType.AddHp);
        
        return ret;
    }
    public static float GetDimensionCubeGainPer()
    {
        float ret = 0f;
        
        ret += GetDimensionBuffAbility(DimensionStatusType.CubeGainPer);

        return ret;
    }
    public static float GetDimensionEssenceGainPer()
    {
        float ret = 0f;
        
        ret += GetDimensionBuffAbility(DimensionStatusType.EssenceGainPer);

        return ret;
    }

    public static float GetDimensionEquipmentAbility(DimensionStatusType type)
    {
        float ret = 0f;

        var grade = ServerData.equipmentTable.TableDatas[EquipmentTable.DimensionEquipment].Value;

        var tableData = TableManager.Instance.DimensionEquip.dataArray[grade];

        if ((DimensionStatusType)tableData.Equipeffecttype1 == type)
        {
            ret += tableData.Equipeffectvalue1;
        }
        if ((DimensionStatusType)tableData.Equipeffecttype2 == type)
        {
            ret += tableData.Equipeffectvalue2;
        }
        if ((DimensionStatusType)tableData.Equipeffecttype3 == type)
        {
            ret += tableData.Equipeffectvalue3;
        }

        return ret;
    }
    public static float GetDimensionBuffAbility(DimensionStatusType type)
    {
        float ret = 0f;

        var seasonData = Utils.GetCurrentDimensionSeasonData();
        var seasonId = seasonData.Id;
        
        //패스 미구매
        if (ServerData.iapServerTable.TableDatas[seasonData.Productid].buyCount.Value < 1)
        {
            return 0f;
        }
        
        
        var tableData = TableManager.Instance.DimensionBuff.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Startseasonid > seasonId) continue;
            if (tableData[i].Endseasonid < seasonId) continue;
            if((DimensionStatusType)tableData[i].Statustype!=type) continue;
            ret += tableData[i].Statusvalue;
        }
        
        return ret;
    }

    #endregion
}