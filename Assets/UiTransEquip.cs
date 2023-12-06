using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTransEquip : MonoBehaviour
{
    private void OnEnable()
    {
        var level = ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        if (level <= 1200000)
        {
            PopupManager.Instance.ShowAlarmMessage("레벨 120만레벨 이상일때 사용하실 수 있습니다");
            this.gameObject.SetActive(false);
            return;
        }
    }
    
}
