using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace DestroyViruses
{
    public class MainPanel : UIPanel
    {
        public UIEventListener inputListenser;

        public override void OnInit()
        {
            inputListenser.onDrag.AddListener(OnDragInput);
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
        }

        public override void OnDestroy()
        {
        }

        private void OnDragInput(Vector2 delta)
        {
            Debug.Log(delta);
        }
    }
}