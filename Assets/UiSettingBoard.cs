using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiSettingBoard : MonoBehaviour
{
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider efxSlider;
    [SerializeField]
    private Slider vierSlider;
    [SerializeField]
    private Slider uiVierSlider;
    [SerializeField]
    private Slider jotStickSlider;

    [SerializeField]
    private List<Toggle> graphicOptionToggle;

    [SerializeField]
    private List<Toggle> frameRateToggle;

    [SerializeField]
    private Toggle showDamageFontToggle;

    [SerializeField]
    private Toggle showEffectToggle;

    //Glow 효과임
    [SerializeField]
    private Toggle shakeCameraToggle;

    [SerializeField]
    private Toggle showSleepPushToggle;
    [SerializeField]
    private Toggle batterySafeModeToggle;

    [SerializeField]
    private Toggle yachaEffectToggle;

    [SerializeField]
    private Toggle hpBarToggle;

    [SerializeField]
    private Toggle enemyViewToggle;

    [SerializeField]
    private TextMeshProUGUI bgmDesc;

    [SerializeField]
    private TextMeshProUGUI sfxDesc;

    [SerializeField]
    private TextMeshProUGUI viewDesc;

    [SerializeField]
    private TextMeshProUGUI uiViewDesc;

    [SerializeField]
    private TextMeshProUGUI uiJoyStickDesc;

    //
    [SerializeField]
    private Toggle sonView;
    [SerializeField]
    private Toggle dogView;
    [SerializeField]
    private Toggle marbleCircleView;
    [SerializeField]
    private Toggle asuarView;
    [SerializeField]
    private Toggle akGuiView;
    [SerializeField]
    private Toggle tailView;
    //
    [SerializeField]
    private Toggle hyonMu;
    [SerializeField]
    private Toggle baekHo;
    [SerializeField]
    private Toggle pet;
    [SerializeField]
    private Toggle orb;
    [SerializeField]
    private Toggle indra;
    [SerializeField]
    private Toggle dragon;
    [SerializeField]
    private Toggle oneSkill;
    //
    [SerializeField]
    private Toggle fourView;
    [SerializeField]
    private Toggle showOtherPlayer;

    [SerializeField]
    private Toggle showFoxCup;
    [SerializeField]
    private Toggle showWolfRing;
    [SerializeField]
    private Toggle showDragonBracelet;
    [SerializeField]
    private Toggle showMunha;

    [SerializeField]
    private Toggle showRingEffect; 
    
    [SerializeField]
    private Toggle newUi;  
    
    [SerializeField]
    private Toggle norigaeSize;
    
    [SerializeField]
    private Toggle showGumgiSoul;
    [SerializeField]
    private Toggle showVisionSkill;
    [SerializeField]
    private Toggle showDosulSkill;

    [SerializeField]
    private Transform playerViewController;

    private void Awake()
    {
        Initialize();
    }


    private void Start()
    {
        playerViewController.parent = InGameCanvas.Instance.transform;
    }

    private bool initialized = false;

    private void Initialize()
    {
        bgmSlider.value = PlayerPrefs.GetFloat(SettingKey.bgmVolume);
        efxSlider.value = PlayerPrefs.GetFloat(SettingKey.efxVolume);
        vierSlider.value = PlayerPrefs.GetFloat(SettingKey.view);
        uiVierSlider.value = PlayerPrefs.GetFloat(SettingKey.uiView);
        jotStickSlider.value = PlayerPrefs.GetFloat(SettingKey.joyStick);

        graphicOptionToggle[PlayerPrefs.GetInt(SettingKey.GraphicOption)].isOn = true;

        frameRateToggle[PlayerPrefs.GetInt(SettingKey.FrameRateOption)].isOn = true;

        showDamageFontToggle.isOn = PlayerPrefs.GetInt(SettingKey.ShowDamageFont) == 1;

        showEffectToggle.isOn = PlayerPrefs.GetInt(SettingKey.ShowEffect) == 1;

        shakeCameraToggle.isOn = PlayerPrefs.GetInt(SettingKey.GlowEffect) == 1;

        showSleepPushToggle.isOn = PlayerPrefs.GetInt(SettingKey.ShowSleepPush) == 1;
        
        batterySafeModeToggle.isOn = PlayerPrefs.GetInt(SettingKey.batterySafeMode) == 1;

        yachaEffectToggle.isOn = PlayerPrefs.GetInt(SettingKey.YachaEffect) == 1;

        hpBarToggle.isOn = PlayerPrefs.GetInt(SettingKey.HpBar) == 1;

        enemyViewToggle.isOn = PlayerPrefs.GetInt(SettingKey.ViewEnemy) == 1;
        //
        sonView.isOn = PlayerPrefs.GetInt(SettingKey.sonView) == 1;
        dogView.isOn = PlayerPrefs.GetInt(SettingKey.dogView) == 1;
        marbleCircleView.isOn = PlayerPrefs.GetInt(SettingKey.marbleCircleView) == 1;
        asuarView.isOn = PlayerPrefs.GetInt(SettingKey.asuarView) == 1;
        akGuiView.isOn = PlayerPrefs.GetInt(SettingKey.akGuiView) == 1;
        tailView.isOn = PlayerPrefs.GetInt(SettingKey.tailView) == 1;
        //
        hyonMu.isOn = PlayerPrefs.GetInt(SettingKey.hyonMu) == 1;
        baekHo.isOn = PlayerPrefs.GetInt(SettingKey.baekHo) == 1;
        pet.isOn = PlayerPrefs.GetInt(SettingKey.pet) == 1;
        orb.isOn = PlayerPrefs.GetInt(SettingKey.orb) == 1;
        indra.isOn = PlayerPrefs.GetInt(SettingKey.indra) == 1;
        dragon.isOn = PlayerPrefs.GetInt(SettingKey.dragon) == 1;
        oneSkill.isOn = PlayerPrefs.GetInt(SettingKey.oneSkill) == 1;
        
        //

        fourView.isOn = PlayerPrefs.GetInt(SettingKey.fourView) == 1;
        showOtherPlayer.isOn = PlayerPrefs.GetInt(SettingKey.showOtherPlayer) == 1;

        showFoxCup.isOn = PlayerPrefs.GetInt(SettingKey.showFoxCup) == 1;
        showWolfRing.isOn = PlayerPrefs.GetInt(SettingKey.showWolfRing) == 1;
        showDragonBracelet.isOn = PlayerPrefs.GetInt(SettingKey.showDragonBracelet) == 1;
        showMunha.isOn = PlayerPrefs.GetInt(SettingKey.showMunha) == 1;
        
        showRingEffect.isOn = PlayerPrefs.GetInt(SettingKey.showRingEffect) == 1;
        newUi.isOn = PlayerPrefs.GetInt(SettingKey.newUi) == 1;
        norigaeSize.isOn = PlayerPrefs.GetInt(SettingKey.norigaeSize) == 1;
        showGumgiSoul.isOn = PlayerPrefs.GetInt(SettingKey.showGumgiSoul) == 1;
        showVisionSkill.isOn = PlayerPrefs.GetInt(SettingKey.showVisionSkill) == 1;
        showDosulSkill.isOn = PlayerPrefs.GetInt(SettingKey.showDosulSkill) == 1;

        initialized = true;

        SetSliderTexts();
    }

    private void SetSliderTexts()
    {
        bgmDesc.SetText(((int)(bgmSlider.value * 100)).ToString());
        sfxDesc.SetText(((int)(efxSlider.value * 100)).ToString());
        viewDesc.SetText(((int)(vierSlider.value * 100)).ToString());
        uiViewDesc.SetText(((int)(uiVierSlider.value * 100)).ToString());
        uiJoyStickDesc.SetText(((int)(jotStickSlider.value * 100)).ToString());
    }


    public void WhenBgmSliderChanged(float value)
    {
        if (initialized == false) return;
        SettingData.bgmVolume.Value = value;
        bgmDesc.SetText(((int)(value * 100)).ToString());
    }

    public void WhenEfxSliderChanged(float value)
    {
        if (initialized == false) return;
        SettingData.efxVolume.Value = value;
        sfxDesc.SetText(((int)(value * 100)).ToString());
    }

    public void WhenViewSliderChanged(float value)
    {
        if (initialized == false) return;
        SettingData.view.Value = value;
        viewDesc.SetText(((int)(value * 100)).ToString());
    }

    public void WhenJoyStickSliderChanged(float value)
    {
        if (initialized == false) return;
        SettingData.joyStick.Value = value;
        uiJoyStickDesc.SetText(((int)(value * 100)).ToString());
    }

    public void WhenUiViewSliderChanged(float value)
    {
        if (initialized == false) return;
        SettingData.uiView.Value = value;
        uiViewDesc.SetText(((int)(value * 100)).ToString());
    }

    public void Graphic_Low_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SettingData.GraphicOption.Value = 0;
            SoundManager.Instance.PlayButtonSound();
        }
    }

    public void Graphic_Middle_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SettingData.GraphicOption.Value = 1;
            SoundManager.Instance.PlayButtonSound();
        }
    }

    public void Graphic_High_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
            SettingData.GraphicOption.Value = 2;
        }
    }

    public void Graphic_Very_High_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
            SettingData.GraphicOption.Value = 3;
        }
    }

    public void FrameRate_Low_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
            SettingData.FrameRateOption.Value = 0;
        }
    }

    public void FrameRate_Middle_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
            SettingData.FrameRateOption.Value = 1;
        }
    }

    public void FrameRate_High_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
            SettingData.FrameRateOption.Value = 2;
        }
    }

    public void ShowDamageFont(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.ShowDamageFont.Value = on ? 1 : 0;
    }

    public void ShowEffect(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.ShowEffect.Value = on ? 1 : 0;
    }

    public void ShakeCamera(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.GlowEffect.Value = on ? 1 : 0;
    }

    public void ShowSleepPush(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.ShowSleepPush.Value = on ? 1 : 0;
    }
    public void BatterySafeMode(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.batterySafeMode.Value = on ? 1 : 0;
    }

    public void YachaEffect(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.YachaEffect.Value = on ? 1 : 0;
    }

    public void HpBar(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.HpBar.Value = on ? 1 : 0;
    }

    public void EnemyView(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.ViewEnemy.Value = on ? 1 : 0;
    }

    //
    public void SonView(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.sonView.Value = on ? 1 : 0;
    }
    public void DogView(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.dogView.Value = on ? 1 : 0;
    }
    public void MarbleCircleView(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.marbleCircleView.Value = on ? 1 : 0;
    }
    public void AsuarView(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.asuarView.Value = on ? 1 : 0;
    }
    public void AkGuiView(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.akGuiView.Value = on ? 1 : 0;
    }
    public void TailView(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.tailView.Value = on ? 1 : 0;
    }
    //
    public void HyonMu(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.hyonMu.Value = on ? 1 : 0;
    }
    public void BaekHo(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.baekHo.Value = on ? 1 : 0;
    }
    public void Pet(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.pet.Value = on ? 1 : 0;
    }
    public void Orb(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.orb.Value = on ? 1 : 0;
    }
    public void Indra(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.indra.Value = on ? 1 : 0;
    }

    public void Dragon(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.dragon.Value = on ? 1 : 0;
    }

    public void OneSkill(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showOneSkillEffect.Value = on ? 1 : 0;
    }

    public void FourView(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.fourView.Value = on ? 1 : 0;
    }
    public void PartyOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showOtherPlayer.Value = on ? 1 : 0;
    }

    //
    public void ShowFoxCupOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showFoxCup.Value = on ? 1 : 0;
    }
    public void ShowWolfRingOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showWolfRing.Value = on ? 1 : 0;
    }
    public void ShowDragonBraceletOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showDragonBracelet.Value = on ? 1 : 0;
    }
    public void ShowMunhaOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showMunha.Value = on ? 1 : 0;
    }

    public void ShowRingEffectOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showRingEffect.Value = on ? 1 : 0;
    } 
    
    public void ShowGumgiSoulOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showGumgiSoul.Value = on ? 1 : 0;
    }
    public void ShowVisionSkillOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showVisionSkill.Value = on ? 1 : 0;
    }
    public void ShowDosulSkillOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showDosulSkill.Value = on ? 1 : 0;
    }
    
    public void NewUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.newUi.Value = on ? 1 : 0;
    }
    
    public void norigaeSizeOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.norigaeSize.Value = on ? 1 : 0;
    }
    //

    public void OnClickStory()
    {
        DialogManager.Instance.SetNextDialog();
    }

    public void OnClickCafeButton()
    {
        Application.OpenURL(CommonString.CafeURL);
    }
    public void LogOut()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "계정을 탈퇴 할까요?\n탈퇴는 7일간의 유예기간 뒤에 처리 됩니다\n그 전에 다시 로그인 하면 계정이 복구 됩니다.", () =>
        {
            PopupManager.Instance.ShowConfirmPopup("알림", "탈퇴 성공! 게임을 종료 해주세요. 데이터 통신이 실패 합니다.", null);
            Backend.BMember.SignOut("e");
        }, () => { });
    }

    private float AutoSaveDelay = 60f;
    private Coroutine autoSaveRoutine = null;
    private IEnumerator AutoSaveRoutine()
    {
        yield return new WaitForSeconds(AutoSaveDelay);
        autoSaveRoutine = null;
    }

    public void OnClickForceSaveButton()
    {
        if (autoSaveRoutine != null)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강제 저장은 1분에 한번만 가능합니다.", null);
            return;
        }

        SaveManager.Instance.SyncDatasInQueue();
        SaveManager.Instance.SyncDailyMissions();

        autoSaveRoutine = CoroutineExecuter.Instance.StartCoroutine(AutoSaveRoutine());
    }

    private void OnDestroy()
    {
        if (CoroutineExecuter.Instance != null && autoSaveRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoSaveRoutine);
        }
    }
}

