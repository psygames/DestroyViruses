using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
            PlayOpenSound();
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
                DoTwEff(iconEffs[i].rectTransform, rawPos[i], targetPos, 0.4f + i * 0.1f);
            }

            gameObject.GetOrAddComponent<CanvasGroup>().DOKill();
            gameObject.GetOrAddComponent<CanvasGroup>().alpha = 1;
            gameObject.GetOrAddComponent<CanvasGroup>().DOFade(0, 0.5f).SetDelay(2f);

            this.StopAllCoroutines();
            this.DelayDo(2.5f, Close);
        }

        private void DoTwEff(RectTransform rect, Vector2 _rawPos, Vector2 targetPos, float _dur)
        {
            rect.GetComponent<Image>().DOKill();
            rect.GetComponent<Image>().SetAlpha(1);
            rect.anchoredPosition = icon.rectTransform.GetUIPos();
            rect.localScale = Vector3.zero;
            rect.DOScale(0.5f, 0.1f).SetDelay(0.9f);
            var cPos = rect.anchoredPosition;
            rect.DOAnchorPos((_rawPos - cPos) * Random.Range(0.65f, 1f) + cPos, 0.1f + Random.Range(0, 0.2f))
                .SetDelay(Random.Range(0.9f, 1f)).OnComplete(() =>
            {
                rect.DOAnchorPos(targetPos, _dur).SetDelay(0.5f).OnComplete(() =>
                {
                    rect.GetComponent<Image>().DOFade(0, 0.15f);
                    PlayItemSound();
                });
            });
        }

        private void PlayOpenSound()
        {
            //TODO: Play Open Sound
            AudioManager.PlaySound("coin");
        }

        private void PlayItemSound()
        {
            //TODO: Play Item Sound
            AudioManager.PlaySound("button_normal");
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