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
        ServerData.iapServerTable.TableDatas[UiHotTimeEventBuyButton.seasonPassKey].buyCount.AsObservable().Subscribe(
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
            var date = GameBalance.HotTimeEventEndPeriod;
            desc += $"~{date.Month}월 {date.Day}일";
            float exp = GameBalance.HotTimeEvent_Exp;
            float gold = GameBalance.HotTimeEvent_Gold;
            float growthStone = GameBalance.HotTimeEvent_GrowthStone;
            float marble = GameBalance.HotTimeEvent_Marble;
            float yoPowerGoods = GameBalance.HotTimeEvent_YoPowerGoods;
            float TaegeukGoods = GameBalance.HotTimeEvent_TaegeukGoods;
            float sasinsuGoods = GameBalance.HotTimeEvent_SasinsuGoods;
            if (ServerData.iapServerTable.TableDatas[UiHotTimeEventBuyButton.seasonPassKey].buyCount.Value > 0)
            {
                exp += GameBalance.HotTimeEvent_Ad_Exp;
                gold += GameBalance.HotTimeEvent_Ad_Gold;
                growthStone += GameBalance.HotTimeEvent_Ad_GrowthStone;
                marble += GameBalance.HotTimeEvent_Ad_Marble;
                yoPowerGoods += GameBalance.HotTimeEvent_Ad_YoPowerGoods;
                TaegeukGoods += GameBalance.HotTimeEvent_Ad_TaegeukGoods;
                sasinsuGoods += GameBalance.HotTimeEvent_Ad_SasinsuGoods;
            }

            if (exp > 0)
            {
                desc += $"\n경험치 +{exp * 100f}%";
            }
            if (gold > 0)
            {
                desc += $"\n금화 +{gold * 100f}%";
            }

            if (growthStone > 0)
            {
                desc += $"\n수련의돌 +{growthStone * 100f}%";
            }

            if (marble > 0)
            {
                desc += $"\n여우구슬 +{marble * 100f}%";
            }
            if (yoPowerGoods > 0)
            {
                desc += $"\n{CommonString.GetItemName(Item_Type.YoPowerGoods)} +{yoPowerGoods * 100f}%";
            }
            if (TaegeukGoods > 0)
            {
                desc += $"\n{CommonString.GetItemName(Item_Type.TaeguekGoods)} +{TaegeukGoods * 100f}%";
            }
            if (sasinsuGoods > 0)
            {
                desc += $"\n{CommonString.GetItemName(Item_Type.SG)} +{sasinsuGoods * 100f}%";
            }
        }

        description.SetText(desc);

        //string timeDesc = string.Empty;

        //timeDesc += "핫타임 진행중!";

        //timeDescription.SetText(timeDesc);
    }

}
