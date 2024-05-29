using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//순서 절대바뀌면안됨
public enum Item_Type
{
    None = -1,
    Gold,
    Jade,
    GrowthStone,
    Memory,
    Ticket,
    Marble,
    Dokebi,
    SkillPartion,
    WeaponUpgradeStone,
    PetUpgradeSoul,
    YomulExchangeStone,
    Songpyeon,
    TigerBossStone,
    Relic,
    RelicTicket,
    RabitBossStone,
    DragonBossStone,
    Event_Item_0,
    StageRelic,
    SnakeStone,
    PeachReal,
    HorseStone,
    SheepStone,
    MonkeyStone,
    MiniGameReward,
    GuildReward,
    CockStone,
    DogStone,
    SulItem,
    PigStone,
    SmithFire,
    FeelMulStone,

    Asura0,
    Asura1,
    Asura2,
    Asura3,
    Asura4,

    Event_Item_1,
    Asura5,
    Aduk,

    SinSkill0,
    SinSkill1,
    SinSkill2,
    SinSkill3,
    LeeMuGiStone,
    ZangStone,
    SP,//검조각
    Hae_Norigae,
    Hae_Pet,
    Sam_Norigae,
    Sam_Pet,

    Indra0,
    Indra1,
    Indra2,
    IndraWeapon,
    KirinNorigae,
    Kirin_Pet,
    IndraPower,
    Event_Item_SnowMan, //눈사람
    NataWeapon,
    RabitNorigae,
    RabitPet,
    NataSkill,
    OrochiWeapon,
    OrochiSkill,
    OrochiTooth0,
    OrochiTooth1,
    DogNorigae,
    DogPet,

    MihoWeapon,
    MihoNorigae,
    ChunMaNorigae,
    ChunMaPet,
    Hel,
    Ym,
    YeaRaeWeapon,
    YeaRaeNorigae,

    GangrimWeapon,
    GangrimNorigae,
    GangrimSkill,
    //이덕춘 두루마리
    du,

    Sun0,
    Sun1,
    Sun2,
    Sun3,
    Sun4,
    HaeWeapon,
    Fw,
    Cw,
    ChunNorigae0,
    ChunNorigae1,
    ChunNorigae2,
    ChunSun0,
    ChunSun1,
    ChunSun2,
    Event_Kill1_Item,//봄나물
    Event_Fall_Gold,
    ChunNorigae3,
    ChunNorigae4,
    FoxMaskPartial,



    RankFrame1 = 100,
    RankFrame2 = 101,
    RankFrame3 = 102,
    RankFrame4 = 103,
    RankFrame5 = 104,
    RankFrame6_20 = 105,
    RankFrame21_100 = 106,
    RankFrame101_1000 = 107,
    RankFrame1001_10000 = 108,

    RankFrame1_relic = 200,
    RankFrame2_relic = 201,
    RankFrame3_relic = 202,
    RankFrame4_relic = 203,
    RankFrame5_relic = 204,
    RankFrame6_20_relic = 205,
    RankFrame21_100_relic = 206,
    RankFrame101_1000_relic = 207,
    RankFrame1001_10000_relic = 208,

    RankFrame1_miniGame = 300,
    RankFrame2_miniGame = 301,
    RankFrame3_miniGame = 302,
    RankFrame4_miniGame = 303,
    RankFrame5_miniGame = 304,
    RankFrame6_20_miniGame = 305,
    RankFrame21_100_miniGame = 306,
    RankFrame101_1000_miniGame = 307,
    RankFrame1001_10000_miniGame = 308,

    RankFrame1_new_miniGame = 309,
    RankFrame2_new_miniGame = 310,
    RankFrame3_new_miniGame = 311,
    RankFrame4_new_miniGame = 312,
    RankFrame5_new_miniGame = 313,
    RankFrame6_20_new_miniGame = 314,
    RankFrame21_100_new_miniGame = 315,
    RankFrame101_1000_new_miniGame = 316,
    RankFrame1001_10000_new_miniGame = 317,
    //구미호(구)
    RankFrame1_guild = 400,
    RankFrame2_guild = 401,
    RankFrame3_guild = 402,
    RankFrame4_guild = 403,
    RankFrame5_guild = 404,
    RankFrame6_20_guild = 405,
    RankFrame21_100_guild = 406,
    RankFrame101_1000_guild = 407,
    //붉은구미호(신)
    RedFoxFrame1_guild      = 410,
    RedFoxFrame2_guild      = 411,
    RedFoxFrame3_guild      = 412,
    RedFoxFrame4_guild      = 413,
    RedFoxFrame5_guild      = 414,
    RedFoxFrame6_20_guild   = 415,
    RedFoxFrame21_100_guild = 416,
    
    UpdateRewardMail = 420,

    MagicStoneBuff = 500,
    //신규
    RankFrame1guild_new = 600,
    RankFrame2guild_new = 601,
    RankFrame3guild_new = 602,
    RankFrame4guild_new = 603,
    RankFrame5guild_new = 604,
    RankFrame6_20_guild_new = 605,
    RankFrame21_50_guild_new = 606,
    RankFrame51_100_guild_new = 607,

    RankFrame1_boss_new = 700,
    RankFrame2_boss_new = 701,
    RankFrame3_boss_new = 702,
    RankFrame4_boss_new = 703,
    RankFrame5_boss_new = 704,
    RankFrame6_10_boss_new = 705,
    RankFrame10_30_boss_new = 706,
    RankFrame30_50boss_new = 707,
    RankFrame50_70_boss_new = 708,
    RankFrame70_100_boss_new = 709,
    RankFrame100_200_boss_new = 710,
    RankFrame200_500_boss_new = 711,
    RankFrame500_1000_boss_new = 712,
    RankFrame1000_3000_boss_new = 713,

