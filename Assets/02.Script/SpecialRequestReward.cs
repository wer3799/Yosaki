using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SpecialRequestReward : MonoBehaviour
{
    [SerializeField] private UiSpecialRequestClearRewardCell cell;

    [SerializeField] private Transform parent;

    [SerializeField] private UiSpecialRequestTotalRewardCell totalCell;
    [SerializeField] private Transform totalCellParent;
    
    
    [SerializeField] private UiSpecialRequestSpecialRewardCell specialCell;
    [SerializeField] private Transform specialCellParent;

    [SerializeField] private TextMeshProUGUI totalStarText;

    
    private int seasonIdx;
    private void Start()
    {
        seasonIdx = (int)ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.currentSeasonIdx).Value;
        
        MakeCell();
        MakeTotalCell();
        MakeSpecialCell();
        
        totalStarText.SetText($"({ServerData.specialRequestBossServerTable.GetTotalStar()}/{TableManager.Instance.SpecialRequestStarRewardTable.dataArray.Last().Starcondition})");
    }

    private void MakeCell()
    {
        var tableData = Utils.GetCurrentSeasonSpecialRequestData();

        var maxCount = 0;
        var list = new List<UiSpecialRequestClearRewardCell>();
        for (int i = 0; i < tableData.Stringid.Length; i++)
        {
            var prefab = Instantiate(cell, parent);
            prefab.Initialize(i);
            list.Add(prefab);
        }

        using var e = list.GetEnumerator();
        while (e.MoveNext())
        {
            e.Current.SetArrange();
        }
    }
    private void MakeTotalCell()
    {
        var tableData = TableManager.Instance.SpecialRequestStarRewardTable.dataArray;

        var list = new List<UiSpecialRequestTotalRewardCell>();
        
        for (var i = 0; i < tableData.Length; i++)
        {
            if (tableData[i].Startseasonid > seasonIdx) continue;
            if (tableData[i].Endseasonid < seasonIdx) continue;
            
            var prefab = Instantiate(totalCell, totalCellParent);
            prefab.Initialize(tableData[i]);
            list.Add(prefab);
        }
        
        using var e = list.GetEnumerator();
        while (e.MoveNext())
        {
            e.Current.SetArrange();
        }
    }
    private void MakeSpecialCell()
    {
        var tableData = TableManager.Instance.SpecialRequestRewardTable.dataArray;
        
        for (var i = 0; i < tableData.Length; i++)
        {
            if(tableData[i].Seasonid!=seasonIdx)continue;
            var prefab = Instantiate(specialCell, specialCellParent);
            prefab.Initialize(tableData[i]);
        }
        
    }
    
}
