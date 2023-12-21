using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class UiBattleContestSweepBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gradeText;
    [SerializeField]
    private TextMeshProUGUI rankRangeText;
    [SerializeField]
    private TextMeshProUGUI currentGradeText;

    [SerializeField]
    private TMP_InputField instantClearNum;
    [SerializeField] private List<UiRewardView> prefabs = new List<UiRewardView>();


    private int currentRank = 10000;
    private int currentIdx = 4;

    private void Start()
    {
        RankManager.Instance.RequestMyStageRank();
        
        UpdateUi();

        Subscribe();


        isInitialize = true;
    }

    private bool isInitialize = false;
    private void OnEnable()
    {
        if (isInitialize == false)
        {
            return;
        }
        currentIdx = GetCurrentIdx(currentRank);

        UpdateUi();
        
    }

    private void Subscribe()
    {
        RankManager.Instance.WhenMyStageRankLoadComplete.AsObservable().Subscribe(e =>
        {
            if (e != null)
            {   
                currentGradeText.SetText($"현재 순위 : {e.Rank}");
                currentRank = e.Rank;
            }
            else
            {
                currentGradeText.SetText($"서버가 불안정합니다.");
                currentRank = 10000;
            }
            
            currentIdx = GetCurrentIdx(currentRank);

            UpdateUi();
        }).AddTo(this);
    }
    public int GetCurrentIdx(int rankIdx)
    {
        var tableData = TableManager.Instance.BattleContestTable.dataArray;
        var data = new BattleContestTableData(){Id = 4};

        foreach (var t in tableData)
        {
            if (t.Maxrank > rankIdx)
            {
                data = t;
                break;
            }
        }

        return data.Id;
    }

    private void UpdateUi()
    {
        UpdateReward(currentIdx);

        UpdateText(currentIdx);
    }

    private void UpdateReward(int idx)
    {
        var data = TableManager.Instance.BattleContestTable.dataArray[idx];
        
        for (int i = 0; i < data.Winvalue.Length; i++)
        {
            var win = new RewardData((Item_Type)data.Rewardtype[i], data.Winvalue[i]);
            prefabs[i].Initialize(win);
        }

    }
    private void UpdateText(int idx)
    {
        var data = TableManager.Instance.BattleContestTable.dataArray[idx];
        
        gradeText.SetText($"{data.Name}");
        rankRangeText.SetText($"({data.Maxrank}~{data.Minrank}위)");

    }


    public void OnClickLeftButton()
    {
        if (currentIdx == TableManager.Instance.BattleContestTable.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");        
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.BattleContestTable.dataArray.Length - 1);

        UpdateUi();
    }

    public void OnClickRightButton()
    {
        currentIdx--;

        if (currentIdx == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("마지막 단계입니다!");
        }
        
        //전설 제외해야해서 min = 1
        currentIdx = Mathf.Clamp(currentIdx, 1, TableManager.Instance.BattleContestTable.dataArray.Length - 1);

        UpdateUi();
    }
        
    public void OnClickInstantClearButton()
    {
        var tableData = TableManager.Instance.BattleContestTable.dataArray[currentIdx];

        if (tableData.Maxrank < currentRank)
        {
            PopupManager.Instance.ShowAlarmMessage("해당 난이도에서는 소탕 할 수 없습니다.");
            return;
        }

        int remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.BattleClear].Value;

        if (remainItemNum <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.BattleClear)}이 없습니다.");
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
                    $"{CommonString.GetItemName(Item_Type.BattleClear)}이 부족합니다!");
                return;
            }
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
            return;
        }
        List<RewardData> rewardData = new List<RewardData>();
        for (int i = 0; i < tableData.Winvalue.Length; i++)
        {
            var win = new RewardData((Item_Type)tableData.Rewardtype[i], tableData.Winvalue[i]);
            rewardData.Add(win);
        }
        
        string desc = "";
            desc +=
                $"{tableData.Name} 난이도를 {inputNum}번 소탕하여\n" +
                $"{CommonString.GetItemName(rewardData[0].itemType)} {Utils.ConvertNum(rewardData[0].amount*inputNum)}점," +
                $"{CommonString.GetItemName(rewardData[1].itemType)} {Utils.ConvertNum(rewardData[1].amount*inputNum)}개를 획득 하시겠습니까?";
        
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
            desc,
            () =>
            {
                var remainItemNum = (int)ServerData.goodsTable.TableDatas[GoodsTable.BattleClear].Value;

                if (remainItemNum <= 0)
                {
                    PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.BattleClear)}이 없습니다.");

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
                            $"{CommonString.GetItemName(Item_Type.BattleClear)}이 부족합니다!");
                        return;
                    }
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage("숫자를 입력해 주세요!");
                    return;
                }

                //실제소탕
//
                string winString = ServerData.etcServerTable.TableDatas[EtcServerTable.battleWinScore].Value;
        
                var scoreList = winString.Split(BossServerTable.rewardSplit).Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();

                scoreList[tableData.Id]+=inputNum;

                string newString = "";
                for (int i = 0; i < scoreList.Count; i++)
                {
                    newString += $"{BossServerTable.rewardSplit}{scoreList[i]}";
                }

                ServerData.etcServerTable.TableDatas[EtcServerTable.battleWinScore].Value = newString;
//
    
                var goods0 = ServerData.goodsTable.ItemTypeToServerString(rewardData[0].itemType);
                var goods1 = ServerData.goodsTable.ItemTypeToServerString(rewardData[1].itemType);
                
                ServerData.goodsTable.TableDatas[GoodsTable.BattleClear].Value -= inputNum;
                ServerData.goodsTable.TableDatas[goods0].Value += Mathf.Round(rewardData[0].amount*inputNum);
                ServerData.goodsTable.TableDatas[goods1].Value += Mathf.Round(rewardData[1].amount*inputNum);

                List<TransactionValue> transactions = new List<TransactionValue>();
                
                Param etcParam = new Param();
        
                etcParam.Add(EtcServerTable.battleWinScore,ServerData.etcServerTable.TableDatas[EtcServerTable.battleWinScore].Value);
                
                transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.BattleClear, ServerData.goodsTable.TableDatas[GoodsTable.BattleClear].Value);
                goodsParam.Add(goods0, ServerData.goodsTable.TableDatas[goods0].Value);
                goodsParam.Add(goods1, ServerData.goodsTable.TableDatas[goods1].Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

                ServerData.SendTransactionV2(transactions,
                    successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,
                            $"소탕 완료!\n"+
                            $"{CommonString.GetItemName(rewardData[0].itemType)} {Utils.ConvertNum(rewardData[0].amount*inputNum)}점," +
                            $"{CommonString.GetItemName(rewardData[1].itemType)} {Utils.ConvertNum(rewardData[1].amount*inputNum)}개 획득!"
                            , null);

                        //남은재화(소탕권) / 사용한재화(소탕권) / 획득한 재화갯수
                        LogManager.Instance.SendLogType("BattleContest", "Clear", $"{ServerData.goodsTable.TableDatas[GoodsTable.BattleClear].Value},{inputNum},{rewardData[0].amount*inputNum},{rewardData[1].amount*inputNum}");
                    });
            }, null);
    }

}
