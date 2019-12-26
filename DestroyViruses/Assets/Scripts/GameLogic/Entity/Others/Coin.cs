using UnityEngine;
using DG.Tweening;

namespace DestroyViruses
{
    public class Coin : EntityBase<Coin>
    {
        public void Reset(Vector2 from, Vector2 to, Vector2 offset)
        {
            float dura = (offset + from - to).magnitude * 0.0015f;
            rectTransform.anchoredPosition = from;
            rectTransform.DOLocalPath(new Vector3[] { from + offset, to }, dura, PathType.CatmullRom)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                PlaySound();
                Recycle();
            });
        }

        public void PlaySound()
        {
            if (TimeUtil.CheckInterval("CoinSfx", ConstTable.table.coinSfxInterval))
            {
                AudioManager.Instance.PlaySound("coin");
            }
        }

        public static void CreateGroup(Vector2 from, Vector2 to, int uiCoinCount)
        {
            int count = Mathf.Clamp(uiCoinCount, 1, 15);
            float radius = 300;
            for (int i = 0; i < count; i++)
            {
                float angle = Random.Range(0, 360);
                Vector2 offset = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.left * radius;
                offset.x *= 0.7f;
                Create().Reset(from, to, offset);
            }
        }
    }
}