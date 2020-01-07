using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnibusEvent;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class WeaponLaserBullet : WeaponBulletBase
    {
        public const float baseWidth = 200;

        public override void Reset(Vector2 position, Vector2 direction, float damage, float[] effects)
        {
            base.Reset(position, direction, damage, effects);

            rectTransform.anchoredPosition = position;
            rectTransform.localScale = new Vector3(0, 0, 1);
            rectTransform.DOScale(effects[1] / baseWidth, 0.5f).OnComplete(() =>
            {
                ForceRecycle();
            });
        }

        protected override void OnHit(VirusBase virus)
        {
            base.OnHit(virus);
            if (virus.isAlive && !virus.isInvincible)
            {
                Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, virus, mDamage));
            }
        }

        protected override void Update()
        {
            if (GameUtil.isFrozen)
                return;
        }
    }
}