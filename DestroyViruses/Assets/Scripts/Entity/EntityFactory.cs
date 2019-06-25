using UnityEngine;
using UnityEditor;
using QFramework;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class EntityFactory : Singleton<EntityFactory>
    {
        private Creator<Bullet> bullet = new Creator<Bullet>("Resources/Prefabs/Bullet", "UIRoot/Entity");
        private Creator<MainPlayer> mainPlayer = new Creator<MainPlayer>("Resources/Prefabs/MainPlayer", "UIRoot/Entity");

        protected EntityFactory() { }

        public Bullet CreateBullet()
        {
            return bullet.Create();
        }

        public void Recycle<T>(T bullet)
        {
            this.bullet.Recycle(bullet);
        }

        public MainPlayer CreateMainPlayer()
        {
            return mainPlayer.Create();
        }


        private class Creator<T> where T : EntityBase
        {
            private SimpleObjectPool<T> m_pool = null;
            private string m_prefabPath = "";

            private ResLoader loader = ResLoader.Allocate();

            private Transform m_root;

            public Creator(string prefabPath, string rootPath)
            {
                m_prefabPath = prefabPath;
                m_root = GameObject.Find(rootPath).transform;
                m_pool = new SimpleObjectPool<T>(CreateInstance, OnReset, 1);
            }

            public void Dispose()
            {
                loader.Recycle2Cache();
            }

            private T CreateInstance()
            {
                var go = loader.LoadSync<GameObject>(m_prefabPath).Instantiate();
                go.transform.SetParent(m_root, false);
                return go.GetComponent<T>();
            }

            private void OnReset(T t)
            {
                // Bullet reset
                t.
            }

            public T Create()
            {
                var entity = m_pool.Allocate();
                entity.AllocateUID();
                return entity;
            }

            public void Recycle(T bullet)
            {
                m_pool.Recycle(bullet);
            }
        }
    }
}