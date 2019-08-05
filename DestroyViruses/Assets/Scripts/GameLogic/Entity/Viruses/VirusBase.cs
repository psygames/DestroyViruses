using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusBase : EntityBase<VirusBase>
    {
        public float moveSpeed = 200;
        public float hp = 10000;
        public float radius = 100;
        public Text hpText;

        public Vector2 moveDirection { get; protected set; }

        public virtual void Reset(Vector2 position)
        {
            hp = 30000;
            rectTransform.anchoredPosition = position;
            moveDirection = Quaternion.AngleAxis(Random.Range(-80f, 80f), Vector3.forward) * Vector2.down;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == TagUtil.Bullet)
            {
                hp -= 100;
            }
        }

        private float mLastHp = 0;
        protected virtual void UpdateHp()
        {
            if (mLastHp != hp)
            {
                hpText.text = Mathf.CeilToInt(hp).ToString();
                mLastHp = hp;
            }

            if (hp <= 0)
            {
                Recycle();
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
                moveDirection = new Vector2(moveDirection.x, -Mathf.Abs(moveDirection.y));
            }
            if (uiPos.x < radius)
            {
                moveDirection = new Vector2(Mathf.Abs(moveDirection.x), moveDirection.y);
            }
            else if (uiPos.x > UIUtil.width - radius)
            {
                moveDirection = new Vector2(-Mathf.Abs(moveDirection.x), moveDirection.y);
            }
            rectTransform.anchoredPosition += moveDirection * moveSpeed * Time.deltaTime;
        }


        protected virtual void Update()
        {
            UpdateHp();
            UpdatePosition();
        }
    }
}