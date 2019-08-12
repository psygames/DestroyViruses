using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusBomb : VirusBase
    {
        public Image shapeBase;
        public Image[] shapes;

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);

            shapeBase.SetSprite($"virus_shape_circle_{index}");
            foreach (var shape in shapes)
            {
                shape.SetSprite($"virus_shape_bomb_{index}");
            }
        }
    }
}