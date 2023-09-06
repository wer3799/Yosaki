using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class UiStageRelicCell : MonoBehaviour
{
    [SerializeField]
    private Image relicIcon;

    [SerializeField]
    private TextMeshProUGUI relicName;

    [SerializeField]
    private TextMeshProUGUI relicDescription;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI priceText;

    private StageRelicData relicLocalData;

    private RelicServerData relicServerData;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockText;

    [SerializeField]
    private GameObject lockMask_Goods;

    [SerializeField]
    private TextMeshProUGUI lockText_Goods;

    private bool subscribed = false;
    private bool IsMaxLevel()
    {
        return relicServerData.level.Value >= relicLocalData.Maxlevel;
    }

    private void UpdateDescription(float level)
    {
        if (levelText == null || priceText == null || relicDescription == null || relicLocalData == null) return;

        if (relicLocalData.Istotalskill)
        {
            level = ServerData.stageRelicServerTable.GetTotalStageRelicLevel();
        }

        levelText.SetText($"LV:{Utils.ConvertBigNum(level)}");

        if (IsMaxLevel() == false)
        {
            priceText.SetText(Utils.ConvertBigNum(relicLocalData.Upgradeprice));
        }
        else
        {
            priceText.SetText("MAX");
        }

        StatusType abilType = (StatusType)relicLocalData.Abiltype;

        if (abilType.IsPercentStat())
        {
            var abilValue = PlayerStats.GetStageRelicHasEffect(abilType);

            float totalLevel = ServerData.stageRelicServerTable.GetTotalStageRelicLevel();

            if (relicLocalData.Istotalskill == false)
            {
                relicDescription.SetText($"{CommonString.GetStatusName(abilType)} {Utils.ConvertBigNum(abilValue * 100f)}%");
            }
            else
            {
                if (relicLocalData.Parentidx==2)
                {
                    if (ServerData.goodsTable.GetTableData(relicLocalData.Requiregoods).Value >= relicLocalData.Requiregoodsvalue)
                    {
                        float superCritical6Value = relicLocalData.Abilvalue * totalLevel * 100f;

                        superCritical6Value *= PlayerStats.GetStageRelicUpgradeValue();

                        relicDescription.SetText($"{CommonString.GetStatusName(abilType)} {superCritical6Value}%");
                    }
                    else
                    {
                        relicDescription.SetText($"{CommonString.GetStatusName(abilType)} 0%");
                    }
                }

                else
                {
                    relicDescription.SetText($"{CommonString.GetStatusName(abilType)} {abilValue * 100f}%");
                }

            }
        }
        else
        {
            var abilValue = PlayerStats.GetStageRelicHasEffect(abilType);

            relicDescription.SetText($"{CommonString.GetStatusName(abilType)} {Utils.ConvertBigNum(abilValue)}");
        }
    }

    private void Subscribe()
    {
        relicServerData.level.AsObservable().Subscribe(level =>
        {

            UpdateDescription(level);

        }).AddTo(this);

        lockMask.SetActive(false);

        if (relicLocalData.Requirerelic != -1)
        {
            var requireServerData = ServerData.stageRelicServerTable.TableDatas[TableManager.Instance.StageRelic.dataArray[relicLocalData.Requirerelic].Stringid];

            lockText.color = CommonUiContainer.Instance.itemGradeColor[TableManager.Instance.StageRelic.dataArray[relicLocalData.Requirerelic].Grade + 1];

            requireServerData.level.AsObservable().Subscribe(requireLevel =>
            {
                lockMask.SetActive(requireLevel < relicLocalData.Requirelevel);

                lockText.SetText($"{TableManager.Instance.StageRelic.dataArray[relicLocalData.Requirerelic].Name} {relicLocalData.Requirelevel}레벨 필요");
            }).AddTo(this);
        }

        if (string.IsNullOrEmpty(relicLocalData.Requiregoods) == false)
        {
            Item_Type goodsType = Utils.EnumUtil<Item_Type>.Parse(relicLocalData.Requiregoods);
            lockText_Goods.SetText($"{CommonString.GetItemName(goodsType)} {Utils.ConvertBigNum(relicLocalData.Requiregoodsvalue)}개 이상일때 해금");

            ServerData.goodsTable.GetTableData(relicLocalData.Requiregoods).AsObservable().Subscribe(e =>
            {
                lockMask_Goods.gameObject.SetActive(e < relicLocalData.Requiregoodsvalue);
            }).AddTo(this);

        }
        else
        {
            lockMask_Goods.gameObject.SetActive(false);
        }

        if (relicLocalData.Istotalskill)
        {
            UiStageRelicBoard.Instance.whenRelicLevelUpButtonClicked.AsObservable().Subscribe(e =>
            {

                UpdateDescription(0f);

            }).AddTo(this);
        }
    }

    public void Initialize(StageRelicData relicLocalData)
    {
        this.relicLocalData = relicLocalData;

        this.relicServerData = ServerData.stageRelicServerTable.TableDatas[this.relicLocalData.Stringid];

        relicName.SetText(this.relicLocalData.Name);
        relicName.color = CommonUiContainer.Instance.itemGradeColor[relicLocalData.Grade + 1];

        if (CommonUiContainer.Instance.stageRelicIconList.Count != 0 &&
            this.relicLocalData.Id < CommonUiContainer.Instance.stageRelicIconList.Count)
        {
            relicIcon.sprite = CommonUiContainer.Instance.stageRelicIconList[this.relicLocalData.Id];
        }

        if (subscribed == false)
        {
            subscribed = true;
            Subscribe();
        }
    }
    public void OnClickUpgrade100Button()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }

        int currentRelicNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value;

        if (currentRelicNum < 1000 * GameBalance.StageRelicUpgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.StageRelic)}이 부족합니다!");
            return;
        }

        float upgradeableNum = relicLocalData.Maxlevel - relicServerData.level.Value;

        upgradeableNum = Mathf.Min(upgradeableNum, 1000);

        ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value -= upgradeableNum * GameBalance.StageRelicUpgradePrice;

        relicServerData.level.Value += upgradeableNum;

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

        float currentRelicNum = ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value;

        if (currentRelicNum < GameBalance.StageRelicUpgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.StageRelic)}이 부족합니다!");
            return;
        }

        float upgradeableMaxNum = relicLocalData.Maxlevel - relicServerData.level.Value;

        float upgradableMaxPrice = (float)upgradeableMaxNum * (float)GameBalance.StageRelicUpgradePrice;

        float diffPrice = currentRelicNum - upgradableMaxPrice;

        //유물 남을때(최대업가능)
        if (diffPrice >= 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value -= (upgradeableMaxNum * GameBalance.StageRelicUpgradePrice);

            relicServerData.level.Value += (int)upgradeableMaxNum;
        }
        else
        {
            int fullUpgradeNum = (int)(currentRelicNum / GameBalance.StageRelicUpgradePrice);

            ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value -= (fullUpgradeNum * GameBalance.StageRelicUpgradePrice);

            relicServerData.level.Value += (int)fullUpgradeNum;
        }


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

        int currentRelicNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value;

        if (currentRelicNum < GameBalance.StageRelicUpgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.StageRelic)}이 부족합니다!");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value -= GameBalance.StageRelicUpgradePrice;

        relicServerData.level.Value++;

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
        PlayerStats.ResetAbilDic();

        UpdateDescription(relicServerData.level.Value);

        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.StageRelic, ServerData.goodsTable.GetTableData(GoodsTable.StageRelic).Value);

        Param relicParam = new Param();
        relicParam.Add(relicLocalData.Stringid, ServerData.stageRelicServerTable.TableDatas[relicLocalData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(StageRelicServerTable.tableName, StageRelicServerTable.Indate, relicParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              //  LogManager.Instance.SendLogType("StageRelic", relicLocalData.Stringid, ServerData.stageRelicServerTable.TableDatas[relicLocalData.Stringid].level.Value.ToString());
          });

        UiStageRelicBoard.Instance.whenRelicLevelUpButtonClicked.Execute();

    }
}
