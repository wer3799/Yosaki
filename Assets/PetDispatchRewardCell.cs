using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UiTwelveRewardPopup;

public class PetDispatchRewardCell : MonoBehaviour
{
    private RewardItem rewardInfo;

    [SerializeField] private Image itemIcon;

    [SerializeField] private TextMeshProUGUI rewardAmount;

    public void Initialize(RewardItem rewardItem)
    {
        rewardInfo = rewardItem;
        
        UpdateUi();
    }

    private void UpdateUi()
    {
        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon(rewardInfo.ItemType);

        rewardAmount.SetText($"{Utils.ConvertBigNum(rewardInfo.ItemValue)}개");
    }
}