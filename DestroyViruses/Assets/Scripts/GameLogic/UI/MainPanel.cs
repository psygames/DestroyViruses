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
            InputListenerInit();
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

        private void InputListenerInit()
        {
            inputListenser.onDown.AddListener((data)=>
            {
                InputManager.Instance.Push(new InputData(InputType.Down, UIUtil.FormatToVirtual(data)));
            });
            inputListenser.onUp.AddListener((data) =>
            {
                InputManager.Instance.Push(new InputData(InputType.Up, UIUtil.FormatToVirtual(data)));
            });
            inputListenser.onDrag.AddListener((data) =>
            {
                InputManager.Instance.Push(new InputData(InputType.Drag, UIUtil.FormatToVirtual(data)));
            });
        }
    }
}