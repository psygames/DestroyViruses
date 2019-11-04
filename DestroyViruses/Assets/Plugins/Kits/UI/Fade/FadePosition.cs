using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class FadePosition : Fade, IFade
{
    public Vector2 from = Vector2.zero;
    public Vector2 to = Vector2.zero;

    public override void FadeIn()
    {
        base.FadeIn();
        if (!enableFadeIn)
            return;
        GetComponent<RectTransform>().anchoredPosition = from;
        GetComponent<RectTransform>().DOAnchorPos(to, fadeInDuration)
            .SetDelay(fadeInDelay).SetEase(fadeInMethod).OnComplete(OnFadeInFinished);
    }

    public override void FadeOut()
    {
        base.FadeOut();
        if (!enableFadeOut)
            return;
        GetComponent<RectTransform>().anchoredPosition = to;
        GetComponent<RectTransform>().DOAnchorPos(from, fadeOutDuration)
            .SetDelay(fadeOutDelay).SetEase(fadeOutMethod).OnComplete(OnFadeOutFinished);
    }

    public override void FadeInImmediately()
    {
        base.FadeInImmediately();
        if (!enableFadeIn)
            return;
        GetComponent<RectTransform>().anchoredPosition = to;
        OnFadeInFinished();
    }

    public override void FadeOutImmediately()
    {
        base.FadeOutImmediately();
        if (!enableFadeOut)
            return;
        GetComponent<RectTransform>().anchoredPosition = from;
        OnFadeOutFinished();
    }
}
