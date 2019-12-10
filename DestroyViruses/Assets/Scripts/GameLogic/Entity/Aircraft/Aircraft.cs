using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class Aircraft : EntityBase<Aircraft>
    {
        public AircraftMovement movement { get; private set; }
        public AircraftFire firement { get; private set; }
        private AircraftInputHandle mInputHandle;

        private AircraftSupport mSupport;

        private void Awake()
        {
            mInputHandle = gameObject.GetOrAddComponent<AircraftInputHandle>();
            movement = gameObject.GetOrAddComponent<AircraftMovement>();
            firement = gameObject.GetOrAddComponent<AircraftFire>();
            mInputHandle.onFire = firement.Fire;
            mInputHandle.onHoldFire = firement.HoldFire;
            mInputHandle.onMove = movement.Move;
        }

        public void Reset()
        {
            rectTransform.anchoredPosition3D = new Vector3(UIUtil.width * 0.5f, 700, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == TagUtil.Virus)
            {
                UnibusEvent.Unibus.Dispatch(EventAircraft.Get(EventAircraft.Action.Crash));
            }
            else if (collision.tag == TagUtil.Buff)
            {
                var buff = collision.gameObject.GetComponent<Buff>();
                buff.ForceRecycle();
                ProxyManager.GetProxy<BuffProxy>().AddBuff(buff.buffID);
            }
        }

        private void Update()
        {
            if (GameUtil.isFrozen)
            {
                firement.HoldFire();
            }

            if (BuffProxy.Ins != null)
            {
                if (BuffProxy.Ins.Has_Effect_Support && (mSupport == null || !mSupport.isAlive))
                {
                    mSupport = AircraftSupport.Create();
                    mSupport.Reset(this, BuffProxy.Ins.Effect_Support);
                }
                else if (!BuffProxy.Ins.Has_Effect_Support && mSupport != null && mSupport.isAlive)
                {
                    mSupport.ForceRecycle();
                }
            }
        }
    }
}