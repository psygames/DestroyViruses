using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
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
            mCurrent = 0;
            Refresh();
        }

        private void Refresh()
        {
            costText.text = mCurrent.KMB();
            var _cur = Mathf.Max(mCurrent, 1);
            gainText.text = FormulaUtil.CoinExchangeFix(_cur, D.I.coinValue).KMB();

            if (_cur <= 1)
            {
                subBtn.SetBtnGrey(true);
            }
            if (mCurrent >= D.I.diamond)
            {
                addBtn.SetBtnGrey(true);
            }

            exchangeBtn.SetBtnGrey(mCurrent <= 0 || mCurrent >= D.I.diamond);
        }

        private void OnDownAdd()
        {

        }

        private void OnUpAdd()
        {

        }

        private void OnDownSub()
        {

        }

        private void OnUpSub()
        {

        }

        private void OnClickExchange()
        {

        }
    }
}