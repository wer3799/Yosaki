using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiByeolHoAbilCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI abilityText;

    public void Initialize(ByeolhoData byeolhoData)
    {
        titleText.SetText($"{Utils.ColorToHexString(CommonUiContainer.Instance.itemGradeColor[byeolhoData.Grade],byeolhoData.Title_Name)}");

        abilityText.SetText($"{CommonString.GetStatusName((StatusType)byeolhoData.Abil_Type)} {Utils.ConvertNum(byeolhoData.Abil_Value*100, 2)}%");
    }
    
}
