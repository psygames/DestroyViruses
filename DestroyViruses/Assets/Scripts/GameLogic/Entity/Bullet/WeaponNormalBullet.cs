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
        private int mHitCount = 0;

        public override void Reset(Vector2 position, Vector2 direction, float damage, float[] effects)
        {
            speed = effects[0];
            mSize = Vector2.one * effects[2];
            base.Reset(position, direction, damage, effects);
            rectTransform.localScale = Vector3.one * effects[2] * 0.01f;
            mHitCount = (int)effects[3];
        }

        protected override void OnHit(VirusBase virus)
        {
            base.OnHit(virus);
            if (virus.isAlive && !virus.isInvincible)
            {
                Vector2 cDir = virus.position - position;
                Vector2 dir = (cDir + virus.direction).normalized;
                virus.SetDirection(dir);
                Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, virus, damage));
            }

            mHitCount--;
            if (mHitCount <= 0)
            {
                ForceRecycle();
            }
        }
    }
}