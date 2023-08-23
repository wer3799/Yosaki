using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Game.GameInfo;
using LitJson;
using System;
using GooglePlayGames.BasicApi;
using UniRx;


public class GoodsTable
{
    public static string Indate;
    public const string tableName = "Goods";
    public static string Gold = "Gold";
    public static string GoldBar = "GoldBar";
    public static string Jade = "Jade"; //옥
    public static string GrowthStone = "GrowthStone";
    public static string Ticket = "Ticket";
    public static string BonusSpinKey = "BonusSpin";
    public static string MarbleKey = "Marble"; //여우구슬
    public static string DokebiKey = "Dokebi2";
    public static string SkillPartion = "SkillPartion";
    public static string WeaponUpgradeStone = "WeaponUpgradeStone";
    public static string PetUpgradeSoul = "PetUpgradeSoul";
    public static string YomulExchangeStone = "YomulExchangeStone";
    public static string Songpyeon = "Songpyeon";
    public static string TigerStone = "TigerStone";
    public static string RabitStone = "RabitStone";
    public static string DragonStone = "DragonStone";
    public static string SnakeStone = "SnakeStone";
    public static string HorseStone = "HorseStone";
    public static string SheepStone = "SheepStone";
    public static string MonkeyStone = "MonkeyStone";
    public static string CockStone = "CockStone";
    public static string DogStone = "DogStone";
    public static string PigStone = "PigStone";

    //포션
    public static string Potion_0 = "Potion_0";
    public static string Potion_1 = "Potion_1";
    public static string Potion_2 = "Potion_2";

    public static string Relic = "Relic";
    public static string RelicTicket = "RelicTicket";

    //할로윈 도깨비
    public static string Event_Item_0 = "Event_Item_1";
    public static string Event_Item_1 = "Event2";
    public static string Event_Item_SnowMan = "Event4_0"; //어린이날.
    public static string Event_Item_SnowMan_All = "Event4_1"; //어린이날.
    public static string StageRelic = "StageRelic";
    public static string GuimoonRelic = "GuimoonRelic";
    public static string GuimoonRelicClearTicket = "GRCT";
    public static string Peach = "PeachReal";
    public static string MiniGameReward = "MiniGameReward";
    public static string MiniGameReward2 = "mgr2";
    public static string GuildReward = "GuildReward";
    public static string SulItem = "SulItem";
    public static string SmithFire = "SmithFire";
    public static string FeelMulStone = "FeelMulStone";

    public static string Asura0 = "a0";
    public static string Asura1 = "a1";
    public static string Asura2 = "a2";
    public static string Asura3 = "a3";
    public static string Asura4 = "a4";
    public static string Asura5 = "a5";

    public static string Indra0 = "i0";
    public static string Indra1 = "i1";
    public static string Indra2 = "i2";
    public static string IndraPower = "ipw";

    public static string Aduk = "ad";

    public static string SinSkill0 = "s0";
    public static string SinSkill1 = "s1";
    public static string SinSkill2 = "s2";
    public static string SinSkill3 = "s3";
    public static string LeeMuGiStone = "LeeMuGiStone";
    public static string ZangStone = "ZS";
    public static string SwordPartial = "SP";

    public static string Hae_Norigae = "hn";
    public static string Hae_Pet = "hp";
    public static string NataSkill = "NataSkill";
    public static string OrochiSkill = "OrochiSkill";
    public static string GangrimSkill = "GangrimSkill";

    public static string OrochiTooth0 = "or0";
    public static string OrochiTooth1 = "or1";

    public static string gumiho0 = "g0";
    public static string gumiho1 = "g1";
    public static string gumiho2 = "g2";
    public static string gumiho3 = "g3";
    public static string gumiho4 = "g4";
    public static string gumiho5 = "g5";
    public static string gumiho6 = "g6";
    public static string gumiho7 = "g7";
    public static string gumiho8 = "g8";

    public const string Hel = "Hel";
    public static string h0 = "h0";
    public static string h1 = "h1";
    public static string h2 = "h2";
    public static string h3 = "h3";
    public static string h4 = "h4";
    public static string h5 = "h5";
    public static string h6 = "h6";
    public static string h7 = "h7";
    public static string h8 = "h8";
    public static string h9 = "h9";
        
    public static string d0 = "d0";
    public static string d1 = "d1";
    public static string d2 = "d2";
    public static string d3 = "d3";
    public static string d4 = "d4";
    public static string d5 = "d5";
    public static string d6 = "d6";
    public static string d7 = "d7";
    
    public static string Ym = "Ym";

    //두루마리
    public static string du = "du";


    public static string Sun0 = "Sun0";
    public static string Sun1 = "Sun1";
    public static string Sun2 = "Sun2";
    public static string Sun3 = "Sun3";
    public static string Sun4 = "Sun4";

    public static string Chun0 = "Chun0";
    public static string Chun1 = "Chun1";
    public static string Chun2 = "Chun2";
    public static string Chun3 = "Chun3";
    public static string Chun4 = "Chun4";

    public static string DokebiSkill0 = "DokebiSkill0";
    public static string DokebiSkill1 = "DokebiSkill1";
    public static string DokebiSkill2 = "DokebiSkill2";
    public static string DokebiSkill3 = "DokebiSkill3";
    public static string DokebiSkill4 = "DokebiSkill4";


    public static string FourSkill0 = "FS0";
    public static string FourSkill1 = "FS1";
    public static string FourSkill2 = "FS2";
    public static string FourSkill3 = "FS3";

    public static string FourSkill4 = "FS4";
    public static string FourSkill5 = "FS5";
    public static string FourSkill6 = "FS6";
    public static string FourSkill7 = "FS7";
    public static string FourSkill8 = "FS8";

    public static string VisionSkill0 = "VisionSkill0";
    public static string VisionSkill1 = "VisionSkill1";
    public static string VisionSkill2 = "VisionSkill2";
    public static string VisionSkill3 = "VisionSkill3";
    public static string VisionSkill4 = "VisionSkill4";
    public static string VisionSkill5 = "VisionSkill5";
    public static string VisionSkill6 = "VisionSkill6";
    public static string VisionSkill7 = "VisionSkill7";
    public static string VisionSkill8 = "VisionSkill8";
    public static string VisionSkill9 = "VisionSkill9";
    public static string VisionSkill10 = "VisionSkill10";
    public static string VisionSkill11 = "VisionSkill11";
    
    public static string ThiefSkill0 = "ThiefSkill0";
    public static string ThiefSkill1 = "ThiefSkill1";
    public static string ThiefSkill2 = "ThiefSkill2";
    public static string ThiefSkill3 = "ThiefSkill3";
    public static string ThiefSkill4 = "ThiefSkill4";

    public static string DarkSkill0 = "DarkSkill0";
    public static string DarkSkill1 = "DarkSkill1";
    public static string DarkSkill2 = "DarkSkill2";
    public static string DarkSkill3 = "DarkSkill3";
    public static string DarkSkill4 = "DarkSkill4";

    public static string Fw = "Fw";
    public const string Cw = "Cw"; //천계꽃

    public static string c0 = "c0"; //천계꽃
    public static string c1 = "c1"; //천계꽃
    public static string c2 = "c2"; //천계꽃
    public static string c3 = "c3"; //천계꽃
    public static string c4 = "c4"; //천계꽃
    public static string c5 = "c5"; //천계꽃
    public static string c6 = "c6"; //천계꽃

