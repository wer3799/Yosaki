using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UiRewardView;

public class UiSumisanTowerBoard : MonoBehaviour
{
    [SerializeField]
    private UiSumisanTowerRewardView uiSumisanTowerRewardView;

    [SerializeField]
    private TextMeshProUGUI currentStageText;

    [SerializeField]
    private TextMeshProUGUI currentCriticalText;
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
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx4).Value;

        return currentFloor >= TableManager.Instance.sumisanTowerTable.dataArray.Length;
    }

    private void SetStageText()
    {
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx4).Value;

        if (currentFloor <= 0)
        {
            currentCriticalText.SetText($"미적용");
        }
        else
        {
            var towerTableData = TableManager.Instance.sumisanTowerTable.dataArray;

            string criticalDesc = string.Empty;

            Dictionary<int, float> abilContainer = new Dictionary<int, float>();

            for (int i = 0; i < currentFloor; i++)
            {
                if (abilContainer.ContainsKey(towerTableData[i].Rewardtype) == false)
                {
                    abilContainer.Add(towerTableData[i].Rewardtype, 0f);
                }

                abilContainer[towerTableData[i].Rewardtype] += towerTableData[i].Rewardvalue;
            }

            var e = abilContainer.GetEnumerator();

            while (e.MoveNext())
            {
                criticalDesc += $"{CommonString.GetStatusName((StatusType)e.Current.Key)} {e.Current.Value * 100}% 증가\n";
            }

            currentCriticalText.SetText(criticalDesc);


        }

        if (IsAllClear() == false)
        {
            currentStageText.SetText($"{currentFloor + 1}층 입장");
        }
        else
        {
            currentStageText.SetText($"도전 완료!");
        }

    }

    private void SetReward()
    {
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx4).Value;

        bool isAllClear = IsAllClear();

        if (isAllClear == false)
        {

            if (currentFloor >= TableManager.Instance.sumisanTowerTable.dataArray.Length)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"잘못된 데이터 idx : {currentFloor}", null);
                return;
            }

            var towerTableData = TableManager.Instance.sumisanTowerTable.dataArray[currentFloor];

            uiSumisanTowerRewardView.UpdateRewardView(towerTableData.Id);
        }
        else
        {
            
            var towerTableData = TableManager.Instance.sumisanTowerTable.dataArray.Last();

            uiSumisanTowerRewardView.UpdateRewardView(towerTableData.Id);
        }

    }

    public void OnClickEnterButton()
    {
        if(IsAllClear())
        {
            return;
        }
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 할까요?", () =>
        {

            GameManager.Instance.LoadContents(GameManager.ContentsType.SumisanTower);

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
