using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class BackgroundView : ViewBase
    {
        public RectTransform moveRoot;
        public Vector2 moveMin = new Vector2(-100, -100);
        public Vector2 moveMax = new Vector2(100, 100);

        public RectTransform shakeRoot;

        private void Awake()
        {
            Unibus.Subscribe<EventVirus>(OnEventVirus);
        }

        Tweener shakeTweener = null;
        private void OnEventVirus(EventVirus evt)
        {
            if (evt.action == EventVirus.Action.DEAD
                && (shakeTweener == null || !shakeTweener.IsPlaying()))
            {
                shakeTweener = shakeRoot.DOShakeAnchorPos(0.1f, ConstTable.table.virusBombShakeScreenRate);
            }
        }

        private void Update()
        {
            Aircraft aircraft = EntityManager.GetAll<Aircraft>().First() as Aircraft;
            if (aircraft == null)
                return;
            var uiPos = aircraft.rectTransform.GetUIPos();
            var xf = 1 - uiPos.x / UIUtil.width;
            var yf = 1 - uiPos.y / UIUtil.height;
            var tarPos = new Vector2(Mathf.Lerp(moveMin.x, moveMax.x, xf), Mathf.Lerp(moveMin.y, moveMax.y, yf));
            moveRoot.anchoredPosition = Vector2.Lerp(moveRoot.anchoredPosition, tarPos, Time.deltaTime * 5f);
        }
    }
}