    public static string Event_Collection = "EC0"; //곶감 - > 봄나물로 대체 -> 수박 
    public static string Event_Collection_All = "ECA0"; //봄나물 총 획득량
    public static string Event_HotTime = "Event_HotTime0"; //핫타임 불꽃
    public static string Event_HotTime_Saved = "EHS0"; //핫타임 불꽃
    public static string Event_Fall_Gold = "Event_Fall_Gold"; //황금 곶감
    public static string Event_NewYear = "Event_NewYear"; //떡국
    public static string Event_NewYear_All = "Event_NewYear_All"; //신년재화 총생산량
    public static string Event_Mission_Refund = "Event_Mission1";//발리볼 //꽃송이 //바람개비.
    public static string Event_Mission = "Event_Mission2";//발리볼 //꽃송이 //바람개비.
    public static string Event_Mission_All = "Event_Mission_All2"; //꽃송이 //바람개비 총생산량(소급 위한)
    public static string Event_Mission2 = "Event_Mission3";//보름달
    public static string Event_Mission2_All = "Event_Mission_All3"; //보름달생산량

    public static string FoxMaskPartial = "FoxMaskPartial"; //여우 탈 재화
    public static string SusanoTreasure = "ST"; // 악귀퇴치 재화


    public const string DokebiFire = "DokebiFire"; //도깨비 나라 재화
    public static string DokebiFireKey = "DokebiFireKey"; //도깨비 불 입장권

    public static string Mileage = "Mileage"; //마일리지
    public static string ClearTicket = "ClearTicket"; //마일리지

    public static string HellPowerUp = "HellPowerUp";
    public static string DokebiTreasure = "DT";
    public static string DokebiFireEnhance = "DFE";
    public static string SahyungTreasure = "SahyungTreasure";
    public static string VisionTreasure = "VisionTreasure";
    public static string DarkTreasure = "DarkTreasure";
    public static string SinsunTreasure = "SinsunTreasure";
    public static string GwisalTreasure = "GwisalTreasure";
    public static string ChunguTreasure = "CT";
    public static string SumiFire = "SumiFire";
    public static string SumiFireKey = "SumiFireKey";
    public static string NewGachaEnergy = "NGE";
    public static string DokebiBundle = "DB";
    public static string SinsuRelic = "SinsuRelic";
    public static string HyungsuRelic = "HyungsuRelic";
    public static string ChunguRelic = "ChunguRelic";
    public static string FoxRelic = "FR";
    public static string FoxRelicClearTicket = "FRCT";
    public static string TransClearTicket = "TCT";
    public static string MeditationGoods = "MG";
    public static string MeditationClearTicket = "MCT";
    public static string DaesanGoods = "DG";

    public static string EventDice = "EventDice";
    public static string Event_SA = "EventSA";
    public static string Tresure = "Tresure";
    public static string SuhoPetFeed = "SuhoPetFeed";
    public static string SuhoPetFeedClear = "SPFC";
    public static string SoulRingClear = "SRC";
    public static string SinsuMarble = "SinsuMarble";
    public static string GuildTowerClearTicket = "GTCT";
    public static string GuildTowerHorn = "GuildTowerHorn";
    public static string SealWeaponClear = "SealWeaponClear";
    public static string DosulGoods = "DosulGoods";
    public static string TransGoods = "TransGoods";
    public static string DosulClear = "DosulClear";


    private Dictionary<string, float> tableSchema = new Dictionary<string, float>()
    {
        { Gold, GameBalance.StartingMoney },
        { GoldBar, 0 },
        { Jade, 0f },
        { GrowthStone, 0f },
        { Ticket, 0f },
        { Potion_0, 0f },
        { Potion_1, 0f },
        { Potion_2, 0f },
        { BonusSpinKey, 0f },
        { MarbleKey, 0f },
        { DokebiKey, 0f },
        { SkillPartion, 0f },
        { WeaponUpgradeStone, 0f },
        { PetUpgradeSoul, 0f },
        { YomulExchangeStone, 0f },
        // {Songpyeon,0f},
        { TigerStone, 0f },
        { Relic, 0f },

        { RelicTicket, GameBalance.DailyRelicTicketGetCount },
        { RabitStone, 0f },
        { Event_Item_0, 0f },
        { Event_Item_1, 0f },
        { Event_Item_SnowMan, 0f },
        { Event_Item_SnowMan_All, 0f },
        { DragonStone, 0f },
        { StageRelic, 0f },
        { GuimoonRelic, 0f },
        { GuimoonRelicClearTicket, 0f },
        { SnakeStone, 0f },
        { Peach, 0f },
        { HorseStone, 0f },
        { SheepStone, 0f },
        { MonkeyStone, 0f },
        { MiniGameReward, 0f },
        { MiniGameReward2, 0f },
        { GuildReward, 0f },
        { CockStone, 0f },
        { DogStone, 0f },
        { SulItem, 0f },
        { PigStone, 0f },
        { SmithFire, 0f },
        { FeelMulStone, 0f },

        { Asura0, 0f },
        { Asura1, 0f },
        { Asura2, 0f },
        { Asura3, 0f },
        { Asura4, 0f },
        { Asura5, 0f },
        { Aduk, 0f },

        { SinSkill0, 0f },
        { SinSkill1, 0f },
        { SinSkill2, 0f },
        { SinSkill3, 0f },
        { LeeMuGiStone, 0f },
        { ZangStone, 0f },
        { SwordPartial, 0f },

        { Hae_Norigae, 0f },
        { Hae_Pet, 0f },

        { Indra0, 0f },
        { Indra1, 0f },
        { Indra2, 0f },
        { IndraPower, 0f },
        { NataSkill, 0f },
        { OrochiSkill, 0f },
        { GangrimSkill, 0f },
        { OrochiTooth0, 0f },
        { OrochiTooth1, 0f },
        //
        { gumiho0, 0f },
        { gumiho1, 0f },
        { gumiho2, 0f },
        { gumiho3, 0f },
        { gumiho4, 0f },
        { gumiho5, 0f },
        { gumiho6, 0f },
        { gumiho7, 0f },
        { gumiho8, 0f },
        //
        { Hel, 0f },
        { h0, 0f },
        { h1, 0f },
        { h2, 0f },
        { h3, 0f },
        { h4, 0f },
        { h5, 0f },
        { h6, 0f },
        { h7, 0f },
        { h8, 0f },
        { h9, 0f },
        { Ym, 0f },
        { du, 0f },

        
        { d0, 0f },
        { d1, 0f },
        { d2, 0f },
        { d3, 0f },
        { d4, 0f },
        { d5, 0f },
        { d6, 0f },
        { d7, 0f },
        
        { Sun0, 0f },
        { Sun1, 0f },
        { Sun2, 0f },
        { Sun3, 0f },
        { Sun4, 0f },

        { Chun0, 0f },
        { Chun1, 0f },
        { Chun2, 0f },
        { Chun3, 0f },
        { Chun4, 0f },

        { DokebiSkill0, 0f },
        { DokebiSkill1, 0f },
        { DokebiSkill2, 0f },
        { DokebiSkill3, 0f },
        { DokebiSkill4, 0f },

        { FourSkill0, 0f },
        { FourSkill1, 0f },
        { FourSkill2, 0f },
        { FourSkill3, 0f },

        { FourSkill4, 0f },
        { FourSkill5, 0f },
        { FourSkill6, 0f },
        { FourSkill7, 0f },
        { FourSkill8, 0f },

        { VisionSkill0, 0f },
        { VisionSkill1, 0f },
        { VisionSkill2, 0f },
        { VisionSkill3, 0f },
        { VisionSkill4, 0f },
        { VisionSkill5, 0f },
        { VisionSkill6, 0f },
        { VisionSkill7, 0f },
        
        { VisionSkill8, 0f },
        { VisionSkill9, 0f },
        { VisionSkill10, 0f },
        { VisionSkill11, 0f },

        { ThiefSkill0, 0f },
        { ThiefSkill1, 0f },
        { ThiefSkill2, 0f },
        { ThiefSkill3, 0f },
        { ThiefSkill4, 0f },

        { DarkSkill0, 0f },
        { DarkSkill1, 0f },
        { DarkSkill2, 0f },
        { DarkSkill3, 0f },
        { DarkSkill4, 0f },

        { Fw, 0f },
        { Cw, 0f },
        //

        { c0, 0f },
        { c1, 0f },
        { c2, 0f },
        { c3, 0f },
        { c4, 0f },
        { c5, 0f },
        { c6, 0f },

        { Event_Collection, 0f },
        { Event_HotTime, 0f },
        { Event_HotTime_Saved, 0f },
        { Event_Collection_All, 0f },
        { Event_Fall_Gold, 0f },
        { Event_NewYear, 0f },
        { Event_NewYear_All, 0f },
        { Event_Mission, 0f },
        { Event_Mission_All, 0f },
        { Event_Mission2, 0f },
        { Event_Mission2_All, 0f },
        { Event_Mission_Refund, 0f },
        { FoxMaskPartial, 0f },

        { DokebiFire, 0f },
        { DokebiFireKey, 0f },
        { DokebiFireEnhance, 0f },

        { Mileage, 0f },
        { ClearTicket, 0f },
        { HellPowerUp, 0f },
        { DokebiTreasure, 0f },
        { SahyungTreasure, 0f },
        { VisionTreasure, 0f },
        { DarkTreasure, 0f },
        { SinsunTreasure, 0f },
        { GwisalTreasure, 0f },
        { ChunguTreasure, 0f },
        { SusanoTreasure, 0f },
        { SumiFire, 0f },
        { SumiFireKey, 0f },
        { NewGachaEnergy, 0f },
        { DokebiBundle, 0f },
        { SinsuRelic, 0f },
        { HyungsuRelic, 0f },
        { ChunguRelic, 0f },
        { FoxRelic, 0f },
        { FoxRelicClearTicket, 0f },
        { TransClearTicket, 0f },
        { MeditationGoods, 0f },
        { MeditationClearTicket, 0f },
        { DaesanGoods, 0f },

        { EventDice, 0f },
        { Event_SA, 0f },
        { Tresure, 0f },
        { SuhoPetFeed, 0f },
        { SuhoPetFeedClear, 0f },
        { SoulRingClear, 0f },
        { SinsuMarble, 0f },
        { GuildTowerClearTicket, 0f },
        { GuildTowerHorn, 0f },
        { SealWeaponClear, 0f },
        { DosulGoods, 0f },
        { TransGoods, 0f },
        { DosulClear, 0f },
    };

