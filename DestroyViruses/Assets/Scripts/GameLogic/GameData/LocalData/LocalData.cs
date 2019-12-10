using UnityEngine;
using System;

namespace DestroyViruses
{
    public class LocalData<T> where T : class, new()
    {
        public void Save()
        {
            LocalDataUtil.Save(GetType().FullName, this);
        }

        public static T Load()
        {
            var data = LocalDataUtil.Load<T>(typeof(T).FullName);
            if (data == null)
                return new T();
            return data;
        }


        private static T sInstance = null;
        public static T Instance
        {
            get
            {
                if (sInstance == null)
                    sInstance = Load();
                return sInstance;
            }
        }

        public static void Reload()
        {
            sInstance = Load();
        }
    }

}
