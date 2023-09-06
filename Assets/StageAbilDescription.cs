using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class StageAbilDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.enhanceRelicIdx).AsObservable().Subscribe(e =>
        {
            description.SetText($"x{PlayerStats.GetStageRelicUpgradeValue()}배");
        }).AddTo(this);
    }

}
