using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class EnergyView : ViewBase
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
            gainText.text = (mCurrent * CT.table.energyExchange).ToString();

            subBtn.SetBtnGrey(mCurrent <= 0);
            addBtn.SetBtnGrey(mCurrent >= D.I.diamond || IsEnergyFull());

            exchangeBtn.SetBtnGrey(mCurrent <= 0);
        }

        private bool IsEnergyFull()
        {
            if (mCurrent * CT.table.energyExchange >= D.I.maxEnergy - D.I.energy)
            {
                return true;
            }
            return false;
        }

        private void OnClickAdd()
        {
            if (IsEnergyFull())
            {
                Toast.Show(LTKey.ENERGY_EXCHANGE_ENERGY_WILL_BE_OVERFLOW.LT());
                return;
            }

            mCurrent = Mathf.Min(D.I.diamond, mCurrent + 1);
            Refresh();
        }

        private void OnClickSub()
        {
            mCurrent = Mathf.Max(0, mCurrent - 1);
            Refresh();
        }

        private void OnClickExchange()
        {
            if (mCurrent > 0)
            {
                D.I.ExchangeEnergy(mCurrent);
                mCurrent = 0;
                Refresh();
                Close();
            }
            else
            {
                Toast.Show(LTKey.EXCHANGE_DIAMOND_NOT_ENOUGH.LT());
            }
        }
    }
}