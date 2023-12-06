using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYomulUpgradeButton : MonoBehaviour
{
    public void OnClickUpgradeButton()
    {
        UiYoumulUpgradeBoard.Instance.ShowUpgradePopup(true);
    }

    public void OnClickYachaUpgradeButton()
    {        
        if (ServerData.weaponTable.TableDatas["weapon21"].hasItem.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("야차검이 필요합니다.");
            return;
        }
        UiYachaUpgradeBoard.Instance.ShowUpgradePopup(true);
    }
}
