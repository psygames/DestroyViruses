using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnibusEvent;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class WeaponRailgunBullet : WeaponBulletBase
    {
        public override void Reset(Vector2 position, Vector2 direction, float damage, float[] effects)
        {
            mSpeed = effects[0];
            base.Reset(position, direction, damage, effects);
        }

        protected override void OnHit(VirusBase virus)
        {
            base.OnHit(virus);
            var hitPos = position + direction;
            AreaHit(hitPos, mEffects[1] * 0.5f, (_vv) =>
            {
                _vv.Stun(mEffects[2]);
                Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, _vv, mDamage));
            });
            ExplosionWeaponRailgunBullet.Create().Reset(hitPos, mEffects[1]);
            ForceRecycle();
        }
    }
}