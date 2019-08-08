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

    public static void SetSprite(this Image image, string atlasPath, string spriteName)
    {
        image.sprite = UIUtil.GetSprite(atlasPath, spriteName);
    }

    public static void SetSprite(this Image image, string uniqueSpriteName)
    {
        //TODO: unique sprite name cached
        image.sprite = UIUtil.GetSprite("SpriteAtlas/Common", uniqueSpriteName);
    }
}
