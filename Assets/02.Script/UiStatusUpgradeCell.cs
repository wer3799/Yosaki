using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;

public class UiStatusUpgradeCell : MonoBehaviour
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
    private Image upgradeButton;

    [SerializeField]
    private Image upgradeButton_100;

    [SerializeField]
    private Image upgradeButton_10000;

    [SerializeField]
    private Image upgradeButton_all;

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

    private StatusSettingData statusData;

    [SerializeField]
    private GameObject coinIcon;

    [SerializeField]
    private GameObject memoryIcon;
    [SerializeField]
    private GameObject goldBarIcon;

    [SerializeField]
    private TextMeshProUGUI upgradeText;

    [SerializeField]
    private GameObject allUpgradeButton;
    [SerializeField]
    private GameObject _upgradeButton;

    [SerializeField]
    private GameObject _100UpgradeButton;

    [SerializeField]
    private GameObject _10000UpgradeButton;

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

    public void Initialize(StatusSettingData statusData)
    {
        this.statusData = statusData;


        if (statusData.STATUSWHERE == StatusWhere.goldbar)
        {
            _upgradeButton.SetActive(false);
            _100UpgradeButton.SetActive(false);
            _10000UpgradeButton.SetActive(false);
        }
        else
        {
            _100UpgradeButton.SetActive(true);
            _10000UpgradeButton.SetActive(statusData.STATUSWHERE != StatusWhere.gold);
        }

        allUpgradeButton.SetActive(statusData.STATUSWHERE != StatusWhere.gold);

        memoryIcon.SetActive(statusData.STATUSWHERE == StatusWhere.memory);

        coinIcon.SetActive(statusData.STATUSWHERE == StatusWhere.gold);
        
        goldBarIcon.SetActive(statusData.STATUSWHERE == StatusWhere.goldbar);

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

        statusIcon.sprite = CommonUiContainer.Instance.statusIcon[statusData.Statustype];
    }
    private void RefreshStatusText()
    {
        float currentStatusValue = ServerData.statusTable.GetStatusValue(statusData.Key, currentLevel);
        float nextStatusValue = ServerData.statusTable.GetStatusValue(statusData.Key, currentLevel + 1);

        float price = 0f;
        if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            price = ServerData.statusTable.GetStatusUpgradePrice(statusData.Key, currentLevel);

            priceText.SetText(Utils.ConvertBigNum(price));

            upgradePrice_gold = price;
        }
        else if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            price = 1;
            priceText.SetText($"{price}개");
        }
        else if (statusData.STATUSWHERE == StatusWhere.goldbar)
        {
            price = statusData.Upgradeprice;
            priceText.SetText($"{price}개");
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            price = 1;
            priceText.SetText($"{price}개");
        }

        statusNameText.SetText(statusData.Description);

        if (statusData.Ispercent == false)
        {
            if (IsMaxLevel() == false)
            {
                descriptionText.SetText($"{Utils.ConvertBigNum(currentStatusValue)}->{Utils.ConvertBigNum(nextStatusValue)}");
            }
            else
            {
                descriptionText.SetText($"{Utils.ConvertBigNum(currentStatusValue)}(MAX)");
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
        ServerData.statusTable.GetTableData(statusData.Key).AsObservable().Subscribe(currentLevel =>
        {
            this.currentLevel = currentLevel;
            RefreshStatusText();
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].AsObservable().Subscribe(e =>
        {
            //초기화 됐을때만
            if (currentLevel != -1 && statusData.STATUSWHERE == StatusWhere.gold)
            {
                RefreshStatusText();
            }
        }).AddTo(this);


        if (this.statusData.STATUSWHERE == StatusWhere.gold)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Gold).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }
        else if (this.statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            ServerData.statusTable.GetTableData(StatusTable.StatPoint).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }
        else if (this.statusData.STATUSWHERE == StatusWhere.memory)
        {
            ServerData.statusTable.GetTableData(StatusTable.Memory).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }
        else if (this.statusData.STATUSWHERE == StatusWhere.goldbar)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).AsObservable().Subscribe(e =>
            {
                SetUpgradeButtonState(CanUpgrade());
            }).AddTo(this);
        }

        if (statusData.Unlocklevel != 0)
        {
            string description = "";
            bool isHaveGoods = false;
            if (statusData.Unlockgoods != 0)
            {
                ServerData.goodsTable.GetTableData(statusData.Needgoodskey).AsObservable().Subscribe(e =>
                {
                    isHaveGoods = statusData.Unlockgoods <= e;
                }).AddTo(this);
                
                var type = ServerData.goodsTable.ServerStringToItemType(statusData.Needgoodskey);

                description += $"{CommonString.GetItemName(type)} {statusData.Unlockgoods} 단계 필요\n";
            }
            else
            {
                //아래 Mask SetActive 위해 true
                isHaveGoods = true;
            }

            description += $"{ TableManager.Instance.StatusDatas[statusData.Needstatuskey].Description} LV : {statusData.Unlocklevel} 이상 필요";
            lockDescription.SetText(description);

            ServerData.statusTable.GetTableData(statusData.Needstatuskey).AsObservable().Subscribe(e =>
            {
                lockMask.SetActive((statusData.Unlocklevel >= e + 2) || !isHaveGoods);
            }).AddTo(this);

        }
        else if (statusData.Unlockgoods != 0)
        {
            var type = ServerData.goodsTable.ServerStringToItemType(statusData.Needgoodskey);
            
            lockDescription.SetText($"{CommonString.GetItemName(type)} {statusData.Unlockgoods} 단계 필요");

            ServerData.goodsTable.GetTableData(statusData.Needgoodskey).AsObservable().Subscribe(e =>
            {
                lockMask.SetActive(statusData.Unlockgoods > e);
            }).AddTo(this);
        }
        else
        {
            lockMask.SetActive(false);
        }
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

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            PointerDown();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            PointerUp();
        }
    }
