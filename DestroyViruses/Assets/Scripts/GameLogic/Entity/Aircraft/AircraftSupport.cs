using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DestroyViruses
{
    public class AircraftSupport : EntityBase<AircraftSupport>
    {
        public Aircraft target { get; private set; }
        private AircraftFire mFire;

        public float speed { get; private set; }

        private Vector3 offset = new Vector3(-200, 0, 0);
        private void Awake()
        {
            mFire = gameObject.GetOrAddComponent<AircraftFire>();
        }

        public void Reset(Aircraft target, float speed, Vector2 offset)
        {
            isAlive = true;
            this.target = target;
            this.speed = speed;
            this.offset = offset;
            rectTransform.anchoredPosition3D = new Vector3(0, target.rectTransform.anchoredPosition.y, 0);
        }

        public void ForceRecycle()
        {
            isAlive = false;
            rectTransform.DOAnchorPos3D(new Vector3(UIUtil.width / 2, -100, 0), 1)
                .SetEase(Ease.InQuad).OnComplete(Recycle);
        }

        private void Update()
        {
            if (target == null)
                return;
            if (target.firement.IsFiring)
                mFire.Fire();
            else
                mFire.HoldFire();

            var dist = offset.magnitude;
            var dir = (rectTransform.anchoredPosition - target.rectTransform.GetUIPos()).normalized;
            var tarpos = target.rectTransform.GetUIPos() + dir * dist;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, tarpos, Time.deltaTime * 8f * GlobalData.slowDownFactor);
        }
    }
}