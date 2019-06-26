using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UniRx;

namespace DestroyViruses
{
    public class MainPlayer : EntityBase, IPoolable
    {
        public string planeResPrefix = "";
        public string fireResPrefix = "";

        public Transform planeRoot;
        public Transform fireRoot;
        public Transform headTrans;

        private static EntityPool<MainPlayer> s_pool = null;

        public bool IsRecycled { get; set; }

        public static MainPlayer Allocate()
        {
            if (s_pool == null)
                s_pool = new EntityPool<MainPlayer>("Resources/Prefabs/MainPlayer", "UIRoot/Entity");
            return s_pool.Create();
        }

        private void Start()
        {
            transform.localPosition = UIUtil.center;

            UseBody(1);
            UseFire(1);

            Observable.Interval(new System.TimeSpan(1000000)).Subscribe((interval) =>
            {
                var b = Bullet.Allocate();
                b.transform.localPosition = UIUtil.GetUIPos(headTrans);
            });
        }

        public void UseBody(int level)
        {
            var obj = loader.LoadSync<GameObject>(planeResPrefix + level).Instantiate();
            obj.transform.SetParent(planeRoot, false);
        }

        public void UseFire(int level)
        {
            var obj = loader.LoadSync<GameObject>(fireResPrefix + level).Instantiate();
            obj.transform.SetParent(fireRoot, false);
        }

        public void OnRecycled()
        {
        }
    }
}