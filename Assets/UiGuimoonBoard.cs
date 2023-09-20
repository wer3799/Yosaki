using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class UiGuimoonBoard : SingletonMono<UiGuimoonBoard>
{
    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private UiGuimoonCell uiGuimoonCell;
    

    private new void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void OnEnable()
    {
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < GameBalance.GuimoonUnlockLevel)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage($"{GameBalance.GuimoonUnlockLevel}레벨 이후에 사용 가능합니다!");
        }
    }

    private void Initialize()
    {
        var tableDatas = TableManager.Instance.GuimoonTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var cell = Instantiate<UiGuimoonCell>(uiGuimoonCell, cellParents);

            cell.Initialize(tableDatas[i]);
        }

    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value += 10000000;
        }
    }

#endif

    public void OnClickAllResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "모든 능력치를 초기화 합니까?", () =>
        {
            float refundCount = 0;

            var tableDatas = TableManager.Instance.GuimoonTable.dataArray;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param guimoonParam = new Param();

            for (int i = 0; i < tableDatas.Length; i++)
            {
                refundCount += ServerData.guimoonServerTable.TableDatas[tableDatas[i].Stringid].level1.Value * tableDatas[i].Upgradeprice1;
                ServerData.guimoonServerTable.TableDatas[tableDatas[i].Stringid].level1.Value = 0;

                guimoonParam.Add(tableDatas[i].Stringid, ServerData.guimoonServerTable.TableDatas[tableDatas[i].Stringid].ConvertToString());
            }

            if (refundCount == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
                return;
            }

            transactions.Add(TransactionValue.SetUpdate(GuimoonServerTable.tableName, GuimoonServerTable.Indate, guimoonParam));


            ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value += refundCount;
            

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.GuimoonRelic, ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            
            PlayerStats.ResetSuperCritical11CalculatedValue();

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
            });

        }, () => { });
    }

    public void RenewalAbil()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"현재 스테이지에 맞게 {CommonString.GetItemName(Item_Type.GuimoonRelic)}을 갱신 하시겠습니까?\n<size=30>(1챕터(100단계)마다 소탕권 1개당 귀문석 1개씩 더 획득할 수 있습니다.)", () =>
        {

            var tableDatas = TableManager.Instance.GuimoonTable.dataArray;

            float prefTotal = ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value;

            for (int i = 0; i < tableDatas.Length; i++)
            {
                prefTotal += ServerData.guimoonServerTable.TableDatas[tableDatas[i].Stringid].level1.Value * tableDatas[i].Upgradeprice1;
            }
            
            var currentScore = GameManager.Instance.CurrentStageData.Guimoonpoint;

            float newTotal = currentScore * (float)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.usedGuimoonRelicTicket).Value;

            int interval = (int)(newTotal - prefTotal);

            if (interval <= 0f)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "갱신할 데이터가 없습니다.", null);
            }
            else
            {
                ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value += interval;
                ServerData.goodsTable.UpData(GoodsTable.GuimoonRelic, false);
            }

        }, () => { });
    }
    private void OnDisable()
    {
        PlayerStats.ResetAbilDic();
    }
}
