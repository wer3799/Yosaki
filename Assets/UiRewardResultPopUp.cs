using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UiRewardView;

public class UiRewardResultPopUp : SingletonMono<UiRewardResultPopUp>
{
    [SerializeField]
    private DungeonRewardView dungeonRewardView;
    [SerializeField]
    private GameObject rootObject;

    [SerializeField] private Button button;
    [SerializeField] private Button mask;
    
    private List<RewardData> rewardList = new List<RewardData>();
    private UnityAction buttonClickAction;
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

    public UiRewardResultPopUp AddEvent(UnityAction action)
    {
        buttonClickAction = action;
        
        button.onClick.AddListener(action);

        mask.onClick.AddListener(action);

        return this;
    }

    public UiRewardResultPopUp RemoveEvent()
    {
        button.onClick.RemoveListener(buttonClickAction);
        mask.onClick.RemoveListener(buttonClickAction);
        return this;
    }
    public UiRewardResultPopUp RemoveAllEvent()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(SetOff);
        mask.onClick.RemoveAllListeners();
        mask.onClick.AddListener(SetOff);
        return this;
    }

    private void SetOff()
    {
        rootObject.SetActive(false);
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
