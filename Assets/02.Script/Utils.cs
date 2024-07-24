using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;
using BackEnd;
using Firebase.Analytics;
using Random = UnityEngine.Random;

public static class Utils
{
    public static bool IsSet(this TutorialStep self, TutorialStep flag)
    {
        return (self & flag) == flag;
    }

    public static bool IsSet(this ManagerDescriptionType self, ManagerDescriptionType flag)
    {
        return (self & flag) == flag;
    }

    public static bool IsSleepItem(this Item_Type type)
    {
        return type == Item_Type.SleepRewardItem;
    }
    public static int GetDayOfWeek()
    {
        var serverTime = ServerData.userInfoTable.currentServerTime;
        return (int)serverTime.DayOfWeek;
    }
    //지배 영향 안받는 능력치
    public static bool IsAbsoluteStatus(this StatusType type)
    {
        return type == StatusType.ExpGainPer||
               type == StatusType.GoldGainPer||
               type == StatusType.GoldBarGainPer||
               type == StatusType.SuhoGainPer||
               type == StatusType.FoxRelicGainPer||
               type == StatusType.DosulGainPer||
               type == StatusType.MeditationGainPer;
    }
    public static bool IsCostumeItem(this Item_Type type)
    {
        return (type >= Item_Type.costume0 && type <= Item_Type.costume99) ||
               (type >= Item_Type.costume100 && type <= Item_Type.costume_end);
    }
    public static bool IsPetItem(this Item_Type type)
    {
        return type == Item_Type.pet0 ||
               type == Item_Type.pet1 ||
               type == Item_Type.pet2 ||
               type == Item_Type.pet3 ||
               type >= Item_Type.pet52 && type < Item_Type.pet_end;
    }
    public static bool IsDimensionPostItem(this Item_Type type)
    {
        return type >= Item_Type.Dimension_Ranking_Reward_1  && type < Item_Type.Dimension_Ranking_Reward_End;
    }
    public static bool IsEventRewardPostItem(this Item_Type type)
    {
        return type >= Item_Type.EventReward_HotTime_0  && type < Item_Type.EventReward_Post_End;
    }

    public static bool IsNorigaeItem(this Item_Type type)
    {
        return (type >= Item_Type.magicBook0 && type <= Item_Type.magicBook_End) ||
               type == Item_Type.MonthNorigae0 ||
               type == Item_Type.MonthNorigae1 ||
               type == Item_Type.MonthNorigae2 ||
               type == Item_Type.MonthNorigae3 ||
               type == Item_Type.MonthNorigae4 ||
               type == Item_Type.MonthNorigae5 ||
               type == Item_Type.MonthNorigae6||
               type == Item_Type.MonthNorigae7||
               type == Item_Type.MonthNorigae8||
               type == Item_Type.MonthNorigae9||
               type == Item_Type.MonthNorigae10||
               type == Item_Type.MonthNorigae11||
               type == Item_Type.magicBook116||
               type == Item_Type.magicBook117||
               type == Item_Type.magicBook120||
               type == Item_Type.magicBook121 ||
               type == Item_Type.magicBook122 ||
               type == Item_Type.magicBook123 ||
               type == Item_Type.magicBook124 ||
               type == Item_Type.magicBook125 ||
               type == Item_Type.magicBook126 ||
               type == Item_Type.magicBook127 ||
               type == Item_Type.magicBook128 ||
               type == Item_Type.magicBook129;
    }

    public static bool IsWeaponItem(this Item_Type type)
    {
        return (type >= Item_Type.weapon0 && type <= Item_Type.weapon_end)||
               type == Item_Type.weapon81 ||
               type == Item_Type.weapon90||
               type == Item_Type.weapon131||
               type == Item_Type.weapon146||
               type == Item_Type.weapon149||
               type == Item_Type.weapon150||
               type == Item_Type.weapon151||
               type == Item_Type.weapon152||
               type == Item_Type.weapon153||
               type == Item_Type.weapon154||
               type == Item_Type.weapon155||
               type == Item_Type.weapon156;
    }
    


    public static bool IsIgnoreMissionKey(this MonthMissionKey type)
    {
        int type_int = (int)type;
        return type_int >= (int)MonthMissionKey.ClearBandit && type <= MonthMissionKey.ClearSumiFire;
    }
    public static bool IsDailyEventMissionKey(this EventMissionKey type)
    {
        bool isDaily = (type >= EventMissionKey.SMISSION1 && type <= EventMissionKey.SMISSION9)||
                       (type >= EventMissionKey.BMISSION1 && type <= EventMissionKey.BMISSION8)||
                       (type >= EventMissionKey.NMARBLEMISSION1 && type <= EventMissionKey.NMARBLEMISSION8)||
                       (type >= EventMissionKey.FMISSION1 && type <= EventMissionKey.FMISSION8);
        return isDaily;
    }

