using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiHotTimeEventBuffIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;


    void Start()
    {
        SetDescription();
        Subscribe();
    }

    void Subscribe()
    {
        ServerData.iapServerTable.TableDatas[UiColdSeasonPassBuyButton.seasonPassKey].buyCount.AsObservable().Subscribe(
            e =>
            {
                SetDescription();
            }).AddTo(this);
    }
    
    private void SetDescription()
    {
        string desc = string.Empty;

    
        if (ServerData.userInfoTable.IsHotTimeEvent())
        {
            desc += $"~11월 1일\n";
            float exp = GameBalance.HotTimeEvent_Exp;
            float gold = GameBalance.HotTimeEvent_Gold;
            float growthStone = GameBalance.HotTimeEvent_GrowthStone;
            float marble = GameBalance.HotTimeEvent_Marble;
            if (ServerData.iapServerTable.TableDatas[UiColdSeasonPassBuyButton.seasonPassKey].buyCount.Value > 0)
            {
                exp += GameBalance.HotTimeEvent_Ad_Exp;
                gold += GameBalance.HotTimeEvent_Ad_Gold;
                growthStone += GameBalance.HotTimeEvent_Ad_GrowthStone;
                marble += GameBalance.HotTimeEvent_Ad_Marble;
            }
            desc += $"경험치 +{exp * 100f}%\n";
            desc += $"금화 +{gold * 100f}%\n";
            desc += $"수련의돌 +{growthStone * 100f}%\n";
            desc += $"여우구슬 +{marble * 100f}%";
        }

        description.SetText(desc);

        //string timeDesc = string.Empty;

        //timeDesc += "핫타임 진행중!";

        //timeDescription.SetText(timeDesc);
    }

}
