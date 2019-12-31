using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class SublingParticle : MonoBehaviour
    {
        public RectTransform bindTarget;
        public int sortingOrder = 1;

        public float zOffset = 0f;
        public float zUnit = 0f;
        public int mScale = 180;

        private int mLastSubling = -1;

        private void Awake()
        {
            transform.localScale = new Vector3(mScale, mScale, 1);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zOffset);
                transform.localScale = new Vector3(mScale, mScale, 1);
                foreach (var particle in GetComponentsInChildren<ParticleSystem>(true))
                {
                    var render = particle.GetComponent<Renderer>();
                    if (render != null)
                        render.sortingOrder = sortingOrder;
                }
                return;
            }
#endif
            var subling = bindTarget == null ? 0 : bindTarget.GetSiblingIndex();
            if (mLastSubling != subling)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zUnit * subling + zOffset);
                mLastSubling = subling;
            }
        }
    }
}