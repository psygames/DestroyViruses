using UnityEngine;
using System;

namespace DestroyViruses
{
    public enum UILayer
    {
        Common = 0,
        Top = 1,
        Bottom = 2,
    }

    public abstract class UIPanel : MonoBehaviour
    {
        protected RectTransform mRectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (mRectTransform == null)
                    mRectTransform = GetComponent<RectTransform>();
                return mRectTransform;
            }
        }
        public abstract void OnInit();
        public abstract void OnOpen();
        public abstract void OnClose();
        public abstract void OnDestroy();
    }
}