using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusBase : EntityBase
    {
        public float moveSpeed = 100;
        public float hp = 10000;
        public Text hpText;

        public static VirusBase Create()
        {
            return EntityManager.Create<VirusBase>();
        }

        public void Reset(Vector2 position)
        {
            hp = 30000;
            rectTransform.anchoredPosition = position;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == TagUtil.Bullet)
            {
                hp -= 100;
            }
        }

        private float mLastHp = 0;
        private void UpdateHp()
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

        private void UpdatePosition()
        {
            rectTransform.anchoredPosition += Vector2.down * moveSpeed * Time.deltaTime;
            if (UIUtil.GetUIPos(rectTransform).y < -100)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, UIUtil.height);
            }
        }


        private void Update()
        {
            UpdateHp();
            UpdatePosition();
        }
    }
}