using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class GameReviveView : ViewBase
    {
        public Text countDownText;

        private float mCountDown = 0;

        protected override void OnOpen()
        {
            mCountDown = ConstTable.table.reviveCountDown;
        }

        private int mLast = -1;
        private void Update()
        {
            mCountDown = this.UpdateCD(mCountDown);

            int cur = Mathf.CeilToInt(mCountDown);
            if (mLast != cur)
            {
                countDownText.text = cur.ToString();
                PlayAni();
                mLast = cur;
            }

            if (mCountDown <= 0)
            {
                var mode = GameModeManager.Instance.currentMode as LevelMode;
                mode?.GiveUpRevive();
                Close();
            }
        }

        private void PlayAni()
        {
            countDownText.rectTransform.DOKill();
            countDownText.rectTransform.localScale = new Vector3(-1, 1, 1);
            countDownText.rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        }

        private void OnClickRevive()
        {
            if (!AdProxy.Ins.ShowAd("revive"))
            {
                Toast.Show("广告播放失败");
            }
            var mode = GameModeManager.Instance.currentMode as LevelMode;
            mode?.DoRevive();
            Close();
        }

        private void OnClickFastForward()
        {
            mCountDown = (int)mCountDown;
        }
    }
}