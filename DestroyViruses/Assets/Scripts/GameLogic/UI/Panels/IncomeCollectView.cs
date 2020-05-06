using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class IncomeCollectView : ViewBase
    {
        public Text normal;
        public Text rewards;

        protected override void OnOpen()
        {
            base.OnOpen();
            Refresh();
        }

        protected override void OnClose()
        {
            base.OnClose();
            AudioManager.PlaySound("button_normal");
        }

        private void Refresh()
        {
            normal.text = D.I.coinIncomeTotal.KMB();
            rewards.text = (D.I.coinIncomeTotal * 2).KMB();
        }

        private void OnClickReceive()
        { 
            PlayCollectEffect();
            D.I.TakeIncomeCoins();
            Close();
        }

        private void OnClickReward()
        {
            AdProxy.Ins.ShowAd(()=>
            {
                PlayCollectEffect();
                D.I.TakeIncomeCoins(2);
                Close();
                Analytics.Event.Advertising("offline_reward");
            }, ()=>
            { 
                Toast.Show(LTKey.AD_PLAY_FAILED.LT());
            });
        }

        private void PlayCollectEffect()
        {
            var uiCoinCount = (int)(D.I.coinIncomeTotal / (CT.table.coinIncomeRefreshCD * D.I.coinIncome));
            var pos = GetComponent<RectTransform>().GetUIPos();
            Coin.CreateGroup(pos, UIUtil.COIN_POS, uiCoinCount);
            AudioManager.PlaySound("collect_coin");
        }
    }
}