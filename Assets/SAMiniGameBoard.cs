using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class SAMiniGameBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI topScore;
    [SerializeField]
    private TextMeshProUGUI accumulScore;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventMiniGameScore_TopRate).AsObservable().Subscribe(
            e =>
            {
                topScore.SetText($"최고 기록 점수 : {(int)e}");
            }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.eventMiniGameScore_Total).AsObservable().Subscribe(
            e =>
            {
                accumulScore.SetText($"누적 점수 : {(int)e}");
            }).AddTo(this);
    }
}
