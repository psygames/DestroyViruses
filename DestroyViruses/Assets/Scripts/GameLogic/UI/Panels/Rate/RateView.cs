using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class RateView : ViewBase
    {
        public ContentGroup stars;
        private int mRateStars;

        protected override void OnOpen()
        {
            base.OnOpen();
            mRateStars = -1;
            Refresh();
        }

        private void Refresh()
        {
            stars.SetData<RateStarItem, bool>(new bool[5], (index, item, data) =>
            {
                item.SetData(index, index <= mRateStars, OnClickStar);
            });
        }

        private void OnClickStar(int index)
        {
            mRateStars = index;
            Refresh();
        }

        private void OnClickSkip()
        {
            Close();
        }

        private void OnClickRate()
        {
            if (mRateStars < 0)
            {
                Toast.Show(LTKey.TAP_STAR_TO_RATE.LT());
                return;
            }

            GameLocalData.Instance.isRateOver = true;
            GameLocalData.Instance.Save();

            if (mRateStars < 4)
            {
                var email = RemoteConfigProxy.Ins.GetValue("feedback_email", "newbrowny123@gmail.com");
                var uri = new System.Uri(string.Format("mailto:{0}?subject={1}", email, Application.productName + " Feedback"));
                Application.OpenURL(uri.AbsoluteUri);
            }
            else
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                Application.OpenURL("market://details?id=" + Application.identifier);
#else
                Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
#endif
            }

            Close();
        }
    }
}