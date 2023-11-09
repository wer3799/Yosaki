using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiBlackFoxCell : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI statusName;

    [SerializeField]
    private TextMeshProUGUI statusDescription;

    [SerializeField]
    private TextMeshProUGUI levelText;


    private BlackFoxAbilData blackFoxLocalData;

    private BlackFoxServerData blackFoxServerData;

    private bool subscribed = false;
    private bool IsMaxLevel()
    {
        return blackFoxServerData.level.Value >= blackFoxLocalData.Maxlevel;
    }

    private void Subscribe()
    {
        blackFoxServerData.level.AsObservable().Subscribe(level =>
        {
            levelText.SetText($"LV:{Utils.ConvertBigNum(level)}");

            StatusType abilType = (StatusType)blackFoxLocalData.Abiltype;
            PlayerStats.ResetBlackFoxHas();

            if (abilType.IsPercentStat())
            {
                var abilValue = PlayerStats.GetBlackFoxEffect(abilType);

                statusDescription.SetText($"{CommonString.GetStatusName(abilType)} {Utils.ConvertNum(abilValue * 100f,2)}%");

            }
            else
            {
                var abilValue = PlayerStats.GetBlackFoxEffect(abilType);

                statusDescription.SetText($"{CommonString.GetStatusName(abilType)} {Utils.ConvertNum(abilValue)}");
            }

        }).AddTo(this);

    }

    public void Initialize(BlackFoxAbilData blackFoxLocalData)
    {
        this.blackFoxLocalData = blackFoxLocalData;

        this.blackFoxServerData = ServerData.blackFoxServerTable.TableDatas[this.blackFoxLocalData.Stringid];

        statusName.SetText(this.blackFoxLocalData.Name);
        statusName.color = CommonUiContainer.Instance.itemGradeColor[blackFoxLocalData.Id];

        icon.sprite = CommonUiContainer.Instance.blackFoxIconList[this.blackFoxLocalData.Id];

        if (subscribed == false)
        {
            subscribed = true;
            Subscribe();
        }
    }
    public void OnClickUpgrade10Button()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }

        float currentNum = ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value;

        if (currentNum < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.BlackFoxGoods)}이 부족합니다!");
            return;
        }

        float upgradeableNum = blackFoxLocalData.Maxlevel - blackFoxServerData.level.Value;

        upgradeableNum = Mathf.Min(upgradeableNum, 10);

        upgradeableNum = Mathf.Min(upgradeableNum, currentNum);

        ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value -= upgradeableNum;

        blackFoxServerData.level.Value += upgradeableNum;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }

    public void OnClickUpgradeAllButton()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }

        float currentNum = ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value;

        if (currentNum < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.BlackFoxGoods)}이 부족합니다!");
            return;
        }

        float upgradeableNum = blackFoxLocalData.Maxlevel - blackFoxServerData.level.Value;

        upgradeableNum = Mathf.Min(upgradeableNum, currentNum);

        ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value -= upgradeableNum;

        blackFoxServerData.level.Value += upgradeableNum;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }
    public void OnClickLevelupButton()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }

        float currentNum = ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value;

        if (currentNum < 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.BlackFoxGoods)}이 부족합니다!");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value --;

        blackFoxServerData.level.Value++;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);

    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.BlackFoxGoods, ServerData.goodsTable.GetTableData(GoodsTable.BlackFoxGoods).Value);

        Param blackFoxParam = new Param();
        blackFoxParam.Add(blackFoxLocalData.Stringid, ServerData.blackFoxServerTable.TableDatas[blackFoxLocalData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(BlackFoxServerTable.tableName, BlackFoxServerTable.Indate, blackFoxParam));
        PlayerStats.ResetAbilDic();

        ServerData.SendTransactionV2(transactions);
    }
}
