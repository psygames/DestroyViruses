using UnityEngine;
using System.Collections.Generic;
using System;

namespace DestroyViruses
{
    public class AnalyticsProxy : ProxyBase<AnalyticsProxy>
    {
        private List<IAnalytics> mAnalyticses = new List<IAnalytics>();
        public bool IsInit => mAnalyticses.All(a => a.IsInit);

        protected override void OnInit()
        {
            base.OnInit();
            // mAnalyticses.Add(new AnalyticsFirebase());
            // mAnalyticses.Add(new AnalyticsFacebook());

            foreach (var ana in mAnalyticses)
            {
                ana.Init();
            }
        }


        public void SetUserProperty(string name, string value)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.SetUserProperty(name, value);
            }
        }


        public void LogEvent(string name, string value)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.LogEvent(name, value);
            }
        }


        public void LogEvent4Log(string name, string message, string stackTrack)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.LogEvent4Log(name, message, stackTrack);
            }
        }

        public void Advertising(string ad)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.Advertising(ad);
            }
        }

        public void ChangeWeapon(int weaponId)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.ChangeWeapon(weaponId);
            }
        }

        public void CoinIncomeTake(float quantity)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.CoinIncomeTake(quantity);
            }
        }

        public void DailySign(int days, float multiple)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.DailySign(days, multiple);
            }
        }

        public void Exchange(float diamond, float coin)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.Exchange(diamond, coin);
            }
        }

        public void GameOver(int level, bool pass, float finishPercent)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.GameOver(level, pass, finishPercent);
            }
        }

        public void GameStart(int level)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.GameStart(level);
            }
        }

        public void Login(string uuid)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.Login(uuid);
            }
        }

        public void UnlockVirus(int virusID)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.UnlockVirus(virusID);
            }
        }

        public void Upgrade(string name, int level)
        {
            foreach (var ana in mAnalyticses)
            {
                if (!ana.IsInit)
                    continue;
                ana.Upgrade(name, level);
            }
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
            public static void Set(string name, string value)
            {
                proxy.SetUserProperty(name, value);
            }
        }

        public static class Event
        {
            public static void Login(string uuid)
            {
                proxy.Login(uuid);
            }

            public static void Upgrade(string name, int level)
            {
                proxy.Upgrade(name, level);
            }

            public static void GameStart(int level)
            {
                proxy.GameStart(level);
            }

            public static void GameOver(int level, bool pass, float finishPercent)
            {
                proxy.GameOver(level, pass, finishPercent);
            }

            public static void Advertising(string name)
            {
                proxy.Advertising(name);
            }

            public static void DailySign(int days, float multiple)
            {
                proxy.DailySign(days, multiple);
            }

            public static void CoinIncomeTake(float quantity)
            {
                proxy.CoinIncomeTake(quantity);
            }

            public static void UnlockVirus(int virusID)
            {
                proxy.UnlockVirus(virusID);
            }

            public static void Exchange(float diamond, float coin)
            {
                proxy.Exchange(diamond, coin);
            }

            public static void ChangeWeapon(int weaponId)
            {
                proxy.ChangeWeapon(weaponId);
            }

            private static Dictionary<string, float> s_errorLogTimeDic = new Dictionary<string, float>();
            private static float s_errorLogRepeatedInterval = 3600;
            public static void Log(LogType type, string message, string stackTrack)
            {
                if (proxy == null)
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
}
