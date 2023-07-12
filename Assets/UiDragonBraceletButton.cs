using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.Serialization;

public class UiDragonBraceletButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    private GameObject wolfRingObject;

    [SerializeField]
    private int lockCount;
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.getDragonBracelet).AsObservable().Subscribe(e =>
        {
            if (e == 0)
            {
                buttonText.SetText("획득");
            }
            else 
            {
                buttonText.SetText("장비 정보"); 
            }
        }).AddTo(this);
    }

    public void OnButtonClick()
    {
        if (ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.getDragonBracelet).Value == 0)
        {
            if (lockCount <= ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).Value)
            {
                ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.getDragonBracelet).Value = 1;
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"천룡 팔찌 획득!", null);
                ServerData.userInfoTable_2.UpDataV2(UserInfoTable_2.getDragonBracelet, false);
                return;
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"환수 장비 강화 \n {lockCount} 에 해금!", null);
                return;
            }
        }
        else
        {
            wolfRingObject.SetActive(true);
        }
    }
}
