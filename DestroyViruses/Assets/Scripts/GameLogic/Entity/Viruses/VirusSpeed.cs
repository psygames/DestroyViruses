using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusSpeed : VirusBase
    {
        public RectTransform rot1;
        public RectTransform rot2;
        public Image triangleImage1;
        public Image triangleImage2;

        private float mRotSpeed = 2;
        private float mEffectCD = 0;

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);

            triangleImage1.SetSprite($"virus_shape_speed_{index}");
            triangleImage2.SetSprite($"virus_shape_speed_{index}");
        }

        protected override void Update()
        {
            base.Update();
            mEffectCD = this.UpdateCD(mEffectCD);
            mRotSpeed = 90;
            if (mEffectCD > 0) mRotSpeed = 1000;
            rot1.rotation = Quaternion.AngleAxis(Time.deltaTime * mRotSpeed, Vector3.back) * rot1.rotation;
            rot2.rotation = Quaternion.AngleAxis(Time.deltaTime * mRotSpeed, Vector3.forward) * rot2.rotation;
            speedMul = mEffectCD > 0 ? table.effect1 : 1f;
        }

        protected override void OnSkillTrigger()
        {
            mEffectCD = table.effect2;
        }
    }
}