using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace DestroyViruses
{
    public class EntityBase : MonoBehaviour
    {
        public virtual long uid { get; protected set; }
        public virtual float hp { get; protected set; }
        public virtual bool isAlive { get { return hp > 0; } }

        static object s_lockUIDTag = new object();
        static int s_uid = 1;
        static long GenUID()
        {
            lock (s_lockUIDTag)
            {
                return s_uid++;
            }
        }

        public void AllocateUID()
        {
            uid = GenUID();
        }

        private ResLoader m_loader = null;
        protected virtual ResLoader loader
        {
            get
            {
                if (m_loader == null)
                    m_loader = ResLoader.Allocate();
                return m_loader;
            }
        }


        protected virtual void OnDestroy()
        {
            if (m_loader != null)
                m_loader.Recycle2Cache();
            m_loader = null;
        }
    }
}