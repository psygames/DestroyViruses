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
            if (mEffectCD > 0) mRotSpeed = 360;
            rot1.rotation = Quaternion.AngleAxis(Time.deltaTime * mRotSpeed, Vector3.back) * rot1.rotation;
            rot2.rotation = Quaternion.AngleAxis(Time.deltaTime * mRotSpeed, Vector3.forward) * rot2.rotation;
        }

        protected override void OnSkillTrigger()
        {
            mEffectCD = table.effect2;
        }

        protected override void UpdatePosition()
        {
            var uiPos = UIUtil.GetUIPos(rectTransform);
            if (uiPos.y < -baseRadius)
            {
                position = new Vector2(rectTransform.anchoredPosition.x, UIUtil.height);
            }
            else if (uiPos.y > UIUtil.height - baseRadius)
            {
                direction = new Vector2(direction.x, -Mathf.Abs(direction.y));
            }
            if (uiPos.x < baseRadius)
            {
                direction = new Vector2(Mathf.Abs(direction.x), direction.y);
            }
            else if (uiPos.x > UIUtil.width - baseRadius)
            {
                direction = new Vector2(-Mathf.Abs(direction.x), direction.y);
            }

            float mulSpeed = 1f;
            if (mEffectCD > 0) mulSpeed = table.effect1;
            position += direction * speed * mulSpeed * Time.deltaTime;
            rectTransform.anchoredPosition = shakeOffset + position;
        }
    }
}