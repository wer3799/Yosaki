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

    [SerializeField] private Image bgImage;

    [SerializeField] private List<Sprite> _sprites;
    
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

        if (tableIdx == 0 || tableIdx == 5 || tableIdx == 9 || tableIdx == 14)
        {
            bgImage.sprite = _sprites[0];
        }
        else
        {
            bgImage.sprite = _sprites[1];
        }
        
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
