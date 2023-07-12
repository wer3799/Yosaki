using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class GuimoonPassIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountText;


    private void OnEnable()
    {
        var tabledata = TableManager.Instance.GuimoonPass.dataArray;

        int idx = -1;
        for (int i = 0; i < tabledata.Length; i++)
        {
            if (PlayerStats.HasGuimoon(tabledata[i].Unlockcondition) == false) continue;
            idx = i;
        }

        if (idx == -1)
        {
            killCountText.SetText($"귀문이 개방되지 않았습니다.");
        }
        else
        {
            killCountText.SetText($"개방된 귀문 : {idx+1} 귀문");
        }
    }
    
}
