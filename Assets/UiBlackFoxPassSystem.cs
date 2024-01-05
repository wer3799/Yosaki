using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UiBlackFoxPassSystem : MonoBehaviour
{
    [SerializeField]
    private UiBlackFoxPassCell uiPassCellPrefab;

    [SerializeField]
    private Transform cellParent;
    [SerializeField]
    private TextMeshProUGUI countText;

    private List<UiBlackFoxPassCell> uiPassCellContainer = new List<UiBlackFoxPassCell>();

    private List<int> splitData_Free;
    private List<int> splitData_Ad;

    private void Start()
    {
        Initialize();

        var str = PlayerStats.GetBlackFoxGrade() < 0 ? "검은 구미호전 단계 : 없음" : $"검은 구미호전 단계 : {PlayerStats.GetBlackFoxGrade()+1}";
        
        countText.SetText($"{str}");
    }

    
    
    private void Initialize()
    {
        var tableData = TableManager.Instance.BlackFoxPass.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var prefab = Instantiate<UiBlackFoxPassCell>(uiPassCellPrefab, cellParent);
            uiPassCellContainer.Add(prefab);
        }

        for (int i = 0; i < uiPassCellContainer.Count; i++)
        {
            if (i < tableData.Length)
            {
                var passInfo = new PassInfo();

                passInfo.require = tableData[i].Unlockcondition;
                passInfo.id = tableData[i].Id;

                passInfo.rewardType_Free = tableData[i].Reward1;
                passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
                passInfo.rewardType_Free_Key = ColdSeasonPassServerTable.blackFoxFree;

                passInfo.rewardType_IAP = tableData[i].Reward2;
                passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
                passInfo.rewardType_IAP_Key = ColdSeasonPassServerTable.blackFoxAd;

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
        
        string freeKey = ColdSeasonPassServerTable.blackFoxFree;
        string adKey = ColdSeasonPassServerTable.blackFoxAd;

        //받은 보상
        var freeValue = int.Parse(ServerData.coldSeasonPassServerTable.TableDatas[freeKey].Value);
        var adValue = int.Parse(ServerData.coldSeasonPassServerTable.TableDatas[adKey].Value);
        
        
        List<int> rewardTypeList = new List<int>();
        
        var tableData = TableManager.Instance.BlackFoxPass.dataArray;

        int rewardedNum = 0;

        bool hasCostumeItem = false;

        for (int i = freeValue+1; i < tableData.Length; i++)
        {
            bool canGetReward = CanGetReward(tableData[i].Unlockcondition);

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
                if (CanGetReward(tableData[i].Unlockcondition) == false) break;

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
            ServerData.coldSeasonPassServerTable.TableDatas[freeKey].Value = $"{freeValue}";
            ServerData.coldSeasonPassServerTable.TableDatas[adKey].Value = $"{adValue}";

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            var e = rewardTypeList.GetEnumerator();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)e.Current), ServerData.goodsTable.GetTableData((Item_Type)e.Current).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param passParam = new Param();

            passParam.Add(freeKey, ServerData.coldSeasonPassServerTable.TableDatas[freeKey].Value);
            passParam.Add(adKey, ServerData.coldSeasonPassServerTable.TableDatas[adKey].Value);

            transactions.Add(TransactionValue.SetUpdate(ColdSeasonPassServerTable.tableName, ColdSeasonPassServerTable.Indate, passParam));

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

    public bool CanGetReward(int require)
    {
        return PlayerStats.GetBlackFoxGrade() >= require;
    }

    private bool HasReward(string key,int id)
    {
        return int.Parse(ServerData.coldSeasonPassServerTable.TableDatas[key].Value) >= id;
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
        return ServerData.iapServerTable.TableDatas[PassBuyButton.blackfoxPassKey].buyCount.Value > 0;
    }
}
