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
        public Text coinText;
        public RadioObjects winLoseRadio;
        public GameObject mysticalBonus;
        public GameObject bonusObj;


        private void OnClickReceive()
        {
            D.I.GameEndReceive();
            GameEnd();
        }

        private void OnClickBonus()
        {
            AdProxy.Ins.ShowAd(() =>
            {
                D.I.GameEndReceive(3);
                AudioManager.PlaySound("collect_coin");
                GameEnd();
                Analytics.Event.Advertising("triple_reward");
            }, () =>
            {
                Toast.Show(LTKey.AD_PLAY_FAILED.LT());
            });
        }

        private void OnClickMysticalBonus()
        {
            Close();
            UIManager.Open<DrawBenchView>(UILayer.Top);
        }

        private void GameEnd()
        {
            Close();
            StateManager.ChangeState<MainState>();
            // 注意放到最后
            if (!Mathf.Approximately(D.I.battleGetCoin, 0))
            {
                Coin.CreateGroup(UIUtil.center + Vector2.down * 100, UIUtil.COIN_POS, 20);
            }
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            coinText.text = D.I.battleGetCoin.KMB();
            winLoseRadio.Radio(!D.I.gameEndWin);
            bonusObj.SetActive(!D.I.noAd);
            mysticalBonus.SetActive(!D.I.noAd && Random.value <= CT.table.mysticalBonusProbability);
        }
    }
}