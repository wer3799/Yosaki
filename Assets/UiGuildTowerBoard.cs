using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UiRewardView;

public class UiGuildTowerBoard : MonoBehaviour
{
    [SerializeField]
    private UiGuildTowerRewardView uiGuildTowerRewardView;

    [SerializeField]
    private TextMeshProUGUI currentStageText;

    [SerializeField]
    private TextMeshProUGUI currentStageText_Real;

    [SerializeField]
    private GameObject normalRoot;

    [SerializeField]
    private GameObject allClearRoot;

    [SerializeField]
    private TMP_InputField instantClearNum;

    void OnEnable()
    {
        SetStageText();
        SetReward();
    }

    private bool IsAllClear()
    {
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorGuildTower).Value;

        return currentFloor >= TableManager.Instance.guildTowerTable.dataArray.Length;
    }

    private void SetStageText()
    {
        if (IsAllClear() == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorGuildTower).Value;
            currentStageText.SetText($"{currentFloor + 1}층 입장");
            currentStageText_Real.SetText($"현재 {currentFloor}층");
        }
        else
        {
            currentStageText.SetText($"도전 완료!");
        }
    }

    private void SetReward()
    {
        bool isAllClear = IsAllClear();

        normalRoot.SetActive(isAllClear == false);
        allClearRoot.SetActive(isAllClear == true);

        if (isAllClear == false)
        {
            int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorGuildTower).Value;

            if (currentFloor >= TableManager.Instance.guildTowerTable.dataArray.Length)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"잘못된 데이터 idx : {currentFloor}", null);
                return;
            }

            var towerTableData = TableManager.Instance.guildTowerTable.dataArray[currentFloor];

            uiGuildTowerRewardView.UpdateRewardView(towerTableData.Id);
        }
        else
        {
            
            var towerTableData = TableManager.Instance.guildTowerTable.dataArray.Last();

            uiGuildTowerRewardView.UpdateRewardView(towerTableData.Id);
        }
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 할까요?", () => { GameManager.Instance.LoadContents(GameManager.ContentsType.GuildTower); }, () => { });
    }

    public void OnClickInstantClearButton()
    {
        int currentClearStageId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.currentFloorGuildTower].Value - 1;

        if (currentClearStageId < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("전갈굴을 클리어 해야 합니다.");
            return;
        }

        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.GuildTowerClearTicket].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GuildTowerClearTicket)}이 없습니다.");
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
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GuildTowerClearTicket)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }


        int instanClearGetNum = (int)TableManager.Instance.guildTowerTable.dataArray[currentClearStageId].Sweepvalue * inputNum;

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
            $"{currentClearStageId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.GuildReward)} {instanClearGetNum}개를 획득 하시겠습니까?\n" +
            $"<color=yellow>({currentClearStageId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.GuildReward)} {(int)TableManager.Instance.guildTowerTable.dataArray[currentClearStageId].Sweepvalue}개 획득)</color>",
            () =>
            {
                int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.GuildTowerClearTicket].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(
                        $"{CommonString.GetItemName(Item_Type.GuildTowerClearTicket)}이 없습니다.");

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
                            $"{CommonString.GetItemName(Item_Type.GuildTowerClearTicket)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
                ServerData.goodsTable.TableDatas[GoodsTable.GuildTowerClearTicket].Value -= inputNum;
                ServerData.goodsTable.TableDatas[GoodsTable.GuildReward].Value += instanClearGetNum;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.GuildTowerClearTicket, ServerData.goodsTable.TableDatas[GoodsTable.GuildTowerClearTicket].Value);
                goodsParam.Add(GoodsTable.GuildReward, ServerData.goodsTable.TableDatas[GoodsTable.GuildReward].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                ServerData.SendTransaction(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"소탕 완료!\n{CommonString.GetItemName(Item_Type.GuildReward)} {Utils.ConvertNum(instanClearGetNum)}개 획득!", null);
                    });
            }, null);
    }
}