    public static bool IsRegainableItem(this Item_Type type)
    {
        return type == Item_Type.WeaponUpgradeStone || //힘의증표
               type == Item_Type.YomulExchangeStone || //탐욕증표
               type == Item_Type.TigerBossStone || //강함
               type == Item_Type.RabitBossStone ||
               type == Item_Type.DragonBossStone ||
               type == Item_Type.SnakeStone ||
               type == Item_Type.HorseStone ||
               type == Item_Type.SheepStone ||
               type == Item_Type.MonkeyStone ||
               type == Item_Type.CockStone ||
               type == Item_Type.DogStone ||
               type == Item_Type.PigStone ||
               type == Item_Type.gumiho0 ||
               type == Item_Type.gumiho1 ||
               type == Item_Type.gumiho2 ||
               type == Item_Type.gumiho3 ||
               type == Item_Type.gumiho4 ||
               type == Item_Type.gumiho5 ||
               type == Item_Type.gumiho6 ||
               type == Item_Type.gumiho7 ||
               type == Item_Type.gumiho8 ||
               type == Item_Type.Asura0 ||
               type == Item_Type.Asura1 ||
               type == Item_Type.Asura2 ||
               type == Item_Type.Asura3 ||
               type == Item_Type.Asura4 ||
               type == Item_Type.Asura5 ||
               type == Item_Type.Indra0 ||
               type == Item_Type.Indra1 ||
               type == Item_Type.Indra2 ||
               type == Item_Type.IndraPower ||
               type == Item_Type.h0 ||
               type == Item_Type.h1 ||
               type == Item_Type.h2 ||
               type == Item_Type.h3 ||
               type == Item_Type.h4 ||
               type == Item_Type.h5 ||
               type == Item_Type.h6 ||
               type == Item_Type.h7 ||
               type == Item_Type.h8 ||
               type == Item_Type.h9 ||
               type == Item_Type.c0 ||
               type == Item_Type.c1 ||
               type == Item_Type.c2 ||
               type == Item_Type.c3 ||
               type == Item_Type.c4 ||
               type == Item_Type.c5 ||
               type == Item_Type.c6 ||
               
               type == Item_Type.SinSkill0 ||
               type == Item_Type.SinSkill1 ||
               type == Item_Type.SinSkill2 ||
               type == Item_Type.SinSkill3 ||

               type == Item_Type.d0 ||
               type == Item_Type.d1 ||
               type == Item_Type.d2 ||
               type == Item_Type.d3 ||
               type == Item_Type.d4 ||
               type == Item_Type.d5 ||
               type == Item_Type.d6 ||
               type == Item_Type.d7
            ;
    }

