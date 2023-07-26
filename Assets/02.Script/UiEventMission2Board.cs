                                                         using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiEventMission2Board : MonoBehaviour
{
 

    private void OnEnable()
    {
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerData.goodsTable.GetTableData(GoodsTable.Event_Mission).Value += 100;
        }

    }
#endif

}
