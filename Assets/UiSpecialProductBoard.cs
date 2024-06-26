using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiSpecialProductBoard : MonoBehaviour
{

    [SerializeField] private UiEventPackageBonusCell prefab;
    [SerializeField] private TextMeshProUGUI descText;

    [SerializeField] private Transform parent;
    

    private void Start()
    {
        MakeCell();

        SetText();
        
        Subscribe();
    }

    private void Subscribe()
    {
        IAPManager.Instance.WhenBuyComplete.AsObservable()
            .Throttle(TimeSpan.FromSeconds(0.5f))
            .Subscribe(e =>
        {
            SetText();
        }).AddTo(this);
    }

    private void SetText()
    {
        descText.SetText($"구입 횟수에 따라\n추가 아이템 획득!\n<color=yellow>구매 횟수 : {GetEventPackageBuyCount()}회</color>");
    }

    private void MakeCell()
    {
        var tableData = TableManager.Instance.EventPackageBonus.dataArray;

        foreach (var t in tableData)
        {
            var cell = Instantiate(prefab, parent);
            cell.Initialize(t);
        }
    }
    
    private int GetEventPackageBuyCount()
    {
        var sum = 0;
        for (int i = 0; i < GameBalance.specialProductId.Count; i++)
        {
            sum += (int)ServerData.iapServerTable.TableDatas[GameBalance.specialProductId[i]].buyCount.Value;
        }
        return sum;
    }
    
}
