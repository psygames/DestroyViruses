using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class ButtonTips : ViewBase
    {
        public Text tipsText;
        public FadeGroup msgFade;

        private void OnEnable()
        {
            msgFade.FadeOutImmediately();
        }

        public void SetData(string tips)
        {
            if (string.IsNullOrEmpty(tips))
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            tipsText.text = tips;
        }

        void OnDownTips()
        {
            msgFade.FadeIn();
        }

        void OnUpTips()
        { 
            msgFade.FadeOut();
        }
    }
}