using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using System.Linq;

public class UiWeaponDetailView : MonoBehaviour
{
    [SerializeField]
    private WeaponView weaponView;

    [SerializeField]
    private WeaponView currentCompareView;

    [SerializeField]
    private WeaponView nextCompareView;

    [SerializeField]
    private TextMeshProUGUI compareAmount1;

    [SerializeField]
    private TextMeshProUGUI compareAmount2;

    [SerializeField]
    private Button upgradeButton;

    [SerializeField]
    private TextMeshProUGUI upgradeButtonText;

    [SerializeField]
    private TextMeshProUGUI weaponAbilityDescription;

    [SerializeField]
    private Button levelUpButton;

    [SerializeField]
    private TextMeshProUGUI levelUpPrice;

    private WeaponData weaponData;
    private MagicBookData magicBookData;

    private string upgradeText = "업그레이드";
    private string maxLevelText = "최대레벨";

    private CompositeDisposable disposables = new CompositeDisposable();

    [SerializeField]
    private Button equipButton;

    [SerializeField]
    private TextMeshProUGUI equipDescription;

    [SerializeField]
    private UiWeaponDetailView weaponDetailViewSub;

    [SerializeField]
    private Button skillInfoButton;

    [SerializeField]
    private UiSkillDescriptionPopup skillDescriptionPopup;

    [SerializeField]
    private GameObject arrowObject;

    private void OnDestroy()
    {
        disposables.Dispose();
    }

    public void OnclickSkillButton()
    {
        bool isWeapon = this.weaponData != null;

        if (isWeapon)
        {
            using var e = TableManager.Instance.SkillData.GetEnumerator();
            while (e.MoveNext())
            {
                if (weaponData.Id == e.Current.Value.Awakeweaponidx)
                {
                    skillDescriptionPopup.Initialize(e.Current.Value);
                    break;
                }
            }
        }
    }

    public void Initialize(WeaponData weaponData, MagicBookData magicBookData)
    {
        this.weaponData = weaponData;
        this.magicBookData = magicBookData;

        SetCurrentWeapon();

        SetNextWeapon();

        SetUpdateButtonState();

        UpdateLevelUpUi();

        UpdateEquipButton();

        Subscribe();

        ShowSubDetailView();

        SetSkillButton();
    }

    private void SetSkillButton()
    {
        //서브뷰에서는 얘안씀
        if (skillInfoButton == null) return;

        bool isWeapon = this.weaponData != null;
        bool hasSkill = false;

        if (isWeapon)
        {
            using var e = TableManager.Instance.SkillData.GetEnumerator();
            while (e.MoveNext())
            {
                if (weaponData.Id == e.Current.Value.Awakeweaponidx)
                {
                    hasSkill = true;
                    break;
                }
            }
        }

        skillInfoButton.interactable = isWeapon && hasSkill;
    }

    private void ShowSubDetailView()
    {
        if (weaponDetailViewSub == null) return;

        //무기일때
        if (weaponData != null)
        {
            weaponDetailViewSub.gameObject.SetActive(true);

            int currentEquipWeaponIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;

            var weaponData = TableManager.Instance.WeaponData[currentEquipWeaponIdx];

            weaponDetailViewSub.Initialize(weaponData, null);
        }
        //마법책일때
        else
        {
            int currentEquipMagicBookIdx = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;

            //미장착
            if (currentEquipMagicBookIdx == -1)
            {
                weaponDetailViewSub.gameObject.SetActive(false);
            }
            else
            {
                weaponDetailViewSub.gameObject.SetActive(true);

                var magicBookData = TableManager.Instance.MagicBoocDatas[currentEquipMagicBookIdx];

                weaponDetailViewSub.Initialize(null, magicBookData);
            }
        }
    }

