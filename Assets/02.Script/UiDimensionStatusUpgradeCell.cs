using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.Serialization;

public class UiDimensionStatusUpgradeCell : MonoBehaviour
{
    [SerializeField]
    private Image statusIcon;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI statusNameText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Image upgradeButton_0;

    [SerializeField]
    private Image upgradeButton_1;

    [SerializeField]
    private Image upgradeButton_2;

    [SerializeField]
    private Sprite enableSprite;

    [SerializeField]
    private Sprite disableSprite;

    [SerializeField]
    private Sprite maxLevelSprite;

    [SerializeField]
    private TextMeshProUGUI priceText;

    private ObscuredFloat currentLevel = -1;

    private ObscuredFloat upgradePrice_gold;

    private WaitForSeconds autuSaveDelay = new WaitForSeconds(1.0f);
    private WaitForSeconds autuUpFirstDelay = new WaitForSeconds(1.0f);
    private WaitForSeconds autuUpDelay = new WaitForSeconds(0.01f);

    private Coroutine autoUpRoutine;
    private Coroutine saveRoutine;

    private DimensionStatusData statusData;


    [SerializeField]
    private TextMeshProUGUI upgradeText;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockDescription;

    private void OnDestroy()
    {
        if (CoroutineExecuter.Instance == null) return;

        if (autoUpRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoUpRoutine);
        }
        if (saveRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(saveRoutine);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            currentLevel.RandomizeCryptoKey();
            upgradePrice_gold.RandomizeCryptoKey();
        }
    }

    public void Initialize(DimensionStatusData statusData)
    {
        this.statusData = statusData;


        Subscribe();

        SetUpgradeButtonState(CanUpgrade());

        if (IsMaxLevel())
        {
            upgradeText.SetText("최고레벨");
        }
        else
        {
            upgradeText.SetText("수련");
        }

        statusIcon.sprite = CommonUiContainer.Instance.dimensionStatusIcon[statusData.Statustype];
    }
    private void RefreshStatusText()
    {
        float currentStatusValue = ServerData.statusTable.GetStatusValue(statusData.Key, currentLevel);
        float nextStatusValue = ServerData.statusTable.GetStatusValue(statusData.Key, currentLevel + 1);

        float price = 0f;
        if (statusData.STATUSWHERE == StatusWhere.dimension)
        {
            price = 1;
            priceText.SetText($"{price}개");
        }
        else if (statusData.STATUSWHERE == StatusWhere.special)
        {
            price = statusData.Upgradeprice;
            priceText.SetText($"{price}개");
        }

        statusNameText.SetText(statusData.Description);

        if (statusData.Ispercent == false)
        {
            if (IsMaxLevel() == false)
            {
                descriptionText.SetText($"{Utils.ConvertNum(currentStatusValue)}->{Utils.ConvertNum(nextStatusValue)}");
            }
            else
            {
                descriptionText.SetText($"{Utils.ConvertNum(currentStatusValue)}(MAX)");
            }
        }
        //%로 표시
        else
        {
            if (IsMaxLevel() == false)
            {
                descriptionText.SetText($"{ Utils.ConvertNum(currentStatusValue*100,2)}%->{Utils.ConvertNum(nextStatusValue*100)}%");
            }
            else
            {
     
                    descriptionText.SetText($"{ Utils.ConvertNum(currentStatusValue*100,2)}%(MAX)");
            }
        }

        levelText.SetText($"Lv : {Utils.ConvertNum(currentLevel)}");
        if (IsMaxLevel())
        {
            upgradeText.SetText("최고레벨");
        }
        else
        {
            upgradeText.SetText("수련");
        }
    }
    private void Subscribe()
    {
        ServerData.dimensionStatusTable.GetTableData(statusData.Key).AsObservable().Subscribe(currentLevel =>
        {
            this.currentLevel = currentLevel;
            RefreshStatusText();
        }).AddTo(this);

        if (this.statusData.STATUSWHERE == StatusWhere.dimension)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DC).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }

        else if (this.statusData.STATUSWHERE == StatusWhere.special)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DE).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }

        if (statusData.Unlocklevel != 0)
        {
            string description = "";
            description += $"{ TableManager.Instance.DimensionStatusDatas[statusData.Needstatuskey].Description} LV : {statusData.Unlocklevel} 이상 필요";
            lockDescription.SetText(description);

            ServerData.dimensionStatusTable.GetTableData(statusData.Needstatuskey).AsObservable().Subscribe(e =>
            {
                lockMask.SetActive((statusData.Unlocklevel >= e + 2));
            }).AddTo(this);

        }
        else
        {
            lockMask.SetActive(false);
        }
    }

    private void SyncToServer()
    {
        if (saveRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(saveRoutine);
        }

        saveRoutine = CoroutineExecuter.Instance.StartCoroutine(SaveRoutine());
    }

    public void PointerDown()
    {
        if (autoUpRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoUpRoutine);
        }

        autoUpRoutine = CoroutineExecuter.Instance.StartCoroutine(AutuUpgradeRoutine());

    }
    public void PointerUp()
    {
        if (autoUpRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoUpRoutine);
        }

        SyncToServer();
    }

    private IEnumerator AutuUpgradeRoutine()
    {
        if (UpgradeStatus() == false)
        {
            DisableUpgradeButton();
            yield break;
        }

        yield return autuUpFirstDelay;

        while (true)
        {
            bool canUpgrade = UpgradeStatus();

            if (canUpgrade == false)
            {
                DisableUpgradeButton();
                break;
            }

            yield return autuUpDelay;
        }
    }
    private bool UpgradeStatus()
    {
        if (CanUpgrade(true) == false)
        {
            return false;
        }

        SoundManager.Instance.PlayButtonSound();
        
        UsePoint();

        ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value += 1;

        return true;
    }

    private void UsePoint()
    {
        if (statusData.STATUSWHERE == StatusWhere.dimension)
        {
            ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value -= 1;
        }
        else if (statusData.STATUSWHERE == StatusWhere.special)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DE).Value -= statusData.Upgradeprice;
        }
    }

    private bool IsMaxLevel()
    {
        return statusData.Maxlv <= ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value;
    }

    private bool CanUpgrade(bool showPopup = false)
    {
        if (IsMaxLevel())
        {
            upgradeText.SetText("최고레벨");

            if (showPopup)
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
            }

            return false;
        }
        
        if (statusData.STATUSWHERE == StatusWhere.special)
        {
            bool ret = ServerData.goodsTable.GetTableData(GoodsTable.DE).Value >= statusData.Upgradeprice;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DE)}가 부족합니다.");
            }

            return ret;
        }
        else if (statusData.STATUSWHERE == StatusWhere.dimension)
        {
            bool ret = ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value > 0;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage("스텟포인트가 부족합니다.");
            }

            return ret;
        }

        return true;
    }

    private void DisableUpgradeButton()
    {
        SetUpgradeButtonState(false);
    }

    private void SetUpgradeButtonState(bool on)
    {
        if (upgradeButton_0 == null) return;

        upgradeButton_0.raycastTarget = on;

        if (IsMaxLevel() == false)
        {
            upgradeButton_0.sprite = on ? enableSprite : disableSprite;
            upgradeButton_1.sprite = on ? enableSprite : disableSprite;
            upgradeButton_2.sprite = on ? enableSprite : disableSprite;
        }
        else
        {
            upgradeButton_0.sprite = maxLevelSprite;
            upgradeButton_1.sprite = maxLevelSprite;
            upgradeButton_2.sprite = maxLevelSprite;
        }
    }

    public void OnClickAllUpgradeButton()
    {
        if (statusData.STATUSWHERE == StatusWhere.dimension)
        {
            var currentStatPoint = ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value;

            if (currentStatPoint <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage("스텟이 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            var currentLevel = ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            var upgradableAmount = maxLevel - currentLevel;

            //맥스렙 가능
            if (currentStatPoint >= upgradableAmount)
            {
                ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value -= (int)upgradableAmount;
                ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value +=(int)upgradableAmount;
            }
            else
            {
                ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value -= (int)currentStatPoint;
                ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value += (int)currentStatPoint;
            }
        }
        
        else if (statusData.STATUSWHERE == StatusWhere.special)
        {
          
            //return ret;
            var currentGoldBar = ServerData.goodsTable.GetTableData(GoodsTable.DE).Value;

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            if (currentGoldBar < statusData.Upgradeprice)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DE)}가 부족합니다.");
                return;
            }


            var currentLevel = ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            //업그레이드가능한 레벨 * 코스트
            //int upgradableAmount = (maxLevel - currentLevel) * statusData.Upgradeprice;
            var upgradableAmount = (maxLevel - currentLevel);

            //2개당 1개로 전환
            var upgradableGoldBar = (int)(currentGoldBar / statusData.Upgradeprice);
            
            //맥스렙 가능
            if (currentGoldBar >= upgradableAmount * statusData.Upgradeprice)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.DE).Value -= upgradableAmount * statusData.Upgradeprice;
                ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.goodsTable.GetTableData(GoodsTable.DE).Value -= upgradableGoldBar * statusData.Upgradeprice;
                ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value += (int)upgradableGoldBar;
            }
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            var currentMemoryPoint = ServerData.dimensionStatusTable.GetTableData(StatusTable.Memory).Value;

            if (currentMemoryPoint <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage("무공비급이 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            var currentLevel = ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            var upgradableAmount = maxLevel - currentLevel;

            //맥스렙 가능
            if (currentMemoryPoint >= upgradableAmount)
            {
                ServerData.dimensionStatusTable.GetTableData(StatusTable.Memory).Value -= (int)upgradableAmount;
                ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value += (int)upgradableAmount;
            }
            else
            {
                ServerData.dimensionStatusTable.GetTableData(StatusTable.Memory).Value -= (int)currentMemoryPoint;
                ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value += currentMemoryPoint;
            }
        }

        //싱크
        SyncToServer();

        SetUpgradeButtonState(CanUpgrade());
    }

    public void OnClick_Upgrade1()
    {
        int upgradeCount = 10;
        
        if (statusData.STATUSWHERE == StatusWhere.dimension)
        {
            var currentStatPoint = ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value;

            if (currentStatPoint <= 0)
            {
                PopupManager.Instance.ShowAlarmMessage("스텟이 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            var currentLevel = ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            var upgradableAmount = maxLevel - currentLevel;

            upgradableAmount = Mathf.Min(upgradableAmount, upgradeCount);

            //맥스렙 가능
            if (currentStatPoint >= upgradableAmount)
            {
                ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value -= upgradableAmount;
                ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value -= currentStatPoint;
                ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value += currentStatPoint;
            }
        }
        else if (statusData.STATUSWHERE == StatusWhere.special)
        {
            var currentGoldBar = (int)ServerData.goodsTable.GetTableData(Item_Type.DE).Value;

            if (currentGoldBar < statusData.Upgradeprice)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.DE)}가 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            var currentLevel = ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            //업그레이드가능한 레벨 * 코스트
            var upgradableAmount = (maxLevel - currentLevel) * statusData.Upgradeprice;

            //2개당 1개로 전환
            var upgradableGoldBar = currentGoldBar / statusData.Upgradeprice;
            upgradableAmount = Mathf.Min(upgradableAmount, upgradeCount * statusData.Upgradeprice);

            //맥스렙 가능
            if (currentGoldBar >= upgradableAmount)
            {
                ServerData.goodsTable.GetTableData(Item_Type.DE).Value -= upgradableAmount;
                ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                //current 20개
                ServerData.goodsTable.GetTableData(Item_Type.DE).Value -= upgradableGoldBar*statusData.Upgradeprice;
                ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value += (int)upgradableGoldBar;
            }
        }
        //싱크
        SyncToServer();

        SetUpgradeButtonState(CanUpgrade());
    }
    private IEnumerator SaveRoutine()
    {
        yield return autuSaveDelay;

        SyncData();

        saveRoutine = null;
    }

    private void SyncData()
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param statusParam = new Param();
        Param goodesParam = new Param();

        //능력치
        statusParam.Add(statusData.Key, ServerData.dimensionStatusTable.GetTableData(statusData.Key).Value);

        //스킬포인트
        if (statusData.STATUSWHERE == StatusWhere.dimension)
        {
            statusParam.Add(DimensionStatusTable.DSP, ServerData.dimensionStatusTable.GetTableData(DimensionStatusTable.DSP).Value);
        }
        else if (statusData.STATUSWHERE == StatusWhere.special)
        {
            goodesParam.Add(GoodsTable.DE, ServerData.goodsTable.GetTableData(GoodsTable.DE).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodesParam));
        }

        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

        ServerData.SendTransaction(transactionList);

        UiPlayerStatBoard.Instance.Refresh();
    }
}
