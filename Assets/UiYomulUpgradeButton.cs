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
            return;
        }
        UiYachaUpgradeBoard.Instance.ShowUpgradePopup(true);
    }
}
