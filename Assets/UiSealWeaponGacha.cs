using System;
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiGachaResultView;
using UniRx;

public class UiSealWeaponGacha : MonoBehaviour
{
    private List<SealSwordData> weaponDatas = new List<SealSwordData>();
    private List<float> probs = new List<float>();
    private List<GachaResultCellInfo> gachaResultCellInfos = new List<GachaResultCellInfo>();


    [SerializeField]
    private TextMeshProUGUI priceTexts;

    public int price = 1;

    [SerializeField]
    private TMP_InputField inputField;
    // [SerializeField]
    // private TextMeshProUGUI freeButtonDesc;

    // private void Subscribe()
    // {
    //     ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Subscribe(e =>
    //     {
    //         freeButtonDesc.SetText(e == 0 ? "무료 뽑기!" : "내일 다시!");
    //     }).AddTo(this);
    // }
    private void Start()
    {
        Initialize();
        //Subscribe();
    }

    private void Initialize()
    {
        //gachaNumTexts.SetText($"{gachaNum}번 소환");

      //  priceTexts.SetText($"{1}개");
    }


    private bool CanGacha(float price)
    {
        float currentBlueStoneNum = ServerData.goodsTable.GetTableData(GoodsTable.SealWeaponClear).Value;
        return currentBlueStoneNum >= price;
    }

    // public void OnClickFreeGacha()
    // {
    //     bool canFreeGacha = ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Value == 0;
    //
    //     if (canFreeGacha == false)
    //     {
    //         PopupManager.Instance.ShowAlarmMessage("오늘은 더이상 받을 수 없습니다.");
    //         return;
    //     }
    //
    //     AdManager.Instance.ShowRewardedReward(() =>
    //     {
    //         ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Value = 1;
    //
    //         List<TransactionValue> transactions = new List<TransactionValue>();
    //
    //         Param userInfoParam = new Param();
    //
    //         userInfoParam.Add(UserInfoTable.freeWeapon, ServerData.userInfoTable.GetTableData(UserInfoTable.freeWeapon).Value);
    //
    //         transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
    //
    //         ServerData.SendTransaction(transactions, successCallBack: () =>
    //         {
    //             this.lastGachaIdx = 2;
    //             int amount = gachaAmount[2];
    //             int price = gachaPrice[2];
    //
    //             //무료라
    //             ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += price;
    //
    //             OnClickOpenButton(2);
    //
    //            // LogManager.Instance.SendLogType("FreeGacha", "Weapon", "");
    //         });
    //
    //     });
    //
    //
    // }

    private float gachaProb0 = 0.4f;
    private float gachaProb1 = 0.3f;
    private float gachaProb2 = 0.2f;
    private float gachaProb3 = 0.1f;

    public void OnClickOpenButton()
    {
        int gachaLevel = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.currentFloorIdx9].Value -1;

        if (gachaLevel < 0)
        {
            gachaLevel = 0;
            PopupManager.Instance.ShowAlarmMessage("1단계를 클리어 해야 합니다.");
            return;
        }

        var gachaTableData = TableManager.Instance.SealTowerTable.dataArray[gachaLevel];
        
        int clearAmount = int.Parse(inputField.text); 
        
        int amount = gachaTableData.Gachacount * clearAmount;

        int price = clearAmount;

