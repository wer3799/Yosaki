using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UiRewardView;

public class UiMeditationTowerBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField]
    private UiMeditationTowerRewardView uiTowerRewardView;


    [SerializeField]
    private TextMeshProUGUI currentStageText_Real;


    // [SerializeField]
    // private GameObject normalRoot;
    //
    // [SerializeField]
    // private GameObject allClearRoot;

    [SerializeField]
    private TMP_InputField instantClearNum;

    private void Start()
    {
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.meditationTowerScore].Value * GameBalance.BossScoreConvertToOrigin)}");
    }

    void OnEnable()
    {
        SetStageText();
        SetReward();
    }

    private bool IsAllClear()
    {
        int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationTowerRewardIndex).Value;

        return currentFloor >= TableManager.Instance.MeditationTower.dataArray.Length;
    }

    private void SetStageText()
    {
        if (IsAllClear() == false)
        {
            int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationTowerRewardIndex).Value;

            if (currentFloor != -1)
            {
                currentStageText_Real.SetText($"현재 {currentFloor + 1}층");
            }
            else
            {
                currentStageText_Real.SetText($"기록 없음");
            }
        }
    }

    private void SetReward()
    {
        bool isAllClear = IsAllClear();

        // normalRoot.SetActive(isAllClear == false);
        // allClearRoot.SetActive(isAllClear == true);

        if (isAllClear == false)
        {
            int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationTowerRewardIndex).Value;

            if (currentFloor >= TableManager.Instance.MeditationTower.dataArray.Length)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"잘못된 데이터 idx : {currentFloor}", null);
                return;
            }

            if (currentFloor == -1)
            {
                currentFloor = 0;
            }

            var towerTableData = TableManager.Instance.MeditationTower.dataArray[currentFloor];

            uiTowerRewardView.UpdateRewardView(towerTableData.Id);
        }
        else
        {
            
            var towerTableData = TableManager.Instance.MeditationTower.dataArray.Last();

            uiTowerRewardView.UpdateRewardView(towerTableData.Id);
        }
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 할까요?", () => { GameManager.Instance.LoadContents(GameManager.ContentsType.MeditationTower); }, () => { });
    }

    public void OnClickInstantClearButton()
    {
        int currentClearStageId = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationTowerRewardIndex).Value;

        if (currentClearStageId < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("내면세계를 클리어 해야 합니다.");
            return;
        }

        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.MeditationClearTicket].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.MeditationClearTicket)}이 없습니다.");
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
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.MeditationClearTicket)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }


        int instantClearGetNum = (int)TableManager.Instance.MeditationTower.dataArray[currentClearStageId].Sweepvalue * inputNum;

        string desc = "";

        if (PlayerStats.GetMeditationGainValue() > 0f)
        {

            desc +=
                $"{currentClearStageId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.MeditationGoods)} {Utils.ConvertNum(instantClearGetNum)}(+{Utils.ConvertNum(instantClearGetNum * PlayerStats.GetMeditationGainValue())})개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({currentClearStageId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.MeditationGoods)} {(int)TableManager.Instance.MeditationTower.dataArray[currentClearStageId].Sweepvalue}(+{Utils.ConvertNum((int)TableManager.Instance.MeditationTower.dataArray[currentClearStageId].Sweepvalue * PlayerStats.GetMeditationGainValue())})개 획득)</color>";

        }
        else
        {
                
            desc +=
                $"{currentClearStageId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.MeditationGoods)} {instantClearGetNum}개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({currentClearStageId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.MeditationGoods)} {(int)TableManager.Instance.MeditationTower.dataArray[currentClearStageId].Sweepvalue}개 획득)</color>";

        }
    

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
            desc,
            () =>
            {
                int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.MeditationClearTicket].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(
                        $"{CommonString.GetItemName(Item_Type.MeditationClearTicket)}이 없습니다.");

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
                            $"{CommonString.GetItemName(Item_Type.MeditationClearTicket)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
                ServerData.goodsTable.TableDatas[GoodsTable.MeditationClearTicket].Value -= inputNum;
                ServerData.goodsTable.TableDatas[GoodsTable.MeditationGoods].Value += Mathf.Round(instantClearGetNum+(PlayerStats.GetMeditationGainValue()*instantClearGetNum));;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.MeditationClearTicket, ServerData.goodsTable.TableDatas[GoodsTable.MeditationClearTicket].Value);
                goodsParam.Add(GoodsTable.MeditationGoods, ServerData.goodsTable.TableDatas[GoodsTable.MeditationGoods].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                ServerData.SendTransactionV2(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"소탕 완료!\n{CommonString.GetItemName(Item_Type.MeditationGoods)} {instantClearGetNum+(PlayerStats.GetMeditationGainValue()*instantClearGetNum)}개 획득!", null);
                    });
            }, null);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.MeditationClearTicket).Value += 1;
        }
    }
#endif
}