    RankFrame1_boss_GangChul         = 720,
    RankFrame2_boss_GangChul         = 721,
    RankFrame3_boss_GangChul         = 722,
    RankFrame4_boss_GangChul         = 723,
    RankFrame5_boss_GangChul         = 724,
    RankFrame6_10_boss_GangChul      = 725,
    RankFrame10_30_boss_GangChul     = 726,
    RankFrame30_50_boss_GangChul     = 727,
    RankFrame50_70_boss_GangChul     = 728,
    RankFrame70_100_boss_GangChul    = 729,
    RankFrame100_200_boss_GangChul   = 730,
    RankFrame200_500_boss_GangChul   = 731,
    RankFrame500_1000_boss_GangChul  = 732,
    RankFrame1000_3000_boss_GangChul = 733,


    RankFrame1_relic_hell = 800,
    RankFrame2_relic_hell = 801,
    RankFrame3_relic_hell = 802,
    RankFrame4_relic_hell = 803,
    RankFrame5_relic_hell = 804,
    RankFrame6_20_relic_hell = 805,
    RankFrame21_100_relic_hell = 806,
    RankFrame101_1000_relic_hell = 807,
    RankFrame1001_10000_relic_hell = 808,

    RankFrame1_2_war_hell = 809,
    RankFrame3_5_war_hell = 810,
    RankFrame6_20_war_hell = 811,
    RankFrame21_50_war_hell = 812,
    RankFrame51_100_war_hell = 813,
    RankFrame101_1000_war_hell = 814,
    RankFrame1001_10000_war_hell = 815,

    //대산군(신)
    Sangun_1guild_new = 840,
    Sangun_2guild_new = 841,
    Sangun_3guild_new = 842,
    Sangun_4guild_new = 843,
    Sangun_5guild_new = 844,
    Sangun_6_20_guild_new = 845,
    Sangun_21_100_guild_new = 846,
    
    //대산군(구)
    RankFrameParty1guild_new = 850,
    RankFrameParty2guild_new = 851,
    RankFrameParty3guild_new = 852,
    RankFrameParty4guild_new = 853,
    RankFrameParty5guild_new = 854,
    RankFrameParty6_20_guild_new = 855,
    RankFrameParty21_50_guild_new = 856,
    RankFrameParty51_100_guild_new = 857,


    PartyRaidRankFrame1 = 860,
    PartyRaidRankFrame2 = 861,
    PartyRaidRankFrame3 = 862,
    PartyRaidRankFrame4 = 863,
    PartyRaidRankFrame5 = 864,
    PartyRaidRankFrame6_20 = 865,
    PartyRaidRankFrame21_100 = 866,
    PartyRaidRankFrame101_1000 = 867,
    PartyRaidRankFrame1001_10000 = 868,

    MergePartyRaidRankFrame1 = 870,
    MergePartyRaidRankFrame2 = 871,
    MergePartyRaidRankFrame3 = 872,
    MergePartyRaidRankFrame4 = 873,
    MergePartyRaidRankFrame5 = 874,
    MergePartyRaidRankFrame6_10 = 875,
    MergePartyRaidRankFrame11_20 = 876,
    MergePartyRaidRankFrame21_50 = 877,
    MergePartyRaidRankFrame51_100 = 878,
    MergePartyRaidRankFrame101_500 = 879,
    MergePartyRaidRankFrame501_1000 = 880,
    MergePartyRaidRankFrame1001_5000 = 881,
    
    MergePartyRaidRankFrame_0_1 = 890,
    MergePartyRaidRankFrame_0_2 = 891,
    MergePartyRaidRankFrame_0_3 = 892,
    MergePartyRaidRankFrame_0_4 = 893,
    MergePartyRaidRankFrame_0_5 = 894,
    MergePartyRaidRankFrame_0_6_10 = 895,
    MergePartyRaidRankFrame_0_11_20 = 896,
    MergePartyRaidRankFrame_0_21_50 = 897,
    MergePartyRaidRankFrame_0_51_100 = 898,
    MergePartyRaidRankFrame_0_101_500 = 899,
    MergePartyRaidRankFrame_0_501_1000 = 900,
    MergePartyRaidRankFrame_0_1001_5000 = 901,
    
    MergePartyRaidRankFrame_1_1 = 910,
    MergePartyRaidRankFrame_1_2 = 911,
    MergePartyRaidRankFrame_1_3 = 912,
    MergePartyRaidRankFrame_1_4 = 913,
    MergePartyRaidRankFrame_1_5 = 914,
    MergePartyRaidRankFrame_1_6_10 = 915,
    MergePartyRaidRankFrame_1_11_20 = 916,
    MergePartyRaidRankFrame_1_21_50 = 917,
    MergePartyRaidRankFrame_1_51_100 = 918,
    MergePartyRaidRankFrame_1_101_500 = 919,
    MergePartyRaidRankFrame_1_501_1000 = 920,
    MergePartyRaidRankFrame_1_1001_5000 = 921,

    //1000~1200 무기
    weapon0 = 1000,
    weapon1 = 1001,
    weapon2 = 1002,
    weapon3 = 1003,
    weapon4 = 1004,
    weapon5 = 1005,
    weapon6 = 1006,
    weapon7 = 1007,
    weapon8 = 1008,
    weapon9 = 1009,
    weapon10 = 1010,
    weapon11 = 1011,
    weapon12 = 1012,
    weapon13 = 1013,
    weapon14 = 1014,
    weapon15 = 1015,
    weapon16 = 1016,
    //
    weapon37 = 1037,
    weapon38 = 1038,
    weapon39 = 1039,
    weapon40 = 1040,
    weapon41 = 1041,
    weapon42 = 1042,
    weapon157 = 1043, //무림
    weapon158 = 1044, //무림
    
