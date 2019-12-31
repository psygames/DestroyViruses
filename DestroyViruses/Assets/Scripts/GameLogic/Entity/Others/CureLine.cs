using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class CureLine : MonoBehaviour
    {
        public LineRenderer line;

        public static Color[] colors = {
            new Color(233 / 255f, 215 / 255f, 70 / 255f),
            new Color(132 / 255f, 223 / 255f, 70 / 255f),
            new Color(64 / 255f, 218 / 255f, 222 / 255f),
            new Color(69 / 255f, 161 / 255f, 224 / 255f),
            new Color(187 / 255f, 70 / 255f, 220 / 255f),
            new Color(220 / 255f, 83 / 255f, 71 / 255f),
        };

        public void SetData(int colorIndex,int endColorIndex, Vector2 toPos)
        {
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, toPos);
            line.startColor = colors[colorIndex];
            line.endColor = colors[endColorIndex];
        }
    }
}