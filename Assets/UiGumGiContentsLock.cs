using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGumGiContentsLock : MonoBehaviour
{
    [SerializeField]
    private int lockLevel = 100000;
    private void OnEnable()
    {
        var curLevel = ServerData.statusTable.GetTableData(StatusTable.Level).Value;

        if (curLevel < lockLevel)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage($"레벨{lockLevel}부터 사용하실 수 있습니다.");
        }
    }

}
