using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnibusEvent;
using UniRx;

namespace DestroyViruses
{
    public class WeaponRailgun : WeaponBase
    {
        public RectTransform unit1;
        public RectTransform unit2;

        public override int unitCount { get { return 2; } }

        protected override void OnUnitFire(int index)
        {
            base.OnUnitFire(index);
            if (index == 0)
            {
                EntityManager.Create<WeaponRailgunBullet>().Reset(unit1.GetUIPos()
                    , Vector2.up
                    , damage, effects, table.explosionSound);
            }
            else if (index == 1)
            {
                EntityManager.Create<WeaponRailgunBullet>().Reset(unit2.GetUIPos()
                    , Vector2.up
                    , damage, effects, table.explosionSound);
            }
            AudioManager.Instance.PlaySound(table.fireSound);
        }

        protected override void Update()
        {
            base.Update();

            unit1.localScale = Vector3.one * GetUnitFill(0);
            unit2.localScale = Vector3.one * GetUnitFill(1);
        }
    }
}