using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using UnityEngine.UI.Extensions;

public class UiInventoryWeaponNewView : FancyCell<WeaponData_Fancy>
{
    WeaponData_Fancy itemData;
    
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private WeaponView weaponView;

    [SerializeField]
    private GameObject equipText;

    [SerializeField]
    private GameObject hasMask;

    private Action<WeaponData, MagicBookData> onClickCallBack;

    private WeaponData weaponData;
    private MagicBookData magicBookData;
    private NewGachaTableData newGachaData;
    private SealSwordData sealSwordData;

    [SerializeField]
    private GameObject upgradeButton;

    [SerializeField]
    private TextMeshProUGUI weaponAbilityDescription;

    [SerializeField]
    private Button levelUpButton;
    [SerializeField]
    private TextMeshProUGUI levelUpButtonText;
    [SerializeField]
    private Image upgradeGoodsImage;

    [SerializeField]
    private TextMeshProUGUI levelUpPrice;

    [SerializeField]
    private Button equipButton;

    [SerializeField]
    private GameObject tutorialObject;

    [SerializeField]
    private GameObject yomulDescription;

    [SerializeField]
    private GameObject yomulUpgradeButton;

    [SerializeField]
    private GameObject sinsuCreateButton;

    [SerializeField]
    private GameObject youngMulCreateButton;

    [SerializeField]
    private GameObject youngMulCreateButton2;

    [SerializeField]
    private GameObject yachaDescription;

    [SerializeField]
    private GameObject yachaUpgradeButton;

    [SerializeField]
    private GameObject feelMulCraftButton;

    [SerializeField]
    private GameObject feelMulUpgradeButton;

    [SerializeField]
    private Image weaponViewEquipButton;

    [SerializeField]
    private TextMeshProUGUI weaponViewEquipDesc;

    [SerializeField]
    private Image magicBookViewEquipButton;

    [SerializeField]
    private TextMeshProUGUI magicBookViewEquipDesc;


    [SerializeField]
    private Sprite weaponViewEquipDisable;

    [SerializeField]
    private Sprite weaponViewEquipEnable;

    [SerializeField]
    private GameObject feelMul2Lock;

    [SerializeField]
    private GameObject feelMul3Lock;

    [SerializeField]
    private GameObject feelMul4Lock;

    [SerializeField]
    private GameObject indraLock;

    [SerializeField]
    private GameObject nataLock;

    [SerializeField]
    private GameObject orochiLock;

    [SerializeField]
    private GameObject feelPaeLock;

    [SerializeField]
    private GameObject gumihoWeaponLock;

    [SerializeField]
    private GameObject hellWeaponLock;

    [SerializeField]
    private GameObject yeoRaeWeaponLock;

    [SerializeField]
    private GameObject weaponLockObject;

    [SerializeField]
    private TextMeshProUGUI weaponLockDescription;

    [SerializeField]
    private GameObject armDescription;

    [SerializeField]
    private GameObject chunDescription;

    [SerializeField]
    private TextMeshProUGUI norigaeDescription;

    [SerializeField]
    private TextMeshProUGUI suhoSinDescription;

    [SerializeField]
    private GameObject foxNorigaeGetButton;
    
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnDestroy()
    {
        disposables.Dispose();
    }
    public WeaponData GetWeaponData()
    {
        return weaponData;
    }

    public MagicBookData GetMagicBookData()
    {
        return magicBookData;
    }

    private void SetEquipButton(bool onOff)
    {
        equipButton.gameObject.SetActive(onOff);

        if (magicBookData != null && magicBookData.MAGICBOOKTYPE == MagicBookType.View)
        {
            equipButton.gameObject.SetActive(false);
        }


        if (weaponData != null && (weaponData.WEAPONTYPE == WeaponType.View||weaponData.WEAPONTYPE == WeaponType.RecommendView|| weaponData.WEAPONTYPE == WeaponType.HasEffectOnly))
        {
            equipButton.gameObject.SetActive(false);
        }

        //사신수
        if (weaponData != null && weaponData.Id >= 67 && weaponData.Id <= 70)
        {
            equipButton.gameObject.SetActive(false);
            //
        }
    }

    public void OnClickWeaponViewButton()
    {
        if (weaponData != null)
        {

            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 무기 외형을 변경 할까요?", () => { ServerData.equipmentTable.ChangeEquip(EquipmentTable.Weapon_View, weaponData.Id); }, () => { });
            //   UiTutorialManager.Instance.SetClear(TutorialStep._10_EquipWeapon);
        }
    }

    public void OnClickMagicBookViewButton()
    {
        if (magicBookData != null)
        {

            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 노리개 외형을 변경 할까요?", () => { ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook_View, magicBookData.Id); }, () => { });
            //   UiTutorialManager.Instance.SetClear(TutorialStep._10_EquipWeapon);
        }
    }

