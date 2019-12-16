using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;
using DG.Tweening;

namespace DestroyViruses
{
    public class ExplosionCrash : ExplosionBase<ExplosionCrash>
    {
        public SpriteAnimation spriteAnimation;
        public void Reset(Vector2 pos)
        {
            rectTransform.anchoredPosition = pos;
            rectTransform.localScale = Vector3.one * 0.3f;
            rectTransform.DOScale(1, 0.15f).SetEase(Ease.OutSine);
            spriteAnimation.Restart();
        }
    }
}