using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace DestroyViruses
{
    [RequireComponent(typeof(Text))]
    public class TextPro : MonoBehaviour
    {
        public string LTKey = "";
        public object[] args;
        private int mMaxSize;


        public Text targetText { get; private set; }
        private void Awake()
        {
            targetText = GetComponent<Text>();
            mMaxSize = targetText.fontSize;
            targetText.resizeTextForBestFit = true;
            targetText.resizeTextMinSize = 10;
            targetText.resizeTextMaxSize = mMaxSize;
        }

        private string mLastLanguage = "";
        private void Update()
        {
            if (string.IsNullOrEmpty(LTKey))
                return;

            if (mLastLanguage != Option.language)
            {
                targetText.text = LT.Get(LTKey, args);
                mLastLanguage = Option.language;
            }
        }


        public void SetTextKey(string key, params object[] args)
        {
            LTKey = key;
            this.args = args;
        }

        public void SetText(string text)
        {
            LTKey = "";
            args = null;
            targetText.text = text;
        }
    }
}