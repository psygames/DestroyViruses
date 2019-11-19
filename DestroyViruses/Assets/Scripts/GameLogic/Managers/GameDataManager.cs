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
        public bool isGameLevelMax { get { return TableGameLevel.Get(gameLevel + 1) == null; } }
        public int unlockedGameLevel { get { return localData.unlockedGameLevel; } }
        public bool isUnlockedGameLevelMax { get { return TableGameLevel.Get(unlockedGameLevel + 1) == null; } }
        public int firePowerLevel { get { return localData.firePowerLevel; } }
        public bool isFirePowerLevelMax { get { return TableFirePower.Get(firePowerLevel + 1) == null; } }
        public int fireSpeedLevel { get { return localData.fireSpeedLevel; } }
        public bool isFireSpeedLevelMax { get { return TableFireSpeed.Get(fireSpeedLevel + 1) == null; } }

        // 计算
        public int firePowerUpCost { get { return (int)FormulaUtil.FirePowerUpCost(firePowerLevel); } }
        public int fireSpeedUpCost { get { return (int)FormulaUtil.FireSpeedUpCost(fireSpeedLevel); } }

        // 临时数据（外部可修改）
        public bool gameEndWin { get; set; }

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
            if (isFirePowerLevelMax)
            {
                DispatchEvent(EventGameData.Action.Error, "已经升至满级");
                return;
            }
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
            if (isFireSpeedLevelMax)
            {
                DispatchEvent(EventGameData.Action.Error, "已经升至满级");
                return;
            }
            if (coin < fireSpeedUpCost)
            {
                DispatchEvent(EventGameData.Action.Error, "升级所需金币不足");
                return;
            }

            localData.coin -= fireSpeedUpCost;
            localData.fireSpeedLevel += 1;
            DispatchEvent(EventGameData.Action.DataChange);
            SaveLocalData();
        }

        public void AddCoin(int count)
        {
            localData.coin += count;
            DispatchEvent(EventGameData.Action.DataChange);
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
        }

        public void UnlockNewLevel()
        {
            if (isUnlockedGameLevelMax)
            {
                return;
            }
            localData.unlockedGameLevel += 1;
            DispatchEvent(EventGameData.Action.DataChange);
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
