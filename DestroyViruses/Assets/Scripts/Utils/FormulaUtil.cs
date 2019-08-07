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

        public static int WaveVirusCount(int wave, int total)
        {
            if (wave == 1)
                return (int)(total * 0.7f);
            else if (wave == 2)
                return total - WaveVirusCount(1, total);
            return 0;
        }

        public static float WaveSpawnInterval(int wave, float interval)
        {
            if (wave == 1)
                return interval;
            else
                return interval * 0.7f;
        }

        public static float RandomInRanage(Vector2 range)
        {
            return Mathf.Lerp(range.x, range.y, UnityEngine.Random.value);
        }
    }
}