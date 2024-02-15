using System;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UiStatusBoard : MonoBehaviour
{
    [SerializeField]
    private UiStatusUpgradeCell statusCellPrefab;

    [SerializeField]
    private Transform goldAbilParent;
    [SerializeField]
    private Transform goldBarAbilParent;

    [SerializeField]
    private Transform skillPointAbilParent;

    [SerializeField]
    private Transform memoryParent;

    [SerializeField]
    private UiTopRankerCell topRankerCell;

    
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var e = TableManager.Instance.StatusDatas.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value.Active == false) continue;
            Transform cellParent = null;

            if (e.Current.Value.STATUSWHERE == StatusWhere.gold)
            {
                cellParent = goldAbilParent;
            }
            else if (e.Current.Value.STATUSWHERE == StatusWhere.goldbar)
            {
                cellParent = goldBarAbilParent;
            }
            else if (e.Current.Value.STATUSWHERE == StatusWhere.statpoint)
            {
                cellParent = skillPointAbilParent;
            }
            else if (e.Current.Value.STATUSWHERE == StatusWhere.memory)
            {
                cellParent = memoryParent;
            }

            var cell = MakeCell(cellParent);

            cell.Initialize(e.Current.Value);
        }
    }

    
    private UiStatusUpgradeCell MakeCell(Transform parent)
    {
        return Instantiate<UiStatusUpgradeCell>(statusCellPrefab, parent);
    }

    private void OnEnable()
    {
        this.transform.SetAsLastSibling();
        UpdatePlayerView();
    }

    private void OnDisable()
    {
        PlayerStats.ResetAbilDic();
    }

    private void UpdatePlayerView()
    {
        int costumeId = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petId = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponId = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].Value;
        int magicBookId = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook_View].Value;
        topRankerCell.Initialize(string.Empty, string.Empty, costumeId, petId, weaponId, magicBookId, ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].Value, string.Empty, ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].Value, ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].Value,ServerData.equipmentTable.TableDatas[EquipmentTable.SuhoAnimal].Value,-1);
    }

    public void OnClickStatResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "특수무공 능력치를 초기화 합니까?", () =>
        {
            ServerData.statusTable.GetTableData(StatusTable.IntLevelAddPer_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.CriticalLevel_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.CriticalDamLevel_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.HpPer_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.MpPer_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.Sin_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.Hyung_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.Chungu_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.Difficulty_StatPoint).Value = 1;
          
            
            ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value = (ServerData.statusTable.GetTableData(StatusTable.Level).Value - 1) * GameBalance.StatPoint;

            ServerData.statusTable.SyncAllData();
        }, null);


    }

    public void OnClickMemoryResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "비급 능력치를 초기화 합니까?", () =>
        {
            var pref = ServerData.statusTable.GetTableData(StatusTable.Memory).Value;

            string log = $"보유 {pref}";

            var tableData = TableManager.Instance.StatusTable.dataArray;

            var usedPoint = 0f;

            for (int i = 0; i < tableData.Length; i++)
            {
                if (tableData[i].STATUSWHERE != StatusWhere.memory) continue;

                usedPoint += (ServerData.statusTable.GetTableData(tableData[i].Key).Value);

                ServerData.statusTable.GetTableData(tableData[i].Key).Value = 0;
            }

            log += $"획득수량 {usedPoint}";

            ServerData.statusTable.GetTableData(StatusTable.Memory).Value = pref + usedPoint;

            log += $"최종 {ServerData.statusTable.GetTableData(StatusTable.Memory).Value}";

            ServerData.statusTable.SyncAllData();

            LogManager.Instance.SendLog("기억 능력치 초기화", log);
        }, null);
    }
    
    public void OnClickTransButton()
    {
        var att = ServerData.statusTable.GetTableData(StatusTable.AttackLevel_Gold).Value;
        var cri = ServerData.statusTable.GetTableData(StatusTable.CriticalLevel_Gold).Value;
        var criDam = ServerData.statusTable.GetTableData(StatusTable.CriticalDamLevel_Gold).Value;
        var hp = ServerData.statusTable.GetTableData(StatusTable.HpLevel_Gold).Value;
        var hpRec = ServerData.statusTable.GetTableData(StatusTable.HpRecover_Gold).Value;

        var gold = (int)(ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value / GameBalance.refundGoldBarRatio);
        
        var refund = criDam - GameBalance.criticalGraduateRefundStandard;
        
        var sum = att + cri + criDam +  hp + hpRec;
        int reqLv = GameBalance.goldGraduateScore;
        if (sum < reqLv)
        {
            PopupManager.Instance.ShowAlarmMessage(
                $"기본무공 강화 총 {reqLv} 레벨이 필요합니다!" +
                $"\n(현재 총 레벨 : {sum})");
        }
        else
        {
            
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice,
                "각성 하시겠습니까??\n" +
                $"각성 후 더 이상 {CommonString.GetItemName(Item_Type.Gold)}를 획득하실 수 없습니다\n" +
                $"각성 후 {CommonString.GetItemName(Item_Type.GoldBar)}를 획득하실 수 있습니다\n" +
                $"각성 후 획득한 {CommonString.GetItemName(Item_Type.GoldBar)}를통해 신규 능력치를 강화하실 수 있습니다", () =>
                {
                    string desc = "각성완료!!";
                    //환급
                    if (refund > 0)
                    {
                        refund *= GameBalance.refundCriDamGoldBarRatio;
                        desc += $"\n초과된 레벨이 {refund} {CommonString.GetItemName(Item_Type.GoldBar)}로 환산되었습니다!";
                    }
              

                    if (gold > 0)
                    {
                        desc += $"\n보유한 금화가 {gold} {CommonString.GetItemName(Item_Type.GoldBar)}로 환산되었습니다!";
                    }
                    
                    List<TransactionValue> transactionList = new List<TransactionValue>();

                    Param goodsParam = new Param();
                    ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value += Math.Max(0, refund) + Math.Max(gold, 0);
                    ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value = 0;
                    goodsParam.Add(GoodsTable.GoldBar, ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value);
                    goodsParam.Add(GoodsTable.Gold, ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value);
                    transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                    
                    
                    Param userInfo_2Param = new Param();
                    ServerData.userInfoTable_2.TableDatas[UserInfoTable_2.graduateGold].Value = 1;
                    userInfo_2Param.Add(UserInfoTable_2.graduateGold, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateGold).Value);
                    transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo_2Param));

                    Param statusParam = new Param();
                    ServerData.statusTable.GetTableData(StatusTable.CriticalDamLevel_Gold).Value = GameBalance.criticalGraduateValue;
                    ServerData.statusTable.GetTableData(StatusTable.AttackLevel_Gold).Value = GameBalance.attackGraduateValue;
                    statusParam.Add(StatusTable.CriticalDamLevel_Gold, ServerData.statusTable.GetTableData(StatusTable.CriticalDamLevel_Gold).Value);
                    statusParam.Add(StatusTable.AttackLevel_Gold, ServerData.statusTable.GetTableData(StatusTable.AttackLevel_Gold).Value);
                    transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));

                    ServerData.SendTransaction(transactionList, successCallBack: () =>
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, desc, null);
                    });
                    

                }, null);
        }

    }
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value++;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value += 10;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value += 100;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value += 1000;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value += 10000;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value += 100000;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value += 10000000;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.GoldBar).Value -= 10000000;
        }
    }
#endif
}
