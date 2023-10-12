using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UiSleepRewardView : SingletonMono<UiSleepRewardView>
{
    [SerializeField]
    private List<TextMeshProUGUI> rewards;

    [SerializeField]
    private GameObject rootObject;
    
    [SerializeField]
    private TextMeshProUGUI timeDescription;


    [SerializeField]
    private GameObject peachObject;
    [SerializeField]
    private GameObject helObject;
    [SerializeField]
    private GameObject chunObject;
    [SerializeField]
    private GameObject dokebiObject;

    [SerializeField]
    private GameObject snowObject;

    [SerializeField]
    private GameObject springObject;

    [SerializeField]
    private GameObject hotTimeObject;
    
    [SerializeField]
    private GameObject seolObject;
    [SerializeField]
    private GameObject yoPowerObject;

    [SerializeField] private Image GoldIcon;
    //[SerializeField]
    //private GameObject winterObject;

    private void Start()
    {
        Subscribe();

        HotTimeEventCheck();
    }
    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateSon).AsObservable().Subscribe(e =>
        {
            peachObject.SetActive(e == 1);
        }).AddTo(this);

        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateHel).AsObservable().Subscribe(e =>
        {
            helObject.SetActive(e == 1);
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateChun).AsObservable().Subscribe(e =>
        {
            chunObject.SetActive(e == 1);
        }).AddTo(this);
        ServerData.userInfoTable.GetTableData(UserInfoTable.graduateDokebiFire).AsObservable().Subscribe(e =>
        {
            dokebiObject.SetActive(e == 1);
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateGold).AsObservable().Subscribe(e =>
        {
            GoldIcon.sprite = e < 1
                ? CommonUiContainer.Instance.GetItemIcon(Item_Type.Gold)
                : CommonUiContainer.Instance.GetItemIcon(Item_Type.GoldBar);
        }).AddTo(this);
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.graduateSeolEvent).AsObservable().Subscribe(e =>
        {
            seolObject.SetActive(e < 1);
        }).AddTo(this);

    }
    

    private void HotTimeEventCheck()
    {
        hotTimeObject.SetActive(ServerData.userInfoTable.IsHotTimeEvent());
    }
    public void CheckReward()
    {
        if (SleepRewardReceiver.Instance.sleepRewardInfo == null) return;

        StartCoroutine(SleepRewardReceiver.Instance.GetSleepReward(ShowReward));
    }

    private void ShowReward()
    {
        rootObject.SetActive(true);

        var reward = SleepRewardReceiver.Instance.sleepRewardInfo;

        TimeSpan ts = TimeSpan.FromSeconds(reward.elapsedSeconds);
        string maxTimeString = TimeSpan.FromSeconds(GameBalance.sleepRewardMaxValue).TotalHours.ToString();

        if (ts.Hours != 0)
        {
            if (ts.Days == 0)
            {
                timeDescription.SetText($"{ts.Hours}시간 {ts.Minutes}분(최대 :{maxTimeString}시간)");
            }
            else
            {
                timeDescription.SetText($"{ts.TotalHours}시간(최대 :{maxTimeString}시간)");
            }
        }
        else
        {
            if (ts.Days == 0)
            {
                timeDescription.SetText($"{ts.Minutes}분 {ts.Seconds}초(최대 :{maxTimeString}시간)");
            }
            else
            {
                timeDescription.SetText($"{ts.TotalHours}시간(최대 :{maxTimeString}시간)");
            }
        }

        //  winterObject.SetActive(ServerData.userInfoTable.CanSpawnEventItem());

        //골드
        rewards[0].SetText(Utils.ConvertBigNum(reward.gold));
        //jade
        rewards[1].SetText(Utils.ConvertBigNum(reward.jade));
        //growthstone
        rewards[2].SetText(Utils.ConvertBigNum(reward.GrowthStone));

        //여우구슬
        rewards[3].SetText(Utils.ConvertBigNum(reward.marble));

        //exp
        rewards[4].SetText(Utils.ConvertBigNum(reward.exp));

        //요괴구슬
        rewards[5].SetText(Utils.ConvertBigNum(reward.yoguiMarble));
        
        //2023 송편
        if (ServerData.userInfoTable.CanSpawnSnowManItem())
        {
            
            if (Utils.HasSnowManEventPass() == false)
            {
                rewards[6].SetText(Utils.ConvertBigNum(reward.eventItem));
            }
            else
            {
                rewards[6].SetText(Utils.ConvertBigNum(reward.eventItem*2f));
            }

            snowObject.SetActive(true);
        }
        else//눈사람
        {
            snowObject.SetActive(false);
        }
        //스테이지relic
        rewards[7].SetText(Utils.ConvertBigNum(reward.stageRelic));

        //설날
        rewards[8].SetText(Utils.ConvertBigNum(reward.sulItem));

        rewards[9].SetText(Utils.ConvertBigNum(reward.springItem));
        
        //봄나물
        if (ServerData.userInfoTable.CanSpawnSpringEventItem())
        {

            if (ServerData.iapServerTable.TableDatas[UiCollectionPass0BuyButton.PassKey].buyCount.Value <= 0)
            {
                rewards[10].SetText(Utils.ConvertBigNum(reward.eventItem));
            }
            else
            {
                rewards[10].SetText(Utils.ConvertBigNum(reward.eventItem * 2));
            }
        }
        else
        {
            springObject.SetActive(false);
        }

        //복숭아
        rewards[11].SetText(Utils.ConvertBigNum(reward.peachItem));
        //불멸석
        rewards[12].SetText(Utils.ConvertBigNum(reward.helItem));
        //불멸석
        rewards[13].SetText(Utils.ConvertBigNum(reward.chunItem));  
        //전리품
        rewards[14].SetText(Utils.ConvertBigNum(reward.dailybootyItem));
        //도깨비불
        rewards[15].SetText(Utils.ConvertBigNum(reward.dokebiItem));
        //핫타임
        if (ServerData.userInfoTable.IsHotTimeEvent())
        {
            rewards[16].SetText(Utils.ConvertBigNum(reward.hotTimeItem));
        }
        else
        {
            rewards[16].SetText("0");
        }
        rewards[17].SetText(Utils.ConvertBigNum(reward.yoPowerItem));
        

        yoPowerObject.SetActive(reward.yoPowerItem > 0);
        
        
        SleepRewardReceiver.Instance.GetRewardSuccess();
    }
}
