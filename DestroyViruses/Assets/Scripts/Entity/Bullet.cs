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
            IsRecycled = true;
            Log.I("»ØÊÕBullet£º{}", uid);
        }

        public bool IsRecycled { get; set; }

        public static Bullet Allocate()
        {
            var bullet = EntityFactory.Instance.CreateBullet();
            bullet.IsRecycled = false;
            return bullet;
        }

        public void Recycle2Cache()
        {
            EntityFactory.Instance.RecycleBullet(this);
        }

        public void Update()
        {
            if (IsRecycled)
                return;

            transform.localPosition += Vector3.up * 10;
            if (transform.localPosition.y > 1000)
                Recycle2Cache();
        }
    }
}