using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;


public class UiRankHonorBoard : SingletonMono<UiRankHonorBoard>
{
    public RankManager.RankInfo rankInfo;
    [SerializeField] private UiRankHonorRewardCell prefab;
    [SerializeField] private Transform parent;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI stageText;

    private List<UiRankHonorRewardCell> cellContainer = new List<UiRankHonorRewardCell>();
    public void Initialize(RankManager.RankInfo info)
    {
        rankInfo = info;
        
        SetText();
        
        MakeCell();
    }


    private void SetText()
    {
        nameText.SetText($"{rankInfo.NickName}");
        stageText.SetText($"{Utils.ConvertStage((int)rankInfo.Score+2)}단계");
    }

    private void MakeCell()
    {
        var tableData = TableManager.Instance.HonorReward.dataArray;

        if (tableData.Length <= cellContainer.Count)
        {
            return;
        }
        
        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate(prefab, parent);
            cell.Initialize(tableData[i]);
            cellContainer.Add(cell);
        }
    }

    public void OnClickAllReceiveButton()
    {
        if (rankInfo == null)
        {
            return;
        }
        
        var rewardIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.rankHonorRewardIdx).Value;
        
        var tableData = TableManager.Instance.HonorReward.dataArray;

        int rewardedNum = 0;

        UiRewardResultPopUp.Instance.Clear();
        
        for (int i = rewardIdx+1; i < tableData.Length; i++)
        {   
            bool canGetReward = rankInfo.Score  >= tableData[i].Stageid;

            if (canGetReward == false) break;

            //무료보상
            if (tableData[i].Id>=rewardIdx)
            {
                ServerData.AddLocalValue((Item_Type)(int)tableData[i].Rewardtype, tableData[i].Rewardvalue);


                UiRewardResultPopUp.Instance.AddOrUpdateReward((Item_Type)(int)tableData[i].Rewardtype, tableData[i].Rewardvalue);
                rewardedNum++;
                rewardIdx = i;
            }
            else
            {
                break;
            }
        }
        if (rewardedNum > 0)
        {
            ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.rankHonorRewardIdx).Value = rewardIdx;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param goodsParam = new Param();
            using var e = UiRewardResultPopUp.Instance.RewardList.GetEnumerator();
            while (e.MoveNext())
            {
                goodsParam.Add(ServerData.goodsTable.ItemTypeToServerString(e.Current.itemType), ServerData.goodsTable.GetTableData(e.Current.itemType).Value);
            }
            transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

            Param userinfo2Param = new Param();

            userinfo2Param.Add(UserInfoTable_2.rankHonorRewardIdx, ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.rankHonorRewardIdx).Value);

            transactions.Add(TransactionValue.SetUpdate(UserInfoTable_2.tableName, UserInfoTable_2.Indate, userinfo2Param));

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
}
