﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace DestroyViruses
{
    public class Virus : EntityBase, IPoolable, IPoolType
    {
        private static EntityPool<Virus> s_pool = null;

        public static Virus Allocate()
        {
            if(s_pool == null)
                s_pool = new EntityPool<Virus>("Resources/Prefabs/Virus", "UIRoot/Entity");
            return s_pool.Create();
        }

        public bool IsRecycled { get; set; }
        public void Recycle2Cache()
        {
            s_pool.Recycle(this);
        }

        public void OnRecycled()
        {
            Log.I("回收Bullet：{0}", uid);
        }


        public void Update()
        {
            if (IsRecycled)
                return;

            transform.localPosition += Vector3.up * 300 * Time.deltaTime;
            if (transform.localPosition.y > UIUtil.height + 100)
                Recycle2Cache();
        }
    }
}