using UnityEngine;

namespace DestroyViruses
{
    public static class ResourceUtil
    {
        public static T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}
