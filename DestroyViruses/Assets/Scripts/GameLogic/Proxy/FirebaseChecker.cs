using System;
using Firebase;
using UnityEngine;

namespace DestroyViruses
{
    public static class FirebaseChecker
    {
        public static bool isInited { get; private set; }
        public static bool isSuccess { get; private set; }

        private static UnityEngine.Events.UnityEvent mSuccessCallback = new UnityEngine.Events.UnityEvent();
        private static bool isIniting = false;

        public static void Check(UnityEngine.Events.UnityAction successCallback)
        {
#if UNITY_EDITOR || !UNITY_ANDROID
            return;
#else
            GameManager.Instance.DelayDo(0.5f, () =>
            {
                 if (!isInited)
                 {
                     mSuccessCallback.AddListener(successCallback);

                     if (!isIniting)
                     {
                         isIniting = true;

                         FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                         {
                             isInited = true;
                             var dependencyStatus = task.Result;
                             if (dependencyStatus == DependencyStatus.Available)
                             {
                                 isSuccess = true;
                                 mSuccessCallback.Invoke();
                             }
                             else
                             {
                                 isSuccess = false;
                                 Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                             }
                         });
                     }
                 }
                 else if (isSuccess)
                 {
                     successCallback.Invoke();
                 }
             });
#endif
        }
    }

}