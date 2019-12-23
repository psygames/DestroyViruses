using UnityEngine;
using System;

namespace DestroyViruses
{
    public class DataProxy : ProxyBase<DataProxy>
    {
        private GameLocalData localData { get { return GameLocalData.Instance; } }

        public float coin { get { return localData.coin; } }
        public float diamond { get { return localData.diamond; } }
        public int gameLevel { get { return localData.gameLevel; } }
        public bool isGameLevelMax { get { return TableGameLevel.Get(gameLevel + 1) == null; } }
        public int unlockedGameLevel { get { return localData.unlockedGameLevel; } }
        public bool isUnlockedGameLevelMax { get { return TableGameLevel.Get(unlockedGameLevel + 1) == null; } }

        public int firePowerLevel { get { return localData.firePowerLevel; } }
        public int firePowerMaxLevel { get { return TableFirePower.GetAll().Max(a => a.id).id; } }
        public bool isFirePowerLevelMax { get { return firePowerLevel >= firePowerMaxLevel; } }
        public float firePowerUpCost { get { return FormulaUtil.FirePowerUpCost(firePowerLevel); } }
        public float firePower { get { return FormulaUtil.FirePower(firePowerLevel); } }

        public int fireSpeedLevel { get { return localData.fireSpeedLevel; } }
        public int fireSpeedMaxLevel { get { return TableFireSpeed.GetAll().Max(a => a.id).id; } }
        public bool isFireSpeedLevelMax { get { return fireSpeedLevel >= fireSpeedMaxLevel; } }
        public float fireSpeedUpCost { get { return FormulaUtil.FireSpeedUpCost(fireSpeedLevel); } }
        public float fireSpeed { get { return FormulaUtil.FireSpeed(fireSpeedLevel); } }

        public int streak { get { return Mathf.Clamp(localData.streak, -6, 6); } }
        public int signDays { get { return localData.signDays; } }
        public int[] unlockedViruses { get { return localData.unlockedViruses; } }

        public int coinValueLevel { get { return localData.coinValueLevel; } }
        public float coinValue { get { return TableCoinValue.Get(coinValueLevel).value; } }
        public float coinValueUpCost { get { return TableCoinValue.Get(coinValueLevel).upcost; } }
        public int coinValueMaxLevel { get { return TableCoinValue.GetAll().Max(a => a.id).id; } }
        public bool isCoinValueLevelMax { get { return coinValueLevel >= coinValueMaxLevel; } }

        public int coinIncomeLevel { get { return localData.coinIncomeLevel; } }
        public float coinIncome { get { return TableCoinIncome.Get(coinIncomeLevel).income; } }
        public float coinIncomeUpCost { get { return TableCoinIncome.Get(coinIncomeLevel).upcost; } }
        public int coinIncomeMaxLevel { get { return TableCoinIncome.GetAll().Max(a => a.id).id; } }
        public bool isCoinIncomeLevelMax { get { return coinIncomeLevel >= coinIncomeMaxLevel; } }
        public bool isCoinIncomePoolFull
        {
            get
            {
                var span = DateTime.Now - new DateTime(localData.lastTakeIncomeTicks);
                return span.TotalSeconds >= ConstTable.table.coinIncomeMaxDuration;
            }
        }

        public float coinIncomeTotal
        {
            get
            {
                if (localData.lastTakeIncomeTicks == 0)
                {
                    localData.lastTakeIncomeTicks = DateTime.Now.Ticks;
                    SaveLocalData();
                }
                var span = DateTime.Now - new DateTime(localData.lastTakeIncomeTicks);
                var secMax = Mathf.Max(ConstTable.table.coinIncomeMaxDuration, (float)span.TotalSeconds);
                return secMax * coinIncome;
            }
        }

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
            Analytics.UserProperty.Set("coin", coin.KMB());
            Analytics.UserProperty.Set("diamond", diamond.KMB());
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
        }

        private void AddCoin(float count)
        {
            localData.coin += count;
        }

        private void AddDiamond(float count)
        {
            localData.diamond += count;
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
        }

        public void DailySign()
        {
            if (!CanDailySign())
            {
                DispatchEvent(EventGameData.Action.Error, "Can't Sign Now.");
                return;
            }

            var days = localData.signDays;
            localData.signDays = (days % 7) + 1;
            localData.lastSignDateTicks = DateTime.Now.Date.Ticks;
            var t = TableDailySign.Get(days);
            if (t.type == 1) AddDiamond(t.count);
            else if (t.type == 2) AddCoin(FormulaUtil.DailySignCoinFix(t.count, D.I.coinValue));
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.DailySign(days);
        }

        public bool CanDailySign()
        {
            var last = new DateTime(localData.lastSignDateTicks);
            return (DateTime.Now.Date - last).TotalDays >= 1;
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

        public void TakeIncomeCoins()
        {
            var gain = coinIncomeTotal;
            localData.lastTakeIncomeTicks = DateTime.Now.Ticks;
            AddCoin(gain);
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.CoinIncomeTake(gain.KMB());
        }

        public void GameEndReceive(float multiple)
        {
            AddCoin(D.I.battleGetCoin);
        }

        public void CoinIncomeLevelUp()
        {
            if (isCoinIncomeLevelMax)
            {
                DispatchEvent(EventGameData.Action.Error, "已经升至满级");
                return;
            }

            var cost = coinIncomeUpCost;
            if (coin < cost)
            {
                DispatchEvent(EventGameData.Action.Error, "升级所需金币不足");
                return;
            }

            localData.coin -= cost;
            localData.coinIncomeLevel += 1;
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.Upgrade("coin_income", coinIncomeLevel);
        }

        public void CoinValueLevelUp()
        {
            if (isCoinValueLevelMax)
            {
                DispatchEvent(EventGameData.Action.Error, "已经升至满级");
                return;
            }

            var cost = coinValueUpCost;
            if (coin < cost)
            {
                DispatchEvent(EventGameData.Action.Error, "升级所需金币不足");
                return;
            }

            localData.coin -= cost;
            localData.coinValueLevel += 1;
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.Upgrade("coin_value", coinIncomeLevel);
        }

        public void ExchangeCoin(float diamond)
        {
            if (diamond > this.diamond)
            {
                DispatchEvent(EventGameData.Action.Error, "钻石不足");
                return;
            }

            localData.diamond -= diamond;
            var addCoin = FormulaUtil.CoinExchangeFix(diamond, coinValue);
            localData.coin += addCoin;


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