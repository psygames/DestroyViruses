using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class Bullet : EntityBase<Bullet>
    {
        public static float BULLET_SPEED => ConstTable.table.bulletSpeed;
        public static float BULLET_HEIGH => ConstTable.table.bulletVDist;
        public static float BULLET_WIDTH => ConstTable.table.bulletHDist;

        public float bornCD = 0.03f;

        protected float mDamage;

        public void Reset(Vector2 position, float offsetX, float damage)
        {
            mDamage = damage;
            rectTransform.anchoredPosition = position;
            rectTransform.DOAnchorPos3DX(position.x + offsetX, bornCD);
            isAlive = true;
        }

        private void OnTriggerEnter2D(Collider2D _collider)
        {
            if (!isAlive)
                return;

            if (_collider.tag == TagUtil.Virus)
            {
                var virus = _collider.GetComponent<VirusBase>();
                if (virus != null && virus.isAlive)
                {
                    Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, virus, mDamage));
                    PlayExplosion();
                    Recycle();
                    isAlive = false;
                }
            }
        }

        static float sNextPlayTime = 0;
        private void PlayExplosion()
        {
            if (Time.time >= sNextPlayTime)
            {
                ExplosionBullet.Create().rectTransform.anchoredPosition = transform.GetUIPos() + Vector2.up * 10;
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
                isAlive = false;
            }

            rectTransform.anchoredPosition += Vector2.up * BULLET_SPEED * Time.deltaTime;
        }
    }
}