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

        private void Awake()
        {
            if (sGreyMat == null)
                sGreyMat = Resources.Load<Material>("Materials/UIGrey");
            if (targetText != null)
                mOrginColor = targetText.color;
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

        private Color mOrginColor = Color.white;
        public void SetGrey(bool isGrey)
        {
            if (targetText != null)
            {
                targetText.color = isGrey ? UIUtil.RED_COLOR : mOrginColor;
            }

            if (targetImage != null && sGreyMat != null)
            {
                targetImage.material = isGrey ? sGreyMat : null;
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