using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class ShopGoodsItem : ViewBase
    {
        public Text title;
        public Text price;
        public Image icon;
        public GameObject extraObj;
        public Text extra;

        private TableShop mTable;
        private float mLastClick = 0;

        public void SetData(int goodsID)
        {
            mTable = TableShop.Get(goodsID);
            title.text = LT.Get(mTable.nameID);
            icon.SetSprite(mTable.image);
            price.text = LT.Get(mTable.priceID);
            extraObj.SetActive(mTable.extra > 0);
            extra.text = $"+{mTable.extra}";
        }

        private void OnClickSelf()
        {
            if (Time.time - mLastClick < 1)
            {
                Toast.Show(LTKey.FREQUENT_OPERATION.LT());
                return;
            }
            mLastClick = Time.time;
            D.I.Purchase(mTable.id);
        }
    }
}