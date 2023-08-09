using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiSoulPassSystem : MonoBehaviour
{
    [SerializeField]
    private UiSoulPassCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;


    private List<UiSoulPassCell> uiPassCellContainer = new List<UiSoulPassCell>();

    private List<int> splitData_Free;
    private List<int> splitData_Ad;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.SoulPass.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var prefab = Instantiate<UiSoulPassCell>(uiPassCellPrefab, cellParent);
            uiPassCellContainer.Add(prefab);
        }

        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            if (i < tableData.Length)
            {
                var passInfo = new EquipmentPassInfo();

                passInfo.require = tableData[i].Unlockstringid;
                passInfo.id = tableData[i].Id;

                passInfo.rewardType_Free = tableData[i].Reward1;
                passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
                passInfo.rewardType_Free_Key = ColdSeasonPassServerTable.soulFree;

                passInfo.rewardType_IAP = tableData[i].Reward2;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = ColdSeasonPassServerTable.soulAd;

                uiPassCellContainer[i].gameObject.SetActive(true);
                uiPassCellContainer[i].Initialize(passInfo);
            }
            else
            {
                uiPassCellContainer[i].gameObject.SetActive(false);
            }
        }
        // cellParent.transform.localPosition = new Vector3(0f, cellParent.transform.localPosition.y, cellParent.transform.localPosition.z);
    }

    public void OnClickAllReceiveButton()
    {
        splitData_Free = GetSplitData(ColdSeasonPassServerTable.soulFree);
        splitData_Ad = GetSplitData(ColdSeasonPassServerTable.soulAd);

        var tableData = TableManager.Instance.SoulPass.dataArray;

        int rewardedNum = 0;

        List<int> typeList = new List<int>();
        
        string free = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.soulFree].Value;
        string ad = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.soulAd].Value;


        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockstringid);

            if (canGetReward == false) continue;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                free += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                rewardedNum++;
                if(!typeList.Contains(tableData[i].Reward1))
                {
                    typeList.Add(tableData[i].Reward1);
                }
            }

            //유로보상
            if (HasPassItem() && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                ad += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);
                rewardedNum++;
                
                if(!typeList.Contains(tableData[i].Reward2))
                {
                    typeList.Add(tableData[i].Reward2);
                }
            }
        }

        if (rewardedNum > 0)
        {
            ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.soulFree].Value = free;
            ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.soulAd].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();

            var e = typeList.GetEnumerator();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(ColdSeasonPassServerTable.soulFree, ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.soulFree].Value);
            passParam.Add(ColdSeasonPassServerTable.soulAd, ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.soulAd].Value);

            transactions.Add(TransactionValue.SetUpdate(ColdSeasonPassServerTable.tableName, ColdSeasonPassServerTable.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null); 
                //LogManager.Instance.SendLogType("SoulPass", "A", "A");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }

    }

    public bool CanGetReward(string require)
    {
        return ServerData.newGachaServerTable.TableDatas[require].hasItem.Value > 0;
    }

    private bool HasReward(List<int> splitData, int id)
    {
        return splitData.Contains(id);
    }

    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.coldSeasonPassServerTable.TableDatas[key].Value.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            if (int.TryParse(splits[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;
    }

    private bool HasPassItem()
    {
        return ServerData.iapServerTable.TableDatas[UiSoulPassBuyButton.seasonPassKey].buyCount.Value > 0;
    }
}
