using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiMagicBookEquipmentCollection : MonoBehaviour
{
    [SerializeField]
    private UiMagicBookCollectionView magicbookCollectionViewPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI rewardDescription;


    [SerializeField]
    private Button getMagicbookButton;

    [SerializeField]
    private TextMeshProUGUI rewardButtonDescription;

    [SerializeField]
    private TextMeshProUGUI abilList;

    List<UiMagicBookCollectionView> cellList = new List<UiMagicBookCollectionView>();

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
        var tableData = TableManager.Instance.MagicBookTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if ((tableData[i].MAGICBOOKTYPE == MagicBookType.View )||
                (tableData[i].MAGICBOOKTYPE == MagicBookType.Basic))
            {
                continue;
            }
            var cell = Instantiate<UiMagicBookCollectionView>(magicbookCollectionViewPrefab, cellParent);

            
            cellList.Add(cell);
            
            cell.Initialize(tableData[i]);
        }
        
        List<(int displayOrder, UiMagicBookCollectionView gameObject)> magicBooks = new List<(int, UiMagicBookCollectionView)>();
        foreach (var cell in cellList)
        {
            if (cell.GetMagicBookData().MAGICBOOKTYPE == MagicBookType.Normal)
            {
                magicBooks.Add((cell.GetMagicBookData().Displayorder, cell));
            }
        }
        magicBooks.Sort((a, b) => a.displayOrder.CompareTo(b.displayOrder));

        for (int i = 0; i < magicBooks.Count; i++)
        {
            UiMagicBookCollectionView magicBookObject = magicBooks[i].gameObject;
            magicBookObject.transform.SetSiblingIndex(i);
        }
    }

    private void UpdateDescription()
    {
        SetAbilText();

        SetRewardText();

    }

    private void SetAbilText()
    {
        
        var tableData = TableManager.Instance.MagicBookTable.dataArray;

        Dictionary<StatusType, float> rewards = new Dictionary<StatusType, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].MAGICBOOKTYPE == MagicBookType.View) continue;
            if (tableData[i].MAGICBOOKTYPE == MagicBookType.Basic) continue;

            StatusType abilType = (StatusType)tableData[i].Collectioneffecttype;

            if (rewards.ContainsKey(abilType) == false)
            {
                var ret = PlayerStats.GetMagicBookCollectionHasValue(abilType);
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
            abilDescription.SetText("노리개가 없습니다.");
        }
        else
        {
            abilDescription.SetText(description);
        }

        string abils = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].MAGICBOOKTYPE == MagicBookType.View) continue;
            if (tableData[i].MAGICBOOKTYPE == MagicBookType.Basic) continue;
            if (Utils.IsPercentStat((StatusType)tableData[i].Collectioneffecttype))
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

        var tableData = TableManager.Instance.MagicBookTable.dataArray;

        Dictionary<Item_Type, float> rewards = new Dictionary<Item_Type, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].MAGICBOOKTYPE == MagicBookType.View) continue;

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
            rewardDescription.SetText("노리개가 없습니다.");
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
        List<string> itemNameList = new List<string>();

        int rewardCount = 0;
        
        var tableData = TableManager.Instance.MagicBookTable.dataArray;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            var serverData = ServerData.magicBookTable.TableDatas[tableData[i].Stringid];
            if(tableData[i].MAGICBOOKTYPE==MagicBookType.Basic) continue;
            if(tableData[i].MAGICBOOKTYPE==MagicBookType.View) continue;
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
                if(itemNameList.Contains(tableData[i].Stringid)==false)
                {
                    itemNameList.Add(tableData[i].Stringid);
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
                if(itemNameList.Contains(tableData[i].Stringid)==false)
                {
                    itemNameList.Add(tableData[i].Stringid);
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
            if (itemNameList.Count != 0)
            {
                using var ItemName = itemNameList.GetEnumerator();
            
                Param itemParam = new Param();
                while(ItemName.MoveNext())
                {
                    string updateValue = ServerData.magicBookTable.TableDatas[ItemName.Current].ConvertToString();
                    itemParam.Add(ItemName.Current, updateValue);
                }
                transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, itemParam));
    
            }
            
            ServerData.SendTransaction(transactions, successCallBack: () =>
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
