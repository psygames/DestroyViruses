using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.Collections.Generic;
using UniRx;

namespace DestroyViruses
{
    public static class UIUtil
    {
        private static RectTransform s_uiRoot = null;
        public static RectTransform uiRoot
        {
            get
            {
                if (s_uiRoot == null)
                {
                    var go = GameObject.Find("UIRoot");
                    if (go == null)
                    {
                        Debug.LogError("Can't find UI Root.");
                        return null;
                    }
                    s_uiRoot = go.GetComponent<RectTransform>();
                }
                return s_uiRoot;
            }
        }

        public static Transform aircraftTransform
        {
            get
            {
                return EntityManager.GetAll<Aircraft>()[0].transform;
            }
        }

        private static CanvasScaler s_canvasScaler = null;
        public static CanvasScaler canvasScaler
        {
            get
            {
                if (s_canvasScaler != null)
                    return s_canvasScaler;
                if (uiRoot == null)
                    return null;
                s_canvasScaler = uiRoot.GetComponent<CanvasScaler>();
                return s_canvasScaler;
            }
        }

        public static float defaultWidth { get { return canvasScaler == null ? 1080 : canvasScaler.referenceResolution.x; } }
        public static float defaultHeight { get { return canvasScaler == null ? 1920 : canvasScaler.referenceResolution.y; } }
        public static float defaultAspect { get { return defaultWidth / defaultHeight; } }
        public static float width { get { return aspect > defaultAspect ? defaultWidth * aspect / defaultAspect : defaultWidth; } }
        public static float height { get { return 1 / aspect * width; } }
        public static float aspect { get { return (float)Screen.width / Screen.height; } }
        public static Vector2 center { get { return size * 0.5f; } }
        public static Vector2 size { get { return new Vector2(width, height); } }
        private static float sRealToVirtualRate { get { return width / Screen.width; } }

        public static float FormatToVirtual(float value)
        {
            return sRealToVirtualRate * value;
        }

        public static Vector2 FormatToVirtual(Vector2 value)
        {
            return sRealToVirtualRate * value;
        }

        public static Vector2 UIWorldToUIPos(Vector3 worldPos)
        {
            if (s_uiRoot == null)
                return Vector2.zero;
            Vector2 uiPos = worldPos / s_uiRoot.localScale.x;
            return uiPos + center;
        }

        public static Vector2 GetUIPos(Transform uiTransform)
        {
            return UIWorldToUIPos(uiTransform.position);
        }

        private static Dictionary<string, SpriteAtlas> sSpriteAtlasDict = null;
        public static void LoadAtlasAll()
        {
            if (sSpriteAtlasDict == null)
            {
                sSpriteAtlasDict = new Dictionary<string, SpriteAtlas>();
                foreach (var atlasName in PathUtil.GetSpriteAtlasNames())
                {
                    var atlas = ResourceUtil.Load<SpriteAtlas>(PathUtil.SpriteAtlas(atlasName));
                    Sprite[] sprites = new Sprite[atlas.spriteCount];
                    atlas.GetSprites(sprites);
                    foreach (var sprite in sprites)
                    {
                        var spriteName = sprite.name.Substring(0, sprite.name.Length - 7);
                        if (!sSpriteAtlasDict.ContainsKey(spriteName))
                        {
                            sSpriteAtlasDict.Add(spriteName, atlas);
                        }
                    }
                    sprites = null;
                }
            }
        }

        public static Sprite GetSprite(string uniqueSpriteName)
        {
            LoadAtlasAll();
            sSpriteAtlasDict.TryGetValue(uniqueSpriteName, out SpriteAtlas spriteAtlas);
            return spriteAtlas?.GetSprite(uniqueSpriteName);
        }

        public static void OnClick(this Button btn, Action callback)
        {
            btn.OnClickAsObservable().TakeUntilDestroy(btn).Subscribe(_ =>
            {
                callback?.Invoke();
            });
        }

        public static Color RED_COLOR = new Color(0.8f, 0.3f, 0.3f);
        public static Color GRAY_COLOR = new Color(0.6f, 0.7f, 0.7f);

        public static Vector2 COIN_POS { get { return new Vector2(72, height - 78); } }
    }
}