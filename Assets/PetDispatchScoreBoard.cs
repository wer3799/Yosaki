using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PetDispatchScoreBoard : MonoBehaviour
{
    [SerializeField]
    private UiPetDispatchView prefab;

    [SerializeField]
    private Transform rewardCellParent;
    
    private List<UiPetDispatchView> cellContainer = new List<UiPetDispatchView>();

    private PetDispatchScoreType _petDispatchScoreType = PetDispatchScoreType.Normal;
    
    private enum PetDispatchScoreType
    {
        Normal,
        Event,
    }

    private int currentIdx = 0;

    private void Start()
    {
        Initialize(currentIdx);
    }

    public void Initialize(int currentIdx)
    {
        this.currentIdx = currentIdx;

        UpdateUi();
    }


    public void UpdateUi()
    {

        var tableData = TableManager.Instance.PetTable.dataArray;

        int slotNum = 0;

        List<PetTableData> petTableDatas = new List<PetTableData>();
        switch (_petDispatchScoreType)
        {
            case PetDispatchScoreType.Normal:
                for (int i = 0; i < tableData.Length; i++)
                {
                    if(tableData[i].PETGETTYPE == PetGetType.Event)continue;
                    petTableDatas.Add(tableData[i]);
                    slotNum++;
                }        
                break;
            case PetDispatchScoreType.Event:
                for (int i = 0; i < tableData.Length; i++)
                {
                    if(tableData[i].PETGETTYPE != PetGetType.Event)continue;
                    petTableDatas.Add(tableData[i]);
                    slotNum++;
                }        
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        while (cellContainer.Count < slotNum)
        {
            UiPetDispatchView rewardCell = Instantiate<UiPetDispatchView>(prefab, rewardCellParent);
            cellContainer.Add(rewardCell);
        }

        for (int i = 0; i < cellContainer.Count; i++)
        {
            if (i < slotNum)
            {
                cellContainer[i].gameObject.SetActive(true);
                cellContainer[i].Initialize(petTableDatas[i]);
            }
            else
            {
                cellContainer[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickTab(int idx)
    {
        _petDispatchScoreType = (PetDispatchScoreType)idx;
        UpdateUi();
    }

}