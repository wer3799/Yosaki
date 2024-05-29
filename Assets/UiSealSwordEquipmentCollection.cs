using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiSealSwordEquipmentCollection : MonoBehaviour
{
    [SerializeField]
    private UiSealSwordCollectionView equipmentCollectionViewPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI rewardDescription;

    [SerializeField]
    private TextMeshProUGUI abilList;


    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        UpdateDescription();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.sealSwordTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiSealSwordCollectionView>(equipmentCollectionViewPrefab, cellParent);

            cell.Initialize(tableData[i]);
        }
    }

    private void UpdateDescription()
    {
        SetAbilText();

        SetRewardText();

    }

    private void SetAbilText()
    {
        
        var tableData = TableManager.Instance.sealSwordTable.dataArray;
        
        Dictionary<StatusType, float> rewards = new Dictionary<StatusType, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            StatusType abilType = (StatusType)tableData[i].Collectioneffecttype;

            if (rewards.ContainsKey(abilType) == false)
            {
                var ret = PlayerStats.GetSealSwordCollectionHasValue(abilType);
                if (ret != 0)
                {
                    rewards.Add(abilType, ret);
                }
            }
        }
        
        using var e = rewards.GetEnumerator();

        string description = "";

        while (e.MoveNext())
        {
            if (Utils.IsPercentStat(e.Current.Key))
            {
                description += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertBigNum(e.Current.Value * 100f)} 증가\n";
            }
            else
            {
                description += $"{CommonString.GetStatusName(e.Current.Key)} {Utils.ConvertBigNum(e.Current.Value)} 증가\n";
            }
        }

        if (rewards.Count == 0)
        {
            abilDescription.SetText("요도가 없습니다.");
        }
        else
        {
            abilDescription.SetText(description);
        }

        string abils = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            if(Utils.IsPercentStat((StatusType)tableData[i].Collectioneffecttype))
            {
                abils += $"{tableData[i].Name} 보유 : {CommonString.GetStatusName((StatusType)tableData[i].Collectioneffecttype)} {Utils.ConvertBigNum(tableData[i].Collectioneffectvalue * 100f)}\n";
            }
            else
            {
                abils += $"{tableData[i].Name} 보유 : {CommonString.GetStatusName((StatusType)tableData[i].Collectioneffecttype)} {Utils.ConvertBigNum(tableData[i].Collectioneffectvalue)}\n";
            }
        }

        abils += "<color=red>모든 효과는 중첩됩니다!</color>";


        abilList.SetText(abils);
    }

    private void SetRewardText()
    {

        var tableData = TableManager.Instance.sealSwordTable.dataArray;

        Dictionary<Item_Type, float> rewards = new Dictionary<Item_Type, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            Item_Type rewardType = (Item_Type)tableData[i].Rewardtype0;
            float rewardValue = tableData[i].Rewardvalue0;

            if (rewards.ContainsKey(rewardType) == false)
            {
                rewards.Add(rewardType, 0f);
            }

            rewards[rewardType] += rewardValue;
        }

        using var e = rewards.GetEnumerator();

        string description = "";

        while (e.MoveNext())
        {
            description += $"{CommonString.GetItemName(e.Current.Key)} {Utils.ConvertBigNum(e.Current.Value)}개\n";
        }

        if (rewards.Count == 0) 
        {
            rewardDescription.SetText("요도가 없습니다.");
        }
        else 
        {
            rewardDescription.SetText(description);
        }

     
    }

    public void OnClickRecieveAllReward()
    {
        List<TransactionValue> transactions = new List<TransactionValue>();
        
        List<int> rewardTypeList = new List<int>();
        List<string> equipmentNameList = new List<string>();

        int rewardCount = 0;
        
        var tableData = TableManager.Instance.sealSwordTable.dataArray;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            var serverData = ServerData.sealSwordServerTable.TableDatas[tableData[i].Stringid];
            //가지고있지 않으면 continue
            if (serverData.hasItem.Value < 1) continue;
            //무료보상 안 받은 경우
            if (serverData.getReward0.Value < 1)
            {
                serverData.getReward0.Value = 1;
                ServerData.goodsTable.GetTableData((Item_Type)tableData[i].Rewardtype0).Value += tableData[i].Rewardvalue0;
                
                if(rewardTypeList.Contains(tableData[i].Rewardtype0)==false)
                {
                    rewardTypeList.Add(tableData[i].Rewardtype0);
                }
                if(equipmentNameList.Contains(tableData[i].Stringid)==false)
                {
                    equipmentNameList.Add(tableData[i].Stringid);
                }
                
                rewardCount++;
            }
            //패스권 안산 경우 continue
            if (ServerData.iapServerTable.TableDatas[UiEquipmentCollectionPassBuyButton.collectionPassKey].buyCount.Value < 1) continue;
            //유료보상 안 받은 경우
            if (serverData.getReward1.Value < 1)
            {
                serverData.getReward1.Value = 1;
                ServerData.goodsTable.GetTableData((Item_Type)tableData[i].Rewardtype1).Value += tableData[i].Rewardvalue1;
                
                if(rewardTypeList.Contains(tableData[i].Rewardtype1)==false)
                {
                    rewardTypeList.Add(tableData[i].Rewardtype1);
                }
                if(equipmentNameList.Contains(tableData[i].Stringid)==false)
                {
                    equipmentNameList.Add(tableData[i].Stringid);
                }
                rewardCount++;
            }
        }
        
        if (rewardCount > 0)
        {
            if (rewardTypeList.Count != 0)
            {
                using var e = rewardTypeList.GetEnumerator();
                Param goodsParam = new Param();
                while(e.MoveNext())
                {
                    goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
                }
                transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
            }
            if (equipmentNameList.Count != 0)
            {
                using var equipment = equipmentNameList.GetEnumerator();
            
                Param equipmentParam = new Param();
                while(equipment.MoveNext())
                {
                    string updateValue = ServerData.sealSwordServerTable.TableDatas[equipment.Current].ConvertToString();
                    equipmentParam.Add(equipment.Current, updateValue);
                }
                transactions.Add(TransactionValue.SetUpdate(SealSwordServerTable.tableName, SealSwordServerTable.Indate, equipmentParam));
    
            }
            
            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령가능한 보상이 없습니다");
        }
    }
}
