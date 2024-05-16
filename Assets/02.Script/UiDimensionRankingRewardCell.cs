using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiDimensionRankingRewardCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI descText;

    public void Initialize(string rank, int amount)
    {
        rankText.SetText($"{rank}등");
        descText.SetText($"{CommonString.GetItemName(Item_Type.ClearTicket)} {amount}개");
    }
}
