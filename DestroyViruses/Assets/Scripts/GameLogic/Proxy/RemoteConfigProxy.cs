using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace DestroyViruses
{
    public class RemoteConfigProxy : ProxyBase<RemoteConfigProxy>
    {
        public bool isInit { get; private set; }
        Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
        private string lastConstGroup;

        protected override void OnInit()
        {
            base.OnInit();
            lastConstGroup = GameLocalData.Instance.lastConstGroup;
        }

        public void InitAfterCheck()
        {
#if UNITY_EDITOR
            isInit = true;
#else

            InitializeFirebase();
#endif
        }

        Task InitializeFirebase()
        {
            var defaults = new Dictionary<string, object>();
            defaults.Add("const_group", lastConstGroup);
            Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
            var fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync();
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        void FetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
            {
                Debug.LogError("Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.LogError("Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed successfully!");
            }

            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
            switch (info.LastFetchStatus)
            {
                case Firebase.RemoteConfig.LastFetchStatus.Success:
                    Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                    isInit = true;
                    Debug.Log(string.Format("Remote data loaded and ready (last fetch time {0}).", info.FetchTime));
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case Firebase.RemoteConfig.FetchFailureReason.Error:
                            Debug.LogError("Fetch failed for unknown reason");
                            break;
                        case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                            Debug.LogError("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Pending:
                    Debug.LogError("Latest Fetch call still pending.");
                    break;
            }
        }

        private bool mLastInit = false;
        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (!mLastInit && isInit)
            {
                var constGroup = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("const_group").StringValue;
                if (!string.IsNullOrEmpty(constGroup))
                {
                    GameLocalData.Instance.lastConstGroup = constGroup;
                    GameLocalData.Instance.Save();
                }
            }
            mLastInit = isInit;
        }
    }
}
