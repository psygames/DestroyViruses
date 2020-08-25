using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

#if USE_FIREBASE
using Firebase.Extensions;
#endif

namespace DestroyViruses
{
    public class RemoteConfigProxy : ProxyBase<RemoteConfigProxy>
    {
        public bool isInit { get; private set; }

        private const string CONST_GROUP = "const_group";
        private const string MIN_VERSION = "min_version";
        private const string LATEST_VERSION = "latest_version";

        protected override void OnInit()
        {
            base.OnInit();
#if UNITY_EDITOR
            return;
#endif
            GameManager.Instance.DelayDo(1.2f, () =>
            {
                // FirebaseChecker.Check(InitializeFirebase);
            });
        }

        void InitializeFirebase()
        {
            UnityEngine.Debug.Log("AnalyticsFacebook Inited");
            var defaults = new Dictionary<string, object>();
            defaults.Add(CONST_GROUP, GameLocalData.Instance.lastConstGroup);
            defaults.Add(MIN_VERSION, GameLocalData.Instance.minVersion);
            defaults.Add(LATEST_VERSION, GameLocalData.Instance.latestVersion);
            // Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
            // var fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync();
            // fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        void FetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
            {
                Debug.LogError("Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.LogError("Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed successfully!");
            }

            /*
            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
            switch (info.LastFetchStatus)
            {
                case Firebase.RemoteConfig.LastFetchStatus.Success:
                    Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                    isInit = true;
                    Debug.Log(string.Format("Remote data loaded and ready (last fetch time {0}).", info.FetchTime));
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case Firebase.RemoteConfig.FetchFailureReason.Error:
                            Debug.LogError("Fetch failed for unknown reason");
                            break;
                        case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                            Debug.LogError("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Pending:
                    Debug.LogError("Latest Fetch call still pending.");
                    break;
            }
            */
        }

        private void OnInitSuccess()
        {
            /*
            Debug.Log("RemoteConfig Init Success!");
            var allKeys = Firebase.RemoteConfig.FirebaseRemoteConfig.Keys;
            LogKeys(allKeys);
            Func<string, bool> hasKey = (str) =>
            {
                foreach (var k in allKeys)
                    if (k == str)
                        return true;
                return false;
            };

            Func<string, Firebase.RemoteConfig.ConfigValue> getVal = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue;

            bool hasChange = false;
            if (hasKey(CONST_GROUP))
            {
                hasChange = true;
                GameLocalData.Instance.lastConstGroup = getVal(CONST_GROUP).StringValue;
            }
            if (hasKey(MIN_VERSION))
            {
                hasChange = true;
                GameLocalData.Instance.minVersion = getVal(MIN_VERSION).StringValue;
            }
            if (hasKey(LATEST_VERSION))
            {
                hasChange = true;
                GameLocalData.Instance.latestVersion = getVal(LATEST_VERSION).StringValue;
            }

            if (hasChange)
            {
                GameLocalData.Instance.Save();
            }
            */
        }

        public string GetValue(string key, string defaultVal = "")
        {
            /*
            foreach (var k in Firebase.RemoteConfig.FirebaseRemoteConfig.Keys)
                if (k == key)
                    return Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(k).StringValue;
            return defaultVal;
            */
            return defaultVal;
        }

        private void LogKeys(IEnumerable<string> keys)
        {
            /*
#if !PUBLISH_BUILD
            foreach (var k in keys)
            {
                var v = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(k);
                Debug.Log($"[Remote Config] {k}: {v.StringValue}");
            }
#endif
            */
        }

        private bool mLastInit = false;
        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (!mLastInit && isInit)
            {
                OnInitSuccess();
            }
            mLastInit = isInit;
        }
    }
}
