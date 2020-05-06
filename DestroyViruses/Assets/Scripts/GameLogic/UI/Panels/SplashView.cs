using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class SplashView : ViewBase
    {
        public Slider fill;
        public Text desc;

        public static bool finish = false;
        private static string message;
        private static float progress;

        private static float finishTime;

        protected override void OnOpen()
        {
            base.OnOpen();
            SplashView.finish = false;
            SplashView.progress = 0;
            SplashView.message = "";
            finishTime = Time.time + 0.5f;
            Update();
        }

        private void Update()
        {
            if (desc.text != message) desc.text = message;
            fill.value = Mathf.Lerp(fill.value, progress, Time.deltaTime * 15);
            SplashView.finish = Time.time > finishTime;
        }

        public static void SetMessage(string message)
        {
            SplashView.message = message;
        }

        public static void SetProgress(float progress)
        {
            SplashView.progress = progress;
        }
    }
}