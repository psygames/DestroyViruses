using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusCure : VirusBase
    {
        public Image[] lines;
        public Image[] buffs;

        public FadeGroup effectTrigger;

        private List<VirusBase> mViruses = new List<VirusBase>();

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);
            foreach (var line in lines)
            {
                line.SetSprite($"cure_line_{index}");
            }
            foreach (var buff in buffs)
            {
                buff.SetSprite($"cure_buff_icon_{index}");
            }
        }

        protected override void OnSkillTrigger()
        {
            base.OnSkillTrigger();
            foreach (var v in mViruses)
            {
                if (v != null && v.isAlive)
                {
                    v.SetHp(v.hp + table.effect1 * v.hpTotal);
                }
            }
            effectTrigger.FadeIn();
        }

        protected override void Update()
        {
            base.Update();

            mViruses.Clear();
            foreach (var v in EntityManager.GetAll<VirusBase>())
            {
                var virus = v as VirusBase;
                if (typeof(VirusCure) != virus.GetType()
                    && GetDist(virus) <= table.effect2)
                {
                    mViruses.Add(virus);
                }
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if (i < mViruses.Count)
                {
                    lines[i].gameObject.SetActive(true);
                    SetLine(lines[i], mViruses[i]);
                }
                else
                {
                    lines[i].gameObject.SetActive(false);
                }
            }
        }

        private void SetLine(Image img, VirusBase v)
        {
            var rec = img.rectTransform;
            var dir = v.position - position;
            rec.sizeDelta = new Vector2(dir.magnitude / scale, rec.sizeDelta.y);
            rec.localRotation = Quaternion.FromToRotation(Vector2.right, dir);
        }
    }
}