    public void Initialize(WeaponData weaponData, MagicBookData magicBookData, NewGachaTableData newGachaTableData, SealSwordData sealSwordData, Action<WeaponData, MagicBookData> onClickCallBack)
    {
        this.weaponData = weaponData;
        this.magicBookData = magicBookData;
        this.newGachaData = newGachaTableData;
        this.sealSwordData = sealSwordData;

        this.onClickCallBack = onClickCallBack;

        tutorialObject.SetActive(weaponData != null && weaponData.Id != 0 && ServerData.weaponTable.TableDatas[weaponData.Stringid].hasItem.Value == 1);

        //요물 설명
        yomulDescription.SetActive(weaponData != null && weaponData.Id == 19);

        yomulUpgradeButton.SetActive(weaponData != null && weaponData.Id == 20);

        //야차 설명
        yachaDescription.SetActive(weaponData != null && weaponData.Id == 20);

        yachaUpgradeButton.SetActive(weaponData != null && weaponData.Id == 21);

        feelMulCraftButton.SetActive(weaponData != null && weaponData.Id == 21);

        feelMulUpgradeButton.SetActive(weaponData != null && weaponData.Id == 22);

        //weaponViewEquipButton.SetActive(weaponData != null);

        armDescription.gameObject.SetActive(weaponData != null && weaponData.Id == 23);
        chunDescription.gameObject.SetActive(weaponData != null && weaponData.Id == 24);

        //신수
        sinsuCreateButton.gameObject.SetActive(magicBookData != null && magicBookData.Id / 4 == 4);

        youngMulCreateButton.gameObject.SetActive(magicBookData != null && magicBookData.Id == 20);
        youngMulCreateButton2.gameObject.SetActive(magicBookData != null && magicBookData.Id == 21);

        norigaeDescription.gameObject.SetActive(true);

        suhoSinDescription.gameObject.SetActive(magicBookData != null);

        foxNorigaeGetButton.SetActive(false);

        if (magicBookData != null)
        {
            foxNorigaeGetButton.SetActive(magicBookData.Id == 28);
        }

        if (magicBookData != null)
        {
            norigaeDescription.SetText($"기본무공 강화\n{Utils.ConvertBigNum(magicBookData.Goldabilratio)}배");
        }

        if (weaponData != null)
        {
            norigaeDescription.SetText($"무공비급 강화\n{weaponData.Specialadd}배");
        }

        if (newGachaData != null)
        {
            norigaeDescription.SetText($"영혼의숲 능력치\n{newGachaData.Specialadd}배 상승\n<color=red>(공격 능력치)");
        }

        if (sealSwordData != null)
        {
            norigaeDescription.SetText($"미정 미정 미정");
        }

        if (weaponData != null)
        {
            title.SetText(weaponData.Name);
            weaponView.Initialize(weaponData, null);
        }
        else if (magicBookData != null)
        {
            title.SetText(magicBookData.Name);
            weaponView.Initialize(null, magicBookData);
        }

        UpdateLevelUpUi();
        
        SubscribeWeapon();

        SetParent();
    }
    private void SubscribeWeapon()
    {
        disposables.Clear();
        if (weaponData != null)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].AsObservable().Subscribe(WhenEquipWeaponChanged).AddTo(disposables);
            ServerData.weaponTable.TableDatas[weaponData.Stringid].hasItem.AsObservable().Subscribe(WhenHasStageChanged).AddTo(disposables);
            ServerData.weaponTable.TableDatas[weaponData.Stringid].amount.AsObservable().Subscribe(WhenAmountChanged).AddTo(disposables);

            ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].AsObservable().Subscribe(WhenEquipWeapon_ViewChanged).AddTo(disposables);
        }
        else if (magicBookData != null)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].AsObservable().Subscribe(WhenEquipMagicBookChanged).AddTo(disposables);
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].hasItem.AsObservable().Subscribe(WhenHasStageChanged).AddTo(disposables);
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].amount.AsObservable().Subscribe(WhenAmountChanged).AddTo(disposables);

            ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].AsObservable().Subscribe(WhenEquipMagicBook_ViewChanged).AddTo(disposables);
        }
        else if (newGachaData != null)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.SoulRing].AsObservable().Subscribe(WhenEquipNewGachaChanged).AddTo(disposables);
            ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].hasItem.AsObservable().Subscribe(WhenHasStageChanged).AddTo(disposables);
            ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].amount.AsObservable().Subscribe(WhenAmountChanged).AddTo(disposables);
        }
        else if (sealSwordData != null)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.SealSword].AsObservable().Subscribe(WhenEquipSealSwordChanged).AddTo(disposables);
            ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].hasItem.AsObservable().Subscribe(WhenHasStageChanged).AddTo(disposables);
            ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].amount.AsObservable().Subscribe(WhenAmountChanged).AddTo(disposables);
        }


        if (weaponData != null)
        {
            ServerData.weaponTable.TableDatas[weaponData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(disposables);
        }
        else if (magicBookData != null)
        {
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(disposables);
        }
        else if (newGachaData != null)
        {
            ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(disposables);
        }
        else if (sealSwordData != null)
        {
            ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(disposables);
        }
    }


    private void WhenItemLevelChanged(int level)
    {
        SetCurrentWeapon();
        UpdateLevelUpUi();
        SetTextLevelUpButton(level);
    }

    private void SetTextLevelUpButton(int level)
    {
        if (level < weaponData.Maxlevel)
        {
            levelUpButtonText.SetText("레벨업");
            upgradeGoodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(Item_Type.GrowthStone);
        }
        else
        {
            if (weaponData.WEAPONTYPE == WeaponType.Normal)
            {
                if (ServerData.weaponTable.TableDatas[weaponData.Stringid].trans.Value > 0)
                {
                    levelUpButtonText.SetText("초월 완료");
                }
                else
                {
                    levelUpButtonText.SetText("초월");
                }   
            }
            else
            {
                levelUpButtonText.SetText("레벨업");
            }
        }
    }
    
    private void UpdateLevelUpUi()
    {
        if (weaponData == null && magicBookData == null && newGachaData == null && sealSwordData == null) return;

        if ((weaponData != null && ServerData.weaponTable.TableDatas[weaponData.Stringid].level.Value >= weaponData.Maxlevel) ||
            (magicBookData != null && ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.Value >= magicBookData.Maxlevel) ||
            (newGachaData != null && ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].level.Value >= newGachaData.Maxlevel) ||
            (sealSwordData != null && ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].level.Value >= sealSwordData.Maxlevel)
           )
        {
            levelUpButton.interactable = false;
            if (weaponData.WEAPONTYPE== WeaponType.Normal)
            {
                if (ServerData.weaponTable.TableDatas[weaponData.Stringid].trans.Value > 0)
                {
                    levelUpPrice.SetText("초월완료");
                    upgradeGoodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(Item_Type.TransGoods);
                }
                else
                {
                    levelUpPrice.SetText($"{weaponData.Transrequirevalue}");
                    upgradeGoodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(Item_Type.TransGoods);
                }   
            }
            else
            {
                levelUpPrice.SetText("최대레벨");
                upgradeGoodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(Item_Type.GrowthStone);
            }
            
            return;
        }


        float price = 0f;
        float currentMagicStoneAmount = 0f;

        if (weaponData != null)
        {
            price = ServerData.weaponTable.GetWeaponLevelUpPrice(weaponData.Stringid);
            currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
        }
        else if (magicBookData != null)
        {
            price = ServerData.magicBookTable.GetMagicBookLevelUpPrice(magicBookData.Stringid);
            currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
        }
        else if (newGachaData != null)
        {
            price = ServerData.newGachaServerTable.GetNewGachaLevelUpPrice(newGachaData.Stringid);
            currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
        }
        else
        {
            //안씀
            price = 0;
            currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
        }


        levelUpPrice.SetText(Utils.ConvertBigNum(price));
        levelUpButton.interactable = currentMagicStoneAmount >= price;
    }

    private void WhenAmountChanged(int amount)
    {
        if (weaponData != null)
        {
            //요물일때 X
            upgradeButton.SetActive(amount >= weaponData.Requireupgrade && weaponData.Id < 19);
            
        }
        else if (magicBookData != null)
        {
            upgradeButton.SetActive(amount >= magicBookData.Requireupgrade && magicBookData.Id < 15);
        }
        else if (newGachaData != null)
        {
            upgradeButton.SetActive(amount >= newGachaData.Requireupgrade);
        }
        else if (sealSwordData != null)
        {
            upgradeButton.SetActive(amount >= sealSwordData.Requireupgrade);
        }
    }

    private void WhenHasStageChanged(int state)
    {
        hasMask.SetActive(state == 0);

        SetEquipButton(state == 1);

        levelUpButton.gameObject.SetActive(state == 1);


        if (weaponData != null)
        {
            weaponViewEquipButton.gameObject.SetActive(state == 1);
            magicBookViewEquipButton.gameObject.SetActive(false);

            feelMul2Lock.SetActive(false);
            feelMul3Lock.SetActive(false);
            feelMul4Lock.SetActive(false);
            indraLock.SetActive(false);
            nataLock.SetActive(false);
            orochiLock.SetActive(false);
            feelPaeLock.SetActive(false);
            gumihoWeaponLock.SetActive(false);
            hellWeaponLock.SetActive(false);
            yeoRaeWeaponLock.SetActive(false);
            weaponLockObject.SetActive(false);


            hasMask.SetActive(false);

            weaponLockObject.gameObject.SetActive(state == 0);
            weaponLockDescription.SetText($"{weaponData.Acquiredescription}");

            if (weaponData.Id == 23)
            {
                weaponLockObject.gameObject.SetActive(false);
                feelMul2Lock.gameObject.SetActive(state == 0);
            }

            else if (weaponData.Id == 24)
            {
                weaponLockObject.gameObject.SetActive(false);
                feelMul3Lock.gameObject.SetActive(state == 0);
            }

            else if (weaponData.Id == 25)
            {
                weaponLockObject.gameObject.SetActive(false);
                feelMul4Lock.gameObject.SetActive(state == 0);
            }

            else if (weaponData.Id == 26)
            {
                weaponLockObject.gameObject.SetActive(false);
                indraLock.gameObject.SetActive(state == 0);
            }

            else if (weaponData.Id == 27)
            {
                weaponLockObject.gameObject.SetActive(false);
                nataLock.gameObject.SetActive(state == 0);
            }

            else if (weaponData.Id == 28)
            {
                weaponLockObject.gameObject.SetActive(false);
                orochiLock.gameObject.SetActive(state == 0);
            }

            else if (weaponData.Id == 29)
            {
                weaponLockObject.gameObject.SetActive(false);
                feelPaeLock.gameObject.SetActive(state == 0);
            }

            else if (weaponData.Id == 30)
            {
                weaponLockObject.gameObject.SetActive(false);
                gumihoWeaponLock.gameObject.SetActive(state == 0);
            }
            else if (weaponData.Id == 31 || weaponData.Id == 32)
            {
                weaponLockObject.gameObject.SetActive(false);
                hellWeaponLock.gameObject.SetActive(state == 0);
            }

            else if (weaponData.Id == 33)
            {
                weaponLockObject.gameObject.SetActive(false);
                yeoRaeWeaponLock.gameObject.SetActive(state == 0);
            }
        }
        else if (magicBookData != null)
        {
            feelMul2Lock.SetActive(false);
            feelMul3Lock.SetActive(false);
            feelMul4Lock.SetActive(false);
            indraLock.SetActive(false);
            nataLock.SetActive(false);
            orochiLock.SetActive(false);
            feelPaeLock.SetActive(false);
            gumihoWeaponLock.SetActive(false);
            hellWeaponLock.SetActive(false);
            yeoRaeWeaponLock.SetActive(false);
            weaponLockObject.SetActive(false);

            hasMask.SetActive(state == 0);

            suhoSinDescription.SetText($"{magicBookData.Acquiredescription}");

            magicBookViewEquipButton.gameObject.SetActive(state == 1);
            weaponViewEquipButton.gameObject.SetActive(false);
        }
        else //스킬데이타 + ring
        {
            levelUpButton.gameObject.SetActive(false);
            feelMul2Lock.SetActive(false);
            feelMul3Lock.SetActive(false);
            feelMul4Lock.SetActive(false);
            indraLock.SetActive(false);
            nataLock.SetActive(false);
            orochiLock.SetActive(false);
            feelPaeLock.SetActive(false);
            gumihoWeaponLock.SetActive(false);
            hellWeaponLock.SetActive(false);
            yeoRaeWeaponLock.SetActive(false);
            weaponLockObject.SetActive(false);

            magicBookViewEquipButton.gameObject.SetActive(false);
            weaponViewEquipButton.gameObject.SetActive(false);
        }
    }

    private void WhenEquipWeaponChanged(int idx)
    {
        equipText.SetActive(idx == this.weaponData.Id);

        UpdateEquipButton();
    }

    private void WhenEquipWeapon_ViewChanged(int idx)
    {
        if (weaponViewEquipDesc != null)
        {
            weaponViewEquipDesc.SetText(idx == this.weaponData.Id ? "적용" : "외형적용");
            weaponViewEquipButton.sprite = (idx == this.weaponData.Id ? weaponViewEquipDisable : weaponViewEquipEnable);
        }
    }

    private void WhenEquipMagicBook_ViewChanged(int idx)
    {
        if (magicBookViewEquipDesc != null)
        {
            magicBookViewEquipDesc.SetText(idx == this.magicBookData.Id ? "적용" : "외형적용");
            magicBookViewEquipButton.sprite = (idx == this.magicBookData.Id ? weaponViewEquipDisable : weaponViewEquipEnable);
        }
    }

    private void WhenEquipMagicBookChanged(int idx)
    {
        equipText.SetActive(idx == this.magicBookData.Id);

        UpdateEquipButton();
    }

    private void WhenEquipNewGachaChanged(int idx)
    {
        equipText.SetActive(idx == this.newGachaData.Id);

        UpdateEquipButton();
    }

    private void WhenEquipSealSwordChanged(int idx)
    {
        equipText.SetActive(idx == this.sealSwordData.Id);

        UpdateEquipButton();
    }

    public void OnClickIcon()
    {
        onClickCallBack?.Invoke(weaponData, magicBookData);
    }

    public void OnClickUpgradeButton()
    {
        if (weaponData != null)
        {
            int amount = ServerData.weaponTable.TableDatas[weaponData.Stringid].amount.Value;

            if (amount < weaponData.Requireupgrade)
            {
                return;
            }

            if (TableManager.Instance.WeaponData.TryGetValue(weaponData.Id + 1, out var nextWeaponData))
            {
                int currentWeaponCount = ServerData.weaponTable.GetCurrentWeaponCount(weaponData.Stringid);
                int nextWeaponCount = ServerData.weaponTable.GetCurrentWeaponCount(nextWeaponData.Stringid);

                int upgradeNum = currentWeaponCount / weaponData.Requireupgrade;

                ServerData.weaponTable.UpData(weaponData, upgradeNum * weaponData.Requireupgrade * -1);
                ServerData.weaponTable.UpData(nextWeaponData, upgradeNum);

                DailyMissionManager.UpdateDailyMission(DailyMissionKey.WeaponUpgrade, upgradeNum);
                ServerData.weaponTable.SyncToServerAll(new List<int>() { weaponData.Id, nextWeaponData.Id });
                itemData.ParentView.SortHasItem();
            }
            else
            {
                //맥스레벨 처리
                PopupManager.Instance.ShowAlarmMessage("더이상 승급이 불가능 합니다.");
            }
        }
        else if (magicBookData != null)
        {
            int amount = ServerData.magicBookTable.TableDatas[magicBookData.Stringid].amount.Value;

            if (amount < magicBookData.Requireupgrade)
            {
                return;
            }

            if (TableManager.Instance.MagicBoocDatas.TryGetValue(magicBookData.Id + 1, out var nextMagicBook))
            {
                int currentWeaponCount = ServerData.magicBookTable.GetCurrentMagicBookCount(magicBookData.Stringid);
                int nextWeaponCount = ServerData.magicBookTable.GetCurrentMagicBookCount(nextMagicBook.Stringid);

                int upgradeNum = currentWeaponCount / magicBookData.Requireupgrade;

                ServerData.magicBookTable.UpData(magicBookData, upgradeNum * magicBookData.Requireupgrade * -1);
                ServerData.magicBookTable.UpData(nextMagicBook, upgradeNum);

                DailyMissionManager.UpdateDailyMission(DailyMissionKey.MagicbookUpgrade, upgradeNum);
                ServerData.magicBookTable.SyncToServerAll(new List<int>() { magicBookData.Id, nextMagicBook.Id });
            }
            else
            {
                //맥스레벨 처리
                PopupManager.Instance.ShowAlarmMessage("더이상 승급이 불가능 합니다.");
            }
        }
        else if (newGachaData != null)
        {
            int amount = ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].amount.Value;

            if (amount < newGachaData.Requireupgrade)
            {
                return;
            }

            if (TableManager.Instance.NewGachaData.TryGetValue(newGachaData.Id + 1, out var nextNewGacha))
            {
                int currentWeaponCount = ServerData.newGachaServerTable.GetCurrentNewGachaCount(newGachaData.Stringid);
                int nextWeaponCount = ServerData.newGachaServerTable.GetCurrentNewGachaCount(nextNewGacha.Stringid);

                int upgradeNum = currentWeaponCount / newGachaData.Requireupgrade;

                ServerData.newGachaServerTable.UpData(newGachaData, upgradeNum * newGachaData.Requireupgrade * -1);
                ServerData.newGachaServerTable.UpData(nextNewGacha, upgradeNum);

                ServerData.newGachaServerTable.SyncToServerAll(new List<int>() { newGachaData.Id, nextNewGacha.Id });
            }
            else
            {
                //맥스레벨 처리
                PopupManager.Instance.ShowAlarmMessage("더이상 승급이 불가능 합니다.");
            }
        }
        else if (sealSwordData != null)
        {
            int amount = ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].amount.Value;

            if (amount < sealSwordData.Requireupgrade)
            {
                return;
            }

            if (TableManager.Instance.SealSwordData.TryGetValue(sealSwordData.Id + 1, out var sealSword))
            {
                int currentWeaponCount = ServerData.sealSwordServerTable.GetCurrentWeaponCount(sealSwordData.Stringid);
                int nextWeaponCount = ServerData.sealSwordServerTable.GetCurrentWeaponCount(sealSword.Stringid);

                int upgradeNum = currentWeaponCount / sealSwordData.Requireupgrade;

                ServerData.sealSwordServerTable.UpData(sealSwordData, upgradeNum * sealSwordData.Requireupgrade * -1);
                ServerData.sealSwordServerTable.UpData(sealSword, upgradeNum);

                ServerData.sealSwordServerTable.SyncToServerAll(new List<int>() { sealSwordData.Id, sealSword.Id });
            }
            else
            {
                //맥스레벨 처리
                PopupManager.Instance.ShowAlarmMessage("더이상 승급이 불가능 합니다.");
            }
        }
    }

    public void OnClickAllUpgradeButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "이전 단계 장비들도 전부 승급 합니까?", () =>
        {
            if (weaponData != null)
            {
                itemData.ParentView.AllUpgradeWeapon(weaponData.Id);
                itemData.ParentView.SortHasItem();
                //parentBoard.AllUpgradeWeapon(weaponData.Id);
                
                //UiEnventoryBoard.Instance.AllUpgradeWeapon(weaponData.Id);
            }
            
            // if (magicBookData != null)
            // {
            //     UiEnventoryBoard.Instance.AllUpgradeMagicBook(magicBookData.Id);
            // }
            // else if (newGachaData != null)
            // {
            //     UiEnventoryBoard.Instance.AllUpgradeNewGacha(newGachaData.Id);
            // }
            // else
            // {
            //     UiSealSwordBoard.Instance.AllUpgradeWeapon(sealSwordData.Id);
            // }
        }, null);
    }

    private void SetCurrentWeapon()
    {

        weaponView.Initialize(weaponData, magicBookData, null, newGachaData, sealSwordData);

        //currentCompareView.Initialize(weaponData, magicBookData);

        //if (weaponData != null)
        //{
        //    compareAmount1.SetText($"{DatabaseManager.weaponTable.GetCurrentWeaponCount(weaponData.Stringid)}/{weaponData.Requireupgrade}");
        //}
        //else
        //{
        //    compareAmount1.SetText($"{DatabaseManager.magicBookTable.GetCurrentMagicBookCount(magicBookData.Stringid)}/{magicBookData.Requireupgrade}");
        //}

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
        else if (magicBookData != null)
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.magicBookData.Magicbookeffectid];
            weaponLevel = ServerData.magicBookTable.TableDatas[this.magicBookData.Stringid].level.Value;
            stringid = this.magicBookData.Stringid;
        }
        else if (newGachaData != null)
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.newGachaData.Effectid];
            weaponLevel = ServerData.newGachaServerTable.TableDatas[this.newGachaData.Stringid].level.Value;
            stringid = this.newGachaData.Stringid;
        }
        else if (sealSwordData != null)
        {
            effectData = TableManager.Instance.WeaponEffectDatas[0];
            weaponLevel = ServerData.sealSwordServerTable.TableDatas[this.sealSwordData.Stringid].level.Value;
            stringid = this.sealSwordData.Stringid;
        }
        else
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.magicBookData.Magicbookeffectid];
            weaponLevel = ServerData.magicBookTable.TableDatas[this.magicBookData.Stringid].level.Value;
            stringid = this.magicBookData.Stringid;
        }

        string description = string.Empty;

        description += "<color=#ff00ffff>장착 효과</color>\n";

        float equipValue1 = 0f, equipValue1_max = 0f, equipValue2 = 0f, equipValue2_max = 0f;
        float hasValue1 = 0f, hasValue2 = 0f, hasValue3 = 0f, hasValue1_max = 0f, hasValue2_max = 0f, hasValue3_max = 0f;
        if (weaponData != null)
        {
            equipValue1 = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1);
            equipValue1_max = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, this.weaponData.Maxlevel);
            equipValue2 = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2);
            equipValue2_max = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, this.weaponData.Maxlevel);

            hasValue1 = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1);
            hasValue1_max = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, this.weaponData.Maxlevel);
            hasValue2 = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2);
            hasValue2_max = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, this.weaponData.Maxlevel);
            hasValue3 = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3);
            hasValue3_max = ServerData.weaponTable.GetWeaponEffectValue(this.weaponData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3, this.weaponData.Maxlevel);
        }
        else if (magicBookData != null)
        {
            equipValue1 = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1);
            equipValue1_max = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, this.magicBookData.Maxlevel);
            equipValue2 = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2);
            equipValue2_max = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, this.magicBookData.Maxlevel);

            hasValue1 = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1);
            hasValue1_max = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, this.magicBookData.Maxlevel);
            hasValue2 = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2);
            hasValue2_max = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, this.magicBookData.Maxlevel);
            hasValue3 = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3);
            hasValue3_max = ServerData.magicBookTable.GetMagicBookEffectValue(this.magicBookData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3, this.magicBookData.Maxlevel);
        }
        else if (newGachaData != null)
        {
            equipValue1 = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1);
            equipValue1_max = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, this.newGachaData.Maxlevel);
            equipValue2 = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2);
            equipValue2_max = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, this.newGachaData.Maxlevel);

            hasValue1 = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1);
            hasValue1_max = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, this.newGachaData.Maxlevel);
            hasValue2 = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2);
            hasValue2_max = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, this.newGachaData.Maxlevel);
            hasValue3 = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3);
            hasValue3_max = ServerData.newGachaServerTable.GetNewGachaEffectValue(this.newGachaData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3, this.newGachaData.Maxlevel);
        }
        else if (sealSwordData != null)
        {
            equipValue1 = ServerData.sealSwordServerTable.GetWeaponEffectValue(this.sealSwordData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1);
            equipValue1_max = ServerData.sealSwordServerTable.GetWeaponEffectValue(this.sealSwordData.Stringid, effectData.Equipeffectbase1, effectData.Equipeffectvalue1, this.sealSwordData.Maxlevel);
            equipValue2 = ServerData.sealSwordServerTable.GetWeaponEffectValue(this.sealSwordData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2);
            equipValue2_max = ServerData.sealSwordServerTable.GetWeaponEffectValue(this.sealSwordData.Stringid, effectData.Equipeffectbase2, effectData.Equipeffectvalue2, this.sealSwordData.Maxlevel);

            hasValue1 = ServerData.sealSwordServerTable.GetWeaponEffectValue(this.sealSwordData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1);
            hasValue1_max = ServerData.sealSwordServerTable.GetWeaponEffectValue(this.sealSwordData.Stringid, effectData.Haseffectbase1, effectData.Haseffectvalue1, this.sealSwordData.Maxlevel);
            hasValue2 = ServerData.sealSwordServerTable.GetWeaponEffectValue(this.sealSwordData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2);
            hasValue2_max = ServerData.sealSwordServerTable.GetWeaponEffectValue(this.sealSwordData.Stringid, effectData.Haseffectbase2, effectData.Haseffectvalue2, this.sealSwordData.Maxlevel);
            hasValue3 = ServerData.sealSwordServerTable.GetWeaponEffectValue(this.sealSwordData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3);
            hasValue3_max = ServerData.sealSwordServerTable.GetWeaponEffectValue(this.sealSwordData.Stringid, effectData.Haseffectbase3, effectData.Haseffectvalue3, this.sealSwordData.Maxlevel);
        }

        if (effectData.Equipeffecttype1 != -1)
        {
            StatusType type = (StatusType)effectData.Equipeffecttype1;

            if (type.IsPercentStat())
            {
                float value = equipValue1 * 100f;
                float value_max = equipValue1_max * 100f;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
            }
            else
            {
                float value = equipValue1;
                float value_max = equipValue1_max;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
            }
        }

        if (effectData.Equipeffecttype2 != -1)
        {
            StatusType type = (StatusType)effectData.Equipeffecttype2;

            if (type.IsPercentStat())
            {
                float value = equipValue2 * 100f;
                float value_max = equipValue2_max * 100f;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
            }
            else
            {
                float value = equipValue2;
                float value_max = equipValue2_max;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
            }
        }

        description += "\n<color=#ffff00ff>보유 효과</color>\n";

        if (effectData.Haseffecttype1 != -1)
        {
            StatusType type = (StatusType)effectData.Haseffecttype1;

            if (type.IsPercentStat())
            {
                float value = hasValue1 * 100f;
                float value_max = hasValue1_max * 100f;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
            }
            else
            {
                float value = hasValue1;
                float value_max = hasValue1_max;

                if (newGachaData != null)
                {
                    description += $"{CommonString.GetStatusName(type)} {(value.ToString())}\n";
                }
                else
                {
                    description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
                }
            }
        }

        if (effectData.Haseffecttype2 != -1)
        {
            StatusType type = (StatusType)effectData.Haseffecttype2;

            if (type.IsPercentStat())
            {
                float value = hasValue2 * 100f;
                float value_max = hasValue2_max * 100f;


                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
            }
            else
            {
                float value = hasValue2;
                float value_max = hasValue2_max;


                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}\n";
            }
        }

        if (effectData.Haseffecttype3 != -1)
        {
            StatusType type = (StatusType)effectData.Haseffecttype3;

            if (type.IsPercentStat())
            {
                float value = hasValue3 * 100f;
                float value_max = hasValue3_max * 100f;


                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}";
            }
            else
            {
                float value = hasValue3;
                float value_max = hasValue3_max;


                description += $"{CommonString.GetStatusName(type)} {Utils.ConvertBigNum(value)}";
            }
        }

        weaponAbilityDescription.SetText(description);
    }


    public void OnClickEquipButton()
    {
        if (weaponData != null)
        {

            if (weaponData.WEAPONTYPE == WeaponType.View ||weaponData.WEAPONTYPE == WeaponType.RecommendView || weaponData.WEAPONTYPE == WeaponType.HasEffectOnly)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 장착 하실수 없습니다.");
                return;
            }

            //현재 장비가 더 높음
            if (ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value > weaponData.Id)
            {
                PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "<color=red>현재 장착중인 무기보다 낮은 등급의 무기입니다.</color>\n정말로 무기를 변경 할까요?", () =>
                {
        
                    ServerData.equipmentTable.ChangeEquip(EquipmentTable.Weapon, weaponData.Id);
                    //ServerData.equipmentTable.ChangeEquip(EquipmentTable.Weapon_View, weaponData.Id);
                }, () => { });
            }
            else
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Weapon, weaponData.Id);
            }

        }
        else if (magicBookData != null)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 노리개를 변경 할까요?\n(외형도 함께 변경 됩니다.)", () =>
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook, magicBookData.Id);
                //ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook_View, magicBookData.Id);
            }, () => { });
        }
        else if (newGachaData != null)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 반지를 변경 할까요?", () => { ServerData.equipmentTable.ChangeEquip(EquipmentTable.SoulRing, newGachaData.Id); }, () => { });
        }
        else
        {

            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 무기를 변경 할까요?", () => { ServerData.equipmentTable.ChangeEquip(EquipmentTable.SealSword, sealSwordData.Id); }, () => { });
        }

        UpdateEquipButton();
    }

    private void UpdateEquipButton()
    {

        int id = this.weaponData != null ? weaponData.Id : this.magicBookData != null ? magicBookData.Id : newGachaData != null ? newGachaData.Id : sealSwordData.Id;

        int has = 0;

        if (weaponData != null)
        {
            has = ServerData.weaponTable.GetWeaponData(weaponData.Stringid).hasItem.Value;
        }
        else if (magicBookData != null)
        {
            has = ServerData.magicBookTable.GetMagicBookData(magicBookData.Stringid).hasItem.Value;
        }

        else if (newGachaData != null)
        {
            has = ServerData.newGachaServerTable.GetNewGachaData(newGachaData.Stringid).hasItem.Value;
        }
        else
        {
            has = ServerData.sealSwordServerTable.GetWeaponData(sealSwordData.Stringid).hasItem.Value;
        }

        SetEquipButton(has == 1);

        if (newGachaData == null && sealSwordData == null)
        {
            levelUpButton.gameObject.SetActive(has == 1);
        }

        if (equipButton.gameObject.activeSelf)
        {

            string key = weaponData != null ? EquipmentTable.Weapon : magicBookData != null ? EquipmentTable.MagicBook : newGachaData !=null ? EquipmentTable.SoulRing : EquipmentTable.SealSword;
            int equipIdx = ServerData.equipmentTable.TableDatas[key].Value;

            equipButton.interactable = equipIdx != id;
            //equipDescription.SetText(equipIdx == id ? "장착중" : "장착");
        }
        // ShowSubDetailView();
    }




    private bool showingPopup = false;


    public void OnClickLevelUpButton()
    {
        if (weaponData != null)
        {

            if (weaponData.WEAPONTYPE == WeaponType.View ||weaponData.WEAPONTYPE == WeaponType.RecommendView || weaponData.WEAPONTYPE == WeaponType.HasEffectOnly)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 레벨업 하실수 없습니다.");
                return;
            }


            float currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
            float levelUpPrice = ServerData.weaponTable.GetWeaponLevelUpPrice(weaponData.Stringid);

           
             
                if (ServerData.weaponTable.TableDatas[weaponData.Stringid].level.Value >= weaponData.Maxlevel)
                { 
                    if (weaponData.WEAPONTYPE == WeaponType.Normal)
                    {
                        if (ServerData.weaponTable.TableDatas[weaponData.Stringid].trans.Value > 0)
                        {
                            PopupManager.Instance.ShowAlarmMessage("이미 초월하였습니다.");
                            return;
                        }
                        else
                        {
                            if (ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value < weaponData.Transrequirevalue)
                            {
                                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TransGoods)}이 부족합니다!(초월동굴에서 획득)");
                                return;
                            }
                            //초월하겠습니까?
                            if(showingPopup==false)
                            {
                                showingPopup = true;
                                PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"초월하시겠습니까?",
                                    () => { TransEquipment(); }, () =>
                                    {
                                        showingPopup = false;
                                    });
                            }
                            return;
                        } 
                    }
                    else
                    {
                        PopupManager.Instance.ShowAlarmMessage("최대 레벨입니다.");
                        return;
                    }
            }
