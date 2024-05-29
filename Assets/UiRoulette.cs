using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using BackEnd;
using static UiRewardView;

public class UiRoulette : MonoBehaviour
{

    private List<BonusRouletteData> tableDataShuffled;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private UiRewardResultView uiRewardResultView;

    private void Start()
    {
        LoadTableData();
    }

    private void LoadTableData()
    {
        tableDataShuffled = TableManager.Instance.BonusRoulette.dataArray.ToList();

        tableDataShuffled.Shuffle();
    }

    private int GetRandomIdx()
    {
        return Utils.GetRandomIdx(tableDataShuffled.Select(e => e.Prob).ToList());
    }

    private int GetRewardAmount(int randIdx)
    {
        var tableData = tableDataShuffled[randIdx];

        return Random.Range((int)tableData.Min, (int)tableData.Max);
    }

    private Item_Type GetRewardType(int randIdx)
    {
        var tableData = tableDataShuffled[randIdx];

        return (Item_Type)tableData.Itemtype;
    }

    public void OnClickAllUseButton()
    {
        animator.SetTrigger("Play");

        int gachaNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value;

        if (gachaNum == 0)
        {
            PopupManager.Instance.ShowAlarmMessage("복주머니가 부족합니다.");
            return;
        }

        ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value = 0f;

        Dictionary<Item_Type, float> rewards = new Dictionary<Item_Type, float>();

        for (int i = 0; i < gachaNum; i++)
        {
            int randIdx = GetRandomIdx();
            
            Item_Type rewardType = GetRewardType(randIdx);

            //각성시
            if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateGold).Value > 0)
            {
                while ((int)rewardType == 0)
                {
                    randIdx = GetRandomIdx();
                    rewardType = GetRewardType(randIdx);
                }
            }
            
            float rewardAmount = GetRewardAmount(randIdx);
            
            if (rewards.ContainsKey(rewardType) == false)
            {
                rewards.Add(rewardType, 0);
            }

            rewards[rewardType] += rewardAmount;

            if (rewardType == Item_Type.Gold)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value += rewardAmount;
            }
            //티켓
            else if (rewardType == Item_Type.Ticket)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value += rewardAmount;
            }
            //매직스톤
            else if (rewardType == Item_Type.GrowthStone)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value += rewardAmount;
            }
            //보석
            else if (rewardType == Item_Type.Jade)
            {
                ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardAmount;
            }
        }

        //서버
        List<RewardData> rewardViewData = new List<RewardData>();

        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param goodsParam = new Param();

        goodsParam.Add(GoodsTable.BonusSpinKey, ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value);

        using var e = rewards.GetEnumerator();

        while (e.MoveNext())
        {
            rewardViewData.Add(new RewardData(e.Current.Key, e.Current.Value));

            if (e.Current.Key == Item_Type.Gold)
            {
                goodsParam.Add(GoodsTable.Gold, ServerData.goodsTable.GetTableData(GoodsTable.Gold).Value);
            }
            //티켓
            else if (e.Current.Key == Item_Type.Ticket)
            {
                goodsParam.Add(GoodsTable.Ticket, ServerData.goodsTable.GetTableData(GoodsTable.Ticket).Value);
            }
            //매직스톤
            else if (e.Current.Key == Item_Type.GrowthStone)
            {
                goodsParam.Add(GoodsTable.GrowthStone, ServerData.goodsTable.GetTableData(GoodsTable.GrowthStone).Value);
            }
            //보석
            else if (e.Current.Key == Item_Type.Jade)
            {
                goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
            }
        }

        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        YorinMissionManager.UpdateYorinMissionClear(YorinMissionKey.YMission5_5, 1);
        ServerData.SendTransaction(transactionList);

        //결과화면

        uiRewardResultView.gameObject.SetActive(true);

        uiRewardResultView.Initialize(rewardViewData);

        //로그
        string log = string.Empty;

        for (int i = 0; i < rewardViewData.Count; i++)
        {
            log += $"{rewardViewData[i].itemType}/{rewardViewData[i].amount} ";
        }

     //   LogManager.Instance.SendLog("복주머니사용", log);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value += 10;
        }
    }
#endif
}
