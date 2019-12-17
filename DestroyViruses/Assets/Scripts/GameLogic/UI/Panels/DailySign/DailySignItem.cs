using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DestroyViruses
{
    public class DailySignItem : MonoBehaviour
    {
        public Text title;
        public Image image;
        [Header("0-未领取, 1-待领取, 2-可领取, 3-已领取")]
        public RadioObjects stateRadio;

        public void SetData(int days)
        {
            var t = TableDailySign.Get(days);
            title.text = LT.Get(t?.nameID);
            image.SetSprite(t?.icon);

            if (days == D.I.signDays)
            {
                if (D.I.CanDailySign()) stateRadio.Radio(2);
                else stateRadio.Radio(1);
            }
            else
            {
                if (days < D.I.signDays) stateRadio.Radio(3);
                else stateRadio.Radio(0);
            }
        }
    }
}