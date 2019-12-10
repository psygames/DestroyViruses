#if mopub_manager
using UnityEngine;

public class FacebookAudienceNetworkNetworkConfig : MoPubNetworkConfig
{
    public override string AdapterConfigurationClassName
    {
        get { return Application.platform == RuntimePlatform.Android
                  ? "com.mopub.mobileads.FacebookAdapterConfiguration"
                  : "FacebookAdapterConfiguration"; }
    }
}
#endif
