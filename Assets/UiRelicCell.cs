using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiRelicCell : MonoBehaviour
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

    private RelicTableData relicLocalData;

    private RelicServerData relicServerData;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockText;


    [SerializeField]
    private GameObject lockMask_San;

    [SerializeField]
    private GameObject lockMask_Chasa;

    [SerializeField]
    private GameObject lockMask_Chun;

    [SerializeField]
    private GameObject lockMask_Dokebi;
    [SerializeField]
    private GameObject lockMask_Sumi;
    [SerializeField]
    private GameObject lockMask_Thief;

    [SerializeField] private Image lockMaskImage;
    [SerializeField]
    private TextMeshProUGUI lockMaskText;
    private bool subscribed = false;
    private bool IsMaxLevel()
    {
        return relicServerData.level.Value >= relicLocalData.Maxlevel;
    }

    private void Subscribe()
    {
        relicServerData.level.AsObservable().Subscribe(level =>
        {
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
                var abilValue = PlayerStats.GetRelicHasEffect(abilType);

                relicDescription.SetText($"{CommonString.GetStatusName(abilType)} {Utils.ConvertNum(abilValue * 100f,2)}%");

            }
            else
            {
                var abilValue = PlayerStats.GetRelicHasEffect(abilType);

                relicDescription.SetText($"{CommonString.GetStatusName(abilType)} {Utils.ConvertNum(abilValue)}");
            }

        }).AddTo(this);

        lockMask.SetActive(false);

        if (relicLocalData.Requirerelic != -1)
        {
            var requireServerData = ServerData.relicServerTable.TableDatas[TableManager.Instance.RelicTable.dataArray[relicLocalData.Requirerelic].Stringid];

            lockText.color = CommonUiContainer.Instance.itemGradeColor[TableManager.Instance.RelicTable.dataArray[relicLocalData.Requirerelic].Grade + 1];

            requireServerData.level.AsObservable().Subscribe(requireLevel =>
            {
                //장산범 아닐때
                if (relicLocalData.Id != 7)
                {
                    lockMask.SetActive(requireLevel < relicLocalData.Requirelevel);
                    lockText.SetText($"{TableManager.Instance.RelicTable.dataArray[relicLocalData.Requirerelic].Name} {relicLocalData.Requirelevel}레벨 필요");
                }

            }).AddTo(this);
        }

        //장산범
        if (relicLocalData.Id == 7)
        {
            lockMask.SetActive(false);

            ServerData.goodsTable.GetTableData(GoodsTable.ZangStone).AsObservable().Subscribe(e =>
            {
                lockMask_San.SetActive(e < 1);
            }).AddTo(this);
        }
        //사인검
        else if (relicLocalData.Id == 8)
        {
            lockMask.SetActive(false);


            ServerData.weaponTable.TableDatas["weapon36"].hasItem.AsObservable().Subscribe(e =>
            {
               lockMask_Chasa.SetActive(e < 1);
            }).AddTo(this);
            
        }
        else if (relicLocalData.Id == 9)
        {
            lockMask.SetActive(false);


            ServerData.weaponTable.TableDatas["weapon50"].hasItem.AsObservable().Subscribe(e =>
            {
               lockMask_Chun.SetActive(e < 1);
            }).AddTo(this);
            
        }
        else if (relicLocalData.Id == 10)
        {
            lockMask.SetActive(false);


            ServerData.weaponTable.TableDatas["weapon77"].hasItem.AsObservable().Subscribe(e =>
            {
               lockMask_Dokebi.SetActive(e < 1);
            }).AddTo(this);
            
        }
        else if (relicLocalData.Id == 11)
        {
            lockMask.SetActive(false);


            ServerData.weaponTable.TableDatas["weapon86"].hasItem.AsObservable().Subscribe(e =>
            {
               lockMask_Sumi.SetActive(e < 1);
            }).AddTo(this);
            
        }
        else if(relicLocalData.Id>=12)
        {
            lockMask.SetActive(false);


            ServerData.weaponTable.TableDatas[$"{relicLocalData.Requireweaponstringid}"].hasItem.AsObservable().Subscribe(e =>
            {
                lockMask_Thief.SetActive(e < 1);
                lockMaskText.SetText($"{relicLocalData.Description}");
                lockMaskImage.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)relicLocalData.Iconitemtype);
            }).AddTo(this);
        }
    }

    public void Initialize(RelicTableData relicLocalData)
    {
        this.relicLocalData = relicLocalData;

        this.relicServerData = ServerData.relicServerTable.TableDatas[this.relicLocalData.Stringid];

        relicName.SetText(this.relicLocalData.Name);
        relicName.color = CommonUiContainer.Instance.itemGradeColor[relicLocalData.Grade + 1];

        relicIcon.sprite = CommonUiContainer.Instance.relicIconList[this.relicLocalData.Id];

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

        float currentRelicNum = ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value;

        if (currentRelicNum < 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Relic)}이 부족합니다!");
            return;
        }

        float upgradeableNum = relicLocalData.Maxlevel - relicServerData.level.Value;

        upgradeableNum = Mathf.Min(upgradeableNum, 10000);

        upgradeableNum = Mathf.Min(upgradeableNum, currentRelicNum);

        ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value -= upgradeableNum;

        relicServerData.level.Value += upgradeableNum;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }

    public void OnClickUpgrade10000Button()
    {
        if (IsMaxLevel())
        {
            PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다!");
            return;
        }

        float currentRelicNum = ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value;

        if (currentRelicNum < 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Relic)}이 부족합니다!");
            return;
        }

        float upgradeableNum = relicLocalData.Maxlevel - relicServerData.level.Value;

        upgradeableNum = Mathf.Min(upgradeableNum, 100000000);

        upgradeableNum = Mathf.Min(upgradeableNum, currentRelicNum);

        ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value -= upgradeableNum;

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

        float currentRelicNum = ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value;

        if (currentRelicNum == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Relic)}이 부족합니다!");
            return;
        }

        float upgradeableNum = relicLocalData.Maxlevel - relicServerData.level.Value;

        upgradeableNum = Mathf.Min(upgradeableNum, currentRelicNum);

        ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value -= upgradeableNum;

        relicServerData.level.Value += upgradeableNum;

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

        float currentRelicNum = ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value;

        if (currentRelicNum == 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Relic)}이 부족합니다!");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value -= relicLocalData.Upgradeprice;

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
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Relic, ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value);

        Param relicParam = new Param();
        relicParam.Add(relicLocalData.Stringid, ServerData.relicServerTable.TableDatas[relicLocalData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(RelicServerTable.tableName, RelicServerTable.Indate, relicParam));

        ServerData.SendTransaction(transactions);
    }
    
    public void OnClickResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "해당 능력치를 초기화 합니까?", () =>
        {
            float refundCount = 0;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param relicParam = new Param();

            refundCount += ServerData.relicServerTable.TableDatas[relicLocalData.Stringid].level.Value * relicLocalData.Upgradeprice;
            ServerData.relicServerTable.TableDatas[relicLocalData.Stringid].level.Value = 0;

            relicParam.Add(relicLocalData.Stringid, ServerData.relicServerTable.TableDatas[relicLocalData.Stringid].ConvertToString());
            

            if (refundCount == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
                return;
            }

            transactions.Add(TransactionValue.SetUpdate(RelicServerTable.tableName, RelicServerTable.Indate, relicParam));


            ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value += refundCount;
            

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Relic, ServerData.goodsTable.GetTableData(GoodsTable.Relic).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            
            PlayerStats.ResetAbilDic();

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("초기화 성공!");
            });

        }, () => { });
    
    }
}
