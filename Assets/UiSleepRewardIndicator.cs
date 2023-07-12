using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class UiSleepRewardIndicator : SingletonMono<UiSleepRewardIndicator>
{
    [SerializeField]
    private Transform rootObject;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private Button button;

    private void Start()
    {
        Subscribe();

        if (ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.graduateGold].Value >= 1)
        {
            ServerData.goodsTable.TableDatas[GoodsTable.Gold].Value = 0;
        }
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].AsObservable().Subscribe(e =>
        {
            rootObject.gameObject.SetActive(e > GameBalance.sleepRewardMinValue);

            TimeSpan ts = TimeSpan.FromSeconds(Mathf.Min((float)e, GameBalance.sleepRewardMaxValue));

            if (ts.Days == 0)
            {
                description.SetText($"{ts.Hours}시간 {ts.Minutes}분");
            }
            else
            {
                description.SetText($"{ts.TotalHours}시간");
            }
        }).AddTo(this);
    }

    public void OnClickGetRewardButton()
    {
        if(ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value> GameBalance.GoldLimit)
        {
            PopupManager.Instance.ShowAlarmMessage("금화가 너무 많습니다!\n기본 무공을 올려주세요!");
            return;
        }
        int elapsedTime = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value;

        if (elapsedTime <= GameBalance.sleepRewardMinValue)
        {
            PopupManager.Instance.ShowAlarmMessage($"보상을 받을 수 없습니다.");
            return;
        }

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("첫번째 스테이지 에서는 받으실 수 없습니다.");
            return;
        }

        button.interactable = false;

        SleepRewardReceiver.Instance.SetElapsedSecond(elapsedTime);

    }

    public void ActiveButton()
    {
        button.interactable = true;
    }

 

}
