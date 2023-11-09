using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UniRx;
using UnityEngine;

public class UiColdSeasonPassPopup : MonoBehaviour
{
    [SerializeField]
    private List<UiBuffPopupView> uiBuffPopupViews;
    
    string costumeKey = "costume174";

    [SerializeField] private GameObject getCostumeButton;

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
        ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.AsObservable().Subscribe(e =>
        {
            getCostumeButton.SetActive(!e);
        }).AddTo(this);
    }
    private void Initialize()
    {
         descText0.SetText(
             //$"경험치 획득 + {GameBalance.HotTimeEvent_Exp * 100}% 증가\n"
        //                   + $"금화 획득 + {GameBalance.HotTimeEvent_Gold * 100}% 증가\n"
                           $"요석 획득 증가 + {GameBalance.HotTimeEvent_YoPowerGoods * 100}% 증가\n"
                          + $"수련의돌 획득 + {GameBalance.HotTimeEvent_GrowthStone * 100}% 증가\n"
                          + $"여우구슬 획득 + {GameBalance.HotTimeEvent_Marble * 100}% 증가\n");
        descText1.SetText(
            // $"경험치 획득 + {GameBalance.HotTimeEvent_Ad_Exp * 100}% 증가\n"
            //               + $"금화 획득 + {GameBalance.HotTimeEvent_Ad_Gold * 100}% 증가\n"
            $"요석 획득 증가 + {GameBalance.HotTimeEvent_Ad_YoPowerGoods * 100}% 증가\n"
                          + $"수련의돌 획득 + {GameBalance.HotTimeEvent_Ad_GrowthStone * 100}% 증가\n"
                          + $"여우구슬 획득 + {GameBalance.HotTimeEvent_Ad_Marble * 100}% 증가\n");

    }
    public void OnClickReceiveCostume()
    {
        if (ServerData.iapServerTable.TableDatas[UiChuseokPassBuyButton.seasonPassKey].buyCount.Value < 1)
        {
            PopupManager.Instance.ShowAlarmMessage("패스권이 필요합니다!");
            return;
        }

        if(ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.Value==true)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유중입니다!");
            return;
        };
        ServerData.costumeServerTable.TableDatas[costumeKey].hasCostume.Value = true;
        
        List<TransactionValue> transactions = new List<TransactionValue>();

        Param costumeParam = new Param();

        costumeParam.Add(costumeKey.ToString(), ServerData.costumeServerTable.TableDatas[costumeKey].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));


        ServerData.SendTransactionV2(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!!", null);
        });
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
