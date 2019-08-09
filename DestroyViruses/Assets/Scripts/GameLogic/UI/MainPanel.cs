using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace DestroyViruses
{
    public class MainPanel : UIPanel
    {
        public UIEventListener inputListenser;
        public float dragBeginThreshold = 100; 

        private Vector2 mTotalDrag = Vector2.zero;

        private void Awake()
        {
            InputListenerInit();
        }

        protected override void OnOpen()
        {
            mTotalDrag = Vector2.zero;
            UIUtil.uiBattleRoot.DOScale(Vector3.one * 1.2f, 0.5f);
        }

        protected override void OnClose()
        {
        }

        private void OnDestroy()
        {
            inputListenser.onDrag.RemoveAllListeners();
        }

        private void InputListenerInit()
        {
            inputListenser.onDrag.AddListener((data) =>
            {
                mTotalDrag += data;
                if (mTotalDrag.magnitude > dragBeginThreshold)
                {
                    GameManager.ChangeState<BattleState>();
                }
            });
        }
    }
}