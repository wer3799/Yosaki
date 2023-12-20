using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using GoogleMobileAds.Api;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using WebSocketSharp;

public class UiWeeklyBossBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI requireScoreText; 
    [SerializeField] private TextMeshProUGUI gradeText; 
    [SerializeField] private TextMeshProUGUI topScoreText;
    [SerializeField] private TextMeshProUGUI sweepCountText;

    [SerializeField] private WeeklyBossRewardCell prefab;
    [SerializeField] private Transform prefabParent;
    [SerializeField] private GameObject applyObject;

    [SerializeField] private UiRewardResultView sweepPopup;
    [SerializeField] private UiRewardResultView rewardPopup;
        
    private List<WeeklyBossRewardCell> cellContainer = new List<WeeklyBossRewardCell>();
    private List<RewardData> rewardDatas = new List<RewardData>();

    private WeeklyBossData[] tableData;
    
    private int currentIdx = -1;
    private int sweepCount = 1;
    private void OnEnable()
    {
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < GameBalance.WeeklyBossLevelLimit)
        {
            PopupManager.Instance.ShowAlarmMessage($"{Utils.ConvertNum(GameBalance.WeeklyBossLevelLimit)} 레벨 달성이 필요합니다.");
            this.gameObject.SetActive(false);
            return;
        }
    }

    private void Start()
    {
        Initialize();

        
        UpdateUi();
    }
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.WT).Value++;
        }
    }
#endif

    private void Initialize()
    {
        tableData = TableManager.Instance.WeeklyBoss.dataArray;

        currentIdx = Mathf.Max(PlayerStats.GetWeeklyBossGrade(), 0);

        if (ServerData.bossScoreTable.GetTableData(BossScoreTable.weeklyBossScore).Value.IsNullOrEmpty())
        {
            topScoreText.SetText($"피해량 없음");
        }
        else
        {
            topScoreText.SetText($"최고 피해량 : {Utils.ConvertNum(ServerData.bossScoreTable.TableDatas_Double[BossScoreTable.weeklyBossScore].Value* GameBalance.BossScoreConvertToOrigin)}");
        }

    }

    private void SetText()
    {
        requireScoreText.SetText($"{Utils.ConvertNum(tableData[currentIdx].Hp)}");
        gradeText.SetText($"{currentIdx+1}단계");
        applyObject.SetActive(currentIdx==PlayerStats.GetWeeklyBossGrade());
    }
    
    public void UpdateUi()
    {
        SetText();

        var currentData = tableData[currentIdx];

        string result = string.Empty;

        int maxGrade = 0;
        int minGrade = 0;

        int slotNum = 0;

        rewardDatas.Clear();
        
        for (int i = 0; i < currentData.Rewardtype.Length; i++)
        {
            if (currentData.Rewardtype[i] == -1)
            {
                break;
            }

            slotNum++;
        }

        while (cellContainer.Count < slotNum)
        {
            WeeklyBossRewardCell rewardCell = Instantiate<WeeklyBossRewardCell>(prefab, prefabParent);
            cellContainer.Add(rewardCell);
        }

        for (int i = 0; i < cellContainer.Count; i++)
        {
            RewardItem rewardItem = new RewardItem((Item_Type)currentData.Rewardtype[i], currentData.Rewardvalue[i]);
            rewardDatas.Add(new RewardData(rewardItem.ItemType,rewardItem.ItemValue));
            if (i < slotNum)
            {
                cellContainer[cellContainer.Count-i-1].gameObject.SetActive(true);
                cellContainer[cellContainer.Count-i-1].Initialize(rewardItem,currentData.Rewardgrade[i]);
            }
            else
            {
                cellContainer[cellContainer.Count-i-1].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        if (currentIdx <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("처음단계 입니다.");
        }

        currentIdx = Mathf.Max(currentIdx, 0);

        UpdateUi();
    }
    public void OnClickRightButton()
    {
        currentIdx++;

        if (currentIdx == tableData.Length)
        {
            PopupManager.Instance.ShowAlarmMessage("최고단계 입니다.");
        }

        currentIdx = Mathf.Min(currentIdx, tableData.Length - 1);
        
        UpdateUi();
    }
    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.WeeklyBoss);
        }, () => { });
    }

    public void InitializeSweepBoard()
    {
        currentIdx = Mathf.Max(PlayerStats.GetWeeklyBossGrade(),0);
        
        UpdateUi();

        sweepPopup.Initialize(rewardDatas,sweepCount);
        
        sweepCountText.SetText($"{sweepCount}");
    }
    public void OnClickLeftButtonFromSweepBoard()
    {
        sweepCount--;
        sweepCount = Mathf.Max(1, sweepCount);
        InitializeSweepBoard();
    }
    public void OnClickRightButtonFromSweepBoard()
    {
        int limitCount = 10;
        
        sweepCount++;
        if (sweepCount > limitCount)
        {
            PopupManager.Instance.ShowAlarmMessage($"최대 {limitCount}개까지 사용 가능합니다!");
        }
        sweepCount = Mathf.Min(limitCount, sweepCount);
        InitializeSweepBoard();
    }

    public void OnClickInstantClear()
    {
        if (PlayerStats.GetWeeklyBossGrade() < 0)
        {
            PopupManager.Instance.ShowAlarmMessage("피해량이 부족합니다!");
            return;
        }
        
        var goodsAmount = (int)ServerData.goodsTable.GetTableData(GoodsTable.WT).Value;

        if (sweepCount > goodsAmount)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.WT)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.WT).Value -= sweepCount;
        
        List<TransactionValue> transactions = new List<TransactionValue>();
            
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.WT, ServerData.goodsTable.GetTableData(GoodsTable.WT).Value);

        var rewardList = new List<RewardData>();
        
        var e = rewardDatas.GetEnumerator();
        while (e.MoveNext())
        {
            ServerData.goodsTable.AddLocalData(ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType), e.Current.amount * sweepCount);
            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType), ServerData.goodsTable.GetTableData(e.Current.itemType).Value);
            rewardList.Add(new RewardData(e.Current.itemType, e.Current.amount * sweepCount));
        }
        
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            
            if (rewardList.Count > 0)
            {
                rewardPopup.gameObject.SetActive(true);
                rewardPopup.Initialize(rewardList);
            }
                    
        });
    }
    
    
}
