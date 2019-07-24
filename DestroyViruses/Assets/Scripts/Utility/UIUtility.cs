using UnityEngine;
using System;
using UnityEngine.UI;

namespace DestroyViruses
{
    public static class UIUtility
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
                        return null;
                    s_uiRoot = go.GetComponent<RectTransform>();
                }
                return s_uiRoot;
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
        public static float aspect { get { return (float)Screen.width / (float)Screen.height; } }
        public static Vector2 center { get { return size * 0.5f; } }
        public static Vector2 size { get { return new Vector2(width, height); } }

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
    }
}