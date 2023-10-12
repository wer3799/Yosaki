using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSuhoAnimalInstantClearPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI rewardDescription;

    [SerializeField]
    private TextMeshProUGUI allRewardDescription;

    [SerializeField]
    private UiAnimalView petIcon;

    [SerializeField]
    private GameObject notHasObject;

    [SerializeField]
    private GameObject hasObject;

    private void Start()
    {
        SetAllRewardDescription();
    }

    private void SetAllRewardDescription()
    {
        var tableData = TableManager.Instance.suhoPetTable.dataArray;

        string description = string.Empty;

        if (PlayerStats.GetSuhoGainValue() > 0f)
        {
            for (int i = 0; i < tableData.Length; i++)
            {
                if(tableData[i].SUHOPETTYPE!=SuhoPetType.Basic) continue;
            
                description += $"{tableData[i].Gradedescription}({tableData[i].Name}) 1회 소탕시 {tableData[i].Sweepvalue}(+{tableData[i].Sweepvalue*PlayerStats.GetSuhoGainValue()})개 획득";

                if (i != tableData.Length - 1)
                {
                    description += "\n";
                }
            }

        }
        else
        {
            for (int i = 0; i < tableData.Length; i++)
            {
                if(tableData[i].SUHOPETTYPE!=SuhoPetType.Basic) continue;
            
                description += $"{tableData[i].Gradedescription}({tableData[i].Name}) 1회 소탕시 {tableData[i].Sweepvalue}개 획득";

                if (i != tableData.Length - 1)
                {
                    description += "\n";
                }
            }

        }
   
        allRewardDescription.SetText(description);
    }

    private void OnEnable()
    {
        UpdateUi();
    }

    private void UpdateUi()
    {
        notHasObject.gameObject.SetActive(false);
        hasObject.gameObject.SetActive(false);

        int lastIdx = ServerData.suhoAnimalServerTable.GetLastPetId();

        if (lastIdx == -1)
        {
            notHasObject.gameObject.SetActive(true);
            return;
        }

        hasObject.gameObject.SetActive(true);
        
        var petTableData = TableManager.Instance.suhoPetTable.dataArray[lastIdx];
        
        petIcon.Initialize(petTableData);
        if (PlayerStats.GetSuhoGainValue() > 0f)
        {
            rewardDescription.SetText(
                $"현재 {petTableData.Gradedescription} 1회 소탕시 {CommonString.GetItemName(Item_Type.SuhoPetFeed)} {Utils.ConvertNum(petTableData.Sweepvalue)}(+{Utils.ConvertNum(petTableData.Sweepvalue*PlayerStats.GetSuhoGainValue())})개 획득!");
        }
        else
        {
            rewardDescription.SetText(
                $"현재 {petTableData.Gradedescription} 1회 소탕시 {CommonString.GetItemName(Item_Type.SuhoPetFeed)} {Utils.ConvertNum(petTableData.Sweepvalue)}개 획득!");
        }
    }
    
}