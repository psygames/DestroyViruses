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
                GameDataManager.Instance.FirePowerUp();
            });

            fireSpeedUpBtn.OnClick(() =>
            {
                GameDataManager.Instance.FireSpeedUp();
            });
        }

        public void SetData()
        {
            firePowerLevelText.text = $"LV.{GDM.ins.firePowerLevel}";
            firePowerUpCostText.text = GDM.ins.firePowerUpCost.KMB();
            fireSpeedLevelText.text = $"LV.{GDM.ins.fireSpeedLevel}";
            fireSpeedUpCostText.text = GDM.ins.fireSpeedUpCost.KMB();

            if (GDM.ins.firePowerUpCost > GDM.ins.coin)
            {
                firePowerUpCostText.color = UIUtil.RED_COLOR;
            }
            else
            {
                firePowerUpCostText.color = Color.white;
            }

            if (GDM.ins.fireSpeedUpCost > GDM.ins.coin)
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