using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using BackEnd;

public class GetNewGachaGoods : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    public Button registerButton;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private int lockCount;

    [SerializeField]
    private TextMeshProUGUI lockDesc;
    
    [SerializeField]
    private GameObject NewGachaObject;
    void Start()
    {
        Subscribe();
    }
    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.getRingGoods].AsObservable().Subscribe(e =>
        {


            buttonDescription.SetText(e == 0 ? "획득" : "오늘 획득함");
        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[UserInfoTable.relicKillCount].AsObservable().Subscribe(e =>
        {
            if (e >= lockCount)
            {
                lockMask.SetActive(false);
                registerButton.interactable = true;
                NewGachaObject.transform.SetAsFirstSibling();
            }
            else
            {
                lockDesc.SetText($"영혼의 숲 처치 수\n{lockCount} 달성시 해금!");
                registerButton.interactable = false;
            }
        }).AddTo(this);
    }


    public void OnClickButton()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.NewGachaEnergy)}은 하루에 한번만 획득 가능합니다!");
            return;
        }

        //int amount = GameBalance.getRingGoodsAmount;
        int amount = GameBalance.getRingGoodsAmount * (int)Mathf.Floor(Mathf.Max(1, (float)ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).Value));

        if (amount == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{amount}개 획득 합니까?", () =>
        {
            if (ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value == 1)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.NewGachaEnergy)}은 하루에 한번만 획득 가능합니다!");
                return;
            }

            ServerData.userInfoTable.GetTableData(UserInfoTable.getRingGoods).Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += amount;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.getRingGoods, ServerData.userInfoTable.TableDatas[UserInfoTable.getRingGoods].Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            EventMissionManager.UpdateEventMissionClear(EventMissionKey.SMISSION9, 1);
            EventMissionManager.UpdateEventMissionClear(EventMissionKey.ANMISSION8, 1);

            if (ServerData.userInfoTable.IsMonthlyPass2() == false)
            {
                EventMissionManager.UpdateEventMissionClear(MonthMissionKey.ClearSoulStone, 1);
            }
            else
            {
                EventMissionManager.UpdateEventMissionClear(MonthMission2Key.ClearSoulStone, 1);
            }
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.NewGachaEnergy)} {amount}개 획득!", null);
            });
        }, null);
    }
}
