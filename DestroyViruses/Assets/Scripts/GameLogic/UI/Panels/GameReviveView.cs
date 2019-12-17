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

        private void Update()
        {
            countDownText.text = Mathf.CeilToInt(mCountDown).ToString();
            mCountDown = this.UpdateCD(mCountDown);
            if (mCountDown <= 0)
            {
                var mode = GameModeManager.Instance.currentMode as LevelMode;
                mode?.GiveUpRevive();
                Close();
            }
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
            mCountDown -= 1;
        }
    }
}