    weapon159 = 1045, //극락
    weapon160 = 1046, //보스도전
    weapon161 = 1047, //보스도전
    
    weapon162 = 1048, //극락
    weapon163 = 1049, //무림
    weapon164 = 1050, //무림
    
    weapon165 = 1051, //극락
    weapon166 = 1052, //무림
    weapon167 = 1053, //무림
    
    weapon168 = 1054, //극락
    weapon169 = 1055, //무림
    weapon170 = 1056, //무림
    weapon171 = 1057, //나선
    
    weapon172 = 1058, //무림
    weapon173 = 1059, //무림
    weapon174 = 1060, //나선
    
    weapon175 = 1061, //삼천
    weapon176 = 1062, //무림
    weapon177 = 1063, //무림
    weapon178 = 1064, //나선
    
    weapon179 = 1065, //삼천
    weapon180 = 1066, //무림
    weapon181 = 1067, //무림
    weapon182 = 1068, //특별
    
    weapon183 = 1069, //삼천
    weapon184 = 1070, //무림
    weapon185 = 1071, //무림
    weapon186 = 1072, //특별
    
    weapon187 = 1073, //삼천
    weapon188 = 1074, //무림
    weapon189 = 1075, //무림
    weapon190 = 1076, //특별

    weapon_end = 1200,
    //

    //코스튬 테이블 키값임 대소문자 변경X
    costume0 = 1201,
    costume1 = 1202,
    costume2 = 1203,
    costume3 = 1204,
    costume4 = 1205,
    costume5 = 1206,
    costume6 = 1207,
    costume7 = 1208,
    costume8 = 1209,
    costume9 = 1210,
    costume10 = 1211,
    costume11 = 1212,
    costume12 = 1213,
    costume13 = 1214,
    costume14 = 1215,
    costume15 = 1216,
    costume16 = 1217,
    costume17 = 1218,
    costume18 = 1219,
    costume19 = 1220,
    costume20 = 1221,
    costume21 = 1222,
    costume22 = 1223,

    costume23 = 1224,
    costume24 = 1225,
    costume25 = 1226,
    costume26 = 1227,

    costume27 = 1228,
    costume28 = 1229,
    costume29 = 1230,

    costume30 = 1231,
    costume31 = 1232,

    costume32 = 1233,
    costume33 = 1234,
    costume34 = 1235,
    costume35 = 1236,
    costume36 = 1237,
    costume37 = 1238,
    costume38 = 1239,
    costume39 = 1240,
    costume40 = 1241,
    costume41 = 1242,

    costume42 = 1243,
    costume43 = 1244,

    costume44 = 1245,
    costume45 = 1246,
    costume46 = 1247,
    costume47 = 1248,
    costume48 = 1249,
    costume49 = 1250,
    costume50 = 1251,
    costume51 = 1252,
    costume52 = 1253,
    costume53 = 1254,

    costume54 = 1255,
    costume55 = 1256,

    costume56 = 1257,
    costume57 = 1258,
    costume58 = 1259,

    costume59 = 1260,
    costume60 = 1261,
    costume61 = 1262,

    costume62 = 1263,
    costume63 = 1264,
    costume64 = 1265,
    costume65 = 1266,

    //도깨비
    costume66 = 1267,
    costume67 = 1268,
    costume68 = 1269,

    //월간 코스튬
    costume69 = 1270,

    //도깨비
    costume70 = 1271,
    costume71 = 1272,

    //크리스마스 코스튬
    costume72 = 1273,
    costume73 = 1274,

    //도깨비
    costume74 = 1275,
    costume75 = 1276,
    //이벤트 검은토끼 + 눈사람
    costume76 = 1277,
    costume77 = 1278,
    //도깨비 3보스
    costume78 = 1279,
    costume79 = 1280,
    costume80 = 1281,
    //수미산
    costume81 = 1282, //지국천왕
    //
    costume82 = 1283, //광목천왕
    costume83 = 1284, //2월 외형
    
    costume84 = 1285, //증장천왕
    costume85 = 1286, //다문천왕

    costume86 = 1287, //그림자동굴

    costume87 = 1288, //수미산추가보스
    costume88 = 1289, //수미산추가보스
    costume89 = 1290, //수미산추가보스

    costume90 = 1291, //봄나물 머슴호연
    costume91 = 1292, //3월 월간외형2
    
    costume92 = 1293, // 도적 외형0
    costume93 = 1294, // 도적 외형1
    costume94 = 1295, // 도적 외형2
    costume95 = 1296, // 도적 외형3
    
    costume96 = 1297, // 닌자 외형0
    costume97 = 1298, // 닌자 외형1
    costume98 = 1299, // 닌자 외형2
    costume99 = 1300, // 새학기
    

    pet0 = 1301,
    pet1 = 1302,
    pet2 = 1303,
    pet3 = 1304,

    costume100 = 1400, //백호 호연    
    
    costume101 = 1401,     //왕들
    costume102 = 1402,     
    costume103 = 1403,     
    costume104 = 1404,     
    
    costume105 = 1405,     //암흑
    costume106 = 1406,     
    costume107 = 1407,     
    costume108 = 1408,
    
    costume109 = 1409, // 5월 월간외형     
    
