﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DestroyViruses
{
    public class AdProxy : ProxyBase<AdProxy>
    {
        private string defalutAdUnitID { get { return TableAds.Get("default").unitID; } }
        private readonly Dictionary<string, bool> loadedAds = new Dictionary<string, bool>();
        private readonly Dictionary<string, float> loadingAds = new Dictionary<string, float>();
        private readonly Dictionary<string, List<MoPub.Reward>> rewards = new Dictionary<string, List<MoPub.Reward>>();

        public const float AD_AUTO_LOAD_DELAY = 1;
        public const float AD_AUTO_LOAD_INTERAL = 1;
        public const float AD_LOAD_TIMEOUT = 10f;

        public const bool ENABLE_LOG = true;
        public bool isInit { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();
            MoPubManager.OnSdkInitializedEvent += MoPubManager_OnSdkInitializedEvent;
            MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
            MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
            MoPubManager.OnInterstitialDismissedEvent += OnInterstitialClosedEvent;
            MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
            MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
            MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
            MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
            MoPubManager.Initial(defalutAdUnitID);
        }

        protected override void OnDestroy()
        {
            MoPubManager.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
            MoPubManager.OnInterstitialFailedEvent -= OnInterstitialFailedEvent;
            MoPubManager.OnInterstitialDismissedEvent -= OnInterstitialClosedEvent;
            MoPubManager.OnRewardedVideoLoadedEvent -= OnRewardedVideoLoadedEvent;
            MoPubManager.OnRewardedVideoFailedEvent -= OnRewardedVideoFailedEvent;
            MoPubManager.OnRewardedVideoFailedToPlayEvent -= OnRewardedVideoFailedToPlayEvent;
            MoPubManager.OnRewardedVideoClosedEvent -= OnRewardedVideoClosedEvent;
            MoPubManager.OnSdkInitializedEvent -= MoPubManager_OnSdkInitializedEvent;
            isInit = false;
            base.OnDestroy();
        }

        private string currentShowUnitId;
        private Action currentSuccessCallback;
        private Action currentErrorCallback;
        private float currentLoadingCD;

        private void AutoPlay(string unitID)
        {
            HoldOn.Stop();
            if (currentShowUnitId != unitID)
                return;
            MoPub.ShowRewardedVideo(unitID);
        }

        private void Success(string unitID)
        {
            HoldOn.Stop();
            if (currentShowUnitId != unitID)
                return;
            currentShowUnitId = "";
            currentLoadingCD = 0;
            currentSuccessCallback?.Invoke();
        }

        private void Failed(string unitID)
        {
            HoldOn.Stop();
            if (currentShowUnitId != unitID)
                return;
            currentShowUnitId = "";
            currentLoadingCD = 0;
            currentErrorCallback?.Invoke();
        }

        public void ShowAd(string adId, Action successCallback, Action errorCallback)
        {
            currentSuccessCallback = successCallback;
            currentErrorCallback = errorCallback;
            var ad = TableAds.Get(adId);
            if (ad == null)
            {
                Log(adId, "广告", "播放", false, "播放广告失败，配置表不存在项");
                Failed(currentShowUnitId);
                return;
            }

#if !UNITY_EDITOR
            HoldOn.Start();
#endif
            currentShowUnitId = ad.unitID;
            currentLoadingCD = AD_LOAD_TIMEOUT;

            Analytics.Event.ClickAd(ad.id, ad.unitID); 
            if (ad.type == "RewardedVideo")
            {
                ShowRewardVideo(ad.unitID);
            }
            else if (ad.type == "Interstitial")
            {
                ShowInterstitial(ad.unitID);
            }
            else
            {
                Failed(currentShowUnitId);
                Log(adId, "类型尚未集成", ad.type, false, "UnitID is AD ID ");
            }
        }


        private void MoPubManager_OnSdkInitializedEvent(string obj)
        {
            isInit = true;
            LoadPlugins();
            m_delayCacheAd = AD_AUTO_LOAD_DELAY;
        }

        private void LoadPlugins()
        {
            LoadInterstitialPlugin();
            LoadRewardedVideosPlugin();
        }

        private void RequestAd(string adId)
        {
            var ad = TableAds.Get(adId);
            if (ad != null)
            {
                if (ad.type == "RewardedVideo")
                {
                    RequestRewardedVideo(ad.unitID);
                }

                if (ad.type == "Interstitial")
                {
                    RequestInterstitial(ad.unitID);
                }
            }
        }

        private void RequestRewardedVideo(string adUnit)
        {
            if (loadingAds.TryGetValue(adUnit, out var tm))
            {
                Log(adUnit, "奖励视频", "正在加载 ", "请稍等。");
            }
            else
            {
                Log(adUnit, "奖励视频", "开始加载", "");
                loadingAds[adUnit] = Time.time;
                MoPub.RequestRewardedVideo(adUnit);
            }
        }

        private void ShowRewardVideo(string adUnit)
        {
            if (!HasLoaded(adUnit) || !MoPub.HasRewardedVideo(adUnit))
            {
                RequestRewardedVideo(adUnit);
                return;
            }
            MoPub.ShowRewardedVideo(adUnit);
        }

        private void RequestInterstitial(string adUnit)
        {
            if (loadingAds.TryGetValue(adUnit, out var tm))
            {
                Log(adUnit, "插屏广告", "正在加载 ", "请稍等。");
            }
            else
            {
                Log(adUnit, "插屏广告", "开始加载", "");
                loadingAds[adUnit] = Time.time;
                MoPub.RequestInterstitialAd(adUnit);
            }
        }

        private void ShowInterstitial(string adUnit)
        {
            if (!HasLoaded(adUnit) || !MoPub.IsInterstitialReady(adUnit))
            {
                RequestInterstitial(adUnit);
                return;
            }
            MoPub.ShowInterstitialAd(adUnit);
        }


        private bool HasLoaded(string adUnit)
        {
            loadedAds.TryGetValue(adUnit, out var val);
            return val;
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
            MoPub.LoadInterstitialPluginsForAdUnits(adUnits.ToArray());
        }

        private void LoadRewardedVideosPlugin()
        {
            var adUnits = GetAdUnits("RewardedVideo");
            MoPub.LoadRewardedVideoPluginsForAdUnits(adUnits.ToArray());
        }

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
            if (loadingAds.ContainsKey(adUnit))
                loadingAds.Remove(adUnit);
        }

        private void LoadAvailableRewards(string adUnitId, List<MoPub.Reward> availableRewards)
        {
            rewards.Remove(adUnitId);
            if (availableRewards != null)
            {
                rewards[adUnitId] = availableRewards;
            }
        }

        private void OnInterstitialLoadedEvent(string adUnitId)
        {
            Log(adUnitId, "插屏广告", "加载", true);
            AdLoaded(adUnitId);
            AutoPlay(adUnitId);
        }


        private void OnInterstitialFailedEvent(string adUnitId, string error)
        {
            Log(adUnitId, "插屏广告", "加载", false, error);
            RemoveLoading(adUnitId);
            Failed(adUnitId);
        }

        private void OnInterstitialClosedEvent(string adUnitId)
        {
            Log(adUnitId, "插屏广告", "播放", true);
            AdDismissed(adUnitId);
            Success(adUnitId);
            if (NeedCache(adUnitId))
            {
                RequestInterstitial(adUnitId);
            }
        }


        private void OnRewardedVideoLoadedEvent(string adUnitId)
        {
            Log(adUnitId, "奖励视频", "加载", true);
            var availableRewards = MoPub.GetAvailableRewards(adUnitId);
            AdLoaded(adUnitId);
            LoadAvailableRewards(adUnitId, availableRewards);
            AutoPlay(adUnitId);
        }


        private void OnRewardedVideoFailedEvent(string adUnitId, string error)
        {
            Log(adUnitId, "奖励视频", "加载", false, error);
            RemoveLoading(adUnitId);
            Failed(adUnitId);
        }


        private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
        {
            Log(adUnitId, "奖励视频", "播放", false, error);
            AdDismissed(adUnitId);
            Failed(adUnitId);
        }

        private void OnRewardedVideoClosedEvent(string adUnitId)
        {
            Log(adUnitId, "奖励视频", "播放", true);
            AdDismissed(adUnitId);
            Success(adUnitId);
            if (NeedCache(adUnitId))
            {
                RequestRewardedVideo(adUnitId);
            }
        }


        private void Log(string unitID, string name, string action, bool success, string error = "")
        {
            if (!ENABLE_LOG)
                return;

            if (success)
            {
                Debug.Log($"{action}{name} 完成: {unitID}");
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

        private bool NeedCache(string unitID)
        {
            foreach (var aid in m_needAutoCacheAds)
            {
                if (TableAds.Get(aid).unitID == unitID)
                    return true;
            }
            return false;
        }

        private void Log(string unitID, string name, string action, string end)
        {
            if (!ENABLE_LOG)
                return;
            Debug.Log($"{action}{name} {end}: {unitID}");
        }

        private float m_cacheAdCd = 0;
        private float m_delayCacheAd;
        private int m_autoCacheIndex = 0;
        private List<string> m_needAutoCacheAds = new List<string>();
        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (m_delayCacheAd > 0)
            {
                m_delayCacheAd -= Time.deltaTime;
                if (m_delayCacheAd <= 0)
                {
                    m_needAutoCacheAds.Clear();
                    foreach (var t in TableAds.GetAll())
                    {
                        if (t.preloadPriority <= 0)
                            continue;
                        m_needAutoCacheAds.Add(t.id);
                    }

                    m_needAutoCacheAds.Sort((a, b) =>
                    {
                        var ta = TableAds.Get(a).preloadPriority;
                        var tb = TableAds.Get(b).preloadPriority;
                        return tb.CompareTo(ta);
                    });
                }
            }

            m_cacheAdCd -= Time.deltaTime;
            if (m_cacheAdCd <= 0 && m_autoCacheIndex < m_needAutoCacheAds.Count)
            {
                m_cacheAdCd = AD_AUTO_LOAD_INTERAL;
                RequestAd(m_needAutoCacheAds[m_autoCacheIndex]);
                m_autoCacheIndex++;
            }

            if (!string.IsNullOrEmpty(currentShowUnitId) && currentLoadingCD > 0)
            {
                currentLoadingCD -= Time.deltaTime;
                if (currentLoadingCD <= 0)
                {
                    Failed(currentShowUnitId);
                }
            }
        }
    }
}