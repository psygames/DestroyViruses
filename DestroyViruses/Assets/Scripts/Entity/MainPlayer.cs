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


        private void Start()
        {
            transform.localPosition = UIUtil.center;

            UseBody(1);
            UseFire(1);

            QEventSystem.RegisterEvent(Event.Input.KEY, OnEventInput);
        }

        private void OnEventInput()
        {

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

        System.IDisposable fireLoop = null;
        private void Fire()
        {
            fireLoop = Observable.Interval(new System.TimeSpan(1000000)).Subscribe((interval) =>
            {
                var b = Bullet.Allocate();
                b.transform.localPosition = UIUtil.GetUIPos(headTrans);
            });
        }

        private void HoldFire()
        {
            if (fireLoop != null)
                fireLoop.Dispose();
            fireLoop = null;
        }


        #region POOL
        private static EntityPool<MainPlayer> s_pool = null;

        public bool IsRecycled { get; set; }

        public static MainPlayer Allocate()
        {
            if (s_pool == null)
                s_pool = new EntityPool<MainPlayer>("Resources/Prefabs/MainPlayer", "UIRoot/Entity");
            return s_pool.Create();
        }

        public void OnRecycled()
        {
        }
        #endregion
    }
}