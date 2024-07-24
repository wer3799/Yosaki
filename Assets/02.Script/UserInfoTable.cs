using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;
using CodeStage.AntiCheat.ObscuredTypes;
using GoogleMobileAds.Api;

public class UserInfoTable
{
    public static string Indate;
    public const string tableName = "UserInfo";

    public const string Hp = "Hp";
    public const string Mp = "Mp";
    public const string LastMap = "LastMap";

    public const string LastLogin = "LastLogin";
    public const string removeAd = "removeAd";

    public const string gachaNum_Weapon = "gachaNum_Weapon";
    public const string gachaNum_Norigae = "gachaNum_Norigae";
    public const string gachaNum_Skill = "gachaNum_Skill";
    public const string gachaNum_NewGacha = "GNNG";

    public const string hackingCount = "hackingCount";

    public const string passSelectedIdx = "passSelectedIdx";
    public const string currentFloorIdx = "currentFloorIdx";
    public const string currentFloorIdx2 = "cf2";
    public const string currentFloorIdx3 = "cf3";
    public const string currentFloorIdx4 = "cf4"; //우마왕
    public const string currentFloorIdx5 = "cf5"; //경락마사지
    public const string RoyalTombFloorIdx = "RoyalTombFloorIdx";
    public const string DarkTowerIdx = "dti";
    public const string currentFloorIdx6 = "cf6"; //신수타워
    public const string currentFloorGuildTower = "cfg0"; //문파 전갈굴
    public const string currentFloorIdx7 = "cf7"; //중단전
    public const string currentFloorIdx9 = "cf9"; //봉인검타워
    public const string foxTowerIdx = "fti"; //여우전
    public const string foxFireIdx = "ffi"; //여우전

    public const string receiveReviewReward = "receiveReviewReward";

    public const string dailyEnemyKillCount = "dailyEnemyKillCount";
    public const string dailybooty = "dailybooty"; //전리품


    public const string dailyTicketBuyCount = "dailyTicketBuyCount";
    public const string receivedTicketReward = "receivedTicketReward";

    public const string bonusDungeonEnterCount = "bonudun6";

    public const string dokebiKillCount3 = "dokebiKillCount7";


    public const string chatBan = "chatBan";

    public const string tutorialCurrentStep = "tutorialCurrentStep";
    public const string tutorialClearFlags = "tutorialClearFlags";

    public const string managerDescriptionFlags = "managerDescriptionFlags";
    public const string attendanceCount = "ac1";
    public const string attendanceCount_100Day = "attendanceCount_100Day";
    public const string attendanceCount_Seol = "atten_Seol";
    public const string attendanceCount_Dol = "atten_Dol";

    public const string marbleAwake = "marbleAwake";
    public const string resetStat = "resetStat";

    public const string buff_gold1 = "gold1_new_new_new";
    public const string buff_gold2 = "gold2_new_new_new";
    public const string buff_exp1 = "exp1_new_new_new";
    public const string buff_exp2 = "exp2_new_new_new";

    public const string guild_buff0 = "guild_buff0";
    public const string guild_buff1 = "guild_buff1";
    public const string guild_buff2 = "guild_buff2";
    public const string guild_buff3 = "guild_buff3";
    public const string one_Buff = "ob";
    public const string mf11_Buff = "mf11";
    public const string ma11_Buff = "ma11";

    public const string mf12_Buff = "mf12";
    public const string ma12_Buff = "ma12";

    public const string season0_Buff = "season0"; //혹서기
    public const string season1_Buff = "season1";
    public const string season2_Buff = "season2"; //혹한기버프
    public const string season3_Buff = "season3";

    public const string winter0_Buff = "winter0"; //겨울훈련 버프
    public const string winter1_Buff = "winter1";

    public const string yomul0_buff = "yomul0_buff";
    public const string yomul1_buff = "yomul1_buff";
    public const string yomul2_buff = "yomul2_buff";
    public const string yomul3_buff = "yomul3_buff";
    public const string yomul4_buff = "yomul4_buff";
    public const string yomul5_buff = "yomul5_buff";
    public const string yomul6_buff = "yomul6_buff";
    public const string yomul7_buff = "yomul7_buff";

    public const string bonusDungeonMaxKillCount = "bonusDungeonMaxKillCount";

    public const string wingPackageRewardReceive = "wingPackageRewardReceive";
    public const string topClearStageId = "topClearStageId";
    public const string selectedSkillGroupId = "selectedSkillGroupId";
    public const string dokebiEnterCount = "dec3";
    public const string dokebiNewEnterCount = "dnec3";
    public const string chatFrame = "chatFrame";
    public const string hellMark = "hellMedal";

    public const string freeWeapon = "freeWeapon";
    public const string freeNorigae = "freeNorigae";
    public const string freeSkill = "freeSkill";
    public const string freeNewGacha = "FNG";

    public const string oakpensionAttendance = "oakpension";
    public const string marblepensionAttendance = "marblepension";
    public const string hellpensionAttendance = "hellpension";
    public const string chunpensionAttendance = "chunpension";
    public const string dokebipensionAttendance = "dokebipension";
    public const string sumipensionAttendance = "sumipension";
    public const string ringpensionAttendance = "ringpension";
    public const string suhopetfeedclearpensionAttendance = "suhopetfeedclearpension";
    public const string foxfirepension = "foxfirepension";
    public const string sealswordpension = "sealswordpension";
    public const string dosulpension = "dosulpension";
    public const string guimoonpension = "guimoonpension";
    public const string meditationpension = "meditationpension";
    public const string taeguekpension = "taeguekpension";
    public const string blacksoulpension = "blacksoulpension";
    public const string studentspotpension = "studentspotpension";
    public const string transjeweltpension = "transjeweltpension";
    public const string masterpension = "masterpension";

    public const string marblePackChange = "marblePackChange";

    public const string yoguiSogulLastClear = "yoguiSogulLastClear";
    public const string oldDokebi2LastClear = "oldlc";
    public const string smithClear = "smithClear";
    public const string gumGiClear = "gumGiClear";
    public const string sumiFireClear = "sfc";
    public const string gumGiSoulClear = "gsc";
    public const string smithTreeClear = "stc";
    public const string sonCloneClear = "sccc";
    public const string flowerClear = "fc";
    public const string DokebiFireClear = "DokebiFireClear";
    public const string DayOfWeekClear = "dowc";

    //홀수 월간훈련(Monthlypass2)
    //public const string killCountTotal2 = "k17";
    public const string monthAttendCount = "mac";
    public const string killCountTotalChild = "fal"; //가을훈련
    public const string killCountTotalWinterPass = "KillCountWinterPass"; //가을훈련

    public const string killCountTotalSeason = "ks1"; //봄훈련

    //public const string killCountTotalSeason2 = "ks2"; //새학기훈련
    public const string killCountTotalSeason3 = "ks3"; //수호훈련


    public const string attenCountBok = "kb";
    public const string attenCountSpring = "acs";
    public const string attenCountChuSeok = "kchu";
    public const string attenCountSeason = "as1";

    public const string usedCollectionCount = "ufc0"; //곶감사용 => 봄나물 사용
    public const string usedSnowManCollectionCount = "uc1"; //어린이날사용 기존 usc


    public const string relicKillCount = "relicKillCount"; // 영숲
    public const string hellRelicKillCount = "hrk"; // 지옥영숲

    public const string usedRelicTicketNum = "usedRelicTicketNum";

    public const string relicpensionAttendance = "relicpension";
    public const string peachAttendance = "peachpension";
    public const string smithpensionAttendance = "smithpension";
    public const string weaponpensionAttendance = "weaponpension";

    public const string monthreset = "manduReset";
    public const string sonScore = "son6";
    public const string hellWarScore = "hws";
    public const string catScore = "csc";
    public const string hellScore = "hels";
    public const string chunClear = "chun"; //꽃
    public const string susanoScore = "susa";
    public const string norigaeScore = "nss";
    public const string gradeScore = "grade";
    public const string yumScore = "yumScore";
    public const string okScore = "okScore";
    public const string doScore = "doScore";
    public const string sumiScore = "sumiScore";
    public const string thiefScore = "thiefScore";
    public const string sleepRewardSavedTime = "sleepRewardSavedTime";
    public const string buffAwake = "buffAwake";
    public const string petAwake = "petAwake";
    public const string IgnoreDamDec = "IgnoreDamDec";
    public const string SendGuildPoint = "SendGuildPoint3";
    public const string cockAwake = "cockAwake";
    public const string dogAwake = "dogAwake";
    public const string basicPackRefund = "basicPackRefund";
    public const string dolPassRefund = "dolPassRefund";
    public const string skillInitialized = "ski";
    public const string smithExp = "smith";
    public const string getSmith = "getSmith";
    public const string getGumGi = "getGumGi";
    public const string getSumiFire = "gsf";
    public const string getFlower = "getc";
    public const string getDokebiFire = "getDokebiFire";
    public const string getRingGoods = "grg";
    public const string getDayOfWeek = "gdow";
    public const string getDokebiBundle = "gdb";

    public const string sendPetExp = "sendPetExp";

    public const string collectionEventInitialize = "cei0";
    public const string trainingEventInitialize0 = "tei0";

    public const string exchangeCount_0 = "ec_0";
    public const string exchangeCount_1 = "ec_1";
    public const string exchangeCount_2 = "ec_2";
    public const string exchangeCount_3 = "ec_3";
    public const string exchangeCount_4 = "ec_4";

    public const string snow_exchangeCount_0 = "co0_0";
    public const string snow_exchangeCount_1 = "co0_1";
    public const string snow_exchangeCount_2 = "co0_2";
    public const string snow_exchangeCount_3 = "co0_3";
    public const string snow_exchangeCount_4 = "co0_4";
    public const string snow_exchangeCount_5 = "co0_5";

    public const string refundFox = "rf";
    public const string sendGangChul = "sg";
    public const string foxMask = "fm";
    public const string relicPackReset = "rr";
    public const string oneAttenEvent = "oe";
    public const string titleRefund = "tii";
    public const string oneAttenEvent_one = "oo";
    public const string relicReset = "rkrk";

    public const string canRecommendCount = "canRecommendCount3";
    public const string mileageRefund = "mr";
    public const string RefundIdx = "RI";
    public const string marRelicRefund = "marRelicRe3";
    public const string growthStoneRefund = "gsRefund";

    public const string purchaseRefund0 = "purchaseRefund0";
    public const string newGachaEnergyRefund = "newGachaEnergyRefund";
    public const string titleConvertNewTitle = "titleConvertNewTitle";
    public const string titleConvertNewTitle2 = "titleConvertNewTitle2";
    public const string relocateLevelPass = "relocateLevelPass";
    public const string chunmaRefund = "chunmaRefund";

