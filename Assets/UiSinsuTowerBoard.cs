using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UiRewardView;

public class UiSinsuTowerBoard : MonoBehaviour
{
    [FormerlySerializedAs("uiTower3RewardView")] [SerializeField]
    private UiTower5RewardView uiTower5RewardView;

    [SerializeField]
    private TextMeshProUGUI currentStageText;

    [SerializeField]
    private GameObject normalRoot;

    [SerializeField]
    private GameObject allClearRoot;
    
    [SerializeField]
    private Toggle towerAutoMode;
    
    private bool initialized = false;
    
    private void Start()
    {
        Subscribe();
        //Initialize();
    }
    
    private void Subscribe()
    {
        SettingData.towerAutoMode.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.towerAutoMode, e);
            Initialize();
        }); 
    }
    private void Initialize()
    {
        if (PlayerPrefs.HasKey(SettingKey.towerAutoMode) == false)
            PlayerPrefs.SetInt(SettingKey.towerAutoMode, 1);     
        
        towerAutoMode.isOn = PlayerPrefs.GetInt(SettingKey.towerAutoMode) == 1;
        
        initialized = true;
    }
    void OnEnable()
    {
        Initialize();
        SetStageText();
        SetReward();
    }

    private bool IsAllClear()
    {
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx6).Value;

        return currentFloor >= TableManager.Instance.sinsuTower.dataArray.Length;
    }
    
    public void AutoModeOnOff(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.towerAutoMode.Value = on ? 1 : 0;
    }
    
    private void SetStageText()
    {
        if (IsAllClear() == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx6).Value;
            currentStageText.SetText($"{currentFloor + 1}층 입장");
        }
        else
        {
            currentStageText.SetText($"도전 완료!");
        }

    }

    private void SetReward()
    {
        bool isAllClear = IsAllClear();

        normalRoot.SetActive(isAllClear == false);
        allClearRoot.SetActive(isAllClear == true);

        if (isAllClear == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx6).Value;

            if (currentFloor >= TableManager.Instance.sinsuTower.dataArray.Length)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"잘못된 데이터 idx : {currentFloor}", null);
                return;
            }

            var towerTableData = TableManager.Instance.sinsuTower.dataArray[currentFloor];

            uiTower5RewardView.UpdateRewardView(towerTableData.Id);
        }
        else
        {
            
            var towerTableData = TableManager.Instance.sinsuTower.dataArray.Last();

            uiTower5RewardView.UpdateRewardView(towerTableData.Id);
        }

    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 할까요?", () =>
        {

            GameManager.Instance.LoadContents(GameManager.ContentsType.SinsuTower);

        }, () => { });
    }
}
