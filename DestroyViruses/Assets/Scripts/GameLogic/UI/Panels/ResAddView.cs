using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;
using System.IO;

namespace DestroyViruses
{
    public class ResAddView : ViewBase
    {
        public enum ResType
        {
            Coin = 0,
            Energy = 1,
            Diamond = 2,
        }

        public static ResType resType { get; set; }
        public static string amountText { get; set; }
        public static bool isOpening { get; private set; }

        public RectTransform center;
        public Image icon;
        public Text title;
        public Image[] iconEffs;

        private Vector2 rawCenterPos;
        private Vector2[] rawPos;

        private void Awake()
        {
            rawCenterPos = center.anchoredPosition;
            rawPos = new Vector2[iconEffs.Length];
            for (int i = 0; i < rawPos.Length; i++)
            {
                rawPos[i] = iconEffs[i].rectTransform.anchoredPosition;
            }
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            isOpening = true;

            title.text = amountText;

            var sp = "";
            var targetPos = Vector2.zero;
            if (resType == ResType.Coin)
            {
                sp = "icon_coin";
                targetPos = UIUtil.COIN_POS;
            }
            else if (resType == ResType.Diamond)
            {
                sp = "icon_diamond";
                targetPos = UIUtil.DIAMOND_POS;
            }
            else if (resType == ResType.Energy)
            {
                sp = "icon_energy";
                targetPos = UIUtil.ENERGY_POS;
            }

            center.DOKill();
            center.localScale = Vector3.one * 0.1f;
            center.DOScale(1, 0.2f);
            center.anchoredPosition = rawCenterPos;
            center.DOAnchorPos(rawCenterPos + Vector2.up * 80, 0.25f);

            icon.SetSprite(sp);
            for (int i = 0; i < iconEffs.Length; i++)
            {
                iconEffs[i].SetSprite(sp);
                DoTwEff(iconEffs[i].rectTransform, rawPos[i], targetPos);
            }

            gameObject.GetOrAddComponent<CanvasGroup>().DOKill();
            gameObject.GetOrAddComponent<CanvasGroup>().alpha = 1;
            gameObject.GetOrAddComponent<CanvasGroup>().DOFade(0, 0.5f).SetDelay(1.5f);

            this.StopAllCoroutines();
            this.DelayDo(2, Close);
        }

        private void DoTwEff(RectTransform rect, Vector2 rawPos, Vector2 targetPos)
        {
            rect.GetComponent<Image>().DOKill();
            rect.GetComponent<Image>().SetAlpha(1);
            rect.anchoredPosition = icon.rectTransform.GetUIPos();
            var cPos = rect.anchoredPosition;
            rect.DOAnchorPos((rawPos - cPos) * Random.Range(0.65f, 1f) + cPos, 0.1f + Random.Range(0, 0.2f))
                .SetDelay(Random.Range(0, 0.1f)).OnComplete(() =>
            {
                rect.DOAnchorPos(targetPos, 0.5f).SetDelay(0.5f).OnComplete(() =>
                {
                    rect.GetComponent<Image>().DOFade(0, 0.15f);
                });
            });
        }

        protected override void OnClose()
        {
            isOpening = false;
            base.OnClose();
        }
    }

    public static class ResAddEffect
    {
        public static void Play(ResAddView.ResType type, int amount)
        {
            //if (!ResAddView.isOpening)
            {
                ResAddView.resType = type;
                ResAddView.amountText = $"+{amount}";
                UIManager.Open<ResAddView>(UILayer.Top);
            }
        }
    }
}