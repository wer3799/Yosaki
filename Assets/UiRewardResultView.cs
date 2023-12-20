using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UiRewardView;

public class UiRewardResultView : MonoBehaviour
{
    [SerializeField]
    private DungeonRewardView dungeonRewardView;
    [SerializeField]
    private GameObject rootObject;

    public void Initialize(List<RewardData> rewardData)
    {
        rootObject.SetActive(true);

        dungeonRewardView.Initalize(rewardData);
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
