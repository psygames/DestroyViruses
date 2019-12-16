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
            GDM.ins.AddCoin((int)GDM.ins.battleGetCoin);
            GameEnd();
        }

        private void OnClickBonus()
        {
            if (!AdProxy.Ins.ShowAd("revive_interstitial"))
            {
                //TODO:TOAST
                return;
            }

            GDM.ins.AddCoin((int)GDM.ins.battleGetCoin * Random.Range(2, 10));
            GameEnd();
        }

        private void GameEnd()
        {
            Close();
            StateManager.ChangeState<MainState>();
            // 注意放到最后
            if (!Mathf.Approximately(GDM.ins.battleGetCoin, 0))
            {
                Coin.CreateGroup(coinTransform.GetUIPos(), UIUtil.COIN_POS, 20);
            }
        }

        protected override void OnOpen()
        {
            coinText.text = GDM.ins.battleGetCoin.KMB();
            winLoseRadio.Radio(!GDM.ins.gameEndWin);
            mRectTransform.localScale = Vector3.zero;
            mRectTransform.DOScale(1, 0.25f).SetDelay(1f);

            var aircraft = EntityManager.GetAll<Aircraft>().First() as Aircraft;
            if (GDM.ins.gameEndWin)
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