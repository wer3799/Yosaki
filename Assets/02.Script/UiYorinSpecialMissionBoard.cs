using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UniRx;
using UnityEngine;

public class UiYorinSpecialMissionBoard : SingletonMono<UiYorinSpecialMissionBoard>
{
    [SerializeField] private UiYorinSpecialGradeCell cell;
    [SerializeField] private Transform cellParent;

    [SerializeField] private UiYorinSpecialMissionCell missionCell;
    [SerializeField] private Transform missionCellParent;

    private List<UiYorinSpecialMissionCell> cellContainer = new List<UiYorinSpecialMissionCell>();
    

    private void Start()
    {
        MakeTabCell();

        MakeMissionCell(0);

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.relicServerTable.TableDatas["relic5"].level.AsObservable().Subscribe(e =>
        {
            if (e >=20000)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission1_1].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.costumeSpecialAbilityServerTable.TableDatas["passive0"].level.AsObservable().Subscribe(e =>
        {
            if (e >= 5)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission1_2].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.weaponTable.TableDatas["weapon24"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission1_3].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.magicBookTable.TableDatas["magicBook21"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission1_4].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        ServerData.weaponTable.TableDatas["weapon25"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission2_1].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        
        //2-2는 보상받기버튼
        
        ServerData.weaponTable.TableDatas["weapon29"].hasItem.AsObservable().Subscribe(e =>
        {
            if (e > 0)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission2_3].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateGold).AsObservable().Subscribe(e =>
        {
            if (e >0)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission2_4].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateGumSoul).AsObservable().Subscribe(e =>
        {
            if (e >0)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission3_1].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.currentFloorIdx5).AsObservable().Subscribe(e =>
        {
            if (e >=10)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission3_2].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.KingTrialGraduateIdx).AsObservable().Subscribe(e =>
        {
            if (e >=1)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission3_3].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateNorigaeSoul).AsObservable().Subscribe(e =>
        {
            if (e >0)
            {
                string key = TableManager.Instance.YorinSpecialMissionDatas[(int)YorinSpecialMissionKey.YSMission3_4].Stringid;
                ServerData.yorinSpecialMissionServerTable.UpdateMissionClearToCount(key, 1);
            }
        }).AddTo(this);
    }

    private void MakeTabCell()
    {
        var tableData = TableManager.Instance.YorinSpecialMission.dataArray;

        var maxCount = 0;
        for (int i = 0; i < tableData.Length; i++)
        {
            maxCount = Mathf.Max(maxCount, tableData[i].Missionstep);
        }

        for (int i = 0; i <= maxCount; i++)
        {
            var prefab = Instantiate(cell, cellParent);
            prefab.Initialize(i);
        }
    }

    public void MakeMissionCell(int idx)
    {
        var tableData = TableManager.Instance.YorinSpecialMission.dataArray;

        var missionList = new List<YorinSpecialMissionData>();

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Missionstep != idx) continue;

            missionList.Add(tableData[i]);
        }
        
        var cellCount = missionList.Count;
        
        while (cellCount > cellContainer.Count)
        {
            var prefab = Instantiate(missionCell, missionCellParent);
            cellContainer.Add(prefab);
        }
        
        for (var i = 0; i < cellContainer.Count; i++)
        {
            if (i < cellCount)
            {
                cellContainer[i].gameObject.SetActive(true);
                cellContainer[i].Initialize(missionList[i]);
                
                cellContainer[i].transform.SetSiblingIndex(missionList[i].Displayorder);
            }
            else
            {
                cellContainer[i].gameObject.SetActive(false);
            }
        }
        
    }

    public void OnClickAllReceiveButton()
    {
        var tableData = TableManager.Instance.YorinSpecialMission.dataArray;
        
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param missionParam = new Param();
        Param goodsParam = new Param();
        Param growthParam = new Param();

        UiRewardResultPopUp.Instance.Clear();
        
        
        for (int i = 0; i<  tableData.Length; i++)
        {
            //받았으면 넘김.
            if(ServerData.yorinSpecialMissionServerTable.TableDatas[tableData[i].Stringid].rewardCount.Value>0) continue;
            //클리어 안했으면 넘김
            if(ServerData.yorinSpecialMissionServerTable.TableDatas[tableData[i].Stringid].clearCount.Value<1) continue;

            UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)tableData[i].Reward1, tableData[i].Reward1_Value);
            UiRewardResultPopUp.Instance.AddOrUpdateReward(Item_Type.Exp, (float)tableData[i].Exp);
            
            ServerData.yorinSpecialMissionServerTable.TableDatas[tableData[i].Stringid].rewardCount.Value = 1;
            missionParam.Add(tableData[i].Stringid, ServerData.yorinSpecialMissionServerTable.TableDatas[tableData[i].Stringid].ConvertToString());

        }

        if (UiRewardResultPopUp.Instance.RewardList.Count > 0)
        {
            using var e = UiRewardResultPopUp.Instance.RewardList.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.itemType == Item_Type.Exp)
                {
                    ServerData.growthTable.GetTableData(GrowthTable.Exp).Value +=  e.Current.amount;
                    growthParam.Add(GrowthTable.Exp, ServerData.growthTable.GetTableData(GrowthTable.Exp).Value);
                    continue;
                }
                var goods = ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType);
                ServerData.goodsTable.GetTableData(goods).Value += e.Current.amount;
                goodsParam.Add(goods, ServerData.goodsTable.GetTableData(goods).Value);
            }

            
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            transactions.Add(TransactionValue.SetUpdate(YorinSpecialMissionServerTable.tableName, YorinSpecialMissionServerTable.Indate, missionParam));
            transactions.Add(TransactionValue.SetUpdate(GrowthTable.tableName, GrowthTable.Indate, growthParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                UiRewardResultPopUp.Instance
                    .Show()
                    .Clear();
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage(CommonString.Reward_Nothing);
        }
    }
}
