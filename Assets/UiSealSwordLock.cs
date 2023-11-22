using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSealSwordLock : MonoBehaviour
{
    [SerializeField]
    private int lockLevel = 2000000;

    private void OnEnable()
    {
        int currnetLevel = (int)ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        bool isLock = currnetLevel < lockLevel;
        
        this.gameObject.SetActive(!isLock);

        if (isLock)
        {
            PopupManager.Instance.ShowAlarmMessage($"레벨 150만 이상일 때 개방됩니다!");
        }
    }

}
