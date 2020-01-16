using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class CoinLevelPanel : ViewBase
    {
        public ButtonPro coinValueUpBtn;
        public Text coinValueLevelText;
        public Slider coinValueFill;

        public ButtonPro coinIncomeUpBtn;
        public Text coinIncomeLevelText;
        public Slider coinIncomeFill;

        private void OnClickCoinValueUp()
        {
            if (coinValueUpBtn.isGrey)
                return;
            D.I.CoinValueLevelUp();
        }

        private void OnClickCoinIncomeUp()
        {
            if (coinIncomeUpBtn.isGrey)
                return;
            D.I.CoinIncomeLevelUp();
        }

        public void SetData()
        {
            coinValueUpBtn.Set4LevelUp(D.I.coinValueUpCost, D.I.isCoinValueLevelMax);
            coinValueLevelText.text = $"{LTKey.COIN_VALUE.LT()} {LTKey.LEVEL_DOT.LT()}{D.I.coinValueLevel}";
            coinValueFill.value = 1f * D.I.coinValueLevel / D.I.coinValueMaxLevel;

            coinIncomeUpBtn.Set4LevelUp(D.I.coinIncomeUpCost, D.I.isCoinValueLevelMax);
            coinIncomeLevelText.text = $"{LTKey.COIN_INCOME.LT()} {LTKey.LEVEL_DOT.LT()}{D.I.coinIncomeLevel}";
            coinIncomeFill.value = 1f * D.I.coinIncomeLevel / D.I.coinIncomeMaxLevel;
        }
    }
}