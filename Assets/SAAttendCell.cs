using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.UI;

public class SAAttendCell : MonoBehaviour
{
    private SecondAttendData tableData;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Button getButton;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField] private Image buttonImage;

    private UiRewardResultView resultView;

    [SerializeField]
    private Image bg;

    [SerializeField]
    private Sprite bg_Normal;

    [SerializeField]
    private Sprite bg_Special;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventAttendRewarded).AsObservable().Subscribe(e => { UpdateUi(); }).AddTo(this);
    }

    public void Initialize(SecondAttendData data, UiRewardResultView result)
    {
        tableData = data;
        resultView = result;
        UpdateUi();
    }

    private void UpdateUi()
    {
        bg.sprite = tableData.Id == 2 || tableData.Id == 6 || tableData.Id == 13 ? bg_Special : bg_Normal;

        titleText.SetText($"2주년\n{tableData.Unlockday}일차 출석체크");


        string descrpition = "";
        for (int i = 0; i < tableData.Reward.Length; i++)
        {
            if ((Item_Type)tableData.Reward[i] == Item_Type.Mileage ||
                (Item_Type)tableData.Reward[i] == Item_Type.Event_SA
               )
            {
                descrpition += $"<color=yellow>{CommonString.GetItemName((Item_Type)tableData.Reward[i]) + " " + Utils.ConvertNum(tableData.Reward_Value[i])}개\n</color>";
            }
            else
            {
                descrpition += CommonString.GetItemName((Item_Type)tableData.Reward[i]) + " " + Utils.ConvertNum(tableData.Reward_Value[i]) + "개\n";
            }
        }

        descriptionText.SetText($"{descrpition}");
        if (CanGetReward())
        {
            getButton.interactable = true;
        }
        else
        {
            getButton.interactable = false;
        }

        if (IsRewarded())
        {
            buttonText.SetText("획득 완료");
        }
        else
        {
            buttonText.SetText("보상 받기");
        }

        buttonImage.enabled = IsRewarded() == false;
    }

    private bool CanGetReward()
    {
        return ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventMission2AttendCount).Value >= tableData.Unlockday;
    }

    private bool IsBeforeRewarded()
    {
        //0일때 1
        return (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventAttendRewarded).Value + 1 == tableData.Unlockday;
    }

    private bool IsRewarded()
    {
        return (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventAttendRewarded).Value >= tableData.Unlockday;
    }

    public void OnClickButton()
    {
        if (CanGetReward() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("일 수가 부족합니다!");
            return;
        }
        else if (IsRewarded() == true)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }
        else if (IsBeforeRewarded() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("이전 보상을 받아주세요!");
            return;
        }


        List<TransactionValue> transactions = new List<TransactionValue>();

        List<UiRewardView.RewardData> rewardData = new List<UiRewardView.RewardData>();

        Param goodsParam = new Param();

        for (int i = 0; i < tableData.Reward.Length; i++)
        {
            ServerData.goodsTable.GetTableData((Item_Type)tableData.Reward[i]).Value += tableData.Reward_Value[i];
            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Reward[i]), ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Reward[i])).Value);
            rewardData.Add(new UiRewardView.RewardData((Item_Type)tableData.Reward[i], tableData.Reward_Value[i]));
        }


        Param userInfo2Param = new Param();

        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventAttendRewarded).Value++;
        userInfo2Param.Add(UserInfoTable_2.eventAttendRewarded, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.eventAttendRewarded].Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            resultView.gameObject.SetActive(true);
            resultView.Initialize(rewardData);
            //PopupManager.Instance.ShowAlarmMessage("보상획득!");
        });
    }
}