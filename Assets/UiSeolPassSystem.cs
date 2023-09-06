using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
public class UiSeolPassSystem : MonoBehaviour
{
    [SerializeField]
    private UiSeolPassCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiSeolPassCell> uiPassCellContainer = new List<UiSeolPassCell>();

    private ObscuredString passShopId;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Seol).Value += 1;
        }
    }
#endif

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.SeolPass.dataArray;

        int interval = tableData.Length - uiPassCellContainer.Count;

        for (int i = 0; i < interval; i++)
        {
            var prefab = Instantiate<UiSeolPassCell>(uiPassCellPrefab, cellParent);
            uiPassCellContainer.Add(prefab);
        }

        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            if (i < tableData.Length)
            {
                var passInfo = new PassInfo();

                passInfo.require = tableData[i].Unlockamount;
                passInfo.id = tableData[i].Id;

                passInfo.rewardType_Free = tableData[i].Reward1;
                passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
                passInfo.rewardType_Free_Key = SeolPassServerTable.MonthlypassFreeReward;

                passInfo.rewardType_IAP = tableData[i].Reward2;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = SeolPassServerTable.MonthlypassAdReward;

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
    private bool CanGetReward(int require)
    {
        int seolKillCount = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.attendanceCount_Seol).Value;
        return seolKillCount >= require;
    }
    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.seolPassServerTable.TableDatas[key].Value.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            if (int.TryParse(splits[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;    
    }
    private bool HasReward(List<int> splitData, int id)
    {
        return splitData.Contains(id);
    }
    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas["sulpass1"].buyCount.Value > 0;

        return hasIapProduct;
    }
    public void OnClickAllReceiveButton()
    {
        string freeKey = SeolPassServerTable.MonthlypassFreeReward;
        string adKey = SeolPassServerTable.MonthlypassAdReward;
        
        List<int> typeList = new List<int>();

        List<int> splitData_Free = GetSplitData(SeolPassServerTable.MonthlypassFreeReward);
        List<int> splitData_Ad = GetSplitData(SeolPassServerTable.MonthlypassAdReward);

        var tableData = TableManager.Instance.SeolPass.dataArray;

        int rewardedNum = 0;

        string free = ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassFreeReward].Value;
        string ad = ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassAdReward].Value;

        bool hasCostumeItem = false;

        for (int i = 0; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockamount);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(splitData_Free, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward1)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                free += $",{tableData[i].Id}";
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                rewardedNum++;
                if(!typeList.Contains(tableData[i].Reward1))
                {
                    typeList.Add(tableData[i].Reward1);
                }
            }

            //유료보상
            if (HasPassItem() && HasReward(splitData_Ad, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward2)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

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
            ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassFreeReward].Value = free;
            ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassAdReward].Value = ad;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            var e = typeList.GetEnumerator();
            
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(SeolPassServerTable.MonthlypassFreeReward, ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassFreeReward].Value);
            passParam.Add(SeolPassServerTable.MonthlypassAdReward, ServerData.seolPassServerTable.TableDatas[SeolPassServerTable.MonthlypassAdReward].Value);

            transactions.Add(TransactionValue.SetUpdate(SeolPassServerTable.tableName, SeolPassServerTable.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                if (hasCostumeItem)
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 아이템은 직접 수령해야 합니다.", null);
                }
                else
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                }
                //LogManager.Instance.SendLogType("ChildPass", "A", "A");
            });
        }
        else
        {
            if (hasCostumeItem)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 아이템은 직접 수령해야 합니다.", null);
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
            }
        }
    }
}
