using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class GameEndPanel : UIPanel
    {
        public Button receiveBtns;
        public Text coinText;

        private void Awake()
        {
            receiveBtns.OnClickAsObservable().Subscribe(_ => OnClickReceive());
        }

        private void OnClickReceive()
        {
            Close();
            GameManager.ChangeState<MainState>();
        }

        protected override void OnOpen()
        {
            coinText.text = "11.5K";
        }
    }
}