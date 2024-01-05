using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = System.Diagnostics.Debug;

public class UiContentsPopup2 : MonoBehaviour
{
    private enum ContentsBoard
    {
        UiRelicBoard,
        YoguisogulBoard,
        GumiHoBoard,
        SonBoard,
        SusanoBoard,
        SinBoard,
        GangchulBoard,
        FoxMaskBoard,
        SuhosinBoard,
        HellBoard,
        ChunBoard,
        YumAndOkBoard,
        GradeTestBoard,
        DokebiBoard,
        SasinsuBoard,
        SumisanBoard,
        SahyungsuBoard,
        UiThiefBoard,
        SmithBoard,
        SuhoAnimal,
        SinsuTower,
        VisionBoard,
        DarkBoard,
        FoxTowerBoard,
        FoxBossBoard,
        SinsunBoard,
        GodTrialBoard,
        TaegeukBoard,
        SangunBoard,
        HyunSangBoard,
        ChunGuBoard,
        VisionTowerBoard,
        SinSkillBoard,
        TransBoard,
        NewBossBoard,
        DanjeonBoard,
        ClosedTrainingBoard,
        DragonBoard,
        SuhoBossBoard,
        BlackFoxBoard,
        DragonPalaceBoard,
    }
    //한계돌파
    private enum GrowthContentsDoor
    {
        Bandit0,
        Bandit1,
        NormalTower,
        HardTower,
        SinsuTower,
        Smith,
        Dokebi,
        Relic,
        Taegeuk,
        YoguiSogul,
        Son,
        Susano,
        FoxMask,
        KingTest,
        GradeTest,
        Vision,
        SuhoPet,
        Fox,
        GodTest,
        TransBoard,
        DanjeonBoard,
        ClosedTrainingBoard,
        BlackFoxBoard,
        
        
    }
    //보스도전
    private enum BossChallengeDoor
    {
        Cat,
        Twelve,
        PetTest,
        NineTales,
        Monster,
        SuhoGod,
        Sasinsu,
        Sahyung,
        Saryong,
        FoxStory,
        Sangun,
        Chungu,
        NewBoss,
        SuhoBoss,
        
    }
    //삼천세계 
    private enum AdventureDoor
    {
        Hell,
        Chun,
        DokebiWorld,
        Sumi,
        Thief,
        Dark,
        Sinsun,
        Dragon,
        DragonPalace,

    }
    
    [SerializeField]
    private List<GameObject> lastBoards;
    [SerializeField]
    private List<GameObject> newGrowthDoors;
    [SerializeField]
    private List<GameObject> oldGrowthDoors;
    [SerializeField]
    private List<GameObject> newBossDoors;
    [SerializeField]
    private List<GameObject> oldBossDoors;
    [SerializeField]
    private List<GameObject> newAdventureDoors;
    [SerializeField]
    private List<GameObject> oldAdventureDoors;
    [SerializeField]
    private List <UiContentsEnterButton> uiContentsEnterButtons;

