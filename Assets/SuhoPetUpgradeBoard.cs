using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using UniRx;
using UnityEngine.Serialization;

public class SuhoPetUpgradeBoard : MonoBehaviour
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
    private UiAnimalView SuhoPetView;

    private int currentIdx = -1;
    [SerializeField] private GameObject transBeforeObject;
    [SerializeField] private GameObject transAfterObject;
    

    [SerializeField] private TextMeshProUGUI transDesc;
    private void Awake()
    {
        oneClickCost.SetText($"{Utils.ConvertNum(GameBalance.GetSuhoPetUpgradePrice)}");
    }

    private void OnEnable()
    {
        if (CheckHasRequire() == false)
        {
            return;
        }

        currentIdx = PlayerStats.GetCurrentSuhoUpgradeIdx();
    }

    private void Start()
    {
        Subscribe();
        
        int lastPetId = ServerData.suhoAnimalServerTable.GetLastPetId();

        var tableData = TableManager.Instance.suhoPetTable.dataArray[lastPetId];

        SuhoPetView.Initialize(tableData);
        
        transDesc.SetText($"(각성으로 효율 {GameBalance.suhoGraduateValue}배 증가)");
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.SuhoEnhance_Level).AsObservable().Subscribe(e => { UpdateByCurrentId(); }).AddTo(this);
        
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.suhoUpgradeGraduateIdx).AsObservable().Subscribe(e =>
        {
            transBeforeObject.SetActive(e < 1);
            transAfterObject.SetActive(e >= 1);
        }).AddTo(this);
    }

    private bool CheckHasRequire()
    {
        var data = TableManager.Instance.suhoPetTable.dataArray[GameBalance.SuhoPetUpgradeIdx];
        
        if (ServerData.suhoAnimalServerTable.TableDatas[data.Stringid].hasItem.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"수호 동물 {data.Gradedescription} {data.Name} 보유 필요");
            this.gameObject.SetActive(false);
            return false;
        }

        return true;
    }

    private void UpdateByCurrentId()
    {
        currentAwakeLevel.SetText($"현재 강화도 : + {ServerData.statusTable.GetTableData(StatusTable.SuhoEnhance_Level).Value}강");

        currentIdx = Mathf.Max(currentIdx, 0);

        currentSelectedObejct.SetActive(currentIdx == PlayerStats.GetCurrentSuhoUpgradeIdx());

        var tableData = TableManager.Instance.SuhoUpgrade.dataArray[currentIdx];

        requireText.SetText($"강화 {tableData.Require} 필요");

        string description = string.Empty;


        float abilValue0 = PlayerStats.GetSuhoUpgradeAbilValue(currentIdx);

        abilDescription.SetText(
            $"{CommonString.GetStatusName(StatusType.SuperCritical11DamPer)} {abilValue0 * 100} 증가");

        gradeText.SetText($"{currentIdx + 1}단계");
    }

    public void OnClickRightButton()
    {
        currentIdx++;
        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.SuhoUpgrade.dataArray.Length - 1);
        UpdateByCurrentId();
    }

    public void OnClickLeftButton()
    {
        currentIdx--;
        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.SuhoUpgrade.dataArray.Length - 1);
        UpdateByCurrentId();
    }

    public void OnClickButton()
    {
        float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value;

        if (currentMarble < GameBalance.GetSuhoPetUpgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SuhoPetFeed)}이 부족합니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value -= GameBalance.GetSuhoPetUpgradePrice;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SuhoPetFeed, ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value);
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.statusTable.GetTableData(StatusTable.SuhoEnhance_Level).Value++;

        Param statusParam = new Param();
        statusParam.Add(StatusTable.SuhoEnhance_Level, ServerData.statusTable.GetTableData(StatusTable.SuhoEnhance_Level).Value);
        transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));


#if UNITY_EDITOR
        Debug.LogError("강화성공");
