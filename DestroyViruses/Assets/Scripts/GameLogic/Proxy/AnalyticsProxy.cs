using UnityEngine;
using System.Collections.Generic;
using System;

using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;

namespace DestroyViruses
{
    public class AnalyticsProxy : ProxyBase<AnalyticsProxy>
    {
        public bool isInit { get; private set; }
        protected override void OnInit()
        {
            base.OnInit();
            FirebaseChecker.Check(() =>
            {
                InitializeFirebase();
            });
        }

        void InitializeFirebase()
        {
            isInit = true;
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            FirebaseAnalytics.SetUserId(DeviceID.UUID);
            FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));

            D.I.AnalyticsSetUserProperty();
            Analytics.Event.Login(DeviceID.UUID);
        }

        public void ResetAnalyticsData()
        {
            if (!isInit)
                return;
            FirebaseAnalytics.ResetAnalyticsData();
        }

        #region UserProperty

        public void SetUserProperty(string name, string property)
        {
            if (!isInit)
            {
                LogWarning($"SetUserProperty [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.SetUserProperty(name, property);
        }

        public void SetUserProperty(string name, object obj)
        {
            if (!isInit)
            {
                LogWarning($"SetUserProperty [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.SetUserProperty(name, obj.ToString());
        }

        #endregion


        #region LogEvent

        public void LogEvent(string name)
        {
            if (!isInit)
            {
                LogWarning($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name);
        }

        public void LogEvent(string name, string parameterName, int parameterValue)
        {
            if (!isInit)
            {
                LogWarning($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public void LogEvent(string name, string parameterName, long parameterValue)
        {
            if (!isInit)
            {
                LogWarning($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public void LogEvent(string name, string parameterName, double parameterValue)
        {
            if (!isInit)
            {
                LogWarning($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public void LogEvent(string name, string parameterName, string parameterValue)
        {
            if (!isInit)
            {
                LogWarning($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public void LogEvent4Log(string name, string message, string stackTrack)
        {
            if (!isInit)
                return;
            FirebaseAnalytics.LogEvent(name,
                new Parameter("message", message),
                new Parameter("stack_track", stackTrack));
        }

        public void LogEvent(string name, params Parameter[] parameters)
        {
            if (!isInit)
            {
                LogWarning($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameters);
        }
        #endregion


        private void LogWarning(string msg)
        {
            // Debug.LogWarning(msg);
        }
    }

    // QUICK ACCESS
    public static class Analytics
    {
        private static AnalyticsProxy proxy
        {
            get
            {
                return ProxyManager.GetProxy<AnalyticsProxy>();
            }
        }

        public static class UserProperty
        {
            public static void Set(string name, object value)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.SetUserProperty(name, value);
            }
        }

        public static class Event
        {
            public static void Login(string uuid)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.LogEvent(FirebaseAnalytics.EventLogin, "uuid", uuid);
            }

            public static void Upgrade(string name, int level)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.LogEvent(name + "_upgrade", FirebaseAnalytics.ParameterLevel, level);
            }

            public static void GameBegin(int level)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.LogEvent(FirebaseAnalytics.EventLevelStart, "game_level", level);
            }

            public static void GameEnd(int level, bool finish, float progress)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.LogEvent(FirebaseAnalytics.EventLevelEnd,
                    new Parameter("game_level", level),
                    new Parameter("finish", finish ? 1 : 0),
                    new Parameter("progress", progress)
                    );
            }

            public static void Advertising(string adName)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.LogEvent("advertising",
                    new Parameter("name", adName));
            }

            public static void DailySign(int days, float multiple)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.LogEvent("daily_sign",
                    new Parameter("days", days),
                    new Parameter("multiple", multiple)
                    );
            }

            public static void CoinIncomeTake(float quantity)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.LogEvent("coin_income_take",
                    new Parameter(FirebaseAnalytics.ParameterQuantity, quantity.KMB())
                    );
            }

            public static void UnlockVirus(int virusID)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.LogEvent("unlock_virus",
                    new Parameter("virus_id", virusID)
                    );
            }

            public static void Exchange(float diamond, float coin)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.LogEvent("coin_exchange",
                    new Parameter("diamond", diamond.KMB()),
                    new Parameter("coin", coin.KMB())
                    );
            }

            public static void ChangeWeapon(int weaponId)
            {
                if (!proxy.isInit)
                {
                    return;
                }
                proxy.LogEvent("change_weapon",
                    new Parameter("weapon_id", weaponId)
                    );
            }

            private static Dictionary<string, float> s_errorLogTimeDic = new Dictionary<string, float>();
            private static float s_errorLogRepeatedInterval = 3600;
            public static void Log(LogType type, string message, string stackTrack)
            {
                if (proxy == null || !proxy.isInit)
                    return;

                if (type == LogType.Error || type == LogType.Exception)
                {
                    s_errorLogTimeDic.TryGetValue(message, out float _time);
                    if (Time.time - _time < s_errorLogRepeatedInterval)
                        return;
                    s_errorLogTimeDic[message] = Time.time;
                }

                var name = "log_" + type.ToString().ToLower();
                proxy.LogEvent4Log(name, message, stackTrack);
            }
        }
    }


    public static class FirebaseChecker
    {
        public static bool isInited { get; private set; }
        public static bool isSuccess { get; private set; }

        private static UnityEngine.Events.UnityEvent mSuccessCallback = new UnityEngine.Events.UnityEvent();
        private static bool isIniting = false;

        public static void Check(UnityEngine.Events.UnityAction successCallback)
        {
#if UNITY_EDITOR || !UNITY_ANDROID
            return;
#else
            GameManager.Instance.DelayDo(0.5f, () =>
            {
                 if (!isInited)
                 {
                     mSuccessCallback.AddListener(successCallback);

                     if (!isIniting)
                     {
                         isIniting = true;

                         FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                         {
                             isInited = true;
                             var dependencyStatus = task.Result;
                             if (dependencyStatus == DependencyStatus.Available)
                             {
                                 isSuccess = true;
                                 mSuccessCallback.Invoke();
                             }
                             else
                             {
                                 isSuccess = false;
                                 Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                             }
                         });
                     }
                 }
                 else if (isSuccess)
                 {
                     successCallback.Invoke();
                 }
             });
#endif
        }
    }
}
