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
            Unibus.Subscribe<EventVirus>(this, OnEventAciton);
        }

        private void OnDisable()
        {
            Unibus.Unsubscribe<EventVirus>(this, OnEventAciton);
        }

        public virtual void Reset(float hp, int size, Vector2 pos, Vector2 direction)
        {
            isAlive = true;
            mHp = hp;
            mSize = size;
            rectTransform.anchoredPosition = pos;
            this.direction = ;
        }

        private void OnEventAciton(EventVirus evt)
        {
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

            var hp = mHp * 0.5f;
            var size = mSize - 1;
            var pos = transform.GetUIPos();
            var a = Create();
            a.Reset(hp, pos, direction);

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