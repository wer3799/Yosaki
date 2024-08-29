using System;
using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class UiAdRewardCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private TextMeshProUGUI buyCountDesc;

    [SerializeField]
    private TextMeshProUGUI itemAmount;

    [SerializeField]
    private TextMeshProUGUI itemAmount_Costume;

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private int tableId;

    [SerializeField]
    Button button;

    
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    private AdRewardTableData tableData;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.etcServerTable.TableDatas[EtcServerTable.AdReward].AsObservable().Subscribe(e=>
        {
            ReFreshExchange();
        }).AddTo(this);

    }

    private void ReFreshExchange()
    {
        if (ServerData.etcServerTable.AdRewardRewarded(tableData.Id) == false)
        {
            buyCountDesc.SetText($"교환 가능");
            button.interactable = true;
        }
        else
        {
            buyCountDesc.SetText($"교환 불가");
            button.interactable = false;
        }
    }
    private void Initialize()
    {

        tableData = TableManager.Instance.adRewardTable.dataArray[tableId];

        itemIcon.gameObject.SetActive(IsCostumeItem() == false);
        skeletonGraphic.gameObject.SetActive(IsCostumeItem());

        if (IsCostumeItem() == false)
        {
            //price.SetText(Utils.ConvertBigNum(tableData.Price));
        }

        //스파인
        if (IsCostumeItem())
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();
            var idx = ServerData.costumeServerTable.TableDatas[itemKey].idx;
            skeletonGraphic.Clear();
            skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.GetCostumeAsset(idx);
            skeletonGraphic.Initialize(true);
            skeletonGraphic.SetMaterialDirty();

            var costumeTable = TableManager.Instance.Costume.dataArray[idx];

            if (itemAmount_Costume != null)
            {
                itemAmount_Costume.SetText($"(능력치 슬롯{costumeTable.Slotnum}개)");
            }
        }

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)tableData.Itemtype);

        itemAmount.SetText($"일일 획득량의 {tableData.Itemvalue * 100}%");

        itemName.SetText(CommonString.GetItemName((Item_Type)tableData.Itemtype));
        
        ReFreshExchange();
    }

    public void OnClickExchangeButton(bool isPopup)
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (isPopup)
            {
                PopupManager.Instance.ShowAlarmMessage("인터넷 연결을 확인해 주세요!");
            }
            return;
        }


        if (ServerData.etcServerTable.AdRewardRewarded(tableData.Id))
        {
            if (isPopup)
            {
                PopupManager.Instance.ShowAlarmMessage("더이상 교환하실 수 없습니다.");
            }
            return;
        }

        if (IsCostumeItem())
        {
            string itemKey = ((Item_Type)tableData.Itemtype).ToString();

            if (ServerData.costumeServerTable.TableDatas[itemKey].hasCostume.Value)
            {
                if (isPopup)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다!");
                }
                return;
            }
        }

        var type = (Item_Type)tableData.Itemtype;
        if (AdManager.Instance.HasRemoveAdProduct())
        {
            RewardRoutine(isPopup);
        }
        else
        {
            if (ItemTypeToAmount(type) > 0)
            {
                PopupManager.Instance.ShowYesNoPopup("알림",
                    $"광고를 보고 {CommonString.GetItemName(type)} {Utils.ConvertBigNum(ItemTypeToAmount(type))}개 획득하시겠습니까?",
                    () => { AdManager.Instance.ShowRewardedReward(RewardRoutine); }, () => { });
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup("알림",
                    $"해당 재화를 획득할 수 있는 컨텐츠를 플레이 한 후에 다시 시도해주세요!",
                    null);
            }
        }
    
            
    }


    private void RewardRoutine(bool isPopUp)
    {
        var realItemAmount = ItemTypeToAmount((Item_Type)tableData.Itemtype);

        if (realItemAmount < 1)
        {
            if (isPopUp)
            {
                PopupManager.Instance.ShowAlarmMessage($"해당 재화를 획득할 수 있는 컨텐츠를 플레이 한 후에 다시 시도해주세요!");
            }
            
            return;
        }
        
        ServerData.AddLocalValue((Item_Type)tableData.Itemtype, realItemAmount);

        if (isPopUp)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName((Item_Type)tableData.Itemtype)} {Utils.ConvertBigNum(realItemAmount)}개 교환 완료");
        }
        ServerData.etcServerTable.TableDatas[EtcServerTable.AdReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine((Item_Type)tableData.Itemtype));

    }
    private void RewardRoutine()
    {
        var realItemAmount = ItemTypeToAmount((Item_Type)tableData.Itemtype);
        
        ServerData.AddLocalValue((Item_Type)tableData.Itemtype, realItemAmount);

        
        PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName((Item_Type)tableData.Itemtype)} {Utils.ConvertBigNum(realItemAmount)}개 교환 완료");
        ServerData.etcServerTable.TableDatas[EtcServerTable.AdReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";
        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine((Item_Type)tableData.Itemtype));

    }

    private bool IsCostumeItem()
    {
        return ((Item_Type)tableData.Itemtype).IsCostumeItem();
    }

    private Coroutine syncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(0.5f);

    private float ItemTypeToAmount(Item_Type type)
    {
        var amountFrom1Day = 0f; 
        switch (type)
        {
            case Item_Type.GrowthStone:
                amountFrom1Day = SleepRewardReceiver.Instance.GetKilledEnemyPerMin(type)*GameBalance.oneDayConvertMin;
                break;
            case Item_Type.StageRelic:
                amountFrom1Day = SleepRewardReceiver.Instance.GetKilledEnemyPerMin(type)*GameBalance.oneDayConvertMin;
                break;
            case Item_Type.Marble:
                amountFrom1Day = SleepRewardReceiver.Instance.GetKilledEnemyPerMin(type)*GameBalance.oneDayConvertMin;
                break;
            case Item_Type.PeachReal:
                if (ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).Value > 0)
                {
                    amountFrom1Day = SleepRewardReceiver.Instance.GetKilledEnemyPerMin(type)*GameBalance.oneDayConvertMin;
                }
                else
                {
                    amountFrom1Day = SonMaximumReward();
                }
                break;
            case Item_Type.SmithFire:
                amountFrom1Day = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.smithClear].Value;
                break;
            case Item_Type.SP:
                amountFrom1Day = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiClear].Value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return amountFrom1Day * tableData.Itemvalue;
    }

    private float SonMaximumReward()
    {
        double score = ServerData.userInfoTable.TableDatas[UserInfoTable.sonScore].Value * GameBalance.BossScoreConvertToOrigin;

        var tableData = TableManager.Instance.SonReward.dataArray;

        var peachAmount = 0;
        for (int i = 0; i < tableData.Length; i++)
        {
            if (score < tableData[i].Score)
            {
                break;
            }
            float amount = tableData[i].Rewardvalue;
            peachAmount += (int)amount;
        }

        return peachAmount;
    }
      public IEnumerator SyncRoutine(Item_Type type)
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        
        if (IsCostumeItem())
        {
            Param costumeParam = new Param();

            string costumeKey = ((Item_Type)tableData.Itemtype).ToString();

            costumeParam.Add(costumeKey.ToString(), ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());

            Param goodsParam = new Param();

            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(type), ServerData.goodsTable.GetTableData(ServerData.goodsTable.ItemTypeToServerString(type)).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));
        }
        else
        { Param goodsParam = new Param();

            goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString((Item_Type)tableData.Itemtype), ServerData.goodsTable.GetTableData((Item_Type)tableData.Itemtype).Value);

            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        }

        Param etcParam = new Param();
        etcParam.Add(EtcServerTable.AdReward, ServerData.etcServerTable.TableDatas[EtcServerTable.AdReward].Value);
        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));


        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            if (IsCostumeItem())
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!!", null);
            }

        });
    }
}
