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
            speed = effects[0];
            base.Reset(position, direction, damage, effects);
        }

        protected override void OnHit(VirusBase virus)
        {
            base.OnHit(virus);
            var hitPos = position + direction;
            AreaHit(hitPos, effects[1] * 0.5f, (_vv) =>
            {
                _vv.Stun(effects[2]);
                Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, _vv, damage));
            });
            ExplosionWeaponRailgunBullet.Create().Reset(hitPos, effects[1]);
            ForceRecycle();
        }
    }
}