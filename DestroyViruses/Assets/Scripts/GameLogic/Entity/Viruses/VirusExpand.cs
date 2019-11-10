using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusExpand : VirusBase
    {
        public Image triangleImage1;
        public FadeScale fadeScale;

        private float mEffectCD = 0;

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);

            triangleImage1.SetSprite($"virus_shape_expand_{index}");
        }

        protected override void OnSkillTrigger()
        {
            base.OnSkillTrigger();
            mEffectCD = table.effect2;
            fadeScale.from = Vector2.one * scale;
            fadeScale.to = Vector2.one * table.effect1 * scale;
            fadeScale.FadeIn();
        }

        protected override void Update()
        {
            base.Update();
            float last = mEffectCD;
            mEffectCD = this.UpdateCD(mEffectCD);
            if (last > 0 && mEffectCD <= 0)
            {
                fadeScale.FadeOut();
            }
        }
    }
}