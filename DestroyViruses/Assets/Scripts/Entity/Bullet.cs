using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace DestroyViruses
{
    public class Bullet : EntityBase, IPoolable, IPoolType
    {
        public void OnRecycled()
        {
            "ªÿ ’¡À".LogInfo();
        }

        public bool IsRecycled { get; set; }

        public static Bullet Allocate()
        {
            return SafeObjectPool<Bullet>.Instance.Allocate();
        }

        public void Recycle2Cache()
        {
            SafeObjectPool<Bullet>.Instance.Recycle(this);
        }
    }

}