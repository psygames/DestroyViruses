using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class ExchangeView : ViewBase
    {
        public ButtonPro addBtn;
        public ButtonPro subBtn;
        public ButtonPro exchangeBtn;
        public Image costIcon;
        public Text costText;
        public Image gainIcon;
        public Text gainText;

        private float mCurrent;

        protected override void OnOpen()
        {
            base.OnOpen();
            mCurrent = 0;
            Refresh();
        }

        protected override void OnClose()
        {
            base.OnClose();
            AudioManager.PlaySound("button_normal");
        }

        private void Refresh()
        {
            costText.text = mCurrent.KMB();
            gainText.text = (mCurrent * FormulaUtil.Expresso(CT.table.formulaArgsCoinExchange)).KMB();

            subBtn.SetBtnGrey(mCurrent <= 0);
            addBtn.SetBtnGrey(mCurrent >= D.I.diamond);

            exchangeBtn.SetBtnGrey(mCurrent <= 0);
        }

        private void OnClickAdd()
        {
            if (D.I.diamond <= 0)
            {
                Toast.Show(LTKey.UPGRADE_LACK_OF_DIAMOND.LT());
                UIManager.Open<CoinView>();
                Close();
                return;
            }

            mCurrent = Mathf.Min(D.I.diamond, mCurrent + Mathf.Max(1, D.I.diamond * 0.1f));
            Refresh();
        }

        private void OnClickSub()
        {
            mCurrent = Mathf.Max(0, mCurrent - Mathf.Max(1, D.I.diamond * 0.1f));
            Refresh();
        }

        private void OnClickExchange()
        {
            if (mCurrent > 0)
            {
                D.I.ExchangeCoin(mCurrent);
                mCurrent = 0;
                Refresh();
                Coin.CreateGroup(UIUtil.center, UIUtil.COIN_POS, 10);
                Close();
            }
            else
            {
                Toast.Show(LTKey.EXCHANGE_DIAMOND_NOT_ENOUGH.LT());
            }
        }
    }
}