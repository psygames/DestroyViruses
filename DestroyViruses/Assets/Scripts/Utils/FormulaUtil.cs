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
            DynamicExpresso.Interpreter interpreter = new DynamicExpresso.Interpreter();

            interpreter.SetFunction("abs", abs);
            interpreter.SetFunction("sqrt", sqrt);
            interpreter.SetFunction("ceil", ceil);
            interpreter.SetFunction("floor", floor);
            interpreter.SetFunction("round", round);
            interpreter.SetFunction("pow", pow);
            interpreter.SetFunction("min", min);
            interpreter.SetFunction("max", max);
            interpreter.SetFunction("clamp", clamp);

            var pms = new List<DynamicExpresso.Parameter>();
            pms.Add(new DynamicExpresso.Parameter("fp", D.I.firePower));
            pms.Add(new DynamicExpresso.Parameter("fs", D.I.fireSpeed));
            pms.Add(new DynamicExpresso.Parameter("cv", D.I.coinValue));
            pms.Add(new DynamicExpresso.Parameter("ci", D.I.coinIncome));
            pms.Add(new DynamicExpresso.Parameter("gl", D.I.gameLevel));
            pms.Add(new DynamicExpresso.Parameter("fpl", TableGameLevel.Get(D.I.gameLevel).firePowerLimitation));
            pms.Add(new DynamicExpresso.Parameter("ugl", D.I.unlockedGameLevel));
            pms.Add(new DynamicExpresso.Parameter("sk", D.I.streak));
            for (int i = 0; i < parameters.Length; i++)
            {
                pms.Add(new DynamicExpresso.Parameter($"a{i}", parameters[i]));
            }
            var obj = interpreter.Eval(expression, pms.ToArray());
            if (obj is int)
                return (int)obj;
            if (obj is double)
                return (float)(double)obj;
            if (obj is float)
                return (float)obj;
            throw new System.Exception($"Error Return Value type {obj.GetType()}");
        }
    }
}