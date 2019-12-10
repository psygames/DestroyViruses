using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class GameDataManager : Singleton<GameDataManager>
    {
        private GameLocalData localData { get { return GameLocalData.Instance; } }

        public long coin { get { return localData.coin; } }
        public long diamond { get { return localData.diamond; } }
        public int gameLevel { get { return localData.gameLevel; } }
        public bool isGameLevelMax { get { return TableGameLevel.Get(gameLevel + 1) == null; } }
        public int unlockedGameLevel { get { return localData.unlockedGameLevel; } }
        public bool isUnlockedGameLevelMax { get { return TableGameLevel.Get(unlockedGameLevel + 1) == null; } }
        public int firePowerLevel { get { return localData.firePowerLevel; } }
        public bool isFirePowerLevelMax { get { return TableFirePower.Get(firePowerLevel + 1) == null; } }
        public int fireSpeedLevel { get { return localData.fireSpeedLevel; } }
        public bool isFireSpeedLevelMax { get { return TableFireSpeed.Get(fireSpeedLevel + 1) == null; } }
        public int streak { get { return Mathf.Clamp(localData.streak, -6, 6); } }

        // 计算
        public long firePowerUpCost { get { return (long)FormulaUtil.FirePowerUpCost(firePowerLevel); } }
        public long fireSpeedUpCost { get { return (long)FormulaUtil.FireSpeedUpCost(fireSpeedLevel); } }
        public float firePower { get { return FormulaUtil.FirePower(firePowerLevel); } }
        public float fireSpeed { get { return FormulaUtil.FireSpeed(fireSpeedLevel); } }

        // 临时数据（外部可修改）
        public bool gameEndWin { get; set; }
        public bool adRevive { get; set; }

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
            DispatchEvent(EventGameData.Action.DataChange);
            SaveLocalData();

            Analytics.Event.Upgrade("fire_power", GDM.ins.firePowerLevel);
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
            DispatchEvent(EventGameData.Action.DataChange);
            SaveLocalData();

            Analytics.Event.Upgrade("fire_speed", GDM.ins.fireSpeedLevel);
            Analytics.UserProperty.Level("fire_speed", fireSpeedLevel);
            Analytics.Event.Cost("coin", cost);
            Analytics.UserProperty.Set("coin", coin);
        }

        public void AddCoin(int count)
        {
            localData.coin += count;
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.Gain("coin", count);
            Analytics.UserProperty.Set("coin", coin);
        }

        public void BattleEnd(bool isWin)
        {
            gameEndWin = isWin;
            // 解锁新关卡
            if (isWin && GDM.ins.gameLevel >= GDM.ins.unlockedGameLevel)
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
        }

        public void UnlockNewLevel()
        {
            if (isUnlockedGameLevelMax)
            {
                return;
            }
            localData.unlockedGameLevel += 1;
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
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.UserProperty.Set("game_level", level);
        }

        private void DispatchEvent(EventGameData.Action action, string errorMsg = "")
        {
            UnibusEvent.Unibus.Dispatch(EventGameData.Get(action, errorMsg));
        }

        public void SaveLocalData()
        {
            localData.Save();
        }

        private void OnApplicationQuit()
        {
            SaveLocalData();
        }
    }

    public static class GDM
    {
        public static GameDataManager ins { get { return GameDataManager.Instance; } }
    };
}
