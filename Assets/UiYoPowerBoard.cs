using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiYoPowerBoard : SingletonMono<UiYoPowerBoard>
{
    private new void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value < GameBalance.YoPowerUnlockStage)
        {
            PopupManager.Instance.ShowAlarmMessage($"{Utils.ConvertStage(GameBalance.YoPowerUnlockStage+2)} 스테이지 달성시 개방!");
            this.gameObject.SetActive(false);
        }
    }

}
