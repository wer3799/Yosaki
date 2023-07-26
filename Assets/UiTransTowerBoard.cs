using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UiRewardView;

public class UiTransTowerBoard : MonoBehaviour
{
    [SerializeField]
    private UiTransTowerRewardView uiTowerRewardView;

    [SerializeField]
    private TextMeshProUGUI currentStageText;

    [SerializeField]
    private TextMeshProUGUI currentStageText_Real;

    [SerializeField]
    private GameObject startObject;
    
    [SerializeField]
    private GameObject normalRoot;

    [SerializeField]
    private GameObject allClearRoot;

    [SerializeField]
    private TMP_InputField instantClearNum;

    private void Start()
    {
        startObject.SetActive(true);
    }

    void OnEnable()
    {
        SetStageText();
        SetReward();
    }

    private bool IsAllClear()
    {
        int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.transTowerIdx).Value;

        return currentFloor >= TableManager.Instance.TransTowerTable.dataArray.Length;
    }

    private void SetStageText()
    {
        if (IsAllClear() == false)
        {
            int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.transTowerIdx).Value;
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
            int currentFloor = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.transTowerIdx).Value;

            if (currentFloor >= TableManager.Instance.TransTowerTable.dataArray.Length)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"잘못된 데이터 idx : {currentFloor}", null);
                return;
            }

            var towerTableData = TableManager.Instance.TransTowerTable.dataArray[currentFloor];

            uiTowerRewardView.UpdateRewardView(towerTableData.Id);
        }
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 할까요?", () => { GameManager.Instance.LoadContents(GameManager.ContentsType.TransTower); }, () => { });
    }

    public void OnClickInstantClearButton()
    {
        int currentClearStageId = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.transTowerIdx).Value - 1;

        if (currentClearStageId < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("초월 동굴을 클리어 해야 합니다.");
            return;
        }

        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.TransClearTicket].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TransClearTicket)}이 없습니다.");
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
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TransClearTicket)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }


    int instanClearGetNum = (int)TableManager.Instance.TransTowerTable.dataArray[currentClearStageId].Sweepvalue * inputNum;

    string desc = "";
        desc +=
            $"{currentClearStageId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.TransGoods)} {instanClearGetNum}개를 획득 하시겠습니까?\n" +
            $"<color=yellow>({currentClearStageId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.TransGoods)} {(int)TableManager.Instance.TransTowerTable.dataArray[currentClearStageId].Sweepvalue}개 획득)</color>";

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
        desc,
        () =>
        {
            int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.TransClearTicket].Value;
    
            if (remainItemNum <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage(
                    $"{CommonString.GetItemName(Item_Type.TransClearTicket)}이 없습니다.");
    
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
                        $"{CommonString.GetItemName(Item_Type.TransClearTicket)}이 부족합니다!");
                    return;
                }
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                return;
            }
    
            //실제소탕
            ServerData.goodsTable.TableDatas[GoodsTable.TransClearTicket].Value -= inputNum;
            ServerData.goodsTable.TableDatas[GoodsTable.TransGoods].Value += instanClearGetNum;
    
            List<TransactionValue> transactions = new List<TransactionValue>();
    
            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.TransClearTicket, ServerData.goodsTable.TableDatas[GoodsTable.TransClearTicket].Value);
            goodsParam.Add(GoodsTable.TransGoods, ServerData.goodsTable.TableDatas[GoodsTable.TransGoods].Value);
    
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
    
            ServerData.SendTransactionV2(transactions,
                successCallBack: () =>
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                        $"소탕 완료!\n{CommonString.GetItemName(Item_Type.TransGoods)} {instanClearGetNum}개 획득!", null);
                });
        }, null);
     }
    
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.TransClearTicket).Value += 1;
        }
    }
#endif
}