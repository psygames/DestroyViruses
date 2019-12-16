using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    [RequireComponent(typeof(Button))]
    public class ButtonPro : MonoBehaviour
    {
        public Image targetImage;
        public Text targetText;

        private static Material sGreyMat;

        private Color mLastColor = Color.white;
        private void Awake()
        {
            if (sGreyMat == null)
                sGreyMat = ResourceUtil.Load<Material>("Materials/UIGrey");
            if (targetText != null)
                mLastColor = targetText.color;
        }

        public void SetGrey(bool isGrey)
        {
            if (targetText != null)
            {
                if (isGrey)
                {
                    targetText.color = UIUtil.RED_COLOR;
                }
                else
                {
                    targetText.color = mLastColor;
                }
            }

            if (targetImage != null && sGreyMat != null)
            {
                if (isGrey)
                {
                    targetImage.material = sGreyMat;
                }
                else
                {
                    targetImage.material = null;
                }
            }
        }

        public void SetData(string text, bool grey)
        {
            if (targetText != null)
                targetText.text = text;
            SetGrey(grey);
        }
    }
}