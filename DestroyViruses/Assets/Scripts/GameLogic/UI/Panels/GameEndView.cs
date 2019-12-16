using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class GameEndView : ViewBase
    {
        public RectTransform coinTransform;
        public Button receiveBtns;
        public Text coinText;
        public RadioObjects winLoseRadio;
        public Button gameEndBonusBtn;

        private void Awake()
        {
            receiveBtns.OnClickAsObservable().Subscribe(_ => OnClickReceive());
            gameEndBonusBtn.OnClick(OnClickBonus);
        }

        private void OnClickReceive()
        {
            D.I.AddCoin((int)D.I.battleGetCoin);
            GameEnd();
        }

        private void OnClickBonus()
        {
            if (!AdProxy.Ins.ShowAd("revive_interstitial"))
            {
                //TODO:TOAST
                return;
            }

            D.I.AddCoin((int)D.I.battleGetCoin * Random.Range(2, 10));
            GameEnd();
        }

        private void GameEnd()
        {
            Close();
            StateManager.ChangeState<MainState>();
            // 注意放到最后
            if (!Mathf.Approximately(D.I.battleGetCoin, 0))
            {
                Coin.CreateGroup(coinTransform.GetUIPos(), UIUtil.COIN_POS, 20);
            }
        }

        protected override void OnOpen()
        {
            coinText.text = D.I.battleGetCoin.KMB();
            winLoseRadio.Radio(!D.I.gameEndWin);
            mRectTransform.localScale = Vector3.zero;
            mRectTransform.DOScale(1, 0.25f).SetDelay(1f);

            var aircraft = EntityManager.GetAll<Aircraft>().First() as Aircraft;
            if (D.I.gameEndWin)
            {
                aircraft.anima.PlayFlyAway();
            }
            else
            {
                aircraft.anima.PlayCrash();
            }
        }
    }
}