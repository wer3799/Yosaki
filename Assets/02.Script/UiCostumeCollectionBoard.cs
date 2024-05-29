using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class UiCostumeCollectionBoard : MonoBehaviour
{
    [SerializeField]
    private CostumeCollectionCell costumeCollectionCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<CostumeCollectionCell> costumeCollectionCellList = new List<CostumeCollectionCell>();

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI costumeAmount;
    
    [SerializeField] private UiRewardResultView _uiRewardResultView;

    private void OnEnable()
    {
        ActiveCheck();
    }

    private void ActiveCheck()
    {
        int costumeAmount = ServerData.costumeServerTable.GetCostumeHasAmount();

        if (costumeAmount < GameBalance.costumeCollectionUnlockNum)
        {
            PopupManager.Instance.ShowAlarmMessage($"외형 {GameBalance.costumeCollectionUnlockNum}개 이상일때 해금 됩니다!");
            this.gameObject.SetActive(false);
        }
    }


    void Start()
    {
        Initialize();

    }

    public void Initialize()
    {
        var stageDatas = TableManager.Instance.costumeCollection.dataArray;

        for (int i = 0; i < stageDatas.Length; i++)
        {
            costumeCollectionCellList.Add(Instantiate<CostumeCollectionCell>(costumeCollectionCellPrefab, cellParent));

        }
        for (int i = 0; i < costumeCollectionCellList.Count; i++)
        {
            costumeCollectionCellList[i].Initialize(stageDatas[i]);
        }

        costumeAmount.SetText($"외형 {ServerData.costumeServerTable.GetCostumeHasAmount()}개 보유중\n" +
            $"외형 강화 효과 {PlayerStats.IsCostumeCollectionEnhance()}배 적용됨");
    }


    public void OnClickAllReceive()
    {
        int costumeAmount = ServerData.costumeServerTable.GetCostumeHasAmount();
        
        var tableData = TableManager.Instance.costumeCollection.dataArray;

        List<RewardItem> rewards = new List<RewardItem>();
        
        for (int i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Require > costumeAmount) break;
            if (ServerData.etcServerTable.HasCostumeColectionFreeReward(i) == true) continue;
            Utils.AddOrUpdateReward(ref rewards,(Item_Type)tableData[i].Rewardtype,tableData[i].Rewardvalue);
            ServerData.etcServerTable.TableDatas[EtcServerTable.CostumeCollectionFreeReward].Value += $"{BossServerTable.rewardSplit}{i}";
        }
        
        for (int i = 0; i < tableData.Length; i++)
        {
            if (HasCostumePassItem() == false) break;
            if (tableData[i].Require > costumeAmount) break;
            if (ServerData.etcServerTable.HasCostumeColectionAdReward(i) == true) continue;
            Utils.AddOrUpdateReward(ref rewards,(Item_Type)tableData[i].Rewardtype2,tableData[i].Rewardvalue2);
            ServerData.etcServerTable.TableDatas[EtcServerTable.CostumeCollectionAdReward].Value += $"{BossServerTable.rewardSplit}{i}";
        }

        if (rewards.Count > 0)
        {            
            
            List<TransactionValue> transactions = new List<TransactionValue>();

            using var e =rewards.GetEnumerator();
            
            Param goodsParam = new Param();
            while (e.MoveNext())
            {
                ServerData.AddLocalValue(e.Current.ItemType, e.Current.ItemValue);
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current.ItemType), ServerData.goodsTable.GetTableData(e.Current.ItemType).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param collectionParam = new Param();

            collectionParam.Add(EtcServerTable.CostumeCollectionFreeReward, ServerData.etcServerTable.TableDatas[EtcServerTable.CostumeCollectionFreeReward].Value);
            collectionParam.Add(EtcServerTable.CostumeCollectionAdReward, ServerData.etcServerTable.TableDatas[EtcServerTable.CostumeCollectionAdReward].Value);
            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, collectionParam));
            ServerData.SendTransactionV2(transactions, successCallBack: () =>
            {
                //PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "보상을 전부 수령했습니다", null);
                //LogManager.Instance.SendLogType("LevelPass", "A", "A");
                
                List<RewardData> rewardData = new List<RewardData>();
                using var e = rewards.GetEnumerator();
                for (int i = 0 ;  i < rewards.Count;i++)
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
            PopupManager.Instance.ShowAlarmMessage("받을 보상이 없습니다.");
        }
        
    }

    private bool HasCostumePassItem()
    {
        return ServerData.iapServerTable.TableDatas[UiCostumeCollectionPassBuyButton.costumePassKey].buyCount.Value > 0;
    }

}
