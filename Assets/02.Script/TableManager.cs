using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using UnityEngine.Serialization;

public class TableManager : SingletonMono<TableManager>
{
    [SerializeField] private EnemyTable enemyTable;
    public EnemyTable EnemyTable => enemyTable;

    private Dictionary<int, EnemyTableData> enemyData = null;

    public Dictionary<int, EnemyTableData> EnemyData
    {
        get
        {
            LoadEnemyData();
            return enemyData;
        }
    }

    private void LoadEnemyData()
    {
        if (enemyData != null) return;

        enemyData = new Dictionary<int, EnemyTableData>();

        for (int i = 0; i < enemyTable.dataArray.Length; i++)
        {
            enemyData.Add(enemyTable.dataArray[i].Id, enemyTable.dataArray[i]);
        }
    }


    [SerializeField] private SkillTable skillTable;
    public SkillTable SkillTable => skillTable;
    private Dictionary<int, SkillTableData> skillData = null;

    public Dictionary<int, SkillTableData> SkillData
    {
        get
        {
            LoadSkillData();
            return skillData;
        }
    }

    private void LoadSkillData()
    {
        if (skillData != null) return;

        skillData = new Dictionary<int, SkillTableData>();

        for (int i = 0; i < skillTable.dataArray.Length; i++)
        {
            skillData.Add(skillTable.dataArray[i].Id, skillTable.dataArray[i]);
        }
    }

    [SerializeField] private NewGachaTable newGachaTable;
    public NewGachaTable NewGachaTable => newGachaTable;
    private Dictionary<int, NewGachaTableData> newGachaData = null;

    public Dictionary<int, NewGachaTableData> NewGachaData
    {
        get
        {
            LoadNewGachaData();
            return newGachaData;
        }
    }

    private void LoadNewGachaData()
    {
        if (newGachaData != null) return;

        newGachaData = new Dictionary<int, NewGachaTableData>();

        for (int i = 0; i < newGachaTable.dataArray.Length; i++)
        {
            newGachaData.Add(newGachaTable.dataArray[i].Id, newGachaTable.dataArray[i]);
        }
    }

    [SerializeField] private Weapon weaponTable;
    public Weapon WeaponTable => weaponTable;

    private Dictionary<int, WeaponData> weaponData = null;

    public Dictionary<int, WeaponData> WeaponData
    {
        get
        {
            LoadWeaponData();
            return weaponData;
        }
    }

