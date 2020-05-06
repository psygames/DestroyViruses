using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DestroyViruses
{
    public class AdProxy : ProxyBase<AdProxy>
    {
        public enum AdType
        {
            None = 0,
            Banner = 1,
            Interstitial = 2,
            RewardedVideo = 3,
        }

        private string defalutAdUnitID { get { return TableAds.GetAll().First().unitID; } }
        private readonly Dictionary<string, bool> loadedAds = new Dictionary<string, bool>();
        private readonly Dictionary<string, float> loadingAds = new Dictionary<string, float>();
        private readonly Dictionary<string, float> delayLoadingAds = new Dictionary<string, float>();

        public const float AD_AUTO_LOAD_DELAY = 1;
        public const float AD_AUTO_LOAD_INTERAL = 5;
        public const float AD_LOAD_TIMEOUT = 10f;
        public const float AD_LOAD_FAILED_RETRY_DELAY = 60f;

        public bool isInit { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();

            GameManager.Instance.DelayDo(1, () =>
            {
                MoPubManager.OnSdkInitializedEvent += MoPubManager_OnSdkInitializedEvent;

                MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
                MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
                MoPubManager.OnInterstitialDismissedEvent += OnInterstitialClosedEvent;

                MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
                MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
                MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
                MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReveicedRewardEvent;
                MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;

                MoPub.InitializeSdk(defalutAdUnitID);
            });
        }

        private void LogEventInit()
        {
            MoPubManager.OnInterstitialClickedEvent += _ => Debug.LogError("OnInterstitialClickedEvent: " + _);
            MoPubManager.OnInterstitialDismissedEvent += _ => Debug.LogError("OnInterstitialDismissedEvent: " + _);
            MoPubManager.OnInterstitialExpiredEvent += _ => Debug.LogError("OnInterstitialExpiredEvent: " + _);
            MoPubManager.OnInterstitialFailedEvent += (_, error) => Debug.LogError("OnInterstitialFailedEvent: " + _ + " error: " + error);
            MoPubManager.OnInterstitialLoadedEvent += _ => Debug.LogError("OnInterstitialLoadedEvent: " + _);
            MoPubManager.OnInterstitialShownEvent += _ => Debug.LogError("OnInterstitialShownEvent: " + _);
            MoPubManager.OnInterstitialClickedEvent += _ => Debug.LogError("OnInterstitialClickedEvent: " + _);

            MoPubManager.OnRewardedVideoLoadedEvent += _ => Debug.LogError("OnRewardedVideoLoadedEvent: " + _);
            MoPubManager.OnRewardedVideoFailedEvent += (_, error) => Debug.LogError("OnRewardedVideoFailedEvent: " + _ + " error: " + error);
            MoPubManager.OnRewardedVideoExpiredEvent += _ => Debug.LogError("OnRewardedVideoExpiredEvent: " + _);
            MoPubManager.OnRewardedVideoShownEvent += _ => Debug.LogError("OnRewardedVideoShownEvent: " + _);
            MoPubManager.OnRewardedVideoClickedEvent += _ => Debug.LogError("OnRewardedVideoClickedEvent: " + _);
            MoPubManager.OnRewardedVideoFailedToPlayEvent += (_, error) => Debug.LogError("OnRewardedVideoFailedToPlayEvent: " + _ + " error: " + error);
            MoPubManager.OnRewardedVideoReceivedRewardEvent += (_, label, amount) => Debug.LogError("OnRewardedVideoReceivedRewardEvent: " + _ + " label: " + label + " amount: " + amount);
            MoPubManager.OnRewardedVideoClosedEvent += _ => Debug.LogError("OnRewardedVideoClosedEvent: " + _);
            MoPubManager.OnRewardedVideoLeavingApplicationEvent += _ => Debug.LogError("OnRewardedVideoLeavingApplicationEvent: " + _);
        }

        protected override void OnDestroy()
        {
            MoPubManager.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
            MoPubManager.OnInterstitialFailedEvent -= OnInterstitialFailedEvent;
            MoPubManager.OnInterstitialDismissedEvent -= OnInterstitialClosedEvent;

            MoPubManager.OnRewardedVideoLoadedEvent -= OnRewardedVideoLoadedEvent;
            MoPubManager.OnRewardedVideoFailedEvent -= OnRewardedVideoFailedEvent;
            MoPubManager.OnRewardedVideoFailedToPlayEvent -= OnRewardedVideoFailedToPlayEvent;
            MoPubManager.OnRewardedVideoReceivedRewardEvent -= OnRewardedVideoReveicedRewardEvent;
            MoPubManager.OnRewardedVideoClosedEvent -= OnRewardedVideoClosedEvent;

            MoPubManager.OnSdkInitializedEvent -= MoPubManager_OnSdkInitializedEvent;
            isInit = false;
            base.OnDestroy();
        }

        #region INITIALIZE
        private void MoPubManager_OnSdkInitializedEvent(string obj)
        {
            Log("", "MoPub SDK", "初始化", true);
            isInit = true;
            LoadPlugins();
            DelayRequestAll();
        }

        private void LoadPlugins()
        {
            LoadInterstitialPlugin();
            LoadRewardedVideosPlugin();
        }

        private List<string> GetAdUnits(string type)
        {
            var ads = TableAds.GetAll().ToList(a => a.type == type);
            var adUnits = new List<string>();
            foreach (var ad in ads)
            {
                if (!adUnits.Contains(ad.unitID))
                {
                    adUnits.Add(ad.unitID);
                }
            }
            return adUnits;
        }

        private void LoadInterstitialPlugin()
        {
            var adUnits = GetAdUnits("Interstitial");
            if (adUnits.Count > 0)
            {
                MoPub.LoadInterstitialPluginsForAdUnits(adUnits.ToArray());
                Log("", "插屏模块插件", "初始化", true);
            }
        }

        private void LoadRewardedVideosPlugin()
        {
            var adUnits = GetAdUnits("RewardedVideo");
            if (adUnits.Count > 0)
            {
                MoPub.LoadRewardedVideoPluginsForAdUnits(adUnits.ToArray());
                Log("", "奖励视频模块插件", "初始化", true);
            }
        }
        #endregion

        #region API
        public void ShowAd(Action successCallback, Action errorCallback)
        {
            currentSuccessCallback = successCallback;
            currentErrorCallback = errorCallback;
            currentShowingAd = true;
            currentNeedAutoPlay = false;
            currentWaitForAutoPlayCD = 0;

            var topAd = "";
            var topAdPriority = 0f;
            foreach (var ad in TableAds.GetAll())
            {
                if (!IsLoaded(ad.unitID))
                    continue;
                if (ad.priority > topAdPriority)
                {
                    topAd = ad.unitID;
                    topAdPriority = ad.priority;
                }
            }

            if (!string.IsNullOrEmpty(topAd))
            {
                ShowAd(topAd);
            }
            else if (loadingAds.Count > 0)
            {
                currentNeedAutoPlay = true;
                currentWaitForAutoPlayCD = AD_LOAD_TIMEOUT;
                HoldOn();
            }
            else
            {
                Failed();
            }
        }
        #endregion

        #region FUNCTIONS
        private Action currentSuccessCallback;
        private Action currentErrorCallback;
        private bool currentShowingAd;
        private bool currentNeedAutoPlay;
        private float currentWaitForAutoPlayCD;

        private void AutoPlay(string unitID)
        {
            if (!currentShowingAd || !currentNeedAutoPlay)
                return;
            Log(unitID, "广告", "自动播放");
            CancelHoldOn();
            currentNeedAutoPlay = false;
            currentWaitForAutoPlayCD = 0;
            ShowAd(unitID);
        }

        private void Success()
        {
            if (!currentShowingAd)
                return;
            CancelHoldOn();
            currentShowingAd = false;
            currentNeedAutoPlay = false;
            currentWaitForAutoPlayCD = 0;
            currentSuccessCallback?.Invoke();
        }

        private void Failed()
        {
            if (!currentShowingAd)
                return;
            CancelHoldOn();
            currentShowingAd = false;
            currentNeedAutoPlay = false;
            currentWaitForAutoPlayCD = 0;
            currentErrorCallback?.Invoke();
        }

        private AdType GetAdType(string adUnit)
        {
            switch (TableAds.Get(a => a.unitID == adUnit).type)
            {
                case "Banner":
                    return AdType.Banner;
                case "Interstitial":
                    return AdType.Interstitial;
                case "RewardedVideo":
                    return AdType.RewardedVideo;
            }
            return AdType.None;
        }

        private void HoldOn()
        {
#if !UNITY_EDITOR
            DestroyViruses.HoldOn.Start();
#endif
        }

        private void CancelHoldOn()
        {
#if !UNITY_EDITOR
            DestroyViruses.HoldOn.Stop();
#endif
        }

        private bool IsLoaded(string unitID)
        {
            loadedAds.TryGetValue(unitID, out var val);
            var adType = GetAdType(unitID);
            if (adType == AdType.RewardedVideo)
                return val && MoPub.HasRewardedVideo(unitID);
            if (adType == AdType.Interstitial)
                return val && MoPub.IsInterstitialReady(unitID);
            return val;
        }

        private bool IsLoading(string unitID)
        {
            return loadingAds.ContainsKey(unitID);
        }

        private void ShowAd(string unitID)
        {
            var adType = GetAdType(unitID);
            if (adType == AdType.Interstitial)
            {
                Log(unitID, "插屏广告", "播放");
                MoPub.ShowInterstitialAd(unitID);
            }
            else if (adType == AdType.RewardedVideo)
            {
                Log(unitID, "奖励视频", "播放");
                MoPub.ShowRewardedVideo(unitID);
            }
            else
            {
                Log(unitID, "广告", "开始播放", false, "unitID 不存在！");
            }
        }

        private void RequestAd(string unitID)
        {
            var adType = GetAdType(unitID);
            if (adType == AdType.Interstitial)
            {
                Log(unitID, "插屏广告", "请求");
                loadingAds[unitID] = Time.time;
                MoPub.RequestInterstitialAd(unitID);
            }
            else if (adType == AdType.RewardedVideo)
            {
                Log(unitID, "奖励视频", "请求");
                loadingAds[unitID] = Time.time;
                MoPub.RequestRewardedVideo(unitID);
            }
            else
            {
                Log(unitID, "广告", "请求", false, "unitID 不存在！");
            }
        }
        #endregion

        private void AdLoaded(string adUnit)
        {
            loadedAds[adUnit] = true;
            RemoveLoading(adUnit);
        }

        private void AdDismissed(string adUnit)
        {
            loadedAds[adUnit] = false;
            RemoveLoading(adUnit);
        }

        private void RemoveLoading(string adUnit)
        {
            if (IsLoading(adUnit))
                loadingAds.Remove(adUnit);
        }

        private void OnInterstitialLoadedEvent(string adUnitId)
        {
            if (IsLoaded(adUnitId))
                return;
            Log(adUnitId, "插屏广告", "加载", true);
            AdLoaded(adUnitId);
            AutoPlay(adUnitId);
        }

        private void OnInterstitialFailedEvent(string adUnitId, string error)
        {
            Log(adUnitId, "插屏广告", "加载", false, error);
            RemoveLoading(adUnitId);
            RequestAdDelay(adUnitId);
        }

        private void OnInterstitialClosedEvent(string adUnitId)
        {
            if (!loadedAds.ContainsKey(adUnitId) || !loadedAds[adUnitId])
                return;
            Log(adUnitId, "插屏广告", "获得奖励", true);
            AdDismissed(adUnitId);
            Success();
            RequestAd(adUnitId);
        }


        private void OnRewardedVideoLoadedEvent(string adUnitId)
        {
            if (IsLoaded(adUnitId))
                return;
            Log(adUnitId, "奖励视频", "加载", true);
            AdLoaded(adUnitId);
            AutoPlay(adUnitId);
        }


        private void OnRewardedVideoFailedEvent(string adUnitId, string error)
        {
            Log(adUnitId, "奖励视频", "加载", false, error);
            RemoveLoading(adUnitId);
            RequestAdDelay(adUnitId);
        }


        private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
        {
            Log(adUnitId, "奖励视频", "播放", false, error);
            AdDismissed(adUnitId);
            RequestAd(adUnitId);
        }

        private void OnRewardedVideoReveicedRewardEvent(string adUnitId, string label, float amount)
        {
            if (!loadedAds.ContainsKey(adUnitId) || !loadedAds[adUnitId])
                return;
            Log(adUnitId, "奖励视频", "获得奖励", true);
            AdDismissed(adUnitId);
            Success();
            RequestAd(adUnitId);
        }

        private void OnRewardedVideoClosedEvent(string adUnitId)
        {
            if (!loadedAds.ContainsKey(adUnitId) || !loadedAds[adUnitId])
                return;
            Log(adUnitId, "奖励视频", "获得奖励", false);
            AdDismissed(adUnitId);
#if UNITY_EDITOR
            Success();
#else
            Failed();
#endif
            RequestAd(adUnitId);
        }


        private void Log(string unitID, string name, string action)
        {
#if !ENABLE_LOG
            return;
#endif
            Debug.Log($"{action}{name}: {unitID}");
        }

        private void Log(string unitID, string name, string action, bool success, string error = "")
        {
#if !ENABLE_LOG
            return;
#endif

            if (success)
            {
                Debug.Log($"{action}{name} 成功: {unitID}");
            }
            else
            {
                if (string.IsNullOrEmpty(error))
                {
                    Debug.Log($"{action}{name} 失败: {unitID}");
                }
                else
                {
                    Debug.LogError($"{action}{name} 失败: {unitID}\nError: {error}");
                }
            }
        }

        private void DelayRequestAll()
        {
            var allAds = TableAds.GetAll().ToList(a => a.priority > 0);
            for (int i = 0; i < allAds.Count; i++)
            {
                RequestAdDelay(allAds[i].unitID, i * AD_AUTO_LOAD_INTERAL + AD_AUTO_LOAD_DELAY);
            }
        }

        private void RequestAdDelay(string unitID, float delay = AD_LOAD_FAILED_RETRY_DELAY)
        {
            delayLoadingAds[unitID] = delay;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            // DELAY REQUEST AD
            var keys = delayLoadingAds.Keys.ToList();
            foreach (var k in keys)
            {
                delayLoadingAds[k] -= Time.deltaTime;
                if (delayLoadingAds[k] <= 0)
                {
                    delayLoadingAds.Remove(k);
                    RequestAd(k);
                }
            }

            // TIMEOUT
            if (currentShowingAd && currentNeedAutoPlay && currentWaitForAutoPlayCD > 0)
            {
                currentWaitForAutoPlayCD -= Time.deltaTime;
                if (currentWaitForAutoPlayCD <= 0)
                {
                    Failed();
                }
            }
        }
    }
}