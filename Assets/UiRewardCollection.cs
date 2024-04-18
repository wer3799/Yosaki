using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiRewardCollection : MonoBehaviour
{
    [SerializeField]
    private Button oniButton;

    [SerializeField]
    private Button oniButton2;

    [SerializeField]
    private Button baekGuiButton;

    [SerializeField]
    private Button sonButton;

    [SerializeField]
    private Button gangChulButton;

    [SerializeField]
    private Button gumGiButton;

    [SerializeField]
    private Button hellFireButton;

    [SerializeField]
    private Button chunFlowerButton;

    [SerializeField]
    private Button hellRelicButton;

    [SerializeField]
    private Button dokebiClearButton;

    [SerializeField]
    private Button sumiClearButton;

    [SerializeField]
    private Button newGachaButton;

    [SerializeField]
    private GameObject Bandi0;
    [SerializeField]
    private GameObject Bandi1;
    
    [SerializeField]
    private GameObject SonObject;
    [SerializeField]
    private GameObject HelObject;
    [SerializeField]
    private GameObject ChunObject;
    [SerializeField]
    private GameObject DokebiObject;
    [SerializeField]
    private GameObject SumiObject;
    [SerializeField]
    private List<GameObject> Objects = new List<GameObject>();

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(e =>
        {
            oniButton.interactable = e >= 300;
            baekGuiButton.interactable = e >= 3000;
            sonButton.interactable = e >= 5000;
            //gangChulButton.interactable = e >= 30000;
            gumGiButton.interactable = e >= 50000;
            hellFireButton.interactable = e >= 50000;
            chunFlowerButton.interactable = e >= 200000;
            hellRelicButton.interactable = e >= 50000;
            dokebiClearButton.interactable = e >= 500000;
            sumiClearButton.interactable = e >= 1000000;
            oniButton2.interactable = e >= 500000;


            if (e >= GameBalance.banditUpgradeLevel)
            {
                Bandi0.SetActive(false);
            }
            else
            {
                Bandi1.SetActive(false);
            }
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.relicKillCount).AsObservable().Subscribe(e =>
        {
            newGachaButton.interactable = e >= 25000;
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).AsObservable().Subscribe(e =>
        {
            SonObject.SetActive(e == 0);
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).AsObservable().Subscribe(e =>
        {
            HelObject.SetActive(e == 0);
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).AsObservable().Subscribe(e =>
        {
            ChunObject.SetActive(e == 0);
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).AsObservable().Subscribe(e =>
        {
            DokebiObject.SetActive(e == 0);
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSumiFire).AsObservable().Subscribe(e =>
        {
            SumiObject.SetActive(e == 0);
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateBackGui).AsObservable().Subscribe(e =>
        {
            Objects[0].SetActive(e == 0);
        }).AddTo(this);
    }

    public void OnClickBanditReward(bool isPopUp = true)
    {
        ContentsRewardManager.Instance.OnClickBanditReward(isPopUp);
    }//★

    public void OnClickDayofWeekReward(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickDayofWeekReward(isPopUp);
    }//★

    public void OnClickOniReward(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickOniReward(isPopUp);
    }//★

    public void OnClickGumgiReward(bool isPopUp=true)
    {        
        ContentsRewardManager.Instance.OnClickGumgiReward(isPopUp);
    }
    public void OnClickChunFlowerReward(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickChunFlowerReward(isPopUp);
    }

    public void OnClickSmithReward(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickSmithReward(isPopUp);
    }
    public void OnClickTrainingReward()
    {
        RewardPopupManager.Instance.OnclickButton();
    }
    public void OnClickDokebiReward(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickDokebiReward(isPopUp);
    }

    public void OnClickSumiReward(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickSumiReward(isPopUp);
    }

    private int GetDayOfweek()
    {
        var serverTime = ServerData.userInfoTable.currentServerTime;
        return (int)serverTime.DayOfWeek;
    }
    public void OnClickGetNewGachaButton(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickGetNewGachaButton(isPopUp);
    }
    public void OnClickSonReceiveButton(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickSonReceiveButton(isPopUp);
    }
    public void OnClickBackGuiReceiveButton(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickBackGuiReceiveButton(isPopUp);
    }
    
    public void OnClickOldDokebi2ReceiveButton(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickOldDokebi2ReceiveButton(isPopUp);
    }
    public void OnClickGangChulReceiveButton(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickGangChulReceiveButton(isPopUp);
    }
    public void OnClickHelRelicReceiveButton(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickHelRelicReceiveButton(isPopUp);
    }
    public void OnClickHellReceiveButton(bool isPopUp=true)
    {
        ContentsRewardManager.Instance.OnClickHellReceiveButton(isPopUp);
    }
    
    public void OnClickAllRewardReceive()
    {
        var lv = ServerData.statusTable.GetTableData(StatusTable.Level).Value;
        if (lv >= GameBalance.banditUpgradeLevel)
        {
            //대왕반딧불전 받기
            OnClickDayofWeekReward(false);
        }
        else
        {
            //반딧불전 받기
            OnClickBanditReward(false);
        }
        OnClickOniReward(false);
        OnClickBackGuiReceiveButton(false);
        OnClickSonReceiveButton(false);
        OnClickSmithReward(false);
        //OnClickGangChulReceiveButton(false);
        OnClickGetNewGachaButton(false);
        
        //오른쪽
        OnClickGumgiReward(false);
        OnClickHellReceiveButton(false);
        OnClickChunFlowerReward(false);
        OnClickHelRelicReceiveButton(false);
        OnClickDokebiReward(false);
        OnClickSumiReward(false);
        //OnClickOldDokebi2ReceiveButton(false);
        
        YorinSpecialMissionManager.UpdateMissionClear(YorinSpecialMissionKey.YSMission2_2, 1);

        PopupManager.Instance.ShowAlarmMessage("받을 수 있는 보상을 전부 받았습니다!");
    }
}
