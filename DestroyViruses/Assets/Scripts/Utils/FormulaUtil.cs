using UnityEngine;

namespace DestroyViruses
{
    public static class FormulaUtil
    {
        public static float FirePower(int firePowerLevel)
        {
            return firePowerLevel * 100f;
        }

        public static float FireSpeed(int fireSpeedLevel)
        {
            return fireSpeedLevel * 20f;
        }

        public static int RandomInProbArray(float[] probArray)
        {
            var v = Random.value;
            float total = 0;
            for (int i = 0; i < probArray.Length; i++)
            {
                total += probArray[i];
            }

            if (total == 0)
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
    }
}