using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnibusEvent;
using UniRx;

namespace DestroyViruses
{
    public class VirusBase : EntityBase
    {
        public const float radius = 100f;

        public Text hpText;
        public Image glowImage;

        protected float mHp;
        protected float mHpTotal;
        protected Vector2 mHpRange;
        protected int mSize;
        protected float mSpeed;

        protected Vector2 mDirection;
        protected Vector2 mPosition;
        protected Vector2 mShakeOffset;


        private void OnEnable()
        {
            this.BindUntilDisable<EventBullet>(OnEventBullet);
        }

        public virtual void Reset(float hp, int size, float speed, Vector2 pos, Vector2 direction, Vector2 hpRange)
        {
            isAlive = true;
            mLastColorIndex = -1;
            mLastHp = -1;
            mHpTotal = hp;
            mHp = hp;
            mSize = size;
            mSpeed = speed;
            mHpRange = hpRange;
            mDirection = direction;
            mPosition = pos;
            rectTransform.anchoredPosition = pos;
            mShakeOffset = Vector2.zero;
            rectTransform.localScale = Vector3.one * GetSizeScale(size);
        }

        private float GetSizeScale(int size)
        {
            return 1 / Mathf.Sqrt(5 - size);
        }

        private void OnEventBullet(EventBullet evt)
        {
            if (evt.target != this)
                return;

            if (evt.action == EventBullet.Action.HIT)
            {
                BeHit(evt.damage);
                mHp = Mathf.Max(0, mHp - evt.damage);
                if (mHp <= 0)
                {
                    BeDead();
                }
            }
        }

        private void BeHit(float damage)
        {
            Unibus.Dispatch(EventVirus.Get(EventVirus.Action.BE_HIT, this, damage));
            PlayHit();
        }


        private void PlayHit()
        {
            mShakeOffset = Random.insideUnitCircle * Random.Range(0, 100);
        }


        private void UpdateShake()
        {

        }

        private void BeDead()
        {
            isAlive = false;
            Unibus.Dispatch(EventVirus.Get(EventVirus.Action.DEAD, this, mHpTotal));
            Recycle();
            PlayDead();
            Divide();
        }

        private void PlayDead()
        {
            int colorCount = 4;
            int type = (int)((mHpTotal - mHpRange.x) / (mHpRange.y - mHpRange.x) * colorCount);
            type = colorCount - Mathf.Clamp(type, 0, colorCount - 1);
            ExplosionVirus.Create().Reset(rectTransform.anchoredPosition, type);
        }

        private void Divide()
        {
            if (mSize <= 1)
                return;

            // 血量较少的不产生分裂
            if ((mHpTotal - mHpRange.x) < (mHpRange.y - mHpRange.x) * 0.2f)
                return;

            var hp = mHpTotal * 0.5f;
            var size = mSize - 1;
            var pos = transform.GetUIPos();

            Vector2 dirA = Quaternion.AngleAxis(Random.Range(-60, -80), Vector3.forward) * Vector2.up;
            Create().Reset(hp, size, mSpeed, pos + dirA * radius * GetSizeScale(size), dirA, mHpRange);

            Vector2 dirB = Quaternion.AngleAxis(Random.Range(60, 80), Vector3.forward) * Vector2.up;
            Create().Reset(hp, size, mSpeed, pos + dirB * radius * GetSizeScale(size), dirB, mHpRange);
        }

        protected VirusBase Create()
        {
            return EntityManager.Create(GetType()) as VirusBase;
        }

        private float mLastHp = -1;
        protected virtual void UpdateHp()
        {
            if (mLastHp != mHp)
            {
                hpText.text = mHp.KMB();
                mLastHp = mHp;
            }
        }

        protected virtual void UpdatePosition()
        {
            var uiPos = UIUtil.GetUIPos(rectTransform);
            if (uiPos.y < -radius)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, UIUtil.height);
            }
            else if (uiPos.y > UIUtil.height - radius)
            {
                mDirection = new Vector2(mDirection.x, -Mathf.Abs(mDirection.y));
            }
            if (uiPos.x < radius)
            {
                mDirection = new Vector2(Mathf.Abs(mDirection.x), mDirection.y);
            }
            else if (uiPos.x > UIUtil.width - radius)
            {
                mDirection = new Vector2(-Mathf.Abs(mDirection.x), mDirection.y);
            }
            rectTransform.anchoredPosition += mDirection * mSpeed * Time.deltaTime;
        }

        private int mLastColorIndex = -1;
        protected virtual void UpdateColor()
        {
            int colorCount = 9;
            int index = (int)((mHp - mHpRange.x) / (mHpRange.y - mHpRange.x) * colorCount);
            index = colorCount - Mathf.Clamp(index, 0, colorCount - 1) - 1;
            if (mLastColorIndex != index)
            {
                OnColorChanged(index);
                mLastColorIndex = index;
            }
        }

        protected virtual void OnColorChanged(int index)
        {
            glowImage.SetSprite($"virus_glow_circle_{index}");
        }

        protected virtual void Update()
        {
            if (GameUtil.isFrozen)
            {
                return;
            }

            UpdateHp();
            UpdatePosition();
            UpdateColor();
        }
    }
}