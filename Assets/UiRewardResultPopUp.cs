using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UiRewardView;

public class UiRewardResultPopUp : SingletonMono<UiRewardResultPopUp>
{
    [SerializeField]
    private DungeonRewardView dungeonRewardView;
    [SerializeField]
    private GameObject rootObject;
    
    private List<RewardData> rewardList = new List<RewardData>();

    public List<RewardData> RewardList => rewardList;
    public UiRewardResultPopUp AddOrUpdateReward(Item_Type itemType, float itemValue)
    {
        int existingRewardIndex = rewardList.FindIndex(r => r.itemType == itemType);

        if (existingRewardIndex >= 0)
        {
            RewardData existingReward = rewardList[existingRewardIndex];
            existingReward.amount += itemValue;
            rewardList[existingRewardIndex] = existingReward;
        }
        else
        {
            rewardList.Add(new RewardData(itemType, itemValue));
        }

        return this;
    }

    public UiRewardResultPopUp Clear()
    {
        rewardList.Clear();

        return this;
    }

    public UiRewardResultPopUp Show()
    {
        rootObject.SetActive(true);

        dungeonRewardView.Initalize(rewardList);
        
        return this;
    }
    public void Initialize(List<RewardData> rewardData,int count)
    {
        List<RewardData> modifiedRewardData = new List<RewardData>();

        foreach (var data in rewardData)
        {
            modifiedRewardData.Add(new RewardData(data.itemType, data.amount * count));
        }

        rootObject.SetActive(true);

        dungeonRewardView.Initalize(modifiedRewardData);
    }
}