    costume110 = 1410, // 심연보스0     
    costume111 = 1411, // 주인보스     
    costume112 = 1412, // 주인보스     
    costume113 = 1413, // 주인보스     
    costume114 = 1414, // 어린이날     
    costume115 = 1415, // 꽃놀이     
    costume116 = 1416, // 신선     
    costume117 = 1417, // 신선     
    costume118 = 1418, // 신선     
    costume119 = 1419, // 월간     
    costume120 = 1420, // 신선     
    costume121 = 1421, // 신선     
    costume122 = 1422, // 천선     
    costume123 = 1423, // 이벤트 외형     
    costume124 = 1424, // 천선     
    costume125 = 1425, // 천선     
    costume126 = 1426, // 천선     
    costume127 = 1427, // 귀인곡     
    costume128 = 1428, // 귀인곡     
    costume129 = 1429, // 귀인곡     
    costume130 = 1430, // 귀인곡     
    costume131 = 1431, // 7월 월간외형     
    costume132 = 1432, // 현상수배     
    costume133 = 1433, // 현상수배     
    costume134 = 1434, // 현상수배     
    costume135 = 1435, // 현상수배     
    costume136 = 1436, // 수박이벤트외형     
    costume137 = 1437, // 이벤트외형     
    costume138 = 1438, // 현상수배     
    costume139 = 1439, // 현상수배
    
    costume140 = 1440, // 현상수배     
    costume141 = 1441, // 현상수배     
    costume142 = 1442, // 현상수배     
    costume143 = 1443, // 이벤트     
    costume144 = 1444, // 8월 월간
    costume145 = 1445, // 현상수배     
    costume146 = 1446, // 현상수배     

    costume147 = 1447, // 수인     
    costume148 = 1448, // 수인     
    
    costume149 = 1449, // 투명
    
    costume150 = 1450, // 현상     
    costume151 = 1451, // 수인     
    costume152 = 1452, // 수인     
    
    costume153 = 1453, // 9월 월간     
    costume154 = 1454, // 송편 보름달 외형 이벤트     
    
    costume155 = 1455, // 현상     
    costume156 = 1456, // 수인     
    costume157 = 1457, // 수인
    
    costume158 = 1458, // 수인     
    costume159 = 1459, // 현상     
    costume160 = 1460, // 10월 월간     
    costume161 = 1461, // 마블     
    costume162 = 1462, // 용인보스     
    costume163 = 1463, // 용인보스     
    costume164 = 1464, // 용인보스     
    
    costume165 = 1465, // 용인보스     
    costume166 = 1466, // 용인보스     
    costume167 = 1467, // 이벤트
    
    costume168 = 1468, //용인
    costume169 = 1469, //용인
    costume170 = 1470, // 11월 월간
    costume171 = 1471, // 길드
    
    costume172 = 1472, //보스
    costume173 = 1473, //보스
    costume174 = 1474, //고양이
    
    costume175 = 1475, //보스
    costume176 = 1476, //보스
    costume177 = 1477, //월간
    costume178 = 1478, //pvp
    
    costume179 = 1479, //보스
    costume180 = 1480, //보스
    costume181 = 1481, //이벤트
    costume182 = 1482, //이벤트
    costume183 = 1483, //용궁
    costume184 = 1484, //용궁
    costume185 = 1485, //월간
    costume186 = 1486, //용궁
    costume187 = 1487, //용궁
    costume188 = 1488, //천마
    costume189 = 1489, //비무
    costume190 = 1490, //주간보스
    
    costume191 = 1491, //천마
    costume192 = 1492, //비무
    costume193 = 1493, //주간보스
    costume194 = 1494, //주간보스
    
    costume195 = 1495, //무림
    costume196 = 1496, //무림
    costume197 = 1497, //패키지외형
    costume198 = 1498, //패키지외형
    costume199 = 1499, //무림
    costume200 = 1500, //보스도전
    costume201 = 1501, //보스도전
    costume202 = 1502, //월간훈련
    
    costume203 = 1503, //극락
    costume204 = 1504, //무림
    costume205 = 1505, //무림
    costume206 = 1506, //이벤트
    
    costume207 = 1507, //
    costume208 = 1508, //
    costume209 = 1509, //
    costume210 = 1510, //이벤트
    
    costume211 = 1511, //극락
    costume212 = 1512, //무림
    costume213 = 1513, //무림
    costume214 = 1514, //월간
    costume215 = 1515, //나선
    
    costume216 = 1516, //무림
    costume217 = 1517, //무림
    costume218 = 1518, //이벤트
    costume219 = 1519, //나선

    costume220 = 1520, //삼천
    costume221 = 1521, //무림
    costume222 = 1522, //무림
    costume223 = 1523, //특별
    costume224 = 1524, //월간
    
    costume225 = 1525, //삼천
    costume226 = 1526, //무림0
    costume227 = 1527, //무림1
    costume228 = 1528, //특별
    costume229 = 1529, //이벤트0
    costume230 = 1530, //이벤트1
    
    costume231 = 1531, //삼천
    costume232 = 1532, //무림0
    costume233 = 1533, //무림1
    costume234 = 1534, //이벤트
    costume235 = 1535, //특별
    
    costume236 = 1536, //삼천
    costume237 = 1537, //무림0
    costume238 = 1538, //무림1
    costume239 = 1539, //특별
    costume240 = 1540, //월간
    costume241 = 1541, //이벤트

    costume_end=1999,

    //2000~2999 마도서
    magicBook0 = 2000,
    magicBook1 = 2001,
    magicBook2 = 2002,
    magicBook3 = 2003,
    magicBook4 = 2004,
    magicBook5 = 2005,
    magicBook6 = 2006,
    magicBook7 = 2007,
    magicBook8 = 2008,
    magicBook9 = 2009,
    magicBook10 = 2010,
    magicBook11 = 2011,
    magicBook130 = 2012,//무림
    magicBook131 = 2013,//무림
    
    magicBook132 = 2014,//무림
    magicBook133 = 2015,//3월 월간
    
    magicBook134 = 2016,//극락
    magicBook135 = 2017,//극락
    
