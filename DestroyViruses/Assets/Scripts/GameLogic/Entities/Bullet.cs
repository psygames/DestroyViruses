using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DestroyViruses
{
    public class Bullet : EntityBase
    {
        public const float BULLET_SPEED = 3500;  // px/s
        public const float BULLET_HEIGH = 200;   // px
        public const float BULLET_WIDTH = 30;   // px

        public float bornCD = 0.03f;

        public static Bullet Create()
        {
            return EntityManager.Create<Bullet>();
        }

        public void Reset(Vector2 position, float offsetX)
        {
            rectTransform.anchoredPosition = position;
            rectTransform.DOAnchorPos3DX(position.x + offsetX, bornCD);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == TagUtil.ScreenEdge)
            {
                Recycle();
            }
            else if (collider.tag == TagUtil.Virus)
            {
                PlayBomb();
                Recycle();
            }
        }

        private void PlayBomb()
        {
            Debug.LogError("play bomb");
        }

        private void Update()
        {
            rectTransform.anchoredPosition += Vector2.up * BULLET_SPEED * Time.deltaTime;
            if (UIUtil.GetUIPos(rectTransform).y > UIUtil.height + BULLET_HEIGH)
            {
                Recycle();
            }
        }
    }
}