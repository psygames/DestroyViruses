using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DestroyViruses
{
    public class ExplosionWeaponCoverItem : ExplosionBase<ExplosionWeaponCoverItem>
    {
        public void Reset(Vector2 pos, string sound)
        {
            rectTransform.anchoredPosition = pos;
            AudioManager.PlaySound(sound);
        }
    }
}