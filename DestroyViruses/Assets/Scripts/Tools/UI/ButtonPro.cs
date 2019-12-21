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
        public void SetTextRed(bool isRed)
        {
            if (targetText != null)
                targetText.color = isRed ? UIUtil.RED_COLOR : mOrginColor;
        }

        public void SetBtnGrey(bool isGrey)
        {
            if (targetImage != null && sGreyMat != null)
                targetImage.material = isGrey ? sGreyMat : null;
        }

        public void SetText(string text)
        {
            if (targetText != null)
                targetText.text = text;
        }

        public void SetData(string text, bool isBtnGrey, bool isTextRed)
        {
            SetText(text);
            SetBtnGrey(isBtnGrey);
            SetTextRed(isTextRed);
        }

        public void Set4LevelUp(float costCoin, bool isMax)
        {
            if (isMax) SetText(LT.table.LEVEL_MAX);
            else SetText(costCoin.KMB());
            SetBtnGrey(isMax || costCoin > D.I.coin);
            SetTextRed(costCoin > D.I.coin);
        }
    }
}