#endif

    private void SyncToServer()
    {
        if (saveRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(saveRoutine);
        }

        saveRoutine = CoroutineExecuter.Instance.StartCoroutine(SaveRoutine());
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

        DailyMissionManager.UpdateDailyMission(DailyMissionKey.AbilUpgrade, 1);
        

        UsePoint();

        ServerData.statusTable.GetTableData(statusData.Key).Value += 1;

        return true;
    }

    private void UsePoint()
    {
        if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value -= upgradePrice_gold;

            UiTutorialManager.Instance.SetClear(TutorialStep.UpgradeGoldStat);
        }
        else if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= 1;
        }
        else if (statusData.STATUSWHERE == StatusWhere.goldbar)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value -= statusData.Upgradeprice;
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= 1;
        }
    }

    private bool IsMaxLevel()
    {
        return statusData.Maxlv <= ServerData.statusTable.GetTableData(statusData.Key).Value;
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

        //구현필요
        if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            bool ret = ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value >= upgradePrice_gold;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName(Item_Type.Gold),JongsungType.Type_IGA)} 부족합니다.");
            }

            return ret;
        }
        else if (statusData.STATUSWHERE == StatusWhere.goldbar)
        {
            bool ret = ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value >= statusData.Upgradeprice;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GoldBar)}가 부족합니다.");
            }

            return ret;
        }
        else if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            bool ret = ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value > 0;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage("스텟포인트가 부족합니다.");
            }

            return ret;
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            bool ret = ServerData.statusTable.GetTableData(StatusTable.Memory).Value > 0;

            if (showPopup && ret == false)
            {
                PopupManager.Instance.ShowAlarmMessage("무공비급이 부족합니다.");
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
        if (upgradeButton == null) return;

        upgradeButton.raycastTarget = on;

        if (IsMaxLevel() == false)
        {
            upgradeButton.sprite = on ? enableSprite : disableSprite;
            upgradeButton_100.sprite = on ? enableSprite : disableSprite;
            upgradeButton_all.sprite = on ? enableSprite : disableSprite;
            upgradeButton_10000.sprite = on ? enableSprite : disableSprite;
        }
        else
        {
            upgradeButton.sprite = maxLevelSprite;
            upgradeButton_100.sprite = maxLevelSprite;
            upgradeButton_all.sprite = maxLevelSprite;
            upgradeButton_10000.sprite = maxLevelSprite;
        }
    }

    public void OnClickAllUpgradeButton()
    {
        if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            var currentStatPoint = ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value;

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

            var currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            var upgradableAmount = maxLevel - currentLevel;

            //맥스렙 가능
            if (currentStatPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= (int)upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value +=(int)upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= (int)currentStatPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += (int)currentStatPoint;
            }
        }
        
        else if (statusData.STATUSWHERE == StatusWhere.goldbar)
        {
            // bool ret = ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value >= upgradePrice_gold;
            //
            // if (showPopup && ret == false)
            // {
            //     PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GoldBar)}가 부족합니다.");
            // }

            //return ret;
            var currentGoldBar = ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value;

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            if (currentGoldBar < statusData.Upgradeprice)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GoldBar)}가 부족합니다.");
                return;
            }


            var currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            //업그레이드가능한 레벨 * 코스트
            //int upgradableAmount = (maxLevel - currentLevel) * statusData.Upgradeprice;
            var upgradableAmount = (maxLevel - currentLevel);

            //2개당 1개로 전환
            var upgradableGoldBar = (int)(currentGoldBar / statusData.Upgradeprice);
            
            //맥스렙 가능
            if (currentGoldBar >= upgradableAmount * statusData.Upgradeprice)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value -= upgradableAmount * statusData.Upgradeprice;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value -= upgradableGoldBar * statusData.Upgradeprice;
                ServerData.statusTable.GetTableData(statusData.Key).Value += (int)upgradableGoldBar;
            }
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            var currentMemoryPoint = ServerData.statusTable.GetTableData(StatusTable.Memory).Value;

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

            var currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            var upgradableAmount = maxLevel - currentLevel;

            //맥스렙 가능
            if (currentMemoryPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= (int)upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += (int)upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= (int)currentMemoryPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += currentMemoryPoint;
            }
        }

        //싱크
        SyncToServer();

        SetUpgradeButtonState(CanUpgrade());
    }

    public void OnClick100_Upgrade()
    {
        if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            var currentStatPoint = ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value;

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

            var currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            var upgradableAmount = maxLevel - currentLevel;

            upgradableAmount = Mathf.Min(upgradableAmount, 100);

            //맥스렙 가능
            if (currentStatPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= currentStatPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += currentStatPoint;
            }
        }
        else if (statusData.STATUSWHERE == StatusWhere.goldbar)
        {
            var currentGoldBar = (int)ServerData.goodsTable.GetTableData(Item_Type.GoldBar).Value;

            if (currentGoldBar < statusData.Upgradeprice)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GoldBar)}가 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

            var currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            //업그레이드가능한 레벨 * 코스트
            var upgradableAmount = (maxLevel - currentLevel) * statusData.Upgradeprice;

            //2개당 1개로 전환
            var upgradableGoldBar = currentGoldBar / statusData.Upgradeprice;
            upgradableAmount = Mathf.Min(upgradableAmount, 100 * statusData.Upgradeprice);

            //맥스렙 가능
            if (currentGoldBar >= upgradableAmount)
            {
                ServerData.goodsTable.GetTableData(Item_Type.GoldBar).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                //current 20개
                ServerData.goodsTable.GetTableData(Item_Type.GoldBar).Value -= upgradableGoldBar*statusData.Upgradeprice;
                ServerData.statusTable.GetTableData(statusData.Key).Value += (int)upgradableGoldBar;
            }
        }
        else if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            
            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }
            var currentGoods = ServerData.goodsTable.GetTableData(Item_Type.Gold).Value;
            var currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;

            int maxLevel = statusData.Maxlv;
            //업그레이드가능한 레벨 * 코스트
            var upgradableAmount = maxLevel - currentLevel;
            var price = 0f;

            var upgradeCount = 0;
            upgradableAmount = Mathf.Min(upgradableAmount, 100);
            for (int i = 0; i < upgradableAmount; i++)
            {
                var addPrice = ServerData.statusTable.GetStatusUpgradePrice(statusData.Key, currentLevel + i);
                if (currentGoods < price + addPrice) break;
                price += addPrice;
                upgradeCount++;
            }
            
            //Debug.LogError($"가능한 강화 수 : {upgradableAmount}\n가격: {price}\n업그레이드 횟수 : {upgradeCount}");
            if (upgradeCount < 1)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Gold)}가 부족합니다.");
                return;
            }
            
            ServerData.goodsTable.GetTableData(Item_Type.Gold).Value -= price;
            ServerData.statusTable.GetTableData(statusData.Key).Value += upgradeCount;
            UiTutorialManager.Instance.SetClear(TutorialStep.UpgradeGoldStat);

        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            var currentMemoryPoint = ServerData.statusTable.GetTableData(StatusTable.Memory).Value;

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

            var currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            var upgradableAmount = maxLevel - currentLevel;

            upgradableAmount = Mathf.Min(upgradableAmount, 100);

            //맥스렙 가능
            if (currentMemoryPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= currentMemoryPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += currentMemoryPoint;
            }
        }

        //싱크
        SyncToServer();

        SetUpgradeButtonState(CanUpgrade());
    }
    public void OnClick10000_Upgrade()
    {
        if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            var currentStatPoint = ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value;

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

            var currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            var upgradableAmount = maxLevel - currentLevel;

            upgradableAmount = Mathf.Min(upgradableAmount, 10000);

            //맥스렙 가능
            if (currentStatPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= (int)upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += (int)upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value -= (int)currentStatPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += (int)currentStatPoint;
            }
        }
        else if (statusData.STATUSWHERE == StatusWhere.goldbar)
        {
            int currentGoldBar = (int)ServerData.goodsTable.GetTableData(Item_Type.GoldBar).Value;

            if (currentGoldBar < statusData.Upgradeprice)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GoldBar)}가 부족합니다.");
                return;
            }

            if (IsMaxLevel())
            {
                PopupManager.Instance.ShowAlarmMessage("최고레벨 입니다.");
                return;
            }

 
            var currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            //업그레이드가능한 레벨 * 코스트
            var upgradableAmount = (maxLevel - currentLevel) * statusData.Upgradeprice;

            //2개당 1개로 전환
            var upgradableGoldBar = currentGoldBar / statusData.Upgradeprice;
            upgradableAmount = Mathf.Min(upgradableAmount, 10000*statusData.Upgradeprice);

            //맥스렙 가능
            if (currentGoldBar >= upgradableAmount)
            {
                ServerData.goodsTable.GetTableData(Item_Type.GoldBar).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.goodsTable.GetTableData(Item_Type.GoldBar).Value -= upgradableGoldBar*statusData.Upgradeprice;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableGoldBar;
            }
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            var currentMemoryPoint = ServerData.statusTable.GetTableData(StatusTable.Memory).Value;

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

            var currentLevel = ServerData.statusTable.GetTableData(statusData.Key).Value;
            int maxLevel = statusData.Maxlv;
            var upgradableAmount = maxLevel - currentLevel;

            upgradableAmount = Mathf.Min(upgradableAmount, 10000);

            //맥스렙 가능
            if (currentMemoryPoint >= upgradableAmount)
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= upgradableAmount;
                ServerData.statusTable.GetTableData(statusData.Key).Value += upgradableAmount;
            }
            else
            {
                ServerData.statusTable.GetTableData(StatusTable.Memory).Value -= currentMemoryPoint;
                ServerData.statusTable.GetTableData(statusData.Key).Value += currentMemoryPoint;
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
        statusParam.Add(statusData.Key, ServerData.statusTable.GetTableData(statusData.Key).Value);

        //스킬포인트
        if (statusData.STATUSWHERE == StatusWhere.statpoint)
        {
            statusParam.Add(StatusTable.StatPoint, ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value);
        }
        else if (statusData.STATUSWHERE == StatusWhere.memory)
        {
            statusParam.Add(StatusTable.Memory, ServerData.statusTable.GetTableData(StatusTable.Memory).Value);
            LogManager.Instance.SendLog("기억능력치", $"key:{statusData.Key} level:{ServerData.statusTable.GetTableData(statusData.Key).Value} memory:{ServerData.statusTable.GetTableData(StatusTable.Memory).Value}");
        }
        else if (statusData.STATUSWHERE == StatusWhere.gold)
        {
            goodesParam.Add(GoodsTable.Gold, ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodesParam));
        }
        else if (statusData.STATUSWHERE == StatusWhere.goldbar)
        {
            goodesParam.Add(GoodsTable.GoldBar, ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodesParam));
        }

        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

        ServerData.SendTransaction(transactionList);

        UiPlayerStatBoard.Instance.Refresh();
    }
}
