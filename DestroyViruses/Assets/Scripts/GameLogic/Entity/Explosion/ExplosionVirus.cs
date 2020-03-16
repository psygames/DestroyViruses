using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using DG.Tweening;

namespace DestroyViruses
{
    public class ExplosionVirus : ExplosionBase<ExplosionVirus>
    {
        public ParticleSystem[] particles;

        public void Reset(Vector2 pos, int type, float scale)
        {
            rectTransform.anchoredPosition = pos;
            rectTransform.localScale = Vector3.one * scale * 0.3f;
            rectTransform.DOScale(scale, 0.3f).SetEase(Ease.OutSine);

            var index = Random.Range(0, particles.Length);
            for (int i = 0; i < particles.Length; i++)
            {
                // TODO:?USE Random effect
                particles[i].gameObject.SetActive(i == index);
            }

            if (TimeUtil.CheckInterval("ExplosionSfx", CT.table.explosionSfxInterval))
            {
                AudioManager.PlaySound("explosion");
            }
            if (Option.vibration)
            {
                Vibrate();
            }
        }

        private void Vibrate()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (TimeUtil.CheckInterval("Vibrate", CT.table.vibrateInterval))
            {
                Vibration.Vibrate((int)(CT.table.vibrateDuration * 1000));
            }
#endif
        }
    }
}