using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class SealSwordEvolutionPassIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countText;


    private void OnEnable()
    {
        var tabledata = TableManager.Instance.sealSwordTable.dataArray;

        var serverData = ServerData.sealSwordServerTable.TableDatas; 
        
        int idx = -1;
        for (int i = 0; i < tabledata.Length; i++)
        {
            if (serverData[tabledata[i].Stringid].hasItem.Value > 0)
            {
                idx = i;
            }
        }

        
        if (idx == -1)
        {
            countText.SetText($"요도를 보유하지 않았습니다.");
        }
        else
        {
            var desc = tabledata[idx].Name;
            countText.SetText($"최고 등급 요도 : {desc.Replace(" 요도","")}");
        }
    }
    
}
