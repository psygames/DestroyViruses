using System;
using System.Collections.Generic;
using UniPool;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    [Serializable]
    public struct PooledPrefab
    {
        public GameObject prefab;
        public Transform root;
    }

    public PooledPrefab[] pooledPrefabs;

    private Dictionary<Type, List<EntityBase>> mInstanceDict = new Dictionary<Type, List<EntityBase>>();

    private void warmPoolAll(int perSize)
    {
        foreach (var p in pooledPrefabs)
        {
            PoolManager.WarmPool(p.prefab, perSize);
        }
    }

    private EntityBase create(Type t)
    {
        if (!typeof(EntityBase).IsAssignableFrom(t))
        {
            Debug.LogError($"Create Entity Failed , {t.Name} is not Assignable To EntityBase");
            return null;
        }

        PooledPrefab? _pooledPrefab = null;
        foreach (var pp in pooledPrefabs)
        {
            if (pp.prefab.GetComponent<EntityBase>().GetType() == t)
            {
                _pooledPrefab = pp;
                break;
            }
        }
        if (_pooledPrefab == null)
        {
            Debug.LogError($"Create Entity Failed , {t.Name} is not pooled");
            return null;
        }

        var entity = PoolManager.SpawnObject(_pooledPrefab.Value.prefab).GetComponent<EntityBase>();
        entity.uid = ++sUidCounter;
        if (_pooledPrefab.Value.root != null
            && _pooledPrefab.Value.root != entity.transform.parent)
        {
            entity.transform.SetParent(_pooledPrefab.Value.root);
            entity.transform.localScale = Vector3.one;
            entity.transform.localPosition = Vector3.zero;
            entity.transform.localRotation = Quaternion.identity;
        }
        var type = entity.GetType();
        if (!mInstanceDict.ContainsKey(type))
        {
            mInstanceDict.Add(type, new List<EntityBase>());
        }
        mInstanceDict[type].Add(entity);
        return entity;
    }

    static long sUidCounter;
    private T create<T>() where T : EntityBase
    {
        var entity = create(typeof(T)) as T;
        return entity;
    }

    private bool recycle(EntityBase entity)
    {
        var type = entity.GetType();
        if (!mInstanceDict.ContainsKey(type))
            return false;
        if (!mInstanceDict[type].Contains(entity))
            return false;
        mInstanceDict[type].Remove(entity);
        PoolManager.ReleaseObject(entity.gameObject);
        return true;
    }

    private List<EntityBase> getAll<T>() where T : EntityBase
    {
        var type = typeof(T);
        if (!mInstanceDict.ContainsKey(type))
        {
            var list = new List<EntityBase>();
            foreach (var kv in mInstanceDict)
            {
                if (type.IsAssignableFrom(kv.Key))
                {
                    list.AddRange(kv.Value);
                }
            }
            return list;
        }
        return mInstanceDict[type];
    }

    private int count<T>(Func<T, bool> predict = null) where T : EntityBase
    {
        int _count = 0;
        var type = typeof(T);
        foreach (var kv in mInstanceDict)
        {
            if (type.IsAssignableFrom(kv.Key))
            {
                if (predict == null)
                {
                    _count += kv.Value.Count;
                }
                else
                {
                    foreach (var e in kv.Value)
                    {
                        if (predict.Invoke(e as T))
                        {
                            _count++;
                        }
                    }
                }
            }
        }
        return _count;
    }


    #region Scripts API
    public static T Create<T>() where T : EntityBase
    {
        return Instance.create<T>();
    }

    public static EntityBase Create(Type type)
    {
        return Instance.create(type);
    }

    public static bool Recycle(EntityBase entity)
    {
        return Instance.recycle(entity);
    }

    public static List<EntityBase> GetAll<T>() where T : EntityBase
    {
        return Instance.getAll<T>();
    }

    public static int Count<T>(Func<T, bool> predict = null) where T : EntityBase
    {
        return Instance.count(predict);
    }

    public static void Clear()
    {
        Instance.mInstanceDict.Clear();
        PoolManager.ReleaseAll();
    }

    public static void WarmPoolAll(int size = 1)
    {
        Instance.warmPoolAll(size);
    }
    #endregion
}
