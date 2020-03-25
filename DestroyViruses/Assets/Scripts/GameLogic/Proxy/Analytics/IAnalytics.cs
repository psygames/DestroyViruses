using System;

namespace DestroyViruses
{
    public interface IAnalytics
    {
        bool IsInit { get; }

        void Init();

        void SetUserProperty(string name, string value);

        void LogEvent(string name, string value);

        void LogEvent4Log(string name, string message, string stackTrack);

        void Login(string uuid);

        void Upgrade(string name, int level);

        void GameStart(int level);

        void GameOver(int level, bool pass, float finishPercent);

        void Advertising(string ad);

        void DailySign(int days, float multiple);

        void CoinIncomeTake(float quantity);

        void UnlockVirus(int virusID);

        void Exchange(float diamond, float coin);

        void ChangeWeapon(int weaponId);

    }
}