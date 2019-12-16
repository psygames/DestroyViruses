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

        private Tweener tw;
        public void PlayStandby()
        {
            path = new Vector3[13];
            var dist = 20;
            for (int i = 0; i < 13; i++)
            {
                var dir = Quaternion.AngleAxis(360 * i / 10, -Vector3.forward) * Vector3.left;
                path[i] = mRectTransform.anchoredPosition3D + dir * dist;
            }
            path[4] = mRectTransform.anchoredPosition3D;
            path[7] = mRectTransform.anchoredPosition3D;
            path[12] = mRectTransform.anchoredPosition3D;
            tw = mRectTransform.DOLocalPath(path, 10, PathType.CatmullRom, PathMode.Full3D, 5).SetLoops(-1).SetEase(Ease.Linear);
        }

        public void PlayCrash()
        {
            mRectTransform.DOScale(Vector3.zero, 0.3f);
            AudioManager.Instance.PlaySound("Sounds/crash");
            ExplosionCrash.Create().Reset(mRectTransform.anchoredPosition);
        }

        public void PlayFlyAway()
        {
            AudioManager.Instance.PlaySound("Sounds/flyaway");
            mRectTransform.DOAnchorPos3D(new Vector3(UIUtil.width / 2, UIUtil.height + 400, 0), 1).SetEase(Ease.InQuad);
        }

        public void StopAll()
        {
            tw?.Kill(true);
        }

        private void OnDisable()
        {
            StopAll();
        }
    }
}