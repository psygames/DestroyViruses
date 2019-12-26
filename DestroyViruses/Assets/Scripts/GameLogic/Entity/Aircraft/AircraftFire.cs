using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public class AircraftFire : MonoBehaviour
    {
        public Transform fireTransform;
        private float mFireOnceDuration;
        private float mFireOnceBullets;
        private float mFireOnceCD;
        private float mFirePower;
        private float mFireSpeed;

        public bool IsFiring { get; private set; }

        private BuffProxy buffProxy { get { return ProxyManager.GetProxy<BuffProxy>(); } }

        private void Awake()
        {
            if (fireTransform == null)
                fireTransform = transform.Find("fireTransform");
            if (fireTransform == null)
                Debug.LogError("fireTransform is null, please take care of this!");
        }

        private void FireOnce()
        {
            var bulletSpeed = TableFireSpeed.Get(D.I.fireSpeedLevel).bulletSpeed;
            mFirePower = D.I.firePower * buffProxy.Effect_FirePower;
            mFireSpeed = D.I.fireSpeed * buffProxy.Effect_FireSpeed;
            mFireSpeed = Mathf.Min(mFireSpeed, ConstTable.table.maxFireSpeed);
            var _bullets = mFireSpeed * Bullet.BULLET_HEIGH / bulletSpeed;
            mFireOnceBullets = Mathf.RoundToInt(_bullets);
            mFireOnceBullets = Mathf.Max(mFireOnceBullets, 1);
            if (_bullets > 1) bulletSpeed = Bullet.BULLET_HEIGH * mFireSpeed / mFireOnceBullets;
            mFireOnceDuration = mFireOnceBullets / mFireSpeed;

            for (int i = 0; i < mFireOnceBullets; i++)
            {
                var x = Bullet.BULLET_WIDTH * (i - (mFireOnceBullets - 1) * 0.5f);
                var bullet = Bullet.Create();
                bullet.Reset(UIUtil.GetUIPos(fireTransform), x, mFirePower, bulletSpeed);
            }
        }

        public void Fire()
        {
            IsFiring = true;
        }

        public void HoldFire()
        {
            IsFiring = false;
        }

        private void Update()
        {
            mFireOnceCD = this.UpdateCD(mFireOnceCD);
            if (IsFiring)
            {
                if (mFireOnceCD <= 0)
                {
                    FireOnce();
                    mFireOnceCD = mFireOnceDuration;
                }

                AudioManager.Instance.PlayFireMusic("hit");
            }
            else
            {
                AudioManager.Instance.StopFireMusic();
            }
        }
    }
}