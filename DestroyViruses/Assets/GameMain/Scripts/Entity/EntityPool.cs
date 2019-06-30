using UnityEngine;
using UnityEditor;
using GameFramework.ObjectPool;

namespace DestroyViruses
{
    public class EntityPool<T> where T : EntityBase
    { 
        private Transform m_root;
        private GameObject m_template;

        public EntityPool(string prefabPath, string rootPath, int initCount = 1)
        {
            //m_template = loader.LoadSync<GameObject>(prefabPath);
            m_root = GameObject.Find(rootPath).transform;
        }

        public void Dispose()
        {
            //loader.Recycle2Cache();
        }

        private T CreateInstance()
        {
            //var go = m_template.Instantiate();
            //go.transform.SetParent(m_root, false);
            //return go.GetComponent<T>();
            return null;
        }

        private void OnReset(T t)
        {
            //t.IsRecycled = true;
            //t.gameObject.SetActive(false);
            //t.OnRecycled();
        }

        public T Create()
        {
            /*var entity = m_pool.Allocate();
            entity.gameObject.SetActive(true);
            entity.AllocateUID();
            entity.IsRecycled = false;
            return entity;*/
            return null;
        }

        public void Recycle(T bullet)
        {
            //m_pool.Recycle(bullet);
        }
    }
}