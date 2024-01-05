using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Image goodsImage;

    public void Initialize(Item_Type type, string text)
    {
        goodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(type);
        descText.SetText(text);
    }
    public void Initialize(Item_Type type, double amount)
    {
        goodsImage.sprite = CommonUiContainer.Instance.GetItemIcon(type);
        descText.SetText(Utils.ConvertNum(amount));
    }
}
