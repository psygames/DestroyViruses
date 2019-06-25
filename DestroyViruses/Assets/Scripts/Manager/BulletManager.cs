using UnityEngine;
using UnityEditor;
using QFramework;
using System;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class BulletManager : Singleton<BulletManager>
    {
        private SimpleObjectPool<Bullet> m_pool = null;
        private const string s_rootName = "UIRoot/Forward/";

        private Transform m_root;

        public void Init()
        {
            m_root = GameObject.Find(s_rootName).transform;

            m_pool = new SimpleObjectPool<Bullet>(CreateBullet, OnResetBullet, 100);
        }

        private Bullet CreateBullet()
        {

        }

        private void OnResetBullet(Bullet action)
        {
        }

    }
}