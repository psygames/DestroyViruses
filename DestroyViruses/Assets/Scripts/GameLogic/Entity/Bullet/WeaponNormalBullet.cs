using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnibusEvent;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class WeaponNormalBullet : WeaponBulletBase
    {
        public override void Reset(Vector2 position, Vector2 direction, float damage, float[] effects)
        {
            mSpeed = effects[0];
            base.Reset(position, direction, damage, effects);
        }

        protected override void OnHit(VirusBase virus)
        {
            base.OnHit(virus);
            if (virus.isAlive && !virus.isInvincible)
            {
                Vector2 cDir = virus.position - position;
                Vector2 dir = (cDir + virus.direction).normalized;
                virus.SetDirection(dir);
                Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, virus, mDamage));
            }
        }
    }
}