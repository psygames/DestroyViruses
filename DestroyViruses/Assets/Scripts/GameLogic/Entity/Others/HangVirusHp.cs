using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class HangVirusHp : MonoBehaviour
    {
        private Transform bindTarget;
        private void Start()
        {
            var parent = transform.parent;
            if (parent == null)
                return;
            var subling = parent.GetComponentInChildren<Subling3D>();
            if (subling == null || subling.transform.childCount <= 0)
                return;
            bindTarget = subling.transform.GetChild(0);
            if (bindTarget == null)
                return;
            transform.SetParent(bindTarget, true);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -bindTarget.localPosition.z + 0.3f);
        }

        private void Update()
        {
            if (bindTarget != null)
            {
            }
        }
    }
}