public static class SettingKey
{
    public static string bgmVolume = "bgmVolume";
    public static string efxVolume = "efxVolume";
    public static string view = "view";
    public static string uiView = "uiView";
    public static string joyStick = "joyStick";
    public static string GraphicOption = "GraphicOption";
    public static string FrameRateOption = "FrameRateOption";
    public static string ShowDamageFont = "ShowDamageFont";
    public static string ShowEffect = "ShowEffect";
    public static string GlowEffect = "GlowEffect";
    public static string PotionUseHpOption = "PotionUseHpOption";
    public static string GachaWhiteEffect = "GachaWhiteEffect";
    public static string ShowSleepPush = "ShowSleepPush";
    public static string batterySafeMode = "batterySafeMode";
    public static string YachaEffect = "YachaEffect";
    public static string HpBar = "HpBar";
    public static string ViewEnemy = "ViewEnemy";
    //
    public static string sonView = "sonView";
    public static string dogView = "dogView";
    public static string marbleCircleView = "marbleCircleView";
    public static string asuarView = "asuarView";
    public static string akGuiView = "akGuiView";
    public static string tailView = "tailView";
    //
    public static string hyonMu = "hyonMu";
    public static string baekHo = "baekHo";
    public static string pet = "pet";
    public static string orb = "orb";
    public static string indra = "indra";
    public static string dragon = "dragon";
    public static string oneSkill = "oneSkill";