    magicBook136 = 2018,//극락
    magicBook137 = 2019,//나선
    magicBook138 = 2020,//월간
    
    magicBook139 = 2021,//나선
    
    magicBook140 = 2022,//삼천
    magicBook141 = 2023,//특별
    magicBook142 = 2024,//월간
    
    magicBook143 = 2025,//특별의뢰
    magicBook144 = 2026,//삼천
    
    magicBook145 = 2027,//삼천
    magicBook146 = 2028,//특별

    magicBook147 = 2029,//삼천
    magicBook148 = 2030,//특별
    magicBook149 = 2031,//월간

    magicBook_End=2999,
    //3000~3100스킬
    skill0 = 3000,
    skill1 = 3001,
    skill2 = 3002,
    skill3 = 3003,
    skill4 = 3004,
    skill5 = 3005,
    skill6 = 3006,
    skill7 = 3007,
    skill8 = 3008,
    skill9 = 3009,
    skill10 = 3010,
    skill11 = 3011,
    
    gumiho0 = 5000,
    gumiho1 = 5001,
    gumiho2 = 5002,
    gumiho3 = 5003,
    gumiho4 = 5004,
    gumiho5 = 5005,
    gumiho6 = 5006,
    gumiho7 = 5007,
    gumiho8 = 5008,

    //지옥 전용템
    h0 = 6000,
    h1 = 6001,
    h2 = 6002,
    h3 = 6003,
    h4 = 6004,
    h5 = 6005,
    h6 = 6006,
    h7 = 6007,
    h8 = 6008,
    h9 = 6009,

    //심연 전용템
    d0 = 6010,
    d1 = 6011,
    d2 = 6012,
    d3 = 6013,
    d4 = 6014,
    d5 = 6015,
    d6 = 6016,
    d7 = 6017,

    //천계 칠션녀 전용템
    c0 = 7000,
    c1 = 7001,
    c2 = 7002,
    c3 = 7003,
    c4 = 7004,
    c5 = 7005,
    c6 = 7006,

    ChunWeapon0 = 7007,
    ChunPet0 = 7008,
    ChunWeapon1 = 7009,
    ChunPet1 = 7010,
    ChunWeapon2 = 7011,
    ChunPet2 = 7012,
    ChunWeapon3 = 7013,
    ChunPet3 = 7014,

    DokebiWeapon0 = 7020,
    DokebiWeapon1 = 7021,
    DokebiWeapon2 = 7022,
    DokebiWeapon3 = 7023,
    DokebiWeapon4 = 7024,
    DokebiWeapon5 = 7025,
    DokebiWeapon6 = 7026,

    DokebiWeapon7 = 7027,
    DokebiWeapon8 = 7028,
    DokebiWeapon9 = 7029,

    DokebiNorigae0 = 7030,
    DokebiNorigae1 = 7031,
    DokebiNorigae2 = 7032,
    DokebiNorigae3 = 7033,
    DokebiNorigae4 = 7034,
    DokebiNorigae5 = 7035,
    DokebiNorigae6 = 7036,

    DokebiNorigae7 = 7037,
    DokebiNorigae8 = 7038,
    DokebiNorigae9 = 7039,

    SasinsuWeapon0 = 7040,
    SasinsuWeapon1 = 7041,
    SasinsuWeapon2 = 7042,
    SasinsuWeapon3 = 7043,
    
    //사신수 진
    SasinsuPet0 = 7050,
    SasinsuPet1 = 7051,
    SasinsuPet2 = 7052,
    SasinsuPet3 = 7053,
    //사신수 극
    SasinsuPet4 = 7054,
    SasinsuPet5 = 7055,
    SasinsuPet6 = 7056,
    SasinsuPet7 = 7057,

    SahyungWeapon0 = 7060,
    SahyungWeapon1 = 7061,
    SahyungWeapon2 = 7062,
    SahyungWeapon3 = 7063,

    SahyungPet0 = 7070,
    SahyungPet1 = 7071,
    SahyungPet2 = 7072,
    SahyungPet3 = 7073,
    
    VisionPet0 = 7080,
    VisionPet1 = 7081,
    VisionPet2 = 7082,
    VisionPet3 = 7083,

    FoxPet0 = 7090,
    FoxPet1 = 7091,
    FoxPet2 = 7092,
    FoxPet3 = 7093,
    
    TigerPet0 = 7094,
    TigerPet1 = 7095,
    TigerPet2 = 7096,
    TigerPet3 = 7097,
    
    
    
    
    DokebiHorn0 = 7100,
    DokebiHorn1 = 7101,
    DokebiHorn2 = 7102,
    DokebiHorn3 = 7103,
    DokebiHorn4 = 7104,
    DokebiHorn5 = 7105,
    DokebiHorn6 = 7106,

    DokebiHorn7 = 7107,
    DokebiHorn8 = 7108,
    DokebiHorn9 = 7109,


    SpecialSuhoPet0 = 7110,
    SpecialSuhoPet1 = 7111,
    SpecialSuhoPet2 = 7112,
    SpecialSuhoPet3 = 7113,
    SpecialSuhoPet4 = 7114,
    SpecialSuhoPet5 = 7115,
    SpecialSuhoPet6 = 7116,
    SpecialSuhoPet7 = 7117,
    
    SumisanWeapon0 = 7120,
    SumisanWeapon1 = 7121,
    SumisanWeapon2 = 7122,
    SumisanWeapon3 = 7123,
    SumisanWeapon4 = 7124,
    SumisanWeapon5 = 7125,
    SumisanWeapon6 = 7126,

