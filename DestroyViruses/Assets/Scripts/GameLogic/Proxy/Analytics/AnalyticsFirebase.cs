using System;
#if USE_FIREBASE
using Firebase.Analytics;

namespace DestroyViruses
{
    public class AnalyticsFirebase : IAnalytics
    {
        public bool IsInit { get; private set; }

        public void Init()
        {
#if UNITY_EDITOR
            return;
#endif
            GameManager.Instance.DelayDo(1.5f, () =>
            {
                FirebaseChecker.Check(InitializeFirebase);
            });
        }

        void InitializeFirebase()
        {
            UnityEngine.Debug.Log("AnalyticsFirebase Inited");
            IsInit = true;
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            FirebaseAnalytics.SetUserId(DeviceID.UUID);
            FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        }

        public void SetUserProperty(string name, string value)
        {
            FirebaseAnalytics.SetUserProperty(name, value);
        }

        public void LogEvent(string name, string value)
        {
            FirebaseAnalytics.LogEvent(name, "value", value);
        }

        public void LogEvent4Log(string name, string message, string stackTrack)
        {
            FirebaseAnalytics.LogEvent(name,
                new Parameter("message", message),
                new Parameter("stack_track", stackTrack));
        }

        public void Advertising(string ad)
        {
            FirebaseAnalytics.LogEvent("advertising",
                new Parameter("name", ad));
        }

        public void ChangeWeapon(int weaponId)
        {
            FirebaseAnalytics.LogEvent("change_weapon",
                new Parameter("weapon_id", weaponId)
                );
        }

        public void CoinIncomeTake(float quantity)
        {
            FirebaseAnalytics.LogEvent("coin_income_take",
                new Parameter(FirebaseAnalytics.ParameterQuantity, quantity.KMB())
                );
        }

        public void DailySign(int days, float multiple)
        {
            FirebaseAnalytics.LogEvent("daily_sign",
                new Parameter("days", days),
                new Parameter("multiple", multiple));
        }

        public void Exchange(float diamond, float coin)
        {
            FirebaseAnalytics.LogEvent("coin_exchange",
                new Parameter("diamond", diamond.KMB()),
                new Parameter("coin", coin.KMB()));
        }

        public void GameOver(int level, bool pass, float finishPercent)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd,
                new Parameter("game_level", level),
                new Parameter("finish", pass ? 1 : 0),
                new Parameter("progress", finishPercent));
        }

        public void GameStart(int level)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, "game_level", level);
        }

        public void Login(string uuid)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin, "uuid", uuid);
        }

        public void UnlockVirus(int virusID)
        {
            FirebaseAnalytics.LogEvent("unlock_virus",
                new Parameter("virus_id", virusID));
        }

        public void Upgrade(string name, int level)
        {
            FirebaseAnalytics.LogEvent(name + "_upgrade", FirebaseAnalytics.ParameterLevel, level);
        }
    }
}
#endif