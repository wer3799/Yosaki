using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RewardData
{
    public Item_Type itemType;
    public float amount;

    public RewardData(Item_Type itemType, float amount)
    {
        this.itemType = itemType;
        this.amount = amount;
    }
}
public class UiRewardView : MonoBehaviour
{


    [SerializeField]
    private Image rewardIcon;

    [SerializeField]
    private TextMeshProUGUI amountText;


    public void Initialize(RewardData rewardData)
    {
        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon(rewardData.itemType);
        var str = Utils.ConvertBigNum(rewardData.amount);
        
        amountText.SetText($"{str}개");
     
    }
}
