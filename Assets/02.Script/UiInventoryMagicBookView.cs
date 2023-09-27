using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using UnityEngine.UI.Extensions;

public class UiInventoryMagicBookView : FancyCell<MagicBook_Fancy>
{
    MagicBook_Fancy itemData;
    
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

    [SerializeField]
    private GameObject upgradeButton;

    [SerializeField]
    private TextMeshProUGUI weaponAbilityDescription;

    [SerializeField]
    private Button levelUpButton;

    [SerializeField]
    private TextMeshProUGUI levelUpPrice;

    [SerializeField]
    private Button equipButton;

    [SerializeField]
    private GameObject sinsuCreateButton;

    [SerializeField]
    private GameObject youngMulCreateButton;

    [SerializeField]
    private GameObject youngMulCreateButton2;


    [SerializeField]
    private Image magicBookViewEquipButton;

    [SerializeField]
    private TextMeshProUGUI magicBookViewEquipDesc;


    [SerializeField]
    private TextMeshProUGUI levelUpButtonText;
    [SerializeField]
    private Image upgradeGoodsImage;

    [SerializeField]
    private Sprite weaponViewEquipDisable;

    [SerializeField]
    private Sprite weaponViewEquipEnable;

    [SerializeField]
    private TextMeshProUGUI norigaeDescription;

    [SerializeField]
    private TextMeshProUGUI suhoSinDescription;

    [SerializeField]
    private GameObject foxNorigaeGetButton;
    private CompositeDisposable disposables = new CompositeDisposable();
    public MagicBookData GetMagicBookData()
    {
        return magicBookData;
    }
    
