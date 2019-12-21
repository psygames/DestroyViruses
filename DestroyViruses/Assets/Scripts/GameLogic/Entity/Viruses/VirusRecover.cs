using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusRecover : VirusBase
    {
        public Image[] buffs;
        public FadeGroup effectTrigger;

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);

            foreach (var buff in buffs)
            {
                buff.SetSprite($"cure_buff_icon_{index}");
            }
        }

        protected override void OnSkillTrigger()
        {
            base.OnSkillTrigger();

            SetHp(hp + table.effect1 * hpTotal);
            effectTrigger.FadeIn();
        }
    }
}