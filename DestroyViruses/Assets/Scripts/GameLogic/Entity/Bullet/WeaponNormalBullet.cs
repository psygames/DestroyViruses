using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnibusEvent;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class WeaponNormalBullet : WeaponBulletBase
    {
        protected override void OnHit(VirusBase virus)
        {
            base.OnHit(virus);
                    
            Vector2 cDir = virus.position - position;
            Vector2 dir = (cDir + virus.direction).normalized;
            virus.SetDirection(dir);

            Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, virus, mDamage));
        }
    }
}