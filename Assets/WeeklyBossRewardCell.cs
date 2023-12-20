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

public class WeeklyBossRewardCell : MonoBehaviour
{
    private RewardItem rewardInfo;

    [SerializeField] private Image itemIcon;

    [SerializeField] private Image bgImage;
    
    [SerializeField] private List<Sprite> bgSprites;

    [SerializeField] private TextMeshProUGUI rewardAmount;

    public void Initialize(RewardItem rewardItem, int grade=0)
    {
        rewardInfo = rewardItem;

        bgImage.sprite = bgSprites[grade];
        
        UpdateUi();
    }

    private void UpdateUi()
    {
        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon(rewardInfo.ItemType);

        rewardAmount.SetText($"{Utils.ConvertBigNum(rewardInfo.ItemValue)}개");
    }
}