using System;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

namespace DestroyViruses
{
    public class AnalyticsFacebook : IAnalytics
    {
        public bool IsInit { get; private set; }

        public void Init()
        {
#if UNITY_EDITOR
            return;
#endif
            GameManager.Instance.DelayDo(1, () =>
            {
                if (!FB.IsInitialized)
                {
                    FB.Init(InitCallback, OnHideUnity);
                }
                else
                {
                    ActivateApp();
                }
            });
        }

        private void ActivateApp()
        {
            FB.ActivateApp();
            FB.Mobile.UserID = DeviceID.UUID;
            IsInit = true;
        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                UnityEngine.Debug.Log("AnalyticsFacebook Inited");
                ActivateApp();
            }
            else
            {
                Debug.LogError("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            //TODO: GAME SHOWN
            //Debug.Log("hiden unity");
        }

        public void SetUserProperty(string name, string value)
        {
            FB.Mobile.UpdateUserProperties(new Dictionary<string, string>() { { name, value } });
        }


        public void LogEvent(string name, string value)
        {
            FB.LogAppEvent(name, null, new Dictionary<string, object>() { { "value", value } });
        }


        public void LogEvent4Log(string name, string message, string stackTrack)
        {
            FB.LogAppEvent(name, null, new Dictionary<string, object>() { { "message", message }, { "stack_track", stackTrack } });
        }

        public void Advertising(string ad)
        {
            FB.LogAppEvent("advertising", null, new Dictionary<string, object>() { { "name", ad } });
        }

        public void ChangeWeapon(int weaponId)
        {
            FB.LogAppEvent("change_weapon", null, new Dictionary<string, object>() { { "weapon_id", weaponId } });
        }

        public void CoinIncomeTake(float quantity)
        {
            FB.LogAppEvent("coin_income_take", null, new Dictionary<string, object>() { { "quantity", quantity.KMB() } });
        }

        public void DailySign(int days, float multiple)
        {
            FB.LogAppEvent("daily_sign", null, new Dictionary<string, object>() { { "days", days }, { "multiple", multiple } });
        }

        public void Exchange(float diamond, float coin)
        {
            FB.LogAppEvent("coin_exchange", null, new Dictionary<string, object>() { { "diamond", diamond.KMB() }, { "coin", coin.KMB() } });
        }

        public void GameOver(int level, bool pass, float finishPercent)
        {
            FB.LogAppEvent("level_end", null, new Dictionary<string, object>() { { "game_level", level }, { "finish", pass ? 1 : 0 }, { "progress", finishPercent } });
        }

        public void GameStart(int level)
        {
            FB.LogAppEvent("level_start", null, new Dictionary<string, object>() { { "game_level", level } });
        }

        public void Login(string uuid)
        {
            FB.LogAppEvent("login", null, new Dictionary<string, object>() { { "uuid", uuid } });
        }

        public void UnlockVirus(int virusID)
        {
            FB.LogAppEvent("unlock_virus", null, new Dictionary<string, object>() { { "virus_id", virusID } });
        }

        public void Upgrade(string name, int level)
        {
            FB.LogAppEvent(name + "_upgrade", null, new Dictionary<string, object>() { { AppEventParameterName.Level, level } });
        }
    }
}