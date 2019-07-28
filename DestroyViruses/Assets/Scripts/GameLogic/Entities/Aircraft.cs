using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class Aircraft : EntityBase
    {
        public static Aircraft Create()
        {
            return EntityManager.Create<Aircraft>();
        }
    }
}