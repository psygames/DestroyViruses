using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class FadeAlpha : Fade, IFade
{
    [Range(0, 1)]
    public float from = 0;
    [Range(0, 1)]
    public float to = 1;

    private CanvasGroup m_canvasGroup;
    private void UseCanvasGroup()
    {
        if (m_canvasGroup == null)
            m_canvasGroup = GetComponent<CanvasGroup>();
        if (m_canvasGroup == null)
            m_canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public override void FadeIn()
    {
        base.FadeIn();
        if (!enableFadeIn)
            return;
        UseCanvasGroup();
        m_canvasGroup.alpha = from;
        DOTween.To(() => m_canvasGroup.alpha, (x) => m_canvasGroup.alpha = x, to, fadeInDuration)
            .SetDelay(fadeInDelay).SetEase(fadeInMethod).OnComplete(OnFadeInFinished);
    }

    public override void FadeOut()
    {
        base.FadeOut();
        if (!enableFadeOut)
            return;
        UseCanvasGroup();
        m_canvasGroup.alpha = to;
        DOTween.To(() => m_canvasGroup.alpha, (x) => m_canvasGroup.alpha = x, from, fadeOutDuration)
            .SetDelay(fadeOutDelay).SetEase(fadeOutMethod).OnComplete(OnFadeOutFinished);
    }

    public override void FadeInImmediately()
    {
        base.FadeInImmediately();
        if (!enableFadeIn)
            return;
        UseCanvasGroup();
        m_canvasGroup.alpha = to;
        OnFadeInFinished();
    }

    public override void FadeOutImmediately()
    {
        base.FadeOutImmediately();
        if (!enableFadeOut)
            return;
        UseCanvasGroup();
        m_canvasGroup.alpha = from;
        OnFadeOutFinished();
    }
}
