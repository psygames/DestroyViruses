using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnibusEvent;
using UniRx;

namespace DestroyViruses
{
    public class WeaponMissile : WeaponBase
    {
        public RectTransform unit1;
        public RectTransform unit2;

        public override int unitCount { get { return 2; } }

        protected override void OnUnitFire(int index)
        {
            base.OnUnitFire(index);
            if (index == 0)
            {
                EntityManager.Create<WeaponMissileBullet>().Reset(unit1.GetUIPos(), Vector2.up, damage, effects);
            }
            else if (index == 1)
            {
                EntityManager.Create<WeaponMissileBullet>().Reset(unit2.GetUIPos(), Vector2.up, damage, effects);
            }
        }

        protected override void Update()
        {
            base.Update();

            unit1.localScale = Vector3.one * GetUnitFill(0);
            unit1.anchoredPosition = Vector2.Lerp(Vector2.zero, Vector2.up * 10, GetUnitFill(0));
            unit2.localScale = Vector3.one * GetUnitFill(1);
            unit2.anchoredPosition = Vector2.Lerp(Vector2.zero, Vector2.up * 10, GetUnitFill(1));
        }
    }
}