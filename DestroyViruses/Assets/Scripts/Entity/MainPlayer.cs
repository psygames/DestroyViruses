using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UniRx;

namespace DestroyViruses
{
    public class MainPlayer : EntityBase
    {
        public string planeResPrefix = "";
        public string fireResPrefix = "";

        public Transform planeRoot;
        public Transform fireRoot;

        private void Start()
        {
            UseBody(1);
            UseFire(1);

            Observable.Interval(new System.TimeSpan(10000000)).Subscribe((interval) =>
            {
                EntityFactory.Instance.CreateBullet();
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
    }
}