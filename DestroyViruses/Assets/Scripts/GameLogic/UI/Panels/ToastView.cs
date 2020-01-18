using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;
using System.IO;

namespace DestroyViruses
{
    public class ToastView : ViewBase
    {
        public static string _text = "";
        public Image bg;
        public Text text;

        protected override void OnOpen()
        {
            base.OnOpen();
            text.text = _text;
            rectTransform.DOKill();
            rectTransform.localScale = new Vector3(0, 1, 1);
            rectTransform.DOScaleX(1, 0.2f).OnComplete(() =>
            {
                rectTransform.DOScaleX(0, 0.2f).SetDelay(2).OnComplete(() =>
                {
                    Close();
                });
            });
        }
    }

    public static class Toast
    {
        public static void Show(string text)
        {
            ToastView._text = text;
            UIManager.Open<ToastView>(UILayer.Top);
            AudioManager.PlaySound("button_grey");
        }
    }
}