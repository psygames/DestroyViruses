using System.Collections.Generic;
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

        public static T RandomInProbDict<T>(Dictionary<T, float> dict)
        {
            float total = 0;
            foreach (var kv in dict)
            {
                total += kv.Value;
            }

            if (Mathf.Approximately(total, 0))
                throw new System.Exception("Prob Dict Total: 0");

            var v = Random.value;
            float cur = 0;
            foreach (var kv in dict)
            {
                cur += kv.Value;
                if (v <= cur / total)
                {
                    return kv.Key;
                }
            }
            throw new System.Exception("Prob Dict Random Value Falut: " + v);
        }

        public static int RandomIndexInProbArray(float[] probArray)
        {
            float total = 0;
            for (int i = 0; i < probArray.Length; i++)
            {
                total += probArray[i];
            }

            if (Mathf.Approximately(total, 0))
                throw new System.Exception("Prob Array Total: 0");

            var v = Random.value;
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

        public static float CoinConvert(int size, float factor1 = 1, float factor2 = 1)
        {
            return ConstTable.table.coinValue[size - 1] * factor1 * factor2;
        }

        public static int GetHpColorIndex(Vector2 hpRange, float hp, int colorCount = 9)
        {
            float percent = Mathf.Sqrt((hp - hpRange.x) / (hpRange.y - hpRange.x));
            int index = Mathf.Clamp((int)(percent * percent * colorCount), 0, colorCount - 1);
            index = colorCount - index - 1; // 翻转
            return index;
        }

        // clamp(pow( pow(a*f1,f6) + pow(b*f2,f7), f3),f4,f5)
        private static float FormulaAB_Add(float a, float b, float[] args)
        {
            var fn = args.Length - 1;
            if (fn < 2)
                return a + b;
            if (fn < 3)
                return (a * args[1]) + (b * args[2]);
            if (fn < 5)
                return Mathf.Pow((a * args[1]) + (b * args[2]), args[3]);
            if (fn < 7)
                return Mathf.Clamp(Mathf.Pow((a * args[1]) + (b * args[2]), args[3]), args[4], args[5]);
            return Mathf.Clamp(Mathf.Pow(Mathf.Pow(a * args[1], args[6]) + Mathf.Pow(b * args[2], args[7]), args[3]), args[4], args[5]);
        }

        // clamp(pow( pow(a*f1,f6) - pow(b*f2,f7), f3),f4,f5)
        private static float FormulaAB_Subtract(float a, float b, float[] args)
        {
            var fn = args.Length - 1;
            if (fn < 2)
                return a - b;
            if (fn < 3)
                return (a * args[1]) - (b * args[2]);
            if (fn < 5)
                return Mathf.Pow((a * args[1]) - (b * args[2]), args[3]);
            if (fn < 7)
                return Mathf.Clamp(Mathf.Pow((a * args[1]) - (b * args[2]), args[3]), args[4], args[5]);
            return Mathf.Clamp(Mathf.Pow(Mathf.Pow(a * args[1], args[6]) - Mathf.Pow(b * args[2], args[7]), args[3]), args[4], args[5]);
        }

        // clamp(pow( pow(a+f1,f6) * pow(b+f2,f7), f3), f4, f5)
        private static float FormulaAB_Multiply(float a, float b, float[] args)
        {
            var fn = args.Length - 1;
            if (fn < 2)
                return a * b;
            if (fn < 3)
                return (a + args[1]) * (b + args[2]);
            if (fn < 5)
                return Mathf.Pow((a + args[1]) * (b + args[2]), args[3]);
            if (fn < 7)
                return Mathf.Clamp(Mathf.Pow((a + args[1]) * (b + args[2]), args[3]), args[4], args[5]);
            return Mathf.Clamp(Mathf.Pow(Mathf.Pow(a + args[1], args[6]) * Mathf.Pow(b + args[2], args[7]), args[3]), args[4], args[5]);
        }

        // clamp(pow( pow(a+f1,f6) / pow(b+f2,f7), f3), f4, f5)
        private static float FormulaAB_Divide(float a, float b, float[] args)
        {
            var fn = args.Length - 1;
            if (fn < 2)
                return a / b;
            if (fn < 3)
                return (a + args[1]) / (b + args[2]);
            if (fn < 5)
                return Mathf.Pow((a + args[1]) / (b + args[2]), args[3]);
            if (fn < 7)
                return Mathf.Clamp(Mathf.Pow((a + args[1]) / (b + args[2]), args[3]), args[4], args[5]);
            return Mathf.Clamp(Mathf.Pow(Mathf.Pow(a + args[1], args[6]) / Mathf.Pow(b + args[2], args[7]), args[3]), args[4], args[5]);
        }

        public static float FormulaAB(float a, float b, float[] args = null)
        {
            if (Mathf.Approximately(args[0], 0))
                return FormulaAB_Add(a, b, args);
            if (Mathf.Approximately(args[0], 1))
                return FormulaAB_Subtract(a, b, args);
            if (Mathf.Approximately(args[0], 2))
                return FormulaAB_Multiply(a, b, args);
            if (Mathf.Approximately(args[0], 3))
                return FormulaAB_Divide(a, b, args);
            throw new System.ArgumentException("数值运算方式错误！");
        }

        public static float VirusSpawnCountFix(int gameLevel, float firePower)
        {
            var tableGameLevel = TableGameLevel.Get(gameLevel);
            return FormulaAB(firePower, tableGameLevel.firePowerLimitation, ConstTable.table.formulaArgsVirusSpawnCountFix);
        }

        public static float VirusHpFix(int gameLevel, float firePower)
        {
            var tableGameLevel = TableGameLevel.Get(gameLevel);
            return FormulaAB(firePower, tableGameLevel.firePowerLimitation, ConstTable.table.formulaArgsVirusHpFix);
        }

        public static float CoinExchangeFix(int gameLevel, float coinValue)
        {
            return FormulaAB(gameLevel, coinValue, ConstTable.table.formulaArgsCoinExchange);
        }
    }
}