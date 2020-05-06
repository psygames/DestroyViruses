using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class WeaponLevelPanel : ViewBase
    {
        public Image icon;

        public ButtonPro powerUpBtn;
        public Text powerLevelText;
        public Slider powerFill;
        public ButtonTips powerTips;

        public ButtonPro speedUpBtn;
        public Text speedLevelText;
        public Slider speedFill;
        public ButtonTips speedTips;

        private void OnClickPowerUp()
        {
            if (D.I.weaponId <= 0)
                return;

            if (D.I.weaponPowerLevel < D.I.firePowerLevel
                && !D.I.isWeaponPowerLevelMax
                && D.I.coin < D.I.weaponPowerUpCost)
            {
                Toast.Show(LTKey.UPGRADE_LACK_OF_COIN.LT());
                UIManager.Open<ExchangeView>();
                return;
            }

            D.I.WeaponPowerLevelUp();
        }

        private void OnClickSpeedUp()
        {
            if (D.I.weaponId <= 0)
                return;

            if (D.I.weaponSpeedLevel < D.I.fireSpeedLevel
                && !D.I.isWeaponSpeedLevelMax
                && D.I.coin < D.I.weaponSpeedUpCost)
            {
                Toast.Show(LTKey.UPGRADE_LACK_OF_COIN.LT());
                UIManager.Open<ExchangeView>();
                return;
            }

            D.I.WeaponSpeedLevelUp();
        }


        public void SetData()
        {
            if (D.I.unlockedGameLevel < CT.table.weaponUnlockLevel)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            if (D.I.weaponId <= 0)
            {
                icon.SetSprite("icon_weapon_default");

                powerFill.value = 0;
                powerLevelText.text = $"{LTKey.FIRE_POWER.LT()} {LTKey.LEVEL_DOT.LT()}1";
                powerUpBtn.SetData("0", true, false);

                speedFill.value = 0;
                speedLevelText.text = $"{LTKey.FIRE_SPEED.LT()} {LTKey.LEVEL_DOT.LT()}1";
                speedUpBtn.SetData("0", true, false);
                return;
            }

            var table = TableWeapon.Get(D.I.weaponId);
            icon.SetSprite(table.icon);

            powerUpBtn.Set4LevelUp(D.I.weaponPowerUpCost, D.I.isWeaponPowerLevelMax);
            powerLevelText.text = $"{LTKey.FIRE_POWER.LT()} {LTKey.LEVEL_DOT.LT()}{D.I.weaponPowerLevel}";
            powerFill.value = 1f * D.I.weaponPowerLevel / D.I.weaponPowerMaxLevel;
            powerTips.SetData(table.powerUpTips.LT());

            speedUpBtn.Set4LevelUp(D.I.weaponSpeedUpCost, D.I.isWeaponSpeedLevelMax);
            speedLevelText.text = $"{LTKey.FIRE_SPEED.LT()} {LTKey.LEVEL_DOT.LT()}{D.I.weaponSpeedLevel}";
            speedFill.value = 1f * D.I.weaponSpeedLevel / D.I.weaponSpeedMaxLevel;
            speedTips.SetData(table.speedUpTips.LT());
        }
    }
}