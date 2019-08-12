using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusCure : VirusBase
    {
        public Image[] shapes;

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);
            foreach (var shape in shapes)
            {
                shape.SetSprite($"virus_shape_cure_{index}");
            }
        }
    }
}