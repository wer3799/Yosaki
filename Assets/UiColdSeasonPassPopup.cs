using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiColdSeasonPassPopup : MonoBehaviour
{
    [SerializeField]
    private List<UiBuffPopupView> uiBuffPopupView_OneYear;

    [SerializeField]
    private UiBuffPopupView uiBuffPopupView_OneYear_2;

    [SerializeField]
    private TextMeshProUGUI descText0;
    [SerializeField]
    private TextMeshProUGUI descText1;
    [SerializeField]
    private TextMeshProUGUI descText2;
    
    void Start()
    {
        Initialize();
        //Subscribe();
    }

    private void Subscribe()
    {
        ServerData.iapServerTable.TableDatas[UiColdSeasonPassBuyButton.seasonPassKey].buyCount.AsObservable().Subscribe(e =>
        {
            descText2.SetText(e > 0 ? "적용중" : "패스권 구매시 적용");
        }).AddTo(this);
    }
    private void Initialize()
    {
        // var tableDatas = TableManager.Instance.BuffTable.dataArray;
        // int count = uiBuffPopupView_OneYear.Count;
        // for (int i = 0; i < count ; i++)
        // {
        //     uiBuffPopupView_OneYear[i].Initalize(tableDatas[23 + i]);
        //
        // }

        descText0.SetText($"경험치 획득 + {GameBalance.HotTimeEvent_Exp * 100}% 증가\n"
                          + $"금화 획득 + {GameBalance.HotTimeEvent_Gold * 100}% 증가\n"
                          + $"수련의돌 획득 + {GameBalance.HotTimeEvent_GrowthStone * 100}% 증가\n"
                          + $"여우구슬 획득 + {GameBalance.HotTimeEvent_Marble * 100}% 증가\n");
        descText1.SetText($"경험치 획득 + {GameBalance.HotTimeEvent_Ad_Exp * 100}% 증가\n"
                          + $"금화 획득 + {GameBalance.HotTimeEvent_Ad_Gold * 100}% 증가\n"
                          + $"수련의돌 획득 + {GameBalance.HotTimeEvent_Ad_GrowthStone * 100}% 증가\n"
                          + $"여우구슬 획득 + {GameBalance.HotTimeEvent_Ad_Marble * 100}% 증가\n");
        descText2.SetText("적용중");
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
