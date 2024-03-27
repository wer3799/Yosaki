using System.Collections;
using System.Collections.Generic;
using BackEnd;
using GoogleMobileAds.Api;
using Newtonsoft.Json;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiSpecialRequestTotalRewardCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI requireText;
    
    [SerializeField] private ItemView rewardView;
    [SerializeField] private Transform rewardViewParent;

    [SerializeField] private Button rewardButton;

    private List<ItemView> cellContainer = new List<ItemView>();
    private SpecialRequestStarRewardTableData tableData;
    public void Initialize(SpecialRequestStarRewardTableData data)
    {
        tableData = data;

        requireText.SetText($"{data.Starcondition}");

        var cellCount = tableData.Rewardtype.Length;

        while (cellCount > cellContainer.Count)
        {
            var prefab = Instantiate(rewardView, rewardViewParent);
            cellContainer.Add(prefab);
        }
        
        for (var i = 0; i < cellContainer.Count; i++)
        {
            if (i < cellCount)
            {
                cellContainer[i].gameObject.SetActive(true);
                cellContainer[i].Initialize((Item_Type)tableData.Rewardtype[i],tableData.Rewardvalue[i]);
            }
            else
            {
                cellContainer[i].gameObject.SetActive(false);
            }
        }
        
        rewardButton.interactable =tableData.Starcondition <= ServerData.specialRequestBossServerTable.GetTotalStar() ;
        
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestTotalRewardIdx)
            .AsObservable()
            .Subscribe(e =>
            {
                if (e >= tableData.Id)
                {
                    transform.SetAsLastSibling();
                }
                
                rewardButton.gameObject.SetActive(e < tableData.Id);
            })
            .AddTo(this);
    }
    public void SetArrange()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestTotalRewardIdx).Value >= tableData.Id)
        {
            transform.SetAsLastSibling();
        }
    }
    private bool IsClear()
    {
        return ServerData.specialRequestBossServerTable.GetTotalStar() >= tableData.Starcondition;
    }
    private bool IsRewarded()
    {
        return ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestTotalRewardIdx).Value >= tableData.Id;
    }
    
    public void OnClickButton()
    {
        
        List<TransactionValue> transactionList = new List<TransactionValue>();
        Param userInfo2Param = new Param();
        Param goodsParam = new Param();
        UiRewardResultPopUp.Instance.Clear();
        
        
        var current = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestTotalRewardIdx).Value;

        var score = ServerData.specialRequestBossServerTable.GetTotalStar();
        
        var data = TableManager.Instance.SpecialRequestStarRewardTable.dataArray;

        var rewardIdx = -1;
        for (int i = current+1; i < data.Length; i++)
        {
            if (score < data[i].Starcondition) break;
            for (int j = 0; j < data[i].Rewardtype.Length; j++)
            {
                UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)data[i].Rewardtype[j], data[i].Rewardvalue[j]);
                rewardIdx = i;
            }
        }


        if (UiRewardResultPopUp.Instance.RewardList.Count < 1)
        {
            return;
        }

        using var e = UiRewardResultPopUp.Instance.RewardList.GetEnumerator();

        
        
        while (e.MoveNext())
        {
            var type = ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType);
            ServerData.goodsTable.AddLocalData(type,e.Current.amount);
            goodsParam.Add(type, ServerData.goodsTable.GetTableData(type).Value);
        }
        
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestTotalRewardIdx).Value = rewardIdx;
        userInfo2Param.Add(UserInfoTable_2.specialRequestTotalRewardIdx, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.specialRequestTotalRewardIdx).Value);
        //패스 보상
        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userInfo2Param));
        
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        
        ServerData.SendTransactionV2(transactionList,successCallBack: () =>
        {
            UiRewardResultPopUp.Instance.Show().Clear();
        });
    }
}
