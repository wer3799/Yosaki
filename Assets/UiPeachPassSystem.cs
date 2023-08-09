using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiPeachPassSystem : MonoBehaviour
{
    [SerializeField]
    private UiPeachPassCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;


    private List<UiPeachPassCell> uiPassCellContainer = new List<UiPeachPassCell>();

    private List<int> splitData_Free;
    private List<int> splitData_Ad;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.PeachPass.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var prefab = Instantiate<UiPeachPassCell>(uiPassCellPrefab, cellParent);
            uiPassCellContainer.Add(prefab);
        }

        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            if (i < tableData.Length)
            {
                var passInfo = new PassInfo();

                passInfo.require = tableData[i].Unlockcount;
                passInfo.id = tableData[i].Id;

                passInfo.rewardType_Free = tableData[i].Reward1;
                passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
                passInfo.rewardType_Free_Key = ColdSeasonPassServerTable.peachFree;

                passInfo.rewardType_IAP = tableData[i].Reward2;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = ColdSeasonPassServerTable.peachAd;

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
        splitData_Free = GetSplitData(ColdSeasonPassServerTable.peachFree);
        splitData_Ad = GetSplitData(ColdSeasonPassServerTable.peachAd);

        var tableData = TableManager.Instance.PeachPass.dataArray;

        int rewardedNum = 0;

        List<int> typeList = new List<int>();
        
        string free = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.peachFree].Value;
        string ad = ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.peachAd].Value;


        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockcount);

            if (canGetReward == false) break;

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
            ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.peachFree].Value = free;
            ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.peachAd].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();

            var e = typeList.GetEnumerator();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(ColdSeasonPassServerTable.peachFree, ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.peachFree].Value);
            passParam.Add(ColdSeasonPassServerTable.peachAd, ServerData.coldSeasonPassServerTable.TableDatas[ColdSeasonPassServerTable.peachAd].Value);

            transactions.Add(TransactionValue.SetUpdate(ColdSeasonPassServerTable.tableName, ColdSeasonPassServerTable.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null); 
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }

    }

    public bool CanGetReward(int require)
    {
        var score = ServerData.userInfoTable.GetTableData(UserInfoTable.sonScore).Value * GameBalance.BossScoreConvertToOrigin;

        var tableData = TableManager.Instance.SonReward.dataArray;

        int idx = -1;
        for (int i = 0; i < tableData.Length; i++)
        {
            if (score > tableData[i].Score)
            {
                idx = i;
            }
            else
            {
                break;
            }
        }

        return idx >= require;
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
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiPeachPassBuyButton.seasonPassKey].buyCount.Value > 0;

        return hasIapProduct;
    }
}
