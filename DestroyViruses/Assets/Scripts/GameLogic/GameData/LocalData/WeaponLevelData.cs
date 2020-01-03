using System;
using UnityEngine;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class WeaponLevelData : LocalData<WeaponLevelData>
    {
        [SerializeField]
        private int[] speedLevel = new int[100];
        [SerializeField]
        private int[] powerLevel = new int[100];

        public WeaponLevelData()
        {
            for (int i = 0; i < speedLevel.Length; i++)
            {
                speedLevel[i] = 1;
            }
            for (int i = 0; i < powerLevel.Length; i++)
            {
                powerLevel[i] = 1;
            }
        }

        public void SetSpeedLevel(int id, int level)
        {
            speedLevel[id] = level;
        }

        public void SetPowerLevel(int id, int level)
        {
            powerLevel[id] = level;
        }

        public int GetSpeedLevel(int id)
        {
            return speedLevel[id];
        }

        public int GetPowerLevel(int id)
        {
            return powerLevel[id];
        }
    }
}
