using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public class AircraftFire : MonoBehaviour
    {
        public Transform fireTransform;

        const float fireSoundCD = 6;

        private float mFireOnceDuration;
        private float mFireOnceBullets;
        private float mFireOnceCD;
        private float mFirePower;
        private float mFireSpeed;

        private float mFireSoundCD;

        public bool IsFiring { get; private set; }

        private void Awake()
        {
            if (fireTransform == null)
                fireTransform = transform.Find("fireTransform");
            if (fireTransform == null)
                Debug.LogError("fireTransform is null, please take care of this!");
        }

        private void FireOnce()
        {
            mFirePower = FormulaUtil.FirePower(GameDataManager.Instance.firePowerLevel);
            mFireSpeed = FormulaUtil.FireSpeed(GameDataManager.Instance.fireSpeedLevel);
            mFireOnceDuration = Bullet.BULLET_HEIGH / Bullet.BULLET_SPEED;
            mFireOnceBullets = Mathf.CeilToInt(mFireSpeed * mFireOnceDuration);
            mFireOnceBullets = Mathf.Max(mFireOnceBullets, ConstTable.table.bulletMaxCount);
            mFireOnceDuration = mFireOnceBullets / mFireSpeed;

            for (int i = 0; i < mFireOnceBullets; i++)
            {
                var x = Bullet.BULLET_WIDTH * (i - (mFireOnceBullets - 1) * 0.5f);
                var bullet = Bullet.Create();
                bullet.Reset(UIUtil.GetUIPos(fireTransform), x, mFirePower);
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
            mFireSoundCD = this.UpdateCD(mFireSoundCD);
            if (IsFiring)
            {
                if (mFireOnceCD <= 0)
                {
                    FireOnce();
                    mFireOnceCD = mFireOnceDuration;
                }

                AudioManager.Instance.PlayFireMusic("Sounds/hit");
            }
            else
            {
                AudioManager.Instance.StopFireMusic();
            }
        }
    }
}