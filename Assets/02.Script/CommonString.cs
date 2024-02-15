using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
    public static string ItemGrade_30 = "용인";
    public static string ItemGrade_31 = "용궁";
    public static string ItemGrade_32 = "???";
    public static string ItemGrade_33 = "???";
    public static string ItemGrade_34 = "극락";
    public static string ItemGrade_35 = "무림";
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
    public static string ItemGrade_30_Norigae = "용인";
    public static string ItemGrade_31_Norigae = "용궁";
    public static string ItemGrade_34_Norigae = "극락";
    public static string ItemGrade_35_Norigae = "무림";

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
    public static string ItemGrade_16_Skill = "신선검";
    public static string ItemGrade_17_Skill = "용인검";
    public static string ItemGrade_18_Skill = "용궁검";

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
    public static string Reward_Before = "이전 보상을 받아주세요!";
    public static string Reward_Can = "조건을 만족하지 못하였습니다.";
    public static string Reward_Has = "이미 보상을 받았습니다.";


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

            case Item_Type.DragonWeapon4:
                return TableManager.Instance.WeaponData[142].Name;
                ;
            case Item_Type.DragonWeapon5:
                return TableManager.Instance.WeaponData[143].Name;
                ;
            case Item_Type.DragonWeapon6:
                return TableManager.Instance.WeaponData[144].Name;
                ;
            case Item_Type.DragonWeapon7:
                return TableManager.Instance.WeaponData[145].Name;
                ;
            case Item_Type.weapon147:
                return TableManager.Instance.WeaponData[147].Name;
                ;
            case Item_Type.weapon148:
                return TableManager.Instance.WeaponData[148].Name;
                
                

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
            case Item_Type.Event_Item_0: return "윷가락";
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
            case Item_Type.Event_Item_SnowMan: return "막대 사탕"; //(구)송편
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
            case Item_Type.VisionSkill12: return "궁극 기술13";
            case Item_Type.VisionSkill13: return "궁극 기술14";
            case Item_Type.VisionSkill14: return "궁극 기술15";
            case Item_Type.VisionSkill15: return "궁극 기술16";
            case Item_Type.VisionSkill16: return "궁극 기술17";
            case Item_Type.VisionSkill17: return "궁극 기술18";
            case Item_Type.VisionSkill18: return "궁극 기술19";
            case Item_Type.VisionSkill19: return "궁극 기술20";
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
            case Item_Type.SinsunSkill0: return "용인 기술1";
            case Item_Type.SinsunSkill1: return "용인 기술2";
            case Item_Type.SinsunSkill2: return "용인 기술3";
            case Item_Type.SinsunSkill3: return "용인 기술4";
            case Item_Type.SinsunSkill4: return "용인 기술5";
            //
            case Item_Type.DragonSkill0: return "신선 기술1";
            case Item_Type.DragonSkill1: return "신선 기술2";
            case Item_Type.DragonSkill2: return "신선 기술3";
            case Item_Type.DragonSkill3: return "신선 기술4";
            case Item_Type.DragonSkill4: return "신선 기술5";
            //
            case Item_Type.DPSkill0: return "용궁 검술1";
            case Item_Type.DPSkill1: return "용궁 검술2";
            case Item_Type.DPSkill2: return "용궁 검술3";
            case Item_Type.DPSkill3: return "용궁 검술4";
            case Item_Type.DPSkill4: return "용궁 검술5";
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
            case Item_Type.Event_Kill1_Item: return "만두"; //봄나물
            case Item_Type.Event_Collection_All: return "만두 총 획득량";
            case Item_Type.Event_Fall_Gold: return "황금 곶감";
            case Item_Type.Event_NewYear: return "떡국";
            case Item_Type.Event_NewYear_All: return "떡국 총 획득량";
            case Item_Type.Event_Mission1: return "설날 까치";
            case Item_Type.Event_Mission1_All: return "설날 까치 총 획득량";
            case Item_Type.Event_Mission2: return "크리스마스 양말";
            case Item_Type.Event_Mission2_All: return "크리스마스 양말 총 획득량";
            case Item_Type.Event_Mission3: return "보름달";
            case Item_Type.Event_Mission3_All: return "보름달 총 획득량";
            case Item_Type.pet52: return TableManager.Instance.PetTable.dataArray[52].Name;
            case Item_Type.pet53: return TableManager.Instance.PetTable.dataArray[53].Name;
            case Item_Type.pet54: return TableManager.Instance.PetTable.dataArray[54].Name;
            case Item_Type.pet55: return TableManager.Instance.PetTable.dataArray[55].Name;
            case Item_Type.pet56: return TableManager.Instance.PetTable.dataArray[56].Name;
            case Item_Type.pet57: return TableManager.Instance.PetTable.dataArray[57].Name;
            case Item_Type.pet58: return TableManager.Instance.PetTable.dataArray[58].Name;
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
            case Item_Type.MonthNorigae10: return "10월 월간 노리개";
            case Item_Type.MonthNorigae11: return "11월 월간 노리개";
            case Item_Type.magicBook116: return TableManager.Instance.MagicBookTable.dataArray[116].Name;
            case Item_Type.magicBook117: return TableManager.Instance.MagicBookTable.dataArray[117].Name;
            case Item_Type.magicBook118: return TableManager.Instance.MagicBookTable.dataArray[118].Name;
            case Item_Type.magicBook119: return TableManager.Instance.MagicBookTable.dataArray[119].Name;
            case Item_Type.magicBook120: return TableManager.Instance.MagicBookTable.dataArray[120].Name;
            case Item_Type.magicBook121: return TableManager.Instance.MagicBookTable.dataArray[121].Name;
            case Item_Type.magicBook122: return TableManager.Instance.MagicBookTable.dataArray[122].Name;
            case Item_Type.magicBook123: return TableManager.Instance.MagicBookTable.dataArray[123].Name;
            case Item_Type.magicBook124: return TableManager.Instance.MagicBookTable.dataArray[124].Name;
            case Item_Type.magicBook125: return TableManager.Instance.MagicBookTable.dataArray[125].Name;
            case Item_Type.magicBook126: return TableManager.Instance.MagicBookTable.dataArray[126].Name;
            case Item_Type.magicBook127: return TableManager.Instance.MagicBookTable.dataArray[127].Name;
            case Item_Type.magicBook128: return TableManager.Instance.MagicBookTable.dataArray[128].Name;
            case Item_Type.magicBook129: return TableManager.Instance.MagicBookTable.dataArray[129].Name;
            case Item_Type.weapon146: return TableManager.Instance.WeaponTable.dataArray[146].Name;
            case Item_Type.DokebiTreasure: return "도깨비 보물";
            case Item_Type.SusanoTreasure: return "악의 씨앗";
            case Item_Type.SahyungTreasure: return "사흉구슬";
            case Item_Type.VisionTreasure: return "비전서";
            case Item_Type.DarkTreasure: return "심연의 정수";
            case Item_Type.SinsunTreasure: return "신선의 보옥";
            case Item_Type.DragonScale: return "용인의 비늘";
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
            case Item_Type.YoPowerGoods: return "요석";
            case Item_Type.TaeguekGoods: return "태극 조각";
            case Item_Type.TaeguekElixir: return "태극 영약";
            case Item_Type.SuhoTreasure: return "수호 구슬";
            case Item_Type.MRT: return "무공 비급";
            case Item_Type.DBT: return "무림 구슬";
            case Item_Type.TransClearTicket: return "초월석 소탕권";
            case Item_Type.Event_SA: return "2주년 도토리";
            case Item_Type.EventDice: return "이벤트 주사위";
            case Item_Type.SuhoPetFeed: return "수호환";
            case Item_Type.SuhoPetFeedClear: return "수호환 소탕권";
            case Item_Type.SinsuMarble: return "사신수구슬";
            case Item_Type.GuildTowerClearTicket: return "전갈굴 소탕권";
            case Item_Type.SoulRingClear: return "영혼석 소탕권";
            case Item_Type.GuildTowerHorn: return "독침";
            case Item_Type.Event_HotTime: return "청룡 복주머니";
            case Item_Type.SealWeaponClear: return "요도 해방서";
            case Item_Type.DosulGoods: return "도술꽃";
            case Item_Type.TransGoods: return "초월석";
            case Item_Type.DosulClear: return "도술꽃 소탕권";
            case Item_Type.MeditationGoods: return "심득 조각";
            case Item_Type.MeditationClearTicket: return "내면세계 입장권";
            case Item_Type.DaesanGoods: return "대산의 정수";
            case Item_Type.HonorGoods: return "명예의 증표";
            case Item_Type.BlackFoxGoods: return "검은 구미호 구슬";
            case Item_Type.BlackFoxClear: return "검은 구미호 구슬 소탕권";
            case Item_Type.ByeolhoGoods: return "수련의 증표";
            case Item_Type.ByeolhoClear: return "수련의 방 입장권";
            case Item_Type.BattleGoods: return "비무 증표";
            case Item_Type.BattleClear: return "비무 대회 입장권";
            case Item_Type.BattleScore: return "비무 점수";
            case Item_Type.DPT: return "거북 문양";
            case Item_Type.GT: return "빙고 티켓";
            case Item_Type.WT: return "적안 마수 소탕권";
            case Item_Type.SG: return "사신수 기운";
            case Item_Type.SC: return "사신수 영약";
            case Item_Type.SB: return "수련서";
        
    }

        if (item_type.IsCostumeItem())
        {
            var idx= int.Parse(item_type.ToString().Substring("costume".Length));
            return TableManager.Instance.Costume.dataArray[idx].Name;

        }
        else if (item_type.IsNorigaeItem())
        {
            var idx= int.Parse(item_type.ToString().Substring("magicBook".Length));
            return TableManager.Instance.MagicBookTable.dataArray[idx].Name;
        }
        else if (item_type.IsWeaponItem())
        {
            var idx= int.Parse(item_type.ToString().Substring("weapon".Length));
            return TableManager.Instance.WeaponData[idx].Name;
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
            case StatusType.DragonHasValueUpgrade:
                return "용인의 비늘 갯수당 효과 증가(%)";
            case StatusType.DragonPalaceHasValueUpgrade:
                return "거북 문양 갯수당 효과 증가(%)";
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
            case StatusType.SuperCritical22DamPer:
                return "[진]귀살베기 피해(%)";
            case StatusType.SuperCritical23DamPer:
                return "심상베기 피해(%)";
            case StatusType.SuperCritical24DamPer:
                return "용베기 피해(%)";
            case StatusType.SuperCritical25DamPer:
                return "요력 피해(%)";
            case StatusType.SuperCritical26DamPer:
                return "진 요도 피해(%)";
            case StatusType.SuperCritical27DamPer:
                return "무공 피해(%)";
            case StatusType.SuperCritical28DamPer:
                return "해신베기 피해(%)";
            case StatusType.SuperCritical29DamPer:
                return "비무 피해(%)";
            case StatusType.SuperCritical30DamPer:
                return "신력 피해(%)";
            case StatusType.SuperCritical31DamPer:
                return "협동 베기(%)";
            case StatusType.SuperCritical32DamPer:
                return "극락 베기(%)";
            case StatusType.SuperCritical33DamPer:
                return "극혈 베기(%)";
            case StatusType.SuperCritical34DamPer:
                return "무림 베기(%)";
            case StatusType.BigiDamPer:
                return "비기 추가 피해량 증가(%)";
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
            case StatusType.MeditationGainPer:
                return "심득 조각 소탕량 증가 (%)";
            case StatusType.YoPowerGoodsGainPer:
                return "요석 획득 증가 (%)";
            case StatusType.TaegeukGoodsGainPer:
                return "태극 조각 획득 증가 (%)";
            case StatusType.SasinsuGoodsGainPer:
                return "사신수 기운 획득 증가 (%)";
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
            case StatusType.StageRelicUpgrade:
                return "유물 복원 강화";
            case StatusType.AddVisionSkillUseCount:
                return "궁극 기술 사용 횟수 증가";
            case StatusType.AddSealSwordSkillHitCount:
                return "요도 타격 횟수 증가";
            case StatusType.ReduceDosulSkillCoolTime:
                return "도술 시간 감소(초)";
            case StatusType.EnhanceVisionSkill:
                return "궁극 기술 강화(%)";
            case StatusType.EnhanceTaegeukCritical:
                return "태극 베기 증폭(%)";
            case StatusType.ReduceSealSwordSkillRequireCount:
                return "요도 시전 기술횟수 감소";
            case StatusType.EnhanceSuhoCritical:
                return "수호베기 증폭(%)";
            case StatusType.EnhanceSinsuCritical:
                return "신수베기 증폭(%)";
            case StatusType.EnhanceHyungsuCritical:
                return "흉수베기 증폭(%)";
            case StatusType.EnhanceSoulCritical:
                return "영혼베기 증폭(%)";
            case StatusType.EnhanceChunguCritical:
                return "천구베기 증폭(%)";
            case StatusType.EnhanceTransCritical:
                return "초월 피해 증폭(%)";
            case StatusType.EnhanceSP:
                return "검기 능력치 효과 증가(%)";
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
    public static string GetJongsung(string str, JongsungType jongsungType)
    {
        return stringUtils.GetJongsung(str, jongsungType);
    }
    private static readonly StringUtils stringUtils = new StringUtils();


}

