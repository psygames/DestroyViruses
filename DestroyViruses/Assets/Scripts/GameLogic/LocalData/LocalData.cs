using UnityEngine;

namespace DestroyViruses
{
    public class LocalData<T>
    {
        public void Save()
        {
            LocalDataUtil.Save(GetType().FullName, this);
        }

        public static T Load()
        {
            return LocalDataUtil.Save(GetType().FullName, this);
        }
    }

}
