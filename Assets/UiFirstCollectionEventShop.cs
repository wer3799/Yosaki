using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UiFirstCollectionEventShop : MonoBehaviour
{
    [SerializeField] private UiCollectionEventCommonView _costumeEventCommonView;
    [SerializeField] private Transform costumeContents;
    
    [SerializeField] private UiCollectionEventCommonView _goodsEventCommonView;
    [SerializeField] private Transform goodsContents;
    

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.commoncollectionEvent.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            if(tableData[i].Active==false) continue;
            if (Utils.IsCostumeItem((Item_Type)tableData[i].Itemtype))
            {
                var prefab = Instantiate(_costumeEventCommonView, costumeContents);
                prefab.Initialize(i);
            }
            else
            {
                var prefab = Instantiate(_goodsEventCommonView, goodsContents);
                prefab.Initialize(i);
            }
        }
    }
}
