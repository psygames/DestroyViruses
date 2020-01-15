using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class CoinIncomePanel : MonoBehaviour
    {
        public Button receiveBtn;
        public Image imageFill;
        public Text countText;

        private float mRefreshCD = 0;

        private void Awake()
        {
            receiveBtn?.OnClick(() =>
            {
                var uiCoinCount = (int)(D.I.coinIncomeTotal / (ConstTable.table.coinIncomeRefreshCD * D.I.coinIncome));
                if (uiCoinCount > 0)
                {
                    var pos = GetComponent<RectTransform>().GetUIPos();
                    Coin.CreateGroup(pos, UIUtil.COIN_POS, uiCoinCount);
                    D.I.TakeIncomeCoins();
                    mRefreshCD = 0;
                    AudioManager.PlaySound("collect_coin");
                }
            });
        }

        private void OnEnable()
        {
            mRefreshCD = ConstTable.table.coinIncomeRefreshCD;
            countText.text = D.I.coinIncomeTotal.KMB();
        }

        private void Update()
        {
            mRefreshCD = this.UpdateCD(mRefreshCD);
            imageFill.fillAmount = 1f - mRefreshCD / ConstTable.table.coinIncomeRefreshCD;
            if (mRefreshCD <= 0)
            {
                mRefreshCD = ConstTable.table.coinIncomeRefreshCD;
                if (D.I.coinIncomeTotal < 1)
                    countText.text = "0";
                else
                    countText.text = D.I.coinIncomeTotal.KMB();
            }
        }
    }
}