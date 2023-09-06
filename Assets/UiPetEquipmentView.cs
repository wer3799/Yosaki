using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiPetEquipmentView : MonoBehaviour
{
    [SerializeField]
    private GameObject unlockButton;

    [SerializeField]
    private GameObject levelUpButton;

    [SerializeField]
    private TextMeshProUGUI unlockButtonDesc;

    [SerializeField]
    private TextMeshProUGUI levelUpButtonDesc;

    [SerializeField]
    private TextMeshProUGUI levelUpButtonDesc_marble;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI equipTitle;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private Image itemIcon;

    private PetEquipmentData petEquipmentData;

    private PetEquipServerData petEquipServerData;

    private bool initialized = false;

    private ObscuredFloat marbleDiscountRatio = 0.8f;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI lockDescription;


    public void Initialize(PetEquipmentData petEquipmentData)
    {
        this.petEquipmentData = petEquipmentData;

        petEquipServerData = ServerData.petEquipmentServerTable.TableDatas[petEquipmentData.Stringid];

        unlockButtonDesc.SetText(Utils.ConvertBigNum(petEquipmentData.Unlockprice));

        itemIcon.sprite = CommonUiContainer.Instance.petEquipment[petEquipmentData.Id];



        equipTitle.color = CommonUiContainer.Instance.petEquipColor[petEquipmentData.Id];

        if (initialized == false)
        {
            initialized = true;
            Subscribe();
        }
    }

    private void Subscribe()
    {
        petEquipServerData.hasAbil.AsObservable().Subscribe(e =>
        {
            unlockButton.SetActive(e == 0);
            levelUpButton.SetActive(e == 1);
        }).AddTo(this);

        petEquipServerData.level.AsObservable().Subscribe(level =>
        {
            WhenLevelChanged(level);
        }).AddTo(this);

        ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).AsObservable().Subscribe(e =>
        {
            if (petEquipmentData.Id != 20)
            {
                equipTitle.SetText($"{petEquipmentData.Name}({e}강)");
            }
            else
            {
                equipTitle.SetText($"{petEquipmentData.Name}");
            }

            WhenLevelChanged(petEquipServerData.level.Value);


            if (petEquipmentData.Needpetawakelevel != 0)
            {
                lockMask.SetActive(e < petEquipmentData.Needpetawakelevel);
                lockDescription.SetText($"환수장비\n{petEquipmentData.Needpetawakelevel}강화 이상 개방");
            }
            else
            {
                lockMask.SetActive(false);
            }



        }).AddTo(this);
    }

    private void WhenLevelChanged(int level)
    {
        var petEquipLevel = ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value;

        levelText.SetText($"(LV:{level})");

        levelUpButtonDesc.SetText(level == petEquipmentData.Maxlevel ? "최고레벨" : Utils.ConvertBigNum(petEquipmentData.Upgradeprice));

        levelUpButtonDesc_marble.SetText(level == petEquipmentData.Maxlevel ? "최고레벨" : Utils.ConvertBigNum(petEquipmentData.Upgradeprice * marbleDiscountRatio));

        string abilDesc = string.Empty;

        StatusType type1 = (StatusType)petEquipmentData.Abiltype1;

        if (type1.IsPercentStat() == false)
        {
            float abilValue1 = petEquipmentData.Abilvalue1 + petEquipmentData.Abiladdvalue1 * level + petEquipmentData.Leveladdvalue1 * petEquipLevel;
            float maxAbil1 = petEquipmentData.Abilvalue1 + petEquipmentData.Abiladdvalue1 * petEquipmentData.Maxlevel;

            abilDesc += $"{CommonString.GetStatusName(type1)} {Utils.ConvertBigNum(abilValue1)}(<color=red>MAX:{Utils.ConvertBigNum(maxAbil1)}</color>)\n";
        }
        else
        {
            float abilValue1 = petEquipmentData.Abilvalue1 + petEquipmentData.Abiladdvalue1 * level + petEquipmentData.Leveladdvalue1 * petEquipLevel;
            float maxAbil1 = petEquipmentData.Abilvalue1 + petEquipmentData.Abiladdvalue1 * petEquipmentData.Maxlevel;

            abilDesc += $"{CommonString.GetStatusName(type1)} {Utils.ConvertBigNum(abilValue1 * 100f)}(<color=red>MAX:{Utils.ConvertBigNum(maxAbil1 * 100f)}</color>)\n";
        }

        StatusType type2 = (StatusType)petEquipmentData.Abiltype2;
        if (type2.IsPercentStat())
        {
            float abilValue2 = petEquipmentData.Abilvalue2 + petEquipmentData.Abiladdvalue2 * level + petEquipmentData.Leveladdvalue2 * petEquipLevel;
            float maxAbil2 = petEquipmentData.Abilvalue2 + petEquipmentData.Abiladdvalue2 * petEquipmentData.Maxlevel;

            abilDesc += $"{CommonString.GetStatusName(type2)} {Utils.ConvertBigNum(abilValue2 * 100f)}%(<color=red>MAX:{Utils.ConvertBigNum(maxAbil2 * 100f)}</color>)";
        }
        else
        {
            float abilValue2 = petEquipmentData.Abilvalue2 + petEquipmentData.Abiladdvalue2 * level + petEquipmentData.Leveladdvalue2 * petEquipLevel;
            float maxAbil2 = petEquipmentData.Abilvalue2 + petEquipmentData.Abiladdvalue2 * petEquipmentData.Maxlevel;

            abilDesc += $"{CommonString.GetStatusName(type2)} {Utils.ConvertBigNum(abilValue2)}(<color=red>MAX:{Utils.ConvertBigNum(maxAbil2)}</color>)";
        }

        if (petEquipmentData.Id != 20)
        {
            abilDescription.SetText(abilDesc);
        }
        else
        {
            abilDescription.SetText(string.Empty);
        }
    }

    public void OnClickUnlockButton()
    {
        var petTableData = TableManager.Instance.PetTable.dataArray[petEquipmentData.Requippetid];

        if (petEquipmentData.Id < 4)
        {
            //청룡
            if (petEquipmentData.Requippetid == 11)
            {
                if (ServerData.petTable.TableDatas[petTableData.Stringid].hasItem.Value == 0)
                {

                    var prefPetData = TableManager.Instance.PetEquipment.dataArray[petEquipmentData.Id - 1];
                    var prefPetEquipData = ServerData.petEquipmentServerTable.TableDatas[prefPetData.Stringid];

                    if (prefPetEquipData.level.Value < 1000)
                    {
                        PopupManager.Instance.ShowAlarmMessage($"청룡 보유중이거나(3단계 환수)\n{prefPetData.Name} LV:{1000}이상일때 제작 가능");
                        return;
                    }
                }
            }
            else
            {

                if (ServerData.petTable.TableDatas[petTableData.Stringid].hasItem.Value == 0)
                {
                    //주작 이하
                    if (petEquipmentData.Requippetid < 10)
                    {
                        PopupManager.Instance.ShowAlarmMessage($"{petTableData.Name} 보유중일때 획득 가능\n(3단계 환수)");
                        return;
                    }
                    else if (petEquipmentData.Requippetid == 10)
                    {
                        var prefPetData = TableManager.Instance.PetEquipment.dataArray[petEquipmentData.Id - 1];
                        var prefPetEquipData = ServerData.petEquipmentServerTable.TableDatas[prefPetData.Stringid];

                        if (prefPetEquipData.level.Value < 1000)
                        {
                            PopupManager.Instance.ShowAlarmMessage($"{petTableData.Name} 보유중이거나(3단계 환수)\n{prefPetData.Name} LV:{1000}이상일때 제작 가능");
                            return;
                        }
                    }
                }
            }
        }



        if (petEquipServerData.hasAbil.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유중입니다.");
            return;
        }

        float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value;

        if (currentMarble < petEquipmentData.Unlockprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= petEquipmentData.Unlockprice;

        petEquipServerData.hasAbil.Value = 1;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param petEquipParam = new Param();
        petEquipParam.Add(petEquipmentData.Stringid, petEquipServerData.ConvertToString());

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);

        transactions.Add(TransactionValue.SetUpdate(PetEquipmentServerTable.tableName, PetEquipmentServerTable.Indate, petEquipParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        //  LogManager.Instance.SendLogType("환수장비", petEquipmentData.Stringid, "g");

        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "환수 장비 획득!", null);
          });

    }

    public void OnClickLevelUpButton()
    {
        int currentLevel = petEquipServerData.level.Value;

        if (currentLevel >= petEquipmentData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고 레벨 입니다.");
            return;
        }

        float currentYoguiMarble = ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value;

        if (currentYoguiMarble < petEquipmentData.Upgradeprice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.PetUpgradeSoul)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value -= petEquipmentData.Upgradeprice;
        petEquipServerData.level.Value++;

        if (upgradeRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(upgradeRoutine);
        }

        upgradeRoutine = CoroutineExecuter.Instance.StartCoroutine(UpgradeRotuine());

    }
    private Coroutine upgradeRoutine;
    private WaitForSeconds upgradeDelay = new WaitForSeconds(0.5f);

    private IEnumerator UpgradeRotuine()
    {
        yield return upgradeDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param petEquipParam = new Param();
        petEquipParam.Add(petEquipmentData.Stringid, petEquipServerData.ConvertToString());

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.PetUpgradeSoul, ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value);

        transactions.Add(TransactionValue.SetUpdate(PetEquipmentServerTable.tableName, PetEquipmentServerTable.Indate, petEquipParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {

        });
    }


    public void OnClickLevelUpButton_Marble()
    {
        int currentLevel = petEquipServerData.level.Value;

        if (currentLevel >= petEquipmentData.Maxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최고 레벨 입니다.");
            return;
        }

        float currentMarble = ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value;

        float upgradePrice = petEquipmentData.Upgradeprice * marbleDiscountRatio;

        if (currentMarble < upgradePrice)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Marble)}이 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value -= upgradePrice;
        petEquipServerData.level.Value++;

        if (upgradeRoutine_marble != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(upgradeRoutine_marble);
        }

        upgradeRoutine_marble = CoroutineExecuter.Instance.StartCoroutine(UpgradeRotuine_Marble());
    }

    private Coroutine upgradeRoutine_marble;

    private IEnumerator UpgradeRotuine_Marble()
    {
        yield return upgradeDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param petEquipParam = new Param();
        petEquipParam.Add(petEquipmentData.Stringid, petEquipServerData.ConvertToString());

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);

        transactions.Add(TransactionValue.SetUpdate(PetEquipmentServerTable.tableName, PetEquipmentServerTable.Indate, petEquipParam));
        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {

        });
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.PetUpgradeSoul).Value += 1000000;
        }
    }
#endif
}