    public WeaponData GetWeaponDataByStringId(string id)
    {
        var e = WeaponData.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Current.Value.Stringid == id) return e.Current.Value;
        }

        return null;
    }

    private void LoadWeaponData()
    {
        if (weaponData != null) return;

        weaponData = new Dictionary<int, WeaponData>();

        for (int i = 0; i < weaponTable.dataArray.Length; i++)
        {
            weaponData.Add(weaponTable.dataArray[i].Id, weaponTable.dataArray[i]);
        }
    }

    [SerializeField] private StageMap stageMapTable;
    public StageMap StageMapTable => stageMapTable;

    private Dictionary<int, StageMapData> stageMapData = null;

    public Dictionary<int, StageMapData> StageMapData
    {
        get
        {
            LoadStageMapData();
            return stageMapData;
        }
    }

    public int GetLastStageIdx()
    {
        return stageMapTable.dataArray[stageMapTable.dataArray.Length - 1].Id;
    }

    public int GetLastStagePreset()
    {
        return stageMapTable.dataArray[stageMapTable.dataArray.Length - 1].Chapteridx;
    }

    private void LoadStageMapData()
    {
        if (stageMapData != null) return;

        stageMapData = new Dictionary<int, StageMapData>();

        for (int i = 0; i < stageMapTable.dataArray.Length; i++)
        {
            stageMapData.Add(stageMapTable.dataArray[i].Id, stageMapTable.dataArray[i]);
        }
    }

    [SerializeField] private DailyMission dailyMission;

    public DailyMission DailyMission => dailyMission;

    private Dictionary<int, DailyMissionData> dailyMissionDatas;

    public Dictionary<int, DailyMissionData> DailyMissionDatas
    {
        get
        {
            if (dailyMissionDatas == null)
            {
                dailyMissionDatas = new Dictionary<int, DailyMissionData>();

                for (int i = 0; i < dailyMission.dataArray.Length; i++)
                {
                    dailyMissionDatas.Add(dailyMission.dataArray[i].Id, dailyMission.dataArray[i]);
                }
            }

            return dailyMissionDatas;
        }
    }

    [SerializeField] private YorinMission yorinMission;
    public YorinMission YorinMission => yorinMission;
    private Dictionary<int, YorinMissionData> yorinMissionDatas;
    
    public Dictionary<int, YorinMissionData> YorinMissionDatas
    {
        get
        {
            if (yorinMissionDatas == null)
            {
                yorinMissionDatas = new Dictionary<int, YorinMissionData>();

                for (int i = 0; i < yorinMission.dataArray.Length; i++)
                {
                    yorinMissionDatas.Add(yorinMission.dataArray[i].Id, yorinMission.dataArray[i]);
                }
            }

            return yorinMissionDatas;
        }
    }

    [SerializeField] private EventMission eventMission;

    public EventMission EventMission => eventMission;

    private Dictionary<int, EventMissionData> eventMissionDatas;

    public Dictionary<int, EventMissionData> EventMissionDatas
    {
        get
        {
            if (eventMissionDatas == null)
            {
                eventMissionDatas = new Dictionary<int, EventMissionData>();

                for (int i = 0; i < eventMission.dataArray.Length; i++)
                {
                    eventMissionDatas.Add(eventMission.dataArray[i].Id, eventMission.dataArray[i]);
                }
            }

            return eventMissionDatas;
        }
    }
    
    [SerializeField] private MonthMission monthMission;

    public MonthMission MonthMission => monthMission;
    private Dictionary<int, MonthMissionData> monthMissionDatas;

    public Dictionary<int, MonthMissionData> MonthMissionDatas
    {
        get
        {
            if (monthMissionDatas == null)
            {
                monthMissionDatas = new Dictionary<int, MonthMissionData>();

                for (int i = 0; i < monthMission.dataArray.Length; i++)
                {
                    monthMissionDatas.Add(monthMission.dataArray[i].Id, monthMission.dataArray[i]);
                }
            }

            return monthMissionDatas;
        }
    }
    [SerializeField] private MonthMission2 monthMission2;
    public MonthMission2 MonthMission2 => monthMission2;
    private Dictionary<int, MonthMission2Data> monthMission2Datas;

    public Dictionary<int, MonthMission2Data> MonthMission2Datas
    {
        get
        {
            if (monthMission2Datas == null)
            {
                monthMission2Datas = new Dictionary<int, MonthMission2Data>();

                for (int i = 0; i < monthMission2.dataArray.Length; i++)
                {
                    monthMission2Datas.Add(monthMission2.dataArray[i].Id, monthMission2.dataArray[i]);
                }
            }

            return monthMission2Datas;
        }
    }
    [SerializeField] private GuideMission guideMission;

    public GuideMission GuideMission => guideMission;

    private Dictionary<int, GuideMissionData> guideMissionDatas;

    public Dictionary<int, GuideMissionData> GuideMissionDatas
    {
        get
        {
            if (guideMissionDatas == null)
            {
                guideMissionDatas = new Dictionary<int, GuideMissionData>();

                for (int i = 0; i < eventMission.dataArray.Length; i++)
                {
                    guideMissionDatas.Add(eventMission.dataArray[i].Id, guideMission.dataArray[i]);
                }
            }

            return guideMissionDatas;
        }
    }
    //


    [SerializeField] private StatusSetting statusTable;
    public StatusSetting StatusTable => statusTable;

    private Dictionary<string, StatusSettingData> statusDatas;

    public Dictionary<string, StatusSettingData> StatusDatas
    {
        get
        {
            if (statusDatas == null)
            {
                statusDatas = new Dictionary<string, StatusSettingData>();

                for (int i = 0; i < statusTable.dataArray.Length; i++)
                {
                    statusDatas.Add(statusTable.dataArray[i].Key, statusTable.dataArray[i]);
                }
            }

            return statusDatas;
        }
    }

    [SerializeField] private WeaponEffect weaponEffectTable;

    private Dictionary<int, WeaponEffectData> weaponEffectDatas;

    public Dictionary<int, WeaponEffectData> WeaponEffectDatas
    {
        get
        {
            if (weaponEffectDatas == null)
            {
                weaponEffectDatas = new Dictionary<int, WeaponEffectData>();

                for (int i = 0; i < weaponEffectTable.dataArray.Length; i++)
                {
                    weaponEffectDatas.Add(weaponEffectTable.dataArray[i].Id, weaponEffectTable.dataArray[i]);
                }
            }

            return weaponEffectDatas;
        }
    }

    [SerializeField] private MagicBook magicBookTable;
    public MagicBook MagicBookTable => magicBookTable;
    private Dictionary<int, MagicBookData> magicBoocDatas;

    public Dictionary<int, MagicBookData> MagicBoocDatas
    {
        get
        {
            if (magicBoocDatas == null)
            {
                magicBoocDatas = new Dictionary<int, MagicBookData>();

                for (int i = 0; i < magicBookTable.dataArray.Length; i++)
                {
                    magicBoocDatas.Add(magicBookTable.dataArray[i].Id, magicBookTable.dataArray[i]);
                }
            }

            return magicBoocDatas;
        }
    }

    public MagicBookData GetMagicBookDataByStringId(string id)
    {
        var e = MagicBoocDatas.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Current.Value.Stringid == id) return e.Current.Value;
        }

        return null;
    }

    [SerializeField] private PetTable petTable;
    public PetTable PetTable => petTable;

    private Dictionary<int, PetTableData> petDatas;

    public Dictionary<int, PetTableData> PetDatas
    {
        get
        {
            if (petDatas == null)
            {
                petDatas = new Dictionary<int, PetTableData>();
                for (int i = 0; i < petTable.dataArray.Length; i++)
                {
                    petDatas.Add(petTable.dataArray[i].Id, petTable.dataArray[i]);
                }
            }

            return petDatas;
        }
    }

    [SerializeField] private LevelPass levelPass;

    public LevelPass LevelPass => levelPass;

    [SerializeField] private Costume costume;

    public Costume Costume => costume;

    private Dictionary<int, CostumeData> costumeData;

    public Dictionary<int, CostumeData> CostumeData
    {
        get
        {
            if (costumeData == null)
            {
                costumeData = new Dictionary<int, CostumeData>();
                for (int i = 0; i < costume.dataArray.Length; i++)
                {
                    costumeData.Add(costume.dataArray[i].Id, costume.dataArray[i]);
                }
            }

            return costumeData;
        }
    }


    [SerializeField] private CostumeAbility costumeAbility;

    public CostumeAbility CostumeAbility => costumeAbility;

    private Dictionary<int, CostumeAbilityData> costumeAbilityData;

    public Dictionary<int, CostumeAbilityData> CostumeAbilityData
    {
        get
        {
            if (costumeAbilityData == null)
            {
                costumeAbilityData = new Dictionary<int, CostumeAbilityData>();

                int costumeMaxGrade = 0;

                for (int i = 0; i < costumeAbility.dataArray.Length; i++)
                {
                    if (costumeMaxGrade < costumeAbility.dataArray[i].Grade)
                    {
                        costumeMaxGrade = costumeAbility.dataArray[i].Grade;
                    }

                    costumeAbilityData.Add(costumeAbility.dataArray[i].Id, costumeAbility.dataArray[i]);
                }

                GameBalance.costumeMaxGrade = costumeMaxGrade;
            }

            return costumeAbilityData;
        }
    }

    private List<ObscuredFloat> costumeProbs;
    private List<ObscuredInt> costumeList = new List<ObscuredInt>();

    public List<ObscuredInt> GetRandomCostumeOptions(int amount)
    {
        if (costumeProbs == null)
        {
            costumeProbs = new List<ObscuredFloat>();

            for (int i = 0; i < costumeAbility.dataArray.Length; i++)
            {
                costumeProbs.Add(costumeAbility.dataArray[i].Prob);
            }
        }

        costumeList.Clear();

        for (int i = 0; i < amount; i++)
        {
            int randomIdx = Utils.GetRandomIdx(costumeProbs.Select(e => (float)e).ToList());
            //0번꺼가 -1라서 얘는 제외
            costumeList.Add(randomIdx - 1);
        }

        return costumeList;
    }

    [SerializeField] private BossTable bossTable;
    public BossTable BossTable => bossTable;


    private Dictionary<int, BossTableData> bossTableData;

    public Dictionary<int, BossTableData> BossTableData
    {
        get
        {
            if (bossTableData == null)
            {
                bossTableData = new Dictionary<int, BossTableData>();
                for (int i = 0; i < bossTable.dataArray.Length; i++)
                {
                    bossTableData.Add(bossTable.dataArray[i].Id, bossTable.dataArray[i]);
                }
            }

            return bossTableData;
        }
    }

    [SerializeField] private TowerTable towerTable;
    public TowerTable TowerTable => towerTable;

    private Dictionary<int, TowerTableData> towerTableData;

    public Dictionary<int, TowerTableData> TowerTableData
    {
        get
        {
            if (towerTableData == null)
            {
                towerTableData = new Dictionary<int, TowerTableData>();

                for (int i = 0; i < towerTable.dataArray.Length; i++)
                {
                    towerTableData.Add(towerTable.dataArray[i].Id, towerTable.dataArray[i]);
                }
            }

            return towerTableData;
        }
    }
    //

    [SerializeField] private TowerTable2 towerTable2;
    public TowerTable2 TowerTable2 => towerTable2;

    private Dictionary<int, TowerTable2Data> towerTableData2;

    public Dictionary<int, TowerTable2Data> TowerTableData2
    {
        get
        {
            if (towerTableData2 == null)
            {
                towerTableData2 = new Dictionary<int, TowerTable2Data>();

                for (int i = 0; i < towerTable2.dataArray.Length; i++)
                {
                    towerTableData2.Add(towerTable2.dataArray[i].Id, towerTable2.dataArray[i]);
                }
            }

            return towerTableData2;
        }
    }
    //


    [SerializeField] private DailyPass dailyPass;
    public DailyPass DailyPass => dailyPass;

    private Dictionary<int, DailyPassData> dailyPassData;

    public Dictionary<int, DailyPassData> DailyPassData
    {
        get
        {
            if (dailyPassData == null)
            {
                dailyPassData = new Dictionary<int, DailyPassData>();

                for (int i = 0; i < dailyPass.dataArray.Length; i++)
                {
                    dailyPassData.Add(dailyPass.dataArray[i].Id, dailyPass.dataArray[i]);
                }
            }

            return dailyPassData;
        }
    }

    [SerializeField] private BonusRoulette bonusRoulette;
    public BonusRoulette BonusRoulette => bonusRoulette;

    [SerializeField] private OldDokebiBonusRoulette oldDokebiBonusRoulette;
    public OldDokebiBonusRoulette OldDokebiBonusRoulette => oldDokebiBonusRoulette;

    [SerializeField] private InAppPurchase inAppPurchase;
    public InAppPurchase InAppPurchase => inAppPurchase;

    private Dictionary<string, InAppPurchaseData> inAppPurchaseData;

    public Dictionary<string, InAppPurchaseData> InAppPurchaseData
    {
        get
        {
            if (inAppPurchaseData == null)
            {
                inAppPurchaseData = new Dictionary<string, InAppPurchaseData>();

                var datas = inAppPurchase.dataArray;

                for (int i = 0; i < datas.Length; i++)
                {
                    inAppPurchaseData.Add(datas[i].Productid, datas[i]);
                }
            }

            return inAppPurchaseData;
        }
    }

    [SerializeField] private LoadingTip loadingTip;
    public LoadingTip LoadingTip => loadingTip;

    [SerializeField] private BadWord badWord;
    public BadWord BadWord => badWord;

    [SerializeField] private AttendanceReward attendanceReward;
    public AttendanceReward AttendanceReward => attendanceReward;

    [SerializeField] private WingTable wingTable;

    public WingTable WingTable => wingTable;

    public int WingMaxValue => wingTable.dataArray[wingTable.dataArray.Length - 1].Requirejump;

    [SerializeField] private BuffTable buffTable;

    public BuffTable BuffTable => buffTable;

    [SerializeField] private PassiveSkill passiveSkill;

    public PassiveSkill PassiveSkill => passiveSkill;

    [SerializeField] private PassiveSkill2 passiveSkill2;

    public PassiveSkill2 PassiveSkill2 => passiveSkill2;

    [SerializeField] private MarbleTable marbleTable;

    public MarbleTable MarbleTable => marbleTable;

    [SerializeField] private Dokebi dokebiTable;

    public Dokebi DokebiTable => dokebiTable;

    [SerializeField] private DokebiRewardTable dokebiRewardTable;
    public DokebiRewardTable DokebiRewardTable => dokebiRewardTable;

    [SerializeField] private TwelveBossTable twelveBossTable;

    public TwelveBossTable TwelveBossTable => twelveBossTable;

    [SerializeField] private TitleTable titleTable;

    public TitleTable TitleTable => titleTable;

    private Dictionary<int, List<TitleTableData>> titleAbils;

    public Dictionary<int, List<TitleTableData>> TitleAbils
    {
        get
        {
            if (titleAbils == null)
            {
                titleAbils = new Dictionary<int, List<TitleTableData>>();

                for (int i = 0; i < titleTable.dataArray.Length; i++)
                {
                    if (titleAbils.ContainsKey(titleTable.dataArray[i].Abiltype1) == false)
                    {
                        titleAbils.Add(titleTable.dataArray[i].Abiltype1, new List<TitleTableData>());
                    }

                    titleAbils[titleTable.dataArray[i].Abiltype1].Add(titleTable.dataArray[i]);
                }
            }

            return titleAbils;
        }
    }

    [SerializeField] private YomulAbil yomulAbilTable;
    public YomulAbil YomulAbilTable => yomulAbilTable;

    [SerializeField] private ChuseokEvent chuseokEventTable;

    public ChuseokEvent ChuseokEventTable => chuseokEventTable;

    [SerializeField] private YoguiSogulTable yoguisogulTable;

    public YoguiSogulTable YoguisogulTable => yoguisogulTable;

    [SerializeField] private OldDokebi2Table oldDokebiTable;

    public OldDokebi2Table OldDokebiTable => oldDokebiTable;

    [SerializeField] private PetEquipment petEquipment;
    public PetEquipment PetEquipment => petEquipment;


    [SerializeField] private MonthlyPass monthlyPass;
    public MonthlyPass MonthlyPass => monthlyPass;

    [SerializeField] private RelicTable rericTable;

    public RelicTable RelicTable => rericTable;


    [SerializeField] private StageRelic stageRelic;

    public StageRelic StageRelic => stageRelic;

    [SerializeField] private monthpass2 monthlyPass2;
    public monthpass2 MonthlyPass2 => monthlyPass2;

    [SerializeField] private MonthlyPassAttend monthlyPassAttend;
    public MonthlyPassAttend MonthlyPassAttend => monthlyPassAttend;

    [SerializeField] private MonthlyPassAttend2 monthlyPassAttend2;
    public MonthlyPassAttend2 MonthlyPassAttend2 => monthlyPassAttend2;
    
    [SerializeField] private YorinAttend yorinAttend;
    public YorinAttend YorinAttend => yorinAttend;
    
    [SerializeField] private SecondAttend secondAttend;
    public SecondAttend SecondAttend => secondAttend;

    [SerializeField] private SonReward sonReward;

    public SonReward SonReward => sonReward;

    [SerializeField] private SonAbil sonAbil;

    public SonAbil SonAbil => sonAbil;

    [SerializeField] private Attendance100 attendanceReward_100;
    public Attendance100 AttendanceReward_100 => attendanceReward_100;

    [SerializeField] private IosCoupon iosCoupon;

    public IosCoupon IosCoupon => iosCoupon;

    [SerializeField] private GuildRewardTable guildRewardTable;

    public GuildRewardTable GuildRewardTable => guildRewardTable;

    [SerializeField] private GuildLevel guildLevel;

    public GuildLevel GuildLevel => guildLevel;

    [SerializeField] private SeolPass seolPass;
    public SeolPass SeolPass => seolPass;

    [SerializeField] private SulPass sulPass;
    public SulPass SulPass => sulPass;

    public SmithTable smithTable;

    public SmithEnemy smithEnemy;

    public GumGiTable gumGiTable;

    public WinterPass winterPass;
    public ChildPass childPass;
    public ColdSeasonPass coldSeasonPass;

    public FoxMask FoxMask;
    public DokebiHorn DokebiHorn;
    public CaveBelt CaveBelt;

    [SerializeField] private SummerCollection summerCollection;

    public SummerCollection SummerCollection => summerCollection;

    public SusanoTable susanoTable;
    public OkTable okTable;
    public YumTable yumTable;
    public DoTable doTable;

    public SumiTable sumiTable;
    public ThiefTable ThiefTable;
    public DarkTable DarkTable;
    public SinsunTable SinsunTable;

    //사신수
    public SasinsuTable sasinsuTable;


    public BokPass bokPass;
    public GuildBook guildBook;
    public OneYearAtten oneYearAtten;
    public HellAbil hellAbil;
    public DarkAbil DarkAbil;
    public HellAbilBase hellAbilBase;
    public HellReward hellReward;
    public DragonBall dragonBall;
    public ChuSeokAtten chuSeokAtten;
    public ColdSeasonAtten coldSeasonAtten;

    public ChunAbilBase chunAbilBase;
    public DokebiAbilBase dokebiAbilBase;
    public ChunMarkAbil chunMarkAbil;
    public FallCollection fallCollection;
    public XMasCollection xMasCollection;
    public MileageReward mileageReward;
    public ClearTicket ClearTicket;
    public PetHome petHome;
    public TowerTable3 towerTable3;
    public TowerTable4 towerTableMulti;
    public TowerTable5 sumisanTowerTable;
    public GradeTestTable gradeTestTable;
    public HyunSangTowerTable HyunSangTowerTable;
    public RelicTest RelicTest;
    public CommonCollectionEvent commoncollectionEvent;
    public CommonEventAttend CommonEventAttend;
    public CommonEventAttend2 CommonEventAttend2;
    public SnowManAtten snowManAtten;
    public SumiAbilBase sumiAbilBase;
    public FoxCup foxCup;
    public DayOfWeekDungeon dayOfWeekDungeon;
    public CostumeCollection costumeCollection;
    public ChoboTable choboTable;
    public TwoCave twoCave;
    public SpringAtten springAtten;
    public TowerTable6 gyungRockTowerTable;
    public DolPass dolPass;
    public NorigaeJewel norigaeJewel;
    public TresureAbilBase tresureAbilBase;
    public RoyalTombTowerTable royalTombTowerTable;
    public SuhopetTable suhoPetTable;
    public AdRewardTable adRewardTable;
    public NewSemesterPass newSemesterPass;
    public TowerTable7 sinsuTower;
    public Title_Stage titleStage;
    public Title_Level titleLevel;
    public Title_Weapon titleWeapon;
    public Title_MagicBook titleMagicBook;
    public GuildTowerTable guildTowerTable;
    public SuhoPass suhoPass;
    public DarkTreasureAbilBase DarkTreasureAbilBase;
    public SinsunTreasureAbilBase SinsunTreasureAbilBase;
    public GwisalTreasureAbilBase GwisalTreasureAbilBase;
    public BlackWolfRing BlackWolfRing;
    public DragonBracelet DragonBracelet;
    public TowerTable8 gyungRockTowerTable2;
    public FoxTower FoxTowerTable;
    public FoxFire FoxFire;
    public DarkTowerTable DarkTowerTable;
    public TestChun TestChun;
    public TestHell TestHell;
    public TestMonkey TestMonkey;
    public TestSword TestSword;
    public TestDo TestDo;
    public TestSumi TestSumi;
    public TestThief TestThief;
    public TestSin TestSin;
    public SealSword sealSwordTable;
    public SealSwordPass SealSwordPass;
    public FoxFirePass FoxFirePass;
    public DosulPass DosulPass;
    public SinsunTowerTable sinsunTowerTable;
    public SoulForestPass SoulForestPass;
    public SwordPass SwordPass;
    public GuimoonTable GuimoonTable;
    public GuimoonPass GuimoonPass;
    public TransTowerTable TransTowerTable;
    public SecondMiniGame SecondMiniGame;
    public RelicUpgrade RelicUpgrade;
    public SuhoUpgrade SuhoUpgrade;
    public Meditation Meditation;
    public MeditationTower MeditationTower;
    public TestDark TestDark;
    public SoulPass SoulPass;
    public PeachPass PeachPass;
    public DanjeonTable DanjeonTable;
    public MeditationPass MeditationPass;
    public RelicSpecial RelicSpecial;
    public RelicEnhance RelicEnhance;
    public PetPass PetPass;
    public GuildExchange GuildExchange;
    public PetDispatch PetDispatch;
    public MarbleEvent MarbleEvent;
    public AbilAwakeTable AbilAwakeTable;
    public ClosedTrainingTable ClosedTrainingTable;
    public DragonTowerTable DragonTowerTable;
    private Dictionary<int, SealSwordData> sealSwordData = null;

    public Dictionary<int, SealSwordData> SealSwordData
    {
        get
        {
            LoadSealSwordData();
            return sealSwordData;
        }
    }
    
    private void LoadSealSwordData()
    {
        if (sealSwordData != null) return;

        sealSwordData = new Dictionary<int, SealSwordData>();

        for (int i = 0; i < sealSwordTable.dataArray.Length; i++)
        {
            sealSwordData.Add(sealSwordTable.dataArray[i].Id, sealSwordTable.dataArray[i]);
        }
    }
    [SerializeField] private YorinSpecialMission yorinSpecialMission;

    public YorinSpecialMission YorinSpecialMission => yorinSpecialMission;
    private Dictionary<int, YorinSpecialMissionData> yorinSpecialMissionDatas;
    
    public Dictionary<int, YorinSpecialMissionData> YorinSpecialMissionDatas
    {
        get
        {
            if (yorinSpecialMissionDatas == null)
            {
                yorinSpecialMissionDatas = new Dictionary<int, YorinSpecialMissionData>();

                for (int i = 0; i < YorinSpecialMission.dataArray.Length; i++)
                {
                    yorinSpecialMissionDatas.Add(yorinSpecialMission.dataArray[i].Id, yorinSpecialMission.dataArray[i]);
                }
            }

            return yorinSpecialMissionDatas;
        }
    }
    public TowerTable9 SealTowerTable;
    public SealSwordAwakeTable SealSwordAwakeTable;
    public DosulAwakeTable DosulAwakeTable;
    public TaegeukTitle taegeukTitle;
    public TowerTable10 gyungRockTowerTable3;
    public TowerTable11 gyungRockTowerTable4;
    public GCPass GCPass;

    public DosulTower dosulTowerTable;
    public Dosul dosulTable;
    public VisionTowerTable visionTowerTable;
    public DaesanExchange DaesanExchange;
    public YokaiPowerOpen YokaiPowerOpen;
    public TaegeukSimbeop TaegeukSimbeop;
    public TowerTable12 gyungRockTowerTable5;
    public MonthBuff MonthBuff;
    public MonthReward MonthReward;
    public SealSwordEvolutionPass SealSwordEvolutionPass;
    public SealSwordEvolution SealSwordEvolution;
    public BlackFoxAbil BlackFoxAbil;
    public BlackFoxTable BlackFoxTable;

    public Byeolho Byeolho;
    public ByeolhoTower ByeolhoTower;

    public BlackFoxPass BlackFoxPass;
    public DosulLevelPass DosulLevelPass;
    
    public BattleContestTable BattleContestTable;

    public BattleContestSpecialExchange BattleContestSpecialExchange;
    public TowerTable13 gyungRockTowerTable6;
    public BattleContestGrade BattleContestGrade;
    public TransUpgrade TransUpgrade;
    public GachaEvent GachaEvent;
    public BlueGangCheol BlueGangCheol;
    public DragonPlaceTowerTable DragonPlaceTowerTable;
    public WeeklyBoss WeeklyBoss;
    public PetAwakeLevel PetAwakeLevel;
    public PetAwakeTable PetAwakeTable;
    public BimuPass BimuPass;
    public PetAwakePass PetAwakePass;
    public StudentTable StudentTable;
    public StudentDispatch StudentDispatch;
    public CostumeSpecialAbility CostumeSpecialAbility;
    public StudentAwakeTable StudentAwakeTable;
    public Title_Special Title_Special;
    public StudentTower StudentTower;
    public EventPackageBonus EventPackageBonus;
    public MurimTowerTable MurimTowerTable;
    public HonorReward HonorReward;
    public BingoEvent BingoEvent;
    public TowerTable14 gyungRockTowerTable7;
    public StudentPass StudentPass;
    public Title_Samcheon Title_Samcheon;
    public StudentSpotTower StudentSpotTower;
    public StudentSpot StudentSpot;
    public StudentSpotPass StudentSpotPass;
    
    public SpecialRequestStarRewardTable SpecialRequestStarRewardTable;
    public SpecialRequestExchangeTable SpecialRequestExchangeTable;
    public SpecialRequestBossTable SpecialRequestBossTable;
    public SpecialRequestTable SpecialRequestTable;
    public SpecialRequestRewardTable SpecialRequestRewardTable;
    public GuildTowerTable2 GuildTowerTable2;
    public GuildPet GuildPet;
    public TowerTable15 gyungRockTowerTable8;
    public TowerTable16 TowerTable16;
    public YeonokTowerTable YeonokTowerTable;
    public HaetalTable HaetalTable;
    public HaetalPass HaetalPass;
    
}


public enum CommonTableEventType
{
    SnowMan,
    Namul,
    Flower,
    DdukGuk,
    HotTime,
    Normal,
    SecondAnniversary,
    FullMoon,
    BattleContest,
}

public enum BingoEventRewardType
{
    Normal,
    HorizontalBingo,
    VerticalBingo,
    DiagonalBingo,
    Finish
}