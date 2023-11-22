using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UiBattleContestMatchingBoard : MonoBehaviour
{
    [SerializeField] private UiRewardView prefab;
    
    [SerializeField] private Transform winParent;
    [SerializeField] private Transform loseParent;

    [SerializeField] private List<TextMeshProUGUI> difficultyText;
    [SerializeField] private List<TextMeshProUGUI> winScore;
    [SerializeField] private List<TextMeshProUGUI> rankRange;

    private List<UiRewardView> winPrefabContainer = new List<UiRewardView>();
    private List<UiRewardView> losePrefabContainer =new List<UiRewardView>();
    
    public enum Difficulty
    {
        None=-1,
        VeryHard,
        Hard,
        Normal,
        Easy,
        VeryEasy,
    }

    private Difficulty _difficulty = Difficulty.VeryHard;
    
    
    void Start()
    {
        UpdateWinText();
        UpdateReward();
        UpdateRankRangeText();
    }

    private void UpdateWinText()
    {
        var splitData = ServerData.etcServerTable.TableDatas[EtcServerTable.battleWinScore].Value.Split(BossServerTable.rewardSplit);
            
        var scoreList = splitData.Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();
        
        
        for (int i = 0; i < scoreList.Count; i++)
        {
            winScore[i].SetText($"({scoreList[i]}승)");
        }
        
    }
    private void UpdateRankRangeText()
    {
        var tableData = TableManager.Instance.BattleContestTable.dataArray;
        
        
        for (int i = 0; i < rankRange.Count; i++)
        {
            difficultyText[i].SetText($"{tableData[i].Name}");
            rankRange[i].SetText($"({tableData[i].Maxrank}~{tableData[i].Minrank}위)");
        }
        
    }

    private void UpdateReward()
    {
        var idx = (int)_difficulty;
        
        var data = TableManager.Instance.BattleContestTable.dataArray[idx];

        List<UiRewardView.RewardData> winData = new List<UiRewardView.RewardData>();
        List<UiRewardView.RewardData> loseData = new List<UiRewardView.RewardData>();
    
        for (int i = 0; i < data.Winvalue.Length; i++)
        {
            var win = new UiRewardView.RewardData((Item_Type)data.Rewardtype[i],data.Winvalue[i]);
            var lose = new UiRewardView.RewardData((Item_Type)data.Rewardtype[i],data.Losevalue[i]);
            winData.Add(win);
            loseData.Add(lose);
        }

        var winInterval = winData.Count - winPrefabContainer.Count;

        for (int i = 0; i < winInterval; i++)
        {
            var winPrefab = Instantiate<UiRewardView>(prefab, winParent);
            winPrefabContainer.Add(winPrefab);
        }

        for (int i = 0; i < winPrefabContainer.Count; i++)
        {
            if (i < winData.Count)
            {
                winPrefabContainer[i].gameObject.SetActive(true);
                winPrefabContainer[i].Initialize(winData[i]);
            }
            else
            {
                winPrefabContainer[i].gameObject.SetActive(false);
            }
        }
        
        var loseInterval = loseData.Count - losePrefabContainer.Count;

        for (int i = 0; i < loseInterval; i++)
        {
            var losePrefab = Instantiate<UiRewardView>(prefab, loseParent);
            losePrefabContainer.Add(losePrefab);
        }

        for (int i = 0; i < losePrefabContainer.Count; i++)
        {
            if (i < loseData.Count)
            {
                losePrefabContainer[i].gameObject.SetActive(true);
                losePrefabContainer[i].Initialize(loseData[i]);
            }
            else
            {
                losePrefabContainer[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickDifficulty(int idx)
    {
        _difficulty = (Difficulty)idx;

        UpdateReward();

    }

    private bool isEnter = false;
    public void OnClickEnterButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.BattleClear).Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("입장권이 부족합니다.");
            return;
        }



        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?\n<color=red>입장시 입장권이 즉시 소모됩니다</color>", () =>
        {
            if (isEnter == true)
            {
                return;
            }
            isEnter = true;
                
            var data = TableManager.Instance.BattleContestTable.dataArray[GameManager.Instance.bossId];
            //패배보상 선제획득
            List<UiRewardView.RewardData> loseData = new List<UiRewardView.RewardData>();
            for (int i = 0; i < data.Losevalue.Length; i++)
            {
                var lose = new UiRewardView.RewardData((Item_Type)data.Rewardtype[i],data.Losevalue[i]);
                loseData.Add(lose);
            }
            Param goodsParam = new Param();
            for (int i = 0; i < loseData.Count; i++)
            {
                ServerData.goodsTable.GetTableData((Item_Type)loseData[i].itemType).Value += loseData[i].amount;
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(loseData[i].itemType),
                    ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString(loseData[i].itemType))
                        .Value);
            }
            
            //소모
            ServerData.goodsTable.GetTableData(GoodsTable.BattleClear).Value--;
            goodsParam.Add(GoodsTable.BattleClear, ServerData.goodsTable.GetTableData(GoodsTable.BattleClear).Value);
            
            List<TransactionValue> transactionList = new List<TransactionValue>();

            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransactionV2(transactionList, successCallBack: () =>
            {
                GameManager.Instance.SetBossId((int)_difficulty);
                GameManager.Instance.LoadContents(GameManager.ContentsType.BattleContest);
                isEnter = false;
            });
                
            
            

        }, () => { });
    }
    
}
