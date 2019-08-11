using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DestroyViruses
{
    public class Coin : EntityBase<Coin>
    {
        public void Reset(Vector2 from, Vector2 to, Vector2 offset)
        {
            rectTransform.anchoredPosition = from;
            rectTransform.DOAnchorPos(from + offset, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                rectTransform.DOAnchorPos(to, 0.25f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    Recycle();
                });
            });
        }

        public static void CreateGroup(Vector2 from, Vector2 to, float coin)
        {
            coin = Mathf.Pow(coin, 0.3f);
            int count = Mathf.Clamp((int)coin, 1, 20);
            float radius = 200;
            for (int i = 0; i < count; i++)
            {
                Vector2 offset = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.down * radius;
                offset.x *= 0.6f;
                Create().Reset(from, to, offset);
            }
        }
    }
}