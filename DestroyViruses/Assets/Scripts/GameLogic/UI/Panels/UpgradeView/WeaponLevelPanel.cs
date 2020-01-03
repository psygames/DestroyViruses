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

        public ButtonPro speedUpBtn;
        public Text speedLevelText;
        public Slider speedFill;

        private void OnClickPowerUp()
        {
            if (D.I.weaponId <= 0)
                return;
            D.I.WeaponPowerLevelUp();
        }

        private void OnClickSpeedUp()
        {
            if (D.I.weaponId <= 0)
                return;
            D.I.WeaponSpeedLevelUp();
        }


        public void SetData()
        {
            if (D.I.weaponId <= 0)
            {
                icon.SetSprite("icon_weapon_default");

                powerFill.value = 0;
                powerLevelText.text = $"{LT.table.FIRE_POWER} {LT.table.LEVEL_DOT}1";
                powerUpBtn.SetData("0", true, false);

                speedFill.value = 0;
                speedLevelText.text = $"{LT.table.FIRE_SPEED} {LT.table.LEVEL_DOT}1";
                speedUpBtn.SetData("0", true, false);
                return;
            }

            icon.SetSprite(TableWeapon.Get(D.I.weaponId).icon);

            powerUpBtn.Set4LevelUp(D.I.weaponPowerUpCost, D.I.isWeaponPowerLevelMax);
            powerLevelText.text = $"{LT.table.FIRE_POWER} {LT.table.LEVEL_DOT}{D.I.weaponPowerLevel}";
            powerFill.value = 1f * D.I.weaponPowerLevel / D.I.weaponPowerMaxLevel;

            speedUpBtn.Set4LevelUp(D.I.weaponSpeedUpCost, D.I.isWeaponSpeedLevelMax);
            speedLevelText.text = $"{LT.table.FIRE_SPEED} {LT.table.LEVEL_DOT}{D.I.weaponSpeedLevel}";
            speedFill.value = 1f * D.I.weaponSpeedLevel / D.I.weaponSpeedMaxLevel;
        }
    }
}