    public static string fourView = "fourView";
    public static string showOtherPlayer = "showOtherPlayer";

    public static string showFoxCup = "showFoxCup";
    public static string showWolfRing = "showWolfRing";
    public static string showDragonBracelet = "showDragonBracelet";
    public static string showMunha = "showMunha";
    public static string showRingEffect = "showRingEffect";
    public static string newUi = "newUi";
    public static string towerAutoMode = "towerAutoMode";
    public static string norigaeSize = "norigaeSize";//수호동물임

    public static string autoVisionSkill = "autoVisionSkill";
    public static string showGumgiSoul = "showGumgiSoul";
    public static string showVisionSkill = "showVisionSkill";
    public static string showDosulSkill = "showDosulSkill";

    public static string showByeolhotitle = "showByeolhotitle";

    public static string showBanditUi = "showBanditUi";
    public static string showTowerUi = "showTowerUi";
    public static string showSmithUi = "showSmithUi";
    public static string showDokebiUi = "showDokebiUi";
    public static string showSoulForestUi = "showSoulForestUi";
    public static string showTaegeukUi = "showTaegeukUi";
    public static string showBackguiUi = "showBackguiUi";
    public static string showSonUi = "showSonUi";
    public static string showSusanoUi = "showSusanoUi";
    public static string showFoxmaskUi = "showFoxmaskUi";
    public static string showKingTestUi = "showKingTestUi";
    public static string showGradeTestUi = "showGradeTestUi";
    public static string showVisionTowerUi = "showVisionTowerUi";
    public static string showSuhoTowerUi = "showSuhoTowerUi";
    public static string showFoxTowerUi = "showFoxTowerUi";
    public static string showGodTrialUi = "showGodTrialUi";
    public static string showTransUi = "showTransUi";
    public static string showDanjeon = "showDanjeon";
    public static string showClosed = "showClosed";
    public static string showBlackFox = "showBlackFox";

    public static string showCatUi = "showCatUi";
    public static string showTwelveUi = "showTwelveUi";
    public static string showHwansuUi = "showHwansuUi";
    public static string showGumihoUi = "showGumihoUi";
    public static string showNewYoguiUi = "showNewYoguiUi";
    public static string showSuhosinUi = "showSuhosinUi";
    public static string showSasinsuUi = "showSasinsuUi";
    public static string showSahyungsuUi = "showSahyungsuUi";
    public static string showVisionBossUi = "showVisionBossUi";
    public static string showFoxUi = "showFoxUi";
    public static string showSangoonUi= "showSangoonUi";
    public static string showChunguUi= "showChunguUi";
    public static string showNewBossUi= "showNewBossUi";
    public static string showSuhoBossUi= "showSuhoBossUi";

