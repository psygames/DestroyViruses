using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniPool;
namespace DestroyViruses
{
    public class EntityBase : MonoBehaviour
    {
        public int uid { get; protected set; } = -1;
        public bool isAlive { get; protected set; } = false;

        private RectTransform mRectTransform = null;
        public RectTransform rectTransform
        {
            get
            {
                if (mRectTransform == null)
                    mRectTransform = GetComponent<RectTransform>();
                return mRectTransform;
            }
        }

        public void Recycle()
        {
            PoolManager.ReleaseObject(gameObject);
        }
    }

    public class EntityBase<T> : EntityBase where T : EntityBase
    {
        public static T Create()
        {
            return EntityManager.Create<T>();
        }
    }
}