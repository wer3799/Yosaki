using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAMiniGameRewardPopup : MonoBehaviour
{
    [SerializeField]
    private UiSAMinigameRewardCell uiCellPrefab;
    [SerializeField]
    private Transform cellParent;
    [SerializeField]
    private UiSAMinigameHighScoreRewardCell uiCellPrefab2;
    [SerializeField]
    private Transform cellParent2;
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.SecondMiniGame.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var prefab = Instantiate<UiSAMinigameRewardCell>(uiCellPrefab, cellParent);
            var passInfo = new DamagePassInfo();

            passInfo.require = tableData[i].Accumulatescore;
            passInfo.id = tableData[i].Id;

            passInfo.rewardType_Free = tableData[i].Reward1;
            passInfo.rewardTypeValue_Free = tableData[i].Reward1_Value;
            passInfo.rewardType_Free_Key = ColdSeasonPassServerTable.secondAccumul;
            //
            // passInfo.rewardType_IAP = tableData[i].Reward2;
            // passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
            // passInfo.rewardType_IAP_Key = ColdSeasonPassServerTable.gangChuldAd;
            
            prefab.Initialize(passInfo);
        }
        for (int i = 0; i < tableData.Length; i++)
        {
            var prefab = Instantiate<UiSAMinigameHighScoreRewardCell>(uiCellPrefab2, cellParent2);
            var passInfo = new DamagePassInfo();

            passInfo.require = tableData[i].Topscore;
            passInfo.id = tableData[i].Id;

            passInfo.rewardType_Free = tableData[i].Reward2;
            passInfo.rewardTypeValue_Free = tableData[i].Reward2_Value;
            passInfo.rewardType_Free_Key = ColdSeasonPassServerTable.secondTop;
            //
            // passInfo.rewardType_IAP = tableData[i].Reward2;
            // passInfo.rewardTypeValue_IAP = tableData[i].Reward2_Value;
            // passInfo.rewardType_IAP_Key = ColdSeasonPassServerTable.gangChuldAd;
            
            prefab.Initialize(passInfo);
        }

    }
}
