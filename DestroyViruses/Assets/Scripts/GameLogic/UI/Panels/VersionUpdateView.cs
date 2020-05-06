using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class VersionUpdateView : ViewBase
    {
        public Text content;
        public ButtonPro cancelBtn;
        
        private bool enableCancel = true;

        protected override void OnOpen()
        {
            base.OnOpen();
            var min = System.Version.Parse(GameLocalData.Instance.minVersion);
            var latest = System.Version.Parse(GameLocalData.Instance.latestVersion);
            var current = System.Version.Parse(Application.version);
            if (current < min)
            {
                content.text = LTKey.UPDATE_CONTENT_FORCE.LT(Application.version, GameLocalData.Instance.minVersion);
                enableCancel = false;
            }
            else if (current < latest)
            {
                content.text = LTKey.UPDATE_CONTENT.LT(Application.version, GameLocalData.Instance.latestVersion);
                enableCancel = true;
            }
            cancelBtn.SetBtnGrey(true);
        }

        private void OnClickConfirm()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            Application.OpenURL("market://details?id=" + Application.identifier);
#else
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
#endif
        }

        private void OnClickCancel()
        {
            if (!enableCancel)
                return;
            Close();
        }
    }
}