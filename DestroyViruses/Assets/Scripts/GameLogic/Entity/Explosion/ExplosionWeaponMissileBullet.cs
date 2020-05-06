using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DestroyViruses
{
    public class ExplosionWeaponMissileBullet : ExplosionBase<ExplosionWeaponMissileBullet>
    {
        public void Reset(Vector2 pos, float radius,string sound)
        {
            rectTransform.anchoredPosition = pos;
            rectTransform.localScale = Vector3.one * radius * 0.01f;
            AudioManager.PlaySound(sound);
        }
    }
}