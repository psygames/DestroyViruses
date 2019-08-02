using System;
using UniPool;
using UnityEngine;

namespace DestroyViruses
{
    public class EntityManager : Singleton<EntityManager>
    {
        [Serializable]
        public struct PooledPrefab
        {
            public GameObject prefab;
            public Transform root;
        }

        public PooledPrefab[] pooledPrefabs;

        private T create<T>() where T : EntityBase
        {
            PooledPrefab? _pooledPrefab = null;
            foreach (var pp in pooledPrefabs)
            {
                if (pp.prefab.GetComponent<EntityBase>().GetType() == typeof(T))
                {
                    _pooledPrefab = pp;
                    break;
                }
            }
            if (_pooledPrefab == null)
            {
                Debug.LogError($"Create Entity Failed , {typeof(T).Name} is not pooled");
                return null;
            }

            var entity = PoolManager.SpawnObject(_pooledPrefab.Value.prefab).GetComponent<T>();
            if (_pooledPrefab.Value.root != null)
            {
                entity.transform.SetParent(_pooledPrefab.Value.root);
            }

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
            //TODO:Clear Entities
        }
        #endregion
    }
}
