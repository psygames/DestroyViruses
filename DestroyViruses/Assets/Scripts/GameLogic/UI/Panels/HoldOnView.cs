using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnibusEvent;
using System.IO;

namespace DestroyViruses
{
    public class HoldOnView : ViewBase
    {
    }

    public static class HoldOn
    {
        private static bool isOpening = false;

        public static void Start()
        {
            if (!isOpening)
            {
                isOpening = true;
                UIManager.Open<HoldOnView>(UILayer.Top);
            }
        }

        public static void Stop()
        {
            if (isOpening)
            {
                isOpening = false;
                UIManager.Close<HoldOnView>();
            }
        }
    }
}