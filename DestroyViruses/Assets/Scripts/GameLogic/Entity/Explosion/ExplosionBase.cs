using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DestroyViruses
{
    public class ExplosionBase<T> : EntityBase<T> where T : ExplosionBase<T>
    {
        public float liveTime = 1;

        private void OnEnable()
        {
            transform.localRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward);
            this.DelayDo(liveTime, Recycle);
        }
    }
}