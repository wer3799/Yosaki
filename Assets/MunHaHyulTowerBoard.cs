using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MunHaHyulTowerBoard : MonoBehaviour
{
    [SerializeField]
    private Image rewardIcon;

    [SerializeField]
    private TextMeshProUGUI rewardDescription;
    
    [SerializeField]
    private TextMeshProUGUI stageDescription;
    [SerializeField]
    private TextMeshProUGUI stageDamageDescription;

    [SerializeField]
    private Button leftButton;

    [SerializeField]
    private Button rightButton;

    [SerializeField] private TMP_InputField instantClearNum;

    [SerializeField] private TextMeshProUGUI currentGradeText;
    [SerializeField] private TextMeshProUGUI highestDamageText;

    private int bossId = 296;

    private int currentId;

    private void Start()
    {
        
        currentId = Mathf.Max(PlayerStats.GetMunhaHyulTowerGrade(),0);
        
        UpdateRewardView(currentId);
    }

    public void UpdateRewardView(int idx)
    {
        currentId = idx;

        stageDescription.SetText($"{currentId + 1}층 보상");
        
        var towerTableData = TableManager.Instance.StudentSpotTower.dataArray[idx];
        
        stageDamageDescription.SetText($"{Utils.ConvertNum(towerTableData.Rewrardcut)}");

        var currentGrade = PlayerStats.GetMunhaHyulTowerGrade();
        if (currentGrade < 0)
        {
            currentGradeText.SetText($"단계 없음");
        }
        else
        {
            currentGradeText.SetText($"현재 {currentGrade+1 }단계");
        }
        
        var scoreValue = ServerData.bossServerTable.TableDatas["b296"].score.Value;
        var score = -1d;
        if (string.IsNullOrEmpty(scoreValue) == false)
        {
            score = double.Parse(scoreValue);
        }
        if (score < 1)
        {
            highestDamageText.SetText($"점수 없음.");
        }
        else
        {
            highestDamageText.SetText($"최고점수 : {Utils.ConvertNum(score)}");
        }
        
        
        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)towerTableData.Rewardtype);

        rewardDescription.SetText($"{CommonString.GetItemName(Item_Type.HYG)}\n" +
                                  $"<color=yellow>클리어 보상 : {Utils.ConvertBigNum(towerTableData.Rewardvalue)}개</color>\n" +
                                  $"소탕 보상 : {Utils.ConvertBigNum(towerTableData.Sweepvalue)}개");

        UpdateButtonState();
    }

    public void OnClickLeftButton()
    {
        currentId--;

        currentId = Mathf.Max(currentId, 0);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }
    public void OnClickRightButton()
    {
        currentId++;

        currentId = Mathf.Min(currentId, TableManager.Instance.StudentSpotTower.dataArray.Length - 1);

        UpdateRewardView(currentId);

        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        leftButton.interactable = currentId != 0;
        rightButton.interactable = currentId != TableManager.Instance.StudentSpotTower.dataArray.Length - 1;
    }
    
    public void OnClickInstantClearButton()
    {
        int currentClearStageId = PlayerStats.GetMunhaHyulTowerGrade();

        if (currentClearStageId < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("혈자리 전수를 클리어 해야 합니다.");
            return;
        }

        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.HYC].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.HYC)}이 없습니다.");
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
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.HYC)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }


        int instantClearGetNum = (int)TableManager.Instance.StudentSpotTower.dataArray[currentClearStageId].Sweepvalue * inputNum;

        string desc = "";

            desc +=
                $"{currentClearStageId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.HYG)} {instantClearGetNum}개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({currentClearStageId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.HYG)} {(int)TableManager.Instance.StudentSpotTower.dataArray[currentClearStageId].Sweepvalue}개 획득)</color>";


        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
            desc,
            () =>
            {
                int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.HYC].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(
                        $"{CommonString.GetItemName(Item_Type.HYC)}이 없습니다.");

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
                            $"{CommonString.GetItemName(Item_Type.HYC)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
                ServerData.goodsTable.TableDatas[GoodsTable.HYC].Value -= inputNum;
                ServerData.goodsTable.TableDatas[GoodsTable.HYG].Value += Mathf.Round(instantClearGetNum);;

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.HYC, ServerData.goodsTable.TableDatas[GoodsTable.HYC].Value);
                goodsParam.Add(GoodsTable.HYG, ServerData.goodsTable.TableDatas[GoodsTable.HYG].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                ServerData.SendTransactionV2(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"소탕 완료!\n{CommonString.GetItemName(Item_Type.HYG)} {Utils.ConvertNum(instantClearGetNum)}개 획득!", null);
                    });
            }, null);
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "도전 할까요?", () =>
        {
            GameManager.Instance.SetBossId(bossId);
            GameManager.Instance.LoadContents(GameManager.ContentsType.TwelveDungeon);
        }, () => { });
        
    }
}
