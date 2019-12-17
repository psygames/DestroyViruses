using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;
using System.IO;

namespace DestroyViruses
{
    public class DailySignView : ViewBase
    {
        public DailySignItem[] items;

        public ButtonPro receiveBtn;
        public ButtonPro receiveBtn2;

        protected override void OnOpen()
        {
            base.OnOpen();
            for (int i = 0; i < items.Length; i++)
            {
                items[i].SetData(i + 1);
            }
        }

        private void OnClickReceive()
        {
            if (D.I.CanDailySign())
                D.I.DailySign();
            else
                Toast.Show("今日已签到");
        }

        private void OnClickReceive2()
        {
            if (D.I.CanDailySign())
            {
                if (AdProxy.Ins.ShowAd("daily_sign"))
                {
                    D.I.DailySign();
                }
                else
                {
                    Toast.Show("广告播放失败");
                }
            }
            else
            {
                Toast.Show("今日已签到");
            }
        }
    }
}