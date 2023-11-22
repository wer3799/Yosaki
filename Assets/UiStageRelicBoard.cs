using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiStageRelicBoard : SingletonMono<UiStageRelicBoard>
{
    [SerializeField]
    private List<Transform> cellParents;

    [SerializeField]
    private List<UiStageRelicCell> uiRelicCell;

    [SerializeField]
    private UiStageRelicCell uiRelicCell_GoodsRelic;

    public ReactiveCommand whenRelicLevelUpButtonClicked = new ReactiveCommand();

    //[SerializeField]
    //private TextMeshProUGUI bestScoreText;

    private new void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void OnEnable()
    {
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < GameBalance.StageRelicUnlockLevel)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage($"{GameBalance.StageRelicUnlockLevel}레벨 이상일 때 개방됩니다!");
        }
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.StageRelic.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var cell = Instantiate<UiStageRelicCell>(uiRelicCell[tableDatas[i].Parentidx], cellParents[tableDatas[i].Parentidx]);

            cell.Initialize(tableDatas[i]);
        }

        //bestScoreText.SetText($"최고점수:{(int)ServerData.userInfoTable.TableDatas[UserInfoTable.relicKillCount].Value}");
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value += 10000000;
        }
    }

#endif

    public void OnClickAllResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "모든 능력치를 초기화 합니까?", () =>
        {
            float refundCount = 0;

            var tableDatas = TableManager.Instance.StageRelic.dataArray;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param relicParam = new Param();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                refundCount += ServerData.stageRelicServerTable.TableDatas[tableDatas[i].Stringid].level.Value;
                ServerData.stageRelicServerTable.TableDatas[tableDatas[i].Stringid].level.Value = 0;

                relicParam.Add(tableDatas[i].Stringid, ServerData.stageRelicServerTable.TableDatas[tableDatas[i].Stringid].ConvertToString());
            }

            if (refundCount == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
                return;
            }

            transactions.Add(TransactionValue.SetUpdate(StageRelicServerTable.tableName, StageRelicServerTable.Indate, relicParam));


            ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value += refundCount;

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.StageRelic, ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
                //  LogManager.Instance.SendLogType("StageRelic", "초기화", $"{refundCount}개");
            });

        }, () => { });
    }

    private void OnDisable()
    {
        PlayerStats.ResetAbilDic();
    }
}
