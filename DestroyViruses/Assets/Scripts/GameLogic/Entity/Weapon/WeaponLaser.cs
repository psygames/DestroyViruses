using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnibusEvent;
using UniRx;

namespace DestroyViruses
{
    public class WeaponLaser : WeaponBase
    {
        public RectTransform firement;
        public ParticleSystem fireEffect;

        private void OnEnable()
        {
            fireEffect.Stop(true);
        }

        protected override void OnUnitFire(int index)
        {
            base.OnUnitFire(index);
            fireEffect.Play(true);
            this.DelayDo(0.5f, () =>
            {
                EntityManager.Create<WeaponLaserBullet>().Reset(firement.GetUIPos(), Vector2.zero, damage, effects);
            });
        }
    }
}