#if UNITY_EDITOR
            levelUpPrice = 0;
#endif
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
            //버프시간저장
            SaveManager.Instance.SyncBuffData();
            
            //서버에 반영
            SyncServerRoutineWeapon();
        }
        else if (magicBookData != null)
        {
            if (magicBookData.MAGICBOOKTYPE == MagicBookType.View)
            {
                PopupManager.Instance.ShowAlarmMessage("외형 아이템은 레벨업 하실수 없습니다.");
                return;
            }

            float currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
            float levelUpPrice = ServerData.magicBookTable.GetMagicBookLevelUpPrice(magicBookData.Stringid);


#if UNITY_EDITOR
            //levelUpPrice = 0;
#endif

            if (ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.Value >= magicBookData.Maxlevel)
            {
                PopupManager.Instance.ShowAlarmMessage("최대레벨 입니다.");
                return;
            }

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
        else
        {
        }
    }

    private void TransEquipment()
    {
        showingPopup= false;

        var require = weaponData.Transrequirevalue;

        if (weaponData.WEAPONTYPE != WeaponType.Normal)
        {
            PopupManager.Instance.ShowAlarmMessage("초월할 수 없는 장비입니다!");
            return;
        }
        
        //개수 체크
        if (ServerData.weaponTable.TableDatas[weaponData.Stringid].trans.Value > 0)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 초월하였습니다!");
            return;
        }
            
        if (ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value < require)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.TransGoods)}이 부족합니다!(초월동굴에서 획득)");
            return;
        }

        
        ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value -= require;
        ServerData.weaponTable.TableDatas[weaponData.Stringid].trans.Value = 1;
        //데이터 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        Param equipmentParam = new Param();

        //재화 차감
        goodsParam.Add(GoodsTable.TransGoods, ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        //레벨 상승
        string updateValue = ServerData.weaponTable.TableDatas[weaponData.Stringid].ConvertToString();
        equipmentParam.Add(weaponData.Stringid, updateValue);

        transactionList.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, equipmentParam));


        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            WhenItemLevelChanged(weaponData.Maxlevel);
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "초월 완료!!", null);
        });
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

    public void OnClickSinsuCreateButton()
    {
        if (magicBookData == null) return;

        UiNorigaeCraftBoard.Instance.Initialize(magicBookData.Id);
    }

    public void OnClickYoungMulCreateButton()
    {
        if (magicBookData == null) return;

        UiYoungMulCraftBoard.Instance.Initialize(magicBookData.Id);
    }

    public void OnClickYoungMulCreateButton2()
    {
        if (magicBookData == null) return;

        UiYoungMulCraftBoard2.Instance.Initialize(magicBookData.Id);
    }

    // private void OnEnable()
    // {
    //     SetParent();
    // }

    public void SetParent()
    {
        // //정렬
        // if (weaponData != null)
        // {
        //     //요물 야차만
        //     if (weaponData.Id >= 20)
        //     {
        //         if (ServerData.weaponTable.TableDatas[weaponData.Stringid].hasItem.Value == 1)
        //         {
        //             this.transform.SetAsFirstSibling();
        //         }
        //     }
        // }
        if (magicBookData != null)
        {
            if (ServerData.magicBookTable.TableDatas[magicBookData.Stringid].hasItem.Value == 1)
            {
                this.transform.SetAsFirstSibling();
            }
        }

        if (newGachaData != null)
        {
            if (ServerData.newGachaServerTable.TableDatas[newGachaData.Stringid].hasItem.Value == 1)
            {
                this.transform.SetAsFirstSibling();
            }
        } 
        
        if (sealSwordData != null)
        {
            if (ServerData.sealSwordServerTable.TableDatas[sealSwordData.Stringid].hasItem.Value == 1)
            {
                this.transform.SetAsFirstSibling();
            }
        }
    }

    public void OnClickGetFeelMul2Button()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.smithExp].Value < 400000)
        {
            PopupManager.Instance.ShowAlarmMessage("도깨비 대장간 레벨 40만 이상일때 획득 하실 수 있습니다.");
            return;
        }

        ServerData.weaponTable.TableDatas["weapon23"].amount.Value += 1;
        ServerData.weaponTable.TableDatas["weapon23"].hasItem.Value = 1;
        ServerData.weaponTable.SyncToServerEach("weapon23");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "무기 획득!", null);
    }

    public void OnClickGetFeelMul3Button()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value < 5000)
        {
            PopupManager.Instance.ShowAlarmMessage("검의 산 처치 5000 이상일때 획득 하실 수 있습니다.");
            return;
        }

        ServerData.weaponTable.TableDatas["weapon24"].amount.Value += 1;
        ServerData.weaponTable.TableDatas["weapon24"].hasItem.Value = 1;
        ServerData.weaponTable.SyncToServerEach("weapon24");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "무기 획득!", null);
    }

    public void OnClickGetFeelMulLastButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value < 6000)
        {
            PopupManager.Instance.ShowAlarmMessage("검의 산 처치 6000 이상일때 획득 하실 수 있습니다.");
            return;
        }

        ServerData.weaponTable.TableDatas["weapon25"].amount.Value += 1;
        ServerData.weaponTable.TableDatas["weapon25"].hasItem.Value = 1;
        ServerData.weaponTable.SyncToServerEach("weapon25");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "무기 획득!", null);
    }

    public void OnClickGetFeelMulLastLastButton()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value < 8000)
        {
            PopupManager.Instance.ShowAlarmMessage("검의 산 처치 8000 이상일때 획득 하실 수 있습니다.");
            return;
        }

        ServerData.weaponTable.TableDatas["weapon29"].amount.Value += 1;
        ServerData.weaponTable.TableDatas["weapon29"].hasItem.Value = 1;
        ServerData.weaponTable.SyncToServerEach("weapon29");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "필멸(패) 획득!", null);
    }

    public void OnClickGetGumihoWeaponButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.gumiho0).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho1).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho2).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho3).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho4).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho5).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho6).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho7).Value == 0 ||
            ServerData.goodsTable.GetTableData(GoodsTable.gumiho8).Value == 0
           )
        {
            PopupManager.Instance.ShowAlarmMessage("구미호전 구미호 꼬리 모두 획득시 획득 하실 수 있습니다.");
            return;
        }

        ServerData.weaponTable.TableDatas["weapon30"].amount.Value += 1;
        ServerData.weaponTable.TableDatas["weapon30"].hasItem.Value = 1;
        ServerData.weaponTable.SyncToServerEach("weapon30");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여우검 획득!", null);
    }

    public void OnClickGetGumihoNorigaeButton()
    {
        if (ServerData.goodsTable.GetTableData(GoodsTable.gumiho7).Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("구미호전 구미호 꼬리8 획득시 획득 하실 수 있습니다.");
            return;
        }

        ServerData.magicBookTable.TableDatas["magicBook28"].amount.Value += 1;
        ServerData.magicBookTable.TableDatas["magicBook28"].hasItem.Value = 1;
        ServerData.magicBookTable.SyncToServerEach("magicBook28");

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여우 노리개 획득!", null);
    }

    public void UpdateUi(WeaponData weaponData)
    {
        this.weaponData = weaponData;
        Initialize(weaponData: weaponData, null, null, null,null);
    }
    public override void UpdateContent(WeaponData_Fancy itemData)
    {
        if (this.itemData != null && this.itemData.WeaponData.Id == itemData.WeaponData.Id)
        {
            return;
        }

        this.itemData = itemData;

//        Debug.LogError("DolpasS!");
        if (itemData.WeaponData != null)
        {
            UpdateUi(this.itemData.WeaponData);
        }
    }

    float currentPosition = 0;
    [SerializeField] Animator animator = default;

    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }

    public override void UpdatePosition(float position)
    {
        currentPosition = position;

        if (animator.isActiveAndEnabled)
        {
            animator.Play(AnimatorHash.Scroll, -1, position);
        }

        animator.speed = 0;
    }

    void OnEnable() => UpdatePosition(currentPosition);
}