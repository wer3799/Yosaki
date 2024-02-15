using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UiRewardView;

public class UiDarkTowerBoard : MonoBehaviour
{
    [SerializeField]
    private UiDarkTowerRewardView uiTowerRewardView;

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
        towerAutoMode.isOn = PlayerPrefs.GetInt(SettingKey.towerAutoMode) == 1;
        
        initialized = true;
    }
    void OnEnable()
    {
        SetStageText();
        SetReward();
    }

    private bool IsAllClear()
    {
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.DarkTowerIdx).Value;

        return currentFloor >= TableManager.Instance.DarkTowerTable.dataArray.Length;
    }

    private void SetStageText()
    {
        if (IsAllClear() == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.DarkTowerIdx).Value;
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
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.DarkTowerIdx).Value;

            if (currentFloor >= TableManager.Instance.DarkTowerTable.dataArray.Length)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"잘못된 데이터 idx : {currentFloor}", null);
                return;
            }

            var towerTableData = TableManager.Instance.DarkTowerTable.dataArray[currentFloor];

            uiTowerRewardView.UpdateRewardView(towerTableData.Id);
        }
        else
        {     
 
            var towerTableData = TableManager.Instance.DarkTowerTable.dataArray.Last();

            uiTowerRewardView.UpdateRewardView(towerTableData.Id);
            
        }


    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 할까요?", () =>
        {

            GameManager.Instance.LoadContents(GameManager.ContentsType.DarkTower);

        }, () => { });
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
}