    public static bool IsTreasureItem(this Item_Type type)
    {
        return type >= Item_Type.MRT && type <= Item_Type.Treasure_End;
    }
    public static bool IsDimensionItem(this Item_Type type)
    {
        return type >= Item_Type.DC && type <= Item_Type.Dimension_End;
    }
    public static bool IsVisionSkill(this Item_Type type)
    {
        return type >= Item_Type.VisionSkill17 && type <= Item_Type.VisionSkill_End;
    }
    public static bool IsGoods(this Item_Type type)
    {
        return type >= Item_Type.SB && type <= Item_Type.Goods_End;
    }
    public static bool IsDimensionContents(this GameManager.ContentsType type)
    {
        return type == GameManager.ContentsType.Dimension;
    }
    public static bool IsGoodsItem(this Item_Type type)
    {
        return type == Item_Type.Gold ||
               type == Item_Type.GoldBar ||
               type == Item_Type.Jade ||
               type == Item_Type.GrowthStone ||
               type == Item_Type.Marble ||
               type == Item_Type.Ticket ||
               type == Item_Type.Songpyeon ||
               type == Item_Type.Event_Item_0 ||
               type == Item_Type.Event_Item_1 ||
               type == Item_Type.PeachReal ||
               type == Item_Type.SP ||
               type == Item_Type.Hae_Norigae ||
               type == Item_Type.Hae_Pet ||
               type == Item_Type.Sam_Pet ||
               type == Item_Type.Sam_Norigae ||
               type == Item_Type.KirinNorigae ||
               type == Item_Type.Kirin_Pet ||
               type == Item_Type.DogNorigae ||
               type == Item_Type.DogPet ||
               type == Item_Type.ChunMaPet ||
               type == Item_Type.ChunPet0 ||
               type == Item_Type.ChunPet1 ||
               type == Item_Type.ChunPet2 ||
               type == Item_Type.ChunPet3 ||
               type == Item_Type.SasinsuPet0 ||
               type == Item_Type.SasinsuPet1 ||
               type == Item_Type.SasinsuPet2 ||
               type == Item_Type.SasinsuPet3 ||
               type == Item_Type.SasinsuPet4 ||
               type == Item_Type.SasinsuPet5 ||
               type == Item_Type.SasinsuPet6 ||
               type == Item_Type.SasinsuPet7 ||
               type == Item_Type.SahyungPet0 ||
               type == Item_Type.SahyungPet1 ||
               type == Item_Type.SahyungPet2 ||
               type == Item_Type.SahyungPet3 ||
               type == Item_Type.VisionPet0 ||
               type == Item_Type.VisionPet1 ||
               type == Item_Type.VisionPet2 ||
               type == Item_Type.VisionPet3 ||
               type == Item_Type.FoxPet0 ||
               type == Item_Type.FoxPet1 ||
               type == Item_Type.FoxPet2 ||
               type == Item_Type.FoxPet3 ||
               type == Item_Type.TigerPet0 ||
               type == Item_Type.TigerPet1 ||
               type == Item_Type.TigerPet2 ||
               type == Item_Type.TigerPet3 ||
               type == Item_Type.ChunGuPet0 ||
               type == Item_Type.ChunGuPet1 ||
               type == Item_Type.ChunGuPet2 ||
               type == Item_Type.ChunGuPet3 ||
               type == Item_Type.pet52 ||
               type == Item_Type.pet53 ||
               type == Item_Type.pet54 ||
               type == Item_Type.pet55 ||
               type == Item_Type.pet56 ||
               type == Item_Type.pet57 ||
               type == Item_Type.pet58 ||
               type == Item_Type.SpecialSuhoPet0 ||
               type == Item_Type.SpecialSuhoPet1 ||
               type == Item_Type.SpecialSuhoPet2 ||
               type == Item_Type.SpecialSuhoPet3 ||
               type == Item_Type.SpecialSuhoPet4 ||
               type == Item_Type.SpecialSuhoPet5 ||
               type == Item_Type.SpecialSuhoPet6 ||
               type == Item_Type.SpecialSuhoPet7 ||
               type == Item_Type.SpecialSuhoPet8 ||
               type == Item_Type.SpecialSuhoPet9 ||
               type == Item_Type.SpecialSuhoPet10 ||
               type == Item_Type.SpecialSuhoPet11 ||
               type == Item_Type.SpecialSuhoPet12 ||
               type == Item_Type.SpecialSuhoPet13 ||
               type == Item_Type.RabitPet ||
               type == Item_Type.RabitNorigae ||
               type == Item_Type.YeaRaeNorigae ||
               type == Item_Type.GangrimNorigae ||
               type == Item_Type.ChunNorigae0 ||
               type == Item_Type.ChunNorigae1 ||
               type == Item_Type.ChunNorigae2 ||
               type == Item_Type.ChunNorigae3 ||
               type == Item_Type.ChunNorigae4 ||
               type == Item_Type.ChunNorigae5 ||
               type == Item_Type.ChunNorigae6 ||
               type == Item_Type.DokebiNorigae0 ||
               type == Item_Type.DokebiNorigae1 ||
               type == Item_Type.DokebiNorigae2 ||
               type == Item_Type.DokebiNorigae3 ||
               type == Item_Type.DokebiNorigae4 ||
               type == Item_Type.DokebiNorigae5 ||
               type == Item_Type.DokebiNorigae6 ||
               type == Item_Type.DokebiNorigae7 ||
               type == Item_Type.DokebiNorigae8 ||
               type == Item_Type.DokebiNorigae9 ||
               type == Item_Type.SumisanNorigae0 ||
               type == Item_Type.MonthNorigae0 ||
               type == Item_Type.MonthNorigae1 ||
               type == Item_Type.MonthNorigae2 ||
               type == Item_Type.MonthNorigae3 ||
               type == Item_Type.MonthNorigae4 ||
               type == Item_Type.MonthNorigae5 ||
               type == Item_Type.MonthNorigae6 ||
               type == Item_Type.MonthNorigae7 ||
               type == Item_Type.MonthNorigae8 ||
               type == Item_Type.MonthNorigae9 ||
               type == Item_Type.MonthNorigae10 ||
               type == Item_Type.MonthNorigae11 ||
               type == Item_Type.magicBook116 ||
               type == Item_Type.magicBook117 ||
               type == Item_Type.magicBook120 ||
               type == Item_Type.magicBook123 ||
               type == Item_Type.weapon146 ||
               type == Item_Type.weapon149 ||
               type == Item_Type.weapon154 ||
               type == Item_Type.DokebiHorn0 ||
               type == Item_Type.DokebiHorn1 ||
               type == Item_Type.DokebiHorn2 ||
               type == Item_Type.DokebiHorn3 ||
               type == Item_Type.DokebiHorn4 ||
               type == Item_Type.DokebiHorn5 ||
               type == Item_Type.DokebiHorn6 ||
               type == Item_Type.DokebiHorn7 ||
               type == Item_Type.DokebiHorn8 ||
               type == Item_Type.DokebiHorn9 ||
               type == Item_Type.ChunSun0 ||
               type == Item_Type.ChunSun1 ||
               type == Item_Type.ChunSun2 ||
               type == Item_Type.YeaRaeWeapon ||
               type == Item_Type.GangrimWeapon ||
               type == Item_Type.HaeWeapon ||
               type == Item_Type.SmithFire ||
               type == Item_Type.Event_Item_SnowMan ||
               type == Item_Type.Event_Item_SnowMan_All ||
               type == Item_Type.MihoNorigae ||
               type == Item_Type.MihoWeapon ||
               type == Item_Type.ChunMaNorigae ||
               type == Item_Type.Hel || type == Item_Type.Ym ||
               type == Item_Type.du ||
               type == Item_Type.Fw ||
               type == Item_Type.Cw ||
               type == Item_Type.DokebiFire ||
               type == Item_Type.DokebiFireKey ||
               type == Item_Type.FoxMaskPartial ||
               type == Item_Type.SusanoTreasure ||
               type == Item_Type.SahyungTreasure ||
               type == Item_Type.VisionTreasure ||
               type == Item_Type.DarkTreasure ||
               type == Item_Type.SinsunTreasure ||
               type == Item_Type.DragonScale ||
               type == Item_Type.GwisalTreasure ||
               type == Item_Type.ChunguTreasure ||
               type == Item_Type.GuildTowerClearTicket ||
               type == Item_Type.SinsuMarble ||
               type == Item_Type.Mileage ||
               type == Item_Type.ClearTicket ||
               type == Item_Type.Event_Kill1_Item ||
               type == Item_Type.Event_HotTime ||
               type == Item_Type.Event_Collection_All ||
               type == Item_Type.Event_Fall_Gold ||
               type == Item_Type.Event_NewYear ||
               type == Item_Type.Event_Mission2 ||
               type == Item_Type.Event_Mission3 ||
               type == Item_Type.SumiFire ||
               type == Item_Type.SealWeaponClear ||
               type == Item_Type.SumiFireKey ||
               type == Item_Type.RelicTicket ||
               type == Item_Type.SinsuRelic ||
               type == Item_Type.HyungsuRelic ||
               type == Item_Type.ChunguRelic ||
               type == Item_Type.FoxRelic ||
               type == Item_Type.FoxRelicClearTicket ||
               type == Item_Type.TransClearTicket ||
               type == Item_Type.MeditationGoods ||
               type == Item_Type.MeditationClearTicket ||
               type == Item_Type.DaesanGoods ||
               type == Item_Type.HonorGoods ||
               type == Item_Type.NewGachaEnergy ||
               type == Item_Type.EventDice ||
               type == Item_Type.Event_SA ||
               type == Item_Type.SuhoPetFeed ||
               type == Item_Type.SuhoPetFeedClear ||
               type == Item_Type.SoulRingClear ||
               type == Item_Type.Tresure ||
               type == Item_Type.DosulGoods ||
               type == Item_Type.TransGoods ||
               type == Item_Type.DosulClear ||
               type == Item_Type.BlackFoxGoods ||
               type == Item_Type.BlackFoxClear ||
               type == Item_Type.TaeguekGoods ||
               type == Item_Type.TaeguekElixir ||
               type == Item_Type.YoPowerGoods ||
               type == Item_Type.ByeolhoGoods ||
               type == Item_Type.ByeolhoClear ||
               type == Item_Type.GuildTowerHorn ||
               type == Item_Type.BattleClear ||
               type == Item_Type.SG ||
               type == Item_Type.SC ||
               type == Item_Type.SB ||
               type == Item_Type.GuimoonRelic ||
               type == Item_Type.GuimoonRelicClearTicket ||
               IsTreasureItem(type)||
               IsDimensionItem(type)||
               IsVisionSkill(type)||
               IsGoods(type)||
               IsSkillItem(type)
            ;
    }

