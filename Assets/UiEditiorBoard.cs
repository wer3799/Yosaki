using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiEditiorBoard : MonoBehaviour
{
   
    [SerializeField]
    private Toggle showBanditToggle;
    [SerializeField]
    private Toggle showTowerToggle;
    [SerializeField]
    private Toggle showSmithToggle;
    [SerializeField]
    private Toggle showDokebiToggle;
    [SerializeField]
    private Toggle showSoulForestToggle;
    [SerializeField]
    private Toggle showTaegeukToggle;
    [SerializeField]
    private Toggle showBackguiToggle;
    [SerializeField]
    private Toggle showSonToggle;
    [SerializeField]
    private Toggle showSusanoToggle;
    [SerializeField]
    private Toggle showFoxmaskToggle;
    [SerializeField]
    private Toggle showKingTestToggle;
    [SerializeField]
    private Toggle showGradeTestToggle;
    [SerializeField]
    private Toggle showVisionTowerToggle;
    [SerializeField]
    private Toggle showSuhoTowerToggle;
    [SerializeField]
    private Toggle showFoxTowerToggle;
    [SerializeField]
    private Toggle showGodTrialToggle;
    [SerializeField]
    private Toggle showTransUiToggle;
    [SerializeField]
    private Toggle showDanjeonUiToggle;
    [SerializeField]
    private Toggle showCatToggle;
    [SerializeField]
    private Toggle showTwelveToggle;
    [SerializeField]
    private Toggle showHwansuToggle;
    [SerializeField]
    private Toggle showGumihoToggle;
    [SerializeField]
    private Toggle showNewYoguiToggle;
    [SerializeField]
    private Toggle showSuhosinToggle;
    [SerializeField]
    private Toggle showSasinsuToggle;
    [SerializeField]
    private Toggle showSahyungsuToggle;
    [SerializeField]
    private Toggle showVisionBossToggle;
    [SerializeField]
    private Toggle showFoxToggle;
    [SerializeField]
    private Toggle showSangoonToggle;
    [SerializeField]
    private Toggle showChunguToggle;
    [SerializeField]
    private Toggle showNewBossToggle;
    [SerializeField]
    private Toggle showHellToggle;
    [SerializeField]
    private Toggle showChunToggle;
    [SerializeField]
    private Toggle showDoToggle;
    [SerializeField]
    private Toggle showSumiToggle;
    [SerializeField]
    private Toggle showThiefToggle;
    [SerializeField]
    private Toggle showDarkToggle;
    [SerializeField]
    private Toggle showSinsunToggle;


    private void Awake()
    {
        Initialize();
    }



    private bool initialized = false;

    private void Initialize()
    {
        
        showBanditToggle.isOn = PlayerPrefs.GetInt(SettingKey.showBanditUi) == 1;
        
        showTowerToggle.isOn = PlayerPrefs.GetInt(SettingKey.showTowerUi) == 1;
        
        showSmithToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSmithUi) == 1;
        
        showDokebiToggle.isOn = PlayerPrefs.GetInt(SettingKey.showDokebiUi) == 1;
        
        showSoulForestToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSoulForestUi) == 1;
        
        showTaegeukToggle.isOn = PlayerPrefs.GetInt(SettingKey.showTaegeukUi) == 1;
        
        showBackguiToggle.isOn = PlayerPrefs.GetInt(SettingKey.showBackguiUi) == 1;
        
        showSonToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSonUi) == 1;
        
        showSusanoToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSusanoUi) == 1;
        
        showFoxmaskToggle.isOn = PlayerPrefs.GetInt(SettingKey.showFoxmaskUi) == 1;
        
        showKingTestToggle.isOn = PlayerPrefs.GetInt(SettingKey.showKingTestUi) == 1;
        
        showGradeTestToggle.isOn = PlayerPrefs.GetInt(SettingKey.showGradeTestUi) == 1;
        
        showVisionTowerToggle.isOn = PlayerPrefs.GetInt(SettingKey.showVisionTowerUi) == 1;

        showSuhoTowerToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSuhoTowerUi) == 1;
        
        showFoxTowerToggle.isOn = PlayerPrefs.GetInt(SettingKey.showFoxTowerUi) == 1;
        
        showGodTrialToggle.isOn = PlayerPrefs.GetInt(SettingKey.showGodTrialUi) == 1;
        
        showTransUiToggle.isOn = PlayerPrefs.GetInt(SettingKey.showTransUi) == 1;
        
        showDanjeonUiToggle.isOn = PlayerPrefs.GetInt(SettingKey.showDanjeon) == 1;
        
        showCatToggle.isOn = PlayerPrefs.GetInt(SettingKey.showCatUi) == 1;
        
        showTwelveToggle.isOn = PlayerPrefs.GetInt(SettingKey.showTwelveUi) == 1;
        
        showHwansuToggle.isOn = PlayerPrefs.GetInt(SettingKey.showHwansuUi) == 1;
        
        showGumihoToggle.isOn = PlayerPrefs.GetInt(SettingKey.showGumihoUi) == 1;
        
        showNewYoguiToggle.isOn = PlayerPrefs.GetInt(SettingKey.showNewYoguiUi) == 1;
        
        showSuhosinToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSuhosinUi) == 1;
        
        showSasinsuToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSasinsuUi) == 1;
        
        showSahyungsuToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSahyungsuUi) == 1;
        
        showVisionBossToggle.isOn = PlayerPrefs.GetInt(SettingKey.showVisionBossUi) == 1;
        
        showFoxToggle.isOn = PlayerPrefs.GetInt(SettingKey.showFoxUi) == 1;
        
        showSangoonToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSangoonUi) == 1;

        showChunguToggle.isOn = PlayerPrefs.GetInt(SettingKey.showChunguUi) == 1;
        
        showNewBossToggle.isOn = PlayerPrefs.GetInt(SettingKey.showNewBossUi) == 1;
        
        showHellToggle.isOn = PlayerPrefs.GetInt(SettingKey.showHellUi) == 1;

        showChunToggle.isOn = PlayerPrefs.GetInt(SettingKey.showChunUi) == 1;

        showDoToggle.isOn = PlayerPrefs.GetInt(SettingKey.showDoUi) == 1;

        showSumiToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSumiUi) == 1;

        showThiefToggle.isOn = PlayerPrefs.GetInt(SettingKey.showThiefUi) == 1;

        showDarkToggle.isOn = PlayerPrefs.GetInt(SettingKey.showDarkUi) == 1;

        showSinsunToggle.isOn = PlayerPrefs.GetInt(SettingKey.showSinsunUi) == 1;


        initialized = true;
    }


    public void BanditUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showBanditUi.Value = on ? 1 : 0;
    }
    public void TowerUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showTowerUi.Value = on ? 1 : 0;
    }
    public void SmithUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSmithUi.Value = on ? 1 : 0;
    }
    public void DokebiUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showDokebiUi.Value = on ? 1 : 0;
    }
    public void SoulForestUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSoulForestUi.Value = on ? 1 : 0;
    }
    public void TaegeukUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showTaegeukUi.Value = on ? 1 : 0;
    }
    public void BackguiUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showBackguiUi.Value = on ? 1 : 0;
    }
    public void SonUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSonUi.Value = on ? 1 : 0;
    }
    public void SusanoUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSusanoUi.Value = on ? 1 : 0;
    }
    public void FoxmaskUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showFoxmaskUi.Value = on ? 1 : 0;
    }
    public void KingTestUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showKingTestUi.Value = on ? 1 : 0;
    }
    public void GradeTestUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showGradeTestUi.Value = on ? 1 : 0;
    }
    public void VisionTowerUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showVisionTowerUi.Value = on ? 1 : 0;
    }
    public void SuhoTowerUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSuhoTowerUi.Value = on ? 1 : 0;
    }
    public void FoxTowerUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showFoxTowerUi.Value = on ? 1 : 0;
    }
    public void GodTrialUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showGodTrialUi.Value = on ? 1 : 0;
    }
    
    public void TransUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showTransUi.Value = on ? 1 : 0;
    }
    
    public void DanjeonUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showDanjeon.Value = on ? 1 : 0;
    }
    
    public void CatUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showCatUi.Value = on ? 1 : 0;
    }
    public void TwelveUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showTwelveUi.Value = on ? 1 : 0;
    }
    public void HwansuUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showHwansuUi.Value = on ? 1 : 0;
    }
    public void GumihoUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showGumihoUi.Value = on ? 1 : 0;
    }
    public void NewYoguiUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showNewYoguiUi.Value = on ? 1 : 0;
    }
    public void SuhosinUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSuhosinUi.Value = on ? 1 : 0;
    }
    public void SasinsuUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSasinsuUi.Value = on ? 1 : 0;
    }
    public void SahyungsuUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSahyungsuUi.Value = on ? 1 : 0;
    }
    public void VisionBossUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showVisionBossUi.Value = on ? 1 : 0;
    }
    public void FoxUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showFoxUi.Value = on ? 1 : 0;
    }
    public void SangoonUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSangoonUi.Value = on ? 1 : 0;
    }
    public void ChunguUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showChunguUi.Value = on ? 1 : 0;
    }
    public void NewBossUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showNewBossUi.Value = on ? 1 : 0;
    }
    public void HellUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showHellUi.Value = on ? 1 : 0;
    }
    public void ChunUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showChunUi.Value = on ? 1 : 0;
    }
    public void DoUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showDoUi.Value = on ? 1 : 0;
    }
    public void SumiUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSumiUi.Value = on ? 1 : 0;
    }
    public void ThiefUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showThiefUi.Value = on ? 1 : 0;
    }
    public void DarkUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showDarkUi.Value = on ? 1 : 0;
    }
    public void SinsunUiOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.showSinsunUi.Value = on ? 1 : 0;
    }

}


