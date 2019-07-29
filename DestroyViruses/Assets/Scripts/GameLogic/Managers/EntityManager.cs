using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniPool;
using UnityEngine;

namespace DestroyViruses
{
    public class EntityManager : Singleton<EntityManager>
    {
        [SerializeField]
        private GameObject[] pooledPrefabs;

        private T create<T>() where T : EntityBase
        {
            GameObject _gameObject = null;
            foreach (var go in pooledPrefabs)
            {
                if (go.GetComponent<EntityBase>().GetType() == typeof(T))
                {
                    _gameObject = go;
                    break;
                }
            }
            if (_gameObject == null)
            {
                Debug.LogError($"Create Entity Failed , {typeof(T).Name} is not pooled");
                return null;
            }

            var entity = PoolManager.SpawnObject(_gameObject).GetComponent<T>();
            entity.transform.localScale = Vector3.one;
            entity.transform.localPosition = Vector3.zero;
            entity.transform.localRotation = Quaternion.identity;
            return entity;
        }


        #region Scripts API
        public static T Create<T>() where T : EntityBase
        {
            return Instance.create<T>();
        }

        public static void Clear()
        {
        }
        #endregion
    }
}
