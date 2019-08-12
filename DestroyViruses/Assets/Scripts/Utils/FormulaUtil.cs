using UnityEngine;

namespace DestroyViruses
{
    public static class FormulaUtil
    {
        public static float FirePower(int firePowerLevel)
        {
            return 100f + firePowerLevel * 20f;
        }

        public static float FirePowerUpCost(int firePowerLevel)
        {
            return firePowerLevel * 6240f;
        }

        public static float FireSpeed(int fireSpeedLevel)
        {
            return 40 + fireSpeedLevel * 5f;
        }


        public static float FireSpeedUpCost(int fireSpeedLevel)
        {
            return fireSpeedLevel * 5130f;
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

        public static int WaveVirusCount(int wave, int total)
        {
            if (wave == 1)
                return (int)(total * 0.4f);
            if (wave == 2)
                return (int)(total * 0.4f);
            if (wave == 3)
                return (int)(total * 0.2f);
            return 0;
        }

        public static float WaveSpawnInterval(int wave, float interval)
        {
            return interval / Mathf.Sqrt(wave);
        }

        public static float CoinConvert(float hp)
        {
            return hp * 0.01f;
        }

        public static float RandomInRanage(Vector2 range)
        {
            return Mathf.Lerp(range.x, range.y, UnityEngine.Random.value);
        }
    }
}