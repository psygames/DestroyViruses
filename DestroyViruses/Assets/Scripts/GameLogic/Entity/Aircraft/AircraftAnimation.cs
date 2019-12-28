using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace DestroyViruses
{
    public class AircraftAnimation : MonoBehaviour
    {
        private RectTransform mRectTransform;
        private Vector3[] path;

        private void Awake()
        {
            mRectTransform = GetComponent<RectTransform>();
        }

        Tweener standByTweener = null;
        public void PlayStandby()
        {
            mRectTransform.anchoredPosition3D = new Vector3(UIUtil.width * 0.5f, 600, 0);
            path = new Vector3[13];
            var dist = 30;
            for (int i = 0; i < 13; i++)
            {
                var dir = Quaternion.AngleAxis(360 * i / 10, -Vector3.forward) * Vector3.left;
                path[i] = mRectTransform.anchoredPosition3D + dir * dist;
            }
            path[4] = mRectTransform.anchoredPosition3D;
            path[7] = mRectTransform.anchoredPosition3D;
            path[12] = mRectTransform.anchoredPosition3D;
            standByTweener = mRectTransform.DOLocalPath(path, 10, PathType.CatmullRom, PathMode.Full3D, 5).SetLoops(-1).SetEase(Ease.Linear);
        }

        public void StopStandBy()
        {
            if (standByTweener != null)
                standByTweener.Kill();
        }

        public void PlayCrash()
        {
            mRectTransform.DOScale(Vector3.zero, 0.3f);
            AudioManager.Instance.PlaySound("crash");
            ExplosionCrash.Create().Reset(mRectTransform.anchoredPosition);
        }

        public void PlayFlyAway()
        {
            AudioManager.Instance.PlaySound("flyaway");
            mRectTransform.DOAnchorPos3D(new Vector3(UIUtil.width * 0.5f, UIUtil.height + 400, 0), 1).SetEase(Ease.InQuad);
        }

        public void PlayHomeIn()
        {
            mRectTransform.anchoredPosition3D = new Vector3(UIUtil.width * 0.5f, 0, 0);
            mRectTransform.localScale = Vector3.one;
            mRectTransform.DOAnchorPos3D(new Vector3(UIUtil.width * 0.5f, 600, 0), 0.3f);
            mRectTransform.DOScale(1.5f, 0.5f).SetDelay(0.3f).OnComplete(() => PlayStandby());
        }

        public void PlayHomeOut()
        {
            mRectTransform.DOScale(1f, 0.5f);
        }

        public void KillAll()
        {
            mRectTransform.DOKill(false);
        }

        public void PlayInvincible(float seconds)
        {
            float flash = 0.25f;
            var canvas = gameObject.GetOrAddComponent<CanvasGroup>();
            canvas.DOFade(0.2f, flash).SetEase(Ease.Linear).SetLoops((int)(seconds / flash), LoopType.Yoyo).OnComplete(() =>
            {
                canvas.alpha = 1;
            });
        }
    }
}