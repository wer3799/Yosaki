using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiBattleContestShopBoard : MonoBehaviour
{
    [SerializeField] private UiBattleContestShopCell prefab;

    [SerializeField] private UiBattleContestSpecialShopCell prefab2;
    
    [SerializeField] private UiBattleContestSpecialShopCell costumeCell;

    [SerializeField] private Transform cellParent;

    [SerializeField] private SkeletonGraphic skeletonGraphic;
    private void Start()
    {
         Initialize();
    }

    private void Initialize()
    {
        var tableData2 = TableManager.Instance.BattleContestSpecialExchange.dataArray;

        foreach (var t in tableData2)
        {
            if (t.Id == 0)
            {
                costumeCell.Initialize(t);
                skeletonGraphic.Clear();
                var idx = ServerData.costumeServerTable.TableDatas[((Item_Type)t.Itemtype).ToString()].idx;
                skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.costumeList[idx];
                skeletonGraphic.Initialize(true);
                skeletonGraphic.SetMaterialDirty();
                
            }
            else
            {
                var cell = Instantiate<UiBattleContestSpecialShopCell>(prefab2, cellParent);
                cell.Initialize(t);    
            }
            
        }
        
        var tableData = TableManager.Instance.xMasCollection.dataArray;

        foreach (var t in tableData)
        {
            if (t.COMMONTABLEEVENTTYPE != CommonTableEventType.BattleContest) continue;
            if (t.Active == false) continue;
            var cell = Instantiate<UiBattleContestShopCell>(prefab, cellParent);

            cell.Initialize(t);
        }
    }
}
