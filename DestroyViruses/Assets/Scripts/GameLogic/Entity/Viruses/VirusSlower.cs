using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusSlower : VirusBase
    {
        public RectTransform line;
        public Image triangleImage1;

        private bool mIsSlow = false;
        private Aircraft aircraft { get { return Aircraft.ins; } }

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);

            triangleImage1.SetSprite($"virus_shape_slower_{index}");
        }

        protected override void Update()
        {
            if (GameUtil.isFrozen)
            {
                return;
            }
            base.Update();
            var dist = (aircraft.headPosition - position).magnitude - radius;
            bool triggerSlow = dist <= table.effect2;
            bool triggerSlowOff = dist > table.effect3;

            if (!mIsSlow && triggerSlow)
            {
                aircraft.movement.moveSpeedRatio *= (1f - table.effect1);
                mIsSlow = true;
            }
            else if (mIsSlow && triggerSlowOff)
            {
                aircraft.movement.moveSpeedRatio /= (1f - table.effect1);
                mIsSlow = false;
            }

            if (mIsSlow)
            {
                line.gameObject.SetActive(true);
                var dir = aircraft.headPosition - position;
                line.sizeDelta = new Vector2(line.sizeDelta.x, dir.magnitude / scale);
                line.localRotation = Quaternion.FromToRotation(Vector2.down, dir);
            }
            else
            {
                line.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (mIsSlow)
            {
                aircraft.movement.moveSpeedRatio /= (1f - table.effect1);
                mIsSlow = false;
            }
        }
    }
}
