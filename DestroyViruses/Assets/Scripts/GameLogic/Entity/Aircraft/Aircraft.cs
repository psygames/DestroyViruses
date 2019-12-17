using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class Aircraft : EntityBase<Aircraft>
    {
        public AircraftMovement movement { get; private set; }
        public AircraftFire firement { get; private set; }
        public AircraftAnimation anima { get; private set; }

        private AircraftInputHandle mInputHandle;
        private AircraftSupport mSupport;
        private RectTransform mHeadRoot;

        public Vector2 headPosition { get { return rectTransform.anchoredPosition + mHeadRoot.anchoredPosition; } }
        public new Collider2D collider2D { get; private set; }

        private void Awake()
        {
            mInputHandle = gameObject.GetOrAddComponent<AircraftInputHandle>();
            movement = gameObject.GetOrAddComponent<AircraftMovement>();
            firement = gameObject.GetOrAddComponent<AircraftFire>();
            anima = gameObject.GetOrAddComponent<AircraftAnimation>();
            mInputHandle.onFire = firement.Fire;
            mInputHandle.onHoldFire = firement.HoldFire;
            mInputHandle.onMove = movement.Move;
            collider2D = GetComponent<Collider2D>();

            if (mHeadRoot == null)
                mHeadRoot = transform.Find("headRoot")?.GetComponent<RectTransform>();
            if (mHeadRoot == null)
                Debug.LogError("headRoot is null, please take care of this!");
        }

        public void Reset()
        {
            rectTransform.anchoredPosition3D = new Vector3(UIUtil.width * 0.5f, 600, 0);
            rectTransform.localScale = Vector3.one;
            anima.StopAll();
        }

        public void Revive()
        {
            float reviveDura = 3;
            collider2D.enabled = false;
            anima.PlayInvincible(reviveDura);
            this.DelayDo(reviveDura, () => { collider2D.enabled = true; });
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
                    var buff = BuffProxy.Ins.GetBuff("support");
                    mSupport.Reset(this, buff.param1, buff.param2);
                }
                else if (!BuffProxy.Ins.Has_Effect_Support && mSupport != null && mSupport.isAlive)
                {
                    mSupport.ForceRecycle();
                }
            }
        }
    }
}