using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class TaegeukBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentFloor;

    [SerializeField]
    private SkeletonGraphic skeletonGraphic;
    
    [SerializeField]
    private Toggle towerAutoMode;
    private bool initialized = false;

    public void Start()
    {
        Subscribe();
        
        towerAutoMode.isOn = PlayerPrefs.GetInt(SettingKey.towerAutoMode) == 1;

        initialized = true;
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.taeguekTower].AsObservable().Subscribe(e => { currentFloor.SetText($"{e + 1}단계 입장"); }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {

            skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(e);

        }).AddTo(this);

    }
    
    public void OnClickEnterButton()
    {
        int currentIdx = (int)ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.taeguekTower].Value;

        if (currentIdx >= TableManager.Instance.taegeukTitle.dataArray.Length)
        {
            PopupManager.Instance.ShowAlarmMessage("도전 완료!");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{currentIdx + 1}단계\n도전 할까요?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.TaeguekTower);

        }, null);
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
