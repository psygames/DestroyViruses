using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnibusEvent;
using UniRx;

namespace DestroyViruses
{
    public class WeaponCover : WeaponBase
    {
        public RectTransform unitRoot;
        public WeaponCoverItem unitTemplate;

        public override int unitCount => 18;
        public override bool autoFire => false;

        private WeaponCoverItem[] units;

        private void Awake()
        {
            if (units == null)
            {
                unitTemplate.gameObject.SetActive(false);
                units = new WeaponCoverItem[unitCount];
                for (int i = 0; i < unitCount; i++)
                {
                    var unit = Instantiate<WeaponCoverItem>(unitTemplate);
                    unit.gameObject.SetActive(true);
                    unit.transform.SetParent(unitRoot, false);
                    unit.SetData(i);
                    unit.onCollider = OnItemCollider;
                    unit.SetReady(false);
                    units[i] = unit;
                }
            }
        }

        protected override void OnUnitReady(int index)
        {
            base.OnUnitReady(index);
            units[index].SetReady(true);
        }

        private void OnItemCollider(int index, VirusBase virus)
        {
            Debug.LogError("collider " + index);
            units[index].SetReady(false);
        }

        protected override void Update()
        {
            base.Update();
            for (int i = 0; i < unitCount; i++)
            {
                units[i].SetFill(GetUnitFill(i));
            }
        }
    }
}