    private void UpdateEquipButton()
    {
        int id = this.weaponData != null ? weaponData.Id : magicBookData.Id;

        int has = 0;

        if (weaponData != null)
        {
            has = ServerData.weaponTable.GetWeaponData(weaponData.Stringid).hasItem.Value;
        }
        else
        {
            has = ServerData.magicBookTable.GetMagicBookData(magicBookData.Stringid).hasItem.Value;
        }

        equipButton.gameObject.SetActive(has == 1);
        levelUpButton.gameObject.SetActive(has == 1);

        if (equipButton.gameObject.activeSelf)
        {
            string key = weaponData != null ? EquipmentTable.Weapon : EquipmentTable.MagicBook;
            int equipIdx = ServerData.equipmentTable.TableDatas[key].Value;

            equipButton.interactable = equipIdx != id;
            equipDescription.SetText(equipIdx == id ? "장착중" : "장착");
        }

        ShowSubDetailView();
    }

    private void Subscribe()
    {
        disposables.Clear();

        ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).AsObservable().Subscribe(WhenMagicStoneAmountChanged).AddTo(disposables);

        if (weaponData != null)
        {
            ServerData.weaponTable.TableDatas[weaponData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(disposables);
        }
        else
        {
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(disposables);
        }
    }

    private void WhenItemLevelChanged(int level)
    {
        SetCurrentWeapon();
        UpdateLevelUpUi();
    }

    private void WhenMagicStoneAmountChanged(float amount)
    {
        if (this.gameObject.activeInHierarchy == false) return;

        UpdateLevelUpUi();
    }

    private void UpdateLevelUpUi()
    {
        if (weaponData == null && magicBookData == null) return;

        if ((weaponData != null && ServerData.weaponTable.TableDatas[weaponData.Stringid].level.Value >= weaponData.Maxlevel) ||
            (magicBookData != null && ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.Value >= magicBookData.Maxlevel)
            )
        {
            levelUpButton.interactable = false;
            levelUpPrice.SetText("최대레벨");
            return;
        }


        float price = 0f;
        float currentMagicStoneAmount = 0f;

        if (weaponData != null)
        {
            price = ServerData.weaponTable.GetWeaponLevelUpPrice(weaponData.Stringid);
            currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
        }
        else
        {
            price = ServerData.magicBookTable.GetMagicBookLevelUpPrice(magicBookData.Stringid);
            currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
        }

        levelUpPrice.SetText(Utils.ConvertBigNum(price));
        levelUpButton.interactable = currentMagicStoneAmount >= price;
    }

    private void SetUpdateButtonState()
    {
        string currentStringid = null;
        int requireUpgrade = 0;
        string nextStringid = null;
        int currentItemCount = 0;

        if (weaponData != null && TableManager.Instance.WeaponData.TryGetValue(weaponData.Id + 1, out var nextWeaponData))
        {
            currentStringid = weaponData.Stringid;
            requireUpgrade = weaponData.Requireupgrade;
            nextStringid = nextWeaponData.Stringid;
            currentItemCount = ServerData.weaponTable.GetCurrentWeaponCount(currentStringid);
        }
        //맥스레벨
        else if (weaponData != null)
        {
            //맥스레벨 처리
            upgradeButton.interactable = false;
            upgradeButtonText.SetText(maxLevelText);
            return;
        }

        if (magicBookData != null && TableManager.Instance.MagicBoocDatas.TryGetValue(magicBookData.Id + 1, out var nextMagicBookData))
        {
            currentStringid = magicBookData.Stringid;
            requireUpgrade = magicBookData.Requireupgrade;
            nextStringid = nextMagicBookData.Stringid;
            currentItemCount = ServerData.magicBookTable.GetCurrentMagicBookCount(currentStringid);
        }
        else if (magicBookData != null)
        {
            //맥스레벨 처리
            upgradeButton.interactable = false;
            upgradeButtonText.SetText(maxLevelText);
            return;
        }



        int upgradeNum = currentItemCount / requireUpgrade;
        upgradeButton.interactable = upgradeNum > 0;
        upgradeButtonText.SetText(upgradeText);

        compareAmount1.gameObject.SetActive(upgradeButton.interactable);
        compareAmount2.gameObject.SetActive(upgradeButton.interactable);

        if (upgradeButton.interactable)
        {
            compareAmount1.SetText($"-{upgradeNum * requireUpgrade * 1}");
            compareAmount2.SetText($"+{upgradeNum}");
        }
    }