    private void LoadLastContentPopup()
    {
        
        GameManager.ContentsType type = GameManager.Instance.lastContentsType2;
        int id = GameManager.Instance.bossId;
        switch (type)
        {
            case GameManager.ContentsType.InfiniteTower:
                UiContentsEnterPopup.Instance.Initialize(type,0);
                break;
            case GameManager.ContentsType.InfiniteTower2:
                UiContentsEnterPopup.Instance.Initialize(type,0);
                break;
                
            case  GameManager.ContentsType.RelicDungeon:
            case  GameManager.ContentsType.RelicTest:
                lastBoards[(int)ContentsBoard.UiRelicBoard].SetActive(true);
                break;
            case GameManager.ContentsType.TwelveDungeon:
                switch (id)
                {
                    //십이지
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        uiContentsEnterButtons[0].OnClickButton();
                        break;
                    //신수스킬
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                        lastBoards[(int)ContentsBoard.SinSkillBoard].SetActive(true);
                        break;
                    //신요괴
                    case 13:
                    case 14:
                    case 19:
                    case 21:
                    case 24:
                    case 26:
                    case 28:
                        lastBoards[(int)ContentsBoard.SinBoard].SetActive(true);
                        break;
                    //강철이
                    case 20:
                        return;
                        lastBoards[(int)ContentsBoard.GangchulBoard].SetActive(true);
                        break;
                    //수호신
                    case 22:
                    case 23:
                    case 25:
                    case 27:
                    case 29:
                    case 39:
                        lastBoards[(int)ContentsBoard.SuhosinBoard].SetActive(true);
                        break;
                    //구미호
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                        lastBoards[(int)ContentsBoard.GumiHoBoard].SetActive(true);
                        break;
                    //지옥요괴전
                    case 40:
                    case 41:
                    case 42:
                    case 43:
                    case 44:
                    case 45:
                    case 46:
                    case 47:
                    case 48:
                    case 49:
                    //지옥보스전
                    case 52:
                    case 54:
                    case 56:
                        lastBoards[(int)ContentsBoard.HellBoard].SetActive(true);
                        break;
                    //여래
                    case 50:
                        lastBoards[(int)ContentsBoard.SonBoard].SetActive(true);
                        break;
                    //칠선녀
                    case 58:
                    case 59:
                    case 60:
                    case 61:
                    case 62:
                    case 63:
                    case 64:
                    //천계보물창고
                    case 66:
                    case 67:
                    case 70:
                    case 71:
                    //서재
                    case 72:
                        lastBoards[(int)ContentsBoard.ChunBoard].SetActive(true);
                        break;
                    //귀신나무
                    case 69:
                        lastBoards[(int)ContentsBoard.FoxMaskBoard].SetActive(true);
                        break;
                    //악의씨앗
                    case 84:
                        lastBoards[(int)ContentsBoard.SusanoBoard].SetActive(true);
                        break;
                    //도깨비 보스
                    case 75:
                    case 76:
                    case 77:
                    case 78:
                    case 79:
                    case 80:
                    case 81:
                    //지키미
                    case 82:
                    //보물도깨비
                    case 83:
                    //도깨비 우두머리
                    case 85:
                    case 86:
                        lastBoards[(int)ContentsBoard.DokebiBoard].SetActive(true);
                    break;
                    //수미산
                    case 87:
                    case 88:
                    case 89:
                    case 90:
                        //수미산지키미
                    case 92:
                    //우두머리
                    case 93:
                    case 94:
                    case 95:
                        //수미숲지키미
                    case 96:
                        lastBoards[(int)ContentsBoard.SumisanBoard].SetActive(true);
                    break;
                    //사신수-황룡
                    case 97:
                        lastBoards[(int)ContentsBoard.SasinsuBoard].SetActive(true);
                    break;
                    //사흉수
                    case 98:
                    case 99:
                    case 100:
                    case 101:
                    case 132:
                        lastBoards[(int)ContentsBoard.SahyungsuBoard].SetActive(true);
                    break;
                    //도둑들
                    case 102:
                    case 103:
                    case 104:
                    case 105:
                    case 106:
                    case 107:
                    case 108:
                    case 109:
                    case 110:
                    case 111:
                    case 112:
                    case 118:
                        lastBoards[(int)ContentsBoard.UiThiefBoard].SetActive(true);
                        break;
                    case 113:
                    case 114:
                    case 115:
                    case 116:
                        lastBoards[(int)ContentsBoard.VisionBoard].SetActive(true);
                        break;
                    case 119:
                    case 120:
                    case 121:
                    case 122:
                    case 123:
                    case 124:
                    case 125:
                    case 126:
                    case 127:
                    case 128:
                    case 129:
                    case 130:
                    case 131:
                    case 133:
                    case 134:
                    case 135:
                    case 150://심연늪
                        lastBoards[(int)ContentsBoard.DarkBoard].SetActive(true);
                        break;
                    case 136:
                    case 137:
                    case 138:
                    case 139:
                        lastBoards[(int)ContentsBoard.FoxBossBoard].SetActive(true);
                        break;
                    case 140:
                    case 141:
                    case 142:
                    case 143:
                    case 144:
                    case 145:
                    case 151:
                    case 152:
                    case 153:
                    case 187:
                        lastBoards[(int)ContentsBoard.SinsunBoard].SetActive(true);
                        break;
                    case 146:
                    case 147:
                    case 148:
                    case 149:
                        lastBoards[(int)ContentsBoard.SangunBoard].SetActive(true);
                        break;
                    case 154:
                    case 155:
                    case 156:
                    case 157:
                    case 158:
                    case 159:
                    case 160:
                    case 161:
                    case 167:
                    case 168:
                    case 169:
                    case 170:
                    case 171:
                    case 172:
                    case 173:
                    case 177:
                    case 180:
                    case 182:
                    case 184:
                        lastBoards[(int)ContentsBoard.HyunSangBoard].SetActive(true);
                        break;
                    
                    case 162:
                    case 163:
                    case 164:
                    case 165:
                    case 166:
                        lastBoards[(int)ContentsBoard.ChunGuBoard].SetActive(true);
                        break;
                    case 174:
                    case 175:
                    case 178:
                    case 179:
                    case 181:
                    case 183:
                        lastBoards[(int)ContentsBoard.NewBossBoard].SetActive(true);
                        break;
                    case 185:
                    case 186:
                    case 188:
                    case 189:
                    case 200:
                    case 201:
                        lastBoards[(int)ContentsBoard.DragonBoard].SetActive(true);
                        break;
                    case 190:
                    case 191:
                    case 192:
                    case 193:
                    case 194:
                    case 195:
                    case 196:
                    case 197:
                    case 198:
                    case 199:
                    case 207:
                    case 208:
                    case 209:
                    case 210:
                    case 211:
                        lastBoards[(int)ContentsBoard.SuhoBossBoard].SetActive(true);
                        break;
                    case 202:
                    case 203:
                    case 205:
                    case 206:
                    case 212:
                    case 213:
                        lastBoards[(int)ContentsBoard.DragonPalaceBoard].SetActive(true);
                        break;
                        
                }
                break;
            case GameManager.ContentsType.YoguiSoGul:
                lastBoards[(int)ContentsBoard.YoguisogulBoard].SetActive(true);
                break;
            case GameManager.ContentsType.Son:
                lastBoards[(int)ContentsBoard.SonBoard].SetActive(true);
                break;
            case GameManager.ContentsType.SonClone:
                lastBoards[(int)ContentsBoard.SonBoard].SetActive(true);
                break;
            case GameManager.ContentsType.Susano:
                lastBoards[(int)ContentsBoard.SusanoBoard].SetActive(true);
                break;
            case GameManager.ContentsType.FoxMask:
                lastBoards[(int)ContentsBoard.FoxMaskBoard].SetActive(true);
                break;
            case GameManager.ContentsType.Hell:
            case GameManager.ContentsType.HellRelic:
            case GameManager.ContentsType.HellWarMode:
            case GameManager.ContentsType.DokebiTower:
                lastBoards[(int)ContentsBoard.HellBoard].SetActive(true);
                break;
            case GameManager.ContentsType.ChunFlower:
                lastBoards[(int)ContentsBoard.ChunBoard].SetActive(true);
                break;
            case GameManager.ContentsType.Yum:
            case GameManager.ContentsType.Ok:
            case GameManager.ContentsType.Do:
            case GameManager.ContentsType.Sumi:
            case GameManager.ContentsType.Thief:
            case GameManager.ContentsType.Dark:
            case GameManager.ContentsType.Sinsun:
                lastBoards[(int)ContentsBoard.YumAndOkBoard].SetActive(true);
                break;
            case GameManager.ContentsType.TestMonkey:
            case GameManager.ContentsType.TestSword:
            case GameManager.ContentsType.TestHell:
            case GameManager.ContentsType.TestChun:
            case GameManager.ContentsType.TestDo:
            case GameManager.ContentsType.TestSumi:
            case GameManager.ContentsType.TestThief:
            case GameManager.ContentsType.TestDark:
            case GameManager.ContentsType.TestSin:
                lastBoards[(int)ContentsBoard.GodTrialBoard].SetActive(true);
                break;
            case GameManager.ContentsType.GradeTest:
                lastBoards[(int)ContentsBoard.GradeTestBoard].SetActive(true);
                break;
            case GameManager.ContentsType.DokebiFire:
                lastBoards[(int)ContentsBoard.DokebiBoard].SetActive(true);
                break;
            case GameManager.ContentsType.Sasinsu:
                lastBoards[(int)ContentsBoard.SasinsuBoard].SetActive(true);
                break;
            case GameManager.ContentsType.SumiFire:
            case GameManager.ContentsType.SumisanTower:
                lastBoards[(int)ContentsBoard.SumisanBoard].SetActive(true);
                break;
            case GameManager.ContentsType.RoyalTombTower :
                lastBoards[(int)ContentsBoard.UiThiefBoard].SetActive(true);
                break;    
            case GameManager.ContentsType.Smith :
            case GameManager.ContentsType.SmithTree :
                lastBoards[(int)ContentsBoard.SmithBoard].SetActive(true);
                break;   
            case GameManager.ContentsType.SuhoAnimal :
                lastBoards[(int)ContentsBoard.SuhoAnimal].SetActive(true);
                break;  
            case GameManager.ContentsType.SinsuTower :
                lastBoards[(int)ContentsBoard.SinsuTower].SetActive(true);
                break;
            case GameManager.ContentsType.DarkTower :
                lastBoards[(int)ContentsBoard.DarkBoard].SetActive(true);
                break;
            case GameManager.ContentsType.FoxTower :
                lastBoards[(int)ContentsBoard.FoxTowerBoard].SetActive(true);
                break;   
            case GameManager.ContentsType.TaeguekTower :
                lastBoards[(int)ContentsBoard.TaegeukBoard].SetActive(true);
                break;
            case GameManager.ContentsType.SinsunTower :
                lastBoards[(int)ContentsBoard.SinsunBoard].SetActive(true);
                break;
            case GameManager.ContentsType.VisionTower :
                lastBoards[(int)ContentsBoard.VisionTowerBoard].SetActive(true);
                break;
            case GameManager.ContentsType.HyunSangTower :
                lastBoards[(int)ContentsBoard.HyunSangBoard].SetActive(true);
                break;
            case GameManager.ContentsType.TransTower :
                lastBoards[(int)ContentsBoard.TransBoard].SetActive(true);
                break;

            case GameManager.ContentsType.Danjeon :
                lastBoards[(int)ContentsBoard.DanjeonBoard].SetActive(true);
                break;

            case GameManager.ContentsType.ClosedTraining :
                lastBoards[(int)ContentsBoard.ClosedTrainingBoard].SetActive(true);
                break;
            case GameManager.ContentsType.DragonTower :
                lastBoards[(int)ContentsBoard.DragonBoard].SetActive(true);
                break;
            case GameManager.ContentsType.BlackFox :
                lastBoards[(int)ContentsBoard.BlackFoxBoard].SetActive(true);
                break;
            case GameManager.ContentsType.DragonPalaceTower :
                lastBoards[(int)ContentsBoard.DragonPalaceBoard].SetActive(true);
                break;

        }
        
        GameManager.Instance.ResetLastContents2();
    }

