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

        private void Awake()
        {
            foreach (var p in particles)
            {
                p.Stop(true);
            }
        }

        public void Reset(Vector2 pos, int type, float scale)
        {
            rectTransform.anchoredPosition = pos;
            rectTransform.localScale = Vector3.one * scale * 0.3f;
            rectTransform.DOScale(scale, 0.3f).SetEase(Ease.OutSine);
            particles[Random.Range(0, particles.Length)].Play(true);

            if (TimeUtil.CheckInterval("ExplosionSfx", ConstTable.table.explosionSfxInterval))
            {
                AudioManager.Instance.PlaySound("explosion");
            }
            if (Option.vibration)
            {
                Vibrate();
            }
        }

        private void Vibrate()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (TimeUtil.CheckInterval("Vibrate", ConstTable.table.vibrateInterval))
            {
                Vibration.Vibrate((int)(ConstTable.table.vibrateDuration * 1000));
            }
#endif
        }
    }
}