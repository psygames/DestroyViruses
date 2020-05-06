using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DestroyViruses
{
    public class ExplosionBullet : ExplosionBase<ExplosionBullet>
    {
        public void Reset(Vector2 pos)
        {
            rectTransform.anchoredPosition = pos;
        }
    }
}