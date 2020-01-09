using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace DestroyViruses
{
    public class ExplosionWeaponCoverItem : ExplosionBase<ExplosionWeaponCoverItem>
    {
        public void Reset(Vector2 pos)
        {
            rectTransform.anchoredPosition = pos;
        }
    }
}