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


    public static void SetSprite(this Image image,string atlasPath, string spriteName)
    {
        image.sprite = UIUtil.GetSprite(atlasPath, spriteName);
    }
}