    public const string exchangeCount_0_Mileage = "mff";
    public const string exchangeCount_1_Mileage = "mgg";
    public const string exchangeCount_2_Mileage = "mbu";
    public const string exchangeCount_3_Mileage = "mcu";
    public const string exchangeCount_4_Mileage = "mdo";

    public const string exchangeCount_5_Mileage = "exchange_mileage5";
    public const string exchangeCount_6_Mileage = "exchange_mileage6";
    public const string exchangeCount_7_Mileage = "exchange_mileage7";
    public const string exchangeCount_8_Mileage = "exchange_mileage8";
    public const string exchangeCount_9_Mileage = "exchange_mileage9";


    public const string exchangeCount_10_Mileage = "exchange_mileage10";
    public const string exchangeCount_11_Mileage = "exchange_mileage11";
    public const string exchangeCount_12_Mileage = "exchange_mileage12";

    public const string exchangeCount_13_Mileage = "mso";
    public const string exchangeCount_14_Mileage = "msm";
    public const string exchangeCount_15_Mileage = "msh";

    public const string exchangeCount_16_Mileage = "m16";
    public const string exchangeCount_17_Mileage = "m17";
    public const string exchangeCount_18_Mileage = "m18";
    public const string exchangeCount_19_Mileage = "m19";
    public const string exchangeCount_20_Mileage = "m20";
    public const string exchangeCount_21_Mileage = "m21";
    public const string exchangeCount_22_Mileage = "m22";
    public const string exchangeCount_23_Mileage = "m23";
    public const string exchangeCount_24_Mileage = "m24";
    public const string exchangeCount_25_Mileage = "m25";
    public const string exchangeCount_26_Mileage = "m26";
    public const string exchangeCount_27_Mileage = "m27";
    public const string exchangeCount_28_Mileage = "m28";
    public const string exchangeCount_29_Mileage = "m29";
    public const string exchangeCount_30_Mileage = "m30";
    public const string exchangeCount_31_Mileage = "m31";

 
    public const string ny_ex_0 = "ny_ex_0";
    public const string ny_ex_1 = "ny_ex_1";
    public const string ny_ex_2 = "ny_ex_2";
    public const string ny_ex_3 = "ny_ex_3";
    public const string ny_ex_4 = "ny_ex_4";
    public const string ny_ex_5 = "ny_ex_5";

    public const string eventMissionInitialize = "emi_0";

    public const string eventMission0_0 = "em_2_0";
    public const string eventMission0_1 = "em_2_1";
    public const string eventMission0_2 = "em_2_2";
    public const string eventMission0_3 = "em_2_3";
    public const string eventMission0_4 = "em_2_4";
    public const string eventMission0_5 = "em_2_5";
    public const string eventMission0_6 = "em_2_6";
    public const string eventMission0_7 = "em_2_7";
    public const string eventMission0_8 = "em_2_8";
    public const string eventMission0_9 = "em_2_9";


    public const string eventMission1_0 = "em_3_0";
    public const string eventMission1_1 = "em_3_1";
    public const string eventMission1_2 = "em_3_2";
    public const string eventMission1_3 = "em_3_3";
    public const string eventMission1_4 = "em_3_4";
    public const string eventMission1_5 = "em_3_5";

    public const string eventMission1_6 = "em_3_6";
    public const string eventMission1_7 = "em_3_7";
    public const string eventMission1_8 = "em_3_8";
    public const string eventMission1_9 = "em_3_9";
    public const string eventMission1_10 = "em_3_10";
    public const string eventMission1_11 = "em_3_11";

    public const string eventMission2_0 = "em_4_0";
    public const string eventMission2_1 = "em_4_1";
    public const string eventMission2_2 = "em_4_2";
    public const string eventMission2_3 = "em_4_3";
    public const string eventMission2_4 = "em_4_4";
    public const string eventMission2_5 = "em_4_5";

    public const string eventMission2_6 = "em_4_6";
    public const string eventMission2_7 = "em_4_7";
    public const string eventMission2_8 = "em_4_8";
    public const string eventMission2_9 = "em_4_9";
    public const string eventMission2_10 = "em_4_10";
    public const string eventMission2_11 = "em_4_11";


    public const string eventMission3_0 = "em_5_0";
    public const string eventMission3_1 = "em_5_1";
    public const string eventMission3_2 = "em_5_2";
    public const string eventMission3_3 = "em_5_3";
    public const string eventMission3_4 = "em_5_4";
    public const string eventMission3_5 = "em_5_5";
    public const string eventMission3_6 = "em_5_6";
    public const string eventMission3_7 = "em_5_7";
    public const string eventMission3_8 = "em_5_8";
    public const string eventMission3_9 = "em_5_9";

    public const string eventMission4_0 = "em_6_0";
    public const string eventMission4_1 = "em_6_1";
    public const string eventMission4_2 = "em_6_2";
    public const string eventMission4_3 = "em_6_3";
    public const string eventMission4_4 = "em_6_4";
    public const string eventMission4_5 = "em_6_5";

    public const string nickNameChange = "nickNameChange";
    public const string getPetHome = "gph";
    public const string dokebiPensionReset = "doke";
    public const string partyTowerRecommend = "partyTowerRec";
    public const string partyTowerFloor = "partyTowerFloor";
    public const string partyTowerFloor2 = "ptf2";

    public const string receivedPartyTowerTicket = "receivedPartyTowerTicket";
    public const string dailySleepRewardReceiveCount = "dss";

    public const string getFoxCup = "gfc";
    public const string getWolfRing = "gwr";

    public const string graduateSon = "GS";
    public const string graduateHel = "GH";
    public const string graduateChun = "graduateChun";
    public const string graduateGumSoul = "graduateGumSoul";
    public const string graduateNorigaeSoul = "graduateNorigaeSoul";
    public const string graduateDokebiFire = "graduateDokebiFire";
    public const string graduateEvilSeed = "graduateEvilSeed";
    public const string graduateGhostTree = "graduateGhostTree";
    public const string getMovingAutoAttack = "GMAA";
    public const string suhoAnimalStart = "Sast";
    public const string titleLevel = "titleLevel";
    public const string titleStage = "titleStage";
    public const string titleWeapon = "titleWeapon";
    public const string titleMagicBook = "titleMagicBook";
    public const string guildTowerStart = "gz0";
    public const string foxTowerStart = "fts";
    public const string sealSwordStart = "ssst";

    public const string gangchulRewardIdx = "gri";

    public double currentServerDate;
    public double attendanceUpdatedTime;
    public DateTime currentServerTime { get; private set; }
    public ReactiveCommand whenServerTimeUpdated = new ReactiveCommand();


