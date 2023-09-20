using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiColdSeasonPassPopup : MonoBehaviour
{
    [SerializeField]
    private List<UiBuffPopupView> uiBuffPopupViews;


    [SerializeField]
    private TextMeshProUGUI descText0;
    [SerializeField]
    private TextMeshProUGUI descText1;
    [SerializeField]
    private TextMeshProUGUI descText2;
    
    void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.iapServerTable.TableDatas[UiChuseokPassBuyButton.seasonPassKey].buyCount.AsObservable().Subscribe(e =>
        {
            descText2.SetText(e > 0 ? "적용중" : "패스권 구매시 적용");
        }).AddTo(this);
    }
    private void Initialize()
    {
        descText0.SetText($"경험치 획득 + {GameBalance.HotTimeEvent_Exp * 100}% 증가\n"
                          + $"금화 획득 + {GameBalance.HotTimeEvent_Gold * 100}% 증가\n"
                          + $"수련의돌 획득 + {GameBalance.HotTimeEvent_GrowthStone * 100}% 증가\n"
                          + $"여우구슬 획득 + {GameBalance.HotTimeEvent_Marble * 100}% 증가\n");
        descText1.SetText($"경험치 획득 + {GameBalance.HotTimeEvent_Ad_Exp * 100}% 증가\n"
                          + $"금화 획득 + {GameBalance.HotTimeEvent_Ad_Gold * 100}% 증가\n"
                          + $"수련의돌 획득 + {GameBalance.HotTimeEvent_Ad_GrowthStone * 100}% 증가\n"
                          + $"여우구슬 획득 + {GameBalance.HotTimeEvent_Ad_Marble * 100}% 증가\n");

    }

    private void OnEnable()
    {
        var severTime = ServerData.userInfoTable.currentServerTime;

        if (ServerData.userInfoTable.IsHotTimeEvent() == false)
        {
            this.gameObject.SetActive(false);
            PopupManager.Instance.ShowAlarmMessage("이벤트가 종료됐습니다.");
        }

    }
}
