using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusDefense : VirusBase
    {
        public Image triangleImage1;

        private float mEffectCD;

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);

            triangleImage1.SetSprite($"virus_shape_defense_{index}");
        }

        protected override void OnSkillTrigger()
        {
            base.OnSkillTrigger();
            mEffectCD = table.effect1;
        }

        protected override void Update()
        {
            base.Update();
            mEffectCD = this.UpdateCD(mEffectCD);
            hpText.gameObject.SetActive(!isInvincible);
        }
    }
}