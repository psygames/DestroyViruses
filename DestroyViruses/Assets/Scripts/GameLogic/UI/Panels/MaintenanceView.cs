using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class MaintenanceView : ViewBase
    {
        public Slider fill;
        public Text secondText;

        private float closeCD;

        protected override void OnOpen()
        {
            base.OnOpen();
            closeCD = 10;
        }

        private void Update()
        {
            closeCD = this.UpdateCD(closeCD);
            if (closeCD <= 0)
            {
                Application.Quit();
            }

            fill.value = closeCD / 10f;
            secondText.text = $"Game will CLOSE in {(int)closeCD} seconds";
        }

        private void OnClickWebsite()
        {
            Application.OpenURL("http://39.105.150.229:8741/VersionCheck/index.html");
        }
    }
}