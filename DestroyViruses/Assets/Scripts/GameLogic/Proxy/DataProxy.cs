using UnityEngine;
using System;

namespace DestroyViruses
{
    public class DataProxy : ProxyBase<DataProxy>
    {
        private GameLocalData localData { get { return GameLocalData.Instance; } }

        public long coin { get { return localData.coin; } }
        public long diamond { get { return localData.diamond; } }
        public int gameLevel { get { return localData.gameLevel; } }
        public bool isGameLevelMax { get { return TableGameLevel.Get(gameLevel + 1) == null; } }
        public int unlockedGameLevel { get { return localData.unlockedGameLevel; } }
        public bool isUnlockedGameLevelMax { get { return TableGameLevel.Get(unlockedGameLevel + 1) == null; } }

        public int firePowerLevel { get { return localData.firePowerLevel; } }
        public int firePowerMaxLevel { get { return TableFirePower.GetAll().Max(a => a.id).id; } }
        public bool isFirePowerLevelMax { get { return firePowerLevel >= firePowerMaxLevel; } }

        public int fireSpeedLevel { get { return localData.fireSpeedLevel; } }
        public int fireSpeedMaxLevel { get { return TableFireSpeed.GetAll().Max(a => a.id).id; } }
        public bool isFireSpeedLevelMax { get { return fireSpeedLevel >= fireSpeedMaxLevel; } }

        public int streak { get { return Mathf.Clamp(localData.streak, -6, 6); } }
        public int signDays { get { return localData.signDays; } }
        public int[] unlockedViruses { get { return localData.unlockedViruses; } }

        // 计算
        public long firePowerUpCost { get { return (long)FormulaUtil.FirePowerUpCost(firePowerLevel); } }
        public long fireSpeedUpCost { get { return (long)FormulaUtil.FireSpeedUpCost(fireSpeedLevel); } }
        public float firePower { get { return FormulaUtil.FirePower(firePowerLevel); } }
        public float fireSpeed { get { return FormulaUtil.FireSpeed(fireSpeedLevel); } }

        // 临时数据（外部可修改）
        public bool gameEndWin { get; set; }
        public bool adRevive { get; set; }
        public int reviveCount { get; set; }
        public int kills4Buff { get; set; }

        // 战斗数据
        public float battleGetCoin
        {
            get
            {
                if (GameModeManager.Instance.currentMode.GetType() == typeof(LevelMode))
                {
                    return (GameModeManager.Instance.currentMode as LevelMode).getCoin;
                }
                return 0;
            }
        }

        // 战斗进度
        public float battleProgress
        {
            get
            {
                if (GameModeManager.Instance.currentMode.GetType() == typeof(LevelMode))
                {
                    return (GameModeManager.Instance.currentMode as LevelMode).progress;
                }
                return 0;
            }
        }

        public void AnalyticsSetUserProperty()
        {
            Analytics.UserProperty.Set("coin", coin);
            Analytics.UserProperty.Set("diamond", diamond);
            Analytics.UserProperty.Set("game_level", gameLevel);
            Analytics.UserProperty.Set("unlocked_game_level", unlockedGameLevel);
            Analytics.UserProperty.Level("fire_power", firePowerLevel);
            Analytics.UserProperty.Level("fire_speed", fireSpeedLevel);
            Analytics.UserProperty.Set("streak", streak);
            Analytics.UserProperty.Set("daily_sign", signDays);
        }

        public void FirePowerUp()
        {
            if (isFirePowerLevelMax)
            {
                DispatchEvent(EventGameData.Action.Error, "已经升至满级");
                return;
            }

            var cost = firePowerUpCost;
            if (coin < cost)
            {
                DispatchEvent(EventGameData.Action.Error, "升级所需金币不足");
                return;
            }

            localData.coin -= cost;
            localData.firePowerLevel += 1;
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.Upgrade("fire_power", firePowerLevel);
            Analytics.UserProperty.Level("fire_power", firePowerLevel);
            Analytics.Event.Cost("coin", cost);
            Analytics.UserProperty.Set("coin", coin);
        }

