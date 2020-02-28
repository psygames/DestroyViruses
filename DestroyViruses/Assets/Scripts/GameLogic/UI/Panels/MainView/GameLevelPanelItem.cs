using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

namespace DestroyViruses
{
    public class GameLevelPanelItem : MonoBehaviour
    {
        public RectTransform rectTransform;
        public Text levelText;
        public GameObject bossTag;
        public Image bgImage;
        public CanvasGroup canvasGroup;

        public int space = 100;

        public void SetLevel(int level)
        {
            if (TableGameLevel.Get(level) == null)
            {
                levelText.text = "-";
                bossTag.SetActive(false);
            }
            else
            {
                levelText.text = level.ToString();
                bossTag.SetActive(TableGameLevel.Get(level).isBoss);
            }

            var pos = level - D.I.gameLevel;
            bool isCenter = pos == 0;
            rectTransform.anchoredPosition = Vector2.right * pos * space;
            var tarscale = isCenter ? 1 : 0.8f;
            rectTransform.localScale = tarscale * Vector3.one;
            var tarcolor = isCenter ? Color.white : Color.grey;
            bgImage.color = tarcolor;
            levelText.color = tarcolor;
            canvasGroup.alpha = Mathf.Abs(pos) <= 1 ? 1 : 0;
        }

        public void FadeToLevelPos(int level)
        {
            float effectDura = 0.5f;
            var pos = level - D.I.gameLevel;
            bool isCenter = pos == 0;
            rectTransform.DOAnchorPosX(pos * space, effectDura);
            var tarscale = isCenter ? 1 : 0.8f;
            rectTransform.DOScale(tarscale, effectDura);
            var tarcolor = isCenter ? Color.white : Color.grey;
            bgImage.DOColor(tarcolor, effectDura);
            levelText.DOColor(tarcolor, effectDura);
            var taralpha = Mathf.Abs(pos) <= 1 ? 1 : 0;
            canvasGroup.DOFade(taralpha, 0.25f);
        }
    }

}