    public static bool IsStatusItem(this Item_Type type)
    {
        return type == Item_Type.Memory;
    }





    public static bool IsSkillItem(this Item_Type type)
    {
        return
            (type >= Item_Type.skill0 && type <= Item_Type.skill8) ||
            (type >= Item_Type.GRSkill0 && type <= Item_Type.Skill_End);
    }

    public static bool IsPercentStat(this StatusType type)
    {
        return
            type != StatusType.MoveSpeed &&
            type != StatusType.DamBalance &&
            type != StatusType.AttackAdd &&
            type != StatusType.Hp &&
            type != StatusType.Mp &&
            type != StatusType.IgnoreDefense &&
            type != StatusType.DashCount &&
            type != StatusType.SkillAttackCount &&
            type != StatusType.SealSwordDam &&
            type != StatusType.AddSummonYogui&&
            type != StatusType.AddVisionSkillUseCount&&
            type != StatusType.AddSealSwordSkillHitCount&&
            type != StatusType.ReduceDosulSkillCoolTime&&
            type != StatusType.ReduceSealSwordSkillRequireCount;
    }
    public static bool IsPercentStat(this DimensionStatusType type)
    {
        return
            type != DimensionStatusType.BaseAttackDam &&
            type != DimensionStatusType.BaseSkillDam &&
            type != DimensionStatusType.AddHp;
    }

    public static bool IsBossContents(this GameManager.ContentsType type)
    {
        return type == GameManager.ContentsType.Boss || //O
               type == GameManager.ContentsType.TwelveDungeon || //O
               type == GameManager.ContentsType.Son || //O
               type == GameManager.ContentsType.FoxMask || //O
               type == GameManager.ContentsType.TaeguekTower || //O
               type == GameManager.ContentsType.Susano || //O
               type == GameManager.ContentsType.Hell || //O
               type == GameManager.ContentsType.HellWarMode || //O
               type == GameManager.ContentsType.PartyRaid || //O
               type == GameManager.ContentsType.Yum ||
               type == GameManager.ContentsType.Ok ||
               type == GameManager.ContentsType.Do ||
               type == GameManager.ContentsType.Sumi ||
               type == GameManager.ContentsType.PartyRaid_Guild || //O
               type == GameManager.ContentsType.Thief|| //O
               type == GameManager.ContentsType.Dark| //O
               type == GameManager.ContentsType.Sinsun; //O
    }

