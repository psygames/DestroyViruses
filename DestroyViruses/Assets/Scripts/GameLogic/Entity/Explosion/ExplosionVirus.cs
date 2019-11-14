using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;
using DG.Tweening;

namespace DestroyViruses
{
    public class ExplosionVirus : ExplosionBase<ExplosionVirus>
    {
        public Image image;
        public SpriteAnimation spriteAnimation;

        public Image[] pieces;

        public void Reset(Vector2 pos, int type, float scale, int frames = 12)
        {
            rectTransform.anchoredPosition = pos;
            rectTransform.localScale = Vector3.one * scale * 0.3f;
            rectTransform.DOScale(scale, 0.3f).SetEase(Ease.OutSine);

            image.SetSprite($"effect_explosion_virus_{type}_1");
            image.SetNativeSize();

            if (spriteAnimation.sprites.Length != frames)
            {
                spriteAnimation.sprites = new Sprite[frames];
            }
            for (int i = 0; i < frames; i++)
            {
                spriteAnimation.sprites[i] = UIUtil.GetSprite($"effect_explosion_virus_{type}_{i + 1}");
            }

            foreach (var p in pieces)
            {
                p.SetSprite($"effect_explosion_virus_piece_{type}");
            }

            spriteAnimation.Restart();

            AudioManager.Instance.PlaySound("Sounds/explosion");
        }
    }
}