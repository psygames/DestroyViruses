using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class DrawBenchView : ViewBase
    {
        public RectTransform numLoop;
        public RectTransform stickDot;
        public RectTransform stick;
        public ButtonPro receiveBtn;
        public ButtonPro closeBtn;

        private bool mNumLoopStart;
        private float maxSpeed = 5000;
        private float accSpeed = 3000;
        private float decSpeed = -2000;
        private float speed = 0;
        private float mCD = 0;
        private float posY = 0;

        protected override void OnOpen()
        {
            base.OnOpen();

            receiveBtn.gameObject.SetActive(true);
            closeBtn.gameObject.SetActive(false);
            numLoop.anchoredPosition = Vector2.zero;
            stickDot.anchoredPosition = new Vector2(0, 175);
            stick.localScale = Vector3.one;
            receiveBtn.SetBtnGrey(false);
            mNumLoopStart = false;
            speed = 0;
            posY = 0;
            lastPosY = 0;
            mCD = 0;
        }

        private void OnClickReceive()
        {
            if (receiveBtn.isGrey)
                return;

            AdProxy.Ins.ShowAd("mystical_reward", () =>
            {
                receiveBtn.SetBtnGrey(true);
                AnimaStart();
            }, () =>
            {
                receiveBtn.gameObject.SetActive(false);
                closeBtn.gameObject.SetActive(true);
                Toast.Show(LTKey.AD_PLAY_FAILED.LT());
            });
        }

        private void OnClickClose()
        {
            Close();
            UIManager.Open<GameEndView>(UILayer.Top);
        }

        private void AnimaStart()
        {
            mCD = Random.Range(4f, 6f);
            stickDot.DOAnchorPos3DY(-175, 0.2f);
            stick.DOScaleY(-1, 0.2f).OnComplete(() =>
            {
                mNumLoopStart = true;
            });
        }

        private void AnimaEnd()
        {
            mNumLoopStart = false;
            var num = Mathf.RoundToInt(numLoop.anchoredPosition.y / 300);
            numLoop.DOAnchorPos3DY(num * 300, 0.1f).OnComplete(() =>
            {
                D.I.GameEndReceive(num + 1);
                this.DelayDo(1.5f, () =>
                {
                    GameEnd();
                });
            });
        }


        private void GameEnd()
        {
            AudioManager.PlaySound("collect_coin");
            Close();
            StateManager.ChangeState<MainState>();
            // 注意放到最后
            if (!Mathf.Approximately(D.I.battleGetCoin, 0))
            {
                Coin.CreateGroup(UIUtil.center + Vector2.down * 100, UIUtil.COIN_POS, 20);
            }
        }

        private float lastPosY = 0;
        private void Update()
        {
            if (!mNumLoopStart)
                return;
            mCD = this.UpdateCD(mCD);
            if (mCD > 0)
            {
                speed = Mathf.Min(maxSpeed, speed + accSpeed * Time.deltaTime);
            }
            else if (mCD <= 0)
            {
                speed = Mathf.Max(0, speed + decSpeed * Time.deltaTime);
            }
            posY += speed * Time.deltaTime;
            numLoop.anchoredPosition = new Vector2(0, posY % 3000);
            if (posY - lastPosY >= 300)
            {
                AudioManager.PlaySound("button_grey");
                lastPosY = posY;
            }

            if (mCD <= 0 && speed <= 0)
            {
                AnimaEnd();
            }
        }
    }
}