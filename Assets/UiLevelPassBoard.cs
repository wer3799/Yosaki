using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI.Extensions;

public struct RewardItem
{
    public Item_Type ItemType;
    public float ItemValue;

    public RewardItem(Item_Type itemType, float itemValue)
    {
        this.ItemType = itemType;
        this.ItemValue = itemValue;
    }
}
public class UiLevelPassBoard : MonoBehaviour
{
    

    
    
    
    private string rewardFreeKey;
    private string iapKey;
    private List<int> splitData_Free;
    private List<int> splitData_Ad;
    [SerializeField] private LevelPassCellCreater _levelPassCellCreater;
    [SerializeField] private Transform cellParent;
    [SerializeField] private Transform scrollCellParent;
    [SerializeField] private SelectableLevelPassButton _selectableLevelPassButton;
    
    [SerializeField] private UiRewardResultView _uiRewardResultView;
    
    private List<RewardItem> rewardList = new List<RewardItem>();
    private void Start()
    {
        // RefundFox();
        Initialize();
    }
    
    private void AddOrUpdateReward(Item_Type itemType, float itemValue)
    {
        int existingRewardIndex = rewardList.FindIndex(r => r.ItemType == itemType);

        if (existingRewardIndex >= 0)
        {
            RewardItem existingReward = rewardList[existingRewardIndex];
            existingReward.ItemValue += itemValue;
            rewardList[existingRewardIndex] = existingReward;
        }
        else
        {
            rewardList.Add(new RewardItem(itemType, itemValue));
        }
    }
    
