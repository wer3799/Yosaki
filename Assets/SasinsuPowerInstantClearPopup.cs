using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SasinsuPowerInstantClearPopup : MonoBehaviour
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
            $"영약 복용시 {CommonString.GetItemName(Item_Type.SG)} {Utils.ConvertNum((int)SleepRewardReceiver.Instance.GetSinsuGoodsPerClearTicket())}개  획득!");
    }

    public void OnClickInstantClearButton()
    {
        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.SC].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SC)}이 없습니다.");
            return;
        }

        if (int.TryParse(instantClearNum.text, out var inputNum))
        {
            if (inputNum < 0)
            {
                PopupManager.Instance.ShowAlarmMessage(CommonString.InstantClear_Minus);
                return;
            }
            if (inputNum == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                return;
            }
            if (inputNum > 200)
            {
                PopupManager.Instance.ShowAlarmMessage("영약은 200개 미만으로 사용가능합니다!");
                return;
            }
            else if (remainItemNum < inputNum)
            {
                PopupManager.Instance.ShowAlarmMessage(
                    $"{CommonString.GetItemName(Item_Type.SC)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }

        var gaingoods = SleepRewardReceiver.Instance.GetSinsuGoodsPerClearTicket();
        var instantClearGetNum = (int)gaingoods * (inputNum);
        
        
        
        string desc = "";

            desc +=
                $"{Utils.ConvertStage(GameManager.Instance.CurrentStageData.Id+2)}단계를 {inputNum}번 영약을 사용하여\n{CommonString.GetItemName(Item_Type.SG)} {instantClearGetNum}개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({Utils.ConvertStage(GameManager.Instance.CurrentStageData.Id+2)}단계 영약 1개당 {CommonString.GetItemName(Item_Type.SG)} {(int)gaingoods}개 획득)</color>";
        
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
            desc,
            () =>
            {
                int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.SC].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(
                        $"{CommonString.GetItemName(Item_Type.SC)}이 없습니다.");

                    return;
                }

                if (int.TryParse(instantClearNum.text, out var inputNum))
                {
                    if (inputNum < 0)
                    {
                        PopupManager.Instance.ShowAlarmMessage(CommonString.InstantClear_Minus);
                        return;
                    }
                    if (inputNum == 0)
                    {
                        PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                        return;
                    }
                    else if (remainItemNum < inputNum)
                    {
                        PopupManager.Instance.ShowAlarmMessage(
                            $"{CommonString.GetItemName(Item_Type.SC)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
                ServerData.goodsTable.TableDatas[GoodsTable.SC].Value -= inputNum;
                ServerData.goodsTable.TableDatas[GoodsTable.SG].Value += instantClearGetNum;
                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.SC, ServerData.goodsTable.TableDatas[GoodsTable.SC].Value);
                goodsParam.Add(GoodsTable.SG, ServerData.goodsTable.TableDatas[GoodsTable.SG].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                ServerData.SendTransactionV2(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"사용 완료!\n{CommonString.GetItemName(Item_Type.SG)} {Utils.ConvertNum(instantClearGetNum)}개 획득!", null);

                        //남은재화(소탕권) / 사용한재화(소탕권) / 획득한 재화갯수
                        LogManager.Instance.SendLogType("Sasinsu", "Clear", $"{ServerData.goodsTable.TableDatas[GoodsTable.SC].Value}#{inputNum}#{instantClearGetNum}");
                    });
            }, null);
    }

}