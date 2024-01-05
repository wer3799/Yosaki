using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiBattleContestShopBoard : MonoBehaviour
{
    [SerializeField] private UiBattleContestShopCell prefab;

    [SerializeField] private UiBattleContestSpecialShopCell prefab2;
    
    [SerializeField] private UiBattleContestSpecialShopCell costumeCell;

    [FormerlySerializedAs("cellParent")] [SerializeField] private Transform costumeParent;
    [FormerlySerializedAs("cellParent")] [SerializeField] private Transform equipmentParent;
    [FormerlySerializedAs("cellParent")] [SerializeField] private Transform etcParent;

    private void Start()
    {
         Initialize();
    }

    private void Initialize()
    {
        var tableData2 = TableManager.Instance.BattleContestSpecialExchange.dataArray;

        foreach (var t in tableData2)
        {
            if (t.Shopcategory==0)
            {
                var cell = Instantiate<UiBattleContestSpecialShopCell>(costumeCell, costumeParent);

                cell.Initialize(t);

                
            }
            else
            {
                var cell = Instantiate<UiBattleContestSpecialShopCell>(prefab2, equipmentParent);
                cell.Initialize(t);    
            }
            
        }
        
        var tableData = TableManager.Instance.xMasCollection.dataArray;

        foreach (var t in tableData)
        {
            if (t.COMMONTABLEEVENTTYPE != CommonTableEventType.BattleContest) continue;
            if (t.Active == false) continue;
            var cell = Instantiate<UiBattleContestShopCell>(prefab, etcParent);

            cell.Initialize(t);
        }
    }
}
