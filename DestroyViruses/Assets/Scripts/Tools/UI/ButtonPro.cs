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
        public bool downUpEvent;

        public string sound = "button_normal";

        private static Material sGreyMat;
        private bool isGrey = false;

        private void Awake()
        {
            if (sGreyMat == null)
                sGreyMat = Resources.Load<Material>("Materials/UIGrey");
            if (targetText != null)
                mOrginColor = targetText.color;
            GetComponent<Button>().onClick.AddListener(OnClick);
            if (downUpEvent)
            {
                gameObject.GetOrAddComponent<UIEventListener>().onDown.AddListener(OnDown);
                gameObject.GetOrAddComponent<UIEventListener>().onUp.AddListener(OnUp);
            }
        }

        private void OnDestroy()
        {
            GetComponent<Button>().onClick.RemoveAllListeners();
            if (downUpEvent)
            {
                gameObject.GetOrAddComponent<UIEventListener>().onDown.RemoveAllListeners();
                gameObject.GetOrAddComponent<UIEventListener>().onUp.RemoveAllListeners();
            }
        }

        private void OnDown(Vector2 vec)
        {
            if (!downUpEvent)
                return;
            SendEvent("OnDown");
        }

        private void OnUp(Vector2 vec)
        {
            if (!downUpEvent)
                return;
            SendEvent("OnUp");
        }

        private void OnClick()
        {
            if (!isGrey)
                AudioManager.Instance.PlaySound(sound);
            SendEvent("OnClick");
        }

        private void SendEvent(string header)
        {
            if (string.IsNullOrEmpty(clickEvent))
                return;
            var view = GetComponentInParent<ViewBase>();
            if (view == null)
                return;
            var method = view.GetType().GetMethod(header + clickEvent, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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
            this.isGrey = isGrey;
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