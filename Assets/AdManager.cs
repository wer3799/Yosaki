using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

using UnityEngine.Events;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;

public class AdManager : SingletonMono<AdManager>,IUnityAdsInitializationListener,IUnityAdsLoadListener,IUnityAdsShowListener
{
    private static string AndroidGameId = "4235399";
    private static string IOSGameId = "4235398";

#if UNITY_ANDROID
    private string rewardedPlacementId = "Rewarded_Android";
#endif

#if UNITY_IOS
    private string rewardedPlacementId = "Rewarded_iOS";
#endif
    private string videoPlacementId = "video";

    public static string bannerPlacement = "Banner_Weapons";

    private ShowOptions options;

    private System.Action rewardEndCallBack;

    private bool admobLoadSuccess = false;

    private new void Awake()
    {
        base.Awake();
#if UNITY_ANDROID
        Advertisement.Initialize(AndroidGameId);
#elif UNITY_IOS
 Advertisement.Initialize(IOSGameId);
#endif
        Initialize();
    }

    private void Initialize()
    {
       
    }

    private void ShowNormalVideo()
    {
        Advertisement.Show(videoPlacementId);
    }
    private void ShowRewardedUnityVideo()
    {
        Advertisement.Show(rewardedPlacementId);
    }

    public void ShowRewardedReward(System.Action callBack)
    {
        //광고제거 구매유저
        if (HasRemoveAdProduct())
        {
            callBack.Invoke();
        }
        else
        {
            this.rewardEndCallBack = callBack;

            if (admobLoadSuccess)
            {
                if (this.rewardedAd.IsLoaded())
                {
                    this.rewardedAd.Show();
                }
                else
                {
                    ShowRewardedUnityVideo();
                }
            }
            else
            {
                ShowRewardedUnityVideo();
            }


        }
    }

    private static string removeAdKey = "removead";
    public bool HasRemoveAdProduct()
    {
        bool hasIapProduct = ServerData.iapServerTable.TableDatas[removeAdKey].buyCount.Value > 0;

        return hasIapProduct;
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            GetReward();
        }
        else if (result == ShowResult.Failed)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광고 재생에 실패했습니다.\n잠시후 다시 시도해주세요", null);
        }
    }

    private void GetReward()
    {
        rewardEndCallBack?.Invoke();
        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광고 보상 획득!", null);
        SoundManager.Instance.PlaySound("GoldUse");
    }


    //admob
    private RewardedAd rewardedAd;
    public void CreateAndLoadRewardedAd()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3565646304666904/2502647555";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3565646304666904/3522185370";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void Start()
    {
        CreateAndLoadRewardedAd();
    }



    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //초기화 광고 성공
        admobLoadSuccess = true;
        Debug.LogError("AdMob_admobLoadSuccess");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //초기화 광고 실패
        admobLoadSuccess = false;
        Debug.LogError("AdMob_admobLoadFailed");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        //X
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        //실패
        Debug.LogError("AdMob_AdLoadFailed");
        ShowRewardedUnityVideo();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        //X
        CreateAndLoadRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        //보상 획득
        StartCoroutine(SlowRewardRoutine());
    }

    private IEnumerator SlowRewardRoutine()
    {
        yield return null;
        yield return null;
        yield return null;
        Debug.LogError("AdMob_GetReward");
        GetReward();
    }


    public void OnInitializationComplete()
    {
    
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
    
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
      
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
    
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
      
    }

    public void OnUnityAdsShowStart(string placementId)
    {
  
    }

    public void OnUnityAdsShowClick(string placementId)
    {
       
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            GetReward();
        }
        else 
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광고 재생에 실패했습니다.\n잠시후 다시 시도해주세요", null);
        }
    }
}
