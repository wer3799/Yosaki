using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using static UiGuildMemberCell;

public static class CommonString
{
    public static string RunAnimKey = "Run";
    public static string WalkAnimKey = "Walk";
    public static string Attack = "Attack";
    public static string BottomBlock = "BottomBlock";
    public static string Platform = "Platform";


    public static string Notice = "알림";
    public static string DataLoadFailedRetry = "데이터 로드에 실패했습니다.\n 다시 로드하시겠습니까?";


    public static string NickNameError_400 = "잘못된 닉네임 입니다.";
    public static string NickNameError_409 = "이미 존재하는 닉네임 입니다.";

    public static string ItemGrade_0 = "하급";
    public static string ItemGrade_1 = "중급";
    public static string ItemGrade_2 = "상급";
    public static string ItemGrade_3 = "특급";
    public static string ItemGrade_4 = "전설";
    public static string ItemGrade_5 = "요물";
    public static string ItemGrade_6 = "야차";
    public static string ItemGrade_6_Ring = "신물";
    public static string ItemGrade_7 = "필멸";
    public static string ItemGrade_8 = "필멸(암)";
    public static string ItemGrade_9 = "필멸(천)";
    public static string ItemGrade_10 = "필멸(극)";
    public static string ItemGrade_11 = "인드라";
    public static string ItemGrade_12 = "나타";
    public static string ItemGrade_13 = "오로치";
    public static string ItemGrade_14 = "필멸(패)";
    public static string ItemGrade_15 = "여우검";
    public static string ItemGrade_16 = "지옥";
    public static string ItemGrade_17 = "여래";
    public static string ItemGrade_18 = "외형";
    public static string ItemGrade_19 = "천상계";
    public static string ItemGrade_20 = "십만대산";
    public static string ItemGrade_21 = "???";
    public static string ItemGrade_22 = "도깨비";
    public static string ItemGrade_23 = "사신수";
    public static string ItemGrade_24 = "수미산";
    public static string ItemGrade_25 = "사흉수";
    public static string ItemGrade_26 = "보물";
    public static string ItemGrade_27 = "암흑";
    public static string ItemGrade_28 = "신선";
    public static string ItemGrade_29 = "현상";
    public static string ItemGrade_5_Norigae = "신물";
    public static string ItemGrade_6_Norigae = "영물";
    public static string ItemGrade_7_Norigae = "영물";
    public static string ItemGrade_8_Norigae = "영물";
    public static string ItemGrade_9_Norigae = "외형";
    public static string ItemGrade_10_Norigae = "영물";
    public static string ItemGrade_11_Norigae = "지옥";
    public static string ItemGrade_12_Norigae = "여래";
    public static string ItemGrade_13_Norigae = "천상계";
    public static string ItemGrade_22_Norigae = "도깨비";
    public static string ItemGrade_24_Norigae = "수미산";
    public static string ItemGrade_26_Norigae = "보물";
    public static string ItemGrade_27_Norigae = "암흑";
    public static string ItemGrade_28_Norigae = "신선";
    public static string ItemGrade_29_Norigae = "현상";

    public static string ItemGrade_4_Skill = "주작";
    public static string ItemGrade_5_Skill = "청룡";
    public static string ItemGrade_6_Skill = "흑룡";
    public static string ItemGrade_7_Skill = "나타";
    public static string ItemGrade_8_Skill = "오로치";
    public static string ItemGrade_9_Skill = "신선검";
    public static string ItemGrade_10_Skill = "천계검";
    public static string ItemGrade_11_Skill = "도깨비검";
    public static string ItemGrade_12_Skill = "금강검";
    public static string ItemGrade_13_Skill = "비전검";
    public static string ItemGrade_14_Skill = "섬광검";
    public static string ItemGrade_15_Skill = "심연검";

    public static string GoldItemName = "금화";
    public static string BonusSpinCoin = "복주머니 뽑기권";

    public static string ContentsName_Boss = "고양이 요괴전";
    public static string ContentsName_FireFly = "반딧불 요괴전";
    public static string ContentsName_InfinityTower = "요괴 도장";
    public static string ContentsName_Dokebi = "도깨비 삼형재";

    public static string ChatConnectString = "채팅 채널에 입장했습니다.";

    public static char ChatSplitChar = '◙';
    public static string GuildText = "문파";
    public static string PartyTowerText = "동굴";
    public static string PartyTower2Text = "그림자 동굴";

    public static string RankPrefix_Level = "레벨";
    public static string RankPrefix_Stage = "스테이지";
    public static string RankPrefix_Boss = "지옥탈환전(지옥)";
    public static string RankPrefix_Real_Boss = "십만대산(개인)";
    public static string RankPrefix_Relic = "영혼의숲(지옥)";
    public static string RankPrefix_MiniGame = "미니게임";
    public static string RankPrefix_GangChul = "강철이";
    public static string RankPrefix_ChunMaTop = "십만대산(파티)";

    public static string[] ThemaName = { "마왕성 정원", "이상한 숲", "마법 동굴", "리퍼의 영역", "지옥 입구", "지옥 성곽", "지옥 안채", "지옥숲" };

    public static string CafeURL = "https://cafe.naver.com/yokiki";

    public static string IOS_nick = "_IOS";
    public static string IOS_loginType = "IOS_LoginType";
    public static string SavedLoginTypeKey = "SavedLoginTypeKey";
    public static string SavedLoginPassWordKey = "SavedLoginPassWordKey";
    public static string weapon78Key = "weapon78";
    public static string weapon79Key = "weapon79";


