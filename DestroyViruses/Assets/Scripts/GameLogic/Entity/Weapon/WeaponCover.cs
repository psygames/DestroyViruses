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
        private int shape = 0;

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
            this.shape = 0;
            for (int i = 0; i < unitCount; i++)
            {
                mUnitCD[i] = 0;
                units[i].SetData(i, effects[1]);
                units[i].SetShape(shape);
                units[i].SetReady(true);
            }
        }

        protected override void OnUnitReady(int index)
        {
            base.OnUnitReady(index);
            units[index].SetReady(true);
            AudioManager.PlaySound(table.fireSound);
        }

        private void OnItemCollider(int index, VirusBase virus)
        {
            base.OnUnitFire(index);
            units[index].SetReady(false);
            Vector2 cDir = virus.position - units[index].position;
            Vector2 dir = (cDir + virus.direction).normalized;
            virus.SetDirection(dir);
            Unibus.Dispatch(EventBullet.Get(EventBullet.Action.HIT, virus, damage));
            ExplosionWeaponCoverItem.Create().Reset(units[index].position, table.explosionSound);
        }

        private bool mLastIsTouchOn = false;
        protected override void Update()
        {
            base.Update();
            if (mLastIsTouchOn && !GlobalData.isBattleTouchOn)
            {
                shape = (shape + 1) % 2;
            }
            mLastIsTouchOn = GlobalData.isBattleTouchOn;

            if (shape == 0)
                unitRoot.localRotation *= Quaternion.AngleAxis(effects[0] * Time.deltaTime * GlobalData.slowDownFactor, Vector3.forward);
            else
                unitRoot.localRotation = Quaternion.identity;

            unitRoot.anchoredPosition = bindRoot.GetUIPos();
            unitRoot.localScale = Aircraft.ins.rectTransform.localScale;

            for (int i = 0; i < unitCount; i++)
            {
                units[i].SetShape(shape);
                units[i].SetFill(GetUnitFill(i));
            }
        }

        protected override float GetUnitFill(int index)
        {
            if (GameUtil.isInHome)
                return 0f;
            return base.GetUnitFill(index);
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