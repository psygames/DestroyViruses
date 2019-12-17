using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class LevelPanel : MonoBehaviour
    {
        public Button firePowerUpBtn;
        public Text firePowerLevelText;
        public Text firePowerUpCostText;
        public Slider firePowerFill;

        public Button fireSpeedUpBtn;
        public Text fireSpeedLevelText;
        public Text fireSpeedUpCostText;
        public Slider fireSpeedFill;

        private void Awake()
        {
            firePowerUpBtn.OnClick(() =>
            {
                if (D.I.isFirePowerLevelMax)
                {

                }
                else
                {
                    D.I.FirePowerUp();
                }
            });

            fireSpeedUpBtn.OnClick(() =>
            {
                if (D.I.isFireSpeedLevelMax)
                {

                }
                else
                {
                    D.I.FireSpeedUp();
                }
            });
        }

        public void SetData()
        {

            // power
            if (D.I.isFirePowerLevelMax)
            {
                firePowerUpCostText.text = LT.table.LEVEL_MAX;
            }
            else
            {
                firePowerUpCostText.text = D.I.firePowerUpCost.KMB();
            }

            if (D.I.isFireSpeedLevelMax)
            {
                fireSpeedUpCostText.text = LT.table.LEVEL_MAX;
            }
            else
            {
                fireSpeedUpCostText.text = D.I.fireSpeedUpCost.KMB();
            }
            firePowerLevelText.text = $"{LT.table.FIRE_POWER} {LT.table.LEVEL_DOT}{D.I.firePowerLevel}";
            firePowerFill.value = 1f * D.I.firePowerLevel / D.I.firePowerMaxLevel;

            // speed
            if (!D.I.isFirePowerLevelMax && D.I.firePowerUpCost > D.I.coin)
            {
                firePowerUpCostText.color = UIUtil.RED_COLOR;
            }
            else
            {
                firePowerUpCostText.color = Color.white;
            }

            if (!D.I.isFireSpeedLevelMax && D.I.fireSpeedUpCost > D.I.coin)
            {
                fireSpeedUpCostText.color = UIUtil.RED_COLOR;
            }
            else
            {
                fireSpeedUpCostText.color = Color.white;
            }
            fireSpeedLevelText.text = $"{LT.table.FIRE_SPEED} {LT.table.LEVEL_DOT}{D.I.fireSpeedLevel}";
            fireSpeedFill.value = 1f * D.I.fireSpeedLevel / D.I.fireSpeedMaxLevel;
        }
    }
}