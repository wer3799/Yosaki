using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using UniRx;

public class TransUpgradeBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI currentAwakeLevel;

    [SerializeField]
    private TextMeshProUGUI requireText;

    [SerializeField]
    private TextMeshProUGUI oneClickCost;

    [SerializeField]
    private GameObject currentSelectedObejct;


    private int currentIdx = -1;

    private void Awake()
    {
        oneClickCost.SetText($"{Utils.ConvertNum(GameBalance.GetTransSoulUpgradePrice)}");
    }

    private void OnEnable()
    {
        currentIdx = PlayerStats.GetCurrentTransUpgradeIdx();
    }

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Trans_Level).AsObservable().Subscribe(e => { UpdateByCurrentId(); }).AddTo(this);
    }

    private void UpdateByCurrentId()
    {
        currentAwakeLevel.SetText($"현재 강화도 : + {ServerData.statusTable.GetTableData(StatusTable.Trans_Level).Value}강");

        currentIdx = Mathf.Max(currentIdx, 0);

        currentSelectedObejct.SetActive(currentIdx == PlayerStats.GetCurrentTransUpgradeIdx());

        var tableData = TableManager.Instance.TransUpgrade.dataArray[currentIdx];

        requireText.SetText($"강화 {tableData.Require} 필요");

        string description = string.Empty;

        float abilValue0 = PlayerStats.GetTransUpgradeAbilValue(currentIdx);

        abilDescription.SetText($"{CommonString.GetStatusName(StatusType.SuperCritical21DamPer)} {Utils.ConvertNum(abilValue0*100,2)} 상승");

        gradeText.SetText($"{currentIdx + 1}단계");
    }

    public void OnClickRightButton()
    {
        currentIdx++;
        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.TransUpgrade.dataArray.Length - 1);
        UpdateByCurrentId();
    }

    public void OnClickLeftButton()
    {
        currentIdx--;
        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.TransUpgrade.dataArray.Length - 1);
        UpdateByCurrentId();
    }

    public void OnClickButton()
    {
        float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value;

        if (currentMarble < GameBalance.GetTransSoulUpgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TransGoods)}이 부족합니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value -= GameBalance.GetTransSoulUpgradePrice;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.TransGoods, ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.statusTable.GetTableData(StatusTable.Trans_Level).Value++;

        Param statusParam = new Param();
        statusParam.Add(StatusTable.Trans_Level, ServerData.statusTable.GetTableData(StatusTable.Trans_Level).Value);
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));


#if UNITY_EDITOR
        Debug.LogError("강화성공");
#endif

        ServerData.SendTransactionV2(transactions, successCallBack: () => { PopupManager.Instance.ShowAlarmMessage("강화 성공!"); });
    }

    //
    public void OnClickButton_All()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"모든 {CommonString.GetItemName(Item_Type.TransGoods)}으로 강화 할까요?", () =>
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupManager.Instance.ShowAlarmMessage("네트워크 연결상태가 좋지 않습니다.\n다음에 다시 시도해 주세요!");
                return;
            }

            int prefUpgrade = PlayerStats.GetCurrentTransUpgradeIdx();

            float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value;

            if (currentMarble < GameBalance.GetTransSoulUpgradePrice)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TransGoods)}이 부족합니다.");
                return;
            }

            int upgradableNum = (int)(ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value / GameBalance.GetTransSoulUpgradePrice);

            ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value -= (GameBalance.GetTransSoulUpgradePrice * upgradableNum);

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.TransGoods, ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.statusTable.GetTableData(StatusTable.Trans_Level).Value += upgradableNum;

            Param statusParam = new Param();
            statusParam.Add(StatusTable.Trans_Level, ServerData.statusTable.GetTableData(StatusTable.Trans_Level).Value);
            transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

            LogManager.Instance.SendLogType("TransUpgrade", "all", $"pref {ServerData.statusTable.GetTableData(StatusTable.Trans_Level).Value - upgradableNum} +{upgradableNum}");

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                LogManager.Instance.SendLogType("TransUpgrade", "complete", "complete");

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"+{upgradableNum} 강화 성공!", null);
                var currentUpgrade = PlayerStats.GetCurrentTransUpgradeIdx();
                if (prefUpgrade < currentUpgrade)
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "단계 상승!", null);
                }
            });
        }, null);
    }

    public TMP_InputField inputField;

    public void OnClickSweepButton()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            if (int.TryParse(inputField.text, out int result))
            {
                if (result < 1)
                {
                    PopupManager.Instance.ShowAlarmMessage("올바른 개수가 아닙니다.");
                    return;
                }

                if (ServerData.goodsTable.GetTableData(GoodsTable.TransClearTicket).Value < result)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TransClearTicket)}이 부족합니다!");
                    return;
                }
                
                int score = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.transTowerIdx).Value;

                if (score < 0)
                {
                    PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
                    return;
                }

                var sweepValue = TableManager.Instance.TransTowerTable.dataArray[score].Sweepvalue;


                PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{sweepValue * result}개 획득 합니까?\n<color=red>({sweepValue} x {result} 획득 가능)</color>", () =>
                {
                    if (result < 1)
                    {
                        PopupManager.Instance.ShowAlarmMessage("올바른 개수가 아닙니다.");
                        return;
                    }

                    if (ServerData.goodsTable.GetTableData(GoodsTable.TransClearTicket).Value < result)
                    {
                        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TransClearTicket)}이 부족합니다!");
                        return;
                    }


                    ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value += sweepValue * result;
                    ServerData.goodsTable.GetTableData(GoodsTable.TransClearTicket).Value -= result;

                    List<TransactionValue> transactions = new List<TransactionValue>();


                    Param goodsParam = new Param();
                    goodsParam.Add(GoodsTable.TransGoods, ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value);
                    goodsParam.Add(GoodsTable.TransClearTicket, ServerData.goodsTable.GetTableData(GoodsTable.TransClearTicket).Value);

                    transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                    ServerData.SendTransactionV2(transactions, successCallBack: () => { PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.TransGoods)} {sweepValue * result}개 획득!", null); });
                }, null);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("소탕 횟수를 입력해주세요.");
            }
        }
    }
}