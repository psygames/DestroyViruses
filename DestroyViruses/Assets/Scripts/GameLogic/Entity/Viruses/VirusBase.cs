using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnibusEvent;

namespace DestroyViruses
{
    public class VirusBase : EntityBase<VirusBase>
    {
        public const float radius = 100f;

        public Text hpText;

        protected float mHp;
        protected float mHpTotal;
        protected Vector2 mHpRange;
        protected int mSize;
        protected float mSpeed;

        public Vector2 direction { get; protected set; }

        private void OnEnable()
        {
            this.BindUntilDisable<EventVirus>(OnEvent);
        }

        public virtual void Reset(float hp, int size, float speed, Vector2 pos, Vector2 direction)
        {
            isAlive = true;
            mHpTotal = hp;
            mHp = hp;
            mSize = size;
            mSpeed = speed;
            this.direction = direction;
            rectTransform.anchoredPosition = pos;
            rectTransform.localScale = Vector3.one * GetSizeScale(size);
        }

        private float GetSizeScale(int size)
        {
            return 1 / Mathf.Sqrt(5 - size);
        }

        private void OnEvent(EventVirus evt)
        {
            if (evt.virus != this)
                return;

            if (evt.action == EventVirus.ActionType.HIT)
            {
                mHp = Mathf.Max(0, mHp - evt.value);
                if (mHp <= 0)
                {
                    BeDead();
                }
            }
        }

        private void BeDead()
        {
            isAlive = false;
            Recycle();
            PlayDead();
            Divide();
        }

        private void PlayDead()
        {
            //TODO:play dead
        }

        private void Divide()
        {
            if (mSize <= 1)
                return;

            var hp = mHpTotal * 0.5f;
            var size = mSize - 1;
            var pos = transform.GetUIPos();

            Vector2 dirA = Quaternion.AngleAxis(Random.Range(-60, -80), Vector3.forward) * Vector2.up;
            Create().Reset(hp, size, mSpeed, pos + dirA * radius * GetSizeScale(size), dirA);

            Vector2 dirB = Quaternion.AngleAxis(Random.Range(60, 80), Vector3.forward) * Vector2.up;
            Create().Reset(hp, size, mSpeed, pos + dirB * radius * GetSizeScale(size), dirB);
        }


        private float mLastHp = 0;
        protected virtual void UpdateHp()
        {
            if (mLastHp != mHp)
            {
                hpText.text = Mathf.CeilToInt(mHp).ToString();
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
                direction = new Vector2(direction.x, -Mathf.Abs(direction.y));
            }
            if (uiPos.x < radius)
            {
                direction = new Vector2(Mathf.Abs(direction.x), direction.y);
            }
            else if (uiPos.x > UIUtil.width - radius)
            {
                direction = new Vector2(-Mathf.Abs(direction.x), direction.y);
            }
            rectTransform.anchoredPosition += direction * mSpeed * Time.deltaTime;
        }


        protected virtual void Update()
        {
            UpdateHp();
            UpdatePosition();
        }
    }
}