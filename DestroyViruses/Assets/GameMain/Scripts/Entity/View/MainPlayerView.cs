using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace DestroyViruses
{
    public class MainPlayerView : MonoBehaviour
    {
        public string planeResPrefix = "";
        public string fireResPrefix = "";

        public Transform planeRoot;
        public Transform fireRoot;
        public Transform headTrans;

        public void UseBody(int level)
        {
             // var obj = loader.LoadSync<GameObject>(planeResPrefix + level).Instantiate();
            // obj.transform.SetParent(planeRoot, false);
        }

        public void UseFire(int level)
        {
            // var obj = loader.LoadSync<GameObject>(fireResPrefix + level).Instantiate();
            //obj.transform.SetParent(fireRoot, false);
        }

        System.IDisposable fireLoop = null;
        private void Fire()
        {
            fireLoop = Observable.Interval(new System.TimeSpan(1000000)).Subscribe((interval) =>
            {
                var b = Bullet.Allocate();
                b.transform.localPosition = UIUtility.GetUIPos(headTrans);
            });
        }

        private void HoldFire()
        {
            if (fireLoop != null)
                fireLoop.Dispose();
            fireLoop = null;
        }
    }
}