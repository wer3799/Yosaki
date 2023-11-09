using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class SuhoBossAbilDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.SuhoTreasure).AsObservable().Subscribe(e =>
        {
            text.SetText($"{CommonString.GetItemName(Item_Type.SuhoTreasure)} 1개당\n수호베기 효과 증폭\n 증폭 : {+PlayerStats.GetEnhanceSuhoCritical()*100}%");
        }).AddTo(this);
    }
}
