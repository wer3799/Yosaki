using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiTreasureIndicator : MonoBehaviour
{

    enum TreasureType
    {
        DBT,
    }

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private StatusType statusType;
    [SerializeField] private Item_Type itemType;
    [SerializeField] private TreasureType treasureType;
    private void Start()
    {
        switch (treasureType)
        {
            case TreasureType.DBT:
            text.SetText($"{CommonString.GetItemName(itemType)} 1개당\n{CommonString.GetStatusName(statusType)} 효과 {GameBalance.difficultyBossTreasureAbilValue*100}% 강화");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
