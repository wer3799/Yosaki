using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class UiLevelIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;

    void Start()
    {
        Subscribe();
    }
    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(WhenLevelChanged).AddTo(this);
    }

    private void WhenLevelChanged(float level)
    {
        levelText.SetText($"{level}");
    }
}
