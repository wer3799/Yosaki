using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class MeditationPassIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countText;


    private void OnEnable()
    {
        var idx = ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.meditationIndex).Value;
        
        if (idx == -1)
        {
            countText.SetText($"명상 단계 : 0단계");
        }
        else
        {
            countText.SetText($"명상 단계 : {idx + 1}단계");
        }
    }
    
}
