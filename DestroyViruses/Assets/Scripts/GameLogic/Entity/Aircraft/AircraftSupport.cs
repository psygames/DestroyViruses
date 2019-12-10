using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void Reset(Aircraft target, float speed,Vector2 offset)
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
            Recycle();
        }

        private void Update()
        {
            if (target == null)
                return;
            if (target.firement.IsFiring)
                mFire.Fire();
            else
                mFire.HoldFire();
            var tarpos = target.rectTransform.anchoredPosition3D + offset;
            rectTransform.anchoredPosition3D = Vector2.MoveTowards(rectTransform.anchoredPosition3D, tarpos, speed * Time.deltaTime);
        }
    }
}