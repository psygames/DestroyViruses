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
            if (probArray.Length <= 0)
                return default;
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
            return CT.table.coinValue[size - 1] * factor1 * factor2;
        }

        public static int GetHpColorIndex(Vector2 hpRange, float hp, int colorCount = 6)
        {
            float percent = (hp - hpRange.x) / (hpRange.y - hpRange.x);
            int index = (int)(percent * colorCount);
            return Mathf.Clamp(index, 0, colorCount - 1);
        }

        public static float Expresso(string expression, params float[] parameters)
        {
            System.Func<float, float> abs = (x) => Mathf.Abs(x);
            System.Func<float, float> sqrt = (x) => Mathf.Sqrt(x);
            System.Func<float, float> ceil = (x) => Mathf.Ceil(x);
            System.Func<float, float> floor = (x) => Mathf.Floor(x);
            System.Func<float, float> round = (x) => Mathf.Round(x);
            System.Func<float, float, float> pow = (x, y) => Mathf.Pow(x, y);
            System.Func<float, float, float> min = (x, y) => Mathf.Min(x, y);
            System.Func<float, float, float> max = (x, y) => Mathf.Max(x, y);
            System.Func<float, float, float, float> clamp = (a, x, y) => Mathf.Clamp(a, x, y);

            /*
            interpreter.SetFunction("abs", abs);
            interpreter.SetFunction("sqrt", sqrt);
            interpreter.SetFunction("ceil", ceil);
            interpreter.SetFunction("floor", floor);
            interpreter.SetFunction("round", round);
            interpreter.SetFunction("pow", pow);
            interpreter.SetFunction("min", min);
            interpreter.SetFunction("max", max);
            interpreter.SetFunction("clamp", clamp);
            */

            Dictionary<string, float> args = new Dictionary<string, float>()
            {
                { "fpl", TableGameLevel.Get(D.I.gameLevel).firePowerLimitation},
                { "fp", D.I.firePower},
                { "cv", D.I.coinValue},
                { "ci", D.I.coinIncome},
                { "ugl", D.I.unlockedGameLevel},
                { "gl", D.I.gameLevel},
                { "sk", D.I.streak},
            };

            foreach (var kv in args)
            {
                expression = expression.Replace(kv.Key, kv.Value.ToString());
            }

            return MtC.Tools.Evaluate.Evaluate.Eval(expression);
        }
    }
}