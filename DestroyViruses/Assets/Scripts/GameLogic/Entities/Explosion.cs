using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace DestroyViruses
{
    public class Explosion : EntityBase<Explosion>
    {
        public float liveTime = 1;

        private void OnEnable()
        {
            transform.localRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward);
            Observable.Timer(TimeSpan.FromSeconds(liveTime)).Do((_) =>
            {
                Recycle();
            }).Subscribe();
        }
    }
}