    private void Initialize()
    {
        var inAppPurchaseData = TableManager.Instance.InAppPurchase.dataArray;
        
        List<int> ints = new List<int>();
        
        foreach (var data in inAppPurchaseData)
        {
            if (data.PASSPRODUCTTYPE == PassProductType.LevelPass)
            {   
                ints.Add(data.Absoluteid);
            }
        }
        
        //자동화 시작하는 인덱스 순서
        for (int i = 0; i < ints.Count; i++)
        {
            var prefab = Instantiate<SelectableLevelPassButton>(_selectableLevelPassButton, cellParent);
            prefab.Initialize(ints[i],_levelPassCellCreater);
        }

    }
    private string refundProduct = "levelpass9";
    private void RefundFox()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.refundFox].Value == 1 || HasLevelPassProduct(refundProduct) == false)
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.refundFox].Value = 1;
            ServerData.userInfoTable.UpData(UserInfoTable.refundFox, false);
            return;
        }

        float refundJade = 0f;
        float refundMarble = 0f;

        var tableData = TableManager.Instance.LevelPass.dataArray;
        splitData_Ad = GetSplitData(NewLevelPass.premiumReward);

        for (int i = 0; i < tableData.Length; i++)
        {
            //비교대상이아님
            if (tableData[i].Shopid.Equals(refundProduct) == false) continue;

            //처리안해도되는거
            if (tableData[i].Wrongbefore == 0) continue;

            //보상 안받은애들은 처리X
            if (HasReward(splitData_Ad, tableData[i].Id) == false) continue;

            //옥
            if (tableData[i].Reward2_Pass == 1)
            {
                refundJade += (tableData[i].Reward2_Value - tableData[i].Wrongbefore);
            }
            //여우구슬
            else if (tableData[i].Reward2_Pass == 5)
            {
                refundMarble += (tableData[i].Reward2_Value - tableData[i].Wrongbefore);
            }
        }

        ServerData.userInfoTable.TableDatas[UserInfoTable.refundFox].Value = 1;

        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += refundJade;
        ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value += refundMarble;

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
        goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);


        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.refundFox, ServerData.userInfoTable.TableDatas[UserInfoTable.refundFox].Value);

        List<TransactionValue> transactions = new List<TransactionValue>();

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));


        ServerData.SendTransaction(transactions, successCallBack: () =>
          {
              PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"옥 : {Utils.ConvertBigNum(refundJade)}개\n여우구슬 : {Utils.ConvertBigNum(refundMarble)}개\n소급 완료!", null);
          });
    }

    public void OnClickAllReceiveButton()
    {
        splitData_Free = GetSplitData(NewLevelPass.freeReward);
        splitData_Ad = GetSplitData(NewLevelPass.premiumReward);

        var tableData = TableManager.Instance.LevelPass.dataArray;

        int rewardedNum = 0;

        string free = ServerData.newLevelPass.TableDatas[NewLevelPass.freeReward].Value;
        string ad = ServerData.newLevelPass.TableDatas[NewLevelPass.premiumReward].Value;

        List<int> rewardTypeList = new List<int>();

        List<RewardItem> rewards = new List<RewardItem>();
        
        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlocklevel);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                free += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1_Free, tableData[i].Reward1_Value);
                if (rewardTypeList.Contains(tableData[i].Reward1_Free) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward1_Free);
                }
                rewardedNum++;
            }

            //유로보상
            if (HasLevelPassProduct(tableData[i].Shopid) && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                ad += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2_Pass, tableData[i].Reward2_Value);
                if (rewardTypeList.Contains(tableData[i].Reward2_Pass) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward2_Pass);
                }

                rewardedNum++;
            }
        }

        if (rewardedNum > 0)
        {
            ServerData.newLevelPass.TableDatas[NewLevelPass.freeReward].Value = free;
            ServerData.newLevelPass.TableDatas[NewLevelPass.premiumReward].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            using var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            //goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
            //goodsParam.Add(GoodsTable.MarbleKey, ServerData.goodsTable.GetTableData(GoodsTable.MarbleKey).Value);
            //goodsParam.Add(GoodsTable.Peach, ServerData.goodsTable.GetTableData(GoodsTable.Peach).Value);
            //goodsParam.Add(GoodsTable.RelicTicket, ServerData.goodsTable.GetTableData(GoodsTable.RelicTicket).Value);
            //goodsParam.Add(GoodsTable.SwordPartial, ServerData.goodsTable.GetTableData(GoodsTable.SwordPartial).Value);
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(NewLevelPass.freeReward, ServerData.newLevelPass.TableDatas[NewLevelPass.freeReward].Value);
            passParam.Add(NewLevelPass.premiumReward, ServerData.newLevelPass.TableDatas[NewLevelPass.premiumReward].Value);

            transactions.Add(TransactionValue.SetUpdate(NewLevelPass.tableName, NewLevelPass.Indate, passParam));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                LogManager.Instance.SendLogType("LevelPass", "A", "A");
            });

        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }
    public void OnClickAllReceiveButtonV2()
    {
        splitData_Free = GetSplitData(NewLevelPass.freeReward);
        splitData_Ad = GetSplitData(NewLevelPass.premiumReward);

        var tableData = TableManager.Instance.LevelPass.dataArray;

        int rewardedNum = 0;

        string free = ServerData.newLevelPass.TableDatas[NewLevelPass.freeReward].Value;
        string ad = ServerData.newLevelPass.TableDatas[NewLevelPass.premiumReward].Value;

        List<int> rewardTypeList = new List<int>();

        rewardList.Clear();
        
        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlocklevel);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                free += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1_Free, tableData[i].Reward1_Value);
                AddOrUpdateReward((Item_Type)(int)tableData[i].Reward1_Free, tableData[i].Reward1_Value);
                if (rewardTypeList.Contains(tableData[i].Reward1_Free) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward1_Free);
                }
                rewardedNum++;
            }

            //유로보상
            if (HasLevelPassProduct(tableData[i].Shopid) && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                ad += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2_Pass, tableData[i].Reward2_Value);
                AddOrUpdateReward((Item_Type)(int)tableData[i].Reward2_Pass, tableData[i].Reward2_Value);
                if (rewardTypeList.Contains(tableData[i].Reward2_Pass) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward2_Pass);
                }

                rewardedNum++;
            }
        }

        if (rewardedNum > 0)
        {
            ServerData.newLevelPass.TableDatas[NewLevelPass.freeReward].Value = free;
            ServerData.newLevelPass.TableDatas[NewLevelPass.premiumReward].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            using var e = rewardTypeList.GetEnumerator();

            Param goodsParam = new Param();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(NewLevelPass.freeReward, ServerData.newLevelPass.TableDatas[NewLevelPass.freeReward].Value);
            passParam.Add(NewLevelPass.premiumReward, ServerData.newLevelPass.TableDatas[NewLevelPass.premiumReward].Value);

            transactions.Add(TransactionValue.SetUpdate(NewLevelPass.tableName, NewLevelPass.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                //PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                LogManager.Instance.SendLogType("LevelPass", "A", "A");

                List<RewardData> rewardData = new List<RewardData>();
                using var e = rewardList.GetEnumerator();
                for (int i = 0 ;  i < rewardList.Count;i++)
                {
                    if (e.MoveNext())
                    {
                        rewardData.Add(new RewardData(e.Current.ItemType,e.Current.ItemValue));
                    }                    
                }
                if (rewardData.Count > 0)
                {
                    _uiRewardResultView.gameObject.SetActive(true);
                    _uiRewardResultView.Initialize(rewardData);
                }
            });

        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }

    private bool CanGetReward(int require)
    {
        int currentLevel = (int)ServerData.statusTable.GetTableData(StatusTable.Level).Value;
        return currentLevel >= require;
    }
    private bool HasReward(List<int> splitData, int id)
    {
        return splitData.Contains(id);
    }

    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.newLevelPass.TableDatas[key].Value.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            if (int.TryParse(splits[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;
    }

    private bool HasLevelPassProduct(string passItem)
    {
        return ServerData.iapServerTable.TableDatas[passItem].buyCount.Value >= 1;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.statusTable.GetTableData(StatusTable.Level).Value += 5000;
        }
    }
#endif
    
}
