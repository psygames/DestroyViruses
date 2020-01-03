using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnibusEvent;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class WeaponBulletBase : EntityBase
    {
        protected int mId;
        protected float mDamage;
        protected float mSpeed;
        protected float mEffect1;
        protected float mEffect2;
        protected float mEffect3;
        protected Vector2 mSize = Vector2.one * 100;

        public Vector2 position { get; protected set; }
        public Vector2 direction { get; protected set; }

        public void Reset(Vector2 position, Vector2 direction, float damage, float speed, float effect1, float effect2, float effect3)
        {
            mDamage = damage;
            mSpeed = speed;
            mEffect1 = effect1;
            mEffect2 = effect2;
            mEffect3 = effect3;
            this.position = position;
            this.direction = direction;
            isAlive = true;
            Update();
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
                    OnHit(virus);
                }
            }
        }

        protected virtual void OnHit(VirusBase virus)
        {

        }

        protected WeaponBulletBase Create()
        {
            return EntityManager.Create(GetType()) as WeaponBulletBase;
        }

        protected virtual void UpdatePosition()
        {
            if (position.y > UIUtil.height + mSize.y
                || position.x < -mSize.x || position.x > UIUtil.width + mSize.x)
            {
                Recycle();
                isAlive = false;
            }

            this.position += direction * mSpeed * GlobalData.slowDownFactor * Time.deltaTime;
            rectTransform.anchoredPosition = position;
        }

        protected virtual void Update()
        {
            if (GameUtil.isFrozen)
                return;

            UpdatePosition();
        }
    }
}