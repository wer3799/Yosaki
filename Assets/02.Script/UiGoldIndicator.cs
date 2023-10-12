using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiGoldIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI goldText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Gold).AsObservable().Subscribe(gold =>
        {
            goldText.SetText($"{Utils.ConvertBigNum((double)gold)}");
        }).AddTo(this);
    }

}
