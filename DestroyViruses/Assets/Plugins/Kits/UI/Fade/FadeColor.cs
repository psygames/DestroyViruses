using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(UnityEngine.UI.Graphic))]
public class FadeColor : Fade, IFade
{
    public Color from = Color.white;
    public Color to = Color.white;

    private UnityEngine.UI.Graphic m_graphic = null;

    void Awake()
    {
        m_graphic = GetComponent<UnityEngine.UI.Graphic>();
    }

    public override void FadeIn()
    {
        base.FadeIn();
        if (!enableFadeIn)
            return;
        if (m_graphic == null)
            return;
        m_graphic.color = from;
        m_graphic.DOColor(to, fadeInDuration).SetDelay(fadeInDelay)
            .SetEase(fadeInMethod).OnComplete(OnFadeInFinished);
    }

    public override void FadeOut()
    {
        base.FadeOut();
        if (!enableFadeOut)
            return;
        if (m_graphic == null)
            return;
        m_graphic.color = to;
        m_graphic.DOColor(from, fadeOutDuration).SetDelay(fadeOutDelay)
            .SetEase(fadeOutMethod).OnComplete(OnFadeOutFinished);
    }

    public override void FadeInImmediately()
    {
        base.FadeInImmediately();
        if (!enableFadeIn)
            return;
        m_graphic.color = to;
        OnFadeInFinished();
    }

    public override void FadeOutImmediately()
    {
        base.FadeOutImmediately();
        if (!enableFadeOut)
            return;
        m_graphic.color = from;
        OnFadeOutFinished();
    }
}
