using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class Buff : EntityBase<Buff>
    {
        public DOTweenAnimation flickerAni;
        public Image icon;

        protected Vector2 mDirection;
        protected float mSpeed;
        protected float mCD;

        protected float flickerTime = 3f;
        protected float radius = 30f;
        public int buffID { get; private set; }

        public void Reset(int buffID, Vector2 position, Vector2 direction, float speed)
        {
            this.buffID = buffID;
            mDirection = direction;
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

            var uiPos = UIUtil.GetUIPos(rectTransform);
            if (uiPos.y < radius)
            {
                mDirection = new Vector2(mDirection.x, Mathf.Abs(mDirection.y));
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

            rectTransform.anchoredPosition += mDirection * GlobalData.slowDownFactor * mSpeed * Time.deltaTime;
        }
    }
}