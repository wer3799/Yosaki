using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiWeaponEquipmentCollection : MonoBehaviour
{
    [SerializeField]
    private UiWeaponCollectionView weaponCollectionViewPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI rewardDescription;


    [SerializeField]
    private Button getWeaponButton;

    [SerializeField]
    private TextMeshProUGUI rewardButtonDescription;

    [SerializeField]
    private TextMeshProUGUI abilList;
    List<UiWeaponCollectionView> cellList = new List<UiWeaponCollectionView>();

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
        var tableData = TableManager.Instance.WeaponTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if ((tableData[i].WEAPONTYPE == WeaponType.View)||(tableData[i].WEAPONTYPE == WeaponType.RecommendView)||
                (tableData[i].WEAPONTYPE == WeaponType.Basic) )
            {
                continue;
            }
            var cell = Instantiate<UiWeaponCollectionView>(weaponCollectionViewPrefab, cellParent);
            cell.Initialize(tableData[i]);

            cellList.Add(cell);
            

        }
        
        List<(int displayOrder, UiWeaponCollectionView gameObject)> weapons = new List<(int, UiWeaponCollectionView)>();
        foreach (var cell in cellList)
        {
            if (cell.GetWeaponData().WEAPONTYPE == WeaponType.Normal || cell.GetWeaponData().WEAPONTYPE == WeaponType.HasEffectOnly)
            {
                weapons.Add((cell.GetWeaponData().Displayorder, cell));
            }
        }
        weapons.Sort((a, b) => a.displayOrder.CompareTo(b.displayOrder));

        for (int i = 0; i < weapons.Count; i++)
        {
            UiWeaponCollectionView weaponObject = weapons[i].gameObject;
            weaponObject.transform.SetSiblingIndex(i);
        }
        

    }
    
    private void SetTransform()
    {

    }
    private void UpdateDescription()
    {
        SetAbilText();

        SetRewardText();

    }

    private void SetAbilText()
    {
        
        var tableData = TableManager.Instance.WeaponTable.dataArray;
        
        var serverData = ServerData.weaponTable.TableDatas;
        Dictionary<StatusType, float> rewards = new Dictionary<StatusType, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].WEAPONTYPE == WeaponType.View) continue;
            if (tableData[i].WEAPONTYPE == WeaponType.RecommendView) continue;
            if (tableData[i].WEAPONTYPE == WeaponType.Basic) continue;

            StatusType abilType = (StatusType)tableData[i].Collectioneffecttype;

            if (rewards.ContainsKey(abilType) == false)
            {
                var ret = PlayerStats.GetWeaponCollectionHasValue(abilType);
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
            abilDescription.SetText("무기가 없습니다.");
        }
        else
        {
            abilDescription.SetText(description);
        }

        string abils = string.Empty;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].WEAPONTYPE == WeaponType.View) continue;
            if (tableData[i].WEAPONTYPE == WeaponType.RecommendView) continue;
            if (tableData[i].WEAPONTYPE == WeaponType.Basic) continue;
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

        var tableData = TableManager.Instance.WeaponTable.dataArray;

        Dictionary<Item_Type, float> rewards = new Dictionary<Item_Type, float>();

        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].WEAPONTYPE == WeaponType.View) continue;
            if (tableData[i].WEAPONTYPE == WeaponType.RecommendView) continue;

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
            rewardDescription.SetText("무기가 없습니다.");
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
        
        var tableData = TableManager.Instance.WeaponTable.dataArray;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            var serverData = ServerData.weaponTable.TableDatas[tableData[i].Stringid];
            //Basic, view는 도감에없으니 제외
            if(tableData[i].WEAPONTYPE==WeaponType.Basic) continue;
            if(tableData[i].WEAPONTYPE==WeaponType.View) continue;
            if(tableData[i].WEAPONTYPE==WeaponType.RecommendView) continue;
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
                    string updateValue = ServerData.weaponTable.TableDatas[ItemName.Current].ConvertToString();
                    itemParam.Add(ItemName.Current, updateValue);
                }
                transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, itemParam));
    
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