    public static bool IsRankFrameItem(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1 && type <= Item_Type.RankFrame1001_10000;
    }

    public static bool IsPartyRaidRankFrameItem(this Item_Type type)
    {
        return type >= Item_Type.PartyRaidRankFrame1 && type <= Item_Type.PartyRaidRankFrame1001_10000;
    }

    public static bool IsMergePartyRaidRankFrameItem(this Item_Type type)
    {
        return type >= Item_Type.MergePartyRaidRankFrame1 && type <= Item_Type.MergePartyRaidRankFrame1001_5000;
    }

    public static bool IsMergePartyRaidRankFrameItem_0(this Item_Type type)
    {
        return type >= Item_Type.MergePartyRaidRankFrame_0_1 && type <= Item_Type.MergePartyRaidRankFrame_0_1001_5000;
    }
    public static bool IsMergePartyRaidRankFrameItem_1(this Item_Type type)
    {
        return type >= Item_Type.MergePartyRaidRankFrame_1_1 && type <= Item_Type.MergePartyRaidRankFrame_1_1001_5000;
    }

    public static bool IsWeeklyStangRankItem_1(this Item_Type type)
    {
        return type >= Item_Type.WeeklyRankingReward_1_1 && type <= Item_Type.WeeklyRankingReward_1_1001_5000;
    }

    public static bool IsRelicRewardItem(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1_relic && type <= Item_Type.RankFrame1001_10000_relic;
    }

    public static bool IsRelicRewardItem_hell(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1_relic_hell && type <= Item_Type.RankFrame1001_10000_relic_hell;
    }

    public static bool IsHellWarItem(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1_2_war_hell && type <= Item_Type.RankFrame1001_10000_war_hell;
    }

    public static bool IsMiniGameRewardItem(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1_miniGame && type <= Item_Type.RankFrame1001_10000_miniGame;
    }

    public static bool IsMiniGameRewardItem2(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1_new_miniGame && type <= Item_Type.RankFrame1001_10000_new_miniGame;
    }

    public static bool IsGuildRewardItem(this Item_Type type)
    {
        return (type >= Item_Type.RankFrame1_guild && type <= Item_Type.RankFrame101_1000_guild) ||
               (type >= Item_Type.RankFrame1guild_new && type <= Item_Type.RankFrame51_100_guild_new) ||
               (type >= Item_Type.RankFrameParty1guild_new && type <= Item_Type.RankFrameParty51_100_guild_new)
            ;
    }
    public static bool IsGuildReward2Item(this Item_Type type)
    {
        return (type >= Item_Type.RedFoxFrame1_guild && type <= Item_Type.RedFoxFrame21_100_guild) ||
               (type >= Item_Type.Sangun_1guild_new && type <= Item_Type.Sangun_21_100_guild_new) 
            ;
    }

    public static bool IsGangChulItem(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1_boss_new && type <= Item_Type.RankFrame1000_3000_boss_new;
    }

    public static bool IsRealGangChulItem(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1_boss_GangChul && type <= Item_Type.RankFrame1000_3000_boss_GangChul;
    }
    public static bool IsUpdateRewardItem(this Item_Type type)
    {
        return type == Item_Type.UpdateRewardMail;
    }

