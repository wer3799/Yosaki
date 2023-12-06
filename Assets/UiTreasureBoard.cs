using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiTreasureBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilDescription;

    enum treasureType
    {
        DragonPalace,
    }

    [SerializeField] private treasureType _treasureType;

    private void OnEnable()
    {
        UpdateAbilityText();
    }

    private void UpdateAbilityText()
    {
        string abilDesc = string.Empty;

        switch (_treasureType)
        {
            case treasureType.DragonPalace:
                abilDesc += $"{CommonString.GetStatusName(StatusType.SuperCritical28DamPer)} {Utils.ConvertNum(PlayerStats.GetDragonPalaceTreasureAbilHasEffect() * 100f, 2)}";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        abilDescription.SetText(abilDesc);
    }
}