    public static string GetContentsName(ContentsType contentsType)
    {
        switch (contentsType)
        {
            case ContentsType.NormalField:
                {
                }
                break;
            case ContentsType.FireFly:
                {
                    return ContentsName_FireFly;
                }
                break;
            case ContentsType.Boss:
                {
                    return ContentsName_Boss;
                }
                break;
            case ContentsType.InfiniteTower:
                {
                    return ContentsName_InfinityTower;
                }
                break;
        }

        return "미등록";
    }

    public static string GetItemName(Item_Type item_type)
    {
        switch (item_type)
        {
            case Item_Type.Gold: return "금화";
            case Item_Type.Jade: return "옥";
            case Item_Type.GrowthStone: return "수련의돌";
            case Item_Type.Memory: return "무공비급";
            case Item_Type.Ticket: return "소환서";
            case Item_Type.costume0: return TableManager.Instance.Costume.dataArray[0].Name;
            case Item_Type.costume1: return TableManager.Instance.Costume.dataArray[1].Name;
            case Item_Type.costume2: return TableManager.Instance.Costume.dataArray[2].Name;
            case Item_Type.costume3: return TableManager.Instance.Costume.dataArray[3].Name;
            case Item_Type.costume4: return TableManager.Instance.Costume.dataArray[4].Name;
            case Item_Type.costume5: return TableManager.Instance.Costume.dataArray[5].Name;
            case Item_Type.costume6: return TableManager.Instance.Costume.dataArray[6].Name;
            case Item_Type.costume7: return TableManager.Instance.Costume.dataArray[7].Name;
            case Item_Type.costume8: return TableManager.Instance.Costume.dataArray[8].Name;
            case Item_Type.costume9: return TableManager.Instance.Costume.dataArray[9].Name;
            case Item_Type.costume10: return TableManager.Instance.Costume.dataArray[10].Name;
            case Item_Type.costume11: return TableManager.Instance.Costume.dataArray[11].Name;
            case Item_Type.costume12: return TableManager.Instance.Costume.dataArray[12].Name;
            case Item_Type.costume13: return TableManager.Instance.Costume.dataArray[13].Name;
            case Item_Type.costume14: return TableManager.Instance.Costume.dataArray[14].Name;
            case Item_Type.costume15: return TableManager.Instance.Costume.dataArray[15].Name;
            case Item_Type.costume16: return TableManager.Instance.Costume.dataArray[16].Name;
            case Item_Type.costume17: return TableManager.Instance.Costume.dataArray[17].Name;
            case Item_Type.costume18: return TableManager.Instance.Costume.dataArray[18].Name;
            case Item_Type.costume19: return TableManager.Instance.Costume.dataArray[19].Name;
            case Item_Type.costume20: return TableManager.Instance.Costume.dataArray[20].Name;
            case Item_Type.costume21: return TableManager.Instance.Costume.dataArray[21].Name;
            case Item_Type.costume22: return TableManager.Instance.Costume.dataArray[22].Name;

            case Item_Type.costume23: return TableManager.Instance.Costume.dataArray[23].Name;
            case Item_Type.costume24: return TableManager.Instance.Costume.dataArray[24].Name;
            case Item_Type.costume25: return TableManager.Instance.Costume.dataArray[25].Name;
            case Item_Type.costume26: return TableManager.Instance.Costume.dataArray[26].Name;
            case Item_Type.costume27: return TableManager.Instance.Costume.dataArray[27].Name;
            case Item_Type.costume28: return TableManager.Instance.Costume.dataArray[28].Name;
            case Item_Type.costume29: return TableManager.Instance.Costume.dataArray[29].Name;
            case Item_Type.costume30: return TableManager.Instance.Costume.dataArray[30].Name;
            case Item_Type.costume31: return TableManager.Instance.Costume.dataArray[31].Name;
            case Item_Type.costume32: return TableManager.Instance.Costume.dataArray[32].Name;
            case Item_Type.costume33: return TableManager.Instance.Costume.dataArray[33].Name;
            case Item_Type.costume34: return TableManager.Instance.Costume.dataArray[34].Name;
            case Item_Type.costume35: return TableManager.Instance.Costume.dataArray[35].Name;
            case Item_Type.costume36: return TableManager.Instance.Costume.dataArray[36].Name;
            case Item_Type.costume37: return TableManager.Instance.Costume.dataArray[37].Name;
            case Item_Type.costume38: return TableManager.Instance.Costume.dataArray[38].Name;
            case Item_Type.costume39: return TableManager.Instance.Costume.dataArray[39].Name;
            case Item_Type.costume40: return TableManager.Instance.Costume.dataArray[40].Name;
            case Item_Type.costume41: return TableManager.Instance.Costume.dataArray[41].Name;
            case Item_Type.costume42: return TableManager.Instance.Costume.dataArray[42].Name;
            case Item_Type.costume43: return TableManager.Instance.Costume.dataArray[43].Name;
            case Item_Type.costume44: return TableManager.Instance.Costume.dataArray[44].Name;
            case Item_Type.costume45: return TableManager.Instance.Costume.dataArray[45].Name;
            case Item_Type.costume46: return TableManager.Instance.Costume.dataArray[46].Name;
            case Item_Type.costume47: return TableManager.Instance.Costume.dataArray[47].Name;
            case Item_Type.costume48: return TableManager.Instance.Costume.dataArray[48].Name;
            case Item_Type.costume49: return TableManager.Instance.Costume.dataArray[49].Name;
            case Item_Type.costume50: return TableManager.Instance.Costume.dataArray[50].Name;
            case Item_Type.costume51: return TableManager.Instance.Costume.dataArray[51].Name;
            case Item_Type.costume52: return TableManager.Instance.Costume.dataArray[52].Name;
            case Item_Type.costume53: return TableManager.Instance.Costume.dataArray[53].Name;
            case Item_Type.costume54: return TableManager.Instance.Costume.dataArray[54].Name;
            case Item_Type.costume55: return TableManager.Instance.Costume.dataArray[55].Name;
            case Item_Type.costume56: return TableManager.Instance.Costume.dataArray[56].Name;
            case Item_Type.costume57: return TableManager.Instance.Costume.dataArray[57].Name;
            case Item_Type.costume58: return TableManager.Instance.Costume.dataArray[58].Name;
            case Item_Type.costume59: return TableManager.Instance.Costume.dataArray[59].Name;
            case Item_Type.costume60: return TableManager.Instance.Costume.dataArray[60].Name;
            case Item_Type.costume61: return TableManager.Instance.Costume.dataArray[61].Name;

            case Item_Type.costume62: return TableManager.Instance.Costume.dataArray[62].Name;
            case Item_Type.costume63: return TableManager.Instance.Costume.dataArray[63].Name;
            case Item_Type.costume64: return TableManager.Instance.Costume.dataArray[64].Name;
            case Item_Type.costume65: return TableManager.Instance.Costume.dataArray[65].Name;
            case Item_Type.costume66: return TableManager.Instance.Costume.dataArray[66].Name;
            case Item_Type.costume67: return TableManager.Instance.Costume.dataArray[67].Name;
            case Item_Type.costume68: return TableManager.Instance.Costume.dataArray[68].Name;
            case Item_Type.costume69: return TableManager.Instance.Costume.dataArray[69].Name;
            case Item_Type.costume70: return TableManager.Instance.Costume.dataArray[70].Name;
            case Item_Type.costume71: return TableManager.Instance.Costume.dataArray[71].Name;
            case Item_Type.costume72: return TableManager.Instance.Costume.dataArray[72].Name;

            case Item_Type.costume73: return TableManager.Instance.Costume.dataArray[73].Name;
            case Item_Type.costume74: return TableManager.Instance.Costume.dataArray[74].Name;
            case Item_Type.costume75: return TableManager.Instance.Costume.dataArray[75].Name;
            case Item_Type.costume76: return TableManager.Instance.Costume.dataArray[76].Name;
            case Item_Type.costume77: return TableManager.Instance.Costume.dataArray[77].Name;
            case Item_Type.costume78: return TableManager.Instance.Costume.dataArray[78].Name;
            case Item_Type.costume79: return TableManager.Instance.Costume.dataArray[79].Name;
            case Item_Type.costume80: return TableManager.Instance.Costume.dataArray[80].Name;
            case Item_Type.costume81: return TableManager.Instance.Costume.dataArray[81].Name;
            case Item_Type.costume82: return TableManager.Instance.Costume.dataArray[82].Name;
            case Item_Type.costume83: return TableManager.Instance.Costume.dataArray[83].Name;
            case Item_Type.costume84: return TableManager.Instance.Costume.dataArray[84].Name;
            case Item_Type.costume85: return TableManager.Instance.Costume.dataArray[85].Name;
            case Item_Type.costume86: return TableManager.Instance.Costume.dataArray[86].Name;
            case Item_Type.costume87: return TableManager.Instance.Costume.dataArray[87].Name;
            case Item_Type.costume88: return TableManager.Instance.Costume.dataArray[88].Name;
            case Item_Type.costume89: return TableManager.Instance.Costume.dataArray[89].Name;
            case Item_Type.costume90: return TableManager.Instance.Costume.dataArray[90].Name;
            case Item_Type.costume91: return TableManager.Instance.Costume.dataArray[91].Name;
            case Item_Type.costume92: return TableManager.Instance.Costume.dataArray[92].Name;
            case Item_Type.costume93: return TableManager.Instance.Costume.dataArray[93].Name;
            case Item_Type.costume94: return TableManager.Instance.Costume.dataArray[94].Name;
            case Item_Type.costume95: return TableManager.Instance.Costume.dataArray[95].Name;
            case Item_Type.costume96: return TableManager.Instance.Costume.dataArray[96].Name;
            case Item_Type.costume97: return TableManager.Instance.Costume.dataArray[97].Name;
            case Item_Type.costume98: return TableManager.Instance.Costume.dataArray[98].Name;
            case Item_Type.costume99: return TableManager.Instance.Costume.dataArray[99].Name;
            case Item_Type.costume100: return TableManager.Instance.Costume.dataArray[100].Name;
            case Item_Type.costume101: return TableManager.Instance.Costume.dataArray[101].Name;
            case Item_Type.costume102: return TableManager.Instance.Costume.dataArray[102].Name;
            case Item_Type.costume103: return TableManager.Instance.Costume.dataArray[103].Name;
            case Item_Type.costume104: return TableManager.Instance.Costume.dataArray[104].Name;
            case Item_Type.costume105: return TableManager.Instance.Costume.dataArray[105].Name;
            case Item_Type.costume106: return TableManager.Instance.Costume.dataArray[106].Name;
            case Item_Type.costume107: return TableManager.Instance.Costume.dataArray[107].Name;
            case Item_Type.costume108: return TableManager.Instance.Costume.dataArray[108].Name;
            case Item_Type.costume109: return TableManager.Instance.Costume.dataArray[109].Name;
            case Item_Type.costume110: return TableManager.Instance.Costume.dataArray[110].Name;
            case Item_Type.costume111: return TableManager.Instance.Costume.dataArray[111].Name;
            case Item_Type.costume112: return TableManager.Instance.Costume.dataArray[112].Name;
            case Item_Type.costume113: return TableManager.Instance.Costume.dataArray[113].Name;
            case Item_Type.costume114: return TableManager.Instance.Costume.dataArray[114].Name;
            case Item_Type.costume115: return TableManager.Instance.Costume.dataArray[115].Name;
            case Item_Type.costume116: return TableManager.Instance.Costume.dataArray[116].Name;
            case Item_Type.costume117: return TableManager.Instance.Costume.dataArray[117].Name;
            case Item_Type.costume118: return TableManager.Instance.Costume.dataArray[118].Name;
            case Item_Type.costume119: return TableManager.Instance.Costume.dataArray[119].Name;
            case Item_Type.costume120: return TableManager.Instance.Costume.dataArray[120].Name;
            case Item_Type.costume121: return TableManager.Instance.Costume.dataArray[121].Name;
            case Item_Type.costume122: return TableManager.Instance.Costume.dataArray[122].Name;
            case Item_Type.costume123: return TableManager.Instance.Costume.dataArray[123].Name;
            case Item_Type.costume124: return TableManager.Instance.Costume.dataArray[124].Name;
            case Item_Type.costume125: return TableManager.Instance.Costume.dataArray[125].Name;
            case Item_Type.costume126: return TableManager.Instance.Costume.dataArray[126].Name;
            case Item_Type.costume127: return TableManager.Instance.Costume.dataArray[127].Name;
            case Item_Type.costume128: return TableManager.Instance.Costume.dataArray[128].Name;
            case Item_Type.costume129: return TableManager.Instance.Costume.dataArray[129].Name;
            case Item_Type.costume130: return TableManager.Instance.Costume.dataArray[130].Name;
            case Item_Type.costume131: return TableManager.Instance.Costume.dataArray[131].Name;
            case Item_Type.costume132: return TableManager.Instance.Costume.dataArray[132].Name;
            case Item_Type.costume133: return TableManager.Instance.Costume.dataArray[133].Name;
            case Item_Type.costume134: return TableManager.Instance.Costume.dataArray[134].Name;
            case Item_Type.costume135: return TableManager.Instance.Costume.dataArray[135].Name;
            case Item_Type.costume136: return TableManager.Instance.Costume.dataArray[136].Name;
            case Item_Type.costume137: return TableManager.Instance.Costume.dataArray[137].Name;
            case Item_Type.costume138: return TableManager.Instance.Costume.dataArray[138].Name;
            case Item_Type.costume139: return TableManager.Instance.Costume.dataArray[139].Name;
            case Item_Type.costume140: return TableManager.Instance.Costume.dataArray[140].Name;
            case Item_Type.costume141: return TableManager.Instance.Costume.dataArray[141].Name;
            case Item_Type.costume142: return TableManager.Instance.Costume.dataArray[142].Name;
            case Item_Type.costume143: return TableManager.Instance.Costume.dataArray[143].Name;
            case Item_Type.costume144: return TableManager.Instance.Costume.dataArray[144].Name;
            case Item_Type.costume145: return TableManager.Instance.Costume.dataArray[145].Name;
            case Item_Type.costume146: return TableManager.Instance.Costume.dataArray[146].Name;
            case Item_Type.costume147: return TableManager.Instance.Costume.dataArray[147].Name;
            case Item_Type.costume148: return TableManager.Instance.Costume.dataArray[148].Name;
            case Item_Type.costume149: return TableManager.Instance.Costume.dataArray[149].Name;
            case Item_Type.costume150: return TableManager.Instance.Costume.dataArray[150].Name;
            case Item_Type.costume151: return TableManager.Instance.Costume.dataArray[151].Name;
            case Item_Type.costume152: return TableManager.Instance.Costume.dataArray[152].Name;
            case Item_Type.costume153: return TableManager.Instance.Costume.dataArray[153].Name;
            case Item_Type.costume154: return TableManager.Instance.Costume.dataArray[154].Name;

            case Item_Type.pet0: return TableManager.Instance.PetDatas[0].Name;
            case Item_Type.pet1: return TableManager.Instance.PetDatas[1].Name;
            case Item_Type.pet2: return TableManager.Instance.PetDatas[2].Name;
            case Item_Type.pet3: return TableManager.Instance.PetDatas[3].Name;
            case Item_Type.Marble: return "여우구슬";
            case Item_Type.MagicStoneBuff: return "기억의파편 버프 +50%(드랍)";
            case Item_Type.weapon12: return "특급 4등급 무기";
            case Item_Type.weapon14: return "특급 2등급 무기";
            //
            case Item_Type.weapon37: return "백운선";
            case Item_Type.weapon38: return "금운선";
            case Item_Type.weapon39: return "홍접선";
            case Item_Type.weapon40: return "화접선";
            case Item_Type.weapon41: return "천성선";
            case Item_Type.weapon42: return "천공선";

            //
            case Item_Type.magicBook11: return "특급 1등급 노리개";
            case Item_Type.skill3: return "전방베기4형 기술";
            case Item_Type.Dokebi: return "도깨비 뿔";
            case Item_Type.SkillPartion: return "기술 조각";
            case Item_Type.WeaponUpgradeStone: return "힘의 증표";
            case Item_Type.PetUpgradeSoul: return "요괴구슬";
            case Item_Type.YomulExchangeStone: return "탐욕의 증표";
            case Item_Type.Songpyeon: return "송편";
            case Item_Type.TigerBossStone: return "강함의 증표";

            case Item_Type.Relic: return "영혼 조각";
            case Item_Type.RelicTicket: return "영혼 열쇠";
            case Item_Type.RabitBossStone: return "영혼의 증표";
            case Item_Type.Event_Item_0: return "눈송이";
            case Item_Type.Event_Item_1: return "벚꽃";
            case Item_Type.StageRelic: return "유물 파편";
            case Item_Type.GuimoonRelic: return "귀문석";
            case Item_Type.GuimoonRelicClearTicket: return "귀문석 소탕권";
            case Item_Type.DragonBossStone: return "천공의 증표";
            case Item_Type.SnakeStone: return "치명의 증표";
            case Item_Type.PeachReal: return "천도 복숭아";
            case Item_Type.HorseStone: return "하늘의 증표";
            case Item_Type.SheepStone: return "폭주석";
            case Item_Type.MonkeyStone: return "지배석";
            case Item_Type.MiniGameReward: return "뽑기권";
            case Item_Type.MiniGameReward2: return "신 뽑기권";
            case Item_Type.GuildReward: return "요괴석";
            case Item_Type.CockStone: return "태양석";
            case Item_Type.DogStone: return "천공석";
            case Item_Type.SulItem: return "설날 복주머니";
            case Item_Type.PigStone: return "십이지석";
            case Item_Type.SmithFire: return "요괴 불꽃";
            case Item_Type.FeelMulStone: return "필멸석";


            case Item_Type.Asura0: return "첫번째팔";
            case Item_Type.Asura1: return "두번째팔";
            case Item_Type.Asura2: return "세번째팔";
            case Item_Type.Asura3: return "네번째팔";
            case Item_Type.Asura4: return "다섯번째팔";
            case Item_Type.Asura5: return "여섯번째팔";

            case Item_Type.Indra0: return "인드라의 힘1";
            case Item_Type.Indra1: return "인드라의 힘2";
            case Item_Type.Indra2: return "인드라의 힘3";
            case Item_Type.IndraPower: return "인드라의 번개";


            case Item_Type.Aduk: return "어둑시니의 뿔";
            case Item_Type.SinSkill0: return "등껍질 부수기";
            case Item_Type.SinSkill1: return "백호 발톱";
            case Item_Type.SinSkill2: return "주작 베기";
            case Item_Type.SinSkill3: return "청룡 베기";
            case Item_Type.LeeMuGiStone: return "여의주";
            case Item_Type.SP: return "검조각";
            case Item_Type.Hae_Norigae: return "해태 노리개 조각";
            case Item_Type.Hae_Pet: return "아기 해태 구슬";
            case Item_Type.Event_Item_SnowMan: return "송편";//(구)팽이
            case Item_Type.NataSkill: return "나타 베기";
            case Item_Type.OrochiSkill: return "오로치 베기";
            case Item_Type.GangrimSkill: return "강림 베기";
            //
            case Item_Type.Sun0: return "선술1";
            case Item_Type.Sun1: return "선술2";
            case Item_Type.Sun2: return "선술3";
            case Item_Type.Sun3: return "선술4";
            case Item_Type.Sun4: return "선술5";

            //
            case Item_Type.Chun0: return "천계술1";
            case Item_Type.Chun1: return "천계술2";
            case Item_Type.Chun2: return "천계술3";
            case Item_Type.Chun3: return "천계술4";
            case Item_Type.Chun4: return "천계술5";
            //
            //
            case Item_Type.DokebiSkill0: return "도깨비술1";
            case Item_Type.DokebiSkill1: return "도깨비술2";
            case Item_Type.DokebiSkill2: return "도깨비술3";
            case Item_Type.DokebiSkill3: return "도깨비술4";
            case Item_Type.DokebiSkill4: return "도깨비술5";
            //
            case Item_Type.FourSkill0: return "사천왕 기술1";
            case Item_Type.FourSkill1: return "사천왕 기술2";
            case Item_Type.FourSkill2: return "사천왕 기술3";
            case Item_Type.FourSkill3: return "사천왕 기술4";

            case Item_Type.FourSkill4: return "사천왕 기술5";
            case Item_Type.FourSkill5: return "사천왕 기술6";
            case Item_Type.FourSkill6: return "사천왕 기술7";
            case Item_Type.FourSkill7: return "사천왕 기술8";
            case Item_Type.FourSkill8: return "사천왕 기술9";
            
            
            case Item_Type.VisionSkill0: return "궁극 기술1";
            case Item_Type.VisionSkill1: return "궁극 기술2";
            case Item_Type.VisionSkill2: return "궁극 기술3";
            case Item_Type.VisionSkill3: return "궁극 기술4";
            case Item_Type.VisionSkill4: return "궁극 기술5";
            case Item_Type.VisionSkill5: return "궁극 기술6";
            case Item_Type.VisionSkill6: return "궁극 기술7";
            case Item_Type.VisionSkill7: return "궁극 기술8";
            case Item_Type.VisionSkill8: return "궁극 기술9";
            case Item_Type.VisionSkill9: return "궁극 기술10";
            case Item_Type.VisionSkill10: return "궁극 기술11";
            case Item_Type.VisionSkill11: return "궁극 기술12";
            case Item_Type.ThiefSkill0: return "도적 기술1";
            case Item_Type.ThiefSkill1: return "도적 기술2";
            case Item_Type.ThiefSkill2: return "도적 기술3";
            case Item_Type.ThiefSkill3: return "도적 기술4";
            case Item_Type.ThiefSkill4: return "도적 기술5";
            case Item_Type.DarkSkill0: return "심연 기술1";
            case Item_Type.DarkSkill1: return "심연 기술2";
            case Item_Type.DarkSkill2: return "심연 기술3";
            case Item_Type.DarkSkill3: return "심연 기술4";
            case Item_Type.DarkSkill4: return "심연 기술5";
            //
            case Item_Type.OrochiTooth0: return "오로치 이빨1";
            case Item_Type.OrochiTooth1: return "오로치 이빨2";

            case Item_Type.gumiho0: return "구미호 꼬리1";
            case Item_Type.gumiho1: return "구미호 꼬리2";
            case Item_Type.gumiho2: return "구미호 꼬리3";
            case Item_Type.gumiho3: return "구미호 꼬리4";
            case Item_Type.gumiho4: return "구미호 꼬리5";
            case Item_Type.gumiho5: return "구미호 꼬리6";
            case Item_Type.gumiho6: return "구미호 꼬리7";
            case Item_Type.gumiho7: return "구미호 꼬리8";
            case Item_Type.gumiho8: return "구미호 꼬리9";

            case Item_Type.h0: return TableManager.Instance.hellAbil.dataArray[0].Name;
            case Item_Type.h1: return TableManager.Instance.hellAbil.dataArray[1].Name;
            case Item_Type.h2: return TableManager.Instance.hellAbil.dataArray[2].Name;
            case Item_Type.h3: return TableManager.Instance.hellAbil.dataArray[3].Name;
            case Item_Type.h4: return TableManager.Instance.hellAbil.dataArray[4].Name;
            case Item_Type.h5: return TableManager.Instance.hellAbil.dataArray[5].Name;
            case Item_Type.h6: return TableManager.Instance.hellAbil.dataArray[6].Name;
            case Item_Type.h7: return TableManager.Instance.hellAbil.dataArray[7].Name;
            case Item_Type.h8: return TableManager.Instance.hellAbil.dataArray[8].Name;
            case Item_Type.h9: return TableManager.Instance.hellAbil.dataArray[9].Name;
            //
            case Item_Type.c0: return TableManager.Instance.hellAbil.dataArray[0].Name;
            case Item_Type.c1: return TableManager.Instance.hellAbil.dataArray[1].Name;
            case Item_Type.c2: return TableManager.Instance.hellAbil.dataArray[2].Name;
            case Item_Type.c3: return TableManager.Instance.hellAbil.dataArray[3].Name;
            case Item_Type.c4: return TableManager.Instance.hellAbil.dataArray[4].Name;
            case Item_Type.c5: return TableManager.Instance.hellAbil.dataArray[5].Name;
            case Item_Type.c6: return TableManager.Instance.hellAbil.dataArray[6].Name;
            
            case Item_Type.d0: return TableManager.Instance.DarkAbil.dataArray[0].Name;
            case Item_Type.d1: return TableManager.Instance.DarkAbil.dataArray[1].Name;
            case Item_Type.d2: return TableManager.Instance.DarkAbil.dataArray[2].Name;
            case Item_Type.d3: return TableManager.Instance.DarkAbil.dataArray[3].Name;
            case Item_Type.d4: return TableManager.Instance.DarkAbil.dataArray[4].Name;
            case Item_Type.d5: return TableManager.Instance.DarkAbil.dataArray[5].Name;
            case Item_Type.d6: return TableManager.Instance.DarkAbil.dataArray[6].Name;
            case Item_Type.d7: return TableManager.Instance.DarkAbil.dataArray[7].Name;

            case Item_Type.Hel: return "불멸석";
            case Item_Type.Ym: return "염주";
            case Item_Type.du: return "저승 명부";
            case Item_Type.Fw: return "분홍 꽃";
            case Item_Type.Cw: return "천계 꽃";
            case Item_Type.Event_Collection: return "수박";//봄나물
            case Item_Type.Event_Collection_All: return "수박 총 획득량";
            case Item_Type.Event_Fall_Gold: return "황금 곶감";
            case Item_Type.Event_NewYear: return "떡국";
            case Item_Type.Event_NewYear_All: return "떡국 총 획득량";
            case Item_Type.Event_Mission: return "서핑 보드";
            case Item_Type.Event_Mission_All: return "서핑보드 총 획득량";
            case Item_Type.Event_Mission2: return "보름달";
            case Item_Type.Event_Mission2_All: return "보름달 총 획득량";
            case Item_Type.pet52: return "요랑";
            case Item_Type.FoxMaskPartial: return "나무조각";
            case Item_Type.DokebiFire: return "도깨비불";
            case Item_Type.DokebiFireKey: return "도깨비불 소탕권";
            case Item_Type.Mileage: return "마일리지";
            case Item_Type.ClearTicket: return "만능 소탕권";
            case Item_Type.HellPower: return "지옥강화석";
            case Item_Type.MonthNorigae0: return "12월 월간 노리개";
            case Item_Type.MonthNorigae1: return "1월 월간 노리개";
            case Item_Type.MonthNorigae2: return "2월 월간 노리개";
            case Item_Type.MonthNorigae3: return "3월 월간 노리개";
            case Item_Type.MonthNorigae4: return "4월 월간 노리개";
            case Item_Type.MonthNorigae5: return "5월 월간 노리개";
            case Item_Type.MonthNorigae6: return "6월 월간 노리개";
            case Item_Type.MonthNorigae7: return "7월 월간 노리개";
            case Item_Type.MonthNorigae8: return "8월 월간 노리개";
            case Item_Type.MonthNorigae9: return "9월 월간 노리개";
            case Item_Type.DokebiTreasure: return "도깨비 보물";
            case Item_Type.SusanoTreasure: return "악의 씨앗";
            case Item_Type.SahyungTreasure: return "사흉구슬";
            case Item_Type.VisionTreasure: return "비전서";
            case Item_Type.DarkTreasure: return "심연의 정수";
            case Item_Type.SinsunTreasure: return "신선의 보옥";
            case Item_Type.GwisalTreasure: return "현상수배 증표";
            case Item_Type.ChunguTreasure: return "천구구슬";
            case Item_Type.SleepRewardItem: return "휴식보상(24시간)";
            case Item_Type.GoldBar: return "백금화";
            case Item_Type.DokebiFireEnhance: return "우두머리 불꽃";
            case Item_Type.SumiFire: return "수미꽃";
            case Item_Type.Tresure: return "도적단 보물";
            case Item_Type.SumiFireKey: return "수미꽃 소탕권";
            case Item_Type.NewGachaEnergy: return "영혼석";
            case Item_Type.weapon81: return "설날 외형 무기";
            case Item_Type.weapon90: return "바람개비 외형 무기";
            case Item_Type.weapon131: return "2주년 외형 무기";
            case Item_Type.DokebiBundle: return "도깨비 보물상자";

            case Item_Type.SinsuRelic: return "황룡의 여의주";
            case Item_Type.HyungsuRelic: return "흑호의 보주";
            case Item_Type.ChunguRelic: return "천신의 보주";
            case Item_Type.FoxRelic: return "여우불씨";
            case Item_Type.FoxRelicClearTicket: return "여우불씨 소탕권";
            case Item_Type.TransClearTicket: return "초월석 소탕권";
            case Item_Type.Event_SA: return "2주년 도토리";
            case Item_Type.EventDice: return "이벤트 주사위";
            case Item_Type.SuhoPetFeed: return "수호환";
            case Item_Type.SuhoPetFeedClear: return "수호환 소탕권";
            case Item_Type.SinsuMarble: return "사신수구슬";
            case Item_Type.GuildTowerClearTicket: return "전갈굴 소탕권";
            case Item_Type.SoulRingClear: return "영혼석 소탕권";
            case Item_Type.GuildTowerHorn: return "독침";
            case Item_Type.Event_HotTime: return "촛불";
            case Item_Type.SealWeaponClear: return "요도 해방서";
            case Item_Type.DosulGoods: return "도술꽃";
            case Item_Type.TransGoods: return "초월석";
            case Item_Type.DosulClear: return "도술꽃 소탕권";
            case Item_Type.MeditationGoods: return "심득 조각";
            case Item_Type.MeditationClearTicket: return "내면세계 입장권";
            case Item_Type.DaesanGoods: return "대산의 정수";

        }
        return "미등록";
    }
    

