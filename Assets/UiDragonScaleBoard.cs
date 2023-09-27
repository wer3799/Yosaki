using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiDragonScaleBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilDescription;


    private void OnEnable()
    {
        UpdateAbilText1();
    }

    private void UpdateAbilText1()
    {
        string abilDesc = string.Empty;
        abilDesc += $"{CommonString.GetStatusName(StatusType.SuperCritical24DamPer)} {PlayerStats.GetDragonScaleAbilHasEffect() * 100f}";
        
        abilDescription.SetText(abilDesc);
    }
}