    SumisanNorigae0 = 7130,
    SumisanNorigae1 = 7131,
    SumisanNorigae2 = 7132,
    SumisanNorigae3 = 7133,
    SumisanNorigae4 = 7134,
    SumisanNorigae5 = 7135,
    SumisanNorigae6 = 7136,

    
    ThiefWeapon0 = 7140,
    ThiefWeapon1 = 7141,
    ThiefWeapon2 = 7142,
    ThiefWeapon3 = 7143,
    
    ThiefNorigae0 = 7150,
    ThiefNorigae1 = 7151,
    ThiefNorigae2 = 7152,
    ThiefNorigae3 = 7153,
    
    NinjaWeapon0 = 7160,
    NinjaWeapon1 = 7161,
    NinjaWeapon2 = 7162,
    
    KingWeapon0 = 7163,
    KingWeapon1 = 7164,
    KingWeapon2 = 7165,
    KingWeapon3 = 7166,
    
    NinjaNorigae0 = 7170,
    NinjaNorigae1 = 7171,
    NinjaNorigae2 = 7172,
    
    KingNorigae0 = 7173,
    KingNorigae1 = 7174,
    KingNorigae2 = 7175,
    KingNorigae3 = 7176,
    
    DarkWeapon0 = 7180,
    DarkWeapon1 = 7181,
    DarkWeapon2 = 7182,
    DarkWeapon3 = 7183,
    
    
    DarkNorigae0 = 7190,
    DarkNorigae1 = 7191,
    DarkNorigae2 = 7192,
    DarkNorigae3 = 7193,
    
    MasterWeapon0 = 7200,
    MasterWeapon1 = 7201,
    MasterWeapon2 = 7202,
    MasterWeapon3 = 7203,
    MasterNorigae0 = 7210,
    MasterNorigae1 = 7211,
    MasterNorigae2 = 7212,
    MasterNorigae3 = 7213,
    
    SinsunWeapon0 = 7220,
    SinsunWeapon1 = 7221,
    SinsunWeapon2 = 7222,
    SinsunWeapon3 = 7223,
    SinsunWeapon4 = 7224,
    SinsunWeapon5 = 7225,
    SinsunWeapon6 = 7226,
    SinsunWeapon7 = 7227,
    SinsunWeapon8 = 7228,
    
    SinsunNorigae0 = 7230,
    SinsunNorigae1 = 7231,
    SinsunNorigae2 = 7232,
    SinsunNorigae3 = 7233,
    SinsunNorigae4 = 7234,
    SinsunNorigae5 = 7235,
    SinsunNorigae6 = 7236,
    SinsunNorigae7 = 7237,
    SinsunNorigae8 = 7238,
    
    
    HyunSangWeapon0 = 7240,
    HyunSangWeapon1 = 7241,
    HyunSangWeapon2 = 7242,
    HyunSangWeapon3 = 7243,
    HyunSangWeapon4 = 7244,
    HyunSangWeapon5 = 7245,
    HyunSangWeapon6 = 7246,
    HyunSangWeapon7 = 7247,
    HyunSangWeapon8 = 7248,
    HyunSangWeapon9 = 7249,
    
    HyunSangNorigae0 = 7250,
    HyunSangNorigae1 = 7251,
    HyunSangNorigae2 = 7252,
    HyunSangNorigae3 = 7253,
    HyunSangNorigae4 = 7254,
    HyunSangNorigae5 = 7255,
    HyunSangNorigae6 = 7256,
    HyunSangNorigae7 = 7257,
    HyunSangNorigae8 = 7258,
    HyunSangNorigae9 = 7259,
    
    
    HyunSangWeapon10 = 7260,
    HyunSangWeapon11 = 7261,
    DragonWeapon0 = 7262,
    DragonWeapon1 = 7263,
    DragonWeapon2 = 7264,
    DragonWeapon3 = 7265,
    DragonWeapon4 = 7266,
    DragonWeapon5 = 7267,
    DragonWeapon6 = 7268,
    DragonWeapon7 = 7269,
    weapon147 = 7270,
    weapon148 = 7271,
    weapon149 = 7272,
    weapon150 = 7273,
    weapon151 = 7274,
    weapon152 = 7275,
    weapon153 = 7276,
    weapon154 = 7277,
    weapon155 = 7278,
    weapon156 = 7279,
    
    
    HyunSangNorigae10 = 7280,
    HyunSangNorigae11 = 7281,
    DragonNorigae0 = 7282,
    DragonNorigae1 = 7283,
    DragonNorigae2 = 7284,
    DragonNorigae3 = 7285,
    magicBook118 = 7286,
    magicBook119 = 7287,
    magicBook120 = 7288,
    
    magicBook121 = 7289,
    magicBook122 = 7290,
    magicBook123 = 7291,
    
    magicBook124 = 7292, //용궁
    magicBook125 = 7293, //용궁
    magicBook126 = 7294,
    magicBook127 = 7295, //용궁
    magicBook128 = 7296,//2월월간
    magicBook129 = 7297,//2월월간
    
    ChunGuPet0 = 7600,
    ChunGuPet1 = 7601,
    ChunGuPet2 = 7602,
    ChunGuPet3 = 7603,
    
    SpecialSuhoPet8 = 7604,
    SpecialSuhoPet9 = 7605,
    SpecialSuhoPet10 = 7606,
    SpecialSuhoPet11 = 7607,
    SpecialSuhoPet12 = 7608,
    SpecialSuhoPet13 = 7609,
    
    
    pet52 = 7700, //이벤트펫
    pet53 = 7701, //패스펫
    pet54 = 7702, //패스펫
    pet55 = 7703, //패스펫
    pet56 = 7704, //패스펫
    pet57 = 7705, //패스펫
    pet58 = 7706, //패스펫

