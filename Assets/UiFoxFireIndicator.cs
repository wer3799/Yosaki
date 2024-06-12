using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using BackEnd;
using UnityEditor;

public class UiFoxFireIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private TextMeshProUGUI currentLevel;

    [SerializeField]
    private TextMeshProUGUI currentLevelDesc;

    [SerializeField]
    private TextMeshProUGUI nextLevelDesc;

    [SerializeField]
    private TextMeshProUGUI currentAbilAmount;

    [SerializeField]
    private TextMeshProUGUI growthStoneGainText;


    [SerializeField]
    private List<Image> marbleCircles;

    [SerializeField] private List<Color> _colors;

    [SerializeField]
    private TMP_InputField instantClearNum;
    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.foxFireIdx).AsObservable().Subscribe(e => { UpdateUi(); }).AddTo(this);
    }

    private void Initialize()
    {
        UpdateUi();
    }

    public void UpdateUi()
    {
        var fireIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxFireIdx).Value;

        var tableData = TableManager.Instance.FoxFire.dataArray;

        string currentDesc = string.Empty;

        string nextDesc = string.Empty;

        if (fireIdx < 0)
        {
            currentLevel.SetText($"0\n단\n계");
            currentLevelDesc.SetText($"없음");
        }
        else
        {
            currentDesc +=
                $"{CommonString.GetStatusName(StatusType.SuperCritical14DamPer)} {tableData[fireIdx].Abil_Value * 100} 증가";

            // if (tableData[fireIdx].Reward_Value > 1)
            // {
            //     currentDesc += $"및 {CommonString.GetItemName((Item_Type)tableData[fireIdx].Reward_Type)}";
            // }


            //growthStoneGainText.SetText($"{Utils.ConvertBigNum(tableData[fireIdx].Reward_Value)}개 획득!");

            currentLevel.SetText($"{tableData[fireIdx].Level}\n단\n계");

            currentLevelDesc.SetText(currentDesc);
        }

        if (tableData.Length > fireIdx + 1)
        {
            nextDesc += $"{CommonString.GetStatusName(StatusType.SuperCritical14DamPer)} {tableData[fireIdx + 1].Abil_Value * 100} 증가";

            // if (tableData[fireIdx + 1].Reward_Value > 1)
            // {
            //     nextDesc += $" 및 {CommonString.GetItemName((Item_Type)tableData[fireIdx + 1].Reward_Type)}" + ";
            // }
            
            growthStoneGainText.SetText($"{Utils.ConvertBigNumForRewardCell(tableData[fireIdx + 1].Reward_Value)}개 획득!");
            
            nextLevelDesc.SetText(nextDesc);
            
            priceText.SetText($"{tableData[fireIdx + 1].Conditoin_Value}");
        }
        else
        {
            nextLevelDesc.SetText("최종단계 달성!");
            priceText.SetText("최종단계 달성!");
        }

        currentAbilAmount.SetText(
            $"{CommonString.GetStatusName(StatusType.SuperCritical14DamPer)} {Utils.ConvertNum(PlayerStats.GetFoxFireEffect(StatusType.SuperCritical14DamPer) * 100)} 증가");

        //그림
        if (fireIdx < 0)
        {
            marbleCircles[0].color = _colors[0];
            marbleCircles[1].color = _colors[0];
            marbleCircles[2].color = _colors[0];
            marbleCircles[3].color = _colors[0];
            return;
        }

        var type = tableData[fireIdx].Type;
        switch (type)
        {
            case 1:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[0];
                marbleCircles[2].color = _colors[0];
                marbleCircles[3].color = _colors[0];
                break;
            case 2:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[1];
                marbleCircles[2].color = _colors[0];
                marbleCircles[3].color = _colors[0];
                break;
            case 3:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[1];
                marbleCircles[2].color = _colors[1];
                marbleCircles[3].color = _colors[0];
                break;
            case 4:
                marbleCircles[0].color = _colors[1];
                marbleCircles[1].color = _colors[1];
                marbleCircles[2].color = _colors[1];
                marbleCircles[3].color = _colors[1];
                break;
            case 5:
                marbleCircles[0].color = _colors[0];
                marbleCircles[1].color = _colors[0];
                marbleCircles[2].color = _colors[0];
                marbleCircles[3].color = _colors[0];
                break;
        }
    }

    public void OnClickUpgradeButton()
    {
        var fireIdx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxFireIdx).Value;

        var tableData = TableManager.Instance.FoxFire.dataArray;

        if (tableData.Length <= fireIdx + 1)
        {
            PopupManager.Instance.ShowAlarmMessage("업데이트 예정입니다!");
            return;
        }

        var require = tableData[fireIdx + 1].Conditoin_Value;

        //조건
        if (require > ServerData.goodsTable.GetTableData(GoodsTable.FoxRelic).Value)
        {
            PopupManager.Instance.ShowAlarmMessage( $"{CommonString.GetItemName(Item_Type.FoxRelic)}가 부족합니다.");
            return;
        }


        ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData[fireIdx + 1].Reward_Type)).Value += tableData[fireIdx + 1].Reward_Value;
        
        ServerData.goodsTable.GetTableData(GoodsTable.FoxRelic).Value -= require;

        ServerData.userInfoTable.GetTableData(UserInfoTable.foxFireIdx).Value++;

        if (tableData[fireIdx + 1].Level != tableData[fireIdx].Level)
        {
            PopupManager.Instance.ShowAlarmMessage($"여우불 단계 상승!!");
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage($"여우불 강화 성공!!");
        }

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private WaitForSeconds syncDelay = new WaitForSeconds(0.2f);

    private Coroutine syncRoutine;

    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        Debug.LogError($"@@@@@@@@@@@@@@@FoxFire SyncComplete@@@@@@@@@@@@@@");

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param userInfoParam = new Param();
        
        userInfoParam.Add(UserInfoTable.foxFireIdx, ServerData.userInfoTable.GetTableData(UserInfoTable.foxFireIdx).Value);
        
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        Param goodsParam = new Param();
        
        goodsParam.Add(GoodsTable.FoxRelic, ServerData.goodsTable.GetTableData(GoodsTable.FoxRelic).Value);
        
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
        
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactionList, successCallBack: () => { });
    }

    public void OnClickInstantClearButton()
    {
        int currentClearStageId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.foxTowerIdx].Value - 1;

        if (currentClearStageId < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("여우전을 클리어 해야 합니다.");
            return;
        }

        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.FoxRelicClearTicket].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.FoxRelicClearTicket)}이 없습니다.");
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
                PopupManager.Instance.ShowAlarmMessage("소탕권은 200개 미만으로 사용가능합니다!");
                return;
            }
            else if (remainItemNum < inputNum)
            {
                PopupManager.Instance.ShowAlarmMessage(
                    $"{CommonString.GetItemName(Item_Type.FoxRelicClearTicket)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }


        int instanClearGetNum = (int)TableManager.Instance.FoxTowerTable.dataArray[currentClearStageId].Sweepvalue *
                                inputNum;

        string desc = "";
        if (PlayerStats.GetFoxRelicGainValue() > 0f)
        {
            desc +=
                $"{currentClearStageId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.FoxRelic)} {instanClearGetNum}(+{Utils.ConvertNum(instanClearGetNum * PlayerStats.GetFoxRelicGainValue())})개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({currentClearStageId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.FoxRelic)} {(int)TableManager.Instance.FoxTowerTable.dataArray[currentClearStageId].Sweepvalue}(+{Utils.ConvertNum((int)TableManager.Instance.FoxTowerTable.dataArray[currentClearStageId].Sweepvalue * PlayerStats.GetFoxRelicGainValue())})개 획득)</color>";
        }
        else
        {
            desc +=
                $"{currentClearStageId + 1}단계를 {inputNum}번 소탕하여\n{CommonString.GetItemName(Item_Type.FoxRelic)} {instanClearGetNum}개를 획득 하시겠습니까?\n" +
                $"<color=yellow>({currentClearStageId + 1}단계 소탕 1회당 {CommonString.GetItemName(Item_Type.FoxRelic)} {(int)TableManager.Instance.FoxTowerTable.dataArray[currentClearStageId].Sweepvalue}개 획득)</color>";
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
            desc,
            () =>
            {
                int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.FoxRelicClearTicket].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage(
                        $"{CommonString.GetItemName(Item_Type.FoxRelicClearTicket)}이 없습니다.");

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
                            $"{CommonString.GetItemName(Item_Type.FoxRelicClearTicket)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
                ServerData.goodsTable.TableDatas[GoodsTable.FoxRelicClearTicket].Value -= inputNum;
                ServerData.goodsTable.TableDatas[GoodsTable.FoxRelic].Value +=
                    Mathf.Round(instanClearGetNum + (instanClearGetNum * PlayerStats.GetFoxRelicGainValue()));

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.FoxRelicClearTicket,
                    ServerData.goodsTable.TableDatas[GoodsTable.FoxRelicClearTicket].Value);
                goodsParam.Add(GoodsTable.FoxRelic, ServerData.goodsTable.TableDatas[GoodsTable.FoxRelic].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                ServerData.SendTransactionV2(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"소탕 완료!\n{CommonString.GetItemName(Item_Type.FoxRelic)} {Utils.ConvertNum(instanClearGetNum + (instanClearGetNum * PlayerStats.GetFoxRelicGainValue()))}개 획득!",
                            null);
                    });
            }, null);
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.FoxRelic).Value += 10000;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.foxFireIdx).Value = -1;
        }
    }

#endif
}