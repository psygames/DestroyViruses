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
        public ContentGroup day6Group;
        public DailySignItem day7;

        public ButtonPro receiveBtn;
        public ButtonPro receiveBtn2;

        protected override void OnOpen()
        {
            base.OnOpen();
            this.BindUntilDisable<EventGameData>(OnEventGameData);
            RefreshUI();
        }

        private void OnEventGameData(EventGameData evt)
        {
            if (evt.action == EventGameData.Action.DataChange)
            {
                RefreshUI();
            }
        }

        private void RefreshUI()
        {
            day6Group.SetData<DailySignItem, int>(new int[] { 1, 2, 3, 4, 5, 6 },
            (index, item, _data) =>
            {
                item.SetData(_data);
            });
            day7.SetData(7);

            receiveBtn.SetBtnGrey(!D.I.CanDailySign());
            receiveBtn2.SetBtnGrey(!D.I.CanDailySign());
        }

        private void OnClickSign()
        {
            if (D.I.CanDailySign())
            {
                Sign(1);
            }
            else
            {
                Toast.Show(LTKey.DAILY_SIGN_ALREADY_SIGNED.LT());
                Close();
            }
        }

        private void OnClickSignDay(int day)
        {
            if (day == D.I.signDays)
            {
                Sign(1);
            }
        }

        private void OnClickSign2()
        {
            if (D.I.CanDailySign())
            {
                if (AdProxy.Ins.ShowAd("daily_sign"))
                {
                    Sign(2);
                }
                else
                {
                    Toast.Show(LTKey.AD_PLAY_FAILED.LT());
                }
            }
            else
            {
                Toast.Show(LTKey.DAILY_SIGN_ALREADY_SIGNED.LT());
                Close();
            }
        }

        private void Sign(float multiple)
        {
            D.I.DailySign(multiple);
            AudioManager.Instance.PlaySound("revive_count_down");
        }
    }
}