    private void OnDestroy()
    {
        disposables.Dispose();
    }
    private void SetEquipButton(bool onOff)
    {
        equipButton.gameObject.SetActive(onOff);

        if (magicBookData != null && magicBookData.MAGICBOOKTYPE == MagicBookType.View)
        {
            equipButton.gameObject.SetActive(false);
        }
    }
    private void SetTextLevelUpButton(int level)
    {
        if (level < magicBookData.Maxlevel)
        {
            levelUpButtonText.SetText("레벨업");
            upgradeGoodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(Item_Type.GrowthStone);
        }
        else
        {
            if (magicBookData.MAGICBOOKTYPE == MagicBookType.Normal)
            {
                if (ServerData.magicBookTable.TableDatas[magicBookData.Stringid].trans.Value > 0)
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
    public void OnClickMagicBookViewButton()
    {
        if (magicBookData != null)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "정말로 노리개 외형을 변경 할까요?", () =>
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook_View, magicBookData.Id);
            }, () => { });
            //   UiTutorialManager.Instance.SetClear(TutorialStep._10_EquipWeapon);
        }
    }

    public void Initialize(WeaponData weaponData, MagicBookData magicBookData, NewGachaTableData newGachaTableData, Action<WeaponData, MagicBookData> onClickCallBack)
    {
        this.weaponData = weaponData;
        this.magicBookData = magicBookData;
        this.newGachaData = newGachaTableData;

        this.onClickCallBack = onClickCallBack;
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

        if (magicBookData != null)
        {
            title.SetText(magicBookData.Name);
            weaponView.Initialize(null, magicBookData);
        }


        SubscribeWeapon();

        SetParent();
    }


    private void SubscribeWeapon()
    {
        disposables.Clear();
        if (magicBookData != null)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].AsObservable().Subscribe(WhenEquipMagicBookChanged).AddTo(disposables);
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].hasItem.AsObservable().Subscribe(WhenHasStageChanged).AddTo(disposables);
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].amount.AsObservable().Subscribe(WhenAmountChanged).AddTo(disposables);

            ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].AsObservable().Subscribe(WhenEquipMagicBook_ViewChanged).AddTo(disposables);
        }

        if (magicBookData != null)
        {
            ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.AsObservable().Subscribe(WhenItemLevelChanged).AddTo(disposables);
        }
    }



    private void WhenItemLevelChanged(int level)
    {
        SetCurrentWeapon();
        UpdateLevelUpUi();
        SetTextLevelUpButton(level);
    }

    private void UpdateLevelUpUi()
    {
        if (magicBookData == null) return;

        if ((magicBookData != null && ServerData.magicBookTable.TableDatas[magicBookData.Stringid].level.Value >= magicBookData.Maxlevel))
        {
            levelUpButton.interactable = false;
            if (magicBookData.MAGICBOOKTYPE== MagicBookType.Normal)
            {
                if (ServerData.magicBookTable.TableDatas[magicBookData.Stringid].trans.Value > 0)
                {
                    levelUpPrice.SetText("초월완료");
                    upgradeGoodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(Item_Type.TransGoods);
                }
                else
                {
                    levelUpPrice.SetText($"{magicBookData.Transrequirevalue}");
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

       if (magicBookData != null)
        {
            price = ServerData.magicBookTable.GetMagicBookLevelUpPrice(magicBookData.Stringid);
            currentMagicStoneAmount = ServerData.goodsTable.GetCurrentGoods(GoodsTable.GrowthStone);
        }
        

        levelUpPrice.SetText(Utils.ConvertBigNum(price));
        levelUpButton.interactable = currentMagicStoneAmount >= price;
    }

    private void WhenAmountChanged(int amount)
    {
        if (magicBookData != null)
        {
            upgradeButton.SetActive(amount >= magicBookData.Requireupgrade && magicBookData.Id < 15);
        }
    }

    private void WhenHasStageChanged(int state)
    {
        hasMask.SetActive(state == 0);

        SetEquipButton(state == 1);

        levelUpButton.gameObject.SetActive(state == 1);


        if (magicBookData != null)
        {
            hasMask.SetActive(state == 0);

            suhoSinDescription.SetText($"{magicBookData.Acquiredescription}");

            magicBookViewEquipButton.gameObject.SetActive(state == 1);
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
    public void OnClickIcon()
    {
        onClickCallBack?.Invoke(weaponData, magicBookData);
    }

    public void OnClickUpgradeButton()
    {

        if (magicBookData != null)
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
                
                itemData.ParentView.SortHasItem();
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
    }

    public void OnClickAllUpgradeButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "이전 단계 장비들도 전부 승급 합니까?", () =>
        {
           if (magicBookData != null)
            {
                itemData.ParentView.AllUpgradeMagicBook(magicBookData.Id);
                itemData.ParentView.SortHasItem();
            }

        }, null);
    }

    private void SetCurrentWeapon()
    {
        weaponView.Initialize(weaponData, magicBookData, null, newGachaData);

        SetWeaponAbilityDescription();
    }


    private void SetWeaponAbilityDescription()
    {
        WeaponEffectData effectData;
        string stringid;
        int weaponLevel = 0;

if (magicBookData != null)
        {
            effectData = TableManager.Instance.WeaponEffectDatas[this.magicBookData.Magicbookeffectid];
            weaponLevel = ServerData.magicBookTable.TableDatas[this.magicBookData.Stringid].level.Value;
            stringid = this.magicBookData.Stringid;
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
        if (magicBookData != null)
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
        else
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

        if (magicBookData != null)
        {
            
            
            //현재 장비가 더 높음
            if (ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value > magicBookData.Id)
            {
                PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "<color=red>현재 장착중인 노리개보다 낮은 등급의 노리개입니다.</color>\n정말로 노리개를 변경 할까요?", () =>
                {
        
                    ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook, magicBookData.Id);
                }, () => { });
            }
            else
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.MagicBook, magicBookData.Id);
            }
            
        }
        UpdateEquipButton();
    }

    private void UpdateEquipButton()
    {
        int id = this.weaponData != null ? weaponData.Id : this.magicBookData != null ? magicBookData.Id : newGachaData.Id;

        int has = 0;

        if (magicBookData != null)
        {
            has = ServerData.magicBookTable.GetMagicBookData(magicBookData.Stringid).hasItem.Value;
        }
        else
        {
            has = ServerData.newGachaServerTable.GetNewGachaData(newGachaData.Stringid).hasItem.Value;
        }

        SetEquipButton(has == 1);
        if (newGachaData == null)
        {
            levelUpButton.gameObject.SetActive(has == 1);
        }

        if (equipButton.gameObject.activeSelf)
        {
            string key = weaponData != null ? EquipmentTable.Weapon : magicBookData != null ? EquipmentTable.MagicBook : EquipmentTable.SoulRing;
            int equipIdx = ServerData.equipmentTable.TableDatas[key].Value;

            equipButton.interactable = equipIdx != id;
            //equipDescription.SetText(equipIdx == id ? "장착중" : "장착");
        }
        // ShowSubDetailView();
    }
    private bool showingPopup = false;

    public void OnClickLevelUpButton()
    {
       
        if (magicBookData != null)
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
                if (magicBookData.MAGICBOOKTYPE == MagicBookType.Normal)
                {
                    if (ServerData.magicBookTable.TableDatas[magicBookData.Stringid].trans.Value > 0)
                    {
                        PopupManager.Instance.ShowAlarmMessage("이미 초월하였습니다.");
                        return;
                    }
                    else
                    {
                        if (ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value < magicBookData.Transrequirevalue)
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
        //버프시간저장
        SaveManager.Instance.SyncBuffData();

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

    public void SetParent()
    {
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
    }

    private void TransEquipment()
    {
        showingPopup= false;

        var require = magicBookData.Transrequirevalue;

        if (magicBookData.MAGICBOOKTYPE != MagicBookType.Normal)
        {
            PopupManager.Instance.ShowAlarmMessage("초월할 수 없는 장비입니다!");
            return;
        }
        
        //개수 체크
        if (ServerData.magicBookTable.TableDatas[magicBookData.Stringid].trans.Value > 0)
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
        ServerData.magicBookTable.TableDatas[magicBookData.Stringid].trans.Value = 1;
        //데이터 싱크
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();
        Param equipmentParam = new Param();

        //재화 차감
        goodsParam.Add(GoodsTable.TransGoods, ServerData.goodsTable.GetTableData(GoodsTable.TransGoods).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        //레벨 상승
        string updateValue = ServerData.magicBookTable.TableDatas[magicBookData.Stringid].ConvertToString();
        equipmentParam.Add(magicBookData.Stringid, updateValue);

        transactionList.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, equipmentParam));


        ServerData.SendTransactionV2(transactionList, successCallBack: () =>
        {
            WhenItemLevelChanged(magicBookData.Maxlevel);
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "초월 완료!!", null);
        });
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

    public void UpdateUi(MagicBookData magicBookData)
    {
        this.magicBookData = magicBookData;
        Initialize(null, magicBookData, null, null);
    }
    public override void UpdateContent(MagicBook_Fancy itemData)
    {
        if (this.itemData != null && this.itemData.MagicBookData.Id == itemData.MagicBookData.Id)
        {
            return;
        }

        this.itemData = itemData;

//        Debug.LogError("DolpasS!");
        if (itemData.MagicBookData != null)
        {
            UpdateUi(this.itemData.MagicBookData);
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
