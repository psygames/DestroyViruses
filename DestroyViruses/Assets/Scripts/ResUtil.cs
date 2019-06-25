using UnityEngine;
using System;
using Object = UnityEngine.Object;

namespace QFramework
{
    public static class ResUtil
    {
        public static T LoadSync<T>(string ownerBundle, string assetName) where T : Object
        {
            ResLoader loader = ResLoader.Allocate();
            T t = loader.LoadSync<T>(ownerBundle, assetName);
            loader.Recycle2Cache();
            loader = null;
            return t;
        }

        public static T LoadSync<T>(string assetName) where T : Object
        {
            ResLoader loader = ResLoader.Allocate();
            T t = loader.LoadSync<T>(assetName);
            loader.Recycle2Cache();
            loader = null;
            return t;
        }

        public static Object LoadSync(string assetName)
        {
            ResLoader loader = ResLoader.Allocate();
            var t = loader.LoadSync(assetName);
            loader.Recycle2Cache();
            loader = null;
            return t;
        }
    }
}