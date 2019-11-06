using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class LoadingPanel : PanelBase
    {
        public Slider fill;
        public Text desc;

        private void Update()
        {
            var state = StateManager.Instance.currentState;
            if(state is ResourceUpdateState)
            {
                var rus = state as ResourceUpdateState;
                fill.value = rus.progress;
                desc.text = rus.message;
            }
        }
    }
}