    pet_end = 7999,
    RecommendWeapon0 = 8000,
    RecommendWeapon1 = 8001,
    RecommendWeapon2 = 8002,
    RecommendWeapon3 = 8003,
    RecommendWeapon4 = 8004,

    RecommendWeapon5 = 8005,
    RecommendWeapon6 = 8006,
    RecommendWeapon7 = 8007,
    RecommendWeapon8 = 8008,
    RecommendWeapon9 = 8009,

    RecommendWeapon10 = 8010,
    RecommendWeapon11 = 8011,
    RecommendWeapon12 = 8012,

    RecommendWeapon13 = 8013,
    RecommendWeapon14 = 8014,
    RecommendWeapon15 = 8015,
    RecommendWeapon16 = 8016,
    RecommendWeapon17 = 8017,
    RecommendWeapon18 = 8018,
    RecommendWeapon19 = 8019,
    RecommendWeapon20 = 8020,
    RecommendWeapon21= 8021,
    RecommendWeapon22 = 8022,
    weapon146 = 8023,

    ChunNorigae5 = 8500,
    ChunNorigae6 = 8501,

    MonthNorigae0 = 8600,
    MonthNorigae1 = 8601,
    weapon81 = 8602,
    MonthNorigae2 = 8603,
    weapon90 = 8604,//바람개비무기
    MonthNorigae3 = 8605, // 월간노리개
    MonthNorigae4 = 8606, // 월간노리개
    MonthNorigae5 = 8607, // 월간노리개
    MonthNorigae6 = 8608, // 월간노리개
    MonthNorigae7 = 8609, // 월간노리개
    MonthNorigae8 = 8610, // 월간노리개
    weapon131 = 8611,//2주년무기
    RecommendNorigae0 = 8612, // 투명노리개
    MonthNorigae9 = 8613, // 월간노리개
    MonthNorigae10 = 8614, // 월간노리개
    MonthNorigae11 = 8615, // 월간노리개
    magicBook116 = 8616,// pvp노리개
    magicBook117 = 8617, // 월간노리개

    Chun0 = 8700, // 천계기술
    Chun1 = 8701,
    Chun2 = 8702,
    Chun3 = 8703,
    Chun4 = 8704,

    DokebiSkill0 = 8710, // 도깨비 기술
    DokebiSkill1 = 8711,
    DokebiSkill2 = 8712,
    DokebiSkill3 = 8713,
    DokebiSkill4 = 8714,

    FourSkill0 = 8720, //사천왕기술
    FourSkill1 = 8721, 
    FourSkill2 = 8722, 
    FourSkill3 = 8723,

    FourSkill4 = 8724, //사천왕 유저기술
    FourSkill5 = 8725,
    FourSkill6 = 8726,
    FourSkill7 = 8727,
    FourSkill8 = 8728,
    
    VisionSkill0 = 8730, //비전
    VisionSkill1 = 8731, 
    VisionSkill2 = 8732, 
    VisionSkill3 = 8733,
    
    VisionSkill4 = 8734,//심연궁극
    VisionSkill5 = 8735,//신선궁극
    VisionSkill6 = 8736,//비전타워신규궁극
    VisionSkill7 = 8737,//비전타워신규궁극
    
    ThiefSkill0 = 8740, //도적스킬
    ThiefSkill1 = 8741, 
    ThiefSkill2 = 8742, 
    ThiefSkill3 = 8743,
    ThiefSkill4 = 8744,
    
    DarkSkill0 = 8745, //심연스킬
    DarkSkill1 = 8746, 
    DarkSkill2 = 8747, 
    DarkSkill3 = 8748,
    DarkSkill4 = 8749,
    
    VisionSkill8 = 8750,//비전
    VisionSkill9 = 8751,//비전
    VisionSkill10 = 8752,//비전
    VisionSkill11 = 8753,//비전
    VisionSkill12 = 8754,//비전
    VisionSkill13 = 8755,//비전

    SinsunSkill0 = 8756, //신선스킬
    SinsunSkill1 = 8757, 
    SinsunSkill2 = 8758, 
    SinsunSkill3 = 8759,
    SinsunSkill4 = 8760,
    VisionSkill14 = 8761,
    
    DragonSkill0 = 8762,
    DragonSkill1 = 8763,
    DragonSkill2 = 8764,
    DragonSkill3 = 8765,
    DragonSkill4 = 8766,
    VisionSkill15 = 8767,
    VisionSkill16 = 8768,
    
    DPSkill0 = 8769,
    DPSkill1 = 8770,
    DPSkill2 = 8771,
    DPSkill3 = 8772,
    DPSkill4 = 8773,
    
    Event_NewYear = 8800, //떡국
    Event_NewYear_All = 8801, // 총 습득량
    
    Event_Mission2 = 8802,//미션2 - 추석
    Event_Mission2_All = 8803, 
    Event_Collection_All =8804,//봄나물 총습득량
    Event_Item_SnowMan_All=8805,

    Event_Mission3 = 8806,//미션 3- 보름달
    Event_Mission3_All = 8807, // 총 습득량
    
    Event_Mission1 = 8808,//미션 1 - 할로윈
    Event_Mission1_All = 8809, 
    
    Mileage = 9000,
    DokebiFire = 9001,
    DokebiFireKey = 9002,
    HellPower = 9003,
    DokebiTreasure = 9004,
    SusanoTreasure = 9005,
    MiniGameReward2 = 9006,
    DokebiFireEnhance = 9007,
    SumiFire = 9008,
    SumiFireKey = 9009,
    NewGachaEnergy = 9010,
    DokebiBundle = 9011,

