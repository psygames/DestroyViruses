using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace DestroyViruses
{
    public class ExplosionWeaponRailgunBullet : ExplosionBase<ExplosionWeaponRailgunBullet>
    {
        public void Reset(Vector2 pos, float radius)
        {
            rectTransform.anchoredPosition = pos;
            rectTransform.localScale = Vector3.one * radius * 0.01f;
        }
    }
}