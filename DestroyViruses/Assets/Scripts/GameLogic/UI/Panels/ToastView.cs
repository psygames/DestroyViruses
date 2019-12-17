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
        public Image bg;
        public Text text;

        protected override void OnOpen()
        {
            base.OnOpen();
            text.text = Toast.text;
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
        public static string text = "";
        public static void Show(string text)
        {
            Toast.text = text;
            UIManager.Instance.Open<ToastView>(UILayer.Top);
        }
    }
}