    SinsuRelic = 9012,
    SahyungTreasure = 9013,
    EventDice = 9014,
    Tresure = 9015,
    SuhoPetFeed = 9016,
    SuhoPetFeedClear = 9017,
    SinsuMarble = 9018,
    VisionTreasure = 9019,
    GuildTowerClearTicket = 9020,
    GuildTowerHorn = 9021,
    DarkTreasure = 9022,
    SoulRingClear = 9023,
    Event_HotTime = 9024,
    HyungsuRelic = 9025,
    FoxRelic= 9026, 
    FoxRelicClearTicket= 9027, 
    SealWeaponClear= 9028, 
    SinsunTreasure= 9029, 
    GoldBar= 9030, 
    GwisalTreasure= 9031,
    DosulGoods= 9032, 
    DosulClear= 9033,
    ChunguTreasure= 9034,
    SleepRewardItem=9035,
    ChunguRelic = 9036,
    GuimoonRelic = 9037,
    GuimoonRelicClearTicket = 9038,
    ClearTicket = 9039,
    TransGoods = 9040,
    TransClearTicket = 9041,
    Event_SA = 9042,
    MeditationGoods = 9043,
    MeditationClearTicket = 9044,
    DaesanGoods = 9045,
    HonorGoods = 9046,
    DragonScale = 9047,
    YoPowerGoods = 9048,
    TaeguekGoods = 9049,
    TaeguekElixir = 9050,
    SuhoTreasure = 9051,
    BlackFoxGoods = 9052,
    BlackFoxClear = 9053,
    ByeolhoGoods = 9054,
    ByeolhoClear = 9055,
    BattleGoods = 9056,
    BattleClear= 9057,
    BattleScore= 9058,
    DPT= 9059,
    GT= 9060,
    WT= 9061,
    SG = 9062,
    SC = 9063,
    SB = 9064,
    HYG = 9065,
    HYC = 9066,
    SRG = 9067,
    TJCT = 9068,//초월유적소탕권
    RJ=9069, //빨
    YJ=9070, //노
    BJ=9071, //파
    
    Goods_End=10000,
    //Treasure=10001~10100
    MRT=10001, //무림유물
    DBT=10002, //난이도유물
    YOT=10003, //연옥유물
    Treasure_End=10100,
    
    //VisionSkill=10001~10100
    VisionSkill17=10101,
    VisionSkill18=10102,
    VisionSkill19=10103,
    VisionSkill20=10104,
    VisionSkill_End=10200,
    
    GRSkill0 = 10201, //극락 스킬
    GRSkill1 = 10202, //극락 스킬
    GRSkill2 = 10203, //극락 스킬
    GRSkill3 = 10204, //극락 스킬
    GRSkill4 = 10205, //극락 스킬
    
    Skill_End= 10400,

    //차원의 균열
    #region dimension

    DC = 15000, //큐브
    DE = 15001, //정수
    DCT = 15002, //소탕권
    Dimension_End = 19999,

    #endregion
    
    Exp = 20000,

    #region Post

    Dimension_Ranking_Reward_1          = 30000,
    Dimension_Ranking_Reward_2          = 30001,
    Dimension_Ranking_Reward_3          = 30002,
    Dimension_Ranking_Reward_4          = 30003,
    Dimension_Ranking_Reward_5          = 30004,
    Dimension_Ranking_Reward_6_20       = 30005,
    Dimension_Ranking_Reward_21_100     = 30006,
    Dimension_Ranking_Reward_101_1000   = 30007,
    Dimension_Ranking_Reward_1001_10000 = 30008,
    
    Dimension_Ranking_Reward_End        = 30100,

    EventReward_HotTime_0 = 30101,
    
    EventReward_Post_End=30200,
    #endregion
}
//
public class DropItem : PoolItem
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Collider2D collider;

    [SerializeField]
    private SpriteRenderer icon;

    private const float gravity = 2f;

    private Item_Type type;
    private float amount;

    private static WaitForSeconds dropItemLifeTime = new WaitForSeconds(120.0f);

    private new void OnDisable()
    {
        base.OnDisable();
        this.gameObject.layer = LayerMasks.DropItemSpawnedLayerMask;
    }

    private void OnEnable()
    {
        Spawned();
    }

    public void Initialize(Item_Type type, float amount)
    {
        this.type = type;

        this.amount = amount + amount * PlayerStats.GetDropAmountAddValue();

        SetIcon();
        SetLifeTime();
    }

    private void SetLifeTime()
    {
        StartCoroutine(LifeRoutine());
    }

    private IEnumerator LifeRoutine()
    {
        yield return dropItemLifeTime;
        this.gameObject.SetActive(false);
    }

    private void SetIcon()
    {
        icon.sprite = CommonUiContainer.Instance.GetItemIcon(type);
    }

    private void Spawned()
    {
        rb.gravityScale = 0f;
        collider.isTrigger = true;
        this.gameObject.layer = LayerMasks.DropItemSpawnedLayerMask;
    }

    private void WhenDropAnimEnd()
    {
        collider.isTrigger = false;
        rb.gravityScale = gravity;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMasks.PlatformLayerMask)
        {
            rb.gravityScale = 0f;
            this.gameObject.layer = LayerMasks.DropItemLayerMask;
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(Tags.Player) || collision.gameObject.tag.Equals(Tags.Pet))
        {
            WhenItemTriggered();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(Tags.Player) || collision.gameObject.tag.Equals(Tags.Pet))
        {
            WhenItemTriggered();
        }
    }

    private void WhenItemTriggered()
    {
        this.gameObject.SetActive(false);

        ApplyItemData();
    }
    private void ApplyItemData()
    {
        switch (type)
        {
            case Item_Type.GrowthStone:
                {
                    ServerData.goodsTable.GetMagicStone(amount);
                }
                break;
            case Item_Type.Marble:
                {
                    ServerData.goodsTable.GetMarble(amount);
                }
                break;
        }
    }

    
}
