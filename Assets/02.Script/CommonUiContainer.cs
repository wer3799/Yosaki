using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CommonUiContainer : SingletonMono<CommonUiContainer>
{
    public List<Sprite> skillGradeFrame;

    public List<Sprite> itemGradeFrame;

    private List<string> itemGradeName_Weapon = new List<string>() { CommonString.ItemGrade_0, 
        CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5,
        CommonString.ItemGrade_6, CommonString.ItemGrade_7, CommonString.ItemGrade_8, CommonString.ItemGrade_9, CommonString.ItemGrade_10,
        CommonString.ItemGrade_11, CommonString.ItemGrade_12, CommonString.ItemGrade_13, CommonString.ItemGrade_14, CommonString.ItemGrade_15, 
        CommonString.ItemGrade_16, CommonString.ItemGrade_17, CommonString.ItemGrade_18, CommonString.ItemGrade_19, CommonString.ItemGrade_20 ,
        CommonString.ItemGrade_21 , CommonString.ItemGrade_22 , CommonString.ItemGrade_23 , CommonString.ItemGrade_24, CommonString.ItemGrade_25,
        CommonString.ItemGrade_26, CommonString.ItemGrade_27, CommonString.ItemGrade_28, CommonString.ItemGrade_29, CommonString.ItemGrade_30,
        CommonString.ItemGrade_31, };
    public List<string> ItemGradeName_Weapon => itemGradeName_Weapon;

    private List<string> itemGradeName_Norigae = new List<string>() { CommonString.ItemGrade_0, 
        CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5_Norigae, //1~5
        CommonString.ItemGrade_6_Norigae, CommonString.ItemGrade_7_Norigae, CommonString.ItemGrade_8_Norigae, CommonString.ItemGrade_9_Norigae, CommonString.ItemGrade_10_Norigae,  //6~10
        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,  //11~15
        CommonString.ItemGrade_11_Norigae, CommonString.ItemGrade_12_Norigae, CommonString.ItemGrade_13_Norigae, string.Empty, string.Empty,//16~20
        string.Empty, CommonString.ItemGrade_22_Norigae, string.Empty,CommonString.ItemGrade_24_Norigae ,string.Empty, //21~25,
        CommonString.ItemGrade_26_Norigae, CommonString.ItemGrade_27_Norigae,CommonString.ItemGrade_28_Norigae,CommonString.ItemGrade_29_Norigae,CommonString.ItemGrade_30_Norigae,
        CommonString.ItemGrade_31_Norigae,
    }; 
    public List<string> ItemGradeName_Norigae => itemGradeName_Norigae;

    private List<string> itemGradeName_Skill = new List<string>() { 
        CommonString.ItemGrade_0,
        CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4_Skill, CommonString.ItemGrade_5_Skill,
        CommonString.ItemGrade_6_Skill, CommonString.ItemGrade_7_Skill, CommonString.ItemGrade_8_Skill, CommonString.ItemGrade_9_Skill, CommonString.ItemGrade_10_Skill,
        CommonString.ItemGrade_11_Skill,string.Empty,string.Empty,string.Empty,string.Empty,
        string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,
        string.Empty,CommonString.ItemGrade_12_Skill,CommonString.ItemGrade_13_Skill,CommonString.ItemGrade_14_Skill,CommonString.ItemGrade_15_Skill,
        string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,
        CommonString.ItemGrade_16_Skill,CommonString.ItemGrade_17_Skill,CommonString.ItemGrade_18_Skill
        
    };
    public List<string> ItemGradeName_Skill => itemGradeName_Skill;

    private List<string> itemGradeName_NewGacha= new List<string>() { 
        CommonString.ItemGrade_0,
        CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5,CommonString.ItemGrade_6_Ring};
    public List<string> ItemGradeName_NewGacha => itemGradeName_NewGacha;  
    
    private List<string> itemGradeName_SealSword= new List<string>() { 
        CommonString.ItemGrade_0,
        CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5,
        CommonString.ItemGrade_6_Ring,CommonString.ItemGrade_6_Norigae,CommonString.ItemGrade_8,CommonString.ItemGrade_9,CommonString.ItemGrade_10,};
    public List<string> ItemGradeName_SealSword => itemGradeName_SealSword;

    public List<Color> itemGradeColor;

    [SerializeField]
    private List<Sprite> costumeThumbnail;

    [SerializeField]
    private List<Sprite> rankFrame;

    [SerializeField]
    public List<Sprite> petEquipment;

    [SerializeField]
    public List<Color> petEquipColor;

    public Sprite GetCostumeThumbnail(int idx)
    {
        if (idx >= costumeThumbnail.Count) return null;

        return costumeThumbnail[idx];
    }



[Header ("Weapon")]

public Sprite IndraWeapon;
public Sprite NataWeapon;
public Sprite OrochiWeapon;
public Sprite MihoWeapon;
public Sprite YeaRaeWeapon;
public Sprite GangrimWeapon;
public Sprite HaeWeapon;
public Sprite ChunWeapon0;
public Sprite ChunWeapon1;
public Sprite ChunWeapon2;
public Sprite ChunWeapon3;

public Sprite DokebiWeapon0;
public Sprite DokebiWeapon1;
public Sprite DokebiWeapon2;
public Sprite DokebiWeapon3;
public Sprite DokebiWeapon4;
public Sprite DokebiWeapon5;
public Sprite DokebiWeapon6;

public Sprite SasinsuWeapon0;
public Sprite SasinsuWeapon1;
public Sprite SasinsuWeapon2;
public Sprite SasinsuWeapon3;

public Sprite DokebiWeapon7;
public Sprite DokebiWeapon8;
public Sprite DokebiWeapon9;


public Sprite SumisanWeapon0;
public Sprite SumisanWeapon1;
public Sprite SumisanWeapon2;
public Sprite SumisanWeapon3;
public Sprite SumisanWeapon4;
public Sprite SumisanWeapon5;
public Sprite SumisanWeapon6;
    
public Sprite SahyungWeapon0;
public Sprite SahyungWeapon1;
public Sprite SahyungWeapon2;
public Sprite SahyungWeapon3;

public Sprite ThiefWeapon0;
public Sprite ThiefWeapon1;
public Sprite ThiefWeapon2;
public Sprite ThiefWeapon3;
        
public Sprite NinjaWeapon0;
public Sprite NinjaWeapon1;
public Sprite NinjaWeapon2;
    
public Sprite KingWeapon0;
public Sprite KingWeapon1;
public Sprite KingWeapon2;
public Sprite KingWeapon3;

public Sprite DarkWeapon0;
public Sprite DarkWeapon1;
public Sprite DarkWeapon2;
public Sprite DarkWeapon3;
public Sprite MasterWeapon0;
public Sprite MasterWeapon1;
public Sprite MasterWeapon2;
public Sprite MasterWeapon3;
public Sprite SinsunWeapon0;
public Sprite SinsunWeapon1;
public Sprite SinsunWeapon2;
public Sprite SinsunWeapon3;
public Sprite SinsunWeapon4;
public Sprite SinsunWeapon5;
public Sprite SinsunWeapon6;
public Sprite SinsunWeapon7;
public Sprite SinsunWeapon8;

public Sprite HyunSangWeapon0;
public Sprite HyunSangWeapon1;
public Sprite HyunSangWeapon2;
public Sprite HyunSangWeapon3;
public Sprite HyunSangWeapon4;
public Sprite HyunSangWeapon5;
public Sprite HyunSangWeapon6;
public Sprite HyunSangWeapon7;
public Sprite HyunSangWeapon8;
public Sprite HyunSangWeapon9;
public Sprite HyunSangWeapon10;
public Sprite HyunSangWeapon11;

public Sprite DragonWeapon0;
public Sprite DragonWeapon1;
public Sprite DragonWeapon2;
public Sprite DragonWeapon3;
public Sprite DragonWeapon4;
public Sprite DragonWeapon5;
public Sprite DragonWeapon6;
public Sprite DragonWeapon7;
public Sprite weapon147;
public Sprite weapon148;
public Sprite weapon149;
public Sprite weapon150;
public Sprite weapon151;
public Sprite weapon152;
public Sprite weapon153;
public Sprite weapon154;
public Sprite weapon155;
public Sprite weapon156;
public Sprite weapon157;
public Sprite weapon158;
    
[Header ("Weapon_View")]

public Sprite ChunSun0;
public Sprite ChunSun1;
public Sprite ChunSun2;
public Sprite RecommendWeapon0;
public Sprite RecommendWeapon1;
public Sprite RecommendWeapon2;
public Sprite RecommendWeapon3;
public Sprite RecommendWeapon4;

public Sprite RecommendWeapon5;
public Sprite RecommendWeapon6;
public Sprite RecommendWeapon7;
public Sprite RecommendWeapon8;
public Sprite RecommendWeapon9;

public Sprite RecommendWeapon10;
public Sprite RecommendWeapon11;
public Sprite RecommendWeapon12;

public Sprite RecommendWeapon13;
public Sprite RecommendWeapon14;
public Sprite RecommendWeapon15;
public Sprite RecommendWeapon16;
public Sprite RecommendWeapon17;
public Sprite RecommendWeapon18;

public Sprite weapon81;

public Sprite RecommendWeapon19;
public Sprite RecommendWeapon20;

public Sprite weapon90;
public Sprite weapon131;

public Sprite RecommendWeapon21;
public Sprite RecommendWeapon22;
[FormerlySerializedAs("PvPWeapon0")] public Sprite weapon146;

    
[Header ("Norigae")]

    public Sprite HaeNorigae;
    public Sprite SamNorigae;
    public Sprite KirinNorigae;
    public Sprite RabitNorigae;
    public Sprite DogNorigae;
    public Sprite MihoNorigae;
    public Sprite ChunMaNorigae;
    public Sprite YeaRaeNorigae;
    public Sprite GangrimNorigae;
    public Sprite ChunNorigae0;
    public Sprite ChunNorigae1;
    public Sprite ChunNorigae2;
    public Sprite ChunNorigae3;
    public Sprite ChunNorigae4;
    public Sprite ChunNorigae5;
    public Sprite ChunNorigae6;

    public Sprite DokebiNorigae0;
    public Sprite DokebiNorigae1;
    public Sprite DokebiNorigae2;
    public Sprite DokebiNorigae3;
    public Sprite DokebiNorigae4;
    public Sprite DokebiNorigae5;
    public Sprite DokebiNorigae6;

    public Sprite DokebiNorigae7;
    public Sprite DokebiNorigae8;
    public Sprite DokebiNorigae9;

    public Sprite SumisanNorigae0;
    public Sprite SumisanNorigae1;
    public Sprite SumisanNorigae2;
    public Sprite SumisanNorigae3;
    public Sprite SumisanNorigae4;
    public Sprite SumisanNorigae5;
    public Sprite SumisanNorigae6;
    
    public Sprite ThiefNorigae0;
    public Sprite ThiefNorigae1;
    public Sprite ThiefNorigae2;
    public Sprite ThiefNorigae3;

    public Sprite NinjaNorigae0;
    public Sprite NinjaNorigae1;
    public Sprite NinjaNorigae2;
    
    public Sprite KingNorigae0;
    public Sprite KingNorigae1;
    public Sprite KingNorigae2;
    public Sprite KingNorigae3;
    
    public Sprite DarkNorigae0;
    public Sprite DarkNorigae1;
    public Sprite DarkNorigae2;
    public Sprite DarkNorigae3;
    
    public Sprite MasterNorigae0;
    public Sprite MasterNorigae1;
    public Sprite MasterNorigae2;
    public Sprite MasterNorigae3;
    
    public Sprite SinsunNorigae0;
    public Sprite SinsunNorigae1;
    public Sprite SinsunNorigae2;
    public Sprite SinsunNorigae3;
    public Sprite SinsunNorigae4;
    public Sprite SinsunNorigae5;
    public Sprite SinsunNorigae6;
    public Sprite SinsunNorigae7;
    public Sprite SinsunNorigae8;
    public Sprite HyunSangNorigae0;
    public Sprite HyunSangNorigae1;
    public Sprite HyunSangNorigae2;
    public Sprite HyunSangNorigae3;
    public Sprite HyunSangNorigae4;
    public Sprite HyunSangNorigae5;
    public Sprite HyunSangNorigae6;
    public Sprite HyunSangNorigae7;
    public Sprite HyunSangNorigae8;
    public Sprite HyunSangNorigae9;
    public Sprite HyunSangNorigae10;
    public Sprite HyunSangNorigae11;
    public Sprite DragonNorigae0;
    public Sprite DragonNorigae1;
    public Sprite DragonNorigae2;
    public Sprite DragonNorigae3;
    public Sprite magicbook118;
    public Sprite magicbook119;
    public Sprite magicbook120;
    public Sprite magicbook121;
    public Sprite magicbook122;
    
    [Header ("Norigae_View")]
    public Sprite MonthNorigae0;
    public Sprite MonthNorigae1;
    public Sprite MonthNorigae2;
    public Sprite MonthNorigae3;
    public Sprite MonthNorigae4;
    public Sprite MonthNorigae5;
    public Sprite MonthNorigae6;
    public Sprite MonthNorigae7;
    public Sprite MonthNorigae8;
    public Sprite MonthNorigae9;
    public Sprite MonthNorigae10;
    public Sprite MonthNorigae11;
    public Sprite RecommendNorigae0;
    public Sprite magicBook116;
    public Sprite magicBook117;
    public Sprite magicbook123;
    public Sprite magicbook124;
    public Sprite magicbook125;
    public Sprite magicbook126;
    public Sprite magicbook127;
    public Sprite magicBook128;
    public Sprite magicBook129;
    public Sprite magicBook130;
    public Sprite magicBook131;

    [Header ("DokebiHorn")]
    public Sprite DokebiHorn0;
    public Sprite DokebiHorn1;
    public Sprite DokebiHorn2;
    public Sprite DokebiHorn3;
    public Sprite DokebiHorn4;
    public Sprite DokebiHorn5;
    public Sprite DokebiHorn6;

    public Sprite DokebiHorn7;
    public Sprite DokebiHorn8;
    public Sprite DokebiHorn9;
    [Header ("Pet")]
    public Sprite HaePet;
    public Sprite SamPet;
    public Sprite Kirin_Pet;
    public Sprite RabitPet;
    public Sprite DogPet;
    public Sprite ChunMaPet;
    public Sprite ChunPet0;
    public Sprite ChunPet1;
    public Sprite ChunPet2;
    public Sprite ChunPet3;
    public Sprite SasinsuPet0;
    public Sprite SasinsuPet1;
    public Sprite SasinsuPet2;
    public Sprite SasinsuPet3;
    public Sprite SasinsuPet4;
    public Sprite SasinsuPet5;
    public Sprite SasinsuPet6;
    public Sprite SasinsuPet7;
    
    public Sprite SahyungPet0;
    public Sprite SahyungPet1;
    public Sprite SahyungPet2;
    public Sprite SahyungPet3;
    
    public Sprite VisionPet0;
    public Sprite VisionPet1;
    public Sprite VisionPet2;
    public Sprite VisionPet3;
    public Sprite FoxPet0;
    public Sprite FoxPet1;
    public Sprite FoxPet2;
    public Sprite FoxPet3;
    public Sprite TigerPet0;
    public Sprite TigerPet1;
    public Sprite TigerPet2;
    public Sprite TigerPet3;
    public Sprite ChunGuPet0;
    public Sprite ChunGuPet1;
    public Sprite ChunGuPet2;
    public Sprite ChunGuPet3;
    [FormerlySerializedAs("EventPet0")] public Sprite pet52;
    public Sprite pet53;
    public Sprite pet54;
    public Sprite pet55;
    public Sprite pet56;
    public Sprite pet57;
    public Sprite pet58;
    public Sprite SpecialSuhoPet0;
    public Sprite SpecialSuhoPet1;
    public Sprite SpecialSuhoPet2;
    public Sprite SpecialSuhoPet3;
    public Sprite SpecialSuhoPet4;
    public Sprite SpecialSuhoPet5;
    public Sprite SpecialSuhoPet6;
    public Sprite SpecialSuhoPet7;
    public Sprite SpecialSuhoPet8;
    public Sprite SpecialSuhoPet9;
    public Sprite SpecialSuhoPet10;
    public Sprite SpecialSuhoPet11;
    public Sprite SpecialSuhoPet12;
    public Sprite SpecialSuhoPet13;


    [Header ("Skill")]

    public Sprite Sun0;
    public Sprite Sun1;
    public Sprite Sun2;
    public Sprite Sun3;
    public Sprite Sun4;

    public Sprite Chun0;
    public Sprite Chun1;
    public Sprite Chun2;
    public Sprite Chun3;
    public Sprite Chun4;

    public Sprite DokebiSkill0;
    public Sprite DokebiSkill1;
    public Sprite DokebiSkill2;
    public Sprite DokebiSkill3;
    public Sprite DokebiSkill4;


    public Sprite FourSkill0;
    public Sprite FourSkill1;
    public Sprite FourSkill2;
    public Sprite FourSkill3;

    public Sprite FourSkill4;
    public Sprite FourSkill5;
    public Sprite FourSkill6;
    public Sprite FourSkill7;
    public Sprite FourSkill8;

    
    public Sprite VisionSkill0;
    public Sprite VisionSkill1;
    public Sprite VisionSkill2;
    public Sprite VisionSkill3;
    public Sprite VisionSkill4;
    public Sprite VisionSkill5;
    public Sprite VisionSkill6;
    public Sprite VisionSkill7;
    public Sprite VisionSkill8;
    public Sprite VisionSkill9;
    public Sprite VisionSkill10;
    public Sprite VisionSkill11;
    public Sprite VisionSkill12;
    public Sprite VisionSkill13;
    public Sprite VisionSkill14;
    public Sprite VisionSkill15;
    public Sprite VisionSkill16;
    public Sprite VisionSkill17;
    public Sprite VisionSkill18;
    
    public Sprite ThiefSkill0;
    public Sprite ThiefSkill1;
    public Sprite ThiefSkill2;
    public Sprite ThiefSkill3;
    public Sprite ThiefSkill4;

    
    public Sprite DarkSkill0;
    public Sprite DarkSkill1;
    public Sprite DarkSkill2;
    public Sprite DarkSkill3;
    public Sprite DarkSkill4;
    
    public Sprite SinsunSkill0;
    public Sprite SinsunSkill1;
    public Sprite SinsunSkill2;
    public Sprite SinsunSkill3;
    public Sprite SinsunSkill4;

    
    public Sprite DragonSkill0;
    public Sprite DragonSkill1;
    public Sprite DragonSkill2;
    public Sprite DragonSkill3;
    public Sprite DragonSkill4;

    public Sprite DPSkill0;
    public Sprite DPSkill1;
    public Sprite DPSkill2;
    public Sprite DPSkill3;
    public Sprite DPSkill4;

    
    [Header ("Goods")]
    
    public Sprite magicStone;
    public Sprite blueStone;
    public Sprite gold;
    public Sprite GoldBar;
    public Sprite memory;
    public Sprite ticket;
    public Sprite marble;
    public Sprite dokebi;
    public Sprite WeaponUpgradeStone;
    public Sprite YomulExchangeStone;
    public Sprite TigerBossStone;
    public Sprite RabitBossStone;
    public Sprite DragonBossStone;
    public Sprite SnakeStone;
    public Sprite HorseStone;
    public Sprite SheepStone;
    public Sprite CockStone;
    public Sprite MonkeyStone;
    public Sprite DogStone;
    public Sprite PigStone;
    public Sprite MiniGameTicket;
    public Sprite MiniGameTicket2;


    public Sprite relicEnter;
    public Sprite SwordPartial;
    public Sprite Hel;
    public Sprite YeoMarble;
    public Sprite du;
    public Sprite Fw;
    public Sprite Cw;
    public Sprite StageRelic;
    public Sprite GuimoonRelic;
    public Sprite GuimoonRelicClearTicket;
    public Sprite Peach;
    public Sprite relic;
    public Sprite FoxMaskPartial;
    public Sprite DokebiFire;
    public Sprite SuhoPetFeed;
    public Sprite SuhoPetFeedClear;
    public Sprite SoulRingClear;
    public Sprite SealWeaponClear;
    public Sprite Mileage;
    public Sprite ClearTicket;
    public Sprite HellPower;
    public Sprite DokebiFireKey;
    public Sprite DokebiTreasure;
    public Sprite DokebiFireEnhance;
    public Sprite SusanoTreasure;
    public Sprite SahyungTreasure;
    public Sprite VisionTreasure;
    public Sprite DarkTreasure;
    public Sprite SinsunTreasure;
    public Sprite DragonScale;
    public Sprite YoPowerGoods;
    public Sprite TaeguekGoods;
    public Sprite TaeguekElixir;
    public Sprite SuhoTreasure;
    
    public Sprite DosulGoods;
    public Sprite TransGoods;
    public Sprite DosulClear;
    public Sprite BlackFoxGoods;
    public Sprite BlackFoxClear;
    public Sprite ByeolhoGoods;
    public Sprite ByeolhoClear;
    public Sprite BattleGoods;
    public Sprite BattleClear;
    public Sprite BattleScore;
    public Sprite DragonPalaceTreasure;
    public Sprite GT;
    public Sprite WT;
    public Sprite SG;
    public Sprite SC;
    public Sprite SB;
    [FormerlySerializedAs("BossTreasure")] 
    public Sprite GwiSalTreasure;
    public Sprite ChunguTreasure;
    [FormerlySerializedAs("MurimTreasure")] public Sprite MRT;
    public Sprite GuildTowerClearTicket;
    public Sprite GuildTowerHorn;
    public Sprite SinsuMarble;
    public Sprite SumiFire;
    public Sprite Tresure;
    public Sprite SumiFireKey;
    public Sprite NewGachaEnergy;
    public Sprite DokebiBundle;
    public Sprite SinsuRelic;
    public Sprite HyungsuRelic;
    public Sprite ChunguRelic;
    public Sprite FoxRelic;
    public Sprite FoxRelicClearTicket;
    public Sprite TransClearTicket;
    public Sprite DaesanGoods;
    public Sprite HonorGoods;
    public Sprite MeditationGoods;
    public Sprite MeditationClearTicket;
    [Header ("Goods_Event")]
    public Sprite Songpyeon;
    public Sprite EventCollection;
    public Sprite EventCollection2;
    public Sprite Event_Fall;
    public Sprite Event_HotTime;
    public Sprite Event_Fall_Gold;
    public Sprite Event_XMas;
    public Sprite Event_Mission1;
    public Sprite Event_Mission2;
    public Sprite Event_Mission3;
    public Sprite EventDice;
    [FormerlySerializedAs("SAEventGoods")] public Sprite Event_SA;
    public List<SkeletonDataAsset> enemySpineAssets;

    public Sprite GuildReward;
    public Sprite SulItem;
    public Sprite FeelMulStone;
    public Sprite SmithFire;
    public Sprite AsuraHand0;
    public Sprite AsuraHand1;
    public Sprite AsuraHand2;
    public Sprite AsuraHand3;
    public Sprite AsuraHand4;
    public Sprite AsuraHand5;

    public Sprite Indra0;
    public Sprite Indra1;
    public Sprite Indra2;
    public Sprite IndraPower;

    public Sprite OrochiTooth0;
    public Sprite OrochiTooth1;

    public Sprite springIcon;
    public Sprite Aduk;

    public Sprite SinSkill0;
    public Sprite SinSkill1;
    public Sprite SinSkill2;
    public Sprite SinSkill3;
    public Sprite LeeMuGiStone;
    public Sprite Event_Item_SnowMan;


    public Sprite NataSkill;
    public Sprite OrochiSkill;
    public Sprite GangrimSkill;
    public Sprite MihoTail;


    public Sprite HellMark0;
    public Sprite HellMark1;
    public Sprite HellMark2;
    public Sprite HellMark3;
    public Sprite HellMark4;
    public Sprite HellMark5;
    public Sprite HellMark6;
    public Sprite HellMark7;
    public Sprite SleepRewardItem;


    public Sprite GetItemIcon(Item_Type type)
    {
        switch (type)
        {
            case Item_Type.Gold:
                return gold;
            case Item_Type.GoldBar:
                return GoldBar;


            case Item_Type.Jade:
                return blueStone;


            case Item_Type.GrowthStone:
                return magicStone;


            case Item_Type.Memory:
                return memory;


            case Item_Type.Ticket:
                return ticket;
            case Item_Type.Marble:
                return marble;
            case Item_Type.Dokebi:
                return dokebi;
            case Item_Type.RankFrame1:
                return rankFrame[8];


            case Item_Type.RankFrame2:
                return rankFrame[7];


            case Item_Type.RankFrame3:
                return rankFrame[6];


            case Item_Type.RankFrame4:
                return rankFrame[5];


            case Item_Type.RankFrame5:
                return rankFrame[4];


            case Item_Type.RankFrame6_20:
                return rankFrame[3];


            case Item_Type.RankFrame21_100:
                return rankFrame[2];


            case Item_Type.RankFrame101_1000:
                return rankFrame[1];


            case Item_Type.RankFrame1001_10000:
                return rankFrame[9];



            case Item_Type.PartyRaidRankFrame1:
                return rankFrame[8];


            case Item_Type.PartyRaidRankFrame2:
                return rankFrame[7];


            case Item_Type.PartyRaidRankFrame3:
                return rankFrame[6];


            case Item_Type.PartyRaidRankFrame4:
                return rankFrame[5];


            case Item_Type.PartyRaidRankFrame5:
                return rankFrame[4];


            case Item_Type.PartyRaidRankFrame6_20:
                return rankFrame[3];


            case Item_Type.PartyRaidRankFrame21_100:
                return rankFrame[2];


            case Item_Type.PartyRaidRankFrame101_1000:
                return rankFrame[1];


            case Item_Type.PartyRaidRankFrame1001_10000:
                return rankFrame[9];


            case Item_Type.MergePartyRaidRankFrame1:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame2:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame3:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame4:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame5:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame6_10:
                return HellMark6;
            case Item_Type.MergePartyRaidRankFrame11_20:
                return HellMark4;
            case Item_Type.MergePartyRaidRankFrame21_50:
                return HellMark3;
            case Item_Type.MergePartyRaidRankFrame51_100:
                return HellMark1;
            case Item_Type.MergePartyRaidRankFrame101_500:
                return HellMark5;
            case Item_Type.MergePartyRaidRankFrame501_1000:
                return HellMark0;
            case Item_Type.MergePartyRaidRankFrame1001_5000:
                return rankFrame[9];

            case Item_Type.MergePartyRaidRankFrame_0_1:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame_0_2:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame_0_3:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame_0_4:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame_0_5:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame_0_6_10:
                return HellMark6;
            case Item_Type.MergePartyRaidRankFrame_0_11_20:
                return HellMark4;
            case Item_Type.MergePartyRaidRankFrame_0_21_50:
                return HellMark3;
            case Item_Type.MergePartyRaidRankFrame_0_51_100:
                return HellMark1;
            case Item_Type.MergePartyRaidRankFrame_0_101_500:
                return HellMark5;
            case Item_Type.MergePartyRaidRankFrame_0_501_1000:
                return HellMark0;
            case Item_Type.MergePartyRaidRankFrame_0_1001_5000:
                return rankFrame[9];
            
            case Item_Type.MergePartyRaidRankFrame_1_1:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame_1_2:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame_1_3:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame_1_4:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame_1_5:
                return HellMark7;
            case Item_Type.MergePartyRaidRankFrame_1_6_10:
                return HellMark6;
            case Item_Type.MergePartyRaidRankFrame_1_11_20:
                return HellMark4;
            case Item_Type.MergePartyRaidRankFrame_1_21_50:
                return HellMark3;
            case Item_Type.MergePartyRaidRankFrame_1_51_100:
                return HellMark1;
            case Item_Type.MergePartyRaidRankFrame_1_101_500:
                return HellMark5;
            case Item_Type.MergePartyRaidRankFrame_1_501_1000:
                return HellMark0;
            case Item_Type.MergePartyRaidRankFrame_1_1001_5000:
                return rankFrame[9];

            case Item_Type.RankFrame1_relic:
            case Item_Type.RankFrame2_relic:
            case Item_Type.RankFrame3_relic:
            case Item_Type.RankFrame4_relic:
            case Item_Type.RankFrame5_relic:
            case Item_Type.RankFrame6_20_relic:
            case Item_Type.RankFrame21_100_relic:
            case Item_Type.RankFrame101_1000_relic:
            case Item_Type.RankFrame1001_10000_relic:
                return relicEnter;



            case Item_Type.RankFrame1_relic_hell:
            case Item_Type.RankFrame2_relic_hell:
            case Item_Type.RankFrame3_relic_hell:
            case Item_Type.RankFrame4_relic_hell:
            case Item_Type.RankFrame5_relic_hell:
            case Item_Type.RankFrame6_20_relic_hell:
            case Item_Type.RankFrame21_100_relic_hell:
            case Item_Type.RankFrame101_1000_relic_hell:
            case Item_Type.RankFrame1001_10000_relic_hell:
                return Hel;


            case Item_Type.RankFrame1_2_war_hell: { return HellMark7; }break;
            case Item_Type.RankFrame3_5_war_hell: { return HellMark6; } break;
            case Item_Type.RankFrame6_20_war_hell: { return HellMark5; } break;
            case Item_Type.RankFrame21_50_war_hell: { return HellMark4; } break;
            case Item_Type.RankFrame51_100_war_hell: { return HellMark3; } break;
            case Item_Type.RankFrame101_1000_war_hell: { return HellMark2; } break;
            case Item_Type.RankFrame1001_10000_war_hell: { return HellMark1; } break;


            case Item_Type.RankFrame1_miniGame:
            case Item_Type.RankFrame2_miniGame:
            case Item_Type.RankFrame3_miniGame:
            case Item_Type.RankFrame4_miniGame:
            case Item_Type.RankFrame5_miniGame:
            case Item_Type.RankFrame6_20_miniGame:
            case Item_Type.RankFrame21_100_miniGame:
            case Item_Type.RankFrame101_1000_miniGame:
            case Item_Type.RankFrame1001_10000_miniGame:
                return MiniGameTicket;



            case Item_Type.RankFrame1_new_miniGame:
            case Item_Type.RankFrame2_new_miniGame:
            case Item_Type.RankFrame3_new_miniGame:
            case Item_Type.RankFrame4_new_miniGame:
            case Item_Type.RankFrame5_new_miniGame:
            case Item_Type.RankFrame6_20_new_miniGame:
            case Item_Type.RankFrame21_100_new_miniGame:
            case Item_Type.RankFrame101_1000_new_miniGame:
            case Item_Type.RankFrame1001_10000_new_miniGame:
                return MiniGameTicket2;
            case Item_Type.UpdateRewardMail:
                return SwordPartial;



            case Item_Type.RankFrame1_guild:
            case Item_Type.RankFrame2_guild:
            case Item_Type.RankFrame3_guild:
            case Item_Type.RankFrame4_guild:
            case Item_Type.RankFrame5_guild:
            case Item_Type.RankFrame6_20_guild:
            case Item_Type.RankFrame21_100_guild:
            case Item_Type.RankFrame101_1000_guild:
                return GuildReward;


            case Item_Type.RedFoxFrame1_guild:     
            case Item_Type.RedFoxFrame2_guild:    
            case Item_Type.RedFoxFrame3_guild:     
            case Item_Type.RedFoxFrame4_guild:     
            case Item_Type.RedFoxFrame5_guild:     
            case Item_Type.RedFoxFrame6_20_guild:  
            case Item_Type.RedFoxFrame21_100_guild:
                return HonorGoods;



            case Item_Type.RankFrame1guild_new:
            case Item_Type.RankFrame2guild_new:
            case Item_Type.RankFrame3guild_new:
            case Item_Type.RankFrame4guild_new:
            case Item_Type.RankFrame5guild_new:
            case Item_Type.RankFrame6_20_guild_new:
            case Item_Type.RankFrame21_50_guild_new:
            case Item_Type.RankFrame51_100_guild_new:
                return GuildReward;



            case Item_Type.RankFrameParty1guild_new:
            case Item_Type.RankFrameParty2guild_new:
            case Item_Type.RankFrameParty3guild_new:
            case Item_Type.RankFrameParty4guild_new:
            case Item_Type.RankFrameParty5guild_new:
            case Item_Type.RankFrameParty6_20_guild_new:
            case Item_Type.RankFrameParty21_50_guild_new:
            case Item_Type.RankFrameParty51_100_guild_new:
                return GuildReward;

            case Item_Type.Sangun_1guild_new:
            case Item_Type.Sangun_2guild_new:
            case Item_Type.Sangun_3guild_new:
            case Item_Type.Sangun_4guild_new:
            case Item_Type.Sangun_5guild_new:
            case Item_Type.Sangun_6_20_guild_new:
            case Item_Type.Sangun_21_100_guild_new:
                return HonorGoods;



            case Item_Type.RankFrame1_boss_new:
            case Item_Type.RankFrame2_boss_new:
            case Item_Type.RankFrame3_boss_new:
            case Item_Type.RankFrame4_boss_new:
            case Item_Type.RankFrame5_boss_new:
            case Item_Type.RankFrame6_10_boss_new:
            case Item_Type.RankFrame10_30_boss_new:
            case Item_Type.RankFrame30_50boss_new:
            case Item_Type.RankFrame50_70_boss_new:
            case Item_Type.RankFrame70_100_boss_new:
            case Item_Type.RankFrame100_200_boss_new:
            case Item_Type.RankFrame200_500_boss_new:
            case Item_Type.RankFrame500_1000_boss_new:
            case Item_Type.RankFrame1000_3000_boss_new:

                return Peach;


            case Item_Type.RankFrame1_boss_GangChul:
            case Item_Type.RankFrame2_boss_GangChul:
            case Item_Type.RankFrame3_boss_GangChul:
            case Item_Type.RankFrame4_boss_GangChul:
            case Item_Type.RankFrame5_boss_GangChul:
            case Item_Type.RankFrame6_10_boss_GangChul:
            case Item_Type.RankFrame10_30_boss_GangChul:
            case Item_Type.RankFrame30_50_boss_GangChul:
            case Item_Type.RankFrame50_70_boss_GangChul:
            case Item_Type.RankFrame70_100_boss_GangChul:
            case Item_Type.RankFrame100_200_boss_GangChul:
            case Item_Type.RankFrame200_500_boss_GangChul:
            case Item_Type.RankFrame500_1000_boss_GangChul:
            case Item_Type.RankFrame1000_3000_boss_GangChul:

                return Cw;



            case Item_Type.WeaponUpgradeStone:
                return WeaponUpgradeStone;


            case Item_Type.YomulExchangeStone:
                return YomulExchangeStone;


            case Item_Type.Songpyeon:
                return Songpyeon;


            case Item_Type.TigerBossStone:
                return TigerBossStone;
            case Item_Type.RabitBossStone:
                return RabitBossStone;
            case Item_Type.DragonBossStone:
                return DragonBossStone;


            case Item_Type.SnakeStone:
                return SnakeStone;


            case Item_Type.HorseStone:
                return HorseStone;


            case Item_Type.SheepStone:
                return SheepStone;


            case Item_Type.CockStone:
                return CockStone;


            case Item_Type.DogStone:
                return DogStone;


            case Item_Type.PigStone:
                return PigStone;


            case Item_Type.MonkeyStone:
                return MonkeyStone;


            case Item_Type.MiniGameReward:
                return MiniGameTicket;


            case Item_Type.MiniGameReward2:
                return MiniGameTicket2;


            case Item_Type.Relic:
                return relic;


            case Item_Type.RelicTicket:
                return relicEnter;


            case Item_Type.Event_Item_0:
                return EventCollection;


            case Item_Type.StageRelic:
                return StageRelic;
            
            case Item_Type.GuimoonRelic:
                return GuimoonRelic;
            case Item_Type.GuimoonRelicClearTicket:
                return GuimoonRelicClearTicket;



            case Item_Type.PeachReal:
                return Peach;



            case Item_Type.SP:
                return SwordPartial;



            case Item_Type.Hel:
                return Hel;



            case Item_Type.Ym:
                return YeoMarble;



            case Item_Type.Fw:
                return Fw;


            case Item_Type.Cw:
                return Cw;

 
            case Item_Type.FoxMaskPartial:
                return FoxMaskPartial;


            case Item_Type.DokebiFire:
                return DokebiFire; 
            case Item_Type.SuhoPetFeed:
                return SuhoPetFeed;    
            case Item_Type.SuhoPetFeedClear:
                return SuhoPetFeedClear;
            case Item_Type.SoulRingClear:
                return SoulRingClear;

   
            case Item_Type.SumiFire:
                return SumiFire; 
            case Item_Type.SealWeaponClear:
                return SealWeaponClear;

 
            case Item_Type.Tresure:
                return Tresure;

 
            
            case Item_Type.SinsuRelic:
                return SinsuRelic;
            case Item_Type.HyungsuRelic:
                return HyungsuRelic;
            case Item_Type.ChunguRelic:
                return ChunguRelic;
            case Item_Type.FoxRelic:
                return FoxRelic;
            case Item_Type.FoxRelicClearTicket:
                return FoxRelicClearTicket;
            case Item_Type.TransClearTicket:
                return TransClearTicket;
            case Item_Type.Event_SA:
                return Event_SA;
            case Item_Type.MeditationGoods:
                return MeditationGoods;
            case Item_Type.MeditationClearTicket:
                return MeditationClearTicket;
            case Item_Type.DaesanGoods:
                return DaesanGoods;
            case Item_Type.HonorGoods:
                return HonorGoods;


            case Item_Type.EventDice:
                return EventDice;




            case Item_Type.NewGachaEnergy:
                return NewGachaEnergy;

   
            case Item_Type.DokebiBundle:
                return DokebiBundle;

   
            case Item_Type.HellPower:
                return HellPower;

  
            case Item_Type.DokebiTreasure:
                return DokebiTreasure;


            case Item_Type.DokebiFireEnhance:
                return DokebiFireEnhance;

 
            case Item_Type.SusanoTreasure:
                return SusanoTreasure;

 
            case Item_Type.SahyungTreasure:
                return SahyungTreasure;    
            case Item_Type.VisionTreasure:
                return VisionTreasure;   
            case Item_Type.DarkTreasure:
                return DarkTreasure;   
            case Item_Type.SinsunTreasure:
                return SinsunTreasure;   
            case Item_Type.DragonScale:
                return DragonScale;      
            case Item_Type.YoPowerGoods:
                return YoPowerGoods;   
            case Item_Type.TaeguekGoods:
                return TaeguekGoods;   
            case Item_Type.TaeguekElixir:
                return TaeguekElixir;   
            case Item_Type.SuhoTreasure:
                return SuhoTreasure;   
            case Item_Type.GwisalTreasure:
                return GwiSalTreasure;   
            case Item_Type.ChunguTreasure:
                return ChunguTreasure;  
            case Item_Type.MRT:
                return MRT;   
            case Item_Type.DosulGoods:
                return DosulGoods;   
            case Item_Type.TransGoods:
                return TransGoods;   
            case Item_Type.DosulClear:
                return DosulClear;   
            
            case Item_Type.BlackFoxGoods:
                return BlackFoxGoods;  
            case Item_Type.BlackFoxClear:
                return BlackFoxClear;   
            case Item_Type.ByeolhoGoods:
                return ByeolhoGoods;  
            case Item_Type.ByeolhoClear:
                return ByeolhoClear;  
            case Item_Type.BattleGoods:
                return BattleGoods;  
            case Item_Type.BattleClear:
                return BattleClear;   
            case Item_Type.BattleScore:
                return BattleScore;   
            case Item_Type.GT:
                return GT;   
            case Item_Type.WT:
                return WT;   
            case Item_Type.SG:
                return SG;   
            case Item_Type.SC:
                return SC;   
            case Item_Type.SB:
                return SB;   
            case Item_Type.DPT:
                return DragonPalaceTreasure;   
            
            case Item_Type.GuildTowerClearTicket:
                return GuildTowerClearTicket;  
            case Item_Type.GuildTowerHorn:
                return GuildTowerHorn; 
            
            case Item_Type.SleepRewardItem:
                return SleepRewardItem;   
            case Item_Type.SinsuMarble:
                return SinsuMarble;

 
            case Item_Type.DokebiFireKey:
                return DokebiFireKey;

 
            case Item_Type.SumiFireKey:
                return SumiFireKey;


            case Item_Type.Mileage:
                return Mileage;
            case Item_Type.ClearTicket:
                return ClearTicket;


            case Item_Type.Event_Kill1_Item:
                return Event_Fall;
            case Item_Type.Event_HotTime:
                return Event_HotTime;


            case Item_Type.Event_Fall_Gold:
                return Event_Fall_Gold;


            case Item_Type.Event_NewYear:
                return Event_XMas;


            case Item_Type.Event_Mission1:
                return Event_Mission1;
            case Item_Type.Event_Mission2:
                return Event_Mission2;

            case Item_Type.Event_Mission3:
                return Event_Mission3;



            case Item_Type.du:
                return du;



            case Item_Type.Hae_Norigae:
                return HaeNorigae;



            case Item_Type.Hae_Pet:
                return HaePet;



            case Item_Type.Sam_Norigae:
                return SamNorigae;



            case Item_Type.KirinNorigae:
                return KirinNorigae;


            case Item_Type.DogNorigae:
                return DogNorigae;


            case Item_Type.RabitNorigae:
                return RabitNorigae;


            case Item_Type.YeaRaeNorigae:
                return YeaRaeNorigae;


            case Item_Type.GangrimNorigae:
                return GangrimNorigae;



            case Item_Type.ChunNorigae0:
                return ChunNorigae0;



            case Item_Type.ChunNorigae1:
                return ChunNorigae1;



            case Item_Type.ChunNorigae2:
                return ChunNorigae2;



            case Item_Type.ChunNorigae3:
                return ChunNorigae3;



            case Item_Type.ChunNorigae4:
                return ChunNorigae4;



            case Item_Type.ChunNorigae5:
                return ChunNorigae5;



            case Item_Type.ChunNorigae6:
                return ChunNorigae6;


            //
            case Item_Type.DokebiNorigae0:
                return DokebiNorigae0;


            case Item_Type.DokebiNorigae1:
                return DokebiNorigae1;


            case Item_Type.DokebiNorigae2:
                return DokebiNorigae2;


            case Item_Type.DokebiNorigae3:
                return DokebiNorigae3;


            case Item_Type.DokebiNorigae4:
                return DokebiNorigae4;


            case Item_Type.DokebiNorigae5:
                return DokebiNorigae5;


            case Item_Type.DokebiNorigae6:
                return DokebiNorigae6;


            //
            case Item_Type.DokebiNorigae7:
                return DokebiNorigae7;


            case Item_Type.DokebiNorigae8:
                return DokebiNorigae8;


            case Item_Type.DokebiNorigae9:
                return DokebiNorigae9;


            //
            
            case Item_Type.SumisanNorigae0:
                return SumisanNorigae0;


            case Item_Type.SumisanNorigae1:
                return SumisanNorigae1;


            case Item_Type.SumisanNorigae2:
                return SumisanNorigae2;


            case Item_Type.SumisanNorigae3:
                return SumisanNorigae3;


            case Item_Type.SumisanNorigae4:
                return SumisanNorigae4;


            case Item_Type.SumisanNorigae5:
                return SumisanNorigae5;


            case Item_Type.SumisanNorigae6:
                return SumisanNorigae6;


            case Item_Type.ThiefNorigae0:
                return ThiefNorigae0;


            case Item_Type.ThiefNorigae1:
                return ThiefNorigae1;


            case Item_Type.ThiefNorigae2:
                return ThiefNorigae2;


            case Item_Type.ThiefNorigae3:
                return ThiefNorigae3;


            //
            
            case Item_Type.NinjaNorigae0:
                return NinjaNorigae0;


            case Item_Type.NinjaNorigae1:
                return NinjaNorigae1;


            case Item_Type.NinjaNorigae2:
                return NinjaNorigae2;
            
            case Item_Type.KingNorigae0:
                return KingNorigae0;
            case Item_Type.KingNorigae1:
                return KingNorigae1;
            case Item_Type.KingNorigae2:
                return KingNorigae2;
            case Item_Type.KingNorigae3:
                return KingNorigae3;
            case Item_Type.DarkNorigae0:
                return DarkNorigae0;
            case Item_Type.DarkNorigae1:
                return DarkNorigae1;
            case Item_Type.DarkNorigae2:
                return DarkNorigae2;
            case Item_Type.DarkNorigae3:
                return DarkNorigae3;
            case Item_Type.MasterNorigae0:
                return MasterNorigae0;
            case Item_Type.MasterNorigae1:
                return MasterNorigae1;
            case Item_Type.MasterNorigae2:
                return MasterNorigae2;
            case Item_Type.MasterNorigae3:
                return MasterNorigae3;

            case Item_Type.SinsunNorigae0:
                return SinsunNorigae0;
            case Item_Type.SinsunNorigae1:
                return SinsunNorigae1;
            case Item_Type.SinsunNorigae2:
                return SinsunNorigae2;
            case Item_Type.SinsunNorigae3:
                return SinsunNorigae3;
            case Item_Type.SinsunNorigae4:
                return SinsunNorigae4;

            case Item_Type.SinsunNorigae5:
                return SinsunNorigae5;
            case Item_Type.SinsunNorigae6:
                return SinsunNorigae6;
            case Item_Type.SinsunNorigae7:
                return SinsunNorigae7;
            case Item_Type.SinsunNorigae8:
                return SinsunNorigae8;
//
            
            case Item_Type.HyunSangNorigae0:
                return HyunSangNorigae0;
            case Item_Type.HyunSangNorigae1:
                return HyunSangNorigae1;
            case Item_Type.HyunSangNorigae2:
                return HyunSangNorigae2;
            case Item_Type.HyunSangNorigae3:
                return HyunSangNorigae3;
            case Item_Type.HyunSangNorigae4:
                return HyunSangNorigae4;
            case Item_Type.HyunSangNorigae5:
                return HyunSangNorigae5;
            case Item_Type.HyunSangNorigae6:
                return HyunSangNorigae6;
            case Item_Type.HyunSangNorigae7:
                return HyunSangNorigae7;
            case Item_Type.HyunSangNorigae8:
                return HyunSangNorigae8;
            case Item_Type.HyunSangNorigae9:
                return HyunSangNorigae9;
            case Item_Type.HyunSangNorigae10:
                return HyunSangNorigae10;
            case Item_Type.HyunSangNorigae11:
                return HyunSangNorigae11;
            case Item_Type.DragonNorigae0:
                return DragonNorigae0;
            case Item_Type.DragonNorigae1:
                return DragonNorigae1;
            case Item_Type.DragonNorigae2:
                return DragonNorigae2;
            case Item_Type.DragonNorigae3:
                return DragonNorigae3;
                
            case Item_Type.magicBook118:
                return magicbook118;
            case Item_Type.magicBook119:
                return magicbook119;
            case Item_Type.magicBook120:
                return magicbook120;
            case Item_Type.magicBook121:
                return magicbook121;
            case Item_Type.magicBook122:
                return magicbook122;
            case Item_Type.magicBook123:
                return magicbook123;
            case Item_Type.magicBook124:
                return magicbook124;
            case Item_Type.magicBook125:
                return magicbook125;
            case Item_Type.magicBook126:
                return magicbook126;
            case Item_Type.magicBook127:
                return magicbook127;
            case Item_Type.magicBook128:
                return magicBook128;
            case Item_Type.magicBook129:
                return magicBook129;
            case Item_Type.magicBook130:
                return magicBook130;
            case Item_Type.magicBook131:
                return magicBook131;
            //
            case Item_Type.MonthNorigae0:
                return MonthNorigae0;


            case Item_Type.MonthNorigae1:
                return MonthNorigae1;


            case Item_Type.MonthNorigae2:
                return MonthNorigae2;
            case Item_Type.MonthNorigae3:
                return MonthNorigae3;
            case Item_Type.MonthNorigae4:
                return MonthNorigae4;
            case Item_Type.MonthNorigae5:
                return MonthNorigae5;
            case Item_Type.MonthNorigae6:
                return MonthNorigae6;
            case Item_Type.MonthNorigae7:
                return MonthNorigae7;
            case Item_Type.MonthNorigae8:
                return MonthNorigae8;
            case Item_Type.MonthNorigae9:
                return MonthNorigae9;
            case Item_Type.MonthNorigae10:
                return MonthNorigae10;
            case Item_Type.MonthNorigae11:
                return MonthNorigae11;
            case Item_Type.RecommendNorigae0:
                return RecommendNorigae0;
            case Item_Type.magicBook116:
                return magicBook116;
            case Item_Type.magicBook117:
                return magicBook117;
            //
            //
            case Item_Type.DokebiHorn0:
                return DokebiHorn0;


            case Item_Type.DokebiHorn1:
                return DokebiHorn1;


            case Item_Type.DokebiHorn2:
                return DokebiHorn2;


            case Item_Type.DokebiHorn3:
                return DokebiHorn3;


            case Item_Type.DokebiHorn4:
                return DokebiHorn4;


            case Item_Type.DokebiHorn5:
                return DokebiHorn5;


            case Item_Type.DokebiHorn6:
                return DokebiHorn6;

            case Item_Type.DokebiHorn7:
                return DokebiHorn7;


            case Item_Type.DokebiHorn8:
                return DokebiHorn8;


            case Item_Type.DokebiHorn9:
                return DokebiHorn9;


            //
            case Item_Type.ChunSun0:
                return ChunSun0;



            case Item_Type.ChunSun1:
                return ChunSun1;



            case Item_Type.ChunSun2:
                return ChunSun2;




            //


            case Item_Type.GangrimWeapon:
                return GangrimWeapon;

   
            
            case Item_Type.YeaRaeWeapon:
                return YeaRaeWeapon;



            case Item_Type.HaeWeapon:
                return HaeWeapon;



            case Item_Type.Sam_Pet:
                return SamPet;


            case Item_Type.Kirin_Pet:
                return Kirin_Pet;


            case Item_Type.RabitPet:
                return RabitPet;


            case Item_Type.DogPet:
                return DogPet;



            case Item_Type.ChunMaPet:
                return ChunMaPet;


            case Item_Type.ChunPet0:
                return ChunPet0;


            case Item_Type.ChunPet1:
                return ChunPet1;


            case Item_Type.ChunPet2:
                return ChunPet2;


            case Item_Type.ChunPet3:
                return ChunPet3;


            case Item_Type.SasinsuPet0:
                return SasinsuPet0;
            case Item_Type.SasinsuPet1:
                return SasinsuPet1;
            case Item_Type.SasinsuPet2:
                return SasinsuPet2;
            case Item_Type.SasinsuPet3:
                return SasinsuPet3;            
            case Item_Type.SasinsuPet4:
                return SasinsuPet4;
            case Item_Type.SasinsuPet5:
                return SasinsuPet5;
            case Item_Type.SasinsuPet6:
                return SasinsuPet6;
            case Item_Type.SasinsuPet7:
                return SasinsuPet7;


            case Item_Type.SahyungPet0:
                return SahyungPet0;
            case Item_Type.SahyungPet1:
                return SahyungPet1;
            case Item_Type.SahyungPet2:
                return SahyungPet2;
            case Item_Type.SahyungPet3:
                return SahyungPet3;
            
            case Item_Type.VisionPet0:
                return VisionPet0;
            case Item_Type.VisionPet1:
                return VisionPet1;
            case Item_Type.VisionPet2:
                return VisionPet2;
            case Item_Type.VisionPet3:
                return VisionPet3;
            
            case Item_Type.FoxPet0:
                return FoxPet0;
            case Item_Type.FoxPet1:
                return FoxPet1;
            case Item_Type.FoxPet2:
                return FoxPet2;
            case Item_Type.FoxPet3:
                return FoxPet3;
            
            case Item_Type.TigerPet0:
                return TigerPet0;
            case Item_Type.TigerPet1:
                return TigerPet1;
            case Item_Type.TigerPet2:
                return TigerPet2;
            case Item_Type.TigerPet3:
                return TigerPet3;
            
            case Item_Type.ChunGuPet0:
                return ChunGuPet0;
            case Item_Type.ChunGuPet1:
                return ChunGuPet1;
            case Item_Type.ChunGuPet2:
                return ChunGuPet2;
            case Item_Type.ChunGuPet3:
                return ChunGuPet3;
            
            case Item_Type.pet52:
                return pet52;
            case Item_Type.pet53:
                return pet53;
            case Item_Type.pet54:
                return pet54;
            case Item_Type.pet55:
                return pet55;
            case Item_Type.pet56:
                return pet56;
            case Item_Type.pet57:
                return pet57;
            case Item_Type.pet58:
                return pet58;
            case Item_Type.SpecialSuhoPet0:
                return SpecialSuhoPet0;
            case Item_Type.SpecialSuhoPet1:
                return SpecialSuhoPet1;
            case Item_Type.SpecialSuhoPet2:
                return SpecialSuhoPet2;
            case Item_Type.SpecialSuhoPet3:
                return SpecialSuhoPet3;
            case Item_Type.SpecialSuhoPet4:
                return SpecialSuhoPet4;
            case Item_Type.SpecialSuhoPet5:
                return SpecialSuhoPet5;
            case Item_Type.SpecialSuhoPet6:
                return SpecialSuhoPet6;
            case Item_Type.SpecialSuhoPet7:
                return SpecialSuhoPet7;
            case Item_Type.SpecialSuhoPet8:
                return SpecialSuhoPet8;
            case Item_Type.SpecialSuhoPet9:
                return SpecialSuhoPet9;
            case Item_Type.SpecialSuhoPet10:
                return SpecialSuhoPet10;
            case Item_Type.SpecialSuhoPet11:
                return SpecialSuhoPet11;
            case Item_Type.SpecialSuhoPet12:
                return SpecialSuhoPet12;
            case Item_Type.SpecialSuhoPet13:
                return SpecialSuhoPet13;



            case Item_Type.GuildReward:
                return GuildReward;


            case Item_Type.SulItem:
                return SulItem;


            case Item_Type.SmithFire:
                return SmithFire;


            case Item_Type.FeelMulStone:
                return FeelMulStone;



            case Item_Type.Asura0:
                return AsuraHand0;


            case Item_Type.Asura1:
                return AsuraHand1;


            case Item_Type.Asura2:
                return AsuraHand2;


            case Item_Type.Asura3:
                return AsuraHand3;


            case Item_Type.Asura4:
                return AsuraHand4;


            case Item_Type.Asura5:
                return AsuraHand5;


            //
            case Item_Type.Indra0:
                return Indra0;


            case Item_Type.Indra1:
                return Indra1;


            case Item_Type.Indra2:
                return Indra2;


            case Item_Type.IndraPower:
                return IndraPower;



            case Item_Type.OrochiTooth0:
                return OrochiTooth0;


            case Item_Type.OrochiTooth1:
                return OrochiTooth1;



            //
            case Item_Type.Aduk:
                return Aduk;


            case Item_Type.Event_Item_1:
                return springIcon;


            case Item_Type.Event_Item_SnowMan:
                return Event_Item_SnowMan;


            //
            case Item_Type.SinSkill0:
                return SinSkill0;


            case Item_Type.SinSkill1:
                return SinSkill1;


            case Item_Type.SinSkill2:
                return SinSkill2;


            case Item_Type.NataSkill:
                return NataSkill;


            case Item_Type.OrochiSkill:
                return OrochiSkill;


            //
            case Item_Type.Sun0:
                return Sun0;


            case Item_Type.Sun1:
                return Sun1;


            case Item_Type.Sun2:
                return Sun2;


            case Item_Type.Sun3:
                return Sun3;


            case Item_Type.Sun4:
                return Sun4;


            //
            case Item_Type.Chun0:
                return Chun0;


            case Item_Type.Chun1:
                return Chun1;


            case Item_Type.Chun2:
                return Chun2;


            case Item_Type.Chun3:
                return Chun3;


            case Item_Type.Chun4:
                return Chun4;


            //
            //
            case Item_Type.DokebiSkill0:
                return DokebiSkill0;


            case Item_Type.DokebiSkill1:
                return DokebiSkill1;


            case Item_Type.DokebiSkill2:
                return DokebiSkill2;


            case Item_Type.DokebiSkill3:
                return DokebiSkill3;


            case Item_Type.DokebiSkill4:
                return DokebiSkill4;


            //
            //
            case Item_Type.FourSkill0:
                return FourSkill0;
            case Item_Type.FourSkill1:
                return FourSkill1;
            case Item_Type.FourSkill2:
                return FourSkill2;
            case Item_Type.FourSkill3:
                return FourSkill3;
            //
            case Item_Type.FourSkill4:
                return FourSkill4;
            case Item_Type.FourSkill5:
                return FourSkill5;
            case Item_Type.FourSkill6:
                return FourSkill6;
            case Item_Type.FourSkill7:
                return FourSkill7;
            case Item_Type.FourSkill8:
                return FourSkill8;
            //
            case Item_Type.VisionSkill0:
                return VisionSkill0;
            case Item_Type.VisionSkill1:
                return VisionSkill1;
            case Item_Type.VisionSkill2:
                return VisionSkill2;
            case Item_Type.VisionSkill3:
                return VisionSkill3;
            case Item_Type.VisionSkill4:
                return VisionSkill4;
            case Item_Type.VisionSkill5:
                return VisionSkill5;
            case Item_Type.VisionSkill6:
                return VisionSkill6;
            case Item_Type.VisionSkill7:
                return VisionSkill7;
            case Item_Type.VisionSkill8:
                return VisionSkill8;
            case Item_Type.VisionSkill9:
                return VisionSkill9;
            case Item_Type.VisionSkill10:
                return VisionSkill10;
            case Item_Type.VisionSkill11:
                return VisionSkill11;
            case Item_Type.VisionSkill12:
                return VisionSkill12;
            case Item_Type.VisionSkill13:
                return VisionSkill13;
            case Item_Type.VisionSkill14:
                return VisionSkill14;
            case Item_Type.VisionSkill15:
                return VisionSkill15;
            case Item_Type.VisionSkill16:
                return VisionSkill16;
            case Item_Type.VisionSkill17:
                return VisionSkill17;
            case Item_Type.VisionSkill18:
                return VisionSkill18;
            //
            //
            case Item_Type.ThiefSkill0:
                return ThiefSkill0;
            case Item_Type.ThiefSkill1:
                return ThiefSkill1;
            case Item_Type.ThiefSkill2:
                return ThiefSkill2;
            case Item_Type.ThiefSkill3:
                return ThiefSkill3;
            case Item_Type.ThiefSkill4:
                return ThiefSkill4;
            //
            case Item_Type.DarkSkill0:
                return DarkSkill0;
            case Item_Type.DarkSkill1:
                return DarkSkill1;
            case Item_Type.DarkSkill2:
                return DarkSkill2;
            case Item_Type.DarkSkill3:
                return DarkSkill3;
            case Item_Type.DarkSkill4:
                return DarkSkill4;
            //
            //
            case Item_Type.SinsunSkill0:
                return SinsunSkill0;
            case Item_Type.SinsunSkill1:
                return SinsunSkill1;
            case Item_Type.SinsunSkill2:
                return SinsunSkill2;
            case Item_Type.SinsunSkill3:
                return SinsunSkill3;
            case Item_Type.SinsunSkill4:
                return SinsunSkill4;
            //
            //
            case Item_Type.DragonSkill0:
                return DragonSkill0;
            case Item_Type.DragonSkill1:
                return DragonSkill1;
            case Item_Type.DragonSkill2:
                return DragonSkill2;
            case Item_Type.DragonSkill3:
                return DragonSkill3;
            case Item_Type.DragonSkill4:
                return DragonSkill4;
            //
            case Item_Type.DPSkill0:
                return DPSkill0;
            case Item_Type.DPSkill1:
                return DPSkill1;
            case Item_Type.DPSkill2:
                return DPSkill2;
            case Item_Type.DPSkill3:
                return DPSkill3;
            case Item_Type.DPSkill4:
                return DPSkill4;
            //
            case Item_Type.GangrimSkill:
                return GangrimSkill;


            case Item_Type.SinSkill3:
                return SinSkill3;


            case Item_Type.LeeMuGiStone:
                return LeeMuGiStone;


            case Item_Type.IndraWeapon:
                return IndraWeapon;


            case Item_Type.NataWeapon:
                return NataWeapon;


            case Item_Type.OrochiWeapon:
                return OrochiWeapon;



            case Item_Type.MihoWeapon:
                return MihoWeapon;


            case Item_Type.ChunWeapon0:
                return ChunWeapon0;


            case Item_Type.ChunWeapon1:
                return ChunWeapon1;


            case Item_Type.ChunWeapon2:
                return ChunWeapon2;


            case Item_Type.ChunWeapon3:
                return ChunWeapon3;


            case Item_Type.DokebiWeapon0:
                return DokebiWeapon0;


            case Item_Type.DokebiWeapon1:
                return DokebiWeapon1;


            case Item_Type.DokebiWeapon2:
                return DokebiWeapon2;


            case Item_Type.DokebiWeapon3:
                return DokebiWeapon3;


            case Item_Type.DokebiWeapon4:
                return DokebiWeapon4;


            case Item_Type.DokebiWeapon5:
                return DokebiWeapon5;


            case Item_Type.DokebiWeapon6:
                return DokebiWeapon6;


            case Item_Type.DokebiWeapon7:
                return DokebiWeapon7;


            case Item_Type.DokebiWeapon8:
                return DokebiWeapon8;


            case Item_Type.DokebiWeapon9:
                return DokebiWeapon9;


            case Item_Type.SumisanWeapon0:
                return SumisanWeapon0;


            case Item_Type.SumisanWeapon1:
                return SumisanWeapon1;


            case Item_Type.SumisanWeapon2:
                return SumisanWeapon2;


            case Item_Type.SumisanWeapon3:
                return SumisanWeapon3;


            case Item_Type.SumisanWeapon4:
                return SumisanWeapon4;


            case Item_Type.SumisanWeapon5:
                return SumisanWeapon5;


            case Item_Type.SumisanWeapon6:
                return SumisanWeapon6;


            case Item_Type.ThiefWeapon0:
                return ThiefWeapon0;
            case Item_Type.ThiefWeapon1:
                return ThiefWeapon1;
            case Item_Type.ThiefWeapon2:
                return ThiefWeapon2;
            case Item_Type.ThiefWeapon3:
                return ThiefWeapon3;

            case Item_Type.NinjaWeapon0:
                return NinjaWeapon0;
            case Item_Type.NinjaWeapon1:
                return NinjaWeapon1;
            case Item_Type.NinjaWeapon2:
                return NinjaWeapon2;


            case Item_Type.KingWeapon0:
                return KingWeapon0;
            case Item_Type.KingWeapon1:
                return KingWeapon1;
            case Item_Type.KingWeapon2:
                return KingWeapon2;
            case Item_Type.KingWeapon3:
                return KingWeapon3;
            
            case Item_Type.DarkWeapon0:
                return DarkWeapon0;
            case Item_Type.DarkWeapon1:
                return DarkWeapon1;
            case Item_Type.DarkWeapon2:
                return DarkWeapon2;
            case Item_Type.DarkWeapon3:
                return DarkWeapon3;
            case Item_Type.MasterWeapon0:
                return MasterWeapon0;
            case Item_Type.MasterWeapon1:
                return MasterWeapon1;
            case Item_Type.MasterWeapon2:
                return MasterWeapon2;
            case Item_Type.MasterWeapon3:
                return MasterWeapon3;
            case Item_Type.SinsunWeapon0:
                return SinsunWeapon0;
            case Item_Type.SinsunWeapon1:
                return SinsunWeapon1;
            case Item_Type.SinsunWeapon2:
                return SinsunWeapon2;
            case Item_Type.SinsunWeapon3:
                return SinsunWeapon3;
            case Item_Type.SinsunWeapon4:
                return SinsunWeapon4;
            case Item_Type.SinsunWeapon5:
                return SinsunWeapon5;
            case Item_Type.SinsunWeapon6:
                return SinsunWeapon6;
            case Item_Type.SinsunWeapon7:
                return SinsunWeapon7;
            case Item_Type.SinsunWeapon8:
                return SinsunWeapon8;

            
            case Item_Type.HyunSangWeapon0:
                return HyunSangWeapon0;
            case Item_Type.HyunSangWeapon1:
                return HyunSangWeapon1;
            case Item_Type.HyunSangWeapon2:
                return HyunSangWeapon2;
            case Item_Type.HyunSangWeapon3:
                return HyunSangWeapon3;
            case Item_Type.HyunSangWeapon4:
                return HyunSangWeapon4;
            case Item_Type.HyunSangWeapon5:
                return HyunSangWeapon5;
            case Item_Type.HyunSangWeapon6:
                return HyunSangWeapon6;
            case Item_Type.HyunSangWeapon7:
                return HyunSangWeapon7;
            case Item_Type.HyunSangWeapon8:
                return HyunSangWeapon8;
            case Item_Type.HyunSangWeapon9:
                return HyunSangWeapon9;
            
            case Item_Type.HyunSangWeapon10:
                return HyunSangWeapon10;
            case Item_Type.HyunSangWeapon11:
                return HyunSangWeapon11;
            case Item_Type.DragonWeapon0:
                return DragonWeapon0;
            case Item_Type.DragonWeapon1:
                return DragonWeapon1;
            case Item_Type.DragonWeapon2:
                return DragonWeapon2;
            case Item_Type.DragonWeapon3:
                return DragonWeapon3;
            case Item_Type.DragonWeapon4:
                return DragonWeapon4;
            case Item_Type.DragonWeapon5:
                return DragonWeapon5;
            case Item_Type.DragonWeapon6:
                return DragonWeapon6;
            case Item_Type.DragonWeapon7:
                return DragonWeapon7;
            case Item_Type.weapon147:
                return weapon147;
            case Item_Type.weapon148:
                return weapon148;
            case Item_Type.weapon149:
                return weapon149;
            case Item_Type.weapon150:
                return weapon150;
            case Item_Type.weapon151:
                return weapon151;
            case Item_Type.weapon152:
                return weapon152;
            case Item_Type.weapon153:
                return weapon153;
            case Item_Type.weapon154:
                return weapon154;
            case Item_Type.weapon155:
                return weapon155;
            case Item_Type.weapon156:
                return weapon156;
            case Item_Type.weapon157:
                return weapon157;
            case Item_Type.weapon158:
                return weapon158;
            
            case Item_Type.SasinsuWeapon0:
                return SasinsuWeapon0;


            case Item_Type.SasinsuWeapon1:
                return SasinsuWeapon1;


            case Item_Type.SasinsuWeapon2:
                return SasinsuWeapon2;


            case Item_Type.SasinsuWeapon3:
                return SasinsuWeapon3;


                
            case Item_Type.SahyungWeapon0:
                return SahyungWeapon0;
            case Item_Type.SahyungWeapon1:
                return SahyungWeapon1;
            case Item_Type.SahyungWeapon2:
                return SahyungWeapon2;
            case Item_Type.SahyungWeapon3:
                return SahyungWeapon3;

            case Item_Type.MihoNorigae:
                return MihoNorigae;



            case Item_Type.ChunMaNorigae:
                return ChunMaNorigae;



            case Item_Type.RecommendWeapon0:
                return RecommendWeapon0;


            case Item_Type.RecommendWeapon1:
                return RecommendWeapon1;


            case Item_Type.RecommendWeapon2:
                return RecommendWeapon2;


            case Item_Type.RecommendWeapon3:
                return RecommendWeapon3;


            case Item_Type.RecommendWeapon4:
                return RecommendWeapon4;


            case Item_Type.RecommendWeapon5:
                return RecommendWeapon5;


            case Item_Type.RecommendWeapon6:
                return RecommendWeapon6;


            case Item_Type.RecommendWeapon7:
                return RecommendWeapon7;


            case Item_Type.RecommendWeapon8:
                return RecommendWeapon8;


            case Item_Type.RecommendWeapon9:
                return RecommendWeapon9;


            case Item_Type.RecommendWeapon10:
                return RecommendWeapon10;


            case Item_Type.RecommendWeapon11:
                return RecommendWeapon11;


            case Item_Type.RecommendWeapon12:
                return RecommendWeapon12;

   
            case Item_Type.RecommendWeapon13:
                return RecommendWeapon13;


            case Item_Type.RecommendWeapon14:
                return RecommendWeapon14;


            case Item_Type.RecommendWeapon15:
                return RecommendWeapon15;


            case Item_Type.RecommendWeapon16:
                return RecommendWeapon16;


            case Item_Type.RecommendWeapon17:
                return RecommendWeapon17;


            case Item_Type.RecommendWeapon18:
                return RecommendWeapon18;


            case Item_Type.RecommendWeapon19:
                return RecommendWeapon19;


            case Item_Type.RecommendWeapon20:
                return RecommendWeapon20;
            case Item_Type.RecommendWeapon21:
                return RecommendWeapon21;


            case Item_Type.RecommendWeapon22:
                return RecommendWeapon22;
            case Item_Type.weapon146:
                return weapon146;


            case Item_Type.weapon81:
                return weapon81;


                
            case Item_Type.weapon90:
                return weapon90;
            case Item_Type.weapon131:
                return weapon131;



            case Item_Type.gumiho0:
            case Item_Type.gumiho1:
            case Item_Type.gumiho2:
            case Item_Type.gumiho3:
            case Item_Type.gumiho4:
            case Item_Type.gumiho5:
            case Item_Type.gumiho6:
            case Item_Type.gumiho7:
            case Item_Type.gumiho8:
                return MihoTail;



            case Item_Type.h0: return CommonResourceContainer.GetHellIconSprite(0);
            case Item_Type.h1: return CommonResourceContainer.GetHellIconSprite(1);
            case Item_Type.h2: return CommonResourceContainer.GetHellIconSprite(2);
            case Item_Type.h3: return CommonResourceContainer.GetHellIconSprite(3);
            case Item_Type.h4: return CommonResourceContainer.GetHellIconSprite(4);
            case Item_Type.h5: return CommonResourceContainer.GetHellIconSprite(5);
            case Item_Type.h6: return CommonResourceContainer.GetHellIconSprite(6);
            case Item_Type.h7: return CommonResourceContainer.GetHellIconSprite(7);
            case Item_Type.h8: return CommonResourceContainer.GetHellIconSprite(8);
            case Item_Type.h9: return CommonResourceContainer.GetHellIconSprite(9);



            case Item_Type.d0: return CommonResourceContainer.GetDarkIconSprite(0);
            case Item_Type.d1: return CommonResourceContainer.GetDarkIconSprite(1);
            case Item_Type.d2: return CommonResourceContainer.GetDarkIconSprite(2);
            case Item_Type.d3: return CommonResourceContainer.GetDarkIconSprite(3);
            case Item_Type.d4: return CommonResourceContainer.GetDarkIconSprite(4);
            case Item_Type.d5: return CommonResourceContainer.GetDarkIconSprite(5);
            case Item_Type.d6: return CommonResourceContainer.GetDarkIconSprite(6);
            case Item_Type.d7: return CommonResourceContainer.GetDarkIconSprite(7);

            case Item_Type.c0: return CommonResourceContainer.GetChunIconSprite(0);
            case Item_Type.c1: return CommonResourceContainer.GetChunIconSprite(1);
            case Item_Type.c2: return CommonResourceContainer.GetChunIconSprite(2);
            case Item_Type.c3: return CommonResourceContainer.GetChunIconSprite(3);
            case Item_Type.c4: return CommonResourceContainer.GetChunIconSprite(4);
            case Item_Type.c5: return CommonResourceContainer.GetChunIconSprite(5);
            case Item_Type.c6:
                return CommonResourceContainer.GetChunIconSprite(6);



        }
        if (type.IsCostumeItem())
        {
            var idx= int.Parse(type.ToString().Substring("costume".Length));
            return costumeThumbnail[idx];

        }
        return null;
    }

    public List<Sprite> statusIcon;

    public List<Sprite> loadingTipIcon;

    public List<Sprite> bossIcon;

    public List<SkeletonDataAsset> costumeList;

    public List<SkeletonDataAsset> petCostumeList;

    public List<GameObject> wingList;

    public List<Sprite> buffIconList;

    public List<Sprite> relicIconList;
    public List<Sprite> blackFoxIconList;
    public List<Sprite> stageRelicIconList;
    public List<Sprite> guimoonIcon1List;
    public List<Sprite> guimoonIcon2List;
    

    public List<RuntimeAnimatorController> sonAnimators;
    public List<Sprite> sonThumbNail;

    public List<Sprite> guildIcon;
    public List<int> guildIconGrade;

    public List<Material> weaponEnhnaceMats;
}
