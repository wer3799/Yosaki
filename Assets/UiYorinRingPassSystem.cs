using System;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class UiYorinRingPassSystem : MonoBehaviour
{
    [SerializeField]
    private UiYorinRingPassCell uiPassCellPrefab;

    [SerializeField]
    private List<Transform> cellParent =new List<Transform>();
    [SerializeField]
    private TextMeshProUGUI countText;

    private List<UiYorinRingPassCell> uiPassCellContainer = new List<UiYorinRingPassCell>();
    [SerializeField] private GameObject mask;
    [SerializeField]
    private TextMeshProUGUI maskTexts;
    private List<int> splitData_Free;
    private List<int> splitData_Ad;

    private int currentIdx = 0;
    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        SetText();
        SetMask(0);
    }
    public void SetMask(int idx)
    {
        currentIdx = idx;

        var tableData = TableManager.Instance.YorinPass.dataArray;

        var level = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).Value;
        
        for (int i = 0; i < tableData.Length; i++)
        {
            if(tableData[i].YORINPASSTYPE!=YorinPassType.NewGacha) continue;
            if (tableData[i].Masknumber != idx) continue;
            if (tableData[i].Unlockcondition > level)
            {
                mask.gameObject.SetActive(true);
                maskTexts.SetText($"영혼의 숲 처치 {tableData[i].Unlockcondition} 이상 시 구매할 수 있는 패스입니다.");
                break;
            }
            else
            {
                mask.gameObject.SetActive(false);
            }
        }
    }
    private void SetText()
    {
        
        int idx = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).Value;
        
        if (idx <= 0)
        {
            countText.SetText($"영혼의숲 기록을 보유하지 않았습니다.");
        }
        else
        {
            countText.SetText($"영혼의 숲 처치 수 : {idx}");
        }
    }
    
    private void Initialize()
    {
        var tableData = TableManager.Instance.YorinNewGachaPass.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            Match match = Regex.Match(tableData[i].Shopid, @"\d+");
        
            if (match.Success)
            {
                int number = int.Parse(match.Value);
                var prefab = Instantiate<UiYorinRingPassCell>(uiPassCellPrefab, cellParent[number]);
                uiPassCellContainer.Add(prefab);
            }
            else
            {
                Debug.LogError("숫자를 찾을 수 없습니다.");
            }
            

        }

        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            if (i < tableData.Length)
            {
                var passInfo = new PassInfo();

                //passInfo.key0 = tableData[i].Unlockstringid;
                passInfo.id = tableData[i].Id;
                passInfo.rewardType_New = tableData[i].Rewardid;
                passInfo.require = tableData[i].Requirelevel;

                passInfo.rewardType_Free = tableData[i].Reward1;
                passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
                passInfo.rewardType_Free_Key = YorinPassServerTable.ring_free;

                passInfo.rewardType_IAP = tableData[i].Reward2;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = tableData[i].Shopid;
                
                
                Match match = Regex.Match(tableData[i].Shopid, @"\d+");
        
                if (match.Success)
                {
                    int number = int.Parse(match.Value);
                    passInfo.passGrade = number;
                }
                else
                {
                    Debug.LogError("숫자를 찾을 수 없습니다.");
                }


                uiPassCellContainer[i].gameObject.SetActive(true);
                uiPassCellContainer[i].Initialize(passInfo);
            }
            else
            {
                uiPassCellContainer[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickAllReceiveButton()
    {
             
        string freeKey = YorinPassServerTable.ring_free;
        string adKey = "";
        var passTableData = TableManager.Instance.YorinPass.dataArray;

        var tableData = TableManager.Instance.YorinNewGachaPass.dataArray;

        for (int i = 0; i < passTableData.Length; i++)
        {
            if (passTableData[i].YORINPASSTYPE != YorinPassType.NewGacha) continue;
            if (passTableData[i].Masknumber != currentIdx) continue;
            adKey = passTableData[i].Productid;
            break;
        }
        
        //받은 보상
        var freeValue = int.Parse(ServerData.yorinPassServerTable.TableDatas[freeKey].Value);
        var adValue = int.Parse(ServerData.yorinPassServerTable.TableDatas[adKey].Value);
        
        int rewardedNum = 0;

        bool hasCostumeItem = false;

        UiRewardResultPopUp.Instance.Clear();
        
        for (int i = freeValue+1; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Requirelevel);

            if (canGetReward == false) break;

            //무료보상
            if (HasReward(freeKey, tableData[i].Id) == false)
            {
                UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);
                
                //ServerData.AddLocalValue((Item_Type)(int)tableData[i].Reward1, tableData[i].Reward1_Value);

                rewardedNum++;
                freeValue = i;
            }
        }
        if(HasPassItem(adKey))
        {
            for (int i = adValue+1; i < tableData.Length; i++)
            {
                if (CanGetReward(tableData[i].Requirelevel) == false) break;
                if (tableData[i].Shopid != adKey) continue;

                //유료보상
                if (HasReward(adKey, tableData[i].Rewardid) == false)
                {
                    UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)(int)tableData[i].Reward2, tableData[i].Reward2_Value);

                    rewardedNum++;
                    adValue = tableData[i].Rewardid;
                }
            }
        }

        if (rewardedNum > 0)
        {
            ServerData.yorinPassServerTable.TableDatas[freeKey].Value = $"{freeValue}";
            ServerData.yorinPassServerTable.TableDatas[adKey].Value = $"{adValue}";

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            Param passParam = new Param();

            
            using var e = UiRewardResultPopUp.Instance.RewardList.GetEnumerator();
            while (e.MoveNext())
            {
                ServerData.AddLocalValue(e.Current.itemType, e.Current.amount);
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType), ServerData.goodsTable.GetTableData(e.Current.itemType).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));


            passParam.Add(freeKey, ServerData.yorinPassServerTable.TableDatas[freeKey].Value);
            passParam.Add(adKey, ServerData.yorinPassServerTable.TableDatas[adKey].Value);

            transactions.Add(TransactionValue.SetUpdate(YorinPassServerTable.tableName, YorinPassServerTable.Indate, passParam));

            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                UiRewardResultPopUp.Instance.Show().Clear();
            });
        }
        else
        {
            PopupManager.Instance.ShowAlarmMessage("수령할 보상이 없습니다.");
        }
    }

    public bool CanGetReward(int require)
    {
        return ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).Value >= require;
    }

    private bool HasReward(string key,int id)
    {
        return int.Parse(ServerData.yorinPassServerTable.TableDatas[key].Value) >= id;
    }

    private bool HasPassItem(string key)
    {
        return ServerData.iapServerTable.TableDatas[key].buyCount.Value > 0;
    }
}
