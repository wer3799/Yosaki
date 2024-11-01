﻿using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UiTrainingPassSystem : MonoBehaviour
{
    [SerializeField]
    private UiTrainingPassCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiTrainingPassCell> uiPassCellContainer = new List<UiTrainingPassCell>();

    private ObscuredString passShopId;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.userInfoTable.GetTableData(UserInfoTable.killCountTotalWinterPass).Value += 1000000;
        }
    }
#endif

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        var tableData = TableManager.Instance.winterPass.dataArray;

        int interval = tableData.Length - uiPassCellContainer.Count;

        for (int i = 0; i < interval; i++)
        {
            var prefab = Instantiate<UiTrainingPassCell>(uiPassCellPrefab, cellParent);
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
                passInfo.rewardType_Free_Key = ChildPassServerTable.childFree;

                passInfo.rewardType_IAP = tableData[i].Reward2;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = ChildPassServerTable.childAd;

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
        string freeKey = ChildPassServerTable.childFree;
        string adKey = ChildPassServerTable.childAd;

        //받은 보상
        var freeValue = int.Parse(ServerData.childPassServerTable.TableDatas[freeKey].Value);
        var adValue = int.Parse(ServerData.childPassServerTable.TableDatas[adKey].Value);
        
        
        List<int> rewardTypeList = new List<int>();
        
        var tableData = TableManager.Instance.winterPass.dataArray;

        int rewardedNum = 0;

        bool hasCostumeItem = false;

        for (int i = freeValue+1; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockamount);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(freeKey, tableData[i].Id) == false)
            {
                if (((Item_Type)(tableData[i].Reward1)).IsCostumeItem())
                {
                    hasCostumeItem = true;
                    break;
                }

                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                if (rewardTypeList.Contains(tableData[i].Reward1) == false)
                {
                    rewardTypeList.Add(tableData[i].Reward1);
                }

                rewardedNum++;
                freeValue = i;
            }
        }
        if(HasPassItem())
        {
            for (int i = adValue+1; i < tableData.Length; i++)
            {
                if (CanGetReward(tableData[i].Unlockamount) == false) break;

                //유료보상
                if (HasReward(adKey, tableData[i].Id) == false)
                {
                    if (((Item_Type)(tableData[i].Reward2)).IsCostumeItem())
                    {
                        hasCostumeItem = true;
                        break;
                    }

                    ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);

                    if (rewardTypeList.Contains(tableData[i].Reward2) == false)
                    {
                        rewardTypeList.Add(tableData[i].Reward2);
                    }

                    rewardedNum++;
                    adValue = i;
                }
            }
        }

        if (hasCostumeItem)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 아이템은 직접 수령해야 합니다.", null);
            return;
        }

        if (rewardedNum > 0)
        {
            ServerData.childPassServerTable.TableDatas[ChildPassServerTable.childFree].Value = $"{freeValue}";
            ServerData.childPassServerTable.TableDatas[ChildPassServerTable.childAd].Value = $"{adValue}";

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            using var e = rewardTypeList.GetEnumerator();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(ChildPassServerTable.childFree, ServerData.childPassServerTable.TableDatas[ChildPassServerTable.childFree].Value);
            passParam.Add(ChildPassServerTable.childAd, ServerData.childPassServerTable.TableDatas[ChildPassServerTable.childAd].Value);

            transactions.Add(TransactionValue.SetUpdate(ChildPassServerTable.tableName, ChildPassServerTable.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                //LogManager.Instance.SendLogType("ChildPass", "A", "A");
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }


    private bool CanGetReward(int require)
    {
        int killCountTotal = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.killCountTotalWinterPass).Value;
        return killCountTotal >= require;
    }
    private bool HasReward(string key,int id)
    {
        return int.Parse(ServerData.childPassServerTable.TableDatas[key].Value) >= id;
    }

    private bool HasPassItem()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[UiTrainingPassBuyButton.productKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    public List<int> GetSplitData(string key)
    {
        List<int> returnValues = new List<int>();

        var splits = ServerData.childPassServerTable.TableDatas[key].Value.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            if (int.TryParse(splits[i], out var result))
            {
                returnValues.Add(result);
            }
        }

        return returnValues;
    }

    public void RefreshCells()
    {
        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            uiPassCellContainer[i].RefreshParent();
        }
    }
}
