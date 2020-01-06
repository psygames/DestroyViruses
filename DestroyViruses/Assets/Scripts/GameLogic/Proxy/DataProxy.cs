using UnityEngine;
using System;
using System.Data;

namespace DestroyViruses
{
    public class DataProxy : ProxyBase<DataProxy>
    {
        private GameLocalData localData { get { return GameLocalData.Instance; } }
        private BookData bookData { get { return BookData.Instance; } }
        private WeaponLevelData weaponLevelData { get { return WeaponLevelData.Instance; } }

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
        public int weaponId { get { return localData.weaponId; } }

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
                var secMax = Mathf.Min(ConstTable.table.coinIncomeMaxDuration, (float)span.TotalSeconds);
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
            Analytics.UserProperty.Set("fire_power_level", firePowerLevel);
            Analytics.UserProperty.Set("fire_speed_level", fireSpeedLevel);
            Analytics.UserProperty.Set("streak", streak);
            Analytics.UserProperty.Set("daily_sign", signDays);
            Analytics.UserProperty.Set("weapon_id", weaponId);
        }

        public void FirePowerUp()
        {
            if (isFirePowerLevelMax)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.ALREADY_LEVEL_MAX.LT());
                return;
            }

            var cost = firePowerUpCost;
            if (coin < cost)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.UPGRADE_LACK_OF_COIN.LT());
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
                DispatchEvent(EventGameData.Action.Error, LTKey.ALREADY_LEVEL_MAX.LT());
                return;
            }

            var cost = fireSpeedUpCost;
            if (coin < cost)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.UPGRADE_LACK_OF_COIN.LT());
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
            SaveBookData();
            DispatchEvent(EventGameData.Action.DataChange);
        }

        public void UnlockNewLevel()
        {
            if (isUnlockedGameLevelMax)
            {
                return;
            }
            localData.unlockedGameLevel += 1;
            // auto equip
            if (weaponId <= 0 && ConstTable.table.weaponUnlockLevel <= unlockedGameLevel)
            {
                localData.weaponId = 1;
            }

            SaveLocalData();
            DispatchEvent(EventGameData.Action.UnlockNewLevel);
            DispatchEvent(EventGameData.Action.DataChange);
        }

        public void SelectGameLevel(int level)
        {
            if (level <= 0)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.GAME_LEVEL_INVALID.LT());
                return;
            }

            if (level > localData.unlockedGameLevel)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.GAME_LEVEL_LOCKED.LT());
                return;
            }

            localData.gameLevel = level;
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);
        }

        public void DailySign(float multiple)
        {
            if (!CanDailySign())
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.DAILY_SIGN_CANT_SIGN);
                return;
            }

            var days = localData.signDays;
            localData.signDays = (days % 7) + 1;
            localData.lastSignDateTicks = DateTime.Now.Date.Ticks;
            var t = TableDailySign.Get(days);
            if (t.type == 1) AddDiamond(t.count);
            else if (t.type == 2) AddCoin(t.count * multiple * FormulaUtil.Expresso(ConstTable.table.formulaArgsDailySignCoin));
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.DailySign(days, multiple);
        }

        public bool CanDailySign()
        {
            var last = new DateTime(localData.lastSignDateTicks);
            return IsDailySignUnlocked() && (DateTime.Now.Date - last).TotalDays >= 1;
        }

        public bool IsDailySignUnlocked()
        {
            return gameLevel >= ConstTable.table.dailySignUnlockLevel;
        }

        public void TakeIncomeCoins()
        {
            var gain = coinIncomeTotal;
            localData.lastTakeIncomeTicks = DateTime.Now.Ticks;
            AddCoin(gain);
            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.CoinIncomeTake(gain);
        }

        public void GameEndReceive(float multiple)
        {
            AddCoin(D.I.battleGetCoin);
        }

        public void CoinIncomeLevelUp()
        {
            if (isCoinIncomeLevelMax)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.ALREADY_LEVEL_MAX.LT());
                return;
            }

            var cost = coinIncomeUpCost;
            if (coin < cost)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.UPGRADE_LACK_OF_COIN.LT());
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
                DispatchEvent(EventGameData.Action.Error, LTKey.ALREADY_LEVEL_MAX.LT());
                return;
            }

            var cost = coinValueUpCost;
            if (coin < cost)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.UPGRADE_LACK_OF_COIN.LT());
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
                DispatchEvent(EventGameData.Action.Error, LTKey.UPGRADE_LACK_OF_DIAMOND.LT());
                return;
            }

            localData.diamond -= diamond;
            var addCoin = diamond * FormulaUtil.Expresso(ConstTable.table.formulaArgsCoinExchange);
            localData.coin += addCoin;

            SaveLocalData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.Exchange(diamond, addCoin);
        }

        #region Virus Book
        public void BookAddCollectCount(int virusID)
        {
            if (!bookData.Exist(virusID))
            {
                bookData.Set(virusID, 0);
            }

            if (bookData.Get(virusID) < ConstTable.table.bookVirusCollectKillCount)
            {
                bookData.Add(virusID, 1);
            }
        }

        public int BookGetCollectCount(int virusID)
        {
            return bookData.Get(virusID);
        }

        public bool BookIsUnlock(int virusID)
        {
            return bookData.Exist(virusID);
        }

        public void BookCollect(int virusID)
        {
            if (!bookData.Exist(virusID))
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.VIRUS_LOCKED.LT());
                return;
            }
            if (bookData.Get(virusID) < ConstTable.table.bookVirusCollectKillCount)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.VIRUS_COLLECT_NOT_ENOUGH.LT());
                return;
            }

            bookData.Add(virusID, -ConstTable.table.bookVirusCollectKillCount);
            AddDiamond(ConstTable.table.bookVirusCollectRewardDiamond);

            SaveLocalData();
            SaveBookData();
            DispatchEvent(EventGameData.Action.DataChange);
        }

        private void SaveBookData()
        {
            bookData.Save();
        }

        #endregion

        #region Weapon
        public int weaponSpeedLevel { get { return weaponLevelData.GetSpeedLevel(weaponId); } }
        public int weaponSpeedMaxLevel { get { return TableWeaponSpeedLevel.GetAll().Max(a => (a.weaponId == weaponId) ? a.level : 0).level; } }
        public bool isWeaponSpeedLevelMax { get { return weaponSpeedLevel >= weaponSpeedMaxLevel; } }
        public float weaponSpeedUpCost { get { return TableWeaponSpeedLevel.Get(a => a.weaponId == weaponId && a.level == weaponSpeedLevel).upCost; } }

        public int weaponPowerLevel { get { return weaponLevelData.GetPowerLevel(weaponId); } }
        public int weaponPowerMaxLevel { get { return TableWeaponPowerLevel.GetAll().Max(a => (a.weaponId == weaponId) ? a.level : 0).level; } }
        public bool isWeaponPowerLevelMax { get { return weaponPowerLevel >= weaponPowerMaxLevel; } }
        public float weaponPowerUpCost { get { return TableWeaponPowerLevel.Get(a => a.weaponId == weaponId && a.level == weaponPowerLevel).upCost; } }

        public void ChangeWeapon(int id)
        {
            if (id > 0 && unlockedGameLevel < TableWeapon.Get(id).unlockLevel)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.NOT_REACH_WEAPON_UNLOCK_GAME_LEVEL.LT());
                return;
            }

            localData.weaponId = id;
            SaveLocalData();

            DispatchEvent(EventGameData.Action.ChangeWeapon);
            DispatchEvent(EventGameData.Action.DataChange);
            Analytics.Event.ChangeWeapon(id);
        }

        public void WeaponSpeedLevelUp()
        {
            if (isWeaponSpeedLevelMax)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.ALREADY_LEVEL_MAX.LT());
                return;
            }

            if (weaponSpeedLevel >= fireSpeedLevel)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.CANNOT_EXCEED_FIRE_SPEED_LEVEL.LT());
                return;
            }

            var cost = weaponSpeedUpCost;
            if (coin < cost)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.UPGRADE_LACK_OF_COIN.LT());
                return;
            }

            localData.coin -= cost;
            weaponLevelData.SetSpeedLevel(weaponId, weaponSpeedLevel + 1);
            SaveLocalData();
            SaveWeaponLevelData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.Upgrade($"{TableWeapon.Get(weaponId).type.ToLower()}_speed", weaponSpeedLevel);
        }

        public void WeaponPowerLevelUp()
        {
            if (isWeaponPowerLevelMax)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.ALREADY_LEVEL_MAX.LT());
                return;
            }

            if (weaponPowerLevel >= firePowerLevel)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.CANNOT_EXCEED_FIRE_POWER_LEVEL.LT());
                return;
            }

            var cost = weaponPowerUpCost;
            if (coin < cost)
            {
                DispatchEvent(EventGameData.Action.Error, LTKey.UPGRADE_LACK_OF_COIN.LT());
                return;
            }

            localData.coin -= cost;
            weaponLevelData.SetPowerLevel(weaponId, weaponPowerLevel + 1);
            SaveLocalData();
            SaveWeaponLevelData();
            DispatchEvent(EventGameData.Action.DataChange);

            Analytics.Event.Upgrade($"{TableWeapon.Get(weaponId).type.ToLower()}_power", weaponPowerLevel);
        }

        private void SaveWeaponLevelData()
        {
            weaponLevelData.Save();
        }
        #endregion


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
    }
}