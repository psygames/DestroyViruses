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
        public Button fireSpeedUpBtn;
        public Text fireSpeedLevelText;
        public Text fireSpeedUpCostText;

        private void Awake()
        {
            firePowerUpBtn.OnClick(() =>
            {
                if (GDM.ins.isFirePowerLevelMax)
                {

                }
                else
                {
                    GDM.ins.FirePowerUp();
                }
            });

            fireSpeedUpBtn.OnClick(() =>
            {
                if (GDM.ins.isFireSpeedLevelMax)
                {

                }
                else
                {
                    GDM.ins.FireSpeedUp();
                }
            });
        }

        public void SetData()
        {
            firePowerLevelText.text = $"{LT.table.LEVEL_DOT}{GDM.ins.firePowerLevel}";
            fireSpeedLevelText.text = $"{LT.table.LEVEL_DOT}{GDM.ins.fireSpeedLevel}";

            if (GDM.ins.isFirePowerLevelMax)
            {
                firePowerUpCostText.text = LT.table.LEVEL_MAX;
            }
            else
            {
                firePowerUpCostText.text = GDM.ins.firePowerUpCost.KMB();
            }

            if (GDM.ins.isFireSpeedLevelMax)
            {
                fireSpeedUpCostText.text = LT.table.LEVEL_MAX;
            }
            else
            {
                fireSpeedUpCostText.text = GDM.ins.fireSpeedUpCost.KMB();
            }

            if (!GDM.ins.isFirePowerLevelMax && GDM.ins.firePowerUpCost > GDM.ins.coin)
            {
                firePowerUpCostText.color = UIUtil.RED_COLOR;
            }
            else
            {
                firePowerUpCostText.color = Color.white;
            }

            if (!GDM.ins.isFireSpeedLevelMax && GDM.ins.fireSpeedUpCost > GDM.ins.coin)
            {
                fireSpeedUpCostText.color = UIUtil.RED_COLOR;
            }
            else
            {
                fireSpeedUpCostText.color = Color.white;
            }
        }
    }
}