        public void FireSpeedUp()
        {
            if (isFireSpeedLevelMax)
            {
                DispatchEvent(EventGameData.Action.Error, "已经升至满级");
                return;
            }

            var cost = fireSpeedUpCost;
            if (coin < cost)
            {
                DispatchEvent(EventGameData.Action.Error, "升级所需金币不足");
                return;
            }

            localData.coin -= cost;
            localData.fireSpeedLevel += 1;
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.Upgrade("fire_speed", fireSpeedLevel);
            Analytics.UserProperty.Level("fire_speed", fireSpeedLevel);
            Analytics.Event.Cost("coin", cost);
            Analytics.UserProperty.Set("coin", coin);
        }

        public void AddCoin(int count)
        {
            localData.coin += count;
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.Gain("coin", count);
            Analytics.UserProperty.Set("coin", coin);
        }

        public void AddDiamond(int count)
        {
            localData.diamond += count;
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.Gain("diamond", count);
            Analytics.UserProperty.Set("diamond", diamond);
        }

        public void BattleEnd(bool isWin)
        {
            gameEndWin = isWin;
            // 解锁新关卡
            if (isWin && gameLevel >= unlockedGameLevel)
            {
                UnlockNewLevel();
                SelectGameLevel(unlockedGameLevel);
            }
            if (isWin && streak >= 0)
            {
                localData.streak += 1;
            }
            else if (!isWin && streak <= 0)
            {
                localData.streak -= 1;
            }
            else
            {
                localData.streak = 0;
            }
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.UserProperty.Set("streak", streak);
        }

        public void UnlockNewLevel()
        {
            if (isUnlockedGameLevelMax)
            {
                return;
            }
            localData.unlockedGameLevel += 1;
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.UserProperty.Set("unlocked_game_level", unlockedGameLevel);
        }

        public void SelectGameLevel(int level)
        {
            if (level <= 0)
            {
                DispatchEvent(EventGameData.Action.Error, "不是有效关卡");
                return;
            }

            if (level > localData.unlockedGameLevel)
            {
                DispatchEvent(EventGameData.Action.Error, "关卡尚未解锁");
                return;
            }

            localData.gameLevel = level;
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.UserProperty.Set("game_level", level);
        }

        public void DailySign()
        {
            if (!CanDailySign())
            {
                DispatchEvent(EventGameData.Action.Error, "Can't Sign Now.");
                return;
            }

            var days = localData.signDays;
            localData.signDays = days + 1;
            localData.lastSignDateTicks = DateTime.Now.Date.Ticks;
            var t = TableDailySign.Get(days);
            if (t.type == 1) AddDiamond(t.count);
            else if (t.type == 2) AddCoin(t.count);
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.DailySign(days);
            Analytics.UserProperty.Set("daily_sign", days);
        }

        public bool CanDailySign()
        {
            var last = new DateTime(localData.lastSignDateTicks);
            return (DateTime.Now.Date - last).Days >= 1;
        }

        public void UnlockVirus(int virusID)
        {
            if (unlockedViruses.Any(a => a == virusID))
            {
                DispatchEvent(EventGameData.Action.Error, "Already Unlocked Virus " + virusID);
                return;
            }

            int[] _virus = new int[unlockedViruses.Length + 1];
            Array.Copy(unlockedViruses, _virus, unlockedViruses.Length);
            _virus[_virus.Length - 1] = virusID;
            localData.unlockedViruses = _virus;
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.UnlockVirus(virusID);
        }

        private void SaveLocalData()
        {
            localData.Save();
        }

        private void DispatchEvent(EventGameData.Action action, string errorMsg = "")
        {
            UnibusEvent.Unibus.Dispatch(EventGameData.Get(action, errorMsg));
        }
    }

    public static class D
    {
        public static DataProxy I { get { return DataProxy.Ins; } }
    };
}