    private void SetCurrentWeapon()
    {
        weaponView.Initialize(weaponData, magicBookData);

        currentCompareView.Initialize(weaponData, magicBookData);

        if (weaponData != null)
        {
            compareAmount1.SetText($"{ServerData.weaponTable.GetCurrentWeaponCount(weaponData.Stringid)}/{weaponData.Requireupgrade}");
        }
        else
        {
            compareAmount1.SetText($"{ServerData.magicBookTable.GetCurrentMagicBookCount(magicBookData.Stringid)}/{magicBookData.Requireupgrade}");
        }

        SetWeaponAbilityDescription();
    }

    private void SetWeaponAbilityDescription()
    {
        WeaponEffectData effectData;
        string stringid;
        int weaponLevel = 0;

        if (weaponData != null)
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.weaponData.Weaponeffectid];
            weaponLevel = ServerData.weaponTable.TableDatas[this.weaponData.Stringid].level.Value;
            stringid = this.weaponData.Stringid;
        }
        else
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.magicBookData.Magicbookeffectid];
            weaponLevel = ServerData.magicBookTable.TableDatas[this.magicBookData.Stringid].level.Value;
            stringid = this.magicBookData.Stringid;

        }

        string description = string.Empty;

        description += "<size=35>장착 효과</size> \n";

        float equipValue1 = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1);
        float equipValue1_max = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, this.weaponData.Maxlevel) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, magicBookData.Maxlevel);

        float equipValue2 = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2);
        float equipValue2_max = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, this.weaponData.Maxlevel) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, magicBookData.Maxlevel);

        float hasValue1 = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1);
        float hasValue1_max = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, this.weaponData.Maxlevel) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, magicBookData.Maxlevel);

        float hasValue2 = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2);
        float hasValue2_max = weaponData != null ? ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, this.weaponData.Maxlevel) : ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, magicBookData.Maxlevel);

        if (effectData.Equipeffecttype1 != -1)
        {
            //%효과
            if (effectData.Equipeffectvalue1 < 1f)
            {
                float value = equipValue1 * 100f;
                float value_max = equipValue1_max * 100f;
                StatusType type = (StatusType)(effectData.Equipeffecttype1);

                description += $"{CommonString.GetStatusName(type)} {value.ToString("F1")}%\n(MAX:{value_max.ToString("F1")}%)\n";
            }
            else
            {
                float value = equipValue1;
                float value_max = equipValue1_max;
                StatusType type = (StatusType)(effectData.Equipeffecttype1);

                description += $"{CommonString.GetStatusName(type)} {value}\n(MAX:{value_max}\n)";
            }
        }

        if (effectData.Equipeffecttype2 != -1)
        {
            //%효과
            if (effectData.Equipeffectvalue2 < 1f)
            {
                float value = equipValue2 * 100f;
                float value_max = equipValue2_max * 100f;
                StatusType type = (StatusType)effectData.Equipeffecttype2;

                description += $"{CommonString.GetStatusName(type)} {value.ToString("F1")}%\n(MAX:{value_max.ToString("F1")}%)\n";
            }
            else
            {
                float value = equipValue2;
                float value_max = equipValue2_max;
                StatusType type = (StatusType)effectData.Equipeffecttype2;

                description += $"{CommonString.GetStatusName(type)} {value}\n(MAX:{value_max})\n";
            }

        }

        description += "\n<size=35>보유 효과</size> \n";

        if (effectData.Haseffecttype1 != -1)
        {
            float value = hasValue1 * 100f;
            float value_max = hasValue1_max * 100f;
            StatusType type = (StatusType)effectData.Haseffecttype1;

            description += $"{CommonString.GetStatusName(type)} {value.ToString("F1")}%\n(MAX:{value_max.ToString("F1")}%)\n";
        }

        if (effectData.Haseffecttype2 != -1)
        {
            float value = hasValue2 * 100f;
            float value_max = hasValue2_max * 100f;
            StatusType type = (StatusType)effectData.Haseffecttype2;

            description += $"{CommonString.GetStatusName(type)} {value.ToString("F1")}%\n(MAX:{value_max.ToString("F1")})%";
        }

        weaponAbilityDescription.SetText(description);
    }

    private void SetNextWeapon()
    {
        if (weaponData != null)
        {
            if (TableManager.Instance.WeaponData.TryGetValue(weaponData.Id + 1, out var nextWeapon) == false)
            {
                //마지막 무기 처리
                nextCompareView.gameObject.SetActive(false);
                arrowObject.SetActive(false);
                return;
            }
            arrowObject.SetActive(true);
            nextCompareView.gameObject.SetActive(true);

            nextCompareView.Initialize(nextWeapon, this.magicBookData);

            compareAmount2.SetText($"{ServerData.weaponTable.GetCurrentWeaponCount(nextWeapon.Stringid)}/{weaponData.Requireupgrade}");
        }


        if (magicBookData != null)
        {
            if (TableManager.Instance.MagicBoocDatas.TryGetValue(magicBookData.Id + 1, out var nextMagicBook) == false)
            {
                //마지막 무기 처리
                nextCompareView.gameObject.SetActive(false);
                arrowObject.SetActive(false);
                return;
            }
            arrowObject.SetActive(true);
            nextCompareView.gameObject.SetActive(true);

            nextCompareView.Initialize(this.weaponData, nextMagicBook);

            compareAmount2.SetText($"{ServerData.magicBookTable.GetCurrentMagicBookCount(nextMagicBook.Stringid)}/{magicBookData.Requireupgrade}");

        }
    }

    public void OnClickUpgradeButton()
    {
        if (weaponData != null)
        {
            if (TableManager.Instance.WeaponData.TryGetValue(weaponData.Id + 1, out var nextWeaponData))
            {
                int currentWeaponCount = ServerData.weaponTable.GetCurrentWeaponCount(weaponData.Stringid);
                int nextWeaponCount = ServerData.weaponTable.GetCurrentWeaponCount(nextWeaponData.Stringid);

                int upgradeNum = currentWeaponCount / weaponData.Requireupgrade;

                ServerData.weaponTable.UpData(weaponData, upgradeNum * weaponData.Requireupgrade * -1);
                ServerData.weaponTable.UpData(nextWeaponData, upgradeNum);


                ServerData.weaponTable.SyncToServerAll(new List<int>() { weaponData.Id, nextWeaponData.Id });

                DailyMissionManager.UpdateDailyMission(DailyMissionKey.WeaponUpgrade, upgradeNum);
            }
            else
            {
                //맥스레벨 처리
            }

            Initialize(this.weaponData, this.magicBookData);


        }
        else
        {
            if (TableManager.Instance.MagicBoocDatas.TryGetValue(magicBookData.Id + 1, out var nextMagicBook))
            {
                int currentWeaponCount = ServerData.magicBookTable.GetCurrentMagicBookCount(magicBookData.Stringid);
                int nextWeaponCount = ServerData.magicBookTable.GetCurrentMagicBookCount(nextMagicBook.Stringid);

                int upgradeNum = currentWeaponCount / magicBookData.Requireupgrade;

                ServerData.magicBookTable.UpData(magicBookData, upgradeNum * magicBookData.Requireupgrade * -1);
                ServerData.magicBookTable.UpData(nextMagicBook, upgradeNum);

                ServerData.magicBookTable.SyncToServerAll(new List<int>() { magicBookData.Id, nextMagicBook.Id });

                DailyMissionManager.UpdateDailyMission(DailyMissionKey.MagicbookUpgrade, upgradeNum);
            }
            else
            {
                //맥스레벨 처리
            }

            Initialize(this.weaponData, this.magicBookData);

        }


    }

    public void OnClickLevelUpButton()
    {
        if (weaponData != null)
        {
            float currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
            float levelUpPrice = ServerData.weaponTable.GetWeaponLevelUpPrice(weaponData.Stringid);

            if (ServerData.weaponTable.TableDatas[weaponData.Stringid].level.Value >= weaponData.Maxlevel)
            {
                PopupManager.Instance.ShowAlarmMessage("최대레벨 입니다.");
                return;
            }

            if (currentMagicStoneAmount < levelUpPrice)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.GrowthStone)} 부족합니다.");
                return;
            }

            SoundManager.Instance.PlayButtonSound();
            //재화 차감
            ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value -= levelUpPrice;
            //레벨 상승
            ServerData.weaponTable.TableDatas[weaponData.Stringid].level.Value++;
            //일일 미션
            DailyMissionManager.UpdateDailyMission(DailyMissionKey.WeaponLevelUp, 1);

            //서버에 반영
            SyncServerRoutineWeapon();
        }
        else
        {
            float currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
            float levelUpPrice = ServerData.magicBookTable.GetMagicBookLevelUpPrice(magicBookData.Stringid);

            if (ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.Value >= magicBookData.Maxlevel)
            {
                PopupManager.Instance.ShowAlarmMessage("최대레벨 입니다.");
                return;
            }
#if UNITY_EDITOR
            levelUpPrice = 0;
#endif
            if (currentMagicStoneAmount < levelUpPrice)
            {
                PopupManager.Instance.ShowAlarmMessage("수련의돌이 부족합니다.");
                return;
            }

            //재화 차감
            ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value -= levelUpPrice;
            //레벨 상승
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.Value++;
            //일일 미션
            DailyMissionManager.UpdateDailyMission(DailyMissionKey.MagicBookLevelUp, 1);
            //서버에 반영
            SyncServerRoutineMagicBook();
        }

    }



    private Dictionary<int, Coroutine> SyncRoutine_weapon = new Dictionary<int, Coroutine>();
    private WaitForSeconds syncWaitTime_weapon = new WaitForSeconds(2.0f);
    private void SyncServerRoutineWeapon()
    {
        if (SyncRoutine_weapon.ContainsKey(weaponData.Id) == false)
        {
            SyncRoutine_weapon.Add(weaponData.Id, null);
        }

        if (SyncRoutine_weapon[weaponData.Id] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutine_weapon[weaponData.Id]);
        }

        SyncRoutine_weapon[weaponData.Id] = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutineWeapon(weaponData.Id));
    }

    private IEnumerator SyncDataRoutineWeapon(int id)
    {
        yield return syncWaitTime_weapon;

        WeaponData weapon = TableManager.Instance.WeaponData[id];

        //데이터 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        Param weaponParam = new Param();

        //재화 차감
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        //레벨 상승
        string updateValue = ServerData.weaponTable.TableDatas[weapon.Stringid].ConvertToString();
        weaponParam.Add(weapon.Stringid, updateValue);
        transactionList.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

        ServerData.SendTransaction(transactionList);

        if (SyncRoutine_weapon != null)
        {
            SyncRoutine_weapon[id] = null;
        }
    }

    //

    private Dictionary<int, Coroutine> SyncRoutineMagicBook = new Dictionary<int, Coroutine>();
    private WaitForSeconds syncWaitTimeMagicBook = new WaitForSeconds(2.0f);
    private void SyncServerRoutineMagicBook()
    {
        if (SyncRoutineMagicBook.ContainsKey(magicBookData.Id) == false)
        {
            SyncRoutineMagicBook.Add(magicBookData.Id, null);
        }

        if (SyncRoutineMagicBook[magicBookData.Id] != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(SyncRoutineMagicBook[magicBookData.Id]);
        }

        SyncRoutineMagicBook[magicBookData.Id] = CoroutineExecuter.Instance.StartCoroutine(SyncDataRoutineMagicBook(magicBookData.Id));
    }

    private IEnumerator SyncDataRoutineMagicBook(int id)
    {
        yield return syncWaitTimeMagicBook;

        MagicBookData magicbook = TableManager.Instance.MagicBoocDatas[id];

        //데이터 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        Param magicBookParam = new Param();

        //재화 차감
        goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        //레벨 상승
        string updateValue = ServerData.magicBookTable.TableDatas[magicbook.Stringid].ConvertToString();
        magicBookParam.Add(magicbook.Stringid, updateValue);

        transactionList.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));


        ServerData.SendTransaction(transactionList);

        if (SyncRoutineMagicBook != null)
        {
            SyncRoutineMagicBook[id] = null;
        }
    }

    //
    public void OnClickEquipButton()
    {
        if (weaponData != null)
        {
            ServerData.equipmentTable.ChangeEquip(EquipmentTable.Weapon, weaponData.Id);
          //  UiTutorialManager.Instance.SetClear(TutorialStep._10_EquipWeapon);
        }
        else
        {
            ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook, magicBookData.Id);
        }

        UpdateEquipButton();
    }

    private void OnEnable()
    {
        UpdateLevelUpUi();
    }
}

