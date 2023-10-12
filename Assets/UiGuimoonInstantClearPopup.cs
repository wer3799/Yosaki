using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGuimoonInstantClearPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI rewardDescription;

    [SerializeField]
    private TMP_InputField instantClearNum;


    private void Start()
    {
        SetAllRewardDescription();
    }

    private void SetAllRewardDescription()
    {
        var currentFloor= ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value;
        
        rewardDescription.SetText($"현재 스테이지 {Utils.ConvertStage((int)currentFloor+2)}단계\n" +
            $"소탕시 귀문석 {GameManager.Instance.CurrentStageData.Guimoonpoint}개  획득!");
    }

    public void OnClickInstantClearButton()
    {

        //레벨제한
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < 1000)
        {
            PopupManager.Instance.ShowAlarmMessage($"레벨 1000 달성을 필요로합니다!");
            return;
        }


        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.GuimoonRelicClearTicket].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GuimoonRelicClearTicket)}이 없습니다.");
            return;
        }

        if (int.TryParse(instantClearNum.text, out var inputNum))
        {
            if (inputNum == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                return;
            }
            if (inputNum > 200)
            {
                PopupManager.Instance.ShowAlarmMessage("소탕권은 200개 미만으로 사용가능합니다!");
                return;
            }
            else if (remainItemNum < inputNum)
            {
                PopupManager.Instance.ShowAlarmMessage(
                    $"{CommonString.GetItemName(Item_Type.GuimoonRelicClearTicket)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }

        int instanClearGetNum = (int)GameManager.Instance.CurrentStageData.Guimoonpoint * inputNum;
        
        
        
        string desc = "";

            desc +=
                $"{Utils.ConvertStage(GameManager.Instance.CurrentStageData.Id+2)}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.GuimoonRelic)} {instanClearGetNum}개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({Utils.ConvertStage(GameManager.Instance.CurrentStageData.Id+2)}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.GuimoonRelic)} {(int)GameManager.Instance.CurrentStageData.Guimoonpoint}개 획득)</color>";
        
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
            desc,
            () =>
            {
                int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.GuimoonRelicClearTicket].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(
                        $"{CommonString.GetItemName(Item_Type.GuimoonRelicClearTicket)}이 없습니다.");

                    return;
                }

                if (int.TryParse(instantClearNum.text, out var inputNum))
                {
                    if (inputNum == 0)
                    {
                        PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                        return;
                    }
                    else if (remainItemNum < inputNum)
                    {
                        PopupManager.Instance.ShowAlarmMessage(
                            $"{CommonString.GetItemName(Item_Type.GuimoonRelicClearTicket)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
                ServerData.goodsTable.TableDatas[GoodsTable.GuimoonRelicClearTicket].Value -= inputNum;
                ServerData.goodsTable.TableDatas[GoodsTable.GuimoonRelic].Value += instanClearGetNum;
                //티켓 사용횟수
                ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.usedGuimoonRelicTicket].Value += inputNum;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.GuimoonRelicClearTicket, ServerData.goodsTable.TableDatas[GoodsTable.GuimoonRelicClearTicket].Value);
                goodsParam.Add(GoodsTable.GuimoonRelic, ServerData.goodsTable.TableDatas[GoodsTable.GuimoonRelic].Value);

                Param userinfo2Param = new Param();
                userinfo2Param.Add(UserInfoTable_2.usedGuimoonRelicTicket, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.usedGuimoonRelicTicket].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));

                ServerData.SendTransactionV2(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"소탕 완료!\n{CommonString.GetItemName(Item_Type.GuimoonRelic)} {Utils.ConvertNum(instanClearGetNum)}개 획득!", null);

                        //남은재화(소탕권) / 사용한재화(소탕권) / 획득한 재화갯수
                        LogManager.Instance.SendLogType("Guimoon", "Clear", $"{ServerData.goodsTable.TableDatas[GoodsTable.GuimoonRelicClearTicket].Value}#{inputNum}#{instanClearGetNum}");
                    });
            }, null);
    }

}