using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class LevelPanel : ViewBase
    {
        public ButtonPro firePowerUpBtn;
        public Text firePowerLevelText;
        public Slider firePowerFill;

        public ButtonPro fireSpeedUpBtn;
        public Text fireSpeedLevelText;
        public Slider fireSpeedFill;

        private void OnClickFirePowerUp()
        {
            if (!D.I.isFirePowerLevelMax && D.I.coin < D.I.firePowerUpCost)
            {
                Toast.Show(LTKey.UPGRADE_LACK_OF_COIN.LT());
                UIManager.Open<ExchangeView>();
                return;
            }
            D.I.FirePowerUp();
        }

        private void OnClickFireSpeedUp()
        {
            if (!D.I.isFireSpeedLevelMax && D.I.coin < D.I.fireSpeedUpCost)
            {
                Toast.Show(LTKey.UPGRADE_LACK_OF_COIN.LT());
                UIManager.Open<ExchangeView>();
                return;
            }
            D.I.FireSpeedUp();
        }


        public void SetData()
        {
            firePowerUpBtn.Set4LevelUp(D.I.firePowerUpCost, D.I.isFirePowerLevelMax);
            firePowerLevelText.text = $"{LTKey.FIRE_POWER.LT()} {LTKey.LEVEL_DOT.LT()}{D.I.firePowerLevel}";
            firePowerFill.value = 1f * D.I.firePowerLevel / D.I.firePowerMaxLevel;

            fireSpeedUpBtn.Set4LevelUp(D.I.fireSpeedUpCost, D.I.isFireSpeedLevelMax);
            fireSpeedLevelText.text = $"{LTKey.FIRE_SPEED.LT()} {LTKey.LEVEL_DOT.LT()}{D.I.fireSpeedLevel}";
            fireSpeedFill.value = 1f * D.I.fireSpeedLevel / D.I.fireSpeedMaxLevel;
        }
    }
}