using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using BackEnd;

public class UiMileageRefund : MonoBehaviour
{
    [SerializeField]
    private UiText uiText;

    [SerializeField]
    private Transform parents;

    [SerializeField]
    private GameObject rootObject;


    void Start()
    {
        RefundRoutine();
        
        DolPassRefundRoutine();
        NewGachaRefundRoutine();
        TitleRefundRoutine();
        ChunmaDokebiFireRefundRoutine();
        TitleRefundRoutine2();
        RelocateLevelPass();
        TowerFloorAdjust();
        
        CommonAttendEventStart();
        InitializeXMasEvent();
        InitializeCollectionEvent();
        ShopItemRefundRoutine();
    }
    private void InitializeCollectionEvent()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.collectionEventInitialize).Value > 0)
            return;

        ServerData.userInfoTable.GetTableData(UserInfoTable.collectionEventInitialize).Value = 1;
        ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_1).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_2).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_3).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_4).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.usedCollectionCount).Value = 0;

        ServerData.oneYearPassServerTable.TableDatas["f5"].Value = string.Empty;
        ServerData.oneYearPassServerTable.TableDatas["a5"].Value = string.Empty;
        
        
        
        List<TransactionValue> transactions = new List<TransactionValue>();
        
        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.collectionEventInitialize, ServerData.userInfoTable.GetTableData(UserInfoTable.collectionEventInitialize).Value);
        userInfoParam.Add(UserInfoTable.exchangeCount, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount).Value);
        userInfoParam.Add(UserInfoTable.exchangeCount_1, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_1).Value);
        userInfoParam.Add(UserInfoTable.exchangeCount_2, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_2).Value);
        userInfoParam.Add(UserInfoTable.exchangeCount_3, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_3).Value);
        userInfoParam.Add(UserInfoTable.exchangeCount_4, ServerData.userInfoTable.GetTableData(UserInfoTable.exchangeCount_4).Value);
        userInfoParam.Add(UserInfoTable.usedCollectionCount, ServerData.userInfoTable.GetTableData(UserInfoTable.usedCollectionCount).Value);

        Param passParam = new Param();
        passParam.Add(OneYearPassServerTable.childFree,ServerData.oneYearPassServerTable.TableDatas[OneYearPassServerTable.childFree].Value);
        passParam.Add(OneYearPassServerTable.childAd,ServerData.oneYearPassServerTable.TableDatas[OneYearPassServerTable.childAd].Value);
        
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        transactions.Add(TransactionValue.SetUpdate(OneYearPassServerTable.tableName, OneYearPassServerTable.Indate, passParam));
        
        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            Debug.LogError("수박(봄나물)이벤트 교환 횟수 초기화!!");
        });
    }

    private void InitializeXMasEvent()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.eventMissionInitialize).Value > 0)
            return;

        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMissionInitialize).Value = 1;
        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_0).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_1).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_2).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_3).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_4).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_5).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_6).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_7).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_8).Value = 0;
        ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_9).Value = 0;
        
        List<TransactionValue> transactions = new List<TransactionValue>();
        
        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.eventMissionInitialize, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMissionInitialize).Value);
        userInfoParam.Add(UserInfoTable.eventMission0_0, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_0).Value);
        userInfoParam.Add(UserInfoTable.eventMission0_1, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_1).Value);
        userInfoParam.Add(UserInfoTable.eventMission0_2, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_2).Value);
        userInfoParam.Add(UserInfoTable.eventMission0_3, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_3).Value);
        userInfoParam.Add(UserInfoTable.eventMission0_4, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_4).Value);
        userInfoParam.Add(UserInfoTable.eventMission0_5, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_5).Value);
        userInfoParam.Add(UserInfoTable.eventMission0_6, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_6).Value);
        userInfoParam.Add(UserInfoTable.eventMission0_7, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_7).Value);
        userInfoParam.Add(UserInfoTable.eventMission0_8, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_8).Value);
        userInfoParam.Add(UserInfoTable.eventMission0_9, ServerData.userInfoTable.GetTableData(UserInfoTable.eventMission0_9).Value);

        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        
        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            Debug.LogError("이벤트 교환 횟수 초기화!!");
        });
    }
    
    private void CommonAttendEventStart()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.commonAttendCount).Value == 0)
        {
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.commonAttendCount).Value = 1;
            ServerData.userInfoTable_2.UpData(UserInfoTable_2.commonAttendCount, false);
        }
    }    
    private void ShopItemRefundRoutine()
    {
        //월간소탕권소급
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value < 1)
        {
           
            ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value++;

            int package1Count = 0;
            int package2Count = 0;

            package1Count = ServerData.iAPServerTableTotal.TableDatas["monthpackage1"].buyCount.Value;
            package2Count = ServerData.iAPServerTableTotal.TableDatas["monthpackage2"].buyCount.Value;

            if (package1Count == 0 && package2Count == 0)
            {
                ServerData.userInfoTable.UpData(UserInfoTable.RefundIdx, false);
            }
            else
            {
                
                int amount1 = 4 * package1Count;
            
                if (package1Count > 0)
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DosulClear).Value += amount1;
                }
            
                int amount2 = 8 * package2Count;
            
                if (package2Count > 0)
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.DosulClear).Value += amount2;
                }

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param goodsParam = new Param();
                goodsParam.Add(GoodsTable.DosulClear, ServerData.goodsTable.GetTableData(GoodsTable.DosulClear).Value);

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.RefundIdx, ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"월간 소탕권 세트 {CommonString.GetItemName(Item_Type.DosulClear)}{amount1+amount2}개 소급 완료!\n", null);
                });
            }

        }
        //패스 소급
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value < 2)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value++;

            int package1Count = 0;
            int package2Count = 0;
            int package3Count = 0;

            package1Count = ServerData.iapServerTable.TableDatas["suhopass"].buyCount.Value;
            package2Count = ServerData.iapServerTable.TableDatas["foxfirepass"].buyCount.Value;
            package3Count = ServerData.iapServerTable.TableDatas["sealswordpass"].buyCount.Value;

            if (package1Count == 0 && package2Count == 0&& package3Count == 0)
            {
                ServerData.userInfoTable.UpData(UserInfoTable.RefundIdx, false);
            }
            else
            {
                
                Param goodsParam = new Param();
                
                int amount1 = 2000 * package1Count;

                string desc = "패스재화 소급\n";
                
                if (package1Count > 0)
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value += amount1;
                    goodsParam.Add(GoodsTable.SuhoPetFeed, ServerData.goodsTable.GetTableData(GoodsTable.SuhoPetFeed).Value);
                    desc+=$"{CommonString.GetItemName(Item_Type.SuhoPetFeed)} {amount1}개 지급!\n";
                }
            
                int amount2 = 35000 * package2Count;
            
                if (package2Count > 0)
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.FoxRelic).Value += amount2;
                    goodsParam.Add(GoodsTable.FoxRelic, ServerData.goodsTable.GetTableData(GoodsTable.FoxRelic).Value);
                    desc+=$"{CommonString.GetItemName(Item_Type.FoxRelic)} {amount2}개 지급!\n";
                }
                int amount3 = 20 * package3Count;
            
                if (package3Count > 0)
                {
                    ServerData.goodsTable.GetTableData(GoodsTable.SealWeaponClear).Value += amount3;
                    goodsParam.Add(GoodsTable.SealWeaponClear, ServerData.goodsTable.GetTableData(GoodsTable.SealWeaponClear).Value);
                    desc+=$"{CommonString.GetItemName(Item_Type.SealWeaponClear)} {amount3}개 지급!\n";
                }

                List<TransactionValue> transactions = new List<TransactionValue>();

                Param userInfoParam = new Param();
                userInfoParam.Add(UserInfoTable.RefundIdx, ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value);

                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
                transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, desc, null);
                });
            } 
        }
        //출석초기화
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value < 3)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value++;
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value=0;
            
            ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendAd].Value = "0";
            ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendFree].Value = "0";
            
            List<TransactionValue> transactions = new List<TransactionValue>();
            
            Param userInfoParam = new Param();
            
            userInfoParam.Add(UserInfoTable.RefundIdx, ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value);
            userInfoParam.Add(UserInfoTable.attendanceCount, ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount).Value);
            
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            
            Param passParam = new Param();
            
            passParam.Add(AttendanceServerTable.attendAd, ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendAd].Value);
            passParam.Add(AttendanceServerTable.attendFree, ServerData.attendanceServerTable.TableDatas[AttendanceServerTable.attendFree].Value);
            
            transactions.Add(TransactionValue.SetUpdate(AttendanceServerTable.tableName, AttendanceServerTable.Indate, passParam));



            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                Debug.LogError("출석 초기화 완료");
            });
        }
        //꽃송이
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value < 4)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value++;

            var flowerSum = (ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission_Refund).Value + 200) / 2;

            var dokebifireSum = flowerSum*170;
            var newGachaEnergySum = flowerSum*300;
            
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += dokebifireSum;
            ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value += newGachaEnergySum;
           
            
            List<TransactionValue> transactions = new List<TransactionValue>();
            
            Param userInfoParam = new Param();
            
            userInfoParam.Add(UserInfoTable.RefundIdx, ServerData.userInfoTable.GetTableData(UserInfoTable.RefundIdx).Value);

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.DokebiFire,ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);
            goodsParam.Add(GoodsTable.NewGachaEnergy,ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);
            
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"꽃송이 재화 일괄 소급\n" +
                                                                            $"{CommonString.GetItemName(Item_Type.DokebiFire)} {dokebifireSum}개 환급\n" +
                                                                            $"{CommonString.GetItemName(Item_Type.NewGachaEnergy)} {newGachaEnergySum}개 환급 ", null);
            });
        }
        else
        {
            return;
        }
        

    }
    private void RefundRoutine()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.mileageRefund).Value != 0)
        {
            rootObject.SetActive(false);
            return;
        }

        float mileageTotalNum = 0;

        string description = string.Empty;

        var serverTable = ServerData.iAPServerTableTotal.TableDatas;

        var localTableData = TableManager.Instance.InAppPurchase.dataArray;

        for (int i = 0; i < localTableData.Length; i++)
        {
            for (int j = 0; j < localTableData[i].Rewardtypes.Length; j++)
            {
                if (localTableData[i].Rewardtypes[j] == 9000)
                {
                    int buyCount = serverTable[localTableData[i].Productid].buyCount.Value;

                    if (buyCount == 0) continue;

                    var mileageNum = localTableData[i].Rewardvalues[j] * buyCount;

                    mileageTotalNum += mileageNum;

                    var textObject = Instantiate<UiText>(uiText, parents);

                    textObject.Initialize($"{localTableData[i].Title} {buyCount}회 구매 마일리지 {mileageNum}개");
                }
            }
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.Mileage).Value += mileageTotalNum;
        ServerData.userInfoTable.GetTableData(UserInfoTable.mileageRefund).Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Mileage, ServerData.goodsTable.GetTableData(GoodsTable.Mileage).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.mileageRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.mileageRefund).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        if (mileageTotalNum == 0)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.mileageRefund).Value = 1;
            ServerData.userInfoTable.UpData(UserInfoTable.mileageRefund, false);

            rootObject.SetActive(false);
        }
        else
        {
            rootObject.SetActive(true);

            var totalText = Instantiate<UiText>(uiText, parents);

            totalText.Initialize($"마일리지 총 {mileageTotalNum}개 소급됨");

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "마일리지 소급 완료!", null);
            });
        }
    }
    private void DolPassRefundRoutine()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.dolPassRefund).Value != 0)
        {
            return;
        }

        if (ServerData.iapServerTable.TableDatas["dolpass"].buyCount.Value < 1)
        {
            //돌패스 구매 x
            ServerData.userInfoTable.GetTableData(UserInfoTable.dolPassRefund).Value = 1;
            ServerData.userInfoTable.UpData(UserInfoTable.dolPassRefund, false);
            return;
        }
        

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value += GameBalance.DolPassDiceRefundValue;
        ServerData.userInfoTable.GetTableData(UserInfoTable.dolPassRefund).Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.EventDice, ServerData.goodsTable.GetTableData(GoodsTable.EventDice).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.dolPassRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.dolPassRefund).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"주사위 총 {GameBalance.DolPassDiceRefundValue}개 소급됨\n" +
                                                                        $"주사위 소급 완료!", null);
        });
        
    }
    private void NewGachaRefundRoutine()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.newGachaEnergyRefund).Value != 0)
        {
            return;
        }

        if (ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).Value>25000)
        {
            //돌패스 구매 x
            ServerData.userInfoTable.GetTableData(UserInfoTable.newGachaEnergyRefund).Value = 1;
            ServerData.userInfoTable.UpData(UserInfoTable.newGachaEnergyRefund, false);
            return;
        }
        

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value -=
            (float)ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).Value;
        if (ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value < 0)
        {
            ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value = 0;
        }
        ServerData.userInfoTable.GetTableData(UserInfoTable.newGachaEnergyRefund).Value = 1;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.NewGachaEnergy, ServerData.goodsTable.GetTableData(GoodsTable.NewGachaEnergy).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.newGachaEnergyRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.newGachaEnergyRefund).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
        });
        
    }

    private void TitleRefundRoutine()
    {
        
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.titleConvertNewTitle).Value != 0)
        {
            return;
        }
        ////////타이틀 재 장착/////////////////
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.TitleSelectId, -1);
        PlayerStats.ResetAbilDic();
        ////////////////////////////////////////
        ServerData.userInfoTable.GetTableData(UserInfoTable.titleConvertNewTitle).Value = 1;
        var tableDatas = TableManager.Instance.TitleTable.dataArray;
        int levelTitleIdx = 0;
        int stageTitleIdx = 0;
        for (int i = 0; i < tableDatas.Length; i++)
        {
            //레벨
            if (tableDatas[i].Displaygroup == 0)
            {
                if (ServerData.titleServerTable.TableDatas[tableDatas[i].Stringid].rewarded.Value > 0)
                {
                    levelTitleIdx = i;
                }
            }
            //스테이지
            else if (tableDatas[i].Displaygroup == 1)
            {
                if (ServerData.titleServerTable.TableDatas[tableDatas[i].Stringid].rewarded.Value > 0)
                {
                    stageTitleIdx = i;
                }
            }
        }
        List<TransactionValue> transactions = new List<TransactionValue>();

        
    
        Param userInfoParam = new Param();
        if (levelTitleIdx == 0)
        {
            //받은게없음
        }
        else
        {
            var currentTitleLevel = TableManager.Instance.TitleTable.dataArray[levelTitleIdx].Condition;
            var levelTableData = TableManager.Instance.titleLevel.dataArray;
            for (int i = 0; i < levelTableData.Length; i++)
            {
                if (levelTableData[i].Condition == currentTitleLevel)
                {
                    ServerData.userInfoTable.GetTableData(UserInfoTable.titleLevel).Value = i;
                    break;
                }
            }
            
            userInfoParam.Add(UserInfoTable.titleLevel, ServerData.userInfoTable.GetTableData(UserInfoTable.titleLevel).Value);
        }
        if (stageTitleIdx == 0)
        {
            //받은게 없음
        }
        else
        {
            var currentTitleStage = TableManager.Instance.TitleTable.dataArray[stageTitleIdx].Condition;
            var stageTableData = TableManager.Instance.titleStage.dataArray;
            for (int i = 0; i < stageTableData.Length; i++)
            {
                if (stageTableData[i].Condition == currentTitleStage)
                {
                    ServerData.userInfoTable.GetTableData(UserInfoTable.titleStage).Value = i;
                    break;
                }
            }
            userInfoParam.Add(UserInfoTable.titleStage, ServerData.userInfoTable.GetTableData(UserInfoTable.titleStage).Value);
        }
        



        userInfoParam.Add(UserInfoTable.titleConvertNewTitle, ServerData.userInfoTable.GetTableData(UserInfoTable.titleConvertNewTitle).Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            Debug.LogError(
                $"Lv : {ServerData.userInfoTable.GetTableData(UserInfoTable.titleLevel).Value} / Stage : {ServerData.userInfoTable.GetTableData(UserInfoTable.titleStage).Value}");
        });
    }    
    private void TitleRefundRoutine2()
    {
        
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.titleConvertNewTitle2).Value != 0)
        {
            return;
        }
        ////////타이틀 재 장착/////////////////
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.TitleSelectId, -1);
        PlayerStats.ResetAbilDic();
        ////////////////////////////////////////
        ServerData.userInfoTable.GetTableData(UserInfoTable.titleConvertNewTitle2).Value = 1;
        var tableDatas = TableManager.Instance.TitleTable.dataArray;
        int weaponTitleIdx = 0;
        for (int i = 0; i < tableDatas.Length; i++)
        {
            //무기
            if (tableDatas[i].Displaygroup == 2)
            {
                if (ServerData.titleServerTable.TableDatas[tableDatas[i].Stringid].rewarded.Value > 0)
                {
                    weaponTitleIdx = i;
                }
            }
        }
        List<TransactionValue> transactions = new List<TransactionValue>();

        
    
        Param userInfoParam = new Param();
        if (weaponTitleIdx == 0)
        {
            //받은게없음
        }
        else if (weaponTitleIdx < 402)
        {
            //필멸무기(암) 미만임
        }
        else
        {
            var weaponId = TableManager.Instance.TitleTable.dataArray[weaponTitleIdx].Id;
            var levelTableData = TableManager.Instance.titleWeapon.dataArray;
            for (int i = 0; i < levelTableData.Length; i++)
            {
                if (levelTableData[i].Titeid == weaponId)
                {
                    ServerData.userInfoTable.GetTableData(UserInfoTable.titleWeapon).Value = i;
                    break;
                }
            }
            
            userInfoParam.Add(UserInfoTable.titleWeapon, ServerData.userInfoTable.GetTableData(UserInfoTable.titleWeapon).Value);
        }

        userInfoParam.Add(UserInfoTable.titleConvertNewTitle2, ServerData.userInfoTable.GetTableData(UserInfoTable.titleConvertNewTitle2).Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            Debug.LogError(
                $"Weapon : {ServerData.userInfoTable.GetTableData(UserInfoTable.titleWeapon).Value} ");
        });
    }
    
    private void TowerFloorAdjust()
    {
        
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.towerFloorAdjust).Value != 0)
        {
            return;
        }
        ////////////////////////////////////////
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.towerFloorAdjust).Value = 1;
        
        List<TransactionValue> transactions = new List<TransactionValue>();
        Param userInfoParam = new Param();

        var floor1 = ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value;

        if (floor1 > 2)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value = Mathf.Max(0, (int)((floor1-1) / 15));
            userInfoParam.Add(UserInfoTable.currentFloorIdx, ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value);
        }
        var floor2 = ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).Value;
        if (floor2>2)
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).Value = Mathf.Max(0, (int)((floor2-1) / 3));
            userInfoParam.Add(UserInfoTable.currentFloorIdx2, ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).Value);
        }

        if (floor1 > 0 || floor2 > 0)
        {
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        }
        

        Param userInfo_2Param = new Param();
        
        userInfo_2Param.Add(UserInfoTable_2.towerFloorAdjust, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.towerFloorAdjust).Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo_2Param));

        
        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            Debug.LogError($"요괴도장 : {ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx).Value}\n" +
                           $"상급 요괴도장 : {ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx2).Value}");
        });
    }
    
    private int GetMaxValueFromStringList(List<string> strList)
    {
        //문자열 비었으면
        if (strList == null || strList.Count == 0)
        {
            return -1;
        }
        List<int> intList = new List<int>();
    
        foreach (string str in strList)
        {
            int num;
            if (int.TryParse(str, out num))
            {
                intList.Add(num);
            }
        }
        //변환한게 비었으면
        if (intList.Count == 0)
        {
            return -1;
        }
        return intList.Max();
    }
    private void RelocateLevelPass()
    {
        
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.relocateLevelPass).Value != 0)
        {
            return;
        }
        ServerData.userInfoTable.GetTableData(UserInfoTable.relocateLevelPass).Value = 1;

        // StagePass
        var stageFreeRewardStr = ServerData.passServerTable.TableDatas[PassServerTable.stagePassReward].Value;
        var stageFreeRewardArr = stageFreeRewardStr.Split(',');
        var stageFreeMax = GetMaxValueFromStringList(stageFreeRewardArr.ToList());

        var stageAdRewardStr = ServerData.passServerTable.TableDatas[PassServerTable.stagePassAdReward].Value;
        var stageAdRewardArr = stageAdRewardStr.Split(',');
        var stageAdMax = GetMaxValueFromStringList(stageAdRewardArr.ToList());

        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.stagePassFree).Value = stageFreeMax;
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.stagePassAd).Value = stageAdMax;
        
        List<TransactionValue> transactions = new List<TransactionValue>();
        
        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.relocateLevelPass, ServerData.userInfoTable.GetTableData(UserInfoTable.relocateLevelPass).Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
        
        Param userInfo2Param = new Param();
        userInfo2Param.Add(UserInfoTable_2.stagePassFree, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.stagePassFree).Value);
        userInfo2Param.Add(UserInfoTable_2.stagePassAd, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.stagePassAd).Value);
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));
        
        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            Debug.LogError($" stageFree : {ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.stagePassFree).Value}" +
                           $"stageAd : {ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.stagePassAd).Value}");
        });
    }
    private void ChunmaDokebiFireRefundRoutine()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.chunmaRefund).Value != 0)
        {
            return;
        }
        ServerData.userInfoTable.GetTableData(UserInfoTable.chunmaRefund).Value = 1;
        var list = ServerData.bossServerTable.GetChunmaRewardedIdxList();
        var tableData= TableManager.Instance.TwelveBossTable.dataArray[55];
        var amount = 0;
        for (int i = 130; i < 133; i++)
        {
            if (list.Contains(i))
            {
                amount += (int)tableData.Rewardvalue[i] - 30;
            }    
        }
        
        List<TransactionValue> transactions = new List<TransactionValue>();
        
        if (amount == 0)
        {        
            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.chunmaRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.chunmaRefund).Value);
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));
            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                Debug.LogError("소급 없음");
            });
        }
        else
        {
            ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value += amount;
        
            Param userInfoParam = new Param();
            userInfoParam.Add(UserInfoTable.chunmaRefund, ServerData.userInfoTable.GetTableData(UserInfoTable.chunmaRefund).Value);
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

            Param goodsParam = new Param();
            goodsParam.Add(GoodsTable.DokebiFire, ServerData.goodsTable.GetTableData(GoodsTable.DokebiFire).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup("십만대산 도깨비불 소급", $"도깨비불 {amount}개 소급 완료", null);
            });
        }

    }
}
