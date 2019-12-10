using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class Buff : EntityBase<Buff>
    {
        public DOTweenAnimation flickerAni;
        public Image icon;

        protected float mSpeed;
        protected float mCD;

        public int buffID { get; private set; }

        public void Reset(int buffID, Vector2 position, float speed)
        {
            this.buffID = buffID;
            mSpeed = speed;
            var t = TableBuff.Get(buffID);
            icon.SetSprite(t.icon);
            mCD = t.effectDuration;
            rectTransform.anchoredPosition = position;
            isAlive = true;
            icon.DOKill();
            icon.SetAlpha(1);
        }

        public void ForceRecycle()
        {
            Recycle();
            isAlive = false;
            icon.DOKill();
        }

        private void Update()
        {
            if (GameUtil.isFrozen)
            {
                return;
            }

            Vector2 uiPos = UIUtil.GetUIPos(rectTransform);
            var radius = 30;
            var flickerTime = 3f;

            if (uiPos.y >= UIUtil.height + radius)
            {
                mSpeed = -Mathf.Abs(mSpeed);
            }
            else if (uiPos.y <= -radius)
            {
                rectTransform.anchoredPosition = Vector2.up * (radius + UIUtil.height);
            }

            var mLastCD = mCD;
            mCD = this.UpdateCD(mCD);
            if (mCD <= 0 && isAlive)
            {
                ForceRecycle();
            }
            else if (mCD <= flickerTime && mLastCD > flickerTime)
            {
                icon.DOFade(0, 0.4f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            }

            rectTransform.anchoredPosition += Vector2.up * mSpeed * Time.deltaTime;
        }
    }
}