#endif

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("강화 성공!");
            currentIdx = PlayerStats.GetCurrentSuhoUpgradeIdx();
            UpdateByCurrentId();
        });
    }

    //
    public void OnClickButton_All()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"모든 {CommonString.GetItemName(Item_Type.SuhoPetFeed)}으로 강화 할까요?", () =>
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupManager.Instance.ShowAlarmMessage("네트워크 연결상태가 좋지 않습니다.\n다음에 다시 시도해 주세요!");
                return;
            }

            int prefSuhoPetUpgrade = PlayerStats.GetCurrentSuhoUpgradeIdx();

            float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value;

            if (currentMarble < GameBalance.GetSuhoPetUpgradePrice)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SuhoPetFeed)}이 부족합니다.");
                return;
            }

            int upgradableNum = (int)(ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value / GameBalance.GetSuhoPetUpgradePrice);

            ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value -= (GameBalance.GetSuhoPetUpgradePrice * upgradableNum);

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.SuhoPetFeed, ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.statusTable.GetTableData(StatusTable.SuhoEnhance_Level).Value += upgradableNum;

            Param statusParam = new Param();
            statusParam.Add(StatusTable.SuhoEnhance_Level, ServerData.statusTable.GetTableData(StatusTable.SuhoEnhance_Level).Value);
            transactions.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

            //LogManager.Instance.SendLogType("SuhoPetUpgrade", "all", $"pref {ServerData.statusTable.GetTableData(StatusTable.SuhoEnhance_Level).Value - upgradableNum} +{upgradableNum}");

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                //LogManager.Instance.SendLogType("SuhoPetUpgrade", "complete", "complete");

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"+{upgradableNum} 강화 성공!", null);
                int currentSuhoPetUpgrade = PlayerStats.GetCurrentSuhoUpgradeIdx();
                if (prefSuhoPetUpgrade < currentSuhoPetUpgrade)
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "단계 상승!", null);
                    currentIdx = PlayerStats.GetCurrentSuhoUpgradeIdx();
                    UpdateByCurrentId();
                }
            });
        }, null);
    }

    public TMP_InputField instantClearNum;

    public void OnClickInstantClearButton()
    {
        int lastPetId = ServerData.suhoAnimalServerTable.GetLastPetId();

        //플레이 X
        if (lastPetId == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("수호동물을 보유하고 있어야 소탕하실 수 있습니다!");
            return;
        }


        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeedClear].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SuhoPetFeedClear)}이 없습니다.");
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
                    $"{CommonString.GetItemName(Item_Type.SuhoPetFeedClear)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }

        var instanClearGetNum = TableManager.Instance.suhoPetTable.dataArray[lastPetId].Sweepvalue * inputNum;

        string desc= "";
        if (PlayerStats.GetSuhoGainValue() > 0f)
        {
            desc +=
                $"{lastPetId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.SuhoPetFeed)} {instanClearGetNum}(+{Utils.ConvertNum(instanClearGetNum*PlayerStats.GetSuhoGainValue())})개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({lastPetId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.SuhoPetFeed)} {(int)TableManager.Instance.suhoPetTable.dataArray[lastPetId].Sweepvalue}(+{Utils.ConvertNum((int)TableManager.Instance.suhoPetTable.dataArray[lastPetId].Sweepvalue*PlayerStats.GetSuhoGainValue())})개 획득)</color>";
        }
        else
        {
            desc +=
                $"{lastPetId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.SuhoPetFeed)} {instanClearGetNum}개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({lastPetId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.SuhoPetFeed)} {(int)TableManager.Instance.suhoPetTable.dataArray[lastPetId].Sweepvalue}개 획득)</color>";
        }
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, desc, () =>
            {
                int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeedClear].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(
                        $"{CommonString.GetItemName(Item_Type.SuhoPetFeedClear)}이 없습니다.");

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
                            $"{CommonString.GetItemName(Item_Type.SuhoPetFeedClear)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
                ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeedClear].Value -= inputNum;
                ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeed].Value +=Mathf.Round(instanClearGetNum + (instanClearGetNum * PlayerStats.GetSuhoGainValue()));

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.SuhoPetFeed, ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeed].Value);
                goodsParam.Add(GoodsTable.SuhoPetFeedClear,
                    ServerData.goodsTable.TableDatas[GoodsTable.SuhoPetFeedClear].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                ServerData.SendTransactionV2(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"소탕 완료!\n{CommonString.GetItemName(Item_Type.SuhoPetFeed)} {Utils.ConvertNum(instanClearGetNum+(instanClearGetNum * PlayerStats.GetSuhoGainValue()))}개 획득!", null);
                    });
            }, null);
    }

    private string suhoKingId = "p47";
    public void OnClickTransButton()
    {
        if (ServerData.suhoAnimalServerTable.TableDatas[suhoKingId].hasItem.Value <1)
        {
            PopupManager.Instance.ShowAlarmMessage($"수호왕 획득 시 각성 가능!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                "수호 보주를 각성하려면 수호왕을 획득해야 합니다.\n" +
                $"각성 시 수호 보주 강화 수치가 1.5배 증가합니다.\n" +
                $"각성 하시겠습니까?", () =>
                {
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.suhoUpgradeGraduateIdx].Value = 1;
                    List<TransactionValue> transactions = new List<TransactionValue>();
                    
                    Param userInfo2Param = new Param();
                    userInfo2Param.Add(UserInfoTable_2.suhoUpgradeGraduateIdx, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.suhoUpgradeGraduateIdx].Value);
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName,UserInfoTable_2.Indate,userInfo2Param));
                    
                    ServerData.SendTransaction(transactions,successCallBack: () =>
                    {
                        UpdateByCurrentId();
                    });
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
              
                    
                }, null);
        }
    }
}