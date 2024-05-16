using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Game.GameInfo;
using LitJson;
using System;
using System.Linq;
using GooglePlayGames.BasicApi;
using Spine;
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
    public static string VisionSkill12 = "VisionSkill12";
    public static string VisionSkill13 = "VisionSkill13";
    public static string VisionSkill14 = "VisionSkill14";
    public static string VisionSkill15 = "VisionSkill15";
    public static string VisionSkill16 = "VisionSkill16";
    public static string VisionSkill17 = "VisionSkill17";
    public static string VisionSkill18 = "VisionSkill18";
    public static string VisionSkill19 = "VisionSkill19";
    public static string VisionSkill20 = "VisionSkill20";
    
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

    public static string SinsunSkill0 = "SinsunSkill0";
    public static string SinsunSkill1 = "SinsunSkill1";
    public static string SinsunSkill2 = "SinsunSkill2";
    public static string SinsunSkill3 = "SinsunSkill3";
    public static string SinsunSkill4 = "SinsunSkill4";
    
    public static string DragonSkill0 = "DragonSkill0";
    public static string DragonSkill1 = "DragonSkill1";
    public static string DragonSkill2 = "DragonSkill2";
    public static string DragonSkill3 = "DragonSkill3";
    public static string DragonSkill4 = "DragonSkill4";

    public static string DPSkill0 = "DPSkill0";
    public static string DPSkill1 = "DPSkill1";
    public static string DPSkill2 = "DPSkill2";
    public static string DPSkill3 = "DPSkill3";
    public static string DPSkill4 = "DPSkill4";

    public static string GRSkill0 = "GRSkill0";
    public static string GRSkill1 = "GRSkill1";
    public static string GRSkill2 = "GRSkill2";
    public static string GRSkill3 = "GRSkill3";
    public static string GRSkill4 = "GRSkill4";

    public static string Fw = "Fw";
    public const string Cw = "Cw"; //천계꽃

    public static string c0 = "c0"; //천계꽃
    public static string c1 = "c1"; //천계꽃
    public static string c2 = "c2"; //천계꽃
    public static string c3 = "c3"; //천계꽃
    public static string c4 = "c4"; //천계꽃
    public static string c5 = "c5"; //천계꽃
    public static string c6 = "c6"; //천계꽃

    public static string Event_Kill1_Item = "EC0"; //보리->만두
    public static string Event_Kill1_Item_All = "ECA0"; 
    public static string Event_HotTime = "Event_HotTime0"; //요석 조각
    public static string Event_HotTime_Saved = "EHS0"; //요석 조각 총획득량
    public static string Event_Fall_Gold = "Event_Fall_Gold"; //황금 곶감
    public static string Event_NewYear = "Event_NewYear"; //떡국
    public static string Event_NewYear_All = "Event_NewYear_All"; //신년재화 총생산량
    public static string Event_Mission_Refund = "Event_Mission1";//발리볼 //꽃송이 //바람개비.
    public static string Event_Mission1 = "E1";//할로윈
    public static string Event_Mission1_All = "E1_All"; //할로윈
    public static string Event_Mission2 = "Event_Mission2";//추석
    public static string Event_Mission2_All = "Event_Mission_All2"; //추석
    public static string Event_Mission3 = "Event_Mission3";//보름달
    public static string Event_Mission3_All = "Event_Mission_All3"; //보름달생산량

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
    public static string DragonScale = "DragonScale";
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
    public static string YoPowerGoods = "YPG";
    public static string TaeguekGoods = "TG";
    public static string TaeguekElixir = "TE";
    public static string SuhoTreasure = "SUT";
    public static string FoxRelicClearTicket = "FRCT";
    public static string TransClearTicket = "TCT";
    public static string MeditationGoods = "MG";
    public static string MeditationClearTicket = "MCT";
    public static string DaesanGoods = "DG";
    public static string HonorGoods = "HG";

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
    public static string BlackFoxGoods = "BFG";
    public static string BlackFoxClear = "BFC";

    public static string ByeolhoGoods = "BHG";
    public static string ByeolhoClear = "BHC";
    
    public static string BattleGoods = "BTG";
    public static string BattleClear = "BTC";
    public static string BattleScore = "BTS";
    public static string DragonPalaceTreasure = "DPT";
    public static string GT = "GT";
    public static string WT = "WT";
    public static string SG = "SG";
    public static string SC = "SC";
    public static string SB = "SB";
    public static string HYG = "HYG";
    public static string HYC = "HYC";
    public static string MRT = "MRT";
    public static string DBT = "DBT";
    public static string SRG = "SRG";
    public static string YOT = "YOT";
    public static string TJCT = "TJCT";
    public static string RJ = "RJ";
    public static string YJ = "YJ";
    public static string BJ = "BJ";
    public static string DC = "DC";
    public static string DE = "DE";
    public static string DCT = "DCT";


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
        { VisionSkill12, 0f },
        { VisionSkill13, 0f },
        { VisionSkill14, 0f },
        { VisionSkill15, 0f },
        { VisionSkill16, 0f },
        { VisionSkill17, 0f },
        { VisionSkill18, 0f },
        { VisionSkill19, 0f },
        { VisionSkill20, 0f },

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

        { SinsunSkill0, 0f },
        { SinsunSkill1, 0f },
        { SinsunSkill2, 0f },
        { SinsunSkill3, 0f },
        { SinsunSkill4, 0f },

        { DragonSkill0, 0f },
        { DragonSkill1, 0f },
        { DragonSkill2, 0f },
        { DragonSkill3, 0f },
        { DragonSkill4, 0f },

        { DPSkill0, 0f },
        { DPSkill1, 0f },
        { DPSkill2, 0f },
        { DPSkill3, 0f },
        { DPSkill4, 0f },

        { GRSkill0, 0f },
        { GRSkill1, 0f },
        { GRSkill2, 0f },
        { GRSkill3, 0f },
        { GRSkill4, 0f },

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

        { Event_Kill1_Item, 0f },
        { Event_HotTime, 0f },
        { Event_HotTime_Saved, 0f },
        { Event_Kill1_Item_All, 0f },
        { Event_Fall_Gold, 0f },
        { Event_NewYear, 0f },
        { Event_NewYear_All, 0f },
        { Event_Mission1, 0f },
        { Event_Mission1_All, 0f },
        { Event_Mission2, 0f },
        { Event_Mission2_All, 0f },
        { Event_Mission3, 0f },
        { Event_Mission3_All, 0f },
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
        { DragonScale, 0f },
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
        { YoPowerGoods, 0f },
        { TaeguekGoods, 0f },
        { TaeguekElixir, 0f },
        { SuhoTreasure, 0f },
        { FoxRelicClearTicket, 0f },
        { TransClearTicket, 0f },
        { MeditationGoods, 0f },
        { MeditationClearTicket, 0f },
        { DaesanGoods, 0f },
        { HonorGoods, 0f },

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
        { BlackFoxGoods, 0f },
        { BlackFoxClear, 0f },
        { ByeolhoGoods, 0f },
        { ByeolhoClear, 0f },
        { BattleGoods, 0f },
        { BattleClear, 7f },
        { BattleScore, 0f },
        { DragonPalaceTreasure, 0f },
        { GT, GameBalance.GachaTicketDailyGetAmount },
        { WT, GameBalance.WeeklyTicketWeeklyGetAmount },
        { SG, 0f },
        { SC, GameBalance.SinsuClearDailyGetAmount },
        { SB, 0f},
        { HYG, 0f},
        { HYC, GameBalance.HyulClearDailyGetAmount},
        { MRT, 0f},
        { DBT, 0f},
        { SRG, 0f},
        { YOT, 0f},
        { TJCT, 1f},
        { RJ, 0f},
        { YJ, 0f},
        { BJ, 0f},
        { DC, 0f},
        { DE, 0f},
        { DCT, GameBalance.DCTDailyGetAmount},
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

    private float goldBarItemAddNum = 0;
    public void GetGoldBar(float amount)
    {
        goldBarItemAddNum += amount;

        //100킬마다 얻게하기 위해서
        if (goldBarItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Goldbar , goldBarGetAmountMin))
        {
        }
        else
        {
            tableDatas[GoldBar].Value += (int)goldBarItemAddNum;
            goldBarItemAddNum -= (int)goldBarItemAddNum;
        }
    }

    static int growThStoneAddAmount = 0;
    static float updateRequireNum_GrowthStone_0 = 10000; //1000억 미만
    static float updateRequireNum_GrowthStone_1 = 100000; //조 미만
    static float updateRequireNum_GrowthStone_2 = 1000000; //조 이상
    static float updateRequireNum_GrowthStone_3 = 10000000; // 10조이상

    private static float stageRelicGetAmountMin = 1;
    private static float goldBarGetAmountMin = 1;
    private static float peachGetAmountMin = 100;
    private static float helGetAmountMin = 100;
    private static float chunGetAmountMin = 100;
    private static float dokebiGetAmountMin = 100;
    private static float yoPowerGetAmountMin = 100;
    private static float taeguekGetAmountMin = 100;
    private static float sgGetAmountMin = 100;
    private static float sumiGetAmountMin = 100;
    
    public void RefreshGetItemAmount()
    {
        stageRelicGetAmountMin = AddGetItemAmount(tableDatas[StageRelic].Value);
        goldBarGetAmountMin = AddGetItemAmount(tableDatas[GoldBar].Value);
        peachGetAmountMin = AddGetItemAmount(tableDatas[Peach].Value);
        helGetAmountMin = AddGetItemAmount(tableDatas[Hel].Value);
        chunGetAmountMin = AddGetItemAmount(tableDatas[Cw].Value);
        dokebiGetAmountMin = AddGetItemAmount(tableDatas[DokebiFire].Value);
        yoPowerGetAmountMin = AddGetItemAmount(tableDatas[YoPowerGoods].Value);
        dokebiGetAmountMin = AddGetItemAmount(tableDatas[DokebiFire].Value);
        taeguekGetAmountMin = AddGetItemAmount(tableDatas[TaeguekGoods].Value);
        sgGetAmountMin = AddGetItemAmount(tableDatas[SG].Value);
        sumiGetAmountMin = AddGetItemAmount(tableDatas[SG].Value);
    }
    
    private int AddGetItemAmount(float amount)
    {
        float result = Mathf.Log10(amount);
        int powerOfTen = Mathf.RoundToInt(result);
        var min = powerOfTen - 6;

        var getItemValue = (int)Mathf.Max(Mathf.Pow(10, min),1);
        
        return getItemValue;
    }
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
        if (peachItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Peachamount, peachGetAmountMin))
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
        if (helItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Helamount, helGetAmountMin))
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
        if (chunItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Chunfloweramount, chunGetAmountMin))
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
        if (dokebiItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Dokebifireamount, dokebiGetAmountMin))
        {
        }
        else
        {
            tableDatas[DokebiFire].Value += (int)dokebiItemAddNum;
            dokebiItemAddNum -= (int)dokebiItemAddNum;
        }
    }
    static float yoPowerItemAddNum = 0;
    public void GetYoPowerItem(float amount)
    {
        yoPowerItemAddNum += amount * (1 + PlayerStats.GetYoPowerGoodsGainValue());

        //1개 획득할때마다 얻게하기 위해서
        if (yoPowerItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Yokaiessence, yoPowerGetAmountMin))
        {
        }
        else
        {
            tableDatas[YoPowerGoods].Value += (int)yoPowerItemAddNum;
            yoPowerItemAddNum -= (int)yoPowerItemAddNum;
        }
    }
    static float taeguekItemAddNum = 0;
    public void GetTaegeukItem(float amount)
    {
        taeguekItemAddNum += amount * (1 + PlayerStats.GetTaegeukGoodsGainValue());

        //1개 획득할때마다 얻게하기 위해서
        if (taeguekItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Taegeuk, taeguekGetAmountMin))
        {
        }
        else
        {
            tableDatas[TaeguekGoods].Value += (int)taeguekItemAddNum;
            taeguekItemAddNum -= (int)taeguekItemAddNum;
        }
    }
    static float sgItemAddNum = 0;
    public void GetSinsuItem(float amount)
    {
        sgItemAddNum += amount * (1 + PlayerStats.GetSasinsuGoodsGainValue());

        //1개 획득할때마다 얻게하기 위해서
        if (sgItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Sinsu, sgGetAmountMin))
        {
        }
        else
        {
            tableDatas[SG].Value += (int)sgItemAddNum;
            sgItemAddNum -= (int)sgItemAddNum;
        }
    }
    //

    static float sumiItemAddNum = 0;
    public void GetSumiItem(float amount)
    {
        sumiItemAddNum += amount * (1 + PlayerStats.GetSumiFireGainValue());

        //1개 획득할때마다 얻게하기 위해서
        if (sumiItemAddNum < Mathf.Max(updateRequireNum * GameManager.Instance.CurrentStageData.Sumifloweramount, sumiGetAmountMin))
        {
        }
        else
        {
            tableDatas[SumiFire].Value += (int)sumiItemAddNum;
            sumiItemAddNum -= (int)sumiItemAddNum;
        }
    }
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
            SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.Event_Kill1_Item)} 획득(+{(int)amount})");

        eventItemAddNum_Spring += (int)amount;

        if (eventItemAddNum_Spring < updateRequireNum)
        {
        }
        else
        {
            //tableDatas[Event_Item_1].Value += eventItemAddNum_Spring;
            tableDatas[Event_Kill1_Item].Value += eventItemAddNum_Spring;
            //총획득량
            if (ServerData.iapServerTable.TableDatas[UiCollectionPass0BuyButton.PassKey].buyCount.Value == 0)
            {
                tableDatas[Event_Kill1_Item_All].Value += eventItemAddNum_Spring;
            }
            else
            {
                tableDatas[Event_Kill1_Item].Value += eventItemAddNum_Spring;
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

    static float stageRelicAddNum = 0;

    public void GetStageRelic(float amount)
    {
        if (SystemMessage.IsMessageQueueFull() == false)
            SystemMessage.Instance.SetMessage($"{CommonString.GetItemName(Item_Type.StageRelic)} 획득(+{(int)amount})");

        stageRelicAddNum += amount;

        if (stageRelicAddNum < Mathf.Max(updateRequireNum , stageRelicGetAmountMin))
        {
        }
        else
        {
            tableDatas[StageRelic].Value += (int)stageRelicAddNum;
            stageRelicAddNum -= (int)stageRelicAddNum;
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
    public void UpDataV2(string key, bool LocalOnly, Action successCallBack=null)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Status {key} is not exist");
            return;
        }

        UpDataV2(key, tableDatas[key].Value, LocalOnly,successCallBack);
    }

    public void UpDataV2(string key, float data, bool LocalOnly, Action successCallBack=null)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"Growth {key} is not exist");
            return;
        }

        tableDatas[key].Value = data;

        if (LocalOnly == false)
        {
            SyncToServerEachV2(key,whenSyncSuccess:successCallBack);
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
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSumiFire).Value > 0)
        {
            goodsParam.Add(GoodsTable.SumiFire, ServerData.goodsTable.GetTableData(GoodsTable.SumiFire).Value);
        }
        //if (ServerData.userInfoTable.CanSpawnEventItem())
        //{
        //    goodsParam.Add(GoodsTable.Event_Item_0, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_0).Value);
        //}

        //goodsParam.Add(GoodsTable.Event_Item_1, ServerData.goodsTable.GetTableData(GoodsTable.Event_Item_1).Value);

        if (ServerData.userInfoTable.CanSpawnSpringEventItem())
        {
            goodsParam.Add(GoodsTable.Event_Kill1_Item, ServerData.goodsTable.GetTableData(GoodsTable.Event_Kill1_Item).Value);
            if (ServerData.iapServerTable.TableDatas[UiCollectionPass0BuyButton.PassKey].buyCount.Value == 0)
            {
                goodsParam.Add(GoodsTable.Event_Kill1_Item_All, ServerData.goodsTable.GetTableData(GoodsTable.Event_Kill1_Item_All).Value);
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
            case Item_Type.VisionSkill12:
            {
                return GoodsTable.VisionSkill12;
            }

            case Item_Type.VisionSkill13:
            {
                return GoodsTable.VisionSkill13;
            }
            case Item_Type.VisionSkill14:
            {
                return GoodsTable.VisionSkill14;
            }
            case Item_Type.VisionSkill15:
            {
                return GoodsTable.VisionSkill15;
            }
            case Item_Type.VisionSkill16:
            {
                return GoodsTable.VisionSkill16;
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
            //            //
            case Item_Type.SinsunSkill0:
            {
                return GoodsTable.SinsunSkill0;
            }

            case Item_Type.SinsunSkill1:
            {
                return GoodsTable.SinsunSkill1;
            }

            case Item_Type.SinsunSkill2:
            {
                return GoodsTable.SinsunSkill2;
            }

            case Item_Type.SinsunSkill3:
            {
                return GoodsTable.SinsunSkill3;
            }

            case Item_Type.SinsunSkill4:
            {
                return GoodsTable.SinsunSkill4;
            }

            //
            //            //
            case Item_Type.DragonSkill0:
            {
                return GoodsTable.DragonSkill0;
            }

            case Item_Type.DragonSkill1:
            {
                return GoodsTable.DragonSkill1;
            }

            case Item_Type.DragonSkill2:
            {
                return GoodsTable.DragonSkill2;
            }

            case Item_Type.DragonSkill3:
            {
                return GoodsTable.DragonSkill3;
            }

            case Item_Type.DragonSkill4:
            {
                return GoodsTable.DragonSkill4;
            }
            //            //
            case Item_Type.DPSkill0:
            {
                return GoodsTable.DPSkill0;
            }

            case Item_Type.DPSkill1:
            {
                return GoodsTable.DPSkill1;
            }

            case Item_Type.DPSkill2:
            {
                return GoodsTable.DPSkill2;
            }

            case Item_Type.DPSkill3:
            {
                return GoodsTable.DPSkill3;
            }

            case Item_Type.DPSkill4:
            {
                return GoodsTable.DPSkill4;
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

            case Item_Type.YoPowerGoods:
            {
                return GoodsTable.YoPowerGoods;
            }

            case Item_Type.TaeguekGoods:
            {
                return GoodsTable.TaeguekGoods;
            }

            case Item_Type.TaeguekElixir:
            {
                return GoodsTable.TaeguekElixir;
            }

            case Item_Type.BlackFoxGoods:
            {
                return GoodsTable.BlackFoxGoods;
            }
            case Item_Type.BlackFoxClear:
            {
                return GoodsTable.BlackFoxClear;
            }
            
            case Item_Type.ByeolhoGoods:
            {
                return GoodsTable.ByeolhoGoods;
            }
            case Item_Type.ByeolhoClear:
            {
                return GoodsTable.ByeolhoClear;
            }
            
            case Item_Type.BattleGoods:
            {
                return GoodsTable.BattleGoods;
            }
            case Item_Type.BattleClear:
            {
                return GoodsTable.BattleClear;
            }
            case Item_Type.BattleScore:
            {
                return GoodsTable.BattleScore;
            }
            case Item_Type.DPT:
            {
                return GoodsTable.DragonPalaceTreasure;
            }
            case Item_Type.GT:
            {
                return GoodsTable.GT;
            }
            
            case Item_Type.WT:
            {
                return GoodsTable.WT;
            }
            case Item_Type.SG:
            {
                return GoodsTable.SG;
            }
            case Item_Type.SC:
            {
                return GoodsTable.SC;
            }
            case Item_Type.SB:
            {
                return GoodsTable.SB;
            }
            
            case Item_Type.SuhoTreasure:
            {
                return GoodsTable.SuhoTreasure;
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
            case Item_Type.HonorGoods:
            {
                return GoodsTable.HonorGoods;
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
            case Item_Type.Event_Mission1:
            {
                return GoodsTable.Event_Mission1;
            }
            case Item_Type.Event_Mission1_All:
            {
                return GoodsTable.Event_Mission1_All;
            }
            case Item_Type.Event_Mission2:
            {
                return GoodsTable.Event_Mission2;
            }
            case Item_Type.Event_Mission2_All:
            {
                return GoodsTable.Event_Mission2_All;
            }
            case Item_Type.Event_Mission3:
            {
                return GoodsTable.Event_Mission3;
            }

            case Item_Type.DokebiFireKey:
            {
                return GoodsTable.DokebiFireKey;
            }

            case Item_Type.SumiFireKey:
            {
                return GoodsTable.SumiFireKey;
            }
            case Item_Type.Event_Kill1_Item:
            {
                return GoodsTable.Event_Kill1_Item;
            }
            case Item_Type.Event_HotTime:
            {
                return GoodsTable.Event_HotTime;
            }
            case Item_Type.Event_Collection_All:
            {
                return GoodsTable.Event_Kill1_Item_All;
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
        else if (GoodsTable.GoldBar == type)
        {
            return Item_Type.GoldBar;
        }
        else if (GoodsTable.Jade == type)
        {
            return Item_Type.Jade;
        }
        else if (GoodsTable.Mileage == type)
        {
            return Item_Type.Mileage;
        }
        else if (GoodsTable.ClearTicket == type)
        {
            return Item_Type.ClearTicket;
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
        else if (GoodsTable.Relic == type)
        {
            return Item_Type.Relic;
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
        else if (GoodsTable.VisionSkill12 == type)
        {
            return Item_Type.VisionSkill12;
        }
        else if (GoodsTable.VisionSkill13 == type)
        {
            return Item_Type.VisionSkill13;
        }
        else if (GoodsTable.VisionSkill14 == type)
        {
            return Item_Type.VisionSkill14;
        }
        else if (GoodsTable.VisionSkill15 == type)
        {
            return Item_Type.VisionSkill15;
        }
        else if (GoodsTable.VisionSkill16 == type)
        {
            return Item_Type.VisionSkill16;
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
        // //
        else if (GoodsTable.SinsunSkill0 == type)
        {
            return Item_Type.SinsunSkill0;
        }

        else if (GoodsTable.SinsunSkill1 == type)
        {
            return Item_Type.SinsunSkill1;
        }

        else if (GoodsTable.SinsunSkill2 == type)
        {
            return Item_Type.SinsunSkill2;
        }

        else if (GoodsTable.SinsunSkill3 == type)
        {
            return Item_Type.SinsunSkill3;
        }
        else if (GoodsTable.SinsunSkill4 == type)
        {
            return Item_Type.SinsunSkill4;
        }

        //
        // //
        else if (GoodsTable.DragonSkill0 == type)
        {
            return Item_Type.DragonSkill0;
        }

        else if (GoodsTable.DragonSkill1 == type)
        {
            return Item_Type.DragonSkill1;
        }

        else if (GoodsTable.DragonSkill2 == type)
        {
            return Item_Type.DragonSkill2;
        }

        else if (GoodsTable.DragonSkill3 == type)
        {
            return Item_Type.DragonSkill3;
        }
        else if (GoodsTable.DragonSkill4 == type)
        {
            return Item_Type.DragonSkill4;
        }

        //
        else if (GoodsTable.DPSkill0 == type)
        {
            return Item_Type.DPSkill0;
        }

        else if (GoodsTable.DPSkill1 == type)
        {
            return Item_Type.DPSkill1;
        }

        else if (GoodsTable.DPSkill2 == type)
        {
            return Item_Type.DPSkill2;
        }

        else if (GoodsTable.DPSkill3 == type)
        {
            return Item_Type.DPSkill3;
        }
        else if (GoodsTable.DPSkill4 == type)
        {
            return Item_Type.DPSkill4;
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
        else if (GoodsTable.BlackFoxGoods == type)
        {
            return Item_Type.BlackFoxGoods;
        }
        else if (GoodsTable.BlackFoxClear == type)
        {
            return Item_Type.BlackFoxClear;
        }

        else if (GoodsTable.ByeolhoGoods == type)
        {
            return Item_Type.ByeolhoGoods;
        }
        else if (GoodsTable.ByeolhoClear == type)
        {
            return Item_Type.ByeolhoClear;
        }

        else if (GoodsTable.BattleGoods == type)
        {
            return Item_Type.BattleGoods;
        }
        else if (GoodsTable.BattleClear == type)
        {
            return Item_Type.BattleClear;
        }
        else if (GoodsTable.BattleScore == type)
        {
            return Item_Type.BattleScore;
        }
        else if (GoodsTable.GT == type)
        {
            return Item_Type.GT;
        }
        else if (GoodsTable.WT == type)
        {
            return Item_Type.WT;
        }
        else if (GoodsTable.SG == type)
        {
            return Item_Type.SG;
        }
        else if (GoodsTable.SC == type)
        {
            return Item_Type.SC;
        }
        else if (GoodsTable.SB == type)
        {
            return Item_Type.SB;
        }

        else if (GoodsTable.DragonPalaceTreasure == type)
        {
            return Item_Type.DPT;
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
        else if (GoodsTable.YoPowerGoods == type)
        {
            return Item_Type.YoPowerGoods;
        }
        else if (GoodsTable.TaeguekGoods == type)
        {
            return Item_Type.TaeguekGoods;
        }
        else if (GoodsTable.TaeguekElixir == type)
        {
            return Item_Type.TaeguekElixir;
        }
        else if (GoodsTable.SuhoTreasure == type)
        {
            return Item_Type.SuhoTreasure;
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
        else if (GoodsTable.HonorGoods == type)
        {
            return Item_Type.HonorGoods;
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
        else if (GoodsTable.Event_Mission1 == type)
        {
            return Item_Type.Event_Mission1;
        }

        else if (GoodsTable.Event_Mission2 == type)
        {
            return Item_Type.Event_Mission2;
        }

        else if (GoodsTable.Event_Mission3 == type)
        {
            return Item_Type.Event_Mission3;
        }

        else if (GoodsTable.DokebiFireKey == type)
        {
            return Item_Type.DokebiFireKey;
        }

        else if (GoodsTable.SumiFireKey == type)
        {
            return Item_Type.SumiFireKey;
        }
        else if (GoodsTable.Event_Kill1_Item == type)
        {
            return Item_Type.Event_Kill1_Item;
        }
        else if (GoodsTable.Event_HotTime == type)
        {
            return Item_Type.Event_HotTime;
        }
        else if (GoodsTable.Event_Kill1_Item_All == type)
        {
            return Item_Type.Event_Collection_All;
        }
        else if (GoodsTable.Event_Fall_Gold == type)
        {
            return Item_Type.Event_Fall_Gold;
        }
        
        else if (GoodsTable.Relic == type)
        {
            return Item_Type.Relic;
        }
        
        else
        {
            if (type.Contains("costume"))
            {
                var tableData  = TableManager.Instance.Costume.dataArray;

                if (tableData.Any(t => t.Stringid == type))
                {
                    return (Item_Type)Enum.Parse(typeof(Item_Type), type);
                }   
            }

            return Enum.TryParse<Item_Type>(type, out Item_Type itemType) ? itemType : Item_Type.None;
        }
    }
}