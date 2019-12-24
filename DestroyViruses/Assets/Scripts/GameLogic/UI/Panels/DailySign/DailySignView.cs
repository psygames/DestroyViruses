﻿using System.Collections;
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
                D.I.DailySign(1);
            }
            else
            {
                Toast.Show("今日已签到");
                Close();
            }
        }

        private void OnClickSign2()
        {
            if (D.I.CanDailySign())
            {
                if (AdProxy.Ins.ShowAd("daily_sign"))
                {
                    D.I.DailySign(2);
                }
                else
                {
                    Toast.Show("广告播放失败");
                }
            }
            else
            {
                Toast.Show("今日已签到");
                Close();
            }
        }
    }
}