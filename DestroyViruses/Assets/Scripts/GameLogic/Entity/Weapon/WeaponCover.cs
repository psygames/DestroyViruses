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
        private RectTransform bindRoot;

        private void Awake()
        {
            if (units == null)
            {
                bindRoot = rectTransform;
                unitTemplate.gameObject.SetActive(false);
                units = new WeaponCoverItem[unitCount];
                for (int i = 0; i < unitCount; i++)
                {
                    var unit = Instantiate(unitTemplate);
                    unit.gameObject.SetActive(true);
                    unit.transform.SetParent(unitRoot, false);
                    unit.onCollider = OnItemCollider;
                    units[i] = unit;
                }

                unitRoot.SetParent(UIUtil.uiRoot.Find("Battle/EntityRoot"), false);
            }
        }

        public override void Reset(int id, int powerLevel, int speedLevel)
        {
            base.Reset(id, powerLevel, speedLevel);
            for (int i = 0; i < unitCount; i++)
            {
                units[i].SetData(i, effects[1]);
                units[i].SetReady(false);
            }
        }

        protected override void OnUnitReady(int index)
        {
            base.OnUnitReady(index);
            units[index].SetReady(true);
        }

        private void OnItemCollider(int index, VirusBase virus)
        {
            base.OnUnitFire(index);
            units[index].SetReady(false);
            Vector2 cDir = virus.position - units[index].position;
            Vector2 dir = (cDir + virus.direction).normalized;
            virus.SetDirection(dir);
            Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, virus, damage));
            ExplosionWeaponCoverItem.Create().Reset(units[index].position);
        }

        protected override void Update()
        {
            base.Update();
            unitRoot.anchoredPosition = bindRoot.GetUIPos();
            unitRoot.localScale = Aircraft.ins.rectTransform.localScale;
            unitRoot.localRotation *= Quaternion.AngleAxis(effects[0] * Time.deltaTime * GlobalData.slowDownFactor, Vector3.forward);
            for (int i = 0; i < unitCount; i++)
            {
                units[i].SetFill(GetUnitFill(i));
            }
        }

        private void OnEnable()
        {
            unitRoot.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            unitRoot.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            DestroyImmediate(unitRoot.gameObject);
        }
    }
}