    private Dictionary<string, double> tableSchema = new Dictionary<string, double>()
    {
        { Hp, 100f },
        { Mp, 100f },
        { LastMap, 0f },
        { LastLogin, 0f },
        { removeAd, 0f },
        { gachaNum_Weapon, 0f },
        { gachaNum_Norigae, 0f },
        { gachaNum_Skill, 0f },
        { gachaNum_NewGacha, 0f },
        { hackingCount, 0f },
        { passSelectedIdx, 0f },
        { currentFloorIdx, 0f },
        { currentFloorIdx2, 0f },
        { currentFloorIdx3, 0f },
        { currentFloorIdx4, 0f },
        { currentFloorIdx5, 0f },
        { currentFloorIdx6, 0f },
        { currentFloorIdx7, 0f },
        { currentFloorIdx9, 0f },
        { foxTowerIdx, 0f },
        { DarkTowerIdx, 0f },
        { foxFireIdx, -1f },
        { currentFloorGuildTower, 0f },
        { RoyalTombFloorIdx, 0f },
        { receiveReviewReward, 0f },
        { dailyEnemyKillCount, 0f },
        { dailyTicketBuyCount, 0f },
        { receivedTicketReward, 0f },
        { bonusDungeonEnterCount, 0f },
        { chatBan, 0f },
        { tutorialCurrentStep, 2f },
        { tutorialClearFlags, 0f },
        { managerDescriptionFlags, 0f },
        { attendanceCount, 0f },
        { marbleAwake, -1f },
        { resetStat, 0f },
        //버프
        { buff_gold1, 0f },
        { buff_gold2, 0f },
        { buff_exp1, 0f },
        { buff_exp2, 0f },

        { guild_buff0, 0f },
        { guild_buff1, 0f },
        { guild_buff2, 0f },
        { guild_buff3, 0f },
        { one_Buff, 0f },
        { mf11_Buff, 0f },
        { ma11_Buff, 0f },

        { mf12_Buff, 0f },
        { ma12_Buff, 0f },

        { season0_Buff, 0f },
        { season1_Buff, 0f },
        { season2_Buff, 0f },
        { season3_Buff, 0f },

        { winter0_Buff, 0f },
        { winter1_Buff, 0f },

        { bonusDungeonMaxKillCount, 0f },
        { wingPackageRewardReceive, 0f },
        { topClearStageId, -1f },
        { selectedSkillGroupId, 0f },
        { dokebiEnterCount, 0f },
        { dokebiNewEnterCount, 0f },
        { chatFrame, 0f },
        { hellMark, 0f },

        { freeWeapon, 0f },
        { freeNorigae, 0f },
        { freeSkill, 0f },
        { freeNewGacha, 0f },

        { dokebiKillCount3, 0f },

        { oakpensionAttendance, 0f },
        { marblepensionAttendance, 0f },
        { hellpensionAttendance, 0f },
        { chunpensionAttendance, 0f },
        { dokebipensionAttendance, 0f },
        { sumipensionAttendance, 0f },
        { ringpensionAttendance, 0f },
        { suhopetfeedclearpensionAttendance, 0f },
        { foxfirepension, 0f },
        { sealswordpension, 0f },
        { dosulpension, 0f },
        { guimoonpension, 0f },
        { meditationpension, 0f },
        { taeguekpension, 0f },
        { blacksoulpension, 0f },
        { studentspotpension, 0f },
        { transjeweltpension, 0f },
        { masterpension, 0f },

        { marblePackChange, 0f },
        { yoguiSogulLastClear, 0f },
        { oldDokebi2LastClear, 0f },
        { smithClear, 0f },
        { gumGiSoulClear, 0f },
        { smithTreeClear, 0f },
        { sonCloneClear, 0f },
        { gumGiClear, 0f },
        { flowerClear, 0f },
        { DokebiFireClear, 0f },
        { DayOfWeekClear, 0f },
        { getFoxCup, 0f },
        { getWolfRing, 0f },
        { graduateSon, 0f },
        { graduateHel, 0f },
        { graduateChun, 0f },
        { graduateGumSoul, 0f },
        { graduateNorigaeSoul, 0f },
        { graduateDokebiFire, 0f },
        { getMovingAutoAttack, 0f },
        { graduateEvilSeed, 0f },
        { graduateGhostTree, 0f },


        { yomul0_buff, 0f },
        { yomul1_buff, 0f },
        { yomul2_buff, 0f },
        { yomul3_buff, 0f },
        { relicKillCount, 0f },
        { hellRelicKillCount, 0f },
        { usedRelicTicketNum, 0f },
        { relicpensionAttendance, 0f },
        { yomul4_buff, 0f },

        { yomul5_buff, 0f },
        //{ killCountTotal2, 0f },
        { monthAttendCount, 1f },
        { killCountTotalChild, 0f },
        { killCountTotalWinterPass, 0f },
        { killCountTotalSeason, 0f },
        //{ killCountTotalSeason2, 0f },
        { killCountTotalSeason3, 0f },
        { attenCountBok, 1f },
        { attenCountSpring, 1f },
        { usedCollectionCount, 0f },
        { usedSnowManCollectionCount, 0f },
        { attenCountChuSeok, 1f },
        { attenCountSeason, 1f },
        { yomul6_buff, 0f },
        { sonScore, 0f },
        { hellWarScore, 0f },
        { catScore, 0f },
        { susanoScore, 0f },
        { norigaeScore, 0f },
        { gradeScore, 0f },
        { yumScore, 0f },
        { okScore, 0f },
        { doScore, 0f },
        { sumiScore, 0f },
        { thiefScore, 0f },
        { sleepRewardSavedTime, 0f },
        { yomul7_buff, 0f },
        { attendanceCount_100Day, 1f },
        { peachAttendance, 0f },
        { smithpensionAttendance, 0f },
        { weaponpensionAttendance, 0f },
        { buffAwake, 0f },
        { petAwake, 0f },
        { IgnoreDamDec, 0f },
        { SendGuildPoint, 0 },
        { cockAwake, 0 },
        { attendanceCount_Seol, 1 },
        { attendanceCount_Dol, 0 },
        { dogAwake, 0 },
        { basicPackRefund, 0 },
        { dolPassRefund, 0 },
        { skillInitialized, 0 },
        { smithExp, 0 },
        { getSmith, 0 },
        { getGumGi, 0 },
        { getSumiFire, 0 },
        { getFlower, 0 },
        { getDokebiFire, 0 },
        { getRingGoods, 0 },
        { getDayOfWeek, 0 },
        { getDokebiBundle, 0 },
        { sendPetExp, 0 },
        { collectionEventInitialize, 0 },
        { trainingEventInitialize0, 0 },
        { exchangeCount_0, 0 },
        { exchangeCount_1, 0 },
        { exchangeCount_2, 0 },
        { exchangeCount_3, 0 },
        { exchangeCount_4, 0 },

        { monthreset, 0 },
        { refundFox, 0 },
        { sendGangChul, 0 },
        { foxMask, 0 },
        { relicPackReset, 0 },
        { hellScore, 0 },
        { chunClear, 0 },
        { oneAttenEvent, 0 },
        { titleRefund, 0 },
        { oneAttenEvent_one, 0 },
        { relicReset, 0 },

        { canRecommendCount, GameBalance.recommendCountPerWeek },
        { mileageRefund, 0 },
        { RefundIdx, 0 },
        { marRelicRefund, 0 },
        { growthStoneRefund, 0 },
        { purchaseRefund0, 0 },
        { newGachaEnergyRefund, 0 },
        { titleConvertNewTitle, 0 },
        { titleConvertNewTitle2, 0 },
        { relocateLevelPass, 0 },
        { chunmaRefund, 0 },

        { exchangeCount_0_Mileage, 0 },
        { exchangeCount_1_Mileage, 0 },
        { exchangeCount_2_Mileage, 0 },
        { exchangeCount_3_Mileage, 0 },
        { exchangeCount_4_Mileage, 0 },

        { exchangeCount_5_Mileage, 0 },
        { exchangeCount_6_Mileage, 0 },
        { exchangeCount_7_Mileage, 0 },
        { exchangeCount_8_Mileage, 0 },
        { exchangeCount_9_Mileage, 0 },

        { exchangeCount_10_Mileage, 0 },
        { exchangeCount_11_Mileage, 0 },
        { exchangeCount_12_Mileage, 0 },
        { exchangeCount_13_Mileage, 0 },
        { exchangeCount_14_Mileage, 0 },
        { exchangeCount_15_Mileage, 0 },

        { exchangeCount_16_Mileage, 0 },
        { exchangeCount_17_Mileage, 0 },
        { exchangeCount_18_Mileage, 0 },
        { exchangeCount_19_Mileage, 0 },
        { exchangeCount_20_Mileage, 0 },
        { exchangeCount_21_Mileage, 0 },
        { exchangeCount_22_Mileage, 0 },
        { exchangeCount_23_Mileage, 0 },
        { exchangeCount_24_Mileage, 0 },
        { exchangeCount_25_Mileage, 0 },
        { exchangeCount_26_Mileage, 0 },
        { exchangeCount_27_Mileage, 0 },
        { exchangeCount_28_Mileage, 0 },
        { exchangeCount_29_Mileage, 0 },
        { exchangeCount_30_Mileage, 0 },
        { exchangeCount_31_Mileage, 0 },

        { ny_ex_0, 0 },
        { ny_ex_1, 0 },
        { ny_ex_2, 0 },
        { ny_ex_3, 0 },
        { ny_ex_4, 0 },
        { ny_ex_5, 0 },
        { eventMissionInitialize, 0 },

        { eventMission0_0, 0 },
        { eventMission0_1, 0 },
        { eventMission0_2, 0 },
        { eventMission0_3, 0 },
        { eventMission0_4, 0 },
        { eventMission0_5, 0 },
        { eventMission0_6, 0 },
        { eventMission0_7, 0 },
        { eventMission0_8, 0 },
        { eventMission0_9, 0 },

        { eventMission1_0, 0 },
        { eventMission1_1, 0 },
        { eventMission1_2, 0 },
        { eventMission1_3, 0 },
        { eventMission1_4, 0 },
        { eventMission1_5, 0 },

        { eventMission1_6, 0 },
        { eventMission1_7, 0 },
        { eventMission1_8, 0 },
        { eventMission1_9, 0 },
        { eventMission1_10, 0 },
        { eventMission1_11, 0 },

        { eventMission2_0, 0 },
        { eventMission2_1, 0 },
        { eventMission2_2, 0 },
        { eventMission2_3, 0 },
        { eventMission2_4, 0 },
        { eventMission2_5, 0 },

        { eventMission2_6, 0 },
        { eventMission2_7, 0 },
        { eventMission2_8, 0 },
        { eventMission2_9, 0 },
        { eventMission2_10, 0 },
        { eventMission2_11, 0 },

        { eventMission3_0, 0 },
        { eventMission3_1, 0 },
        { eventMission3_2, 0 },
        { eventMission3_3, 0 },
        { eventMission3_4, 0 },
        { eventMission3_5, 0 },
        { eventMission3_6, 0 },
        { eventMission3_7, 0 },
        { eventMission3_8, 0 },
        { eventMission3_9, 0 },

        { eventMission4_0, 0 },
        { eventMission4_1, 0 },
        { eventMission4_2, 0 },
        { eventMission4_3, 0 },
        { eventMission4_4, 0 },
        { eventMission4_5, 0 },

        { nickNameChange, 0 },
        { getPetHome, 0 },
        { dokebiPensionReset, 0 },
        { partyTowerRecommend, GameBalance.recommendCountPerWeek_PartyTower },
        { partyTowerFloor, 0 },
        { partyTowerFloor2, 0 },
        { receivedPartyTowerTicket, 0f },

        { snow_exchangeCount_0, 0f },
        { snow_exchangeCount_1, 0f },
        { snow_exchangeCount_2, 0f },
        { snow_exchangeCount_3, 0f },
        { snow_exchangeCount_4, 0f },
        { snow_exchangeCount_5, 0f },
        { dailySleepRewardReceiveCount, 0f },
        { sumiFireClear, 0f },
        { suhoAnimalStart, 0f },
        { dailybooty, 0f },
        { titleLevel, -1f },
        { titleStage, -1f },
        { titleWeapon, -1f },
        { titleMagicBook, -1f },
        { guildTowerStart, 0f },
        { foxTowerStart, 0f },
        { sealSwordStart, 0f },

        { gangchulRewardIdx, -1f },
    };

    private Dictionary<string, ReactiveProperty<double>> tableDatas = new Dictionary<string, ReactiveProperty<double>>();
    public Dictionary<string, ReactiveProperty<double>> TableDatas => tableDatas;

    public ReactiveProperty<double> GetTableData(string key)
    {
        return tableDatas[key];
    }

    public ReactiveCommand WhenDateChanged = new ReactiveCommand();

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

