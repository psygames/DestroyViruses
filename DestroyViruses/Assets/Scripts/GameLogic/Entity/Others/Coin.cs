using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DestroyViruses
{
    public class Coin : EntityBase<Coin>
    {
        private static float sLastSoundTime = 0;

        public void Reset(Vector2 from, Vector2 to, Vector2 offset)
        {
            rectTransform.anchoredPosition = from;
            rectTransform.DOAnchorPos(from + offset, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                float dura = (offset + from - to).magnitude * 0.001f;
                rectTransform.DOAnchorPos(to, dura).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Recycle();
                    PlaySound();
                });
            });
        }

        public void PlaySound()
        {
            if (Time.time - sLastSoundTime > 0.2f)
            {
                AudioManager.Instance.PlaySound("Sounds/coin");
                sLastSoundTime = Time.time;
            }
        }

        public static void CreateGroup(Vector2 from, Vector2 to, int coinCount)
        {
            int count = Mathf.Clamp(coinCount, 1, 12);
            float radius = 400;
            for (int i = 0; i < count; i++)
            {
                Vector2 offset = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.down * radius;
                offset.x *= 0.7f;
                Create().Reset(from, to, offset);
            }
        }
    }
}