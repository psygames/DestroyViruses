using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;
using System.IO;

namespace DestroyViruses
{
    public class DebugView : ViewBase
    {
        public InputField input1;
        public InputField input2;

        private void Awake()
        {
            input1.text = "100";
            input2.text = "100";
        }

        private float GetFloat1()
        {
            float.TryParse(input1.text, out float val);
            return val;
        }

        private int GetInt1()
        {
            int.TryParse(input1.text, out int val);
            return val;
        }

        private float GetFloat2()
        {
            float.TryParse(input2.text, out float val);
            return val;
        }

        private int GetInt2()
        {
            int.TryParse(input2.text, out int val);
            return val;
        }

        private void SaveAndDispatch()
        {
            GameLocalData.Instance.Save();
            Unibus.Dispatch(EventGameData.Get(EventGameData.Action.DataChange));
        }

        private void OnClickReporter()
        {
        }

        private void OnClickClear()
        {
            if (Directory.Exists(Application.persistentDataPath))
            {
                Directory.Delete(Application.persistentDataPath, true);
            }
            PlayerPrefs.DeleteAll();
            GameLocalData.Reload();
            BookData.Reload();
            WeaponLevelData.Reload();
            SaveAndDispatch();
        }

        private void OnClickSelect()
        {
            var level = GetInt1();
            var maxLevel = TableGameLevel.GetAll().Max(a => a.id);
            level = Mathf.Clamp(level, 1, maxLevel.id);
            GameLocalData.Instance.gameLevel = level;
            GameLocalData.Instance.unlockedGameLevel = level;
            if (GameLocalData.Instance.weaponId == 0
                && level >= ConstTable.table.weaponUnlockLevel)
            {
                GameLocalData.Instance.weaponId = 1;
            }
            SaveAndDispatch();
        }

        private void OnClickCollectCount()
        {
            int count = GetInt1();
            var bookIndex = new int[] { 1, 4, 7, 10, 12, 15 };
            for (int i = 0; i < bookIndex.Length; i++)
            {
                BookData.Instance.Set(bookIndex[i], count);
            }
            SaveAndDispatch();
        }

        private void OnClickCoin()
        {
            GameLocalData.Instance.coin = GetFloat1();
            SaveAndDispatch();
        }

        private void OnClickDiamond()
        {
            GameLocalData.Instance.diamond = GetFloat1();
            SaveAndDispatch();
        }

        private void OnClickEnergy()
        {
            GameLocalData.Instance.energy = Mathf.Clamp(GetInt1(), 0, ConstTable.table.energyMax);
            SaveAndDispatch();
        }

        private void OnClickPower()
        {
            GameLocalData.Instance.firePowerLevel = Mathf.Clamp(GetInt1(), 1, D.I.firePowerMaxLevel);
            SaveAndDispatch();
        }

        private void OnClickFireSpeed()
        {
            GameLocalData.Instance.fireSpeedLevel = Mathf.Clamp(GetInt1(), 1, D.I.fireSpeedMaxLevel);
            SaveAndDispatch();
        }

        private void OnClickWeaponPower()
        {
            if (D.I.weaponId == 0)
            {
                Toast.Show("尚未解锁武器");
                return;
            }
            WeaponLevelData.Instance.SetPowerLevel(D.I.weaponId, Mathf.Clamp(GetInt1(), 1, D.I.weaponPowerMaxLevel));
            SaveAndDispatch();
        }

        private void OnClickWeaponFireSpeed()
        {
            if (D.I.weaponId == 0)
            {
                Toast.Show("尚未解锁武器");
                return;
            }
            WeaponLevelData.Instance.SetSpeedLevel(D.I.weaponId, Mathf.Clamp(GetInt1(), 1, D.I.weaponSpeedMaxLevel));
            SaveAndDispatch();
        }

        private void OnClickCoinValue()
        {
            GameLocalData.Instance.coinValueLevel = Mathf.Clamp(GetInt1(), 1, D.I.coinValueMaxLevel);
            SaveAndDispatch();
        }

        private void OnClickCoinIncome()
        {
            GameLocalData.Instance.coinIncomeLevel = Mathf.Clamp(GetInt1(), 1, D.I.coinIncomeMaxLevel);
            SaveAndDispatch();
        }

        private void OnClickVip()
        {
            GameLocalData.Instance.lastVipRewardDays = -1;
            GameLocalData.Instance.lastVipTicks = System.DateTime.Now.Ticks;
            SaveAndDispatch();
        }

        private void OnClickNoAd()
        {
            GameLocalData.Instance.noAd = !GameLocalData.Instance.noAd;
            if (GameLocalData.Instance.noAd)
            {
                Toast.Show("AD OFF");
            }
            else
            {
                Toast.Show("AD ON");
            }
            SaveAndDispatch();
        }
    }
}