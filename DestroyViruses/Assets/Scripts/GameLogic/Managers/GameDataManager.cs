using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class GameDataManager : Singleton<GameDataManager>
    {
        private GameLocalData localData { get { return GameLocalData.Instance; } }

        public int coin { get { return localData.coin; } }
        public int diamond { get { return localData.diamond; } }
        public int gameLevel { get { return localData.gameLevel; } }
        public int unlockedGameLevel { get { return localData.unlockedGameLevel; } }
        public int firePowerLevel { get { return localData.firePowerLevel; } }
        public int fireSpeedLevel { get { return localData.fireSpeedLevel; } }

        // 计算
        public float firePowerUpCost { get { return FormulaUtil.FirePowerUpCost(firePowerLevel); } }
        public float fireSpeedUpCost { get { return FormulaUtil.FireSpeedUpCost(fireSpeedLevel); } }

        // 临时数据（外部可修改）
        public bool newLevelUnlocked { get; set; }

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


        public void FirePowerUp()
        {
            if (coin < (int)firePowerUpCost)
            {
                DispatchEvent(EventGameData.Action.Error, "升级所需金币不足");
                return;
            }

            localData.coin -= (int)firePowerUpCost;
            localData.firePowerLevel += 1;
            DispatchEvent(EventGameData.Action.DataChange);
            SaveLocalData();
        }

        public void FireSpeedUp()
        {
            if (coin < (int)fireSpeedUpCost)
            {
                DispatchEvent(EventGameData.Action.Error, "升级所需金币不足");
                return;
            }

            localData.coin -= (int)fireSpeedUpCost;
            localData.fireSpeedLevel += 1;
            DispatchEvent(EventGameData.Action.DataChange);
            SaveLocalData();
        }

        public void AddCoin(int count)
        {
            localData.coin += count;
            DispatchEvent(EventGameData.Action.DataChange);
        }

        public void UnlockNewLevel()
        {
            localData.unlockedGameLevel += 1;
            DispatchEvent(EventGameData.Action.DataChange);
            newLevelUnlocked = true;
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