    public static string showHellUi = "showHellUi";
    public static string showChunUi = "showChunUi";
    public static string showDoUi = "showDoUi";
    public static string showSumiUi = "showSumiUi";
    public static string showThiefUi = "showThiefUi";
    public static string showDarkUi = "showDarkUi";
    public static string showSinsunUi = "showSinsunUi";
    public static string showDragonUi = "showDragonUi";
    public static string showDragonPalaceUi = "showDragonPalaceUi";
    public static string showMurimUi = "showMurimUi";

}

public static class SettingData
{
    //볼룸
    public static ReactiveProperty<float> bgmVolume = new ReactiveProperty<float>();
    public static ReactiveProperty<float> efxVolume = new ReactiveProperty<float>();
    public static ReactiveProperty<float> view = new ReactiveProperty<float>();
    public static ReactiveProperty<float> uiView = new ReactiveProperty<float>();
    public static ReactiveProperty<float> joyStick = new ReactiveProperty<float>();
    public static ReactiveProperty<int> GraphicOption = new ReactiveProperty<int>(); //하중상최상
    public static ReactiveProperty<int> FrameRateOption = new ReactiveProperty<int>(); //30 45 60
    public static ReactiveProperty<int> ShowDamageFont = new ReactiveProperty<int>();
    public static ReactiveProperty<int> ShowEffect = new ReactiveProperty<int>();
    public static ReactiveProperty<int> GlowEffect = new ReactiveProperty<int>();
    public static ReactiveProperty<int> PotionUseHpOption = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> GachaWhiteEffect = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> ShowSleepPush = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> batterySafeMode = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    
    public static ReactiveProperty<int> YachaEffect = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> HpBar = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> ViewEnemy = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    //
    public static ReactiveProperty<int> sonView = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> dogView = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> marbleCircleView = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> asuarView = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> akGuiView = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> tailView = new ReactiveProperty<int>();//x이하일떄 (3개옵션)


    public static ReactiveProperty<int> hyonMu = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> baekHo = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> pet = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> orb = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> indra = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> dragon = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> showOneSkillEffect = new ReactiveProperty<int>();//x이하일떄 (3개옵션)

    public static ReactiveProperty<int> fourView = new ReactiveProperty<int>();//x이하일떄 (3개옵션)

    public static ReactiveProperty<int> showOtherPlayer = new ReactiveProperty<int>();//x이하일떄 (3개옵션)

    public static ReactiveProperty<int> showFoxCup = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> showWolfRing = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> showDragonBracelet = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> showMunha = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> showRingEffect = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> newUi = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> towerAutoMode = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> showByeolhotitle = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> norigaeSize = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    
    public static ReactiveProperty<int> autoVisionSkill = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> showGumgiSoul = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> showVisionSkill = new ReactiveProperty<int>();//x이하일떄 (3개옵션)
    public static ReactiveProperty<int> showDosulSkill = new ReactiveProperty<int>();//x이하일떄 (3개옵션)

    //한계돌파
    public static ReactiveProperty<int> showBanditUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showTowerUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSmithUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showDokebiUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSoulForestUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showTaegeukUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showBackguiUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSonUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSusanoUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showFoxmaskUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showKingTestUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showGradeTestUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showVisionTowerUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSuhoTowerUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showFoxTowerUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showGodTrialUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showTransUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showDanjeon = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showClosed = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showBlackFox = new ReactiveProperty<int>();

    //보스도전
    public static ReactiveProperty<int> showCatUi    = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showTwelveUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showHwansuUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showGumihoUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showNewYoguiUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSuhosinUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSasinsuUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSahyungsuUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showVisionBossUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showFoxUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSangoonUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showChunguUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showNewBossUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSuhoBossUi = new ReactiveProperty<int>();

    //삼천세계
    public static ReactiveProperty<int> showHellUi  = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showChunUi  = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showDoUi    = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSumiUi  = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showThiefUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showDarkUi  = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showSinsunUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showDragonUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showDragonPalaceUi = new ReactiveProperty<int>();
    public static ReactiveProperty<int> showMurimUi = new ReactiveProperty<int>();
    public static int screenWidth = Screen.width;
    public static int screenHeight = Screen.height;

    public static void InitFirst()
    {
        FirstInit();
        Initialize();
    }
    static void FirstInit()
    {
        if (PlayerPrefs.HasKey(SettingKey.bgmVolume) == false)
            PlayerPrefs.SetFloat(SettingKey.bgmVolume, 0.5f);

        if (PlayerPrefs.HasKey(SettingKey.efxVolume) == false)
            PlayerPrefs.SetFloat(SettingKey.efxVolume, 0.5f);

        if (PlayerPrefs.HasKey(SettingKey.view) == false)
            PlayerPrefs.SetFloat(SettingKey.view, 1f);

        if (PlayerPrefs.HasKey(SettingKey.GraphicOption) == false)
            PlayerPrefs.SetInt(SettingKey.GraphicOption, 2);

        if (PlayerPrefs.HasKey(SettingKey.FrameRateOption) == false)
            PlayerPrefs.SetInt(SettingKey.FrameRateOption, 2);

        if (PlayerPrefs.HasKey(SettingKey.ShowDamageFont) == false)
            PlayerPrefs.SetInt(SettingKey.ShowDamageFont, 1);

        if (PlayerPrefs.HasKey(SettingKey.ShowEffect) == false)
            PlayerPrefs.SetInt(SettingKey.ShowEffect, 1);

        if (PlayerPrefs.HasKey(SettingKey.GlowEffect) == false)
            PlayerPrefs.SetInt(SettingKey.GlowEffect, 0);

        if (PlayerPrefs.HasKey(SettingKey.PotionUseHpOption) == false)
            PlayerPrefs.SetInt(SettingKey.PotionUseHpOption, 1);

        if (PlayerPrefs.HasKey(SettingKey.uiView) == false)
            PlayerPrefs.SetFloat(SettingKey.uiView, 0f);

        if (PlayerPrefs.HasKey(SettingKey.joyStick) == false)
            PlayerPrefs.SetFloat(SettingKey.joyStick, 0f);

        if (PlayerPrefs.HasKey(SettingKey.GachaWhiteEffect) == false)
            PlayerPrefs.SetInt(SettingKey.GachaWhiteEffect, 1);

        if (PlayerPrefs.HasKey(SettingKey.ShowSleepPush) == false)
            PlayerPrefs.SetInt(SettingKey.ShowSleepPush, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.batterySafeMode) == false)
            PlayerPrefs.SetInt(SettingKey.batterySafeMode, 0);

