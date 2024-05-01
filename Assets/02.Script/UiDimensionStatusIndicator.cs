using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;


public class UiDimensionStatusIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countText;

    [SerializeField]
    private string statusKey;

    [SerializeField]
    private string prefixDesc;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.dimensionStatusTable.GetTableData(statusKey).AsObservable().Subscribe(statusPoint =>
        {
            countText.SetText($"{prefixDesc}{statusPoint}");
        }).AddTo(this);
    }
}
