using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiFastSleepRewardBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private Button button;

    [SerializeField]
    private GameObject waitDescription;

    [SerializeField]
    private GameObject allReceiveButton;

    [SerializeField] private List<UiAdRewardCell> _adRewardCells;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.dailySleepRewardReceiveCount].AsObservable().Subscribe(e =>
        {
            if (e == 0)
            {
                buttonDescription.SetText($"무료 받기\n{e}/{GameBalance.fastSleepRewardMaxCount}");
            }
            else
            {
                buttonDescription.SetText($"보상 받기\n{e}/{GameBalance.fastSleepRewardMaxCount}");
            }
        }).AddTo(this);
    }

    public void OnClickAddRewardButton()
    {
        double currentSleepTime = ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value;

        if (currentSleepTime >= GameBalance.sleepRewardMaxValue)
        {
            PopupManager.Instance.ShowAlarmMessage("휴식 보상이 최대 입니다.\n먼저 휴식 보상을 사용 해 주세요!");
            return;
        }

        int rewardedCount = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.dailySleepRewardReceiveCount].Value;

        if (rewardedCount >= GameBalance.fastSleepRewardMaxCount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 받으실 수 없습니다.");
            return;
        }

        //두번째부터 봐야됨.
        if (rewardedCount != 0)
        {
            AdManager.Instance.ShowRewardedReward(RewardRoutine);
        }
        // 첫번째는 광고 안봐도 됨.
        else
        {
            //광고제거시 5개받음
            if (HasRemoveAd())
            {
                RewardRoutine(GameBalance.fastSleepRewardMaxCount - rewardedCount);
            }
            else
            {
                RewardRoutine();
            }
        }
    }

    private bool HasRemoveAd()
    {
        return ServerData.iapServerTable.TableDatas["removead"].buyCount.Value > 0;

    }
    
    public void OnClickAllReceiveButton()
    {
        if (ServerData.iapServerTable.TableDatas["removead"].buyCount.Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("광고제거가 필요합니다.");
            return;
        }

        foreach (var adRewardCell in _adRewardCells)
        {
            adRewardCell.OnClickExchangeButton(false);
        }

        PopupManager.Instance.ShowAlarmMessage("모두 받기 완료!");
    }

    private void RewardRoutine()
    {
        double currentSleepTime = ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value;

        if (currentSleepTime >= GameBalance.sleepRewardMaxValue)
        {
            PopupManager.Instance.ShowAlarmMessage("휴식 보상이 최대 입니다.\n먼저 휴식 보상을 사용 해 주세요!");
            return;
        }

        int rewardedCount = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.dailySleepRewardReceiveCount].Value;

        if (rewardedCount >= GameBalance.fastSleepRewardMaxCount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 받으실 수 없습니다.");
            return;
        }


        button.interactable = false;
        waitDescription.SetActive(true);

        //24시간 예외처리
        ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value += GameBalance.fastSleepRewardTimeValue;

        ServerData.userInfoTable.TableDatas[UserInfoTable.dailySleepRewardReceiveCount].Value++;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userinfoParam = new Param();
        userinfoParam.Add(UserInfoTable.sleepRewardSavedTime, ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value);
        userinfoParam.Add(UserInfoTable.dailySleepRewardReceiveCount, ServerData.userInfoTable.TableDatas[UserInfoTable.dailySleepRewardReceiveCount].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));

        EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION3, 1);
        if (ServerData.userInfoTable.IsMonthlyPass2() == false)
        {
            EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearFast, 1);
        }
        else
        {
            EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearFast, 1);
        }
        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            button.interactable = true;
            waitDescription.SetActive(false);

            UiSleepRewardIndicator.Instance.ActiveButton();
            SleepRewardReceiver.Instance.SetComplete = false;

            PopupManager.Instance.ShowAlarmMessage("휴식보상이 추가 됐습니다(1시간)");
        });
    }
    private void RewardRoutine(int count)
    {
        double currentSleepTime = ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value;

        if (currentSleepTime >= GameBalance.sleepRewardMaxValue)
        {
            PopupManager.Instance.ShowAlarmMessage("휴식 보상이 최대 입니다.\n먼저 휴식 보상을 사용 해 주세요!");
            return;
        }

        int rewardedCount = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.dailySleepRewardReceiveCount].Value;

        if (rewardedCount >= GameBalance.fastSleepRewardMaxCount)
        {
            PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 받으실 수 없습니다.");
            return;
        }


        button.interactable = false;
        waitDescription.SetActive(true);

        //24시간 예외처리
        ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value +=
            GameBalance.fastSleepRewardTimeValue * count;

        ServerData.userInfoTable.TableDatas[UserInfoTable.dailySleepRewardReceiveCount].Value += count;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param userinfoParam = new Param();
        userinfoParam.Add(UserInfoTable.sleepRewardSavedTime, ServerData.userInfoTable.TableDatas[UserInfoTable.sleepRewardSavedTime].Value);
        userinfoParam.Add(UserInfoTable.dailySleepRewardReceiveCount, ServerData.userInfoTable.TableDatas[UserInfoTable.dailySleepRewardReceiveCount].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userinfoParam));

        EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION3, count);
        if (ServerData.userInfoTable.IsMonthlyPass2() == false)
        {
            EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearFast, count);
        }
        else
        {
            EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearFast, count);
        }
        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            button.interactable = true;
            waitDescription.SetActive(false);

            UiSleepRewardIndicator.Instance.ActiveButton();
            SleepRewardReceiver.Instance.SetComplete = false;

            PopupManager.Instance.ShowAlarmMessage($"휴식보상이 추가 됐습니다({count}시간)");
        });
    }
}