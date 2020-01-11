using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnibusEvent;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class WeaponMissileBullet : WeaponBulletBase
    {
        private VirusBase mTarget = null;

        public override void Reset(Vector2 position, Vector2 direction, float damage, float[] effects, string explosionSound)
        {
            mTarget = null;
            rectTransform.localRotation = Quaternion.identity;
            DOTween.To(() => speed, _ => speed = _, effects[0], 1f).SetEase(Ease.InQuart).SetDelay(0.5f);
            base.Reset(position, direction, damage, effects, explosionSound);
        }

        void UpdateDirection()
        {
            if (mTarget == null)
            {
                mTarget = EntityManager.GetAll<VirusBase>().Min(a => (a as VirusBase).hp) as VirusBase;
                return;
            }

            if (!mTarget.isAlive)
                return;

            var fromRot = Quaternion.FromToRotation(Vector3.up, direction);
            var toRot = Quaternion.FromToRotation(Vector3.up, (mTarget.position - position).normalized);
            var maxAngle = effects[2] * Time.deltaTime * GlobalData.slowDownFactor;
            var tarRot = Quaternion.RotateTowards(fromRot, toRot, maxAngle);
            direction = tarRot * Vector3.up;
            rectTransform.localRotation = tarRot;
        }

        protected override void Update()
        {
            if (GameUtil.isFrozen)
                return;

            UpdateDirection();
            base.Update();
        }

        protected override void OnHit(VirusBase virus)
        {
            base.OnHit(virus);
            var hitPos = position + direction * 50;
            AreaHit(hitPos, effects[1] * 0.5f, (_vv) =>
            {
                Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, _vv, damage));
            });
            ExplosionWeaponMissileBullet.Create().Reset(hitPos, effects[1], explosionSound);
            ForceRecycle();
        }
    }
}