using UnityEngine;
using Plugins.XAsset;

namespace DestroyViruses
{
    public static class ResourceUtil
    {
        public static T Load<T>(string path) where T : Object
        {
            var asset = Assets.Load(path, typeof(Object));
            if (asset.asset is GameObject)
            {
                var go = asset.asset as GameObject;
                return go.GetComponent<T>();
            }
            return asset.asset as T;
        }
    }
}
