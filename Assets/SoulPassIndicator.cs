using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class SoulPassIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countText;


    private void OnEnable()
    {
        var tabledata = TableManager.Instance.NewGachaTable.dataArray;

        var serverData = ServerData.newGachaServerTable.TableDatas; 
        
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
            countText.SetText($"반지를 보유하지 않았습니다.");
        }
        else
        {
            var desc = tabledata[idx].Skillname;
            countText.SetText($"최고 등급 영혼 반지 : {desc.Replace(" 반지","")}");
        }
    }
    
}
