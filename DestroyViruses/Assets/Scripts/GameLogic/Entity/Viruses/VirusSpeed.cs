using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusSpeed : VirusBase
    {
        public Image triangleImage1;
        public Image triangleImage2;

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);

            triangleImage1.SetSprite($"virus_shape_speed_{index}");
            triangleImage2.SetSprite($"virus_shape_speed_{index}");
        }
    }
}