    public static string GetHellMarkAbilName(int grade)
    {
        switch (grade)
        {
            case 0:
                {
                    return "없음";
                }
                break;
            case 1:
                {
                    return "산원 증표";
                }
                break;
            case 2:
                {
                    return "별장 증표";
                }
                break;
            case 3:
                {
                    return "낭장 증표";
                }
                break;
            case 4:
                {
                    return "중랑장 증표";
                }
                break;
            case 5:
                {
                    return "섭장군 증표";
                }
                break;
            case 6:
                {
                    return "대장군 증표";
                }
                break;
            case 7:
                {
                    return "상장군 증표";
                }
                break;

            default:
                return "미등록";
        }
    }

    public static string GetStatusName(StatusType type)
    {
        switch (type)
        {
            case StatusType.AttackAddPer:
                return "공격력 증가(%)";
            case StatusType.CriticalProb:
                return "크리티컬 확률(%)";
            case StatusType.CriticalDam:
                return "크리티컬 데미지(%)";
            case StatusType.SkillCoolTime:
                return "기술 시전 속도(%)";
            case StatusType.SkillDamage:
                return "추가 기술 데미지(%)";
            case StatusType.MoveSpeed:
                return $"이동 속도 증가";
            case StatusType.DamBalance:
                return "최소데미지 보정(%)";
            case StatusType.HpAddPer:
                return "체력 증가(%)";
            case StatusType.MpAddPer:
                return "마력 증가(%)";
            case StatusType.GoldGainPer:
                return "금화 획득 증가(%)";
            case StatusType.ExpGainPer:
                return "경험치 획득 증가(%)";
            case StatusType.AttackAdd:
                return "공격력";
            case StatusType.Hp:
                return "체력";
            case StatusType.Mp:
                return "마력";
            case StatusType.HpRecover:
                return "5초당 체력 회복(%)";
            case StatusType.MpRecover:
                return "5초당 마력 회복(%)";
            case StatusType.MagicStoneAddPer:
                return "수련의돌 추가 획득(%)";
            case StatusType.Damdecrease:
                return "피해 감소(%)";
            case StatusType.IgnoreDefense:
                return "방어력 무시";
            case StatusType.PenetrateDefense:
                return "초과 방어력당 추가 피해량(%)";
            case StatusType.DashCount:
                return "순보 횟수";
            case StatusType.DropAmountAddPer:
                return "몬스터 전리품 수량 증가(%)";
            case StatusType.BossDamAddPer:
                return "보스 데미지 증가(%)";
            case StatusType.SkillAttackCount:
                return "기술 타격 횟수 증가";
            case StatusType.SuperCritical1Prob:
                return "천공베기 확률(%)";
            case StatusType.SuperCritical1DamPer:
                return "천공베기 피해(%)";
            case StatusType.MarbleAddPer:
                return "여우구슬 추가 획득(%)";
            case StatusType.SuperCritical2DamPer:
                return "필멸 피해(%)";
            case StatusType.growthStoneUp:
                return "수련의돌 추가 획득";
            case StatusType.WeaponHasUp:
                return "무기 보유효과 강화";
            case StatusType.NorigaeHasUp:
                return "노리개 보유효과 강화";
            case StatusType.PetEquipHasUp:
                return "환수장비 보유효과 강화";
            case StatusType.PetEquipProbUp:
                return "환수장비 강화확률 증가";
            case StatusType.DecreaseBossHp:
                return "스테이지 보스 체력 감소(%)";
            case StatusType.SuperCritical3DamPer:
                return "지옥베기 피해(%)";
            case StatusType.SuperCritical4DamPer:
                return "천상베기 피해(%)";
            case StatusType.MonthBuff:
                return "월간훈련 버프";
            case StatusType.FlowerHasValueUpgrade:
                return "천계 꽃 레벨당 천상베기 피해량 증가(%)";
            case StatusType.HellHasValueUpgrade:
                return "지옥불꽃 레벨당 지옥베기 피해량 증가(%)";
            case StatusType.SuperCritical5DamPer:
                return "귀신베기 피해(%)";
            case StatusType.SuperCritical6DamPer:
                return "신수베기 피해(%)";
            case StatusType.SuperCritical7DamPer:
                return "금강베기 피해(%)";
            case StatusType.DokebiFireHasValueUpgrade:
                return "도깨비 불 레벨당 귀신베기 피해량 증가(%)";
            case StatusType.SumiHasValueUpgrade:
                return "수미꽃 레벨당 금강베기 피해량 증가(%)";
            case StatusType.TreasureHasValueUpgrade:
                return "도적단 보물 레벨당 섬광베기 피해량 증가(%)";
            case StatusType.DarkHasValueUpgrade:
                return "심연의 정수 레벨당 심연베기 피해량 증가(%)";
            case StatusType.SinsunHasValueUpgrade:
                return "신선의 보옥 레벨당 신선베기 피해량 증가(%)";
            case StatusType.HyunsangHasValueUpgrade:
                return "현상수배 증표 갯수당 효과 증가(%)";
            case StatusType.SuperCritical8DamPer:
                return "하단전베기 피해(%)";
            case StatusType.SuperCritical9DamPer:
                return "흉수베기 피해(%)";   
            case StatusType.SuperCritical10DamPer:
                return "섬광베기 피해(%)";  
            
            case StatusType.SuperCritical11DamPer:
                return "수호베기 피해(%)"; 
            case StatusType.SuperCritical12DamPer:
                return "심연베기 피해(%)"; 
            case StatusType.NorigaeGoldAbilUp:
                return "노리개 기본무공 강화효과(%)";
            case StatusType.SuperCritical13DamPer:
                return "중단전베기 피해(%)";
            case StatusType.SuperCritical18DamPer:
                return "상단전베기 피해(%)";
            case StatusType.SuperCritical14DamPer:
                return "여우베기 피해(%)";
            case StatusType.SuperCritical15DamPer:
                return "신선베기 피해(%)";  
            case StatusType.SuperCritical16DamPer:
                return "태극베기 피해(%)";
            case StatusType.SuperCritical17DamPer:
                return "영혼베기 피해(%)";
            case StatusType.GoldBarGainPer:
                return "백금화 획득 증가(%)";
            case StatusType.SuperCritical19DamPer:
                return "귀살베기 피해(%)";
            case StatusType.SuperCritical20DamPer:
                return "천구베기 피해(%)";
            case StatusType.SuperCritical21DamPer:
                return "초월 피해(%)";
            case StatusType.SealSwordDam:
                return "요도 피해량 증가";
            case StatusType.DosulDamPer:
                return "도술 추가 피해량 증가(%)";
            case StatusType.PeachGainPer:
                return "각성 시 복숭아 방치 획득량 증가 (%)";
            case StatusType.HellGainPer:
                return "각성 시 불멸석 방치 획득량 증가 (%)";
            case StatusType.ChunGainPer:
                return "각성 시 천계꽃 방치 획득량 증가 (%)";
            case StatusType.DokebiFireGainPer:
                return "각성 시 도깨비불 방치 획득량 증가(%)";
            case StatusType.SealAttackSpeed:
                return "요도 시전 속도 증가 (%)";
            case StatusType.SuhoGainPer:
                return "수호환 소탕량 증가 (%)";
            case StatusType.FoxRelicGainPer:
                return "여우불 소탕량 증가 (%)";
            case StatusType.PeachAbilUpgradePer:
                return "복숭아 능력치 효과 (%)";
            case StatusType.DosulGainPer:
                return "도술꽃 소탕량 증가 (%)";
            case StatusType.AddSummonYogui:
                return "요괴 추가 소환";
            case StatusType.SuperCritical8AddDam:
                return "하단전 베기 증폭 (%)";
            case StatusType.SuperCritical13AddDam:
                return "중단전 베기 증폭 (%)";
            case StatusType.SuperCritical18AddDam:
                return "상단전 베기 증폭 (%)";
        }

        return "등록필요";
    }

    public static string GetDialogTextName(DialogCharcterType type)
    {
        return "미등록";
    }

    public static string GetGuildGradeName(GuildGrade grade)
    {
        switch (grade)
        {
            case GuildGrade.Member:
                return "문파원";
                break;
            case GuildGrade.ViceMaster:
                return "부문주";
                break;
            case GuildGrade.Master:
                return "문주";
                break;
        }

        return "미등록";
    }
}
