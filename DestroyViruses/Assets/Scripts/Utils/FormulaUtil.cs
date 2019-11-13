using UnityEngine;

namespace DestroyViruses
{
    public static class FormulaUtil
    {
        public static float FirePower(int firePowerLevel)
        {
            return TableFirePower.Get(firePowerLevel).power;
        }

        public static float FirePowerUpCost(int firePowerLevel)
        {
            return TableFirePower.Get(firePowerLevel).upcost;
        }

        public static float FireSpeed(int fireSpeedLevel)
        {
            return TableFireSpeed.Get(fireSpeedLevel).fireSpeed;
        }

        public static float FireSpeedUpCost(int fireSpeedLevel)
        {
            return TableFireSpeed.Get(fireSpeedLevel).upcost;
        }

        public static T RandomInArray<T>(T[] probArray)
        {
            return probArray[Random.Range(0, probArray.Length)];
        }

        public static int RandomIndexInProbArray(float[] probArray)
        {
            var v = Random.value;
            float total = 0;
            for (int i = 0; i < probArray.Length; i++)
            {
                total += probArray[i];
            }

            if (Mathf.Approximately(total,0))
                throw new System.Exception("Prob Array Total: 0");

            float cur = 0;
            for (int i = 0; i < probArray.Length; i++)
            {
                cur += probArray[i];
                if (v <= cur / total)
                {
                    return i;
                }
            }
            return probArray.Length - 1;
        }

        public static float CoinConvert(float hp)
        {
            return hp * ConstTable.table.hpToCoin;
        }

        public static float RandomInRanage(Vector2 range)
        {
            return Mathf.Lerp(range.x, range.y, Random.value);
        }
    }
}