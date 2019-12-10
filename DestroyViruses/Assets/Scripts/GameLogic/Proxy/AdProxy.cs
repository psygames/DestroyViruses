using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class AdProxy : ProxyBase<AdProxy>
    {
        private string defalutAdUnitID { get { return TableAds.Get("default").unitID; } }
        private readonly Dictionary<string, bool> _adUnitToLoadedMapping = new Dictionary<string, bool>();
        private readonly Dictionary<string, float> _adUnitLoadingMapping = new Dictionary<string, float>();
        private readonly Dictionary<string, List<MoPub.Reward>> _adUnitToRewardsMapping =
            new Dictionary<string, List<MoPub.Reward>>();
        private float m_delayCacheRewardAll;

        public const float AD_AUTO_LOAD_DELAY = 5;
        public const float AD_LOAD_TIMEOUT = 60;

        public const bool ENABLE_LOG = true;

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
            base.OnDestroy();
        }

        public bool ShowAd(string adId)
        {
            var ad = TableAds.Get(adId);
            if (ad == null)
            {
                Debug.LogError("播放广告失败，配置表不存在项：" + adId);
                return false;
            }
            Analytics.Event.ClickAd(ad.id, ad.unitID);
            if (ad.type == "RewardedVideo")
            {
                return ShowRewardVideo(ad.unitID);
            }

            if (ad.type == "Interstitial")
            {
                return ShowInterstitial(ad.unitID);
            }

            Log(adId, "类型尚未继承", ad.type, false, "UnitID is AD ID ");
            return false;
        }


        private void MoPubManager_OnSdkInitializedEvent(string obj)
        {
            LoadPlugins();
            m_delayCacheRewardAll = AD_AUTO_LOAD_DELAY;
        }

        private void LoadPlugins()
        {
            Debug.Log("加载 AD Plugins");
            LoadInterstitialPlugin();
            LoadRewardedVideosPlugin();
        }

        private void CacheRequestAll()
        {
            foreach (var adUnit in GetAdUnits("RewardedVideo"))
            {
                RequestRewardedVideo(adUnit);
            }

            foreach (var adUnit in GetAdUnits("Interstitial"))
            {
                RequestInterstitial(adUnit);
            }
        }

        private void RequestRewardedVideo(string adUnit)
        {
            if (_adUnitLoadingMapping.TryGetValue(adUnit, out var tm))
            {
                if (Time.time - tm < AD_LOAD_TIMEOUT)
                {
                    Log(adUnit, "奖励视频", "正在加载 ", "请稍等。");
                }
                else
                {
                    Log(adUnit, "奖励视频", "加载超时", "重新请求。");
                    _adUnitLoadingMapping[adUnit] = Time.time;
                    MoPub.RequestRewardedVideo(adUnit, customerId: DeviceID.UUID);
                }
            }
            else
            {
                Log(adUnit, "奖励视频", "开始加载", "");
                _adUnitLoadingMapping[adUnit] = Time.time;
                MoPub.RequestRewardedVideo(adUnit, customerId: DeviceID.UUID);
            }
        }

        private bool ShowRewardVideo(string adUnit)
        {
            if (!HasLoaded(adUnit) || !MoPub.HasRewardedVideo(adUnit))
            {
                RequestRewardedVideo(adUnit);
                return false;
            }
            MoPub.ShowRewardedVideo(adUnit);
            return true;
        }


        private void RequestInterstitial(string adUnit)
        {
            if (_adUnitLoadingMapping.TryGetValue(adUnit, out var tm))
            {
                if (Time.time - tm < AD_LOAD_TIMEOUT)
                {
                    Log(adUnit, "插屏广告", "正在加载 ", "请稍等。");
                }
                else
                {
                    Log(adUnit, "插屏广告", "加载超时", "重新请求。");
                    _adUnitLoadingMapping[adUnit] = Time.time;
                    MoPub.RequestInterstitialAd(adUnit);
                }
            }
            else
            {
                Log(adUnit, "插屏广告", "开始加载", "");
                _adUnitLoadingMapping[adUnit] = Time.time;
                MoPub.RequestInterstitialAd(adUnit);
            }
        }

        private bool ShowInterstitial(string adUnit)
        {
            if (!HasLoaded(adUnit))
            {
                RequestInterstitial(adUnit);
                return false;
            }
            MoPub.ShowInterstitialAd(adUnit);
            return true;
        }


        private bool HasLoaded(string adUnit)
        {
            _adUnitToLoadedMapping.TryGetValue(adUnit, out var val);
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
            _adUnitToLoadedMapping[adUnit] = true;
            _adUnitLoadingMapping.Remove(adUnit);
        }

        private void AdDismissed(string adUnit)
        {
            _adUnitToLoadedMapping[adUnit] = false;
        }

        private void LoadAvailableRewards(string adUnitId, List<MoPub.Reward> availableRewards)
        {
            _adUnitToRewardsMapping.Remove(adUnitId);
            if (availableRewards != null)
            {
                _adUnitToRewardsMapping[adUnitId] = availableRewards;
            }
        }


        private void OnInterstitialLoadedEvent(string adUnitId)
        {
            Log(adUnitId, "插屏广告", "加载", true);
            AdLoaded(adUnitId);
        }


        private void OnInterstitialFailedEvent(string adUnitId, string error)
        {
            Log(adUnitId, "插屏广告", "加载", false, error);
            _adUnitLoadingMapping.Remove(adUnitId);
        }

        private void OnInterstitialClosedEvent(string adUnitId)
        {
            Log(adUnitId, "插屏广告", "播放", true);
            AdDismissed(adUnitId);
            RequestInterstitial(adUnitId);
        }


        private void OnRewardedVideoLoadedEvent(string adUnitId)
        {
            Log(adUnitId, "奖励视频", "加载", true);
            var availableRewards = MoPub.GetAvailableRewards(adUnitId);
            AdLoaded(adUnitId);
            LoadAvailableRewards(adUnitId, availableRewards);
        }


        private void OnRewardedVideoFailedEvent(string adUnitId, string error)
        {
            Log(adUnitId, "奖励视频", "加载", false, error);
            _adUnitLoadingMapping.Remove(adUnitId);
        }


        private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
        {
            Log(adUnitId, "奖励视频", "播放", false, error);
            AdDismissed(adUnitId);
        }

        private void OnRewardedVideoClosedEvent(string adUnitId)
        {
            Log(adUnitId, "奖励视频", "播放", true);
            AdDismissed(adUnitId);
            RequestRewardedVideo(adUnitId);
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

        private void Log(string unitID, string name, string action, string end)
        {
            if (!ENABLE_LOG)
                return;
            Debug.Log($"{action}{name} {end}: {unitID}");
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (m_delayCacheRewardAll > 0)
            {
                m_delayCacheRewardAll -= Time.deltaTime;
                if (m_delayCacheRewardAll <= 0)
                {
                    CacheRequestAll();
                }
            }
        }
    }
}