        if (PlayerPrefs.HasKey(SettingKey.YachaEffect) == false)
            PlayerPrefs.SetInt(SettingKey.YachaEffect, 1);

        if (PlayerPrefs.HasKey(SettingKey.HpBar) == false)
            PlayerPrefs.SetInt(SettingKey.HpBar, 1);

        //
        if (PlayerPrefs.HasKey(SettingKey.sonView) == false)
            PlayerPrefs.SetInt(SettingKey.sonView, 1);


        if (PlayerPrefs.HasKey(SettingKey.dogView) == false)
            PlayerPrefs.SetInt(SettingKey.dogView, 1);

        if (PlayerPrefs.HasKey(SettingKey.marbleCircleView) == false)
            PlayerPrefs.SetInt(SettingKey.marbleCircleView, 1);

        if (PlayerPrefs.HasKey(SettingKey.asuarView) == false)
            PlayerPrefs.SetInt(SettingKey.asuarView, 1);

        if (PlayerPrefs.HasKey(SettingKey.akGuiView) == false)
            PlayerPrefs.SetInt(SettingKey.akGuiView, 1);

        if (PlayerPrefs.HasKey(SettingKey.tailView) == false)
            PlayerPrefs.SetInt(SettingKey.tailView, 1);

        //
        if (PlayerPrefs.HasKey(SettingKey.hyonMu) == false)
            PlayerPrefs.SetInt(SettingKey.hyonMu, 1);

        if (PlayerPrefs.HasKey(SettingKey.baekHo) == false)
            PlayerPrefs.SetInt(SettingKey.baekHo, 1);

        if (PlayerPrefs.HasKey(SettingKey.pet) == false)
            PlayerPrefs.SetInt(SettingKey.pet, 1);

        if (PlayerPrefs.HasKey(SettingKey.orb) == false)
            PlayerPrefs.SetInt(SettingKey.orb, 1);

        if (PlayerPrefs.HasKey(SettingKey.indra) == false)
            PlayerPrefs.SetInt(SettingKey.indra, 1);

        if (PlayerPrefs.HasKey(SettingKey.dragon) == false)
            PlayerPrefs.SetInt(SettingKey.dragon, 1);

        if (PlayerPrefs.HasKey(SettingKey.oneSkill) == false)
            PlayerPrefs.SetInt(SettingKey.oneSkill, 0);
        //


        if (PlayerPrefs.HasKey(SettingKey.fourView) == false)
            PlayerPrefs.SetInt(SettingKey.fourView, 1);

        if (PlayerPrefs.HasKey(SettingKey.ViewEnemy) == false)
            PlayerPrefs.SetInt(SettingKey.ViewEnemy, 1);

        if (PlayerPrefs.HasKey(SettingKey.showOtherPlayer) == false)
            PlayerPrefs.SetInt(SettingKey.showOtherPlayer, 1);

