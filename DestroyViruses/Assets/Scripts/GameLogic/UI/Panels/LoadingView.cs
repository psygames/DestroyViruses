using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class LoadingView : ViewBase
    {
        public Slider fill;
        public Text desc;

        private void Update()
        {
            var state = StateManager.Instance.currentState;
            if (state is HotUpdateState)
            {
                var rus = state as HotUpdateState;
                fill.value = Mathf.Lerp(fill.value, rus.progress, Time.deltaTime * 15);
                desc.text = rus.message;
            }
        }
    }
}