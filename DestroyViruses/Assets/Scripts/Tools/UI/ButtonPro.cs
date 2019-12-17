using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace DestroyViruses
{
    [RequireComponent(typeof(Button))]
    public class ButtonPro : MonoBehaviour
    {
        public Image targetImage;
        public Text targetText;
        public string clickEvent;
        public int clickEventID;

        private static Material sGreyMat;

        private Color mLastColor = Color.white;
        private void Awake()
        {
            if (sGreyMat == null)
                sGreyMat = ResourceUtil.Load<Material>("Materials/UIGrey");
            if (targetText != null)
                mLastColor = targetText.color;

            GetComponent<Button>().OnClick(OnClick);
        }

        private void OnClick()
        {
            if (string.IsNullOrEmpty(clickEvent))
                return;
            var view = GetComponentInParent<ViewBase>();
            if (view == null)
                return;
            var method = view.GetType().GetMethod("OnClick" + clickEvent, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
                return;
            if (method.GetParameters().Length == 1)
                method.Invoke(view, new object[] { clickEventID });
            else if (method.GetParameters().Length == 0)
                method.Invoke(view, new object[] { });
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