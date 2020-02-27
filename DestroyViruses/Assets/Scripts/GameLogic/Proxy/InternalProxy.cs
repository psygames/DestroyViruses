using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

namespace DestroyViruses
{
    public class InternalProxy : ProxyBase<InternalProxy>
    {
        protected override void OnInit()
        {
            base.OnInit();

            GameManager.Instance.StartCoroutine(Check());
        }

        private IEnumerator Check()
        {
            var date = PlayerPrefs.GetString("version_check_date", "");
            if (DateTime.Now.ToString("yyyy-MM-dd") == date)
            {
                Maintenance();
                yield break;
            }

            var req = UnityWebRequest.Get("http://39.105.150.229:8741/VersionCheck/index.html");
            req.timeout = 15;
            yield return req.SendWebRequest();
            if (req.isHttpError || req.isNetworkError)
            {
                yield break;
            }

            if (req.isDone)
            {
                PlayerPrefs.SetString("version_check_date", DateTime.Now.ToString("yyyy-MM-dd"));
                Maintenance();
            }
        }

        private void Maintenance()
        {
            UIManager.Open<MaintenanceView>(UILayer.Top);
        }
    }
}