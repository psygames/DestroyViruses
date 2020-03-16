using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace DestroyViruses
{
    public class RateStarItem : ViewBase
    {
        public Image forward;
        private int mIndex;
        private Action<int> mCallback;

        public void SetData(int index, bool isOn, Action<int> callback)
        {
            mIndex = index;
            mCallback = callback;
            forward.gameObject.SetActive(isOn);
        }

        private void OnClickSelf()
        {
            mCallback?.Invoke(mIndex);
        }
    }
}
