using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class Bullet : EntityBase<Bullet>
    {
        public const float BULLET_SPEED = 3500;  // px/s
        public const float BULLET_HEIGH = 200;   // px
        public const float BULLET_WIDTH = 30;   // px

        public float bornCD = 0.03f;

        protected float mDamage;

        public void Reset(Vector2 position, float offsetX, float damage)
        {
            mDamage = damage;
            rectTransform.anchoredPosition = position;
            rectTransform.DOAnchorPos3DX(position.x + offsetX, bornCD);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == TagUtil.Virus)
            {
                NotifyHit(collider.GetComponent<VirusBase>());
                PlayExplosion();
                Recycle();
            }
        }

        private void NotifyHit(VirusBase virus)
        {
            Unibus.Dispatch(EventVirus.Get(EventVirus.ActionType.HIT, virus, mDamage));
        }

        static float sNextPlayTime = 0;
        private void PlayExplosion()
        {
            if (Time.time >= sNextPlayTime)
            {
                Explosion.Create().rectTransform.anchoredPosition = transform.GetUIPos() + Vector2.up * 10;
                sNextPlayTime = Time.time + 0.1f;
            }
        }

        private void Update()
        {
            Vector2 uiPos = UIUtil.GetUIPos(rectTransform);
            if (uiPos.y > UIUtil.height + BULLET_HEIGH
                || uiPos.x > UIUtil.width + BULLET_WIDTH || uiPos.x < -BULLET_WIDTH)
            {
                Recycle();
            }

            rectTransform.anchoredPosition += Vector2.up * BULLET_SPEED * Time.deltaTime;
        }
    }
}