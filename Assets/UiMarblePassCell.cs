using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using BackEnd;
using TMPro;
using UnityEngine.UI.Extensions;

public class UiMarblePassCell : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI itemAmount;
    [SerializeField]
    private TextMeshProUGUI tableIdxText;

    [SerializeField] private GameObject effect;

    private MarbleEventData marbleEventData;

    [SerializeField] int tableIdx= 0;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        marbleEventData = TableManager.Instance.MarbleEvent.dataArray[tableIdx];

        SetAmount();

        SetItemIcon();
        
        tableIdxText.SetText($"{tableIdx+1}");
    }

    private void SetAmount()
    {
        itemAmount.SetText(Utils.ConvertBigNum(marbleEventData.Itemvalue)); 
    }

    private void SetItemIcon()
    {
        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)marbleEventData.Itemtype);
    }


    public void SetEffect(bool _bool)
    {
        effect.SetActive(_bool);
    }
}
