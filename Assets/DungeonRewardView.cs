using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UiRewardView;

public class DungeonRewardView : MonoBehaviour
{
    [SerializeField]
    private Transform viewParent;

public void Initalize(List<RewardData> rewardDatas)
    {
        var views = viewParent.GetComponentsInChildren<UiRewardView>();

        if (views.Length != 0)
        {
            for (int i = 0; i < views.Length; i++)
            {
                Destroy(views[i].gameObject);
            }
        }

        var prefab = CommonPrefabContainer.Instance.uiRewardViewPrefab;

        for (int i = 0; i < rewardDatas.Count; i++)
        {
            if (rewardDatas[i].amount < 1) continue;
            var view = Instantiate<UiRewardView>(prefab, viewParent);

            view.Initialize(rewardDatas[i]);
        }
    }
}
