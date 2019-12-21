using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class Subling3D : MonoBehaviour
    {
        public RectTransform bindTarget;
        public float zOffset = 0.3f;
        public float zUnit = 0.3f;

        private int mLastSubling = -1;
        private int mScale = 260;
        private int mRotY = 180;

        private void Awake()
        {
            transform.localScale = new Vector3(mScale, mScale, 1);
            transform.rotation = Quaternion.Euler(0, mRotY, 0);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zOffset);
                transform.localScale = new Vector3(mScale, mScale, 1);
                transform.rotation = Quaternion.Euler(0, mRotY, 0);
                return;
            }
#endif
            var subling = bindTarget.GetSiblingIndex();
            if (mLastSubling != subling)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zUnit * subling + zOffset);
                mLastSubling = subling;
            }
        }
    }
}