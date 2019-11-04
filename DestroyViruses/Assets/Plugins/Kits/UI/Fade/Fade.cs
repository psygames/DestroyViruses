using System;
using UnityEngine;
using DG.Tweening;

public class Fade : MonoBehaviour, IFade
{
    public bool enableFadeIn = true;
    public bool onEnableFadeIn = false;
    public float fadeInDelay = 0f;
    public float fadeInDuration = 0.3f;
    public Ease fadeInMethod = Ease.OutSine;
    public event Action onFadeInComplete;

    public bool enableFadeOut = true;
    public bool outSetInactive = true;
    public float fadeOutDelay = 0f;
    public float fadeOutDuration = 0.3f;
    public Ease fadeOutMethod = Ease.OutSine;
    public event Action onFadeOutComplete;

    public bool autoFadeOut = false;
    public bool isIn { get; protected set; }

    protected virtual void OnEnable()
    {
        if (onEnableFadeIn)
            FadeIn();
    }

    public virtual void FadeIn()
    {
        if (!enableFadeIn)
            return;
        isIn = true;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }

    public virtual void FadeOut()
    {
        if (!enableFadeOut)
            return;
        isIn = false;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }

    public virtual void FadeInImmediately()
    {
        if (!enableFadeIn)
            return;
        isIn = true;
    }

    public virtual void FadeOutImmediately()
    {
        if (!enableFadeOut)
            return;
        isIn = false;
        if (outSetInactive && gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    protected virtual void OnFadeInFinished()
    {
        if (autoFadeOut && isIn && gameObject.activeInHierarchy)
            FadeOut();
        if (onFadeInComplete != null)
            onFadeInComplete.Invoke();
    }

    protected virtual void OnFadeOutFinished()
    {
        if (outSetInactive && gameObject.activeSelf)
            gameObject.SetActive(false);
        if (onFadeOutComplete != null)
            onFadeOutComplete.Invoke();
    }

}
