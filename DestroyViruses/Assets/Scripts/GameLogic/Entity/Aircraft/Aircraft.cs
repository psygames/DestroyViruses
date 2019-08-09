using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class Aircraft : EntityBase<Aircraft>
    {
        private AircraftInputHandle mInputHandle;
        private AircraftMovement mMovement;
        private AircraftFire mFire;

        private void Awake()
        {
            mInputHandle = gameObject.GetOrAddComponent<AircraftInputHandle>();
            mMovement = gameObject.GetOrAddComponent<AircraftMovement>();
            mFire = gameObject.GetOrAddComponent<AircraftFire>();
            mInputHandle.onFire = mFire.Fire;
            mInputHandle.onHoldFire = mFire.HoldFire;
            mInputHandle.onMove = mMovement.Move;
        }

        public void Reset()
        {
            rectTransform.anchoredPosition3D = new Vector3(UIUtil.width * 0.5f, 700, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == TagUtil.Virus)
            {
                UnibusEvent.Unibus.Dispatch(EventAircraft.Get(EventAircraft.ActionType.Crash));
            }
        }

        private void Update()
        {
            if (GameUtil.isFrozen)
            {
                mFire.HoldFire();
            }
        }
    }
}