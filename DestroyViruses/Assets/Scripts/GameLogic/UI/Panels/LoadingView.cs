using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class LoadingView : ViewBase
    {
        public Slider fill;
        public Text desc;

        private static string message;
        private static float progress;

        protected override void OnOpen()
        {
            base.OnOpen();

            LoadingView.message = "";
            LoadingView.progress = 0;
            Update();
        }

        private void Update()
        {
            if (desc.text != message) desc.text = message;
            fill.value = Mathf.Lerp(fill.value, progress, Time.deltaTime * 15);
        }

        public static void SetMessage(string message)
        {
            LoadingView.message = message;
        }

        public static void SetProgress(float progress)
        {
            LoadingView.progress = progress;
        }
    }
}