    private void Subscribe()
    {
        SettingData.showBanditUi.AsObservable().Subscribe(e =>
        {
            if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < GameBalance.banditUpgradeLevel)
            {
                    newGrowthDoors[(int)GrowthContentsDoor.Bandit0].SetActive(e == 1);
                    oldGrowthDoors[(int)GrowthContentsDoor.Bandit0].SetActive(e == 1);
            }
            else
            {
                    newGrowthDoors[(int)GrowthContentsDoor.Bandit1].SetActive(e == 1);
                    oldGrowthDoors[(int)GrowthContentsDoor.Bandit1].SetActive(e == 1);
            }
        }).AddTo(this);
        SettingData.showTowerUi.AsObservable().Subscribe(e =>
        {
            if (ServerData.userInfoTable.IsLastFloor()==false)
            {
                newGrowthDoors[(int)GrowthContentsDoor.NormalTower].SetActive(e == 1);
                oldGrowthDoors[(int)GrowthContentsDoor.NormalTower].SetActive(e == 1);
            }
            else if(ServerData.userInfoTable.IsLastFloor2()==false)
            {
                newGrowthDoors[(int)GrowthContentsDoor.HardTower].SetActive(e == 1);
                oldGrowthDoors[(int)GrowthContentsDoor.HardTower].SetActive(e == 1);
            }
            else if (ServerData.userInfoTable.IsLastFloor3() == false)
            {
                newGrowthDoors[(int)GrowthContentsDoor.SinsuTower].SetActive(e == 1);
                oldGrowthDoors[(int)GrowthContentsDoor.SinsuTower].SetActive(e == 1);
            }
            else
            {
                newGrowthDoors[(int)GrowthContentsDoor.SinsuTower].SetActive(e == 1);
                oldGrowthDoors[(int)GrowthContentsDoor.SinsuTower].SetActive(e == 1);
            }
        }).AddTo(this);
        SettingData.showSmithUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.Smith].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.Smith].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showDokebiUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.Dokebi].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.Dokebi].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showSoulForestUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.Relic].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.Relic].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showTaegeukUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.Taegeuk].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.Taegeuk].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showBackguiUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.YoguiSogul].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.YoguiSogul].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showSonUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.Son].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.Son].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showSusanoUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.Susano].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.Susano].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showFoxmaskUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.FoxMask].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.FoxMask].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showKingTestUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.KingTest].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.KingTest].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showGradeTestUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.GradeTest].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.GradeTest].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showVisionTowerUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.Vision].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.Vision].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showSuhoTowerUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.SuhoPet].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.SuhoPet].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showFoxTowerUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.Fox].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.Fox].SetActive(e == 1);
        }).AddTo(this);       
        SettingData.showGodTrialUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.GodTest].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.GodTest].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showTransUi.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.TransBoard].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.TransBoard].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showDanjeon.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.DanjeonBoard].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.DanjeonBoard].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showClosed.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.ClosedTrainingBoard].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.ClosedTrainingBoard].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showBlackFox.AsObservable().Subscribe(e =>
        {
            newGrowthDoors[(int)GrowthContentsDoor.BlackFoxBoard].SetActive(e == 1);
            oldGrowthDoors[(int)GrowthContentsDoor.BlackFoxBoard].SetActive(e == 1);
        }).AddTo(this);
        
        ////////////보스도전
        
        SettingData.showCatUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.Cat].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.Cat].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showTwelveUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.Twelve].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.Twelve].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showHwansuUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.PetTest].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.PetTest].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showGumihoUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.NineTales].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.NineTales].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showNewYoguiUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.Monster].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.Monster].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showSuhosinUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.SuhoGod].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.SuhoGod].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showSasinsuUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.Sasinsu].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.Sasinsu].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showSahyungsuUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.Sahyung].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.Sahyung].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showVisionBossUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.Saryong].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.Saryong].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showFoxUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.FoxStory].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.FoxStory].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showSangoonUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.Sangun].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.Sangun].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showChunguUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.Chungu].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.Chungu].SetActive(e == 1);
        }).AddTo(this);;
        
        SettingData.showNewBossUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.NewBoss].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.NewBoss].SetActive(e == 1);
        }).AddTo(this);;
        SettingData.showSuhoBossUi.AsObservable().Subscribe(e =>
        {
            newBossDoors[(int)BossChallengeDoor.SuhoBoss].SetActive(e == 1);
            oldBossDoors[(int)BossChallengeDoor.SuhoBoss].SetActive(e == 1);
        }).AddTo(this);;
        
        ///////삼천세계
        SettingData.showHellUi.AsObservable().Subscribe(e =>
        {
            newAdventureDoors[(int)AdventureDoor.Hell].SetActive(e == 1);
            oldAdventureDoors[(int)AdventureDoor.Hell].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showChunUi.AsObservable().Subscribe(e =>
        {
            newAdventureDoors[(int)AdventureDoor.Chun].SetActive(e == 1);
            oldAdventureDoors[(int)AdventureDoor.Chun].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showDoUi.AsObservable().Subscribe(e =>
        {
            newAdventureDoors[(int)AdventureDoor.DokebiWorld].SetActive(e == 1);
            oldAdventureDoors[(int)AdventureDoor.DokebiWorld].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showSumiUi.AsObservable().Subscribe(e =>
        {
            newAdventureDoors[(int)AdventureDoor.Sumi].SetActive(e == 1);
            oldAdventureDoors[(int)AdventureDoor.Sumi].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showThiefUi.AsObservable().Subscribe(e =>
        {
            newAdventureDoors[(int)AdventureDoor.Thief].SetActive(e == 1);
            oldAdventureDoors[(int)AdventureDoor.Thief].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showDarkUi.AsObservable().Subscribe(e =>
        {
            newAdventureDoors[(int)AdventureDoor.Dark].SetActive(e == 1);
            oldAdventureDoors[(int)AdventureDoor.Dark].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showSinsunUi.AsObservable().Subscribe(e =>
        {
            newAdventureDoors[(int)AdventureDoor.Sinsun].SetActive(e == 1);
            oldAdventureDoors[(int)AdventureDoor.Sinsun].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showDragonUi.AsObservable().Subscribe(e =>
        {
            newAdventureDoors[(int)AdventureDoor.Dragon].SetActive(e == 1);
            oldAdventureDoors[(int)AdventureDoor.Dragon].SetActive(e == 1);
        }).AddTo(this);
        SettingData.showDragonPalaceUi.AsObservable().Subscribe(e =>
        {
            newAdventureDoors[(int)AdventureDoor.DragonPalace].SetActive(e == 1);
            oldAdventureDoors[(int)AdventureDoor.DragonPalace].SetActive(e == 1);
        }).AddTo(this);
    }
    void Start()
    {
        LoadLastContentPopup();
        Subscribe();
    }
}