public enum JongsungType
{
    /// <summary>
    /// 은(는)
    /// </summary>
    Type_EunNeun,
    /// <summary>
    /// 이(가)
    /// </summary>
    Type_IGA,
    /// <summary>
    /// 을(를)
    /// </summary>
    Type_EulRul,
    /// <summary>
    /// 과(와)
    /// </summary>
    Type_GwaWa
}

public class StringUtils
{
    private StringBuilder sb = new StringBuilder();

    private Dictionary<JongsungType, Dictionary<string, string>> textCache =
        new Dictionary<JongsungType, Dictionary<string, string>>();

    private Dictionary<JongsungType, (char, char)> jongsungContainer = new Dictionary<JongsungType, (char, char)>()
    {
        { JongsungType.Type_EunNeun, ('은', '는') },
        { JongsungType.Type_IGA, ('이', '가') },
        { JongsungType.Type_EulRul, ('을', '를') },
        { JongsungType.Type_GwaWa, ('과', '와') },
    };

    public string GetJongsung(string str, JongsungType jongsungType)
    {
        string result = str;

        if (textCache.ContainsKey(jongsungType) == false)
        {
            textCache.Add(jongsungType, new Dictionary<string, string>());
        }

        if (textCache[jongsungType].ContainsKey(str) == true)
        {
            return textCache[jongsungType][str];
        }

        sb.Clear();

        for (int i = 1; i <= str.Length; i++)
        {
            char lastWord = str[str.Length - i];

            if (lastWord >= 0xAC00 && lastWord <= 0xD7A3)
            {
                int localCode = lastWord - 0xAC00;
                int jongCode = localCode % 28;

                if (jongCode == 0)
                {
                    sb.Append(str);
                    sb.Append(jongsungContainer[jongsungType].Item2);

                    result = sb.ToString();
                }
                else
                {
                    sb.Append(str);
                    sb.Append(jongsungContainer[jongsungType].Item1);

                    result = sb.ToString();
                }

                break;
            }
        }

        if (textCache[jongsungType].ContainsKey(str) == false)
        {
            textCache[jongsungType].Add(str, result);
        }

        return textCache[jongsungType][str];
    }
}