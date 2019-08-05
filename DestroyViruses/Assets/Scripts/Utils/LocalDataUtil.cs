using UnityEngine;
namespace DestroyViruses
{
    public static class LocalDataUtil
    {
        public static void Save(string key, object data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
        }

        public static T Load<T>(string key)
        {
            var json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(json);
        }
    }
}
