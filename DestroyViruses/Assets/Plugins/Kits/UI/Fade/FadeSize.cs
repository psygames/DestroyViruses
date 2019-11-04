using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class FadeSize : Fade, IFade
{
    public Vector2 from = Vector2.zero;
    public Vector2 to = Vector2.zero;

    public override void FadeIn()
    {
        base.FadeIn();
        if (!enableFadeIn)
            return;
        GetComponent<RectTransform>().sizeDelta = from;
        GetComponent<RectTransform>().DOSizeDelta(to, fadeInDuration)
            .SetDelay(fadeInDelay).SetEase(fadeInMethod).OnComplete(OnFadeInFinished);
    }

    public override void FadeOut()
    {
        base.FadeOut();
        if (!enableFadeOut)
            return;
        GetComponent<RectTransform>().sizeDelta = to;
        GetComponent<RectTransform>().DOSizeDelta(from, fadeOutDuration)
            .SetDelay(fadeOutDelay).SetEase(fadeOutMethod).OnComplete(OnFadeOutFinished);
    }

    public override void FadeInImmediately()
    {
        base.FadeInImmediately();
        if (!enableFadeIn)
            return;
        GetComponent<RectTransform>().sizeDelta = to;
        OnFadeInFinished();
    }

    public override void FadeOutImmediately()
    {
        base.FadeOutImmediately();
        if (!enableFadeOut)
            return;
        GetComponent<RectTransform>().sizeDelta = from;
        OnFadeOutFinished();
    }
}
