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
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    Debug.Log("Analytics 初始化完成");
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            });
        }

        void InitializeFirebase()
        {
            isInit = true;
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            FirebaseAnalytics.SetUserId(DeviceID.UUID);
            FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        }

        public void ResetAnalyticsData()
        {
            FirebaseAnalytics.ResetAnalyticsData();
        }

        #region UserProperty

        public void SetUserProperty(string name, string property)
        {
            if (!isInit)
            {
                LogError($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.SetUserProperty(name, property);
        }

        public void SetUserProperty(string name, object obj)
        {
            if (!isInit)
            {
                LogError($"LogEvent [{name}] failed cause sdk not initialized.");
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
                LogError($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name);
        }

        public void LogEvent(string name, string parameterName, int parameterValue)
        {
            if (!isInit)
            {
                LogError($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public void LogEvent(string name, string parameterName, long parameterValue)
        {
            if (!isInit)
            {
                LogError($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public void LogEvent(string name, string parameterName, double parameterValue)
        {
            if (!isInit)
            {
                LogError($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public void LogEvent(string name, string parameterName, string parameterValue)
        {
            if (!isInit)
            {
                LogError($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public void LogEvent4Log(string name, string message)
        {
            if (!isInit)
                return;
            FirebaseAnalytics.LogEvent(name, "message", message);
        }

        public void LogEvent(string name, params Parameter[] parameters)
        {
            if (!isInit)
            {
                LogError($"LogEvent [{name}] failed cause sdk not initialized.");
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameters);
        }
        #endregion


        private void LogError(string msg)
        {
            Debug.LogError(msg);
        }
    }

    // QUICK ACCESS
    public static class Analytics
    {
        private static AnalyticsProxy proxy
        {
            get
            {
                var p = ProxyManager.GetProxy<AnalyticsProxy>();
                if (p == null)
                    Debug.LogError("Error: AnalyticsProxy is null.");
                return p;
            }
        }

        public static class UserProperty
        {
            public static void Set(string name, object value)
            {
                proxy.SetUserProperty(name, value);
            }

            public static void Level(string name, int level)
            {
                proxy.SetUserProperty(name + "_level", level);
            }
        }

        public static class Event
        {
            public static void AppOpen()
            {
                proxy.LogEvent(FirebaseAnalytics.EventAppOpen);
            }

            public static void Login(string uuid)
            {
                proxy.LogEvent(FirebaseAnalytics.EventLogin, "uuid", uuid);
            }

            public static void Upgrade(string name, int level)
            {
                proxy.LogEvent(name + "_upgrade", FirebaseAnalytics.ParameterLevel, level);
            }

            public static void GameBegin(int level)
            {
                proxy.LogEvent(FirebaseAnalytics.EventLevelStart, FirebaseAnalytics.ParameterLevel, level);
            }

            public static void GameEnd(int level, bool finish, float progress)
            {
                proxy.LogEvent(FirebaseAnalytics.EventLevelEnd,
                    new Parameter(FirebaseAnalytics.ParameterLevel, level),
                    new Parameter("finish", finish ? 1 : 0),
                    new Parameter("progress", progress)
                    );
            }

            public static void Gain(string name, long quantity)
            {
                proxy.LogEvent("gain",
                    new Parameter("name", name),
                    new Parameter(FirebaseAnalytics.ParameterQuantity, quantity)
                    );
            }

            public static void Cost(string name, long quantity)
            {
                proxy.LogEvent("cost",
                    new Parameter("name", name),
                    new Parameter(FirebaseAnalytics.ParameterQuantity, quantity)
                    );
            }

            public static void ClickAd(string adName, string adUnit)
            {
                proxy.LogEvent("advertising",
                    new Parameter("name", adName),
                    new Parameter("unitID", adUnit)
                    );
            }


            private static Dictionary<string, float> s_errorLogTimeDic = new Dictionary<string, float>();
            private static float s_errorLogRepeatedInterval = 10;
            public static void Log(LogType type, string errorMsg)
            {
                if (type == LogType.Error || type == LogType.Exception)
                {
                    s_errorLogTimeDic.TryGetValue(errorMsg, out float _time);
                    if (Time.time - _time < s_errorLogRepeatedInterval)
                        return;
                    s_errorLogTimeDic[errorMsg] = Time.time;
                }

                var name = "log-" + type.ToString().ToLower();
                proxy.LogEvent4Log(name, errorMsg);
            }
        }
    }

}
