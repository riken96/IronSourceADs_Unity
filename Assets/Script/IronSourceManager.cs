using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceManager : MonoBehaviour
{
    public string appkey;
    public bool testMode = false;
    public static IronSourceManager instance;
    public GameObject adLoader;

    public IronSourceBannerPosition baanerPosition;
    private static float lastInterstitial;
    public int NoAds
    {
        get
        {
            return PlayerPrefs.GetInt("NoAds", 0);
        }
        set
        {
            PlayerPrefs.SetInt("NoAds", value);
        }
    }


    void OnEnable()
    {
        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoAdClickedEvent;

        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialAdClosedEvent;

        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
    }

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        Setup();
    }


    public void Setup()
    {
        IronSource.Agent.validateIntegration();
        Debug.Log("unity-script: unity version" + IronSource.unityVersion());
        Debug.Log("unity-script: IronSource.Agent.init");
        IronSource.Agent.init(appkey);
        // bannerImage.SetActive(false);

        if (NoAds == 0)
        {
            Debug.Log("no ads is inactive");
            ShowBannerAd();
            LoadInterstitialAd(0);
        }
        else
        {
            Debug.Log("no ads is activated");
        }
        lastInterstitial = -1000f;

    }

    
    #region BannerAD


    public void ShowBannerAd()
    {
        if (NoAds != 1)
        {
            Debug.Log("Banner ad load");
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, baanerPosition);
        }
    }
    public void HideBannerAd()
    {
        Debug.Log("Hide Banner Ad");
        IronSource.Agent.destroyBanner();
        
    }
    void BannerAdLoadedEvent(IronSourceAdInfo obj)
    {
        Debug.Log("RefreshBanner");

    }

    void BannerAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void BannerAdClickedEvent(IronSourceAdInfo obj)
    {
        Debug.Log("unity-script: I got BannerAdClickedEvent");
    }

    void BannerAdScreenPresentedEvent(IronSourceAdInfo obj)
    {
        Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
    }

    void BannerAdScreenDismissedEvent(IronSourceAdInfo obj)
    {
        Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
    }

    void BannerAdLeftApplicationEvent(IronSourceAdInfo obj)
    {
        Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
    }
    #endregion



    #region IntrestitialADs
    [Header("IntrestitialADs")]
    public int tryCount;
    public int ReloadTime = 40;
    bool isLoadingISIntrestital;
    public bool isIntrestitiallShowing = false;
    public Action<bool> _callbackIntrestital;

    public void LoadInterstitialAd(float delay)
    {
        Invoke("LoadInterstitialAds", delay);
    }

    public void ExampleShowIntrestitialWithCallback()
    {
        ShowInterstitialTimer(TestIntrestitialWithCallback);
    }

    public void ExampleShowIntrestitialWithLoaderCallback()
    {
        ShowForceInterstitialWithLoader(TestIntrestitialWithCallback, 3);
    }

    public void TestIntrestitialWithCallback(bool isCompleted)
    {

        if (isCompleted)
        {
            //Give reward here
            Debug.Log("Intrestitial  completed  Do other thing");

        }
        else
        {
            Debug.Log("Intrestitial  has issue");

            // do next step as reward not available
        }
    }

    public void LoadInterstitialAds()
    {
        if (!IronSource.Agent.isInterstitialReady() && !isLoadingISIntrestital)
        {
            isLoadingISIntrestital = true;
            IronSource.Agent.loadInterstitial();
        }
    }
    public bool ISIntrestitialReadyToShow(bool ForceShow = false)
    {
        if (!IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.loadInterstitial();
        }
        float time = Time.time;
        bool isloadedTime = false;
        if (time - lastInterstitial >= ReloadTime)
        {
            //ShowInterstitial(f);
            //lastInterstitial = time;
            isloadedTime = true;
        }
        if (ForceShow)
        {
            isloadedTime = true;
        }
        return  IronSource.Agent.isInterstitialReady() && isloadedTime;
    }

    public void ShowForceInterstitialWithLoader(Action<bool> onComplete, int _tryCount)
    {
        tryCount = _tryCount;
        // adLoader.SetActive(true);
        //alreay showing
        if (isIntrestitiallShowing)
        {
            return;
        }

        // No Ads Purchased
        if (NoAds == 1)
        {
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(false);
            _callbackIntrestital = null;
            return;
        }

        //Regular call

        adLoader.SetActive(true);

        isIntrestitiallShowing = true;
        _callbackIntrestital = onComplete;

        if (testMode)
        {
            isIntrestitiallShowing = false;
            adLoader.SetActive(false);
            if (_callbackIntrestital == null)
            {
                return;
            }

            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
            return;
        }

#if UNITY_EDITOR
        isIntrestitiallShowing = false;
        adLoader.SetActive(false);
        if (_callbackIntrestital == null)
        {
            return;
        }

        _callbackIntrestital.Invoke(true);
        _callbackIntrestital = null;
        return;
#endif
       
         if (ISIntrestitialReadyToShow(true))
        {
            IronSource.Agent.showInterstitial();
            lastInterstitial = Time.time;
        }
        else
        {

            if (tryCount > 0)
            {
                // tryCount--;
                isIntrestitiallShowing = false;
                LoadInterstitialAd(0);
                StartCoroutine(IEShowForceInterstitialWithLoader(_callbackIntrestital));
            }
            else
            {
                adLoader.SetActive(false);
                isIntrestitiallShowing = false;
                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
            }
            LoadInterstitialAd(0f);
        }
    }

    public IEnumerator IEShowForceInterstitialWithLoader(Action<bool> onComplete)
    {
        tryCount--;
        if (tryCount > 0)
        {
            yield return new WaitForSeconds(1f);
            ShowForceInterstitialWithLoader(onComplete, tryCount);
        }
        else
        {
            tryCount = 0;
            adLoader.SetActive(false);
            isIntrestitiallShowing = false;
            if (_callbackIntrestital != null)
            {
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
            }
            LoadInterstitialAd(0f);
        }
    }


    public void ShowInterstitialTimer(Action<bool> onComplete)
    {
        tryCount = 0;
        Debug.Log("isIntrestitiallShowing => " + isIntrestitiallShowing);
        if (isIntrestitiallShowing)
        {
            return;
        }

        if (NoAds == 1)
        {
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(false);
            _callbackIntrestital = null;
            return;
        }
        isIntrestitiallShowing = true;
        _callbackIntrestital = onComplete;

        if (testMode)
        {
            isIntrestitiallShowing = false;
            adLoader.SetActive(false);
            if (_callbackIntrestital == null)
            {
                return;
            }

            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
            return;
        }

#if UNITY_EDITOR
        isIntrestitiallShowing = false;
        if (_callbackIntrestital == null)
        {
            return;
        }
        _callbackIntrestital.Invoke(true);
        _callbackIntrestital = null;
        return;
#endif
        //Regular call

        adLoader.SetActive(true);
        Debug.Log("ShowInterstitialTimer");
        
         if (ISIntrestitialReadyToShow())
        {
            Debug.Log("Ready to show");
            IronSource.Agent.showInterstitial();
            lastInterstitial = Time.time;
        }
        else
        {
                Debug.Log("can't show");
                adLoader.SetActive(false);
                isIntrestitiallShowing = false;
                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
                LoadInterstitialAd(0f);
        }
    }


    void InterstitialAdReadyEvent(IronSourceAdInfo adInfo)
    {
        isLoadingISIntrestital = false;
        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        //  Time.timeScale = 1;
        isLoadingISIntrestital = false;
        LoadInterstitialAd(0f);
        isIntrestitiallShowing = false;


        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void InterstitialAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
        adLoader.SetActive(false);
    }

    void InterstitialAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        tryCount = 0;
        adLoader.SetActive(false);
        isIntrestitiallShowing = false;
        if (tryCount <= 0)
        {

            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(false);
            _callbackIntrestital = null;
        }
        //Time.timeScale = 1;
        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());

    }

    void InterstitialAdClickedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
    }

    void InterstitialAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        // Time.timeScale = 0;
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
    }

    void InterstitialAdClosedEvent(IronSourceAdInfo adInfo)
    {
        //  Time.timeScale = 1;
        tryCount = 0;
        adLoader.SetActive(false);
        LoadInterstitialAd(0f);
        isIntrestitiallShowing = false;
        if (_callbackIntrestital == null)
        {
            return;
        }
        _callbackIntrestital.Invoke(true);
        _callbackIntrestital = null;

        Debug.Log("heheheheheh");

    }
    #endregion


    #region RewardADs
    [Header("RewardAD")]
    public Action<bool> _callback;
    [HideInInspector]
    public bool isRewardShowing = false;
    string VideoFor = "";
    [HideInInspector]
    public bool isRewardGiven = false;


    public void ExampleShowReward()
    {
        ShowRewardVideo(ExampleShowRewardAssign);
    }

    public void ExampleShowRewardAssign(bool isrewarded)
    {
        if (isrewarded)
        {
            //Give reward here
            Debug.Log("Reward Given");
        }
        else
        {
            Debug.Log("Reward Eroor Given");
            // do next step as reward not available
        }
    }

    public void ShowRewardVideo(Action<bool> onComplete)
    {
        Debug.Log("Show Reward video Ad");

        if (isRewardShowing)
        {
            return;
        }



        isRewardGiven = false;
        isRewardShowing = true;
        _callback = onComplete;

        if (testMode)
        {
            isRewardShowing = false;
            if (_callback == null)
            {
                return;
            }
            _callback.Invoke(true);
            _callback = null;
            return;
        }
        //GiveRewardToUser();
#if UNITY_EDITOR
        isRewardShowing = false;
        if (_callback == null)
        {
            return;
        }
        _callback.Invoke(true);
        _callback = null;
        return;
#endif

        
         if (IsISRewardAdAvailable())
        {
            //SoundManager.Instance.StopAllSoundRunningVideoAd(true);
            IronSource.Agent.showRewardedVideo();
            
        }
        
        else
        {
            
            IronSource.Agent.loadRewardedVideo();
            Debug.LogError("Problem in showing video");
            isRewardShowing = false;
            if (_callback == null)
            {
                return;
            }
            _callback.Invoke(false);
            _callback = null;
            return;
            // NotificationHandler.Instance.ShowNotification("Reward Ad is not available!");
        }
    }


    public bool IsISRewardAdAvailable()
    {

        return  IronSource.Agent.isRewardedVideoAvailable();

        //return AdmobManager.Instance.interstitial != null && AdmobManager.Instance.interstitial.CanShowAd();
    }

    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
    }

    void RewardedVideoAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        isRewardGiven = true;
        isRewardShowing = false;
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
    }

    void RewardedVideoAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        isRewardGiven = true;
        isRewardShowing = false;
        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + placement.getRewardAmount() + " name = " + placement.getRewardName());
        //GiveRewardToUser();
    }

    void RewardedVideoAdClosedEvent(IronSourceAdInfo adInfo)
    {

        if (isRewardGiven)
        {
            if (_callback == null)
            {
                return;
            }
            _callback.Invoke(true);
            _callback = null;
        }
        else
        {
            if (_callback == null)
            {
                return;
            }
            _callback.Invoke(false);
            _callback = null;
        }
        isRewardShowing = false;
        //Time.timeScale = 1;
        // GiveRewardToUser();
        Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
    }
    private void RewardedVideoOnAdUnavailable()
    {
        Debug.Log("unity-script: RewardedVideoOnAdUnavailable");
        //throw new NotImplementedException();
    }

    private void RewardedVideoOnAdAvailable(IronSourceAdInfo obj)
    {
        Debug.Log("unity-script: RewardedVideoOnAdAvailable");
        //  throw new NotImplementedException();
    }
    void RewardedVideoAdStartedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
    }

    void RewardedVideoAdEndedEvent()
    {
        isRewardShowing = false;
        Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
    }

    void RewardedVideoAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        isRewardShowing = false;
        if (isRewardGiven)
        {
            if (_callback == null)
            {
                return;
            }
            _callback.Invoke(false);
            _callback = null;
        }
        // Time.timeScale = 1;
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + placement.getRewardName());
    }
    #endregion

}