    private ReactiveDictionary<string, ReactiveProperty<float>> tableDatas = new ReactiveDictionary<string, ReactiveProperty<float>>();
    public ReactiveDictionary<string, ReactiveProperty<float>> TableDatas => tableDatas;

    public static string GetPosionKey(int idx)
    {
        if (idx == 0)
        {
            return Potion_0;
        }
        else if (idx == 1)
        {
            return Potion_1;
        }
        else
        {
            return Potion_2;
        }
    }

    public ReactiveProperty<float> GetTableData(string key)
    {
        return tableDatas[key];
    }

    public float GetCurrentGoods(string key)
    {
        return tableDatas[key].Value;
    }

    public void GetGold(float amount)
    {
        tableDatas[Gold].Value += amount;
    }
    public void GetGoldBar(float amount)
    {
        tableDatas[GoldBar].Value += amount;
    }

    static int growThStoneAddAmount = 0;
    static float updateRequireNum_GrowthStone_0 = 10000; //1000억 미만
    static float updateRequireNum_GrowthStone_1 = 100000; //조 미만
    static float updateRequireNum_GrowthStone_2 = 1000000; //조 이상
    static float updateRequireNum_GrowthStone_3 = 10000000; // 10조이상

    public float FloatGetUpdateRequireGrowthStone()
    {
        //10조이상
        if (tableDatas[GrowthStone].Value >= 10000000000000f)
        {
            return updateRequireNum_GrowthStone_3;
        }

        //1조 이상
        if (tableDatas[GrowthStone].Value >= 1000000000000f)
        {
            return updateRequireNum_GrowthStone_2;
        }

        //1000억 이상
        if (tableDatas[GrowthStone].Value >= 100000000000f)
        {
            return updateRequireNum_GrowthStone_1;
        }

        return updateRequireNum_GrowthStone_0;
    }

