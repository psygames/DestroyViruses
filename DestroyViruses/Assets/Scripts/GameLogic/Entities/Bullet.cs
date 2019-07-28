using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class Bullet : EntityBase
    {
        public static Bullet Create()
        {
            return EntityManager.Create<Bullet>();
        }
    }
}