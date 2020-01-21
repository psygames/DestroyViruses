using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VipRewardItem : MonoBehaviour
    {
        public Image icon;
        public Text text;

        public void SetData(Tuple<string, string> _data)
        {
            icon.SetSprite(_data.Item1);
            text.text = _data.Item2;
        }
    }
}