using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class UiGuimoonCell : MonoBehaviour
{
    [FormerlySerializedAs("relicIcon")] [SerializeField]
    private Image relicIcon1;
    [SerializeField]
    private Image relicIcon2;

    [FormerlySerializedAs("relicName")] [SerializeField]
    private TextMeshProUGUI relicName1; //하급정리장식
    [SerializeField]
    private TextMeshProUGUI relicName2; //하급정리장식

    [FormerlySerializedAs("relicDescription")] [SerializeField]
    private TextMeshProUGUI relicDescription1;// 공격력증가
    [SerializeField]
    private TextMeshProUGUI relicDescription2;

    [FormerlySerializedAs("levelText")] [SerializeField]
    private TextMeshProUGUI levelText1;
    [SerializeField]
    private TextMeshProUGUI levelText2;

    [SerializeField]
    private TextMeshProUGUI priceText;

    private GuimoonTableData guimoonLocalData;

    private GuimoonServerData guimoonServerData;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockText;
    [SerializeField]
    private TextMeshProUGUI stageText;


    private bool subscribed = false;
    private bool IsMaxLevel()
    {
        return guimoonServerData.level1.Value >= guimoonLocalData.Maxlevel1;
    }

    public void OnClickUnlockButton()
    {
        //해금
        if (guimoonLocalData.Unlockstageid > ServerData.userInfoTable.GetTableData(UserInfoTable.topClearStageId).Value+2)
        {
            PopupManager.Instance.ShowAlarmMessage("스테이지가 부족합니다!");
            return;
        }

        ServerData.guimoonServerTable.TableDatas[guimoonLocalData.Stringid].level2.Value = 1;
            
        List<TransactionValue> transactions = new List<TransactionValue>();
    
        Param guimoonParam = new Param();
        guimoonParam.Add(guimoonLocalData.Stringid, ServerData.guimoonServerTable.TableDatas[guimoonLocalData.Stringid].ConvertToString());
    
        transactions.Add(TransactionValue.SetUpdate(GuimoonServerTable.tableName, GuimoonServerTable.Indate, guimoonParam));
    
        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            PlayerStats.ResetAbilDic();
        });
    }
    
    
    private void UpdateDescription1(float level)
    {
        levelText1.SetText($"LV:{Utils.ConvertBigNum(level)}");
        
        if (IsMaxLevel() == false)
        {
            priceText.SetText(Utils.ConvertBigNum(guimoonLocalData.Upgradeprice1));
        }
        else
        {
            priceText.SetText("MAX");
        }
    
        StatusType abiltype1 = (StatusType)guimoonLocalData.Abiltype1;

        var abilValue1 = guimoonLocalData.Abilvalue1*guimoonServerData.level1.Value;
        if (abiltype1.IsPercentStat())
        {
            relicDescription1.SetText($"{CommonString.GetStatusName(abiltype1)} {Utils.ConvertNum(abilValue1*100,2)}%");
        }
        else
        {
            relicDescription1.SetText($"{CommonString.GetStatusName(abiltype1)} {Utils.ConvertNum(abilValue1)}");
        }
        //수호베기면
        if (abiltype1 == StatusType.SuperCritical11DamPer)
        {
            PlayerStats.ResetSuperCritical11CalculatedValue();
        }
        
    }
    private void UpdateDescription2(float level)
    {
        levelText2.SetText($"LV:{Utils.ConvertBigNum(level)}");
        
        StatusType abiltype2 = (StatusType)guimoonLocalData.Abiltype2;

        var abilValue2 = guimoonLocalData.Abilvalue2*guimoonServerData.level2.Value;

        if (abiltype2.IsPercentStat())
        {
            relicDescription2.SetText($"{CommonString.GetStatusName(abiltype2)} {Utils.ConvertNum(abilValue2*100,2)}%");
        }
        else
        {
            relicDescription2.SetText($"{CommonString.GetStatusName(abiltype2)} {Utils.ConvertNum(abilValue2)}");
        }
        
    }
    
    private void Subscribe()
    {
        guimoonServerData.level1.AsObservable().Subscribe(level =>
        {
            UpdateDescription1(level);
        }).AddTo(this);
        guimoonServerData.level2.AsObservable().Subscribe(level =>
        {
            UpdateDescription2(level);
            lockMask.SetActive(level<1);
        }).AddTo(this);
    }

    public void Initialize(GuimoonTableData guimoonLocalData)
    {
        this.guimoonLocalData = guimoonLocalData;

        this.guimoonServerData = ServerData.guimoonServerTable.TableDatas[this.guimoonLocalData.Stringid];
        
        relicName1.SetText(this.guimoonLocalData.Name1);
        relicName2.SetText(this.guimoonLocalData.Name2);

        var grade = Mathf.Min(guimoonLocalData.Id, CommonUiContainer.Instance.itemGradeColor.Count - 1);
        
        // relicName1.color = CommonUiContainer.Instance.itemGradeColor[grade+1];
        // relicName2.color = CommonUiContainer.Instance.itemGradeColor[grade+1];
        
        if (CommonUiContainer.Instance.guimoonIcon1List.Count != 0 &&
            this.guimoonLocalData.Id < CommonUiContainer.Instance.guimoonIcon1List.Count)
        {
            relicIcon1.sprite = CommonUiContainer.Instance.guimoonIcon1List[this.guimoonLocalData.Id];
        }
        if (CommonUiContainer.Instance.guimoonIcon2List.Count != 0 &&
            this.guimoonLocalData.Id < CommonUiContainer.Instance.guimoonIcon2List.Count)
        {
            relicIcon2.sprite = CommonUiContainer.Instance.guimoonIcon2List[this.guimoonLocalData.Id];
        }

        lockText.SetText(
            $"제{guimoonLocalData.Id + 1}문\n(스테이지 {Utils.ConvertStage(guimoonLocalData.Unlockstageid)}달성 필요)");

        stageText.SetText($"제 {guimoonLocalData.Id + 1}문");
        
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
    
        int currentGuimoonNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value;
    
        if (currentGuimoonNum < 100 * guimoonLocalData.Upgradeprice1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GuimoonRelic)}이 부족합니다!");
            return;
        }
    
        float upgradeableNum = guimoonLocalData.Maxlevel1 - guimoonServerData.level1.Value;
    
        upgradeableNum = Mathf.Min(upgradeableNum, 100);
    
        ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value -= upgradeableNum * guimoonLocalData.Upgradeprice1;
    
        guimoonServerData.level1.Value += upgradeableNum;
    
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }
    
        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    
    }
    public void OnClickResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "해당 능력치를 초기화 합니까?", () =>
        {
            float refundCount = 0;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param guimoonParam = new Param();

            refundCount += ServerData.guimoonServerTable.TableDatas[guimoonLocalData.Stringid].level1.Value * guimoonLocalData.Upgradeprice1;
            ServerData.guimoonServerTable.TableDatas[guimoonLocalData.Stringid].level1.Value = 0;

            guimoonParam.Add(guimoonLocalData.Stringid, ServerData.guimoonServerTable.TableDatas[guimoonLocalData.Stringid].ConvertToString());
            

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
            
            PlayerStats.ResetAbilDic();

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
            });

        }, () => { });
    
    }
    public void OnClickUpgradeAllButton()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }
    
        float currentRelicNum = ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value;
    
        if (currentRelicNum < guimoonLocalData.Upgradeprice1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GuimoonRelic)}이 부족합니다!");
            return;
        }
    
        float upgradeableMaxNum = guimoonLocalData.Maxlevel1 - guimoonServerData.level1.Value;
    
        float upgradableMaxPrice = (float)upgradeableMaxNum * (float)guimoonLocalData.Upgradeprice1;
    
        float diffPrice = currentRelicNum - upgradableMaxPrice;
    
        //유물 남을때(최대업가능)
        if (diffPrice >= 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value -= (upgradeableMaxNum * guimoonLocalData.Upgradeprice1);
    
            guimoonServerData.level1.Value += (int)upgradeableMaxNum;
        }
        else
        {
            int fullUpgradeNum = (int)(currentRelicNum / guimoonLocalData.Upgradeprice1);
    
            ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value -= (fullUpgradeNum * guimoonLocalData.Upgradeprice1);
    
            guimoonServerData.level1.Value += (int)fullUpgradeNum;
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
    
        int currentRelicNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value;
    
        if (currentRelicNum < guimoonLocalData.Upgradeprice1)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GuimoonRelic)}이 부족합니다!");
            return;
        }
    
        ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value -= guimoonLocalData.Upgradeprice1;
    
        guimoonServerData.level1.Value++;
    
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
    
        UpdateDescription1(guimoonServerData.level1.Value);
        
        UpdateDescription2(guimoonServerData.level2.Value);
    
        yield return syncDelay;
    
        List<TransactionValue> transactions = new List<TransactionValue>();
    
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.GuimoonRelic, ServerData.goodsTable.GetTableData(GoodsTable.GuimoonRelic).Value);
    
        Param guimoonParam = new Param();
        guimoonParam.Add(guimoonLocalData.Stringid, ServerData.guimoonServerTable.TableDatas[guimoonLocalData.Stringid].ConvertToString());
    
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(GuimoonServerTable.tableName, GuimoonServerTable.Indate, guimoonParam));
    
        ServerData.SendTransactionV2(transactions, successCallBack: () =>
          {
              //  LogManager.Instance.SendLogType("GuimoonRelic", relicLocalData.Stringid, ServerData.GuimoonRelicServerTable.TableDatas[relicLocalData.Stringid].level.Value.ToString());
          });
    
    
    }
}
