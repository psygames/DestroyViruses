using UnityEngine;
using Plugins.XAsset;

namespace DestroyViruses
{
    public static class ResourceUtil
    {
        public static T Load<T>(string path) where T : Object
        {
            Asset asset;
            if (typeof(T) == typeof(Texture2D)
                || typeof(T) == typeof(AudioClip)
                || typeof(T) == typeof(TextAsset))
            {
                asset = Assets.Load(path, typeof(T));
            }
            else
            {
                asset = Assets.Load(path, typeof(Object));
            }

            if (asset.asset is GameObject)
            {
                var go = asset.asset as GameObject;
                return go.GetComponent<T>();
            }
            return asset.asset as T;
        }

        public static Asset Load(string path)
        {
            return Assets.Load(path, typeof(Object));
        }
    }
}