    public static int GetRandomIdx(List<float> inputDatas)
    {
        float total = 0;

        for (int i = 0; i < inputDatas.Count; i++)
        {
            total += inputDatas[i];
        }

        float pivot = UnityEngine.Random.Range(0f, 1f) * total;

        for (int i = 0; i < inputDatas.Count; i++)
        {
            if (pivot < inputDatas[i])
            {
                return i;
            }
            else
            {
                pivot -= inputDatas[i];
            }
        }

        return 0;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Rect GetWorldBounds(this BoxCollider2D boxCollider2D)
    {
        float worldRight = boxCollider2D.transform
            .TransformPoint(boxCollider2D.offset + new Vector2(boxCollider2D.size.x * 0.5f, 0)).x;
        float worldLeft = boxCollider2D.transform
            .TransformPoint(boxCollider2D.offset - new Vector2(boxCollider2D.size.x * 0.5f, 0)).x;

        float worldTop = boxCollider2D.transform
            .TransformPoint(boxCollider2D.offset + new Vector2(0, boxCollider2D.size.y * 0.5f)).y;
        float worldBottom = boxCollider2D.transform
            .TransformPoint(boxCollider2D.offset - new Vector2(0, boxCollider2D.size.y * 0.5f)).y;

        return new Rect(
            worldLeft,
            worldBottom,
            worldRight - worldLeft,
            worldTop - worldBottom
        );
    }

    public static string ListToString(List<string> list)
    {
        return String.Join(", ", list.ToArray());
    }

    public static List<string> StringToList(string data)
    {
        return data.Split(',').ToList();
    }

    public static DateTime ConvertFromUnixTimestamp(double timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return origin.AddSeconds(timestamp + 1620000000f);
    }
    public static string ConvertStage(int currentStage)
    {
        string stage="";

        var firstInt = currentStage / 100;
        var secondInt = currentStage - (firstInt * 100);
        var firststring = (firstInt + 1).ToString();
        var secondstring = "";
        if (firstInt > 0)
        {
            secondstring = (secondInt+1).ToString();
        }
        else
        {
            secondstring = (secondInt).ToString();
            
        }
        stage += firststring + "-" + secondstring;
        return stage;

    }    

    public static void AddOrUpdateValue<T1,T2>(ref Dictionary<T1, T2> dictionary, T1 key, T2 value)
    {
        if (dictionary.ContainsKey(key))
        {
            // 형식 T2로 변환
            T2 existingValue = dictionary[key];

            // 덧셈을 수행하고 결과를 T2로 변환
            T2 newValue = (T2)Convert.ChangeType(Convert.ToDouble(existingValue) + Convert.ToDouble(value), typeof(T2));

            // 딕셔너리 업데이트
            dictionary[key] = newValue;        
        }
        else
        {
            dictionary.Add(key, value);
        }
    }
    public static void AddOrUpdateReward(ref List<RewardItem> rewardList, Item_Type itemType, float itemValue)
    {
        int existingRewardIndex = rewardList.FindIndex(r => r.ItemType == itemType);

        if (existingRewardIndex >= 0)
        {
            RewardItem existingReward = rewardList[existingRewardIndex];
            existingReward.ItemValue += itemValue;
            rewardList[existingRewardIndex] = existingReward;
        }
        else
        {
            rewardList.Add(new RewardItem(itemType, itemValue));
        }
    }

    public static double ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return diff.TotalSeconds - 1620000000f;
    }

