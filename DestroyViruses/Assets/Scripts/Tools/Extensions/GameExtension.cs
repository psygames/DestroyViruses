using UnityEngine;
using UnityEngine.UI;
using System;
using DestroyViruses;

public static class GameExtension
{
    public static float UpdateCD(this MonoBehaviour monoBehaviour, float value, float scale = 1f)
    {
        return Mathf.Max(0, value - Time.deltaTime * scale);
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

    public static void SetAlpha(this Image image, float a)
    {
        var color = image.color;
        color.a = a;
        image.color = color;
    }

    // 数值单位转换
    private static string[] sKMBUnits = { "K", "M", "B", "T", "P", "E", "Z", "Y", "S" };
    private static float[] sKMBDivs = { 1E3f, 1E6f, 1E9f, 1E12f, 1E15f, 1E18f, 1E21f, 1E24f, 1E27f };
    public static string KMB(this float value)
    {
        if (Mathf.Approximately(0, value))
            return "0";

        if (value < sKMBDivs[0])
            return Mathf.CeilToInt(value).ToString();

        for (int i = 0; i < sKMBUnits.Length; i++)
        {
            if (value >= sKMBDivs[i] && value < sKMBDivs[i + 1])
            {
                value = value / sKMBDivs[i];
                int d1 = Mathf.CeilToInt(value * 10) % 10;
                if (d1 == 0)
                    return $"{(int)value}{sKMBUnits[i]}";
                return $"{(int)value}.{d1}{sKMBUnits[i]}";
            }
        }
        return "!KMB";
    }
}
