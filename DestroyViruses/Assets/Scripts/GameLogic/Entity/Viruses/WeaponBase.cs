using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnibusEvent;
using UniRx;

namespace DestroyViruses
{
    public class WeaponBase : EntityBase
    {
        public int id { get; private set; }
        public TableWeapon table { get; private set; }
        public TableWeaponPowerLevel tablePower { get; private set; }
        public TableWeaponSpeedLevel tableSpeed { get; private set; }
        public float rechargeDuration { get; private set; }

        public float damage { get; protected set; }
        public float effect1 { get; protected set; }
        public float effect2 { get; protected set; }
        public float effect3 { get; protected set; }

        // override if need
        public virtual bool autoFire { get; } = true;
        public virtual int unitCount { get; } = 1;


        protected int mUnitIndex;
        protected List<float> mUnitCD = new List<float>();

        public virtual void Reset(int id, int powerLevel, int speedLevel)
        {
            this.id = id;
            if (id <= 0) return;
            table = TableWeapon.Get(id);
            tablePower = TableWeaponPowerLevel.Get(_ => _.weaponId == id && _.level == powerLevel);
            tableSpeed = TableWeaponSpeedLevel.Get(_ => _.weaponId == id && _.level == speedLevel);
            rechargeDuration = tableSpeed.recharge;
            damage = tablePower.damage;
            effect1 = table.effect1 * tablePower.effectFactor1 * tableSpeed.effectFactor1;
            effect2 = table.effect2 * tablePower.effectFactor2 * tableSpeed.effectFactor2;
            effect3 = table.effect3 * tablePower.effectFactor3 * tableSpeed.effectFactor3;
            mUnitIndex = 0;
            mUnitCD.Clear();
            for (int i = 0; i < unitCount; i++)
            {
                mUnitCD.Add(rechargeDuration);
            }
        }

        protected virtual void OnUnitReady(int index)
        {
            if (autoFire)
            {
                OnUnitFire(index);
            }
        }

        protected virtual void OnUnitFire(int index)
        {
            mUnitCD[index] = rechargeDuration;
        }

        protected virtual void UpdateCD()
        {
            if (rechargeDuration <= 0)
                return;

            if (mUnitCD[mUnitIndex] <= 0)
            {
                mUnitIndex = (mUnitIndex + 1) % unitCount;
                return;
            }

            mUnitCD[mUnitIndex] = this.UpdateCD(mUnitCD[mUnitIndex], GlobalData.slowDownFactor);
            if (mUnitCD[mUnitIndex] <= 0)
            {
                OnUnitReady(mUnitIndex);
                mUnitIndex = (mUnitIndex + 1) % unitCount;
            }
        }

        protected float GetUnitCD(int index)
        {
            if (mUnitCD.Count > index)
                return mUnitCD[index];
            return 0;
        }

        protected float GetUnitFill(int index)
        {
            if (GameUtil.isInHome || rechargeDuration <= 0)
                return 1f;
            return 1f - GetUnitCD(index) / rechargeDuration;
        }

        protected virtual void Update()
        {
            if (GameUtil.isFrozen)
            {
                return;
            }

            UpdateCD();
        }
    }
}