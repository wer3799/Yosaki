using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class UiYoguiSogulEnterPopup : MonoBehaviour
{
    [SerializeField]
    private UiYoguiSogulRewardCell cellPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI lastClearStageDesc;

    private List<UiYoguiSogulRewardCell> cellLists = new List<UiYoguiSogulRewardCell>();

    [SerializeField]
    private TextMeshProUGUI passiveDescription;

     [SerializeField] private GameObject TransBeforeObject;
     [SerializeField] private GameObject TransAfterObject;
    
    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateBackGui)
            .AsObservable()
            .Subscribe(e =>
            {
                TransBeforeObject.SetActive(e < 1);
                TransAfterObject.SetActive(e > 0);
            })
            .AddTo(this);
    }
    private void Initialize()
    {
        var tableDatas = TableManager.Instance.YoguisogulTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Rewardtype == -1) continue;

            var cell = Instantiate<UiYoguiSogulRewardCell>(cellPrefab, cellParent);

            cell.Initialize(tableDatas[i]);

            cellLists.Add(cell);
        }

        lastClearStageDesc.SetText($"최고 단계 : {(int)(ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value)}");

        passiveDescription.SetText($"{PlayerStats.sogulGab}단계당 {PlayerStats.sogulValuePerGab * 100f}% 상승!");
    }

    public void OnClickEnterButton()
    {
        GameManager.Instance.LoadContents(GameManager.ContentsType.YoguiSoGul);
    }

    public void OnClickAllReceiveButton()
    {
        GuideMissionManager.UpdateGuideMissionClear(GuideMissionKey.ClearBackgui);

        int lastClearStageId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value;

        var tableDatas = TableManager.Instance.YoguisogulTable.dataArray;

        var GetYoguiSoguilRewardedList = ServerData.etcServerTable.GetYoguiSoguilRewardedList();

        int rewardReceiveCount = 0;

        string addStringValue = string.Empty;


        for (int i = 0; i < tableDatas.Length; i++)
        {
            if (tableDatas[i].Rewardtype == -1) continue;

            if (lastClearStageId < tableDatas[i].Stage) break;

            if (GetYoguiSoguilRewardedList.Contains(tableDatas[i].Stage) == true) continue;

            ServerData.AddLocalValue((Item_Type)tableDatas[i].Rewardtype, tableDatas[i].Rewardvalue);

            addStringValue += $"{BossServerTable.rewardSplit}{tableDatas[i].Stage}";

            rewardReceiveCount++;
        }


        if (rewardReceiveCount > 0)
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value += addStringValue;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            rewardParam.Add(EtcServerTable.yoguiSogulReward, ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

            Param goodsParam = new Param();

            goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
            goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                // LogManager.Instance.SendLogType("Son", "all", "");
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상이 없습니다.");
        }
    }
    public void OnClickTransButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value < GameBalance.BackguiGraduateScore)
        {
            PopupManager.Instance.ShowAlarmMessage($"백귀 야행을 각성하려면 최고 등급 {GameBalance.BackguiGraduateScore} 이상이어야 합니다!");
        }
        else
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                $"백귀 야행 각성시 최고 등급이 {GameBalance.BackguiFixedScore}로 고정 됩니다. \n" +
                $"각성 후 매일 접속 시  {CommonString.GetItemName(Item_Type.RelicTicket)}가 {GameBalance.BackguiGraduateGainSoulKey}개 지급됩니다.\n" +
                "각성 하시겠습니까??" +
                "\n\n<color=red><size=35>*각성전에 보상을 획득해 주세요.</color></size>", () =>
                {
                    List<TransactionValue> transactions = new List<TransactionValue>();
                    Param userinfoParam = new Param();
                    Param userinfo2Param = new Param();
                    
                    ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value = GameBalance.BackguiFixedScore;
                    userinfoParam.Add(UserInfoTable.yoguiSogulLastClear, ServerData.userInfoTable.TableDatas[UserInfoTable.yoguiSogulLastClear].Value);
                    
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.graduateBackGui].Value = 1;
                    userinfo2Param.Add(UserInfoTable_2.graduateBackGui, ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.graduateBackGui].Value);
                    
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName,UserInfoTable.Indate,userinfoParam));
                    transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName,UserInfoTable_2.Indate,userinfo2Param));
                    

                    
                    ServerData.SendTransaction(transactions,successCallBack: () =>
                    {
                        
                    });
                    
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "각성 완료!!", null);
              
                }, null);
        }
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.etcServerTable.TableDatas[EtcServerTable.yoguiSogulReward].Value = string.Empty;
        }
    }
#endif
}