        //재화 체크
        if (CanGacha(price) == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SealWeaponClear)}이 부족합니다.");
            return;
        }

        weaponDatas.Clear();
        probs.Clear();
        gachaResultCellInfos.Clear();

        var weaponTable = TableManager.Instance.sealSwordTable.dataArray;

        for (int i = 0; i < weaponTable.Length; i++)
        {
            weaponDatas.Add(weaponTable[i]);
        }
        
        probs.Add(gachaTableData.Gachalv1 * gachaProb0);
        probs.Add(gachaTableData.Gachalv1 * gachaProb1);
        probs.Add(gachaTableData.Gachalv1 * gachaProb2);
        probs.Add(gachaTableData.Gachalv1 * gachaProb3);

        probs.Add(gachaTableData.Gachalv2 * gachaProb0);
        probs.Add(gachaTableData.Gachalv2 * gachaProb1);
        probs.Add(gachaTableData.Gachalv2 * gachaProb2);
        probs.Add(gachaTableData.Gachalv2 * gachaProb3);
        
        probs.Add(gachaTableData.Gachalv3 * gachaProb0);
        probs.Add(gachaTableData.Gachalv3 * gachaProb1);
        probs.Add(gachaTableData.Gachalv3 * gachaProb2);
        probs.Add(gachaTableData.Gachalv3 * gachaProb3);
        
        probs.Add(gachaTableData.Gachalv4 * gachaProb0);
        probs.Add(gachaTableData.Gachalv4 * gachaProb1);
        probs.Add(gachaTableData.Gachalv4 * gachaProb2);
        probs.Add(gachaTableData.Gachalv4 * gachaProb3);
        
        probs.Add(gachaTableData.Gachalv5 * gachaProb0);
        probs.Add(gachaTableData.Gachalv5 * gachaProb1);
        probs.Add(gachaTableData.Gachalv5 * gachaProb2);
        probs.Add(gachaTableData.Gachalv5 * gachaProb3);
        
        probs.Add(gachaTableData.Gachalv6 * gachaProb0);
        probs.Add(gachaTableData.Gachalv6 * gachaProb1);
        probs.Add(gachaTableData.Gachalv6 * gachaProb2);
        probs.Add(gachaTableData.Gachalv6 * gachaProb3);
        
        probs.Add(gachaTableData.Gachalv7 * gachaProb0);
        probs.Add(gachaTableData.Gachalv7 * gachaProb1);
        probs.Add(gachaTableData.Gachalv7 * gachaProb2);
        probs.Add(gachaTableData.Gachalv7 * gachaProb3);

        List<int> serverUpdateList = new List<int>();

        //로컬 데이터 갱신

        //재화
        ServerData.goodsTable.GetTableData(GoodsTable.SealWeaponClear).Value -= price;

        //가챠갯수
        //ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Weapon).Value += amount;

        //무기
        for (int i = 0; i < amount; i++)
        {
            int randomIdx = Utils.GetRandomIdx(probs);
            var cellInfo = new GachaResultCellInfo();

            cellInfo.amount = 1;
            cellInfo.sealSwordData = weaponDatas[randomIdx];
            gachaResultCellInfos.Add(cellInfo);

            ServerData.sealSwordServerTable.UpData(weaponDatas[randomIdx], cellInfo.amount);
            serverUpdateList.Add(weaponDatas[randomIdx].Id);
        }

        SyncServer(serverUpdateList, price, serverUpdateList.Count);
        
        UiGachaResultView_SealWeapon.Instance.Initialize(gachaResultCellInfos, () =>
        {
          //  OnClickOpenButton(lastGachaIdx);
        });
        
        
        SoundManager.Instance.PlaySound("Reward");

    }

    //서버 갱신만
    private void SyncServer(List<int> serverUpdateList, int price, int gachaCount)
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        //재화
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SealWeaponClear, ServerData.goodsTable.GetTableData(GoodsTable.SealWeaponClear).Value);

        // //가챠횟수
        // Param gachaNumParam = new Param();
        // gachaNumParam.Add(UserInfoTable.gachaNum_Weapon, ServerData.userInfoTable.GetTableData(UserInfoTable.gachaNum_Weapon).Value);

        //무기
        Param weaponParam = new Param();
        var table = TableManager.Instance.sealSwordTable.dataArray;
        var tableDatas = ServerData.sealSwordServerTable.TableDatas;
        
        for (int i = 0; i < table.Length; i++)
        {
            if (serverUpdateList != null && serverUpdateList.Contains(table[i].Id) == false) continue;

            string key = table[i].Stringid;
            //hasitem 1
            weaponParam.Add(key, tableDatas[key].ConvertToString());
        }
        //

        //재화
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        //가챠횟수
        //transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, gachaNumParam));
        //무기
        transactionList.Add(TransactionValue.SetUpdate(SealSwordServerTable.tableName, SealSwordServerTable.Indate, weaponParam));

        ServerData.SendTransaction(transactionList);
    }
    
    
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.TableDatas[GoodsTable.SealWeaponClear].Value += 10;
        }
    }
#endif
}