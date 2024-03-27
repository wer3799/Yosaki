using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ExchangeData
{
    public string key;
    public int maxExchanceCount;

    public RewardItem rewardItem;
    
    public RewardItem costItem;

    public void Initialize(RewardItem rewardItem, RewardItem costItem, int maxExchanceCount = 0, string key = "")
    {
        this.rewardItem = rewardItem;
        this.costItem = costItem;
        this.key = key;
        this.maxExchanceCount = maxExchanceCount;
    }
}
public class UiShopCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private TextMeshProUGUI buyCountDesc;

    [SerializeField]
    private TextMeshProUGUI itemAmount;
    
    [SerializeField]
    private TextMeshProUGUI price;


    [SerializeField]
    private Image rewardItemIcon;

    [SerializeField]
    private Image costItemIcon;
    

    [SerializeField] private GameObject allExchangeButton;

    private ExchangeData data;

    private void Subscribe()
    {
        if (string.IsNullOrEmpty(data.key) == false)
        {
            ServerData.userInfoTable_2.TableDatas[data.key].AsObservable().Subscribe(e =>
            {

                buyCountDesc.SetText($"교환 가능 : {e}/{data.maxExchanceCount}");
                
            }).AddTo(this);
        }
        else
        {
            buyCountDesc.SetText("");
        }
    }

    public void Initialize(ExchangeData _data)
    {
        data = _data;

        price.SetText(Utils.ConvertBigNum(data.costItem.ItemValue));


        rewardItemIcon.sprite = CommonUiContainer.Instance.GetItemIcon(data.rewardItem.ItemType);
        
        costItemIcon.sprite = CommonUiContainer.Instance.GetItemIcon(data.costItem.ItemType);

        itemAmount.SetText(Utils.ConvertBigNum(data.rewardItem.ItemValue) + "개");

        itemName.SetText(CommonString.GetItemName(data.rewardItem.ItemType));
        
        allExchangeButton.SetActive(string.IsNullOrEmpty(data.key) == true);


        Subscribe();
    }    
    
    public void OnClickExchangeButton()
    {
  
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
            return;
        }


        if (string.IsNullOrEmpty(data.key) == false)
        {
            if (ServerData.userInfoTable_2.TableDatas[data.key].Value >= data.maxExchanceCount)
            {
                PopupManager.Instance.ShowAlarmMessage("더이상 교환하실 수 없습니다.");
                return;
            }
        }

        int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(data.costItem.ItemType).Value;

        if (currentEventItemNum < data.costItem.ItemValue)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName(data.costItem.ItemType),JongsungType.Type_IGA)} 부족합니다.");
            return;
        }

        var stringId = ServerData.goodsTable.ItemTypeToServerString(data.costItem.ItemType);
        
        PopupManager.Instance.ShowAlarmMessage("교환 완료");

        //로컬
        ServerData.goodsTable.GetTableData(stringId).Value -= data.costItem.ItemValue;


        if (string.IsNullOrEmpty(data.key) == false)
        {
            ServerData.userInfoTable_2.TableDatas[data.key].Value++;
        }

        ServerData.AddLocalValue(data.rewardItem.ItemType, data.rewardItem.ItemValue);

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());

    }
    public void OnClickExchangeAllButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "전부 교환 합니까?", () =>
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
                return;
            }


            if (string.IsNullOrEmpty(data.key) == false)
            {
                //주간상품은 해당 버튼이 없어야 함.
                return;
            }

            var stringId = ServerData.goodsTable.ItemTypeToServerString(data.costItem.ItemType);

            
            int currentEventItemNum = (int)ServerData.goodsTable.GetTableData(stringId).Value;

            if (currentEventItemNum < data.costItem.ItemValue)
            {
                PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetJongsung(CommonString.GetItemName(data.costItem.ItemType),JongsungType.Type_IGA)} 부족합니다.");

                return;
            }

        
            PopupManager.Instance.ShowAlarmMessage("교환 완료");
        
            var exchangeCount = (int)(currentEventItemNum / data.costItem.ItemValue);

            //로컬
            ServerData.goodsTable.GetTableData(stringId).Value -= data.costItem.ItemValue * exchangeCount;


            ServerData.AddLocalValue(data.rewardItem.ItemType, data.rewardItem.ItemValue * exchangeCount);

            if (syncRoutine != null)
            {
                CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
            }

            syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
        }, null);
    }
   

    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);

    public IEnumerator SyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        
        var stringId = ServerData.goodsTable.ItemTypeToServerString(data.costItem.ItemType);


        goodsParam.Add(stringId, ServerData.goodsTable.GetTableData(stringId).Value);


        goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(data.rewardItem.ItemType), ServerData.goodsTable.GetTableData(data.rewardItem.ItemType).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        

        Param userInfo2Param = new Param();

        if (string.IsNullOrEmpty(data.key) == false)
        {
            userInfo2Param.Add(data.key, ServerData.userInfoTable_2.TableDatas[data.key].Value);
        }

        if (userInfo2Param.Count > 0)
        {
            transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));
        }

        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            //   LogManager.Instance.SendLogType("chuseokExchange", "Costume", ((Item_Type)tableData.Itemtype).ToString());
        });
    }
}
