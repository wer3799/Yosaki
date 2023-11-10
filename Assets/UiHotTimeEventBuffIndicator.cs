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
            desc += $"~12월 17일\n";
            float exp = GameBalance.HotTimeEvent_Exp;
            float gold = GameBalance.HotTimeEvent_Gold;
            float growthStone = GameBalance.HotTimeEvent_GrowthStone;
            float marble = GameBalance.HotTimeEvent_Marble;
            float yoPowerGoods = GameBalance.HotTimeEvent_YoPowerGoods;
            if (ServerData.iapServerTable.TableDatas[UiColdSeasonPassBuyButton.seasonPassKey].buyCount.Value > 0)
            {
                exp += GameBalance.HotTimeEvent_Ad_Exp;
                gold += GameBalance.HotTimeEvent_Ad_Gold;
                growthStone += GameBalance.HotTimeEvent_Ad_GrowthStone;
                marble += GameBalance.HotTimeEvent_Ad_Marble;
                yoPowerGoods += GameBalance.HotTimeEvent_Ad_YoPowerGoods;
            }

            if (exp > 0)
            {
                desc += $"경험치 +{exp * 100f}%\n";
            }
            if (gold > 0)
            {
                desc += $"금화 +{gold * 100f}%\n";
            }

            if (growthStone > 0)
            {
                desc += $"수련의돌 +{growthStone * 100f}%\n";
            }

            if (marble > 0)
            {
                desc += $"여우구슬 +{marble * 100f}%\n";
            }
            if (marble > 0)
            {
                desc += $"요석 +{yoPowerGoods * 100f}%";
            }
        }

        description.SetText(desc);

        //string timeDesc = string.Empty;

        //timeDesc += "핫타임 진행중!";

        //timeDescription.SetText(timeDesc);
    }

}
