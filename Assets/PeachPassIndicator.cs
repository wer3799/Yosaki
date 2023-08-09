using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class PeachPassIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountText;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.sonScore).AsObservable().Subscribe(e =>
        {
            var score = e * GameBalance.BossScoreConvertToOrigin;

            var tableData = TableManager.Instance.SonReward.dataArray;

            int idx = -1;
            for (int i = 0; i < tableData.Length; i++)
            {
                if (score > tableData[i].Score)
                {
                    idx = i;
                }
                else
                {
                    break;
                }
            }

            killCountText.SetText($"현재 손오공 단계 : {idx+1} 단계");
        }).AddTo(this);
    }
}
