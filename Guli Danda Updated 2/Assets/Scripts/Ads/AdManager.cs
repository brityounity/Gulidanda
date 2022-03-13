using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    private DatabaseController databaseController;
    private HomeSceneManager homeSceneManager;
    private BannerView bannerAd;
    private InterstitialAd interstitialAd;

    public bool showBannerAd;
    public void Start()
    {
        databaseController = FindObjectOfType<DatabaseController>();
        homeSceneManager = GetComponent<HomeSceneManager>();
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        if (showBannerAd == true)
        {
            this.RequestBanner();
        }

        
    }
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator) 
            .AddKeyword("unity-admob-sample")
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")
            .Build();
    }
    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-2757805452219503/2259901487";
#if UNITY_ANDROID
         adUnitId = "ca-app-pub-2757805452219503/2259901487";
        #elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-2757805452219503/2398490529";
        #else
                 string adUnitId = "unexpected_platform";
        #endif
        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        // Create a 320x50 banner at the top of the screen.
        this.bannerAd = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerAd.LoadAd(request);

    }


    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-2757805452219503/9143846365";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-2757805452219503/3828968175";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(adUnitId);
        interstitialAd.OnAdClosed += HandleRewardedAdClosed;
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitialAd.LoadAd(CreateAdRequest());
        // Load an interstitial ad
        ShowInterstitialAd();
    }

    private void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
           
        }
        else
        {
            Debug.Log("not ready");
        }
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        DateTime setBlockTime = DateTime.Now.AddMinutes(60);
        databaseController.AddCoin(500);
        PlayerPrefs.SetString("block time", setBlockTime.ToBinary().ToString());
        homeSceneManager.coinText.text = databaseController.GetUserInfo().total_coin.ToString();
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }
    #endregion
}
