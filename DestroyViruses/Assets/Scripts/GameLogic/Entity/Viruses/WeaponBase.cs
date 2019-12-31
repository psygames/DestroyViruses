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
        public const float baseRadius = 125f;

        public int id { get; protected set; }
        public TableWeapon table { get; protected set; }
        public TableWeaponPowerLevel tablePower { get; protected set; }
        public TableWeaponSpeedLevel tableSpeed { get; protected set; }
        public float cd { get; protected set; }

        protected virtual void Awake()
        {
        }

        protected virtual void OnEnable()
        {
        }

        public virtual void Reset(int id, int powerLevel,int speedLevel)
        {
            this.id = id;
            table = TableWeapon.Get(id);
            tablePower = TableWeaponPowerLevel.Get(_ => _.weaponId == id && _.level == powerLevel);
            tableSpeed = TableWeaponSpeedLevel.Get(_ => _.weaponId == id && _.level == powerLevel);
            cd = tableSpeed.recharge;
        }

        protected VirusBase Create()
        {
            return EntityManager.Create(GetType()) as VirusBase;
        }

        protected virtual void OnSkillTrigger()
        {

        }

        protected virtual void UpdateCD()
        {
            if (tableSpeed.recharge <= 0)
                return;

            cd = this.UpdateCD(cd, GlobalData.slowDownFactor);
            if (cd <= 0)
            {
                cd = tableSpeed.recharge;
                OnSkillTrigger();
            }
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