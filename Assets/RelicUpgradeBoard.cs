using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using UniRx;

public class RelicUpgradeBoard : MonoBehaviour
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

    [SerializeField]
    private WeaponView weaponView;

    private int currentIdx = -1;

    private void Awake()
    {
        oneClickCost.SetText($"{Utils.ConvertNum(GameBalance.GetSoulRingUpgradePrice)}");
    }

    private void OnEnable()
    {
        if (CheckHasRing() == false)
        {
            return;
        }

        currentIdx = PlayerStats.GetCurrentRelicUpgradeIdx();
    }

    private void Start()
    {
        Subscribe();

        weaponView.Initialize(null, null, null, TableManager.Instance.NewGachaTable.dataArray[TableManager.Instance.NewGachaTable.dataArray.Length - 1]);
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.RingEnhance_Level).AsObservable().Subscribe(e => { UpdateByCurrentId(); }).AddTo(this);
    }

    private bool CheckHasRing()
    {
        if (ServerData.newGachaServerTable.TableDatas["Ring27"].hasItem.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("신물 1등급 반지가 필요합니다.");
            this.gameObject.SetActive(false);
            return false;
        }

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_NewGacha).Value - GameBalance.GraduateSoulRing >= GameBalance.GraduateSoulRingGetInterval)
        {
            PopupManager.Instance.ShowAlarmMessage("상점 - 소환 - 영혼 반지에서 아직 획득하지 않은 전설 1등급 반지 보상이 존재합니다.");
            this.gameObject.SetActive(false);
            return false;
        }

        return true;
    }

    private void UpdateByCurrentId()
    {
        currentAwakeLevel.SetText($"현재 강화도 : + {ServerData.statusTable.GetTableData(StatusTable.RingEnhance_Level).Value}강");

        currentIdx = Mathf.Max(currentIdx, 0);

        currentSelectedObejct.SetActive(currentIdx == PlayerStats.GetCurrentRelicUpgradeIdx());

        var tableData = TableManager.Instance.RelicUpgrade.dataArray[currentIdx];

        requireText.SetText($"강화 {tableData.Require} 필요");

        string description = string.Empty;

        float abilValue0 = PlayerStats.GetRelicUpgradeAbilValue(currentIdx);

        abilDescription.SetText($"영혼의숲 능력치 {abilValue0}배 상승");

        gradeText.SetText($"{currentIdx + 1}단계");
    }

    public void OnClickRightButton()
    {
        currentIdx++;
        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.RelicUpgrade.dataArray.Length - 1);
        UpdateByCurrentId();
    }

    public void OnClickLeftButton()
    {
        currentIdx--;
        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.RelicUpgrade.dataArray.Length - 1);
        UpdateByCurrentId();
    }

    public void OnClickButton()
    {
        float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value;

        if (currentMarble < GameBalance.GetSoulRingUpgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.NewGachaEnergy)}이 부족합니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value -= GameBalance.GetSoulRingUpgradePrice;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.statusTable.GetTableData(StatusTable.RingEnhance_Level).Value++;

        Param statusParam = new Param();
        statusParam.Add(StatusTable.RingEnhance_Level, ServerData.statusTable.GetTableData(StatusTable.RingEnhance_Level).Value);
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));


#if UNITY_EDITOR
        Debug.LogError("강화성공");
#endif

        ServerData.SendTransactionV2(transactions, successCallBack: () => { PopupManager.Instance.ShowAlarmMessage("강화 성공!"); });
    }

    //
    public void OnClickButton_All()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"모든 {CommonString.GetItemName(Item_Type.NewGachaEnergy)}으로 강화 할까요?", () =>
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupManager.Instance.ShowAlarmMessage("네트워크 연결상태가 좋지 않습니다.\n다음에 다시 시도해 주세요!");
                return;
            }

            int prefRelicUpgrade = PlayerStats.GetCurrentRelicUpgradeIdx();

            float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value;

            if (currentMarble < GameBalance.GetSoulRingUpgradePrice)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.NewGachaEnergy)}이 부족합니다.");
                return;
            }

            int upgradableNum = (int)(ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value / GameBalance.GetSoulRingUpgradePrice);

            ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value -= (GameBalance.GetSoulRingUpgradePrice * upgradableNum);

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.statusTable.GetTableData(StatusTable.RingEnhance_Level).Value += upgradableNum;

            Param statusParam = new Param();
            statusParam.Add(StatusTable.RingEnhance_Level, ServerData.statusTable.GetTableData(StatusTable.RingEnhance_Level).Value);
            transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

            LogManager.Instance.SendLogType("RelicUpgrade", "all", $"pref {ServerData.statusTable.GetTableData(StatusTable.RingEnhance_Level).Value - upgradableNum} +{upgradableNum}");

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                LogManager.Instance.SendLogType("RelicUpgrade", "complete", "complete");

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"+{upgradableNum} 강화 성공!", null);
                int currentRelicUpgrade = PlayerStats.GetCurrentRelicUpgradeIdx();
                if (prefRelicUpgrade < currentRelicUpgrade)
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "단계 상승!", null);
                }
            });
        }, null);
    }

    public TMP_InputField inputField;

    public void OnClickGetManyEnergyButton()
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

                if (ServerData.goodsTable.GetTableData(GoodsTable.SoulRingClear).Value < result)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SoulRingClear)}이 부족합니다!");
                    return;
                }

                int score = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.relicKillCount].Value;

                if (score == 0)
                {
                    PopupManager.Instance.ShowAlarmMessage("점수가 등록되지 않았습니다.");
                    return;
                }


                PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{score * result}개 획득 합니까?\n<color=red>({score} x {result} 획득 가능)</color>", () =>
                {
                    if (result < 1)
                    {
                        PopupManager.Instance.ShowAlarmMessage("올바른 개수가 아닙니다.");
                        return;
                    }

                    if (ServerData.goodsTable.GetTableData(GoodsTable.SoulRingClear).Value < result)
                    {
                        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SoulRingClear)}이 부족합니다!");
                        return;
                    }


                    ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += score * result;
                    ServerData.goodsTable.GetTableData(GoodsTable.SoulRingClear).Value -= result;

                    List<TransactionValue> transactions = new List<TransactionValue>();


                    Param goodsParam = new Param();
                    goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);
                    goodsParam.Add(GoodsTable.SoulRingClear, ServerData.goodsTable.GetTableData(GoodsTable.SoulRingClear).Value);

                    transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                    ServerData.SendTransaction(transactions, successCallBack: () => { PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{CommonString.GetItemName(Item_Type.NewGachaEnergy)} {score * result}개 획득!", null); });
                }, null);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("소탕 횟수를 입력해주세요.");
            }
        }
    }
}