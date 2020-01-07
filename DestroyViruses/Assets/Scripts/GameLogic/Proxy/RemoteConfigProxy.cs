using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class RemoteConfigProxy : ProxyBase<RemoteConfigProxy>
    {
        protected override void OnInit()
        {
            base.OnInit();
            var defaults = new Dictionary<string, object>();
            defaults.Add("const_group", "default");
            Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void FetchAsync()
        {
        }
    }
}