        if (PlayerPrefs.HasKey(SettingKey.showFoxCup) == false)
            PlayerPrefs.SetInt(SettingKey.showFoxCup, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showWolfRing) == false)
            PlayerPrefs.SetInt(SettingKey.showWolfRing, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showDragonBracelet) == false)
            PlayerPrefs.SetInt(SettingKey.showDragonBracelet, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showMunha) == false)
            PlayerPrefs.SetInt(SettingKey.showMunha, 1);

        if (PlayerPrefs.HasKey(SettingKey.showRingEffect) == false)
            PlayerPrefs.SetInt(SettingKey.showRingEffect, 1); 
        
        if (PlayerPrefs.HasKey(SettingKey.showGumgiSoul) == false)
            PlayerPrefs.SetInt(SettingKey.showGumgiSoul, 1);   
        
        if (PlayerPrefs.HasKey(SettingKey.showVisionSkill) == false)
            PlayerPrefs.SetInt(SettingKey.showVisionSkill, 1);   
        
        if (PlayerPrefs.HasKey(SettingKey.showDosulSkill) == false)
            PlayerPrefs.SetInt(SettingKey.showDosulSkill, 1);   
        
        if (PlayerPrefs.HasKey(SettingKey.newUi) == false)
            PlayerPrefs.SetInt(SettingKey.newUi, 1);     
        
        if (PlayerPrefs.HasKey(SettingKey.towerAutoMode) == false)
            PlayerPrefs.SetInt(SettingKey.towerAutoMode, 1);     
        
        if (PlayerPrefs.HasKey(SettingKey.showByeolhotitle) == false)
            PlayerPrefs.SetInt(SettingKey.showByeolhotitle, 1);     
        
        if (PlayerPrefs.HasKey(SettingKey.norigaeSize) == false)
            PlayerPrefs.SetInt(SettingKey.norigaeSize, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.autoVisionSkill) == false)
            PlayerPrefs.SetInt(SettingKey.autoVisionSkill, 1);
        //한계돌파
        if (PlayerPrefs.HasKey(SettingKey.showBanditUi) == false)
            PlayerPrefs.SetInt(SettingKey.showBanditUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showTowerUi) == false)
            PlayerPrefs.SetInt(SettingKey.showTowerUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showSmithUi) == false)
            PlayerPrefs.SetInt(SettingKey.showSmithUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showDokebiUi) == false)
            PlayerPrefs.SetInt(SettingKey.showDokebiUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showTaegeukUi) == false)
            PlayerPrefs.SetInt(SettingKey.showTaegeukUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showSoulForestUi) == false)
            PlayerPrefs.SetInt(SettingKey.showSoulForestUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showBackguiUi) == false)
            PlayerPrefs.SetInt(SettingKey.showBackguiUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showSonUi) == false)
            PlayerPrefs.SetInt(SettingKey.showSonUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showSusanoUi) == false)
            PlayerPrefs.SetInt(SettingKey.showSusanoUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showFoxmaskUi) == false)
            PlayerPrefs.SetInt(SettingKey.showFoxmaskUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showKingTestUi) == false)
            PlayerPrefs.SetInt(SettingKey.showKingTestUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showGradeTestUi) == false)
            PlayerPrefs.SetInt(SettingKey.showGradeTestUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showVisionTowerUi) == false)
            PlayerPrefs.SetInt(SettingKey.showVisionTowerUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showSuhoTowerUi) == false)
            PlayerPrefs.SetInt(SettingKey.showSuhoTowerUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showFoxTowerUi) == false)
            PlayerPrefs.SetInt(SettingKey.showFoxTowerUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showGodTrialUi) == false)
            PlayerPrefs.SetInt(SettingKey.showGodTrialUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showTransUi) == false)
            PlayerPrefs.SetInt(SettingKey.showTransUi, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showDanjeon) == false)
            PlayerPrefs.SetInt(SettingKey.showDanjeon, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showClosed) == false)
            PlayerPrefs.SetInt(SettingKey.showClosed, 1);
        
        if (PlayerPrefs.HasKey(SettingKey.showBlackFox) == false)
            PlayerPrefs.SetInt(SettingKey.showBlackFox, 1);
        
        //

    }

    static void Initialize()
    {
        bgmVolume.Value = PlayerPrefs.GetFloat(SettingKey.bgmVolume, 1f);
        efxVolume.Value = PlayerPrefs.GetFloat(SettingKey.efxVolume, 1f);
        view.Value = PlayerPrefs.GetFloat(SettingKey.view, 0.5f);
        GraphicOption.Value = PlayerPrefs.GetInt(SettingKey.GraphicOption, 2);
        FrameRateOption.Value = PlayerPrefs.GetInt(SettingKey.FrameRateOption, 2);
        ShowDamageFont.Value = PlayerPrefs.GetInt(SettingKey.ShowDamageFont, 1);
        ShowEffect.Value = PlayerPrefs.GetInt(SettingKey.ShowEffect, 1);
        GlowEffect.Value = PlayerPrefs.GetInt(SettingKey.GlowEffect, 1);
        PotionUseHpOption.Value = PlayerPrefs.GetInt(SettingKey.PotionUseHpOption, 1);
        uiView.Value = PlayerPrefs.GetFloat(SettingKey.uiView, 0f);
        joyStick.Value = PlayerPrefs.GetFloat(SettingKey.joyStick, 0f);
        GachaWhiteEffect.Value = PlayerPrefs.GetInt(SettingKey.GachaWhiteEffect, 1);
        ShowSleepPush.Value = PlayerPrefs.GetInt(SettingKey.ShowSleepPush, 1);
        batterySafeMode.Value = PlayerPrefs.GetInt(SettingKey.batterySafeMode, 1);
        YachaEffect.Value = PlayerPrefs.GetInt(SettingKey.YachaEffect, 1);
        HpBar.Value = PlayerPrefs.GetInt(SettingKey.HpBar, 1);
        ViewEnemy.Value = PlayerPrefs.GetInt(SettingKey.ViewEnemy, 1);
        //
        sonView.Value = PlayerPrefs.GetInt(SettingKey.sonView, 1);
        fourView.Value = PlayerPrefs.GetInt(SettingKey.fourView, 1);
        dogView.Value = PlayerPrefs.GetInt(SettingKey.dogView, 1);
        marbleCircleView.Value = PlayerPrefs.GetInt(SettingKey.marbleCircleView, 1);
        asuarView.Value = PlayerPrefs.GetInt(SettingKey.asuarView, 1);
        akGuiView.Value = PlayerPrefs.GetInt(SettingKey.akGuiView, 1);
        tailView.Value = PlayerPrefs.GetInt(SettingKey.tailView, 1);

        hyonMu.Value = PlayerPrefs.GetInt(SettingKey.hyonMu, 1);
        baekHo.Value = PlayerPrefs.GetInt(SettingKey.baekHo, 1);
        pet.Value = PlayerPrefs.GetInt(SettingKey.pet, 1);
        orb.Value = PlayerPrefs.GetInt(SettingKey.orb, 1);
        indra.Value = PlayerPrefs.GetInt(SettingKey.indra, 1);
        dragon.Value = PlayerPrefs.GetInt(SettingKey.dragon, 1);


        showOneSkillEffect.Value = PlayerPrefs.GetInt(SettingKey.oneSkill, 0);

        showOtherPlayer.Value = PlayerPrefs.GetInt(SettingKey.showOtherPlayer, 1);

        showFoxCup.Value = PlayerPrefs.GetInt(SettingKey.showFoxCup, 1);
        showWolfRing.Value = PlayerPrefs.GetInt(SettingKey.showWolfRing, 1);
        showDragonBracelet.Value = PlayerPrefs.GetInt(SettingKey.showDragonBracelet, 1);
        showMunha.Value = PlayerPrefs.GetInt(SettingKey.showMunha, 1);
        showRingEffect.Value = PlayerPrefs.GetInt(SettingKey.showRingEffect, 1);
        
        newUi.Value = PlayerPrefs.GetInt(SettingKey.newUi, 1);
        
        towerAutoMode.Value = PlayerPrefs.GetInt(SettingKey.towerAutoMode, 1);
        
        showByeolhotitle.Value = PlayerPrefs.GetInt(SettingKey.showByeolhotitle, 1);
        
        norigaeSize.Value = PlayerPrefs.GetInt(SettingKey.norigaeSize, 1);
        
        autoVisionSkill.Value = PlayerPrefs.GetInt(SettingKey.autoVisionSkill, 1);
        
        showGumgiSoul.Value = PlayerPrefs.GetInt(SettingKey.showGumgiSoul, 1);
        
        showVisionSkill.Value = PlayerPrefs.GetInt(SettingKey.showVisionSkill, 1);
        
        showDosulSkill.Value = PlayerPrefs.GetInt(SettingKey.showDosulSkill, 1);
        
        showBanditUi.Value = PlayerPrefs.GetInt(SettingKey.showBanditUi, 1);
        
        showTowerUi.Value = PlayerPrefs.GetInt(SettingKey.showTowerUi, 1);
        
        showSmithUi.Value = PlayerPrefs.GetInt(SettingKey.showSmithUi, 1);

        showDokebiUi.Value = PlayerPrefs.GetInt(SettingKey.showDokebiUi, 1);
        
        showSoulForestUi.Value = PlayerPrefs.GetInt(SettingKey.showSoulForestUi, 1);
        
        showTaegeukUi.Value = PlayerPrefs.GetInt(SettingKey.showTaegeukUi, 1);

        showBackguiUi.Value = PlayerPrefs.GetInt(SettingKey.showBackguiUi, 1);
        
        showSonUi.Value = PlayerPrefs.GetInt(SettingKey.showSonUi, 1);
        
        showSusanoUi.Value = PlayerPrefs.GetInt(SettingKey.showSusanoUi, 1);

        showFoxmaskUi.Value = PlayerPrefs.GetInt(SettingKey.showFoxmaskUi, 1);
        
        showKingTestUi.Value = PlayerPrefs.GetInt(SettingKey.showKingTestUi, 1);
        
        showGradeTestUi.Value = PlayerPrefs.GetInt(SettingKey.showGradeTestUi, 1);
        
        showVisionTowerUi.Value = PlayerPrefs.GetInt(SettingKey.showVisionTowerUi, 1);
        
        showSuhoTowerUi.Value = PlayerPrefs.GetInt(SettingKey.showSuhoTowerUi, 1);
        
        showFoxTowerUi.Value = PlayerPrefs.GetInt(SettingKey.showFoxTowerUi, 1);
        
        showGodTrialUi.Value = PlayerPrefs.GetInt(SettingKey.showGodTrialUi, 1);
        
        showTransUi.Value = PlayerPrefs.GetInt(SettingKey.showTransUi, 1);
        
        showDanjeon.Value = PlayerPrefs.GetInt(SettingKey.showDanjeon, 1);
        
        showClosed.Value = PlayerPrefs.GetInt(SettingKey.showClosed, 1);
        
        showBlackFox.Value = PlayerPrefs.GetInt(SettingKey.showBlackFox, 1);
        
        
        //보스도전
        showCatUi.Value = PlayerPrefs.GetInt(SettingKey.showCatUi, 1);

        showTwelveUi.Value = PlayerPrefs.GetInt(SettingKey.showTwelveUi, 1);

        showHwansuUi.Value = PlayerPrefs.GetInt(SettingKey.showHwansuUi, 1);

        showGumihoUi.Value = PlayerPrefs.GetInt(SettingKey.showGumihoUi, 1);

        showNewYoguiUi.Value = PlayerPrefs.GetInt(SettingKey.showNewYoguiUi, 1);

        showSuhosinUi.Value = PlayerPrefs.GetInt(SettingKey.showSuhosinUi, 1);

        showSasinsuUi.Value = PlayerPrefs.GetInt(SettingKey.showSasinsuUi, 1);

        showSahyungsuUi.Value = PlayerPrefs.GetInt(SettingKey.showSahyungsuUi, 1);

        showVisionBossUi.Value = PlayerPrefs.GetInt(SettingKey.showVisionBossUi, 1);

        showFoxUi.Value = PlayerPrefs.GetInt(SettingKey.showFoxUi, 1);

        showSangoonUi.Value = PlayerPrefs.GetInt(SettingKey.showSangoonUi, 1);

        showChunguUi.Value = PlayerPrefs.GetInt(SettingKey.showChunguUi, 1);

        showNewBossUi.Value = PlayerPrefs.GetInt(SettingKey.showNewBossUi, 1);
        
        showSuhoBossUi.Value = PlayerPrefs.GetInt(SettingKey.showSuhoBossUi, 1);

        showHellUi.Value = PlayerPrefs.GetInt(SettingKey.showHellUi, 1);

        showChunUi.Value = PlayerPrefs.GetInt(SettingKey.showChunUi, 1);

        showDoUi.Value = PlayerPrefs.GetInt(SettingKey.showDoUi, 1);

        showSumiUi.Value = PlayerPrefs.GetInt(SettingKey.showSumiUi, 1);

        showThiefUi.Value = PlayerPrefs.GetInt(SettingKey.showThiefUi, 1);

        showDarkUi.Value = PlayerPrefs.GetInt(SettingKey.showDarkUi, 1);

        showSinsunUi.Value = PlayerPrefs.GetInt(SettingKey.showSinsunUi, 1);

        showDragonUi.Value = PlayerPrefs.GetInt(SettingKey.showDragonUi, 1);

        showDragonPalaceUi.Value = PlayerPrefs.GetInt(SettingKey.showDragonPalaceUi, 1);

        showMurimUi.Value = PlayerPrefs.GetInt(SettingKey.showMurimUi, 1);

        Subscribe();
    }

    static void Subscribe()
    {
        bgmVolume.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetFloat(SettingKey.bgmVolume, e);
        });

        efxVolume.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetFloat(SettingKey.efxVolume, e);
        });

        view.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetFloat(SettingKey.view, e);
        }); ;

        joyStick.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetFloat(SettingKey.joyStick, e);
        });

        uiView.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetFloat(SettingKey.uiView, e);
        });

        GraphicOption.AsObservable().Subscribe(e => { PlayerPrefs.SetInt(SettingKey.GraphicOption, e); });
        FrameRateOption.AsObservable().Subscribe(e => { PlayerPrefs.SetInt(SettingKey.FrameRateOption, e); });

        ShowDamageFont.AsObservable().Subscribe(e => { PlayerPrefs.SetInt(SettingKey.ShowDamageFont, e); });
        ShowEffect.AsObservable().Subscribe(e => { PlayerPrefs.SetInt(SettingKey.ShowEffect, e); });
        GlowEffect.AsObservable().Subscribe(e => { PlayerPrefs.SetInt(SettingKey.GlowEffect, e); });
        PotionUseHpOption.AsObservable().Subscribe(e =>
        {
            Debug.LogError($"Potion optionChanged {e}");
            PlayerPrefs.SetInt(SettingKey.PotionUseHpOption, e);
        });

        GraphicOption.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.GraphicOption, e);
            SetGraphicOption(e);
        });

        GachaWhiteEffect.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.GachaWhiteEffect, e);
        });

        ShowSleepPush.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.ShowSleepPush, e);
        });
        batterySafeMode.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.batterySafeMode, e);
        });

        YachaEffect.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.YachaEffect, e);
        });

        HpBar.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.HpBar, e);
        });

        ViewEnemy.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.ViewEnemy, e);
        });
        //
        sonView.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.sonView, e);
        });
        dogView.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.dogView, e);
        });
        marbleCircleView.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.marbleCircleView, e);
        });
        asuarView.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.asuarView, e);
        });
        akGuiView.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.akGuiView, e);
        });
        tailView.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.tailView, e);
        });
        //
        hyonMu.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.hyonMu, e);
        });
        baekHo.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.baekHo, e);
        });
        pet.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.pet, e);
        });
        orb.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.orb, e);
        });
        indra.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.indra, e);
        });
        dragon.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.dragon, e);
        });
        showOneSkillEffect.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.oneSkill, e);
        });
        //
        fourView.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.fourView, e);
        });

        showOtherPlayer.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showOtherPlayer, e);
        }); 
        
        showGumgiSoul.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showGumgiSoul, e);
        });
        
        showVisionSkill.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showVisionSkill, e);
        });
        showDosulSkill.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showDosulSkill, e);
        });

        showFoxCup.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showFoxCup, e);
        });

        showWolfRing.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showWolfRing, e);
        });

        showDragonBracelet.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showDragonBracelet, e);
        });
        
        showMunha.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showMunha, e);
        });

        showRingEffect.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showRingEffect, e);
        }); 
        
        newUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.newUi, e);
        }); 
        towerAutoMode.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.towerAutoMode, e);
        }); 
        showByeolhotitle.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showByeolhotitle, e);
        }); 
        
        norigaeSize.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.norigaeSize, e);
        });
        autoVisionSkill.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.autoVisionSkill, e);
        });
        showBanditUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showBanditUi, e);
        });
        showTowerUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showTowerUi, e);
        });
        showSmithUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSmithUi, e);
        });
        showDokebiUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showDokebiUi, e);
        });
        showSoulForestUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSoulForestUi, e);
        });
        showTaegeukUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showTaegeukUi, e);
        });
        showBackguiUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showBackguiUi, e);
        });
        showSonUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSonUi, e);
        });
        showSusanoUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSusanoUi, e);
        });
        showFoxmaskUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showFoxmaskUi, e);
        });
        showKingTestUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showKingTestUi, e);
        });
        showGradeTestUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showGradeTestUi, e);
        });
        showVisionTowerUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showVisionTowerUi, e);
        });
        showSuhoTowerUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSuhoTowerUi, e);
        });
        showFoxTowerUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showFoxTowerUi, e);
        });
        showGodTrialUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showGodTrialUi, e);
        });
        showTransUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showTransUi, e);
        });
        showDanjeon.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showDanjeon, e);
        });
        showClosed.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showClosed, e);
        });
        showBlackFox.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showBlackFox, e);
        });
