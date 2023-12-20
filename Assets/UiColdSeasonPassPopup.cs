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
        ServerData.iapServerTable.TableDatas[UiHotTimeEventBuyButton.seasonPassKey].buyCount.AsObservable().Subscribe(e =>
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
        string desc0 = "";
        string desc1 = "";
///////////////////
        if (GameBalance.HotTimeEvent_TaegeukGoods > 0)
        {
            desc0 += $"{CommonString.GetStatusName(StatusType.TaegeukGoodsGainPer)} + {GameBalance.HotTimeEvent_TaegeukGoods * 100}% 증가\n";
        }
        if (GameBalance.HotTimeEvent_YoPowerGoods > 0)
        {
            desc0 += $"{CommonString.GetStatusName(StatusType.YoPowerGoodsGainPer)} + {GameBalance.HotTimeEvent_YoPowerGoods * 100}% 증가\n";
        }
        if (GameBalance.HotTimeEvent_GrowthStone > 0)
        {
            desc0 += $"수련의돌 획득 + {GameBalance.HotTimeEvent_GrowthStone * 100}% 증가\n";
        }
        if (GameBalance.HotTimeEvent_Marble > 0)
        {
            desc0 += $"여우구슬 획득 + {GameBalance.HotTimeEvent_Marble * 100}% 증가\n";
        }
        if (GameBalance.HotTimeEvent_Exp > 0)
        {
            desc0 += $"경험치 획득 + {GameBalance.HotTimeEvent_Exp * 100}% 증가\n";
        }
        if (GameBalance.HotTimeEvent_Gold > 0)
        {
            desc0 += $"금화 획득 + {GameBalance.HotTimeEvent_Gold * 100}% 증가\n";
        }       
        ///////////////////////////////
        if (GameBalance.HotTimeEvent_Ad_TaegeukGoods > 0)
        {
            desc1 += $"{CommonString.GetStatusName(StatusType.TaegeukGoodsGainPer)} + {GameBalance.HotTimeEvent_Ad_TaegeukGoods * 100}% 증가\n";
        }
        if (GameBalance.HotTimeEvent_Ad_YoPowerGoods > 0)
        {
            desc1 += $"{CommonString.GetStatusName(StatusType.YoPowerGoodsGainPer)} + {GameBalance.HotTimeEvent_Ad_YoPowerGoods * 100}% 증가\n";
        }
        if (GameBalance.HotTimeEvent_Ad_GrowthStone > 0)
        {
            desc1 += $"수련의돌 획득 + {GameBalance.HotTimeEvent_Ad_GrowthStone * 100}% 증가\n";
        }
        if (GameBalance.HotTimeEvent_Ad_Marble > 0)
        {
            desc1 += $"여우구슬 획득 + {GameBalance.HotTimeEvent_Ad_Marble * 100}% 증가\n";
        }
        if (GameBalance.HotTimeEvent_Ad_Exp > 0)
        {
            desc1 += $"경험치 획득 + {GameBalance.HotTimeEvent_Ad_Exp * 100}% 증가\n";
        }
        if (GameBalance.HotTimeEvent_Ad_Gold > 0)
        {
            desc1 += $"금화 획득 + {GameBalance.HotTimeEvent_Ad_Gold * 100}% 증가\n";
        }
///////////////////////

        descText0.SetText(desc0);
             
        descText1.SetText(desc1);

    }
    public void OnClickReceiveCostume()
    {
        if (Utils.HasHotTimeEventPass()==false)
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
