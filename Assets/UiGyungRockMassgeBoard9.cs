using Photon.Pun.Demo.Cockpit;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGyungRockMassgeBoard9 : MonoBehaviour
{
 [SerializeField] private TextMeshProUGUI currentFloorDescription;
    [SerializeField] private TextMeshProUGUI currentFloorAbil;
    [SerializeField] private TextMeshProUGUI totalAbil;
    [SerializeField] private TextMeshProUGUI buttonDescription;
    [SerializeField] private TextMeshProUGUI nextFloorDescription;

    [SerializeField] private List<Image> markLists;
    [SerializeField] private List<Animator> markList_Anim;
    [SerializeField] private TextMeshProUGUI dokChimEffect;
    [SerializeField] private TextMeshProUGUI dokChimAbilPer;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        totalAbil.SetText(
            ($"{CommonString.GetStatusName((StatusType.SuperCritical33DamPer))} 총 {PlayerStats.GetGyungRockEffect9(StatusType.SuperCritical33DamPer) * 100f}% 적용됨"));

        int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower9).Value;

        currentFloorDescription.SetText($"{currentFloor}단계");

        currentFloorAbil.SetText($"혈자리(극혈3) {currentFloor}개 개방됨");

        if (currentFloor >= TableManager.Instance.gyungRockTowerTable9.dataArray.Length)
        {
            buttonDescription.SetText(($"최고단계"));
        }
        else
        {
            buttonDescription.SetText($"{currentFloor + 1}단계 도전");
        }

        for (int i = 0; i < markLists.Count; i++)
        {
            markLists[i].color = currentFloor > i ? Color.green : Color.red;

            markList_Anim[i].enabled = currentFloor > i;
        }

        dokChimEffect.SetText($"전갈굴(문파) 독침 효과로 극혈베기 피해량 {Utils.ConvertNum(Mathf.Round(PlayerStats.GetGuildTowerChimUpgradeValue() * 100f))}% 증폭됨<color=yellow>(+{PlayerStats.GetGuildTowerChimUpgradeValue() * PlayerStats.GetGyungRockEffect9(StatusType.SuperCritical33DamPer) * 100f}%)</color>");
        
        dokChimAbilPer.SetText($"1개당 단전베기 피해 {Utils.ConvertNum(Mathf.Round(GameBalance.GuildTowerChimAbilUpValue * 100f))}% 강화");
    }

    public void OnClickEnterButton()
    {
        int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.gyungRockTower9).Value;

        if (currentFloor < 0 || currentFloor >= TableManager.Instance.gyungRockTowerTable9.dataArray.Length)
        {
            PopupManager.Instance.ShowAlarmMessage(("최고 단계 입니다!"));
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () => { GameManager.Instance.LoadContents(GameManager.ContentsType.GyungRockTower9); }, () => { });
    }
}
