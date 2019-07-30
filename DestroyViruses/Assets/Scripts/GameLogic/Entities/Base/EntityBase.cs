using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniPool;
namespace DestroyViruses
{
    public class EntityBase : MonoBehaviour
    {
        public const string prefabPathRoot = "Prefabs/Entities/";
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
}