    public static int GetWeekNumber(DateTime currentDate)
    {
        DateTime startDate = new DateTime(2021, 1, 1); //기준일

        Calendar calenderCalc = CultureInfo.CurrentCulture.Calendar;

        return calenderCalc.GetWeekOfYear(currentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) -
               calenderCalc.GetWeekOfYear(startDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    public static float SmallFloatToDecimalFloat(float _float, int essenceNum = 1)
    {
        if (_float == 0) return 0f;

        int count = 0;
        float result = _float;
        //essenceNum =  보관할 숫자 수 
        while (_float < Mathf.Pow(10, essenceNum - 1))
        {
            _float *= 10f;
            count++;
        }

        float devideNum = Mathf.Pow(10, count);
        result = Mathf.Round(_float) / devideNum;
        return result;
    }

    #region BigFloat

    private static string[] goldUnitArr = GameBalance.damageUnit;

    private static double p = (double)Mathf.Pow(10, 4);
    private static List<double> numList = new List<double>();
    private static List<string> numStringList = new List<string>();
    private static string zeroString = "0";

    public static string ConvertBigNum(double data)
    {
#if UNITY_EDITOR
        bool isUnderZero = data < 0;
        if (data < 0)
        {
            data *= -1f;
        }
#endif
        //
        if (data == 0f)
        {
            return zeroString;
        }

        double value = data;

        numList.Clear();
        numStringList.Clear();

        do
        {
            numList.Add((value % p));
            value /= p;
        } while (value >= 1);

        string retStr = "";

        if (numList.Count >= 3)
        {
            for (int i = numList.Count - 1; i >= numList.Count - 2; i--)
            {
                if (numList[i] == 0) continue;

                if (Math.Truncate(numList[i]) == 0) continue;
                var item = Math.Truncate(numList[i]) + goldUnitArr[i];
                numStringList.Add(item);

            }

            for (int i = 0; i < numStringList.Count; i++)
            {
                retStr += numStringList[i];
            }
#if UNITY_EDITOR
            if (isUnderZero)
            {
                return "-" + retStr;
            }
#endif
            return retStr;
        }
        else
        {
            for (int i = 0; i < numList.Count; i++)
            {
                if (numList[i] == 0) continue;
                retStr = Math.Truncate(numList[i]) + goldUnitArr[i] + retStr;
            }
#if UNITY_EDITOR
            if (isUnderZero)
            {
                return "-" + retStr;
            }
#endif
            return retStr;
        }
    }

//소수점 변환 count= 표시할 자리수
    public static string ConvertSmallNum(float data, int count)
    {
        int smallCount = 1;
        for (int i = 0; i < count; i++)
        {
            smallCount *= 10;
        }

        var result = Mathf.Round(data * smallCount) / smallCount;

        return result.ToString();
    }

    public static string ConvertNum(double data, int count = 0)
    {
        if (data > 10000)
        {
            return ConvertBigNumForRewardCell(data);
        }
        else
        {
            return ConvertSmallNum((float)data, count);
        }
    }
    
//
    

    public static string ConvertBigNumForRewardCell(double data)
    {

        data += data / 100000000d;

#if UNITY_EDITOR
        bool isUnderZero = data < 0;
        if (data < 0)
        {
            data *= -1f;
        }
#endif
        //
        if (data == 0f)
        {
            return zeroString;
        }

        double value = data;

        numList.Clear();
        numStringList.Clear();

        do
        {
            numList.Add((value % p));
            value /= p;
        } while (value >= 1);

        string retStr = "";

        if (numList.Count >= 3)
        {
            for (int i = numList.Count - 1; i >= numList.Count - 2; i--)
            {
                if (numList[i] == 0) continue;
                if(numList[i]<1) continue;
                if (Math.Truncate(numList[i]) == 0) continue;
                var item = Math.Truncate(numList[i]) + goldUnitArr[i];
                numStringList.Add(item);
            }

            for (int i = 0; i < numStringList.Count; i++)
            {
                retStr += numStringList[i];
            }
#if UNITY_EDITOR
            if (isUnderZero)
            {
                return "-" + retStr;
            }
#endif
            return retStr;
        }
        else
        {
            for (int i = 0; i < numList.Count; i++)
            {
                if (Math.Truncate(numList[i]) == 0) continue;
                retStr = Math.Truncate(numList[i]) + goldUnitArr[i] + retStr;
            }
#if UNITY_EDITOR
            if (isUnderZero)
            {
                return "-" + retStr;
            }
#endif
            return retStr;
        }
    }
    

    #endregion

    public static void RestartApplication()
    {
#if UNITY_IOS
        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "네트워크 연결이 끊겼습니다.\n앱을 종료합니다.",confirmCallBack:()=>
        {
            Application.Quit();
        });

        return;
#endif
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject pm = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", Application.identifier);
            intent.Call<AndroidJavaObject>("setFlags", 0x20000000); //Intent.FLAG_ACTIVITY_SINGLE_TOP

            AndroidJavaClass pendingIntent = new AndroidJavaClass("android.app.PendingIntent");
            AndroidJavaObject contentIntent =
                pendingIntent.CallStatic<AndroidJavaObject>("getActivity", currentActivity, 0, intent,
                    0x8000000); //PendingIntent.FLAG_UPDATE_CURRENT = 134217728 [0x8000000]
            AndroidJavaObject alarmManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "alarm");
            AndroidJavaClass system = new AndroidJavaClass("java.lang.System");
            long currentTime = system.CallStatic<long>("currentTimeMillis");
            alarmManager.Call("set", 1, currentTime + 1000, contentIntent); // android.app.AlarmManager.RTC = 1 [0x1]

            Debug.LogError("alarm_manager set time " + currentTime + 1000);
            currentActivity.Call("finish");

            AndroidJavaClass process = new AndroidJavaClass("android.os.Process");
            int pid = process.CallStatic<int>("myPid");
            process.CallStatic("killProcess", pid);
        }
    }

    public static bool HasBadWord(string text)
    {
        var tableData = TableManager.Instance.BadWord.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            string compareValue = tableData[i].Text;
            if (text.IndexOf(compareValue, System.StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return true;
            }
        }

        return false;
    }

    public static string GetOriginNickName(string nickName)
    {
        return nickName.Replace(CommonString.IOS_nick, "");
    }

    public static bool IsBirdNorigae(int idx)
    {
        return idx == 45 || idx == 50;
    }

    public static bool IsPartyTowerMaxFloor(int idx)
    {
        return ServerData.userInfoTable.TableDatas[UserInfoTable.partyTowerFloor].Value ==
               TableManager.Instance.towerTableMulti.dataArray.Length;
    }

    public static class EnumUtil<T>
    {
        public static T Parse(string s)
        {
            return (T)Enum.Parse(typeof(T), s);
        }
    }

    public static float GetDokebiTreasureAddValue()
    {
        return ServerData.goodsTable.GetTableData(GoodsTable.DokebiTreasure).Value * GameBalance.dokebiTreasureAddValue;
    }

    public static string GetGoodsNameByType(Item_Type type)
    {
        switch (type)
        {
            case Item_Type.Marble: return GoodsTable.MarbleKey;
            case Item_Type.PeachReal: return GoodsTable.Peach;
            case Item_Type.RelicTicket: return GoodsTable.RelicTicket;
            case Item_Type.GrowthStone: return GoodsTable.GrowthStone;
            case Item_Type.Ticket: return GoodsTable.Ticket;
        }

        return string.Empty;
    }

    public static void SendFirebaseEvent(string message)
    {
        FirebaseAnalytics.LogEvent(message);
        Debug.LogError($"Firebase log sended : {message}");
    }

    public static bool HasHotTimeEventPass()
    {
        return ServerData.iapServerTable.TableDatas[UiHotTimeEventBuyButton.seasonPassKey].buyCount.Value > 0;
    }
    public static bool HasSnowManEventPass()
    {
        return ServerData.iapServerTable.TableDatas[UiSnowManEventBuyButton.fallPassKey].buyCount.Value > 0;
    }
    public static float GetRandomExcluding(float min, float max, float excludeMin, float excludeMax)
    {
        float randomValue;
        do
        {
            randomValue = Random.Range(min, max);
        }
        while (randomValue >= excludeMin && randomValue <= excludeMax);

        return randomValue;
    }
    public static  string ColorToHexString(Color color)
    {
        // 각 컴포넌트 값을 0부터 255까지의 정수로 변환하여 16진수로 변환 후 합치기
        string hexString = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}",
            Mathf.RoundToInt(color.r * 255),
            Mathf.RoundToInt(color.g * 255),
            Mathf.RoundToInt(color.b * 255),
            Mathf.RoundToInt(color.a * 255));

        return hexString;
    }
    public static  string ColorToHexString(Color color,string str)
    {
        string hexString = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}",
            Mathf.RoundToInt(color.r * 255),
            Mathf.RoundToInt(color.g * 255),
            Mathf.RoundToInt(color.b * 255),
            Mathf.RoundToInt(color.a * 255));

        return $"<color={hexString}>{str}</color>";
    }
    public static bool IsNumberInRange(float a, float b, float c)
    {
        // c가 a와 b 사이에 있는지 확인
        return (c >= Mathf.Min(a, b) && c <= Mathf.Max(a, b));
    }
    public static DateTime GetBackendServerTime()
    {
        BackendReturnObject servertime = Backend.Utils.GetServerTime();
        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        return DateTime.Parse(time).ToUniversalTime().AddHours(9);
    }

    public static TimeSpan GetTimeDifferenceFromKorea()
    {
        // 현재 지역 시간을 가져옵니다.
        DateTime localTime = DateTime.Now;
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
        DateTime utcTime = DateTime.UtcNow;

        TimeZoneInfo koreaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
        
        DateTime koreaTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, koreaTimeZone);
        
        //테스트용 영국
        //TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        //DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, localTimeZone);

        TimeSpan localUtcOffSet = localTimeZone.GetUtcOffset(localTime);
        TimeSpan koreaUtcOffSet = koreaTimeZone.GetUtcOffset(koreaTime);

        // 지역 시간과 한국 시간의 차이를 계산합니다.
        TimeSpan timeDifference = localUtcOffSet-koreaUtcOffSet;
        
        return timeDifference;
    }    
    public static TimeSpan GetTimeRemaining(DateTime targetTime)
    {
        var timeRemaining = targetTime - DateTime.Now;
        return timeRemaining.Add(Utils.GetTimeDifferenceFromKorea());
    }
    
    public static SpecialRequestTableData GetCurrentSeasonSpecialRequestData()
    {
        
        var tableData = TableManager.Instance.SpecialRequestTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var endData = tableData[i].Enddate.Split('-');
            DateTime endDate = new DateTime(int.Parse(endData[0]), int.Parse(endData[1]), int.Parse(endData[2]));
            endDate = endDate.AddDays(1);//5월5일을 넣으면 5월6일00시에끝나야함.
            var result = DateTime.Compare(ServerData.userInfoTable.currentServerTime, endDate);
            
            switch (result)
            {
                //아직 안지남
                case -1 :
                case 0:
                    return tableData[i];
                //지남
                case 1:
                    continue;
                default:
                    continue;
            }    
        }

        return null;

    }
    public static int GetCurrentDimensionSeasonIdx()
    {
        
        var tableData = TableManager.Instance.DimensionSeason.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var endData = tableData[i].Enddate.Split('-');
            DateTime endDate = new DateTime(int.Parse(endData[0]), int.Parse(endData[1]), int.Parse(endData[2]));
            endDate = endDate.AddDays(1);//5월5일을 넣으면 5월6일00시에끝나야함.
            var result = DateTime.Compare(ServerData.userInfoTable.currentServerTime, endDate);
            
            switch (result)
            {
                //아직 안지남
                case -1 :
                case 0:
                    return tableData[i].Id;
                //지남
                case 1:
                    continue;
                default:
                    continue;
            }    
        }

        return -1;

    }
    public static DimensionSeasonData GetCurrentDimensionSeasonData()
    {
        
        var tableData = TableManager.Instance.DimensionSeason.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var endData = tableData[i].Enddate.Split('-');
            DateTime endDate = new DateTime(int.Parse(endData[0]), int.Parse(endData[1]), int.Parse(endData[2]));
            endDate = endDate.AddDays(1);//5월5일을 넣으면 5월6일00시에끝나야함.
            var result = DateTime.Compare(ServerData.userInfoTable.currentServerTime, endDate);
            
            switch (result)
            {
                //아직 안지남
                case -1 :
                case 0:
                    return tableData[i];
                //지남
                case 1:
                    continue;
                default:
                    continue;
            }    
        }

        return null;

    }

    public static bool IsBuyDimensionPass()
    {
        return ServerData.iapServerTable.TableDatas[GetCurrentDimensionSeasonData().Productid].buyCount.Value > 0;
    }

    public static bool IsAnniversaryMission(this EventMissionKey key)
    {
        return key >= EventMissionKey.ANMISSION1 && key <= EventMissionKey.ANMISSION8;
    }
}