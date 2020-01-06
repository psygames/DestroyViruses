using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnibusEvent;
using UnityEngine.UI;
using System;

namespace DestroyViruses
{
    public class WeaponBulletBase : EntityBase
    {
        protected int mId;
        protected float mDamage;
        protected float mSpeed;
        protected float[] mEffects = new float[5];
        protected Vector2 mSize = Vector2.one * 100;

        public Vector2 position { get; protected set; }
        public Vector2 direction { get; protected set; }

        public virtual void Reset(Vector2 position, Vector2 direction, float damage, float[] effects)
        {
            mDamage = damage;
            Array.Copy(effects, mEffects, mEffects.Length);
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
            if (position.y > UIUtil.height + mSize.y || position.y < -mSize.y
                || position.x < -mSize.x || position.x > UIUtil.width + mSize.x)
            {
                ForceRecycle();
            }

            this.position += direction * mSpeed * GlobalData.slowDownFactor * Time.deltaTime;
            rectTransform.anchoredPosition = position;
        }

        protected float GetDist(Vector2 hitPos, VirusBase virus)
        {
            return Mathf.Max(0, (hitPos - virus.position).magnitude - virus.radius);
        }

        protected virtual void Update()
        {
            if (GameUtil.isFrozen)
                return;

            UpdatePosition();
        }

        protected virtual void ForceRecycle()
        {
            isAlive = false;
            Recycle();
        }
    }
}