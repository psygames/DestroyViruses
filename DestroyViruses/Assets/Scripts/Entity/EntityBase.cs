using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace DestroyViruses
{
    public class EntityBase : MonoBehaviour
    {
        public virtual float hp { get; protected set; }
        public virtual bool isAlive { get { return hp > 0; } }

        protected ResLoader loader = ResLoader.Allocate();

        protected virtual void OnDestroy()
        {
            loader.Recycle2Cache();
        }
    }
}