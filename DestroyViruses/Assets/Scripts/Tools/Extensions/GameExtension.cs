using UnityEngine;
using UnityEngine.UI;
using System;
using DestroyViruses;

public static class GameExtension
{
    public static float UpdateCD(this MonoBehaviour monoBehaviour, float value)
    {
        return Mathf.Max(0, value - Time.deltaTime);
    }


    public static Vector2 GetUIPos(this Transform transformm)
    {
        return UIUtil.GetUIPos(transformm);
    }

    public static void SetSprite(this Image image, string uniqueSpriteName)
    {
        image.sprite = UIUtil.GetSprite(uniqueSpriteName);
        if (image.type == Image.Type.Simple)
        {
            image.SetNativeSize();
        }
    }

    // 数值单位转换

    private static string[] sKBMUnits = { "K", "M", "B" };
    private static float[] sKBMDivs = { 1000f, 1000000f, 1000000000f, 1000000000000f };
    public static string KMB(this float value)
    {
        if (value < sKBMDivs[0])
        {
            return Mathf.CeilToInt(value).ToString();
        }

        for (int i = 0; i < sKBMUnits.Length; i++)
        {
            if (value >= sKBMDivs[i] && value < sKBMDivs[i + 1])
            {
                value = value / sKBMDivs[i];
                int d1 = Mathf.CeilToInt(value * 10) % 10;
                if (d1 == 0)
                    return (Mathf.CeilToInt(value)).ToString() + sKBMUnits[i];
                return (Mathf.CeilToInt(value)).ToString() + "." + d1.ToString() + sKBMUnits[i];
            }
        }
        return "!KBM";
    }

    // 数值单位转换
    public static string KMB(this int value)
    {
        return KMB((float)value);
    }
}