    public void GetMagicStone(float amount)
    {
        amount += PlayerStats.GetSmithValue(StatusType.growthStoneUp);

        float magicStonePlusValue = PlayerStats.GetMagicStonePlusValue();

        int amount_int = (int)(amount + amount * magicStonePlusValue);

        if (SystemMessage.IsMessageQueueFull() == false)
            SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.GrowthStone)} 획득(+{amount_int})");

        growThStoneAddAmount += amount_int;

        if (growThStoneAddAmount < FloatGetUpdateRequireGrowthStone())
        {
        }
        else
        {
            tableDatas[GrowthStone].Value += growThStoneAddAmount;
            growThStoneAddAmount = 0;
        }
    }

    static int marbleAddAmount = 0;
    static float updateRequireNum = 100;

    public void GetMarble(float amount)
    {
        float magicMarblePlusValue = PlayerStats.GetMarblePlusValue();

        int amount_int = (int)(amount + amount * magicMarblePlusValue);

        if (SystemMessage.IsMessageQueueFull() == false)
            SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.Marble)} 획득(+{amount_int})");

        marbleAddAmount += amount_int;

        if (marbleAddAmount < updateRequireNum)
        {
        }
        else
        {
            tableDatas[MarbleKey].Value += marbleAddAmount;
            marbleAddAmount = 0;
        }
    }

    static int soulAddAmount = 0;

    public void GetPetUpgradeSoul(float amount)
    {
        if (SystemMessage.IsMessageQueueFull() == false)
            SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.PetUpgradeSoul)} 획득(+{(int)amount})");

        soulAddAmount += (int)amount;

        if (soulAddAmount < updateRequireNum)
        {
        }
        else
        {
            tableDatas[PetUpgradeSoul].Value += soulAddAmount;
            soulAddAmount = 0;
        }
    }

    static int eventItemAddNum = 0;

    public void GetEventItem(float amount)
    {
        eventItemAddNum += (int)amount;

        if (eventItemAddNum < updateRequireNum)
        {
        }
        else
        {
            tableDatas[Event_Item_SnowMan].Value += eventItemAddNum;
            
            if (Utils.HasSnowManEventPass() == false)
            {
                tableDatas[Event_Item_SnowMan_All].Value += eventItemAddNum;
            }
            else
            {
                tableDatas[Event_Item_SnowMan].Value += eventItemAddNum;
            }
            eventItemAddNum = 0;
        }
    }

    static float peachItemAddNum = 0;

    public void GetPeachItem(float amount)
    {
        peachItemAddNum += amount * (1 + PlayerStats.GetPeachGainValue());

        //100킬마다 얻게하기 위해서
        if (peachItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Peachamount, 1))
        {
        }
        else
        {
            tableDatas[Peach].Value += (int)peachItemAddNum;
            peachItemAddNum -= (int)peachItemAddNum;
        }
    }

    //
    static float helItemAddNum = 0;

    public void GetHelItem(float amount)
    {
        helItemAddNum += amount * (1 + PlayerStats.GetHellGainValue());

        //1개 획득할때마다 얻게하기 위해서
        if (helItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Helamount, 1))
        {
        }
        else
        {
            tableDatas[Hel].Value += (int)helItemAddNum;
            helItemAddNum -= (int)helItemAddNum;
        }
    }

    //
    //
    static float chunItemAddNum = 0;

    public void GetChunItem(float amount)
    {
        chunItemAddNum += amount * (1 + PlayerStats.GetChunGainValue());

        //1개 획득할때마다 얻게하기 위해서
        if (chunItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Chunfloweramount, 1))
        {
        }
        else
        {
            tableDatas[Cw].Value += (int)chunItemAddNum;
            chunItemAddNum -= (int)chunItemAddNum;
        }
    }
    
    static float dokebiItemAddNum = 0;
    public void GetDokebiItem(float amount)
    {
        dokebiItemAddNum += amount * (1 + PlayerStats.GetDokebiFireGainValue());

        //1개 획득할때마다 얻게하기 위해서
        if (dokebiItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Dokebifireamount, 1))
        {
        }
        else
        {
            tableDatas[DokebiFire].Value += (int)dokebiItemAddNum;
            dokebiItemAddNum -= (int)dokebiItemAddNum;
        }
    }
    //

    public int GetFourSkillHasCount()
    {
        int fourLevel = 0;

        if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill0).Value == 1)
        {
            fourLevel++;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill1).Value == 1)
        {
            fourLevel++;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill2).Value == 1)
        {
            fourLevel++;
        }

        if (ServerData.goodsTable.GetTableData(GoodsTable.FourSkill3).Value == 1)
        {
            fourLevel++;
        }

        return fourLevel;
    }

    //보유비전스킬개수
    public int GetVisionSkillIdx()
    {
        var visionSkillIdxList = PlayerSkillCaster.Instance.GetVisionSkillIdxList();

        var idx = 0;

        for (int i = 0; i < visionSkillIdxList.Count; i++)
        {
            if (ServerData.goodsTable.GetTableData(TableManager.Instance.SkillTable.dataArray[visionSkillIdxList[i]].Skillclassname).Value >0 )
            {
                idx = visionSkillIdxList[i];
            }
            // else
            // {
            //     break;
            // }
        }
        return idx;
    }

    static int eventItemAddNum_Spring = 0;

    public void GetSpringEventItem(float amount)
    {
        if (SystemMessage.IsMessageQueueFull() == false)
            SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.Event_Collection)} 획득(+{(int)amount})");

        eventItemAddNum_Spring += (int)amount;

        if (eventItemAddNum_Spring < updateRequireNum)
        {
        }
        else
        {
            //tableDatas[Event_Item_1].Value += eventItemAddNum_Spring;
            tableDatas[Event_Collection].Value += eventItemAddNum_Spring;
            //총획득량
            if (ServerData.iapServerTable.TableDatas[UiCollectionPass0BuyButton.PassKey].buyCount.Value == 0)
            {
                tableDatas[Event_Collection_All].Value += eventItemAddNum_Spring;
            }
            else
            {
                tableDatas[Event_Collection].Value += eventItemAddNum_Spring;
            }

            eventItemAddNum_Spring = 0;
        }
    }

    //

    static int sulAddNum = 0;

    public void GetsulItem(float amount)
    {
        if (SystemMessage.IsMessageQueueFull() == false)
            SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.SulItem)} 획득(+{(int)amount})");

        sulAddNum += (int)amount;

        if (sulAddNum < updateRequireNum)
        {
        }
        else
        {
            tableDatas[SulItem].Value += sulAddNum;
            sulAddNum = 0;
        }
    }

    static int stageRelicAddNum = 0;

    public void GetStageRelic(float amount)
    {
        if (SystemMessage.IsMessageQueueFull() == false)
            SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.StageRelic)} 획득(+{(int)amount})");

        stageRelicAddNum += (int)amount;

        if (stageRelicAddNum < updateRequireNum)
        {
        }
        else
        {
            tableDatas[StageRelic].Value += stageRelicAddNum;
            stageRelicAddNum = 0;
        }
    }

    public void Initialize()
    {
        tableDatas.Clear();

        SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, new Where(), callback =>
        {
            // 이후 처리
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("LoadStatusFailed");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, CommonString.DataLoadFailedRetry, Initialize);
                return;
            }

            var rows = callback.Rows();

            //맨처음 초기화
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                var e = tableSchema.GetEnumerator();

                while (e.MoveNext())
                {
                    defultValues.Add(e.Current.Key, e.Current.Value);
                    tableDatas.Add(e.Current.Key, new ReactiveProperty<float>(e.Current.Value));
                }

                var bro = Backend.GameData.Insert(tableName, defultValues);

                if (bro.IsSuccess() == false)
                {
                    // 이후 처리
                    ServerData.ShowCommonErrorPopup(bro, Initialize);
                    return;
                }
                else
                {
                    var jsonData = bro.GetReturnValuetoJSON();
                    if (jsonData.Keys.Count > 0)
                    {
                        Indate = jsonData[0].ToString();
                    }

                    // data.
                    // statusIndate = data[DatabaseManager.inDate_str][DatabaseManager.format_string].ToString();
                }

                return;
            }
            //나중에 칼럼 추가됐을때 업데이트
            else
            {
                Param defultValues = new Param();
                int paramCount = 0;

                JsonData data = rows[0];

                if (data.Keys.Contains(ServerData.inDate_str))
                {
                    Indate = data[ServerData.inDate_str][ServerData.format_string].ToString();
                }

                var e = tableSchema.GetEnumerator();

                for (int i = 0; i < data.Keys.Count; i++)
                {
                    while (e.MoveNext())
                    {
                        if (data.Keys.Contains(e.Current.Key))
                        {
                            //값로드
                            var value = data[e.Current.Key][ServerData.format_Number].ToString();
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<float>(float.Parse(value)));
                        }
                        else
                        {
                            defultValues.Add(e.Current.Key, e.Current.Value);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<float>(e.Current.Value));
                            paramCount++;
                        }
                    }
                }

                if (paramCount != 0)
                {
                    var bro = Backend.GameData.Update(tableName, Indate, defultValues);

                    if (bro.IsSuccess() == false)
                    {
                        ServerData.ShowCommonErrorPopup(bro, Initialize);
                        return;
                    }
                }
            }
        });
    }

    public void AddLocalData(string key, float amount)
    {
        tableDatas[key].Value += amount;
    }

    public void UpData(string key, bool LocalOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Status {key} is not exist");
            return;
        }

        UpData(key, tableDatas[key].Value, LocalOnly);
    }

    public void UpData(string key, float data, bool LocalOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Growth {key} is not exist");
            return;
        }

        tableDatas[key].Value = data;

        if (LocalOnly == false)
        {
            SyncToServerEach(key);
        }
    }
    public void UpDataV2(string key, bool LocalOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Status {key} is not exist");
            return;
        }

        UpDataV2(key, tableDatas[key].Value, LocalOnly);
    }

    public void UpDataV2(string key, float data, bool LocalOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Growth {key} is not exist");
            return;
        }

        tableDatas[key].Value = data;

        if (LocalOnly == false)
        {
            SyncToServerEachV2(key);
        }
    }

    public void SyncToServerEach(string key, Action whenSyncSuccess = null, Action whenRequestComplete = null, Action whenRequestFailed = null)
    {
        Param param = new Param();
        param.Add(key, tableDatas[key].Value);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            whenRequestComplete?.Invoke();
            if (e.IsSuccess())
            {
                whenSyncSuccess?.Invoke();
            }
            else if (e.IsSuccess() == false)
            {
                if (whenRequestFailed != null)
                {
                    whenRequestFailed.Invoke();
                }

                Debug.Log($"Growth {key} sync failed");
                return;
            }
        });
    }
    public void SyncToServerEachV2(string key, Action whenSyncSuccess = null, Action whenRequestComplete = null, Action whenRequestFailed = null)
    {
        Param param = new Param();
        param.Add(key, tableDatas[key].Value);

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
        {
            whenRequestComplete?.Invoke();
#if UNITY_EDITOR
            Debug.LogError($"테이블 : {tableName} / 키 : {key} / 수량 : {tableDatas[key].Value}");
#endif
            if (e.IsSuccess())
            {
                whenSyncSuccess?.Invoke();
            }
            else if (e.IsSuccess() == false)
            {
                if (whenRequestFailed != null)
                {
                    whenRequestFailed.Invoke();
                }

                Debug.Log($"Growth {key} sync failed");
                return;
            }
        });
    }

    public void SyncAllData(List<string> ignoreList = null)
    {
        Param param = new Param();

        var e = tableDatas.GetEnumerator();
        while (e.MoveNext())
        {
            if (ignoreList != null && ignoreList.Contains(e.Current.Key) == true) continue;
            param.Add(e.Current.Key, e.Current.Value.Value);
        }

        SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, bro =>
        {
#if UNITY_EDITOR
            if (bro.IsSuccess() == false)
            {
                Debug.Log($"SyncAllData {tableName} up failed");
                return;
            }
            else
            {
                Debug.Log($"SyncAllData {tableName} up Complete");
                return;
            }
#endif
        });
    }

    public void SyncAllDataForce()
    {
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Gold, ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value);
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
        goodsParam.Add(GoodsTable.PetUpgradeSoul, ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value);
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).Value > 0)
        {
            goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
        }

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).Value > 0)
        {
            goodsParam.Add(GoodsTable.Hel, ServerData.goodsTable.GetTableData(GoodsTable.Hel).Value);
        }

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).Value > 0)
        {
            goodsParam.Add(GoodsTable.Cw, ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value);
        }
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).Value > 0)
        {
            goodsParam.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);
        }
        //if (ServerData.userInfoTable.CanSpawnEventItem())
        //{
        //    goodsParam.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);
        //}

        //goodsParam.Add(GoodsTable.Event_Item_1, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value);

        if (ServerData.userInfoTable.CanSpawnSpringEventItem())
        {
            goodsParam.Add(GoodsTable.Event_Collection, ServerData.goodsTable.GetTableData(GoodsTable.Event_Collection).Value);
            if (ServerData.iapServerTable.TableDatas[UiCollectionPass0BuyButton.PassKey].buyCount.Value == 0)
            {
                goodsParam.Add(GoodsTable.Event_Collection_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_Collection_All).Value);
            }
        }

        if (ServerData.userInfoTable.CanSpawnSnowManItem())
        {
            goodsParam.Add(GoodsTable.Event_Item_SnowMan, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_SnowMan).Value);
          
            if (Utils.HasSnowManEventPass() == false)
            {
                goodsParam.Add(GoodsTable.Event_Item_SnowMan_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_SnowMan_All).Value);
            }
        }

        goodsParam.Add(GoodsTable.Event_NewYear, ServerData.goodsTable.GetTableData(GoodsTable.Event_NewYear).Value);

        goodsParam.Add(GoodsTable.SulItem, ServerData.goodsTable.GetTableData(GoodsTable.SulItem).Value);
        goodsParam.Add(GoodsTable.StageRelic, ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value);
        goodsParam.Add(GoodsTable.BonusSpinKey, ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.sleepRewardSavedTime, ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value);

        //경험치
        Param statusParam = new Param();
        //레벨
        statusParam.Add(StatusTable.Level, ServerData.statusTable.GetTableData(StatusTable.Level).Value);

        //스킬포인트
        statusParam.Add(StatusTable.SkillPoint, ServerData.statusTable.GetTableData(StatusTable.SkillPoint).Value);

        //스탯포인트
        statusParam.Add(StatusTable.StatPoint, ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value);

        Param growthParam = new Param();
        growthParam.Add(GrowthTable.Exp, ServerData.growthTable.GetTableData(GrowthTable.Exp).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        transactions.Add(TransactionValue.SetUpdate(GrowthTable.tableName, GrowthTable.Indate, growthParam));

        var bro = Backend.GameData.TransactionWrite(transactions);
    }

    public ReactiveProperty<float> GetTableData(Item_Type key)
    {
        string stringKey = ItemTypeToServerString(key);
        return tableDatas[stringKey];
    }

    public string ItemTypeToServerString(Item_Type type)
    {
        switch (type)
        {
            case Item_Type.Gold:
            {
                return GoodsTable.Gold;
            }
            case Item_Type.GoldBar:
            {
                return GoodsTable.GoldBar;
            }
            case Item_Type.Jade:
            {
                return GoodsTable.Jade;
            }
            case Item_Type.GrowthStone:
            {
                return GoodsTable.GrowthStone;
            }
            case Item_Type.Ticket:
            {
                return GoodsTable.Ticket;
            }

            case Item_Type.Marble:
            {
                return GoodsTable.MarbleKey;
            }

            case Item_Type.Songpyeon:
            {
                return GoodsTable.Songpyeon;
            }

            case Item_Type.RelicTicket:
            {
                return GoodsTable.RelicTicket;
            }

            case Item_Type.Event_Item_0:
            {
                return GoodsTable.Event_Item_0;
            }


            case Item_Type.Event_Item_1:
            {
                return GoodsTable.Event_Item_1;
            }

            case Item_Type.Event_Item_SnowMan:
            {
                return GoodsTable.Event_Item_SnowMan;
            }

            case Item_Type.Event_Item_SnowMan_All:
            {
                return GoodsTable.Event_Item_SnowMan_All;
            }


            case Item_Type.SulItem:
            {
                return GoodsTable.SulItem;
            }


            case Item_Type.FeelMulStone:
            {
                return GoodsTable.FeelMulStone;
            }


            case Item_Type.Asura0:
            {
                return GoodsTable.Asura0;
            }


            case Item_Type.Asura1:
            {
                return GoodsTable.Asura1;
            }


            case Item_Type.Asura2:
            {
                return GoodsTable.Asura2;
            }


            case Item_Type.Asura3:
            {
                return GoodsTable.Asura3;
            }

            case Item_Type.Asura4:
            {
                return GoodsTable.Asura4;
            }


            case Item_Type.Asura5:
            {
                return GoodsTable.Asura5;
            }

            case Item_Type.Aduk:
            {
                return GoodsTable.Aduk;
            }


            case Item_Type.LeeMuGiStone:
            {
                return GoodsTable.LeeMuGiStone;
            }


            //
            case Item_Type.SinSkill0:
            {
                return GoodsTable.SinSkill0;
            }

            case Item_Type.SinSkill1:
            {
                return GoodsTable.SinSkill1;
            }

            case Item_Type.SinSkill2:
            {
                return GoodsTable.SinSkill2;
            }

            case Item_Type.SinSkill3:
            {
                return GoodsTable.SinSkill3;
            }

            case Item_Type.NataSkill:
            {
                return GoodsTable.NataSkill;
            }

            case Item_Type.OrochiSkill:
            {
                return GoodsTable.OrochiSkill;
            }

            //
            case Item_Type.Sun0:
            {
                return GoodsTable.Sun0;
            }

            case Item_Type.Sun1:
            {
                return GoodsTable.Sun1;
            }

            case Item_Type.Sun2:
            {
                return GoodsTable.Sun2;
            }

            case Item_Type.Sun3:
            {
                return GoodsTable.Sun3;
            }

            case Item_Type.Sun4:
            {
                return GoodsTable.Sun4;
            }

            //
            case Item_Type.Chun0:
            {
                return GoodsTable.Chun0;
            }

            case Item_Type.Chun1:
            {
                return GoodsTable.Chun1;
            }

            case Item_Type.Chun2:
            {
                return GoodsTable.Chun2;
            }

            case Item_Type.Chun3:
            {
                return GoodsTable.Chun3;
            }

            case Item_Type.Chun4:
            {
                return GoodsTable.Chun4;
            }

            case Item_Type.DokebiSkill0:
            {
                return GoodsTable.DokebiSkill0;
            }

            case Item_Type.DokebiSkill1:
            {
                return GoodsTable.DokebiSkill1;
            }

            case Item_Type.DokebiSkill2:
            {
                return GoodsTable.DokebiSkill2;
            }

            case Item_Type.DokebiSkill3:
            {
                return GoodsTable.DokebiSkill3;
            }

            case Item_Type.DokebiSkill4:
            {
                return GoodsTable.DokebiSkill4;
            }

            //            //
            case Item_Type.FourSkill0:
            {
                return GoodsTable.FourSkill0;
            }

            case Item_Type.FourSkill1:
            {
                return GoodsTable.FourSkill1;
            }

            case Item_Type.FourSkill2:
            {
                return GoodsTable.FourSkill2;
            }

            case Item_Type.FourSkill3:
            {
                return GoodsTable.FourSkill3;
            }

            //   //
            case Item_Type.FourSkill4:
            {
                return GoodsTable.FourSkill4;
            }

            case Item_Type.FourSkill5:
            {
                return GoodsTable.FourSkill5;
            }

            case Item_Type.FourSkill6:
            {
                return GoodsTable.FourSkill6;
            }

            case Item_Type.FourSkill7:
            {
                return GoodsTable.FourSkill7;
            }
            case Item_Type.FourSkill8:
            {
                return GoodsTable.FourSkill8;
            }

            //            //
            case Item_Type.VisionSkill0:
            {
                return GoodsTable.VisionSkill0;
            }

            case Item_Type.VisionSkill1:
            {
                return GoodsTable.VisionSkill1;
            }

            case Item_Type.VisionSkill2:
            {
                return GoodsTable.VisionSkill2;
            }

            case Item_Type.VisionSkill3:
            {
                return GoodsTable.VisionSkill3;
            }
            case Item_Type.VisionSkill4:
            {
                return GoodsTable.VisionSkill4;
            }
            case Item_Type.VisionSkill5:
            {
                return GoodsTable.VisionSkill5;
            }
            case Item_Type.VisionSkill6:
            {
                return GoodsTable.VisionSkill6;
            }
            case Item_Type.VisionSkill7:
            {
                return GoodsTable.VisionSkill7;
            }
            case Item_Type.VisionSkill8:
            {
                return GoodsTable.VisionSkill8;
            }
            case Item_Type.VisionSkill9:
            {
                return GoodsTable.VisionSkill9;
            }
            case Item_Type.VisionSkill10:
            {
                return GoodsTable.VisionSkill10;
            }
            case Item_Type.VisionSkill11:
            {
                return GoodsTable.VisionSkill11;
            }

            //
            //            //
            case Item_Type.ThiefSkill0:
            {
                return GoodsTable.ThiefSkill0;
            }

            case Item_Type.ThiefSkill1:
            {
                return GoodsTable.ThiefSkill1;
            }

            case Item_Type.ThiefSkill2:
            {
                return GoodsTable.ThiefSkill2;
            }

            case Item_Type.ThiefSkill3:
            {
                return GoodsTable.ThiefSkill3;
            }

            case Item_Type.ThiefSkill4:
            {
                return GoodsTable.ThiefSkill4;
            }

            //
            //            //
            case Item_Type.DarkSkill0:
            {
                return GoodsTable.DarkSkill0;
            }

            case Item_Type.DarkSkill1:
            {
                return GoodsTable.DarkSkill1;
            }

            case Item_Type.DarkSkill2:
            {
                return GoodsTable.DarkSkill2;
            }

            case Item_Type.DarkSkill3:
            {
                return GoodsTable.DarkSkill3;
            }

            case Item_Type.DarkSkill4:
            {
                return GoodsTable.DarkSkill4;
            }

            //
            case Item_Type.GangrimSkill:
            {
                return GoodsTable.GangrimSkill;
            }

            //

            case Item_Type.SmithFire:
            {
                return GoodsTable.SmithFire;
            }


            case Item_Type.StageRelic:
            {
                return GoodsTable.StageRelic;
            }

            case Item_Type.GuimoonRelic:
            {
                return GoodsTable.GuimoonRelic;
            }

            case Item_Type.GuimoonRelicClearTicket:
            {
                return GoodsTable.GuimoonRelicClearTicket;
            }


            case Item_Type.PeachReal:
            {
                return GoodsTable.Peach;
            }

            case Item_Type.GuildReward:
            {
                return GoodsTable.GuildReward;
            }

            case Item_Type.SP:
            {
                return GoodsTable.SwordPartial;
            }

            case Item_Type.Hel:
            {
                return GoodsTable.Hel;
            }

            case Item_Type.Ym:
            {
                return GoodsTable.Ym;
            }

            case Item_Type.Fw:
            {
                return GoodsTable.Fw;
            }


            case Item_Type.Cw:
            {
                return GoodsTable.Cw;
            }

            case Item_Type.SuhoPetFeed:
            {
                return GoodsTable.SuhoPetFeed;
            }
            case Item_Type.SuhoPetFeedClear:
            {
                return GoodsTable.SuhoPetFeedClear;
            }
            case Item_Type.SoulRingClear:
            {
                return GoodsTable.SoulRingClear;
            }

            case Item_Type.SumiFire:
            {
                return GoodsTable.SumiFire;
            }
            
            case Item_Type.SealWeaponClear:
            {
                return GoodsTable.SealWeaponClear;
            }
            case Item_Type.SinsuRelic:
            {
                return GoodsTable.SinsuRelic;
            }
            case Item_Type.HyungsuRelic:
            {
                return GoodsTable.HyungsuRelic;
            }
            case Item_Type.ChunguRelic:
            {
                return GoodsTable.ChunguRelic;
            }
            case Item_Type.FoxRelic:
            {
                return GoodsTable.FoxRelic;
            }

            case Item_Type.FoxRelicClearTicket:
            {
                return GoodsTable.FoxRelicClearTicket;
            }
            case Item_Type.TransClearTicket:
            {
                return GoodsTable.TransClearTicket;
            }
            case Item_Type.MeditationGoods:
            {
                return GoodsTable.MeditationGoods;
            }
            case Item_Type.MeditationClearTicket:
            {
                return GoodsTable.MeditationClearTicket;
            }
            case Item_Type.DaesanGoods:
            {
                return GoodsTable.DaesanGoods;
            }

            case Item_Type.EventDice:
            {
                return GoodsTable.EventDice;
            }

            case Item_Type.Event_SA:
            {
                return GoodsTable.Event_SA;
            }

            case Item_Type.NewGachaEnergy:
            {
                return GoodsTable.NewGachaEnergy;
            }

            case Item_Type.DokebiBundle:
            {
                return GoodsTable.DokebiBundle;
            }
            case Item_Type.Event_Mission:
            {
                return GoodsTable.Event_Mission;
            }
            case Item_Type.Event_Mission2:
            {
                return GoodsTable.Event_Mission2;
            }

            case Item_Type.DokebiFireKey:
            {
                return GoodsTable.DokebiFireKey;
            }

            case Item_Type.SumiFireKey:
            {
                return GoodsTable.SumiFireKey;
            }
            case Item_Type.Event_Collection:
            {
                return GoodsTable.Event_Collection;
            }
            case Item_Type.Event_HotTime:
            {
                return GoodsTable.Event_HotTime;
            }
            case Item_Type.Event_Collection_All:
            {
                return GoodsTable.Event_Collection_All;
            }
            case Item_Type.Event_Fall_Gold:
            {
                return GoodsTable.Event_Fall_Gold;
            }
            //
            case Item_Type.WeaponUpgradeStone:
            {
                return GoodsTable.WeaponUpgradeStone;
            }
            case Item_Type.YomulExchangeStone:
            {
                return GoodsTable.YomulExchangeStone;
            }
            case Item_Type.TigerBossStone:
            {
                return GoodsTable.TigerStone;
            }
            case Item_Type.RabitBossStone:
            {
                return GoodsTable.RabitStone;
            }
            case Item_Type.DragonBossStone:
            {
                return GoodsTable.DragonStone;
            }
            case Item_Type.SnakeStone:
            {
                return GoodsTable.SnakeStone;
            }
            case Item_Type.HorseStone:
            {
                return GoodsTable.HorseStone;
            }
            case Item_Type.SheepStone:
            {
                return GoodsTable.SheepStone;
            }
            case Item_Type.MonkeyStone:
            {
                return GoodsTable.MonkeyStone;
            }
            case Item_Type.CockStone:
            {
                return GoodsTable.CockStone;
            }
            case Item_Type.DogStone:
            {
                return GoodsTable.DogStone;
            }
            case Item_Type.PigStone:
            {
                return GoodsTable.PigStone;
            }

            //
            case Item_Type.gumiho0:
            {
                return GoodsTable.gumiho0;
            }
            case Item_Type.gumiho1:
            {
                return GoodsTable.gumiho1;
            }
            case Item_Type.gumiho2:
            {
                return GoodsTable.gumiho2;
            }
            case Item_Type.gumiho3:
            {
                return GoodsTable.gumiho3;
            }
            case Item_Type.gumiho4:
            {
                return GoodsTable.gumiho4;
            }
            case Item_Type.gumiho5:
            {
                return GoodsTable.gumiho5;
            }
            case Item_Type.gumiho6:
            {
                return GoodsTable.gumiho6;
            }
            case Item_Type.gumiho7:
            {
                return GoodsTable.gumiho7;
            }
            case Item_Type.gumiho8:
            {
                return GoodsTable.gumiho8;
            }
            //
            case Item_Type.Indra0:
            {
                return GoodsTable.Indra0;
            }

            case Item_Type.Indra1:
            {
                return GoodsTable.Indra1;
            }

            case Item_Type.Indra2:
            {
                return GoodsTable.Indra2;
            }

            case Item_Type.IndraPower:
            {
                return GoodsTable.IndraPower;
            }


            default:
                return type.ToString();
        }
    }

    public Item_Type ServerStringToItemType(string type)
    {
        if (GoodsTable.Gold == type)
        {
            return Item_Type.Gold;
        }
        if (GoodsTable.GoldBar == type)
        {
            return Item_Type.GoldBar;
        }
        else if (GoodsTable.Jade == type)
        {
            return Item_Type.Jade;
        }
        else if (GoodsTable.GrowthStone == type)
        {
            return Item_Type.GrowthStone;
        }
        else if (GoodsTable.Ticket == type)
        {
            return Item_Type.Ticket;
        }

        else if (GoodsTable.MarbleKey == type)
        {
            return Item_Type.Marble;
        }

        else if (GoodsTable.Songpyeon == type)
        {
            return Item_Type.Songpyeon;
        }

        else if (GoodsTable.RelicTicket == type)
        {
            return Item_Type.RelicTicket;
        }

        else if (GoodsTable.Event_Item_0 == type)
        {
            return Item_Type.Event_Item_0;
        }


        else if (GoodsTable.Event_Item_1 == type)
        {
            return Item_Type.Event_Item_1;
        }

        else if (GoodsTable.Event_Item_SnowMan == type)
        {
            return Item_Type.Event_Item_SnowMan;
        }

        else if (GoodsTable.Event_Item_SnowMan_All == type)
        {
            return Item_Type.Event_Item_SnowMan_All;
        }


        else if (GoodsTable.SulItem == type)
        {
            return Item_Type.SulItem;
        }


        else if (GoodsTable.FeelMulStone == type)
        {
            return Item_Type.FeelMulStone;
        }


        else if (GoodsTable.Asura0 == type)
        {
            return Item_Type.Asura0;
        }


        else if (GoodsTable.Asura1 == type)
        {
            return Item_Type.Asura1;
        }


        else if (GoodsTable.Asura2 == type)
        {
            return Item_Type.Asura2;
        }


        else if (GoodsTable.Asura3 == type)
        {
            return Item_Type.Asura3;
        }

        else if (GoodsTable.Asura4 == type)
        {
            return Item_Type.Asura4;
        }


        else if (GoodsTable.Asura5 == type)
        {
            return Item_Type.Asura5;
        }

        else if (GoodsTable.Aduk == type)
        {
            return Item_Type.Aduk;
        }


        else if (GoodsTable.LeeMuGiStone == type)
        {
            return Item_Type.LeeMuGiStone;
        }

        //
        else if (GoodsTable.SinSkill0 == type)
        {
            return Item_Type.SinSkill0;
        }

        else if (GoodsTable.SinSkill1 == type)
        {
            return Item_Type.SinSkill1;
        }

        else if (GoodsTable.SinSkill2 == type)
        {
            return Item_Type.SinSkill2;
        }

        else if (GoodsTable.SinSkill3 == type)
        {
            return Item_Type.SinSkill3;
        }

        else if (GoodsTable.NataSkill == type)
        {
            return Item_Type.NataSkill;
        }

        else if (GoodsTable.OrochiSkill == type)
        {
            return Item_Type.OrochiSkill;
        }

        //
        else if (GoodsTable.Sun0 == type)
        {
            return Item_Type.Sun0;
        }

        else if (GoodsTable.Sun1 == type)
        {
            return Item_Type.Sun1;
        }

        else if (GoodsTable.Sun2 == type)
        {
            return Item_Type.Sun2;
        }

        else if (GoodsTable.Sun3 == type)
        {
            return Item_Type.Sun3;
        }

        else if (GoodsTable.Sun4 == type)
        {
            return Item_Type.Sun4;
        }

        //
        else if (GoodsTable.Chun0 == type)
        {
            return Item_Type.Chun0;
        }

        else if (GoodsTable.Chun1 == type)
        {
            return Item_Type.Chun1;
        }

        else if (GoodsTable.Chun2 == type)
        {
            return Item_Type.Chun2;
        }

        else if (GoodsTable.Chun3 == type)
        {
            return Item_Type.Chun3;
        }

        else if (GoodsTable.Chun4 == type)
        {
            return Item_Type.Chun4;
        }

        else if (GoodsTable.DokebiSkill0 == type)
        {
            return Item_Type.DokebiSkill0;
        }

        else if (GoodsTable.DokebiSkill1 == type)
        {
            return Item_Type.DokebiSkill1;
        }

        else if (GoodsTable.DokebiSkill2 == type)
        {
            return Item_Type.DokebiSkill2;
        }

        else if (GoodsTable.DokebiSkill3 == type)
        {
            return Item_Type.DokebiSkill3;
        }

        else if (GoodsTable.DokebiSkill4 == type)
        {
            return Item_Type.DokebiSkill4;
        }

        //            //
        else if (GoodsTable.FourSkill0 == type)
        {
            return Item_Type.FourSkill0;
        }

        else if (GoodsTable.FourSkill1 == type)
        {
            return Item_Type.FourSkill1;
        }

        else if (GoodsTable.FourSkill2 == type)
        {
            return Item_Type.FourSkill2;
        }

        else if (GoodsTable.FourSkill3 == type)
        {
            return Item_Type.FourSkill3;
        }

        //   //
        else if (GoodsTable.FourSkill4 == type)
        {
            return Item_Type.FourSkill4;
        }

        else if (GoodsTable.FourSkill5 == type)
        {
            return Item_Type.FourSkill5;
        }

        else if (GoodsTable.FourSkill6 == type)
        {
            return Item_Type.FourSkill6;
        }

        else if (GoodsTable.FourSkill7 == type)
        {
            return Item_Type.FourSkill7;
        }
        else if (GoodsTable.FourSkill8 == type)
        {
            return Item_Type.FourSkill8;
        }

        //            //
        else if (GoodsTable.VisionSkill0 == type)
        {
            return Item_Type.VisionSkill0;
        }

        else if (GoodsTable.VisionSkill1 == type)
        {
            return Item_Type.VisionSkill1;
        }

        else if (GoodsTable.VisionSkill2 == type)
        {
            return Item_Type.VisionSkill2;
        }

        else if (GoodsTable.VisionSkill3 == type)
        {
            return Item_Type.VisionSkill3;
        }
        else if (GoodsTable.VisionSkill4 == type)
        {
            return Item_Type.VisionSkill4;
        }
        else if (GoodsTable.VisionSkill5 == type)
        {
            return Item_Type.VisionSkill5;
        }
        else if (GoodsTable.VisionSkill6 == type)
        {
            return Item_Type.VisionSkill6;
        }
        else if (GoodsTable.VisionSkill7 == type)
        {
            return Item_Type.VisionSkill7;
        }
        else if (GoodsTable.VisionSkill8 == type)
        {
            return Item_Type.VisionSkill8;
        }
        else if (GoodsTable.VisionSkill9 == type)
        {
            return Item_Type.VisionSkill9;
        }
        else if (GoodsTable.VisionSkill10 == type)
        {
            return Item_Type.VisionSkill10;
        }
        else if (GoodsTable.VisionSkill11 == type)
        {
            return Item_Type.VisionSkill11;
        }

        // //
        else if (GoodsTable.ThiefSkill0 == type)
        {
            return Item_Type.ThiefSkill0;
        }

        else if (GoodsTable.ThiefSkill1 == type)
        {
            return Item_Type.ThiefSkill1;
        }

        else if (GoodsTable.ThiefSkill2 == type)
        {
            return Item_Type.ThiefSkill2;
        }

        else if (GoodsTable.ThiefSkill3 == type)
        {
            return Item_Type.ThiefSkill3;
        }
        else if (GoodsTable.ThiefSkill4 == type)
        {
            return Item_Type.ThiefSkill4;
        }

        //

        // //
        else if (GoodsTable.DarkSkill0 == type)
        {
            return Item_Type.DarkSkill0;
        }

        else if (GoodsTable.DarkSkill1 == type)
        {
            return Item_Type.DarkSkill1;
        }

        else if (GoodsTable.DarkSkill2 == type)
        {
            return Item_Type.DarkSkill2;
        }

        else if (GoodsTable.DarkSkill3 == type)
        {
            return Item_Type.DarkSkill3;
        }
        else if (GoodsTable.DarkSkill4 == type)
        {
            return Item_Type.DarkSkill4;
        }

        //
        else if (GoodsTable.GangrimSkill == type)
        {
            return Item_Type.GangrimSkill;
        }

        //

        else if (GoodsTable.SmithFire == type)
        {
            return Item_Type.SmithFire;
        }


        else if (GoodsTable.StageRelic == type)
        {
            return Item_Type.StageRelic;
        }
        
        else if (GoodsTable.GuimoonRelic == type)
        {
            return Item_Type.GuimoonRelic;
        }

        else if (GoodsTable.GuimoonRelicClearTicket == type)
        {
            return Item_Type.GuimoonRelicClearTicket;
        }


        else if (GoodsTable.Peach == type)
        {
            return Item_Type.PeachReal;
        }

        else if (GoodsTable.GuildReward == type)
        {
            return Item_Type.GuildReward;
        }

        else if (GoodsTable.SwordPartial == type)
        {
            return Item_Type.SP;
        }

        else if (GoodsTable.Hel == type)
        {
            return Item_Type.Hel;
        }

        else if (GoodsTable.Ym == type)
        {
            return Item_Type.Ym;
        }

        else if (GoodsTable.Fw == type)
        {
            return Item_Type.Fw;
        }


        else if (GoodsTable.Cw == type)
        {
            return Item_Type.Cw;
        }

        else if (GoodsTable.DokebiFire == type)
        {
            return Item_Type.DokebiFire;
        }
        else if (GoodsTable.SuhoPetFeed == type)
        {
            return Item_Type.SuhoPetFeed;
        }
        else if (GoodsTable.SuhoPetFeedClear == type)
        {
            return Item_Type.SuhoPetFeedClear;
        }
        else if (GoodsTable.SoulRingClear == type)
        {
            return Item_Type.SoulRingClear;
        }

        else if (GoodsTable.SumiFire == type)
        {
            return Item_Type.SumiFire;
        }  
        else if (GoodsTable.SealWeaponClear == type)
        {
            return Item_Type.SealWeaponClear;
        }

        else if (GoodsTable.DosulGoods == type)
        {
            return Item_Type.DosulGoods;
        }
        else if (GoodsTable.TransGoods == type)
        {
            return Item_Type.TransGoods;
        }
        else if (GoodsTable.DosulClear == type)
        {
            return Item_Type.DosulClear;
        }

        else if (GoodsTable.SinsuRelic == type)
        {
            return Item_Type.SinsuRelic;
        }
        else if (GoodsTable.HyungsuRelic == type)
        {
            return Item_Type.HyungsuRelic;
        }
        else if (GoodsTable.ChunguRelic == type)
        {
            return Item_Type.ChunguRelic;
        }
        else if (GoodsTable.FoxRelic == type)
        {
            return Item_Type.FoxRelic;
        }
        else if (GoodsTable.TransGoods == type)
        {
            return Item_Type.TransGoods;
        }
        else if (GoodsTable.FoxRelicClearTicket == type)
        {
            return Item_Type.FoxRelicClearTicket;
        }
        else if (GoodsTable.TransClearTicket == type)
        {
            return Item_Type.TransClearTicket;
        }
        else if (GoodsTable.MeditationGoods == type)
        {
            return Item_Type.MeditationGoods;
        }
        else if (GoodsTable.MeditationClearTicket == type)
        {
            return Item_Type.MeditationClearTicket;
        }
        else if (GoodsTable.DaesanGoods == type)
        {
            return Item_Type.DaesanGoods;
        }
        else if (GoodsTable.EventDice == type)
        {
            return Item_Type.EventDice;
        }
        else if (GoodsTable.Event_SA == type)
        {
            return Item_Type.Event_SA;
        }

        else if (GoodsTable.NewGachaEnergy == type)
        {
            return Item_Type.NewGachaEnergy;
        }

        else if (GoodsTable.DokebiBundle == type)
        {
            return Item_Type.DokebiBundle;
        }
        else if (GoodsTable.Event_Mission == type)
        {
            return Item_Type.Event_Mission;
        }

        else if (GoodsTable.Event_Mission2 == type)
        {
            return Item_Type.Event_Mission2;
        }

        else if (GoodsTable.DokebiFireKey == type)
        {
            return Item_Type.DokebiFireKey;
        }

        else if (GoodsTable.SumiFireKey == type)
        {
            return Item_Type.SumiFireKey;
        }
        else if (GoodsTable.Event_Collection == type)
        {
            return Item_Type.Event_Collection;
        }
        else if (GoodsTable.Event_HotTime == type)
        {
            return Item_Type.Event_HotTime;
        }
        else if (GoodsTable.Event_Collection_All == type)
        {
            return Item_Type.Event_Collection_All;
        }
        else if (GoodsTable.Event_Fall_Gold == type)
        {
            return Item_Type.Event_Fall_Gold;
        }
        else
        {
            return Item_Type.None;
        }
    }
}