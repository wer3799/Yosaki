using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class UiDosulSweepBoard : MonoBehaviour
{
    // [SerializeField]
    // private TextMeshProUGUI rewardDescription;

    // [SerializeField]
    // private TextMeshProUGUI allRewardDescription;

    // [SerializeField]
    // private GameObject notHasObject;
    //
    // [SerializeField]
    // private GameObject hasObject;

    [SerializeField]
    private TMP_InputField instantClearNum;

    private void Start()
    {
        // SetAllRewardDescription();
    }

    // private void SetAllRewardDescription()
    // {
    //     var tableData = TableManager.Instance.dosulTowerTable.dataArray;
    //
    //     string description = string.Empty;
    //
    //     for (int i = 0; i < tableData.Length; i++)
    //     {
    //         
    //         description += $"{tableData[i].Id+1}단계 1회 소탕시 {tableData[i].Sweepvalue}개 획득";
    //
    //         if (i != tableData.Length - 1)
    //         {
    //             description += "\n";
    //         }
    //     }
    //
    //     allRewardDescription.SetText(description);
    // }

    private void OnEnable()
    {
        UpdateUi();
    }

    private void UpdateUi()
    {
        // notHasObject.gameObject.SetActive(false);
        // hasObject.gameObject.SetActive(false);

        // int lastIdx = PlayerStats.GetDosulGrade();
        //
        // if (lastIdx == -1)
        // {
        //     notHasObject.gameObject.SetActive(true);
        //     return;
        // }
        //
        // hasObject.gameObject.SetActive(true);
        //
        // var dosulTableData = TableManager.Instance.dosulTowerTable.dataArray[lastIdx];

        // rewardDescription.SetText(
        //     $"현재 {dosulTableData.Id + 1}단계 1회 소탕시 {CommonString.GetItemName(Item_Type.DosulGoods)} {dosulTableData.Sweepvalue}개 획득!");
    }

    public void OnClickInstantClearButton()
    {
        int currentGradeId = PlayerStats.GetDosulGrade();

        //플레이 X
        if (currentGradeId == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("단계를 클리어 하셔야 소탕하실 수 있습니다!");
            return;
        }


        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.DosulClear].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DosulClear)}이 없습니다.");
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
                    $"{CommonString.GetItemName(Item_Type.DosulClear)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }

        int instanClearGetNum = (int)TableManager.Instance.dosulTowerTable.dataArray[currentGradeId].Sweepvalue * inputNum;
        string desc = "";
        if (PlayerStats.GetDosulGainValue() > 0f)
        {
            desc +=
                $"{currentGradeId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.DosulGoods)} {Utils.ConvertNum(instanClearGetNum)}(+{Utils.ConvertNum(instanClearGetNum * PlayerStats.GetDosulGainValue())})개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({currentGradeId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.DosulGoods)} {(int)TableManager.Instance.dosulTowerTable.dataArray[currentGradeId].Sweepvalue}(+{Utils.ConvertNum((int)TableManager.Instance.dosulTowerTable.dataArray[currentGradeId].Sweepvalue * PlayerStats.GetDosulGainValue())})개 획득)</color>";
        }
        else
        {
            desc +=
                $"{currentGradeId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.DosulGoods)} {Utils.ConvertNum(instanClearGetNum)}개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({currentGradeId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.DosulGoods)} {Utils.ConvertNum(TableManager.Instance.dosulTowerTable.dataArray[currentGradeId].Sweepvalue)}개 획득)</color>";
        }
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
            desc,
            () =>
            {
                int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.DosulClear].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(
                        $"{CommonString.GetItemName(Item_Type.DosulClear)}이 없습니다.");

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
                            $"{CommonString.GetItemName(Item_Type.DosulClear)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
                ServerData.goodsTable.TableDatas[GoodsTable.DosulClear].Value -= inputNum;
                ServerData.goodsTable.TableDatas[GoodsTable.DosulGoods].Value += Mathf.Round(instanClearGetNum+(PlayerStats.GetDosulGainValue()*instanClearGetNum));

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.DosulClear, ServerData.goodsTable.TableDatas[GoodsTable.DosulClear].Value);
                goodsParam.Add(GoodsTable.DosulGoods,
                    ServerData.goodsTable.TableDatas[GoodsTable.DosulGoods].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                ServerData.SendTransaction(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"소탕 완료!\n{CommonString.GetItemName(Item_Type.DosulGoods)} {instanClearGetNum+(PlayerStats.GetDosulGainValue()*instanClearGetNum)}개 획득!", null);

                        //남은재화(소탕권) / 사용한재화(소탕권) / 획득한 재화갯수
                        LogManager.Instance.SendLogType("Dosul", "Clear", $"{ServerData.goodsTable.TableDatas[GoodsTable.DosulClear].Value},{inputNum},{instanClearGetNum+(PlayerStats.GetDosulGainValue()*instanClearGetNum)}");
                    });
            }, null);
    }

    public void OnClickAllRecieveButton()
    {
        var rewardedIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dosulRewardIdx).Value;

        int currentGradeId = PlayerStats.GetDosulGrade();
        //플레이 X
        if (currentGradeId < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("등록된 점수가 없습니다");
            return;
        }

        if (currentGradeId <= rewardedIdx)
        {
            PopupManager.Instance.ShowAlarmMessage("받을 보상이 없습니다!");
            return;
        }

        var tableData = TableManager.Instance.dosulTowerTable.dataArray;

        float sumValue = 0f;
        //받보상 +1부터 현재 단계까지
        for (int i = rewardedIdx + 1; i <= currentGradeId; i++)
        {
            sumValue += tableData[i].Rewardvalue;
        }

        string descriptionText = string.Empty;

        if (rewardedIdx + 2 != currentGradeId + 1)
        {
            descriptionText = $"{rewardedIdx + 2}단계부터 {currentGradeId + 1}단계까지 보상을 수령합니다.\n{CommonString.GetItemName(Item_Type.DosulGoods)} {Utils.ConvertNum(sumValue)}개를 획득 하시겠습니까?\n";
        }
        else
        {
            descriptionText = $"{rewardedIdx + 2}단계 보상을 수령합니다.\n{CommonString.GetItemName(Item_Type.DosulGoods)} {Utils.ConvertNum(sumValue)}개를 획득 하시겠습니까?\n";
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, descriptionText
            ,
            () =>
            {
                int rewarededindex = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dosulRewardIdx).Value;

                if (currentGradeId <= rewarededindex)
                {
                    PopupManager.Instance.ShowAlarmMessage("받을 보상이 없습니다!");
                    return;
                }

                ServerData.goodsTable.TableDatas[GoodsTable.DosulGoods].Value += sumValue;

                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.dosulRewardIdx).Value = currentGradeId;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.DosulGoods, ServerData.goodsTable.TableDatas[GoodsTable.DosulGoods].Value);

                Param userInfo2Param = new Param();
                userInfo2Param.Add(UserInfoTable_2.dosulRewardIdx, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.dosulRewardIdx].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));

                ServerData.SendTransaction(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"모두 받기 완료!\n{CommonString.GetItemName(Item_Type.DosulGoods)} {sumValue}개 획득!", null);
                        sumValue = 0;
                        //LogManager.Instance.SendLogType("Dosul","Clear",$"{ServerData.goodsTable.TableDatas[GoodsTable.DosulClear].Value},{sumValue}");
                    });
            }, null);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.TableDatas[GoodsTable.DosulClear].Value += 10;
        }
    }
#endif
}