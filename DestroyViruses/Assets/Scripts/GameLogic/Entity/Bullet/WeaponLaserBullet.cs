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

        public override void Reset(Vector2 position, Vector2 direction, float damage, float[] effects, string explosionSound)
        {
            base.Reset(position, direction, damage, effects, explosionSound);

            rectTransform.anchoredPosition = position;
            rectTransform.localScale = new Vector3(0, 0, 1);
            rectTransform.DOScale(new Vector3(effects[1] / baseWidth, 1, 1), 0.4f).OnComplete(() =>
            {
                ForceRecycle();
            });
            AudioManager.Instance.PlaySound(explosionSound);
            this.DelayDo(0.2f, CheckHit);
        }

        private void CheckHit()
        {
            foreach (var _v in EntityManager.GetAll<VirusBase>())
            {
                var virus = _v as VirusBase;
                if (!virus.isAlive || virus.isInvincible)
                    continue;
                if (virus.position.y > position.y
                    && Mathf.Abs(virus.position.x - position.x) < effects[1] * 0.5f + virus.radius)
                {
                    Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, virus, damage));
                }
            }
        }

        protected override void Update()
        {
            if (GameUtil.isFrozen)
                return;
        }
    }
}