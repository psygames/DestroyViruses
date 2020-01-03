using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusCure : VirusBase
    {
        public ContentGroup linesGroup;
        public FadeGroup effectTrigger;
        private List<VirusBase> mViruses = new List<VirusBase>();

        public override void Reset(int id, float hp, int size, float speed, Vector2 pos, Vector2 direction, Vector2 hpRange, bool isMatrix)
        {
            base.Reset(id, hp, size, speed, pos, direction, hpRange, isMatrix);
            Update();
        }

        protected override void OnSkillTrigger()
        {
            base.OnSkillTrigger();
            foreach (var v in mViruses)
            {
                if (v != null && v.isAlive && !v.isHpFull)
                {
                    v.SetHp(v.hp + table.effect1 * v.hpTotal);
                    v.PlayCure();
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            for (int i = mViruses.Count - 1; i >= 0; i--)
            {
                var v = mViruses[i];
                if (v.isAlive && GetDist(v) > table.effect3)
                {
                    mViruses.RemoveAt(i);
                }
            }
            foreach (var v in EntityManager.GetAll<VirusBase>())
            {
                var virus = v as VirusBase;
                if (typeof(VirusCure) != virus.GetType()
                    && !mViruses.Contains(virus)
                    && GetDist(virus) <= table.effect2)
                {
                    mViruses.Add(virus);
                }
            }

            linesGroup.SetData<CureLine, VirusBase>(mViruses, (index, item, data) =>
            {
                item.SetData(colorIndex, data.colorIndex, (data.position - position) / scale);
            });
        }
    }
}