using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UiDimensionSweepBoard : MonoBehaviour
{
    
    [SerializeField] private DimensionEquiepmentCollectionCell sweepCell;
    [SerializeField] private Transform sweepCellParent;
    [SerializeField] private TextMeshProUGUI titleText; 
    [SerializeField] private TextMeshProUGUI gradeProbText; 
    [SerializeField] private TextMeshProUGUI levelProbText;

    private void Start()
    {
        // currentId = UiDimensionBoard.Instance.GetRank();
        // SetUI(currentId);
    }

    private int currentId = 0;
    private void SetUI(int idx)
    {
        var tableData = TableManager.Instance.DimensionDungeon.dataArray[idx];
        titleText.SetText($"{idx+1}단계 소탕 정보");
        
       // using var e = clearRewards.GetEnumerator();
        // while (e.MoveNext())
        // {
        //     e.Current.gameObject.SetActive(false);
        // }
        for (int i = 0; i < tableData.Rewardtype.Length; i++)
        {
         //   clearRewards[i].gameObject.SetActive(true);
        //    clearRewards[i].Initialize((Item_Type)tableData.Rewardtype[i],tableData.Rewardvalue[i]);
        }
    }
    // Start is called before the first frame update
    public void OnClickLeftButton()
    {
        currentId--;

        currentId = Mathf.Max(currentId, 0);

     //   SetDungeonUI(currentId);

    //    UpdateButtonState();
    }
    public void OnClickRightButton()
    {
        //currentId++;

        //currentId = Mathf.Min(currentId, GetLength());

      //  SetDungeonUI(currentId);

       // UpdateButtonState();
    }
}