//
        showCatUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showCatUi, e);
        });
        showTwelveUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showTwelveUi, e);
        });
        showHwansuUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showHwansuUi, e);
        });
        showGumihoUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showGumihoUi, e);
        });
        showNewYoguiUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showNewYoguiUi, e);
        });
        showSuhosinUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSuhosinUi, e);
        });
        showSasinsuUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSasinsuUi, e);
        });
        showSahyungsuUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSahyungsuUi, e);
        });
        showVisionBossUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showVisionBossUi, e);
        });
        showFoxUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showFoxUi, e);
        });
        showSangoonUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSangoonUi, e);
        });
        showChunguUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showChunguUi, e);
        });
        showNewBossUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showNewBossUi, e);
        });
        showSuhoBossUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSuhoBossUi, e);
        });
        
        showHellUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showHellUi, e);
        });
        showChunUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showChunUi, e);
        });
        showDoUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showDoUi, e);
        });
        showSumiUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSumiUi, e);
        });
        showThiefUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showThiefUi, e);
        });
        showDarkUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showDarkUi, e);
        });
        showSinsunUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showSinsunUi, e);
        });
        showDragonUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showDragonUi, e);
        });
        showDragonPalaceUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showDragonPalaceUi, e);
        });

        showMurimUi.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.showMurimUi, e);
        });

    }

    public static void SetGraphicOption(int option)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (option == 0)
            {
                Screen.SetResolution(640, 640 * screenHeight / screenWidth, true);
            }
            else if (option == 1)
            {
                Screen.SetResolution(1280, 1280 * screenHeight / screenWidth, true);
            }
            else if (option == 2)
            {
                Screen.SetResolution(1500, 1500 * screenHeight / screenWidth, true);
            }
            else
            {
                Screen.SetResolution(screenWidth, screenHeight, true);
            }
        }
    }
}


