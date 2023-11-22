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

public class UIByeolhoTowerBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField]
    private UiByeolhoTowerRewardView uiTowerRewardView;


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
        scoreText.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.byeolhoTowerScore].Value * GameBalance.BossScoreConvertToOrigin)}");
    }

    void OnEnable()
    {
        SetStageText();
        SetReward();
    }

    private bool IsAllClear()
    {
        int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoTowerRewardIndex).Value;

        return currentFloor >= TableManager.Instance.ByeolhoTower.dataArray.Length;
    }

    private void SetStageText()
    {
        if (IsAllClear() == false)
        {
            int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoTowerRewardIndex).Value;

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
            int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoTowerRewardIndex).Value;

            if (currentFloor >= TableManager.Instance.ByeolhoTower.dataArray.Length)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"잘못된 데이터 idx : {currentFloor}", null);
                return;
            }

            if (currentFloor == -1)
            {
                currentFloor = 0;
            }

            var towerTableData = TableManager.Instance.ByeolhoTower.dataArray[currentFloor];

            uiTowerRewardView.UpdateRewardView(towerTableData.Id);
        }
        else
        {
            
            var towerTableData = TableManager.Instance.ByeolhoTower.dataArray.Last();

            uiTowerRewardView.UpdateRewardView(towerTableData.Id);
        }
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 할까요?", () => { GameManager.Instance.LoadContents(GameManager.ContentsType.ByeolhoTower); }, () => { });
    }

    public void OnClickInstantClearButton()
    {
        int currentClearStageId = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.byeolhoTowerRewardIndex).Value;

        if (currentClearStageId < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("수련의방을 클리어 해야 합니다.");
            return;
        }

        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.ByeolhoClear].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.ByeolhoClear)}이 없습니다.");
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
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.ByeolhoClear)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }


        int instantClearGetNum = (int)TableManager.Instance.ByeolhoTower.dataArray[currentClearStageId].Sweepvalue * inputNum;

        string desc = "";

            desc +=
                $"{currentClearStageId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.ByeolhoGoods)} {instantClearGetNum}개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({currentClearStageId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.ByeolhoGoods)} {(int)TableManager.Instance.ByeolhoTower.dataArray[currentClearStageId].Sweepvalue}개 획득)</color>";


        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
            desc,
            () =>
            {
                int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.ByeolhoClear].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(
                        $"{CommonString.GetItemName(Item_Type.ByeolhoClear)}이 없습니다.");

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
                            $"{CommonString.GetItemName(Item_Type.ByeolhoClear)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
                ServerData.goodsTable.TableDatas[GoodsTable.ByeolhoClear].Value -= inputNum;
                ServerData.goodsTable.TableDatas[GoodsTable.ByeolhoGoods].Value += Mathf.Round(instantClearGetNum);;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.ByeolhoClear, ServerData.goodsTable.TableDatas[GoodsTable.ByeolhoClear].Value);
                goodsParam.Add(GoodsTable.ByeolhoGoods, ServerData.goodsTable.TableDatas[GoodsTable.ByeolhoGoods].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                ServerData.SendTransactionV2(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"소탕 완료!\n{CommonString.GetItemName(Item_Type.ByeolhoGoods)} {Utils.ConvertNum(instantClearGetNum)}개 획득!", null);
                    });
            }, null);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.ByeolhoClear).Value += 1;
        }
    }
#endif
}