            //맨처음 초기화(캐릭터생성)
            if (rows.Count <= 0)
            {
                Param defultValues = new Param();

                using var e = tableSchema.GetEnumerator();

                while (e.MoveNext())
                {
                    if (e.Current.Key != LastLogin)
                    {
                        //소급코드들 
                        if (e.Current.Key == titleConvertNewTitle ||
                            e.Current.Key == titleConvertNewTitle2 ||
                            e.Current.Key == relocateLevelPass ||
                            e.Current.Key == dolPassRefund ||
                            e.Current.Key == mileageRefund ||
                            e.Current.Key == newGachaEnergyRefund ||
                            e.Current.Key == chunmaRefund ||
                            e.Current.Key == purchaseRefund0 ||
                            e.Current.Key == marRelicRefund ||
                            e.Current.Key == growthStoneRefund ||
                            e.Current.Key == basicPackRefund ||
                            e.Current.Key == refundFox ||
                            e.Current.Key == titleRefund ||
                            e.Current.Key == trainingEventInitialize0
                           )
                        {
                            defultValues.Add(e.Current.Key, 1);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(1));
                        }
                        else if (e.Current.Key == collectionEventInitialize)
                        {
                            defultValues.Add(e.Current.Key, 5);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(5));
                        }
                        else if (e.Current.Key == eventMissionInitialize)
                        {
                            var idx = 89;
                            defultValues.Add(e.Current.Key, idx);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(idx));
                        }
                        else if (e.Current.Key == RefundIdx)
                        {
                            defultValues.Add(e.Current.Key, 6);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(6));
                        }
                        else
                        {
                            defultValues.Add(e.Current.Key, e.Current.Value);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(e.Current.Value));
                        }
                    }
                    else
                    {
                        BackendReturnObject servertime = Backend.Utils.GetServerTime();

                        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
                        DateTime currentServerTime = DateTime.Parse(time).ToUniversalTime().AddHours(9);

                        currentServerDate = (double)Utils.ConvertToUnixTimestamp(currentServerTime);

                        defultValues.Add(e.Current.Key, (double)currentServerDate);
                        tableDatas.Add(e.Current.Key, new ReactiveProperty<double>((double)currentServerDate));
                    }
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

                using var e = tableSchema.GetEnumerator();

                for (int i = 0; i < data.Keys.Count; i++)
                {
                    while (e.MoveNext())
                    {
                        if (data.Keys.Contains(e.Current.Key))
                        {
                            //값로드
                            var value = data[e.Current.Key][ServerData.format_Number].ToString();

                            //기술슬롯 삭제되면서 0번으로 고정
                            if (e.Current.Key.Equals(selectedSkillGroupId))
                            {
                                value = "0";
                            }

                            tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(double.Parse(value)));
                        }
                        else
                        {
                            defultValues.Add(e.Current.Key, e.Current.Value);
                            tableDatas.Add(e.Current.Key, new ReactiveProperty<double>(e.Current.Value));

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
                        return; //
                    }
                }
            }
        });
    }

    public void UpData(string key, bool LocalOnly)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"UserInfoTable {key} is not exist");
            return;
        }

        UpData(key, tableDatas[key].Value, LocalOnly);
    }

    public void UpData(string key, double data, bool LocalOnly, Action failCallBack = null)
    {
        if (tableDatas.ContainsKey(key) == false)
        {
            Debug.Log($"UserInfoTable {key} is not exist");
            return;
        }

        tableDatas[key].Value = data;

        if (LocalOnly == false)
        {
            Param param = new Param();
            param.Add(key, tableDatas[key].Value);

            SendQueue.Enqueue(Backend.GameData.Update, tableName, Indate, param, e =>
            {
                if (e.IsSuccess() == false)
                {
                    failCallBack?.Invoke();
                    Debug.LogError($"UserInfoTable {key} up failed");
                    return;
                }
            });
        }
    }

    public void AutoUpdateRoutine()
    {
        UpdateLastLoginTime();
        UpdatekillCount();
    }

    private void UpdatekillCount()
    {
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userInfoParam = new Param();

        userInfoParam.Add(dailyEnemyKillCount, tableDatas[dailyEnemyKillCount].Value);


        userInfoParam.Add(dailybooty, tableDatas[dailybooty].Value);

        // if (ServerData.userInfoTable.IsMonthlyPass2() == false)
        // {
        // }
        // else
        // {
        //     userInfoParam.Add(killCountTotal2, tableDatas[killCountTotal2].Value);
        // }

        userInfoParam.Add(killCountTotalWinterPass, tableDatas[killCountTotalWinterPass].Value);

        userInfoParam.Add(killCountTotalSeason, tableDatas[killCountTotalSeason].Value);

        //userInfoParam.Add(killCountTotalSeason2, tableDatas[killCountTotalSeason2].Value);
        //수호
        userInfoParam.Add(killCountTotalSeason3, tableDatas[killCountTotalSeason3].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions);
    }

    private void UpdatejumpCount()
    {
        // UpData(jumpCount, false);
    }

    private bool isFirstInit = true;


    public void UpdateLastLoginTime()
    {
        SendQueue.Enqueue(Backend.Utils.GetServerTime, (bro) =>
        {
            var isSuccess = bro.IsSuccess();
            var statusCode = bro.GetStatusCode();
            var returnValue = bro.GetReturnValue();

            if (isSuccess && statusCode.Equals("200") && returnValue != null)
            {
                string time = bro.GetReturnValuetoJSON()["utcTime"].ToString();

                currentServerTime = DateTime.Parse(time).ToUniversalTime().AddHours(9);

#if UNITY_EDITOR
                currentServerTime = currentServerTime.ToUniversalTime().AddDays(GameBalance.addDay).AddMinutes(GameBalance.addMinutes);
                //currentServerTime = currentServerTime.AddDays(15);
#endif

                whenServerTimeUpdated.Execute();

                currentServerDate = (double)Utils.ConvertToUnixTimestamp(currentServerTime);

                //day check
                DateTime savedDate = Utils.ConvertFromUnixTimestamp(tableDatas[LastLogin].Value - 2f);

                if (isFirstInit)
                {
                    isFirstInit = false;
                    int elapsedTime = (int)(currentServerTime - savedDate).TotalSeconds;

                    //최소조건 안됨 (시간,첫 접속)
                    if (elapsedTime < GameBalance.sleepRewardMinValue || ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value == -1)
                    {
                        return;
                    }
                    else
                    {
                        //서버에 저장시켜봄
                        Param userInfoParam = new Param();

                        ServerData.userInfoTable.tableDatas[UserInfoTable.sleepRewardSavedTime].Value += elapsedTime;

                        userInfoParam.Add(sleepRewardSavedTime, ServerData.userInfoTable.tableDatas[UserInfoTable.sleepRewardSavedTime].Value);

                        var returnBro = Backend.GameData.Update(tableName, Indate, userInfoParam);

                        if (returnBro.IsSuccess() == false)
                        {
                            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "네트워크가 불안정 합니다.\n앱을 재실행 합니다.", () => { Utils.RestartApplication(); });

                            return;
                        }
                    }
                }

                //week check
                int currentWeek = Utils.GetWeekNumber(currentServerTime);

                int savedWeek = Utils.GetWeekNumber(savedDate);

                if (savedDate.Month != currentServerTime.Month || savedDate.Day != currentServerTime.Day)
                {
                    Debug.LogError("@@@Day Changed!");
                    if (savedWeek != currentWeek)
                    {
                        Debug.LogError("@@@Week Changed!");
                    }

                    if (savedDate.Month != currentServerTime.Month)
                    {
                        Debug.LogError("@@@Month Changed!");
                    }

                    //날짜 바뀜
                    DateChanged(currentServerTime.Day, savedWeek != currentWeek,
                        savedDate.Month != currentServerTime.Month);
                    attendanceUpdatedTime = currentServerTime.Day;
                }
                else
                {
                    UpdateLastLoginOnly();
                }
            }
            else
            {
                // LogManager.Instance.SendLog("출석", $"{isSuccess}/{statusCode}/{returnValue}");
            }
        });
    }

    private void UpdateLastLoginOnly()
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value = (double)currentServerDate;

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.LastLogin, Math.Truncate(ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value));
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactionList, true);
    }


    private void DateChanged(int day, bool weekChanged, bool monthChanged)
    {
        WhenDateChanged.Execute();

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //
        var table = TableManager.Instance.EventMission.dataArray;

        Param eventMissionParam = new Param();
        for (int i = 0; i < table.Length; i++)
        {
            //주간미션은 초기화안함.
            if (table[i].EVENTMISSIONTYPE == EventMissionType.FIRST) continue;
            if (table[i].EVENTMISSIONTYPE == EventMissionType.AFIRST) continue;
            if (table[i].EVENTMISSIONTYPE == EventMissionType.FINISHMARBLE) continue;
            if (table[i].EVENTMISSIONTYPE == EventMissionType.THIRD) continue;
            if (table[i].EVENTMISSIONTYPE == EventMissionType.ANNIVERSARY) continue;
            ServerData.eventMissionTable.TableDatas[table[i].Stringid].clearCount.Value = 0;
            ServerData.eventMissionTable.TableDatas[table[i].Stringid].rewardCount.Value = 0;

            eventMissionParam.Add(table[i].Stringid, ServerData.eventMissionTable.TableDatas[table[i].Stringid].ConvertToString());
        }


        //
        ClearDailyMission();


        BattleContestData.ClearData();
        
        //일일초기화
        Param dailyPassParam = new Param();
        Param userInfoParam = new Param();
        Param userInfo2Param = new Param();
        Param equipmentParam = new Param();
        Param dimensionStatusParam = new Param();

        ServerData.dailyPassServerTable.ResetDailyPassLocal();
        dailyPassParam.Add(DailyPassServerTable.DailypassFreeReward, ServerData.dailyPassServerTable.TableDatas[DailyPassServerTable.DailypassFreeReward].Value);
        dailyPassParam.Add(DailyPassServerTable.DailypassAdReward, ServerData.dailyPassServerTable.TableDatas[DailyPassServerTable.DailypassAdReward].Value);
        transactionList.Add(TransactionValue.SetUpdate(DailyPassServerTable.tableName, DailyPassServerTable.Indate, dailyPassParam));


        //일일초기화
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value = 0;

        ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.freeNorigae).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.freeSkill).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.freeNewGacha).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.SendGuildPoint).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.sendGangChul).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.getSmith).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.sendPetExp).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.getGumGi).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.oneAttenEvent).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.getFlower).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.getPetHome).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.dailySleepRewardReceiveCount).Value = 0;

        //버프
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold1).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold2).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp1).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp2).Value = 0;

        ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff0).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff1).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff2).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff3).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.one_Buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.mf11_Buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.ma11_Buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.mf12_Buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.ma12_Buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.season0_Buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.season1_Buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.season2_Buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.season3_Buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.winter0_Buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.winter1_Buff).Value = 0;

        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul0_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul1_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul2_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul3_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul4_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul5_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul6_buff).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.yomul7_buff).Value = 0;

        //

        ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value = (double)currentServerDate;

        
        //월간 초기화
        if (monthChanged)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.nickNameChange).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.monthAttendCount).Value = 0;

            #region DimensionMonthInit

            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionGrade).Value = 0;
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dimensionSpecialRewardIdx).Value = -1;
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.currentDimensionSeasonIdx).Value = Utils.GetCurrentDimensionSeasonData().Id;
            
            userInfo2Param.Add(UserInfoTable_2.dimensionGrade, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dimensionGrade].Value);
            userInfo2Param.Add(UserInfoTable_2.dimensionSpecialRewardIdx, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dimensionSpecialRewardIdx].Value);
            userInfo2Param.Add(UserInfoTable_2.currentDimensionSeasonIdx, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.currentDimensionSeasonIdx].Value);
 
            ServerData.equipmentTable.TableDatas[EquipmentTable.DimensionEquipment].Value = 0;
            equipmentParam.Add(EquipmentTable.DimensionEquipment, ServerData.equipmentTable.TableDatas[EquipmentTable.DimensionEquipment].Value);
            transactionList.Add(TransactionValue.SetUpdate(EquipmentTable.tableName, EquipmentTable.Indate, equipmentParam));

            ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.Level).Value = 0;
            ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value = 0;
            var dsData = TableManager.Instance.DimensionStatus.dataArray;

            for (int i = 0; i < dsData.Length; i++)
            {
                if (dsData[i].STATUSWHERE == StatusWhere.dimension)
                {
                    ServerData.dimensionStatusTable.GetTableData(dsData[i].Key).Value = 0;
                    dimensionStatusParam.Add(dsData[i].Key , ServerData.dimensionStatusTable.GetTableData(dsData[i].Key).Value);
                }
            }
            
            ServerData.goodsTable.GetTableData(GoodsTable.DCT).Value = 0;
            ServerData.goodsTable.GetTableData(GoodsTable.DE).Value = 0;
            dimensionStatusParam.Add(DimensionStatusTable.Level, ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.Level).Value);
            dimensionStatusParam.Add(DimensionStatusTable.DSP, ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value);
            transactionList.Add(TransactionValue.SetUpdate(DimensionStatusTable.tableName, DimensionStatusTable.Indate, dimensionStatusParam));

            #endregion

            //월간 훈련 보상

            //홀수면 트루
            if (IsMonthlyPass2())
            {
                //홀수
                Param monthpass2Param = new Param();

                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.oddMonthKillCount).Value = 0;

                ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassFreeReward].Value = "-1";
                ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAdReward].Value = "-1";
                ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAttendFreeReward].Value = "-1";
                ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAttendAdReward].Value = "-1";
                
                monthpass2Param.Add(MonthlyPassServerTable2.MonthlypassFreeReward, ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassFreeReward].Value);
                monthpass2Param.Add(MonthlyPassServerTable2.MonthlypassAdReward, ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAdReward].Value);
                monthpass2Param.Add(MonthlyPassServerTable2.MonthlypassAttendFreeReward, ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAttendFreeReward].Value);
                monthpass2Param.Add(MonthlyPassServerTable2.MonthlypassAttendAdReward, ServerData.monthlyPassServerTable2.TableDatas[MonthlyPassServerTable2.MonthlypassAttendAdReward].Value);

                transactionList.Add(TransactionValue.SetUpdate(MonthlyPassServerTable2.tableName, MonthlyPassServerTable2.Indate, monthpass2Param));

                userInfo2Param.Add(UserInfoTable_2.oddMonthKillCount, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.oddMonthKillCount].Value);
            }
            else
            {
                //짝수
                Param monthpassParam = new Param();

                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.evenMonthKillCount).Value = 0;
                
                ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassFreeReward].Value = "-1";
                ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAdReward].Value = "-1";
                ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAttendFreeReward].Value = "-1";
                ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAttendAdReward].Value = "-1";


                monthpassParam.Add(MonthlyPassServerTable.MonthlypassFreeReward, ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassFreeReward].Value);
                monthpassParam.Add(MonthlyPassServerTable.MonthlypassAdReward, ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAdReward].Value);
                monthpassParam.Add(MonthlyPassServerTable.MonthlypassAttendFreeReward, ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAttendFreeReward].Value);
                monthpassParam.Add(MonthlyPassServerTable.MonthlypassAttendAdReward, ServerData.monthlyPassServerTable.TableDatas[MonthlyPassServerTable.MonthlypassAttendAdReward].Value);

                transactionList.Add(TransactionValue.SetUpdate(MonthlyPassServerTable.tableName, MonthlyPassServerTable.Indate, monthpassParam));

                userInfo2Param.Add(UserInfoTable_2.evenMonthKillCount, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.evenMonthKillCount].Value);

            }

            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.GangChulReset].Value = 0;

            userInfo2Param.Add(UserInfoTable_2.GangChulReset, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.GangChulReset].Value);
        }

        transactionList.Add(TransactionValue.SetUpdate(EventMissionTable.tableName, EventMissionTable.Indate, eventMissionParam));

        //두번타는거 방지
        if (attendanceUpdatedTime != day)
        {
            if (ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value != 0)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value++;
            }

            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_100Day).Value++;
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Seol).Value++;
            ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountBok).Value++;
            ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountChuSeok).Value++;
            ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountSeason).Value++;
            ServerData.userInfoTable.GetTableData(UserInfoTable.monthAttendCount).Value++;

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.oakpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.oakpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.marblepensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.marblepensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.hellpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.hellpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.chunpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.chunpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.dokebipensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.dokebipensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.sumipensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.sumipensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.ringpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.ringpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.suhopetfeedclearpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.suhopetfeedclearpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.foxfirepension].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.foxfirepension).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.sealswordpension].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.sealswordpension).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.dosulpension].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.dosulpension).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.guimoonpension].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.guimoonpension).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.meditationpension].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.meditationpension).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.taeguekpension].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.taeguekpension).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.blacksoulpension].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.blacksoulpension).Value++;
            }
            if (ServerData.iapServerTable.TableDatas[UserInfoTable.studentspotpension].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.studentspotpension).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.transjeweltpension].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.transjeweltpension).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.masterpension].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.masterpension).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.relicpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.relicpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.peachAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.peachAttendance).Value++;
            }


            if (ServerData.iapServerTable.TableDatas[UserInfoTable.smithpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.smithpensionAttendance).Value++;
            }

            if (ServerData.iapServerTable.TableDatas[UserInfoTable.weaponpensionAttendance].buyCount.Value > 0f)
            {
                ServerData.userInfoTable.GetTableData(UserInfoTable.weaponpensionAttendance).Value++;
            }
            
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventMission1AttendCount).Value++;
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.commonAttend2Count).Value++;
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventMission2AttendCount).Value++;
            //신년 출석 이벤트
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventAttendCount).Value++;
            

        }

        attendanceUpdatedTime = ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value;

        userInfoParam.Add(UserInfoTable.dailyEnemyKillCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).Value);
        userInfoParam.Add(UserInfoTable.dailyTicketBuyCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dailyTicketBuyCount).Value);
        userInfoParam.Add(UserInfoTable.receivedTicketReward, ServerData.userInfoTable.GetTableData(UserInfoTable.receivedTicketReward).Value);
        userInfoParam.Add(UserInfoTable.bonusDungeonEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.bonusDungeonEnterCount).Value);
        userInfoParam.Add(UserInfoTable.dokebiEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiEnterCount).Value);
        userInfoParam.Add(UserInfoTable.dokebiNewEnterCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebiNewEnterCount).Value);
        userInfoParam.Add(UserInfoTable.LastLogin, Math.Truncate(ServerData.userInfoTable.GetTableData(UserInfoTable.LastLogin).Value));
        userInfoParam.Add(UserInfoTable.attendanceCount, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value);
        userInfoParam.Add(UserInfoTable.attendanceCount_100Day, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_100Day).Value);
        userInfoParam.Add(UserInfoTable.attendanceCount_Seol, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Seol).Value);
        userInfoParam.Add(UserInfoTable.attenCountBok, ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountBok).Value);
        userInfoParam.Add(UserInfoTable.attenCountChuSeok, ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountChuSeok).Value);
        userInfoParam.Add(UserInfoTable.attenCountSeason, ServerData.userInfoTable.GetTableData(UserInfoTable.attenCountSeason).Value);
        userInfoParam.Add(UserInfoTable.monthAttendCount, ServerData.userInfoTable.GetTableData(UserInfoTable.monthAttendCount).Value);

        userInfoParam.Add(UserInfoTable.oakpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.oakpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.marblepensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.marblepensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.relicpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.relicpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.peachAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.peachAttendance).Value);
        userInfoParam.Add(UserInfoTable.smithpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.smithpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.weaponpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.weaponpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.hellpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.hellpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.chunpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.chunpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.dokebipensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.dokebipensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.sumipensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.sumipensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.ringpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.ringpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.suhopetfeedclearpensionAttendance, ServerData.userInfoTable.GetTableData(UserInfoTable.suhopetfeedclearpensionAttendance).Value);
        userInfoParam.Add(UserInfoTable.foxfirepension, ServerData.userInfoTable.GetTableData(UserInfoTable.foxfirepension).Value);
        userInfoParam.Add(UserInfoTable.sealswordpension, ServerData.userInfoTable.GetTableData(UserInfoTable.sealswordpension).Value);
        userInfoParam.Add(UserInfoTable.dosulpension, ServerData.userInfoTable.GetTableData(UserInfoTable.dosulpension).Value);
        userInfoParam.Add(UserInfoTable.guimoonpension, ServerData.userInfoTable.GetTableData(UserInfoTable.guimoonpension).Value);
        userInfoParam.Add(UserInfoTable.meditationpension, ServerData.userInfoTable.GetTableData(UserInfoTable.meditationpension).Value);
        userInfoParam.Add(UserInfoTable.taeguekpension, ServerData.userInfoTable.GetTableData(UserInfoTable.taeguekpension).Value);
        userInfoParam.Add(UserInfoTable.blacksoulpension, ServerData.userInfoTable.GetTableData(UserInfoTable.blacksoulpension).Value);
        userInfoParam.Add(UserInfoTable.studentspotpension, ServerData.userInfoTable.GetTableData(UserInfoTable.studentspotpension).Value);
        userInfoParam.Add(UserInfoTable.transjeweltpension, ServerData.userInfoTable.GetTableData(UserInfoTable.transjeweltpension).Value);
        userInfoParam.Add(UserInfoTable.masterpension, ServerData.userInfoTable.GetTableData(UserInfoTable.masterpension).Value);


        userInfoParam.Add(UserInfoTable.freeWeapon, ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Value);
        userInfoParam.Add(UserInfoTable.freeNorigae, ServerData.userInfoTable.GetTableData(UserInfoTable.freeNorigae).Value);
        userInfoParam.Add(UserInfoTable.freeSkill, ServerData.userInfoTable.GetTableData(UserInfoTable.freeSkill).Value);
        userInfoParam.Add(UserInfoTable.freeNewGacha, ServerData.userInfoTable.GetTableData(UserInfoTable.freeNewGacha).Value);
        userInfoParam.Add(UserInfoTable.SendGuildPoint, ServerData.userInfoTable.GetTableData(UserInfoTable.SendGuildPoint).Value);
        userInfoParam.Add(UserInfoTable.sendGangChul, ServerData.userInfoTable.GetTableData(UserInfoTable.sendGangChul).Value);
        userInfoParam.Add(UserInfoTable.getSmith, ServerData.userInfoTable.GetTableData(UserInfoTable.getSmith).Value);
        userInfoParam.Add(UserInfoTable.getFlower, ServerData.userInfoTable.GetTableData(UserInfoTable.getFlower).Value);
        userInfoParam.Add(UserInfoTable.getDokebiFire, ServerData.userInfoTable.GetTableData(UserInfoTable.getDokebiFire).Value);
        userInfoParam.Add(UserInfoTable.getRingGoods, ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value);
        userInfoParam.Add(UserInfoTable.getSumiFire, ServerData.userInfoTable.GetTableData(UserInfoTable.getSumiFire).Value);
        userInfoParam.Add(UserInfoTable.getGumGi, ServerData.userInfoTable.GetTableData(UserInfoTable.getGumGi).Value);
        userInfoParam.Add(UserInfoTable.getDayOfWeek, ServerData.userInfoTable.GetTableData(UserInfoTable.getDayOfWeek).Value);
        userInfoParam.Add(UserInfoTable.sendPetExp, ServerData.userInfoTable.GetTableData(UserInfoTable.sendPetExp).Value);
        userInfoParam.Add(UserInfoTable.oneAttenEvent, ServerData.userInfoTable.GetTableData(UserInfoTable.oneAttenEvent).Value);
        userInfoParam.Add(UserInfoTable.getPetHome, ServerData.userInfoTable.GetTableData(UserInfoTable.getPetHome).Value);
        userInfoParam.Add(UserInfoTable.dailySleepRewardReceiveCount, ServerData.userInfoTable.GetTableData(UserInfoTable.dailySleepRewardReceiveCount).Value);

        userInfoParam.Add(UserInfoTable.buff_gold1, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold1).Value);
        userInfoParam.Add(UserInfoTable.buff_gold2, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_gold2).Value);
        userInfoParam.Add(UserInfoTable.buff_exp1, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp1).Value);
        userInfoParam.Add(UserInfoTable.buff_exp2, ServerData.userInfoTable.GetTableData(UserInfoTable.buff_exp2).Value);

        userInfoParam.Add(UserInfoTable.guild_buff0, ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff0).Value);
        userInfoParam.Add(UserInfoTable.guild_buff1, ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff1).Value);
        userInfoParam.Add(UserInfoTable.guild_buff2, ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff2).Value);
        userInfoParam.Add(UserInfoTable.guild_buff3, ServerData.userInfoTable.GetTableData(UserInfoTable.guild_buff3).Value);
        userInfoParam.Add(UserInfoTable.one_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.one_Buff).Value);
        userInfoParam.Add(UserInfoTable.mf11_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.mf11_Buff).Value);
        userInfoParam.Add(UserInfoTable.ma11_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.ma11_Buff).Value);
        userInfoParam.Add(UserInfoTable.mf12_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.mf12_Buff).Value);
        userInfoParam.Add(UserInfoTable.ma12_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.ma12_Buff).Value);
        userInfoParam.Add(UserInfoTable.season0_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.season0_Buff).Value);
        userInfoParam.Add(UserInfoTable.season1_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.season1_Buff).Value);
        userInfoParam.Add(UserInfoTable.season2_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.season2_Buff).Value);
        userInfoParam.Add(UserInfoTable.season3_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.season3_Buff).Value);
        userInfoParam.Add(UserInfoTable.winter0_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.winter0_Buff).Value);
        userInfoParam.Add(UserInfoTable.winter1_Buff, ServerData.userInfoTable.GetTableData(UserInfoTable.winter1_Buff).Value);

        userInfoParam.Add(UserInfoTable.yomul0_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul0_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul1_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul1_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul2_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul2_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul3_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul3_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul4_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul4_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul5_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul5_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul6_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul6_buff).Value);
        userInfoParam.Add(UserInfoTable.yomul7_buff, ServerData.userInfoTable.GetTableData(UserInfoTable.yomul7_buff).Value);

        userInfo2Param.Add(UserInfoTable_2.eventMission1AttendCount, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.eventMission1AttendCount].Value);
        userInfo2Param.Add(UserInfoTable_2.commonAttend2Count, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.commonAttend2Count].Value);
        userInfo2Param.Add(UserInfoTable_2.eventMission2AttendCount, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.eventMission2AttendCount].Value);
        userInfo2Param.Add(UserInfoTable_2.eventAttendCount, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.eventAttendCount].Value);

        if (monthChanged)
        {
            userInfoParam.Add(UserInfoTable.nickNameChange, ServerData.userInfoTable.GetTableData(UserInfoTable.nickNameChange).Value);
        }

        //채팅 테두리 초기화
        if (weekChanged)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value = 0f;
            userInfoParam.Add(UserInfoTable.chatFrame, ServerData.userInfoTable.GetTableData(UserInfoTable.chatFrame).Value);


            ServerData.userInfoTable.GetTableData(UserInfoTable.hellMark).Value = 0f;
            userInfoParam.Add(UserInfoTable.hellMark, ServerData.userInfoTable.GetTableData(UserInfoTable.hellMark).Value);

            //추천권 초기화
            ServerData.userInfoTable.GetTableData(UserInfoTable.canRecommendCount).Value = GameBalance.recommendCountPerWeek;
            userInfoParam.Add(UserInfoTable.canRecommendCount, ServerData.userInfoTable.GetTableData(UserInfoTable.canRecommendCount).Value);

            ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerRecommend).Value = GameBalance.recommendCountPerWeek_PartyTower;
            userInfoParam.Add(UserInfoTable.partyTowerRecommend, ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerRecommend).Value);

            //십만동굴 ad초기화
            ServerData.userInfoTable.GetTableData(UserInfoTable.receivedPartyTowerTicket).Value = 0;
            userInfoParam.Add(UserInfoTable.receivedPartyTowerTicket, ServerData.userInfoTable.GetTableData(UserInfoTable.receivedPartyTowerTicket).Value);

            //마일리지 상점 초기화
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_0_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_1_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_2_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_3_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_4_Mileage).Value = 0;
            //마일리지 상점 초기화
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_5_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_6_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_7_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_8_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_9_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_10_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_11_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_12_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_13_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_14_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_15_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_16_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_17_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_18_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_19_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_20_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_21_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_22_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_23_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_24_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_25_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_26_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_27_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_28_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_29_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_30_Mileage).Value = 0;
            ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_31_Mileage).Value = 0;

                
            userInfoParam.Add(UserInfoTable.exchangeCount_0_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_0_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_1_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_1_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_2_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_2_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_3_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_3_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_4_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_4_Mileage).Value);

            userInfoParam.Add(UserInfoTable.exchangeCount_5_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_5_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_6_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_6_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_7_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_7_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_8_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_8_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_9_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_9_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_10_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_10_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_11_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_11_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_12_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_12_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_13_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_13_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_14_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_14_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_15_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_15_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_16_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_16_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_17_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_17_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_18_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_18_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_19_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_19_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_20_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_20_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_21_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_21_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_22_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_22_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_23_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_23_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_24_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_24_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_25_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_25_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_26_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_26_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_27_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_27_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_28_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_28_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_29_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_29_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_30_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_30_Mileage).Value);
            userInfoParam.Add(UserInfoTable.exchangeCount_31_Mileage, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_31_Mileage).Value);
            
            
        }

        Param iapParam = null;

        var iapTable = TableManager.Instance.InAppPurchase.dataArray;

        for (int i = 0; i < iapTable.Length; i++)
        {
            bool isDayChagned = (iapTable[i].BUYTYPE == BuyType.DayOfOne || iapTable[i].BUYTYPE == BuyType.DayOfFive);
            bool isWeekChagned = weekChanged == true && (iapTable[i].BUYTYPE == BuyType.WeekOfTwo || iapTable[i].BUYTYPE == BuyType.WeekOfFive || iapTable[i].BUYTYPE == BuyType.WeekOfOne);
            bool isMonthChanged = monthChanged == true && (iapTable[i].BUYTYPE == BuyType.MonthOfOne || iapTable[i].BUYTYPE == BuyType.MonthOfFive || iapTable[i].BUYTYPE == BuyType.MonthOfTen);

            if (isDayChagned || isWeekChagned || isMonthChanged)
            {
                if (iapParam == null)
                {
                    iapParam = new Param();
                }

                ServerData.iapServerTable.TableDatas[iapTable[i].Productid].buyCount.Value = 0;
                iapParam.Add(iapTable[i].Productid, ServerData.iapServerTable.TableDatas[iapTable[i].Productid].ConvertToString());
            }
        }

        if (iapParam != null)
        {
            transactionList.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));
        }

        //티켓
        ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += GameBalance.DailyRelicTicketGetCount;

        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateBackGui).Value > 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value += GameBalance.BackguiGraduateGainSoulKey;
        }


        //주사위
        ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value += GameBalance.DailyEventDiceGetCount;
        

        Param goodsParam = new Param();

        //엘릭서
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value >= GameBalance.TaegeukSimbeopUnlockStage)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.TaeguekElixir).Value += GameBalance.DailyElixirGetCount;
            goodsParam.Add(GoodsTable.TaeguekElixir, ServerData.goodsTable.GetTableData(GoodsTable.TaeguekElixir).Value);
        }
        goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
        goodsParam.Add(GoodsTable.EventDice, ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value);

        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value >= 3000000)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxClear).Value += GameBalance.DailyBlackFoxClearGetCount;
            goodsParam.Add(GoodsTable.BlackFoxClear, ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxClear).Value);
        }
        
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value >= 1000000)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.ByeolhoClear).Value += GameBalance.DailyByeolhoClearGetCount;
            goodsParam.Add(GoodsTable.ByeolhoClear, ServerData.goodsTable.GetTableData(GoodsTable.ByeolhoClear).Value);
        }
        
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.suhoAnimalStart].Value != 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeedClear).Value += GameBalance.DailyPetFeedClearGetValue;
        }

        //여우굴 소탕권
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.foxTowerStart].Value != 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.FoxRelicClearTicket).Value += GameBalance.FoxTowerTicketDailyGetAmount;
        }

        //귀문
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.guimoonRelicStart].Value != 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelicClearTicket).Value += GameBalance.GuimoonTicketDailyGetAmount;
        }

        //귀문
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationStart].Value != 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.MeditationClearTicket).Value += GameBalance.MeditationTicketDailyGetAmount;
        }
        //봉인검
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.sealSwordStart].Value != 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.SealWeaponClear).Value += GameBalance.SealSwordTicketDailyGetAmount;
        }
        
        ServerData.goodsTable.GetTableData(GoodsTable.GT).Value += GameBalance.GachaTicketDailyGetAmount;
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value >= 18998)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.SC).Value += GameBalance.SinsuClearDailyGetAmount;
        }

        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value >= 3000000)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.HYC).Value += GameBalance.HyulClearDailyGetAmount;
            
            if (currentServerTime.Day!=1)
            {
                var amount = GameBalance.DCTDailyGetAmount;

                if (ServerData.iapServerTable.TableDatas[Utils.GetCurrentDimensionSeasonData().Productid].buyCount.Value > 0)
                {
                    amount += GameBalance.DCTDailyPassGetAmount;
                }
                ServerData.goodsTable.GetTableData(GoodsTable.DCT).Value += amount;
                LogManager.Instance.SendLogType("DCT","gain",$"{amount}");

            }

        }

        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value >= 3500000)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.TJCT).Value += GameBalance.TJCTDailyGetAmount;
            goodsParam.Add(GoodsTable.TJCT, ServerData.goodsTable.GetTableData(GoodsTable.TJCT).Value);
        }

        //도술
        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulStart].Value != 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DosulClear).Value += GameBalance.dailyDosulClearTicketGetValue;
        }
        var rewardData = TableManager.Instance.MonthReward.dataArray;

        #region MonthPassReward
        //홀수월+패스구매시
        if (ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton2.monthPassKey].buyCount.Value > 0 && IsMonthlyPass2() == true)
        {
            foreach (var t in rewardData)
            {
                if (t.Monthsort != true) continue;
                ServerData.goodsTable.GetTableData((Item_Type)t.Itemtype).Value += t.Itemvalue;
            }
        }

        //짝수월+패스구매시
        if (ServerData.iapServerTable.TableDatas[UiMonthPassBuyButton.monthPassKey].buyCount.Value > 0 && IsMonthlyPass2() == false)
        {
            foreach (var t in rewardData)
            {
                if (t.Monthsort != false) continue;
                ServerData.goodsTable.GetTableData((Item_Type)t.Itemtype).Value += t.Itemvalue;
            }
        }
        #endregion
  


        //문파 소탕권
        if (tableDatas[guildTowerStart].Value != 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GuildTowerClearTicket).Value++;
            goodsParam.Add(GoodsTable.GuildTowerClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.GuildTowerClearTicket).Value);
        }
        //

        //길드보상 초기화
        ServerData.bossServerTable.TableDatas["boss12"].rewardedId.Value = string.Empty;
        //ServerData.bossServerTable.TableDatas["boss20"].rewardedId.Value = string.Empty; //강철이 초기화 x
        ServerData.bossServerTable.TableDatas["b73"].rewardedId.Value = string.Empty;


        Param bossParam = new Param();

        bossParam.Add("boss12", ServerData.bossServerTable.TableDatas["boss12"].ConvertToString());
        //bossParam.Add("boss20", ServerData.bossServerTable.TableDatas["boss20"].ConvertToString()); //강철이 초기화 x
        bossParam.Add("b73", ServerData.bossServerTable.TableDatas["b73"].ConvertToString());

        if (weekChanged)
        {
            //천마보상주간초기화
            ServerData.bossServerTable.TableDatas["b55"].rewardedId.Value = string.Empty;
            bossParam.Add("b55", ServerData.bossServerTable.TableDatas["b55"].ConvertToString());
        }


        //요괴소굴
        Param yoguiSogulParam = new Param();

        //손오공
        ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value = string.Empty;
        ServerData.etcServerTable.TableDatas[EtcServerTable.hellReward].Value = string.Empty;
        ServerData.etcServerTable.TableDatas[EtcServerTable.AdReward].Value = string.Empty;
        yoguiSogulParam.Add(EtcServerTable.sonReward, ServerData.etcServerTable.TableDatas[EtcServerTable.sonReward].Value);
        yoguiSogulParam.Add(EtcServerTable.hellReward, ServerData.etcServerTable.TableDatas[EtcServerTable.hellReward].Value);
        yoguiSogulParam.Add(EtcServerTable.AdReward, ServerData.etcServerTable.TableDatas[EtcServerTable.AdReward].Value);

        //로컬
        ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value = string.Empty;
        yoguiSogulParam.Add(EtcServerTable.yoguiSogulReward, ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value);

        ServerData.etcServerTable.TableDatas[EtcServerTable.oldDokebi2Reward].Value = string.Empty;
        yoguiSogulParam.Add(EtcServerTable.oldDokebi2Reward, ServerData.etcServerTable.TableDatas[EtcServerTable.oldDokebi2Reward].Value);

        ServerData.etcServerTable.TableDatas[EtcServerTable.guildAttenReward].Value = string.Empty;
        yoguiSogulParam.Add(EtcServerTable.guildAttenReward, ServerData.etcServerTable.TableDatas[EtcServerTable.guildAttenReward].Value);


        var curSeason = Utils.GetCurrentSeasonSpecialRequestData();

        var userSeasonData= (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.currentSeasonIdx).Value;
        //특별의뢰 시즌 변화!!
        if(curSeason.Id!=userSeasonData)
        {
            Param specialRequestParam = new Param();
            
            for (int i = 0; i < curSeason.Stringid.Length; i++)
            {
                ServerData.specialRequestBossServerTable.TableDatas[curSeason.Stringid[i]].score.Value = -1;
                ServerData.specialRequestBossServerTable.TableDatas[curSeason.Stringid[i]].isRewarded.Value = 0;
                specialRequestParam.Add(curSeason.Stringid[i], ServerData.specialRequestBossServerTable.TableDatas[curSeason.Stringid[i]].ConvertToString());
            }
            transactionList.Add(TransactionValue.SetUpdate(SpecialRequestBossServerTable.tableName, SpecialRequestBossServerTable.Indate, specialRequestParam));

            var exchangeData = TableManager.Instance.SpecialRequestExchangeTable.dataArray;

            List<string> keys =  new List<string>();
            for (int i = 0; i < exchangeData.Length; i++)
            {
                if (string.IsNullOrEmpty(exchangeData[i].Exchangekey)) continue;
                if (keys.Contains(exchangeData[i].Exchangekey) == false)
                {
                    keys.Add(exchangeData[i].Exchangekey);
                }
            }

            using var e = keys.GetEnumerator();

            while (e.MoveNext())
            {
                if (e.Current != null)
                {
                    ServerData.userInfoTable_2.GetTableData(e.Current).Value = 0;
                    userInfo2Param.Add(e.Current, ServerData.userInfoTable_2.TableDatas[e.Current].Value);
                }
            }

            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestTotalRewardIdx).Value = -1;
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestSpecialRewardIdx).Value = -1;
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.currentSeasonIdx).Value = curSeason.Id;
            
            userInfo2Param.Add(UserInfoTable_2.specialRequestTotalRewardIdx, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.specialRequestTotalRewardIdx].Value);
            userInfo2Param.Add(UserInfoTable_2.specialRequestSpecialRewardIdx, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.specialRequestSpecialRewardIdx].Value);
            userInfo2Param.Add(UserInfoTable_2.currentSeasonIdx, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.currentSeasonIdx].Value);

            Debug.LogError("특별의뢰 시즌 변화@@@@@@");
        }
        

        //주간초기화
        if (weekChanged)
        {
            ServerData.bossServerTable.TableDatas["b53"].rewardedId.Value = string.Empty;
            bossParam.Add("b53", ServerData.bossServerTable.TableDatas["b53"].ConvertToString());

            //문파 타워보상
            ServerData.bossServerTable.TableDatas["b117"].rewardedId.Value = string.Empty;
            bossParam.Add("b117", ServerData.bossServerTable.TableDatas["b117"].ConvertToString());

            goodsParam.Add(GoodsTable.DokebiFireKey, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFireKey).Value);


            
            
            ServerData.etcServerTable.TableDatas[EtcServerTable.chunmaTopScore].Value = string.Empty;

            yoguiSogulParam.Add(EtcServerTable.chunmaTopScore, ServerData.etcServerTable.TableDatas[EtcServerTable.chunmaTopScore].Value);

            //초월던전 소탕권
            if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.transTowerStart].Value != 0)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.TransClearTicket).Value += GameBalance.TransTicketWeeklyGetAmount;
                goodsParam.Add(GoodsTable.TransClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.TransClearTicket).Value);
            }

            if (ServerData.statusTable.GetTableData(StatusTable.Level).Value >= GameBalance.WeeklyBattleClearLevelLimit)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.BattleClear).Value += GameBalance.WeeklyBattleClearGetCount;
                goodsParam.Add(GoodsTable.BattleClear,ServerData.goodsTable.GetTableData(GoodsTable.BattleClear).Value);
            }
            
            if (ServerData.statusTable.GetTableData(StatusTable.Level).Value >= GameBalance.WeeklyBossLevelLimit)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.WT).Value += GameBalance.WeeklyTicketWeeklyGetAmount;
                goodsParam.Add(GoodsTable.WT,ServerData.goodsTable.GetTableData(GoodsTable.WT).Value);
            }
            
            
            //십만대산 교환소 주간 교환 횟수 초기화
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan1].Value = 0;
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan2].Value = 0;
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan3].Value = 0;
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan4].Value = 0;
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan5].Value = 0;
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan6].Value = 0;
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Guild1].Value = 0;
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Guild2].Value = 0;
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Guild3].Value = 0;
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Guild4].Value = 0;
            ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Guild5].Value = 0;
            userInfo2Param.Add(UserInfoTable_2.exchage_Daesan1, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan1].Value);
            userInfo2Param.Add(UserInfoTable_2.exchage_Daesan2, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan2].Value);
            userInfo2Param.Add(UserInfoTable_2.exchage_Daesan3, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan3].Value);
            userInfo2Param.Add(UserInfoTable_2.exchage_Daesan4, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan4].Value);
            userInfo2Param.Add(UserInfoTable_2.exchage_Daesan5, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan5].Value);
            userInfo2Param.Add(UserInfoTable_2.exchage_Daesan6, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Daesan6].Value);
            userInfo2Param.Add(UserInfoTable_2.exchage_Guild1, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Guild1].Value);
            userInfo2Param.Add(UserInfoTable_2.exchage_Guild2, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Guild2].Value);
            userInfo2Param.Add(UserInfoTable_2.exchage_Guild3, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Guild3].Value);
            userInfo2Param.Add(UserInfoTable_2.exchage_Guild4, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Guild4].Value);
            userInfo2Param.Add(UserInfoTable_2.exchage_Guild5, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.exchage_Guild5].Value);
            
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.guildPetWeeklyRewardIndex).Value = -1;
            userInfo2Param.Add(UserInfoTable_2.guildPetWeeklyRewardIndex, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.guildPetWeeklyRewardIndex].Value);

            //비무대회 주간 교환 횟수 초기화
            ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_0].Value = 0;
            ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_1].Value = 0;
            ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_2].Value = 0;
            ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_3].Value = 0;
            ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_4].Value = 0;
            ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_5].Value = 0;
            userInfoParam.Add(UserInfoTable.eventMission4_0, ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_0].Value);
            userInfoParam.Add(UserInfoTable.eventMission4_1, ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_1].Value);
            userInfoParam.Add(UserInfoTable.eventMission4_2, ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_2].Value);
            userInfoParam.Add(UserInfoTable.eventMission4_3, ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_3].Value);
            userInfoParam.Add(UserInfoTable.eventMission4_4, ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_4].Value);
            userInfoParam.Add(UserInfoTable.eventMission4_5, ServerData.userInfoTable.TableDatas[UserInfoTable.eventMission4_5].Value);

            //그림자보스
            //ServerData.bossServerTable.TableDatas["b91"].rewardedId.Value = string.Empty;
            //bossParam.Add("b91", ServerData.bossServerTable.TableDatas["b91"].ConvertToString());
        }
        
        goodsParam.Add(GoodsTable.GuimoonRelicClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelicClearTicket).Value);
        goodsParam.Add(GoodsTable.SealWeaponClear, ServerData.goodsTable.GetTableData(GoodsTable.SealWeaponClear).Value);
        goodsParam.Add(GoodsTable.DosulClear, ServerData.goodsTable.GetTableData(GoodsTable.DosulClear).Value);
        goodsParam.Add(GoodsTable.SuhoPetFeedClear, ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeedClear).Value);
        goodsParam.Add(GoodsTable.FoxRelicClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.FoxRelicClearTicket).Value);
        goodsParam.Add(GoodsTable.MeditationClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.MeditationClearTicket).Value);
        goodsParam.Add(GoodsTable.GT, ServerData.goodsTable.GetTableData(GoodsTable.GT).Value);
        goodsParam.Add(GoodsTable.SC, ServerData.goodsTable.GetTableData(GoodsTable.SC).Value);
        goodsParam.Add(GoodsTable.HYC, ServerData.goodsTable.GetTableData(GoodsTable.HYC).Value);
        goodsParam.Add(GoodsTable.DCT, ServerData.goodsTable.GetTableData(GoodsTable.DCT).Value);
        goodsParam.Add(GoodsTable.DC, ServerData.goodsTable.GetTableData(GoodsTable.DC).Value);
        goodsParam.Add(GoodsTable.DE, ServerData.goodsTable.GetTableData(GoodsTable.DE).Value);

        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));
        transactionList.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, yoguiSogulParam));
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactionList.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));


        ServerData.SendTransaction(transactionList, false);
    }

    private void WeekChanged()
    {
    }

    private void MonthChanged()
    {
    }

    public void ClearDailyMission()
    {
        DailyMissionManager.UpdateDailyMission(DailyMissionKey.Attendance, 1);
    }

    public bool HasRemoveAd()
    {
        return tableDatas[removeAd].Value == 1;
    }

    //튜브
    public bool CanSpawnSnowManItem()
    {
        //targetDate가 am 00시 기준이므로 AddDays해야함
        DateTime targetDate = new DateTime(2024, 7, 9);

        return currentServerTime <= targetDate.AddDays(1);
    }

    //벌꿀 이벤트
    public bool CanSpawnSpringEventItem()
    {
        DateTime targetDate = new DateTime(2024, 8, 20);

        return currentServerTime <= targetDate.AddDays(1);
    }

    //미션1 이벤트
    public bool IsMissionEventPeriod()
    {
        DateTime targetDate = new DateTime(2024, 8, 20);

        return currentServerTime <= targetDate.AddDays(1);
    }

    public bool CanMakeEventItem()
    {
        //if (currentServerTime.Month == 11) return true;
        //if (currentServerTime.Month == 12) return true;
        //if (currentServerTime.Month == 1) return true;
        //if (currentServerTime.Month == 2) return true;

        return false;
    }

    public bool CanBuyEventPackage()
    {
        if (currentServerTime.Month == 11) return true;
        if (currentServerTime.Month == 12) return true;
        if (currentServerTime.Month == 1) return true;
        if (currentServerTime.Month == 2) return true;

        return false;
    }

    public bool CanRecordGuildScore()
    {
        if (currentServerTime.Hour == 23
            || currentServerTime.Hour == 0
            || currentServerTime.Hour == 1
            || currentServerTime.Hour == 2
            || currentServerTime.Hour == 3
            || currentServerTime.Hour == 4) return false;

        return true;
    }

    public bool IsRankUpdateTime()
    {
        if (currentServerTime.Hour == 4)
            return false;

        return true;
    }

    public bool IsHotTime()
    {
#if UNITY_EDITOR
        // return true;
#endif

        if (currentServerTime.DayOfWeek != DayOfWeek.Sunday && currentServerTime.DayOfWeek != DayOfWeek.Saturday)
        {
            int currentHour = currentServerTime.Hour;
            return currentHour >= GameBalance.HotTime_Start && currentHour < GameBalance.HotTime_End;
        }
        else
        {
            int currentHour = currentServerTime.Hour;
            return currentHour >= GameBalance.HotTime_Start_Weekend && currentHour < GameBalance.HotTime_End;
        }
    }

    public bool IsHotTimeEvent()
    {
// #if UNITY_EDITOR
//         return true;
// #endif
        var hotTime = GameBalance.HotTimeEventEndPeriod;

        var preMonth = hotTime.Month - 1;

        if (preMonth < 1)
        {
            preMonth = 12;
        }
        
        return currentServerTime.Year <= hotTime.Year && currentServerTime.Month <= preMonth ||
               (currentServerTime.Year <= hotTime.Year &&currentServerTime.Month <= hotTime.Month && currentServerTime.Day <= hotTime.Day);
    }
    
    

    public bool IsMileageEvent(MileageRewardData rewardData)
    {
        if (rewardData == null)
        {
            rewardData = TableManager.Instance.mileageReward.dataArray[5];
        }

        var splitData = rewardData.Eventperiod.Split('-');

        DateTime eventPeriod =
            new DateTime(int.Parse(splitData[0]), int.Parse(splitData[1]), int.Parse(splitData[2]));
        eventPeriod = eventPeriod.AddDays(1); //5월5일을 넣으면 5월6일00시에끝나야함.
        var result = DateTime.Compare(ServerData.userInfoTable.currentServerTime, eventPeriod);


        switch (result)
        {
            //아직 안지남
            case -1:
            case 0:
                return true;
            //지남
            case 1:
                return false;
            default:
                return false;
        }
    }

    public bool IsMileageEvent()
    {
        var tableData = TableManager.Instance.mileageReward.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var splitData = tableData[i].Eventperiod.Split('-');

            DateTime eventPeriod =
                new DateTime(int.Parse(splitData[0]), int.Parse(splitData[1]), int.Parse(splitData[2]));
            eventPeriod = eventPeriod.AddDays(1); //5월5일을 넣으면 5월6일00시에끝나야함.
            var result = DateTime.Compare(ServerData.userInfoTable.currentServerTime, eventPeriod);


            //안지난게 있으면 true
            switch (result)
            {
                //아직 안지남
                case -1:
                case 0:
                    return true;
                //지남
                case 1:
                    break;
                //return false;
                default:
                    break;
                //return false;
            }
        }

        return false;
    }

    public bool IsWeekend()
    {
        return currentServerTime.DayOfWeek == DayOfWeek.Sunday || currentServerTime.DayOfWeek == DayOfWeek.Saturday;
    }

    static int totalKillCount = 0;
    static double updateRequireNum = 100;

    public void GetKillCountTotal()
    {
        totalKillCount += (int)GameManager.Instance.CurrentStageData.Marbleamount;

        if (totalKillCount < updateRequireNum)
        {
        }
        else
        {
            totalKillCount = 0;

            // if (IsMonthlyPass2() == false)
            // {
            // }
            // else
            // {
            //     tableDatas[killCountTotal2].Value += updateRequireNum;
            // }

            //tableDatas[killCountTotalChild].Value += updateRequireNum;
            tableDatas[killCountTotalWinterPass].Value += updateRequireNum;
            tableDatas[killCountTotalSeason].Value += updateRequireNum;
            //tableDatas[killCountTotalSeason2].Value += updateRequireNum;
            tableDatas[killCountTotalSeason3].Value += updateRequireNum;
            //tableDatas[attenCountOne].Value += updateRequireNum;
        }
    }

    public bool IsLastFloor()
    {
        return tableDatas[currentFloorIdx].Value >= 20; //구 301
    }

    public bool IsLastFloor2()
    {
        return tableDatas[currentFloorIdx2].Value >= 100;
    }

    public bool IsLastFloor3()
    {
        return tableDatas[currentFloorIdx6].Value >= TableManager.Instance.sinsuTower.dataArray.Length;
    }

    public bool CanPlayGangChul()
    {
        return currentServerTime.DayOfWeek == DayOfWeek.Monday ||
               currentServerTime.DayOfWeek == DayOfWeek.Tuesday ||
               currentServerTime.DayOfWeek == DayOfWeek.Wednesday ||
               currentServerTime.DayOfWeek == DayOfWeek.Thursday ||
               currentServerTime.DayOfWeek == DayOfWeek.Friday;
    }

    public bool IsMonthlyPass2()
    {
// #if UNITY_EDITOR
//         return GameBalance.isOddMonthlyPass;
// #endif
        //홀수 달의 경우 true , true면 MonthlyPass2
        return (currentServerTime.Month % 2) == 1;
    }

    // 여름훈련
    public bool IsEventPassPeriod()
    {
        return (currentServerTime.Month <= 7 && currentServerTime.Day <= 30);
    }

    // 가을훈련
    public bool IsEventPass2Period()
    {
        return (currentServerTime.Year <= 2023 && (currentServerTime.Month <= 12 || (currentServerTime.Month == 1 && currentServerTime.Day <= 2)));
    }

    public ReactiveProperty<bool> SnowCollectionComplete = new ReactiveProperty<bool>(false);

    public void UpdateSnowCollectionComplete()
    {
        if (SnowCollectionComplete.Value == true) return;

        var tableData = TableManager.Instance.commoncollectionEvent.dataArray;

        bool allComplete = true;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].COMMONTABLEEVENTTYPE != CommonTableEventType.SnowMan) continue;

            if (tableData[i].Active == false) continue;

            if (tableData[i].Lastexchange == true) continue;

            if (string.IsNullOrEmpty(tableData[i].Exchangekey) == true) continue;

            if (this.tableDatas[tableData[i].Exchangekey].Value < tableData[i].Exchangemaxcount)
            {
                allComplete = false;
                break;
            }
        }

        if (allComplete)
        {
            SnowCollectionComplete.Value = true;
        }
    }

    public ReactiveProperty<bool> DDukGukCollectionComplete = new ReactiveProperty<bool>(false);

    public void UpdateDdukGukCollectionComplete()
    {
        if (DDukGukCollectionComplete.Value == true) return;

        var tableData = TableManager.Instance.xMasCollection.dataArray;

        bool allComplete = true;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].COMMONTABLEEVENTTYPE != CommonTableEventType.DdukGuk) continue;

            if (tableData[i].Active == false) continue;

            if (tableData[i].Lastexchange == true) continue;

            if (string.IsNullOrEmpty(tableData[i].Exchangekey) == true) continue;

            if (this.tableDatas[tableData[i].Exchangekey].Value < tableData[i].Exchangemaxcount)
            {
                allComplete = false;
                break;
            }
        }

        if (allComplete)
        {
            DDukGukCollectionComplete.Value = true;
        }
    }
}
//