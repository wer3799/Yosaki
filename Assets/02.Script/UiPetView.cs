using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Spine.Unity;
using BackEnd;

public enum PetGetType
{
    Ad, Gem, Shop, Evolution, Sasinsu,Sahyungsu,Vision,Event
}

public class UiPetView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private TextMeshProUGUI description1;

    [SerializeField]
    private TextMeshProUGUI description2;

    [SerializeField]
    private TextMeshProUGUI buttonDescription;

    [SerializeField]
    private TextMeshProUGUI petName;

    private PetTableData petData;
    private PetServerData petServerData;

    [SerializeField]
    private GameObject adIcon;

    [SerializeField]
    private GameObject gemIcon;

    [SerializeField]
    private Image button;

    [SerializeField]
    private Sprite normalButtonSprite;

    [SerializeField]
    private Sprite equipButtonSprite;

    [SerializeField]
    private GameObject tutorialObject;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI levelUpPrice_Soul;

    [SerializeField]
    private TextMeshProUGUI levelUpPrice_Marble;

    [SerializeField]
    private GameObject petAwakeButton;

    [SerializeField]
    private TextMeshProUGUI awakePrice;

    [SerializeField]
    private GameObject awakeButton;

    [SerializeField]
    private GameObject normalPetObject;

    [SerializeField]
    private GameObject leemugiPetObject;

    [SerializeField]
    private TextMeshProUGUI leemugiGetDescription;

    [SerializeField]
    private GameObject goldDragonPetObject;

    [SerializeField]
    private TextMeshProUGUI goldDragonGetDescription;
    //
    private void SetPetSpine(int idx)
    {
        skeletonGraphic.Clear();

        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[idx];

        if (idx >= 24 && idx <= 27)
        {
            skeletonGraphic.startingAnimation = "idle";

        }
        if (idx >= 32 && idx <= 35)
        {
            skeletonGraphic.startingAnimation = "idle";

        }
        else if (idx != 15)
        {
            skeletonGraphic.startingAnimation = "walk";
        }
        else
        {
            skeletonGraphic.startingAnimation = "idel";
        }

        //백호각성
        if (idx == 25 || idx == 26)
        {
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -257f, 0f);
        }
        //극사신수각성
        else if (idx == 48)
        {
            skeletonGraphic.transform.localPosition = new Vector3(45f, -229f, 0f);
        }
        else if (idx == 49)
        {
            skeletonGraphic.transform.localPosition = new Vector3(19f, -275f, 0f);
        }
        else if (idx == 50)
        {
            skeletonGraphic.transform.localPosition = new Vector3(58.78f, -296.7f, 0f);
        }
        else if (idx == 51)
        {
            skeletonGraphic.transform.localPosition = new Vector3(54f, -188f, 0f);
        }

        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }

    private bool initialized = false;

    public void Initialize(PetTableData petData)
    {
        this.petData = petData;

        this.petServerData = ServerData.petTable.TableDatas[petData.Stringid];

        SetPetSpine(petData.Id);

        if (petName != null)
        {
            petName.color = CommonUiContainer.Instance.itemGradeColor[(petData.Id / 4) + 1];
        }

        awakePrice.SetText(Utils.ConvertBigNum(petData.Awakeprice));

        UpdateUi();

        if (initialized == false)
        {
            initialized = true;
            Subscribe();
        }

        SetAbilityText();

        tutorialObject.SetActive(petData.PETGETTYPE == PetGetType.Gem && petData.Price == 0f);

        normalPetObject.SetActive(petData.Id != 12 || petData.Id != 13);

        if (leemugiPetObject != null)
            leemugiPetObject.SetActive(petData.Id == 12);

        if (goldDragonPetObject != null)
            goldDragonPetObject.SetActive(petData.Id == 13);

        if (petData.Id == 12)
        {
            normalPetObject.SetActive(ServerData.petTable.TableDatas["pet12"].hasItem.Value == 1);
            leemugiGetDescription.SetText($"야차 지배 LV :{GameBalance.LeeMuGiGetLevel}에 획득 가능");
        }

        if (petData.Id == 13)
        {
            normalPetObject.SetActive(ServerData.petTable.TableDatas["pet13"].hasItem.Value == 1);
            goldDragonGetDescription.SetText($"필멸 여의주 LV :{GameBalance.GoldGetLevel}에 획득 가능");
        }

    }

    private void SetAbilityText()
    {
        string abilityStr = string.Empty;

        int currentLevel = ServerData.petTable.TableDatas[petData.Stringid].level.Value;
        int maxLevel = petData.Maxlevel;
        int petAwakeLevel = ServerData.statusTable.GetTableData(StatusTable.PetAwakeLevel).Value;

        if (petData.Hasvalue1 != 0f)
        {
            StatusType statusType = (StatusType)petData.Hastype1;
            bool isPercent = statusType.IsPercentStat();

            float value = petData.Hasvalue1 + currentLevel * petData.Hasaddvalue1;

            if (statusType.IsAbsoluteStatus()==false)
                value += value * ((float)petAwakeLevel * GameBalance.PetAwakeValuePerLevel);

            if (petData.Id < 12)
            {
                abilityStr += $"{CommonString.GetStatusName(statusType)} : {Utils.ConvertBigNum((value) * (isPercent ? 100 : 1f))}(<color=red>MAX:{Utils.ConvertBigNum((petData.Hasvalue1 + maxLevel * petData.Hasaddvalue1) * (isPercent ? 100 : 1f))}</color>)\n";
            }
            else
            {
                abilityStr += $"{CommonString.GetStatusName(statusType)} : {Utils.ConvertBigNum((value) * (isPercent ? 100 : 1f))}\n";
            }
        }

        if (petData.Hasvalue2 != 0f)
        {
            StatusType statusType = (StatusType)petData.Hastype2;
            bool isPercent = statusType.IsPercentStat();

            float value = petData.Hasvalue2 + currentLevel * petData.Hasaddvalue2;

            if (statusType.IsAbsoluteStatus()==false)
                value += value * ((float)petAwakeLevel * GameBalance.PetAwakeValuePerLevel);

            if (petData.Id < 12)
            {
                abilityStr += $"{CommonString.GetStatusName(statusType)} : {Utils.ConvertBigNum((value) * (isPercent ? 100 : 1f))}(<color=red>MAX:{Utils.ConvertBigNum((petData.Hasvalue2 + maxLevel * petData.Hasaddvalue2) * (isPercent ? 100 : 1f))}</color>)\n";
            }
            else
            {
                abilityStr += $"{CommonString.GetStatusName(statusType)} : {Utils.ConvertBigNum((value) * (isPercent ? 100 : 1f))}\n";
            }
        }

        if (petData.Hasvalue3 != 0f)
        {
            StatusType statusType = (StatusType)petData.Hastype3;
            bool isPercent = statusType.IsPercentStat();

            float value = petData.Hasvalue3 + currentLevel * petData.Hasaddvalue3;

            if (statusType.IsAbsoluteStatus()==false)
                value += value * ((float)petAwakeLevel * GameBalance.PetAwakeValuePerLevel);

            if (petData.Id < 12)
            {
                abilityStr += $"{CommonString.GetStatusName(statusType)} : {Utils.ConvertBigNum((value) * (isPercent ? 100 : 1f))}(<color=red>MAX:{Utils.ConvertBigNum((petData.Hasvalue3 + maxLevel * petData.Hasaddvalue3) * (isPercent ? 100 : 1f))}</color>)\n";
            }
            else
            {
                abilityStr += $"{CommonString.GetStatusName(statusType)} : {Utils.ConvertBigNum((value) * (isPercent ? 100 : 1f))}\n";
            }
        }

        if (petData.Hasvalue4 != 0f)
        {
            StatusType statusType = (StatusType)petData.Hastype4;
            bool isPercent = statusType.IsPercentStat();

            float value = petData.Hasvalue4 + currentLevel * petData.Hasaddvalue4;

            if (statusType.IsAbsoluteStatus()==false)
                value += value * ((float)petAwakeLevel * GameBalance.PetAwakeValuePerLevel);

            if (petData.Id < 12)
            {
                abilityStr += $"{CommonString.GetStatusName(statusType)} : {Utils.ConvertBigNum((value) * (isPercent ? 100 : 1f))}(<color=red>MAX:{Utils.ConvertBigNum((petData.Hasvalue4 + maxLevel * petData.Hasaddvalue4) * (isPercent ? 100 : 1f))}</color>)";
            }
            else
            {
                abilityStr += $"{CommonString.GetStatusName(statusType)} : {Utils.ConvertBigNum((value) * (isPercent ? 100 : 1f))}";
            }
        }


        if (abilityStr.Equals(string.Empty))
        {
            abilityStr = "효과 없음";
        }

        description2.SetText(abilityStr);
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].AsObservable().Subscribe(e =>
        {
            UpdateUi();
        }).AddTo(this);

        petServerData.hasItem.AsObservable().Subscribe(e =>
        {
            if (e == 1)
            {
                UpdateUi();
            }
            string defix = e == 1 ? "" : "(미보유)";

            if (petName != null)
            {
                petName.SetText($"{petData.Name}{defix}");
            }
        }).AddTo(this);

        petServerData.level.AsObservable().Subscribe(e =>
        {
            if (e == petData.Maxlevel)
            {
                levelText.SetText($"LV : {petData.Maxlevel}(MAX)");

                levelUpPrice_Soul.SetText("최대레벨");

                levelUpPrice_Marble.SetText("최대레벨");
            }
            else
            {
                levelText.SetText($"(LV : {e})");

                levelUpPrice_Soul.SetText($"{Utils.ConvertBigNum(GetPetLevelUpPrice_Soul(e))}\n레벨업");

                levelUpPrice_Marble.SetText($"{Utils.ConvertBigNum(GetPetLevelUpPrice_Marble(e))}\n레벨업");
            }

            SetAbilityText();

        }).AddTo(this);

        ServerData.statusTable.GetTableData(StatusTable.PetAwakeLevel).AsObservable().Subscribe(e =>
        {

            SetAbilityText();

        }).AddTo(this);

        if (petData.Nextpetid != -1)
        {
            var nextPetTableData = TableManager.Instance.PetTable.dataArray[petData.Nextpetid];

            var newPetServerData = ServerData.petTable.TableDatas[nextPetTableData.Stringid];

            newPetServerData.hasItem.Subscribe(e =>
            {
                awakeButton.SetActive(e == 0);
            }).AddTo(this);

        }

        if (petData.Id == 12)
        {
            ServerData.petTable.TableDatas["pet12"].hasItem.AsObservable().Subscribe(e =>
            {
                leemugiGetDescription.gameObject.SetActive(e == 0);
                normalPetObject.SetActive(ServerData.petTable.TableDatas["pet12"].hasItem.Value == 1);
            }).AddTo(this);
        }

        if (petData.Id == 13)
        {
            ServerData.petTable.TableDatas["pet13"].hasItem.AsObservable().Subscribe(e =>
            {
                goldDragonGetDescription.gameObject.SetActive(e == 0);
                normalPetObject.SetActive(ServerData.petTable.TableDatas["pet13"].hasItem.Value == 1);
            }).AddTo(this);
        }

    }

    private void UpdateUi()
    {
        if (petData.Nextpetid != -1)
        {
            var nextPetTableData = TableManager.Instance.PetTable.dataArray[petData.Nextpetid];

            var newPetServerData = ServerData.petTable.TableDatas[nextPetTableData.Stringid];

            petAwakeButton.SetActive(newPetServerData.hasItem.Value == 0);
        }
        else
        {
            petAwakeButton.SetActive(false);
        }

        adIcon.SetActive(petData.PETGETTYPE == PetGetType.Ad);
        gemIcon.SetActive(petData.PETGETTYPE == PetGetType.Gem && ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value != 1);

        button.sprite = normalButtonSprite;

        //장착중
        if (ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value == petData.Id)
        {
            if (petData.PETGETTYPE != PetGetType.Ad)
            {
                buttonDescription.SetText("장착중");
                button.sprite = equipButtonSprite;
                return;
            }
            //광고펫
            else
            {
                //시간남아있음
                if (petServerData.remainSec.Value > 0)
                {
                    buttonDescription.SetText($"장착중\n{petServerData.remainSec.Value / 60}분 남음");
                    button.sprite = equipButtonSprite;
                    return;
                }
                //시간 다씀
                else
                {
                    buttonDescription.SetText($"광고보고 대여\n({petData.Time / 60}분)");
                    return;
                }
            }
        }

        //무료펫
        if (petData.PETGETTYPE == PetGetType.Ad)
        {
            //보유
            if (petServerData.hasItem.Value == 1 && petServerData.remainSec.Value > 0)
            {
                buttonDescription.SetText("장착하기");
            }
            //미보유
            else
            {
                buttonDescription.SetText($"광고보고 대여\n({petData.Time / 60}분)");
            }
        }
        //유료펫
        else if (petData.PETGETTYPE == PetGetType.Gem)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                buttonDescription.SetText("장착하기");
            }
            //미보유
            else
            {
                if (petData.Price == 0f)
                {
                    buttonDescription.SetText("무료");
                }
                else
                {
                    buttonDescription.SetText($"{Utils.ConvertBigNum(petData.Price)}");
                }
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Shop)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                buttonDescription.SetText("장착하기");
            }
            //미보유
            else
            {
                buttonDescription.SetText($"{petData.Getspecialdescription}");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Evolution)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                buttonDescription.SetText("장착하기");
            }
            //미보유
            else
            {
                int prefPetId = petData.Id - 4;

                var prefPetData = TableManager.Instance.PetTable.dataArray[prefPetId];

                buttonDescription.SetText($"{petData.Getspecialdescription}");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Sasinsu)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                buttonDescription.SetText("장착하기");
            }
            //미보유
            else
            {
                buttonDescription.SetText($"{petData.Getspecialdescription}");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Sahyungsu)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                buttonDescription.SetText("장착하기");
            }
            else
            {
                buttonDescription.SetText($"{petData.Getspecialdescription}");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Vision)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                buttonDescription.SetText("장착하기");
            }
            else
            {
                buttonDescription.SetText($"{petData.Getspecialdescription}");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Event)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                buttonDescription.SetText("장착하기");
            }
            else
            {
                buttonDescription.SetText($"{petData.Getspecialdescription}");
            }
        }

    }

    public void OnClickGetButton()
    {
        //무료펫
        if (petData.PETGETTYPE == PetGetType.Ad)
        {
            //보유
            if (petServerData.hasItem.Value == 1 && petServerData.remainSec.Value > 0)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                BuyPetRoutine();
            }
        }
        //유료펫
        else if (petData.PETGETTYPE == PetGetType.Gem)
        {
            //보유
            if (petServerData.hasItem.Value == 1)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                float currentBlueStone = ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value;

                if (currentBlueStone >= petData.Price)
                {
                    BuyPetRoutine();
                }
                else
                {
                    PopupManager.Instance.ShowAlarmMessage($"{petData.Onclickbuttondescription}");
                }
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Shop)
        {
            if (petServerData.hasItem.Value == 1)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                PopupManager.Instance.ShowAlarmMessage($"{petData.Onclickbuttondescription}");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Evolution)
        {
            if (petServerData.hasItem.Value == 1)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                    PopupManager.Instance.ShowAlarmMessage($"{petData.Onclickbuttondescription}");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Sasinsu)
        {
            if (petServerData.hasItem.Value == 1)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                //진
                if (petServerData.idx == 24 || petServerData.idx == 25 || petServerData.idx == 26 || petServerData.idx == 27)
                {

                    if (petServerData.idx == 24)
                    {
                        if (ServerData.petTable.TableDatas["pet8"].hasItem.Value != 1)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"{TableManager.Instance.PetTable.dataArray[8].Name}이 필요합니다.");
                            return;
                        }
                    }
                    else if (petServerData.idx == 25)
                    {
                        if (ServerData.petTable.TableDatas["pet9"].hasItem.Value != 1)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"{TableManager.Instance.PetTable.dataArray[9].Name}가 필요합니다.");
                            return;
                        }
                    }
                    else if (petServerData.idx == 26)
                    {
                        if (ServerData.petTable.TableDatas["pet10"].hasItem.Value != 1)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"{TableManager.Instance.PetTable.dataArray[10].Name}이 필요합니다.");
                            return;
                        }
                    }
                    else if (petServerData.idx == 27)
                    {
                        if (ServerData.petTable.TableDatas["pet11"].hasItem.Value != 1)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"{TableManager.Instance.PetTable.dataArray[11].Name}이 필요합니다.");
                            return;
                        }
                    }
                   
                    if (ServerData.sasinsuServerTable.TableDatas[$"b{petServerData.idx - 24}"].score.Value < TableManager.Instance.sasinsuTable.dataArray[petServerData.idx - 24].Score[6])
                    {
                        PopupManager.Instance.ShowAlarmMessage($"별자리 컨텐츠 {TableManager.Instance.sasinsuTable.dataArray[petServerData.idx - 24].Name}에서 마지막 별자리 달성시 획득 가능!");
                        return;
                    }
                    else
                    {
                        BuyPetRoutine();
                    }
                }
                //극
                else if (petServerData.idx == 48 || petServerData.idx == 49 || petServerData.idx == 50 ||
                         petServerData.idx == 51)
                {
                    if (petServerData.idx == 48)
                    {
                        if (ServerData.petTable.TableDatas["pet24"].hasItem.Value != 1)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"{TableManager.Instance.PetTable.dataArray[24].Name}이 필요합니다.");
                            return;
                        }

                        if (ServerData.statusTable.GoldBarUpgradeSum() <GameBalance.goldBarBlack)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"백금화 강화 {Utils.ConvertNum(GameBalance.goldBarBlack)}이 필요합니다.");
                            return;
                        }
                    }
                    else if (petServerData.idx == 49)
                    {
                        if (ServerData.petTable.TableDatas["pet25"].hasItem.Value != 1)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"{TableManager.Instance.PetTable.dataArray[25].Name}이 필요합니다.");
                            return;
                        }
                        
                        if (ServerData.statusTable.GoldBarUpgradeSum() <GameBalance.goldBarWhite)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"백금화 강화 {Utils.ConvertNum(GameBalance.goldBarWhite)}이 필요합니다.");
                            return;
                        }
                    }
                    else if (petServerData.idx == 50)
                    {
                        if (ServerData.petTable.TableDatas["pet26"].hasItem.Value != 1)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"{TableManager.Instance.PetTable.dataArray[26].Name}이 필요합니다.");
                            return;
                        }
                        
                        if (ServerData.statusTable.GoldBarUpgradeSum() <GameBalance.goldBarRed)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"백금화 강화 {Utils.ConvertNum(GameBalance.goldBarRed)}이 필요합니다.");
                            return;
                        }
                    }
                    else if (petServerData.idx == 51)
                    {
                        if (ServerData.petTable.TableDatas["pet27"].hasItem.Value != 1)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"{TableManager.Instance.PetTable.dataArray[27].Name}이 필요합니다.");
                            return;
                        }
                        
                        if (ServerData.statusTable.GoldBarUpgradeSum() <GameBalance.goldBarBlue)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"백금화 강화 {Utils.ConvertNum(GameBalance.goldBarBlue)}이 필요합니다.");
                            return;
                        }
                    }
                    
                    BuyPetRoutine();
                }
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Sahyungsu)
        {
            if (petServerData.hasItem.Value == 1)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
               PopupManager.Instance.ShowAlarmMessage($"사흉수 컨텐츠 {petData.Name}에서 획득 가능!");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Vision)
        {
            if (petServerData.hasItem.Value == 1)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
               PopupManager.Instance.ShowAlarmMessage($"사룡전 컨텐츠 {petData.Name}에서 획득 가능!");
            }
        }
        else if (petData.PETGETTYPE == PetGetType.Event)
        {
            if (petServerData.hasItem.Value == 1)
            {
                ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            }
            //미보유
            else
            {
                PopupManager.Instance.ShowAlarmMessage($"{petData.Onclickbuttondescription}");
            }
        }
        UpdateUi();
    }

    private void BuyPetRoutine()
    {
        UiTutorialManager.Instance.SetClear(TutorialStep.GetPet);

        //무료펫
        if (petData.PETGETTYPE == PetGetType.Ad)
        {
            AdManager.Instance.ShowRewardedReward(BuyFreePet);
        }
        //유료펫
        else if (petData.PETGETTYPE == PetGetType.Gem)
        {
            ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value = 1;
            ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= petData.Price;
            //
            List<TransactionValue> transactionList = new List<TransactionValue>();

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
            transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param petParam = new Param();
            petParam.Add(petData.Stringid, ServerData.petTable.TableDatas[petData.Stringid].ConvertToString());
            transactionList.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

            ServerData.SendTransaction(transactionList, successCallBack: UpdateUi);
            ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
        }
        //사신수
        else if (petData.PETGETTYPE == PetGetType.Sasinsu)
        {

            ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value = 1;

            List<TransactionValue> transactionList = new List<TransactionValue>();

            Param petParam = new Param();
            petParam.Add(petData.Stringid, ServerData.petTable.TableDatas[petData.Stringid].ConvertToString());
            transactionList.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

            ServerData.SendTransactionV2(transactionList, successCallBack: UpdateUi);
            ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
            
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice,$"{petData.Name} 획득!",null);
        }
    }

    public void BuyFreePet()
    {
        var localTableData = TableManager.Instance.PetDatas[petData.Id];
        ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value = 1;
        ServerData.petTable.TableDatas[petData.Stringid].remainSec.Value = (int)petData.Time;
        ServerData.petTable.UpdateData(petData.Stringid);
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.Pet, petData.Id);
        PlayerPet.Instance.WhenPetEquipIdxChanged(0);
        UpdateUi();
    }

    public void OnClickLevelUpButton_Marble()
    {
        if (ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("환수가 없습니다.");
            return;
        }

        int currentLevel = ServerData.petTable.TableDatas[petData.Stringid].level.Value;
        int maxLevel = petData.Maxlevel;

        if (currentLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 레벨 입니다.");
            return;
        }

        int currentMarble = (int)ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value;
        int price_Marble = GetPetLevelUpPrice_Marble(currentLevel);

        if (currentMarble < price_Marble)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        //로컬데이터
        ServerData.petTable.TableDatas[petData.Stringid].level.Value++;
        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= price_Marble;

        if (serverSyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(serverSyncRoutine);
        }

        serverSyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncServerData());
    }


    public void OnClickLevelUpButton_Soul()
    {
        if (ServerData.petTable.TableDatas[petData.Stringid].hasItem.Value == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("환수가 없습니다.");
            return;
        }

        int currentLevel = ServerData.petTable.TableDatas[petData.Stringid].level.Value;
        int maxLevel = petData.Maxlevel;

        if (currentLevel >= maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 레벨 입니다.");
            return;
        }

        int currentSoul = (int)ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value;
        int price_Soul = GetPetLevelUpPrice_Soul(currentLevel);

        if (currentSoul < price_Soul)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.PetUpgradeSoul)}이 부족합니다.");
            return;
        }

        //로컬데이터
        ServerData.petTable.TableDatas[petData.Stringid].level.Value++;
        ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value -= price_Soul;

        if (serverSyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(serverSyncRoutine);
        }

        serverSyncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncServerData());
    }

    //서버 싱크

    private Coroutine serverSyncRoutine;

    private IEnumerator SyncServerData()
    {
        yield return new WaitForSeconds(1.0f);

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
        goodsParam.Add(GoodsTable.PetUpgradeSoul, ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value);

        Param petParam = new Param();
        petParam.Add(petData.Stringid, ServerData.petTable.TableDatas[petData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

        ServerData.SendTransaction(transactions);
    }

    public void OnClickAwakeButton()
    {

        var nextPetTableData = TableManager.Instance.PetTable.dataArray[petData.Nextpetid];
        var newPetServerData = ServerData.petTable.TableDatas[nextPetTableData.Stringid];

        if (newPetServerData.hasItem.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 각성 했습니다.");
            return;
        }

        int currentLevel = ServerData.petTable.TableDatas[petData.Stringid].level.Value;
        int maxLevel = petData.Maxlevel;

        if (currentLevel < maxLevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 레벨이 아닙니다.");
            return;
        }

        int currentMarble = (int)ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value;
        int price_Marble = petData.Awakeprice;

        if (currentMarble < price_Marble)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= price_Marble;


        newPetServerData.hasItem.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);

        Param petParam = new Param();
        petParam.Add(nextPetTableData.Stringid, ServerData.petTable.TableDatas[nextPetTableData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{nextPetTableData.Name} 획득!", null);

              //   LogManager.Instance.SendLogType("Pet", "각성", $"{nextPetTableData.Name }");
          });
    }


    public int GetPetLevelUpPrice_Soul(int level)
    {
        int ret = GetPetLevelUpPrice_Marble(level);
        return ret + (int)((float)ret * 0.3f);
    }

    public int GetPetLevelUpPrice_Marble(int level)
    {
        return (int)Mathf.Pow(level + 1, petData.Upgradepowvalue);
    }

    public void GetLeeMuGiButton()
    {
        int yachaZiBaeLevel = ServerData.statusTable.GetTableData(StatusTable.PetAwakeLevel).Value;

        if (yachaZiBaeLevel < GameBalance.LeeMuGiGetLevel)
        {
            PopupManager.Instance.ShowAlarmMessage($"야차 지배 LV:{GameBalance.LeeMuGiGetLevel}에 획득할 수 있습니다.");
            return;
        }

        ServerData.petTable.TableDatas["pet12"].hasItem.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param petParam = new Param();

        petParam.Add("pet12", ServerData.petTable.TableDatas["pet12"].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "이무기 획득!", null);
          });

    }

    public void GetGoldDragonButton()
    {
        int LeeMugiLevel = ServerData.statusTable.GetTableData(StatusTable.LeeMuGi).Value;

        if (LeeMugiLevel < GameBalance.GoldGetLevel)
        {
            PopupManager.Instance.ShowAlarmMessage($"필멸 여의주 LV:{GameBalance.GoldGetLevel}에 획득할 수 있습니다.");
            return;
        }

        ServerData.petTable.TableDatas["pet13"].hasItem.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param petParam = new Param();

        petParam.Add("pet13", ServerData.petTable.TableDatas["pet13"].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "황룡 획득!", null);
        });

    }
}
