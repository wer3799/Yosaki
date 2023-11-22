using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiBoardLevelLock : MonoBehaviour
{
    [SerializeField]
    private int unlockLevel;


    private void OnEnable()
    {
        if (ServerData.statusTable.GetTableData(StatusTable.Level).Value < unlockLevel)
        {
            PopupManager.Instance.ShowAlarmMessage($"레벨 {Utils.ConvertNum(unlockLevel)} 이상일 때 개방됩니다!");
            gameObject.SetActive(false);
        }
    }


}
