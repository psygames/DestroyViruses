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
        private static bool needCallback = false;

        public static void Check(UnityEngine.Events.UnityAction successCallback)
        {
#if UNITY_EDITOR
            return;
#endif
            if (isSuccess)
            {
                successCallback.Invoke();
                return;
            }

            if (!isInited)
            {
                mSuccessCallback.RemoveListener(successCallback);
                mSuccessCallback.AddListener(successCallback);

                if (!isIniting)
                {
                    isIniting = true;

                    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                    {
                        isIniting = false;
                        isInited = true;
                        var dependencyStatus = task.Result;
                        if (dependencyStatus == DependencyStatus.Available)
                        {
                            isSuccess = true;
                            needCallback = true;
                        }
                        else
                        {
                            isSuccess = false;
                            Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                        }
                    });
                }
            }
        }

        public static void Update(float deltaTime)
        {
            if (!isInited || !needCallback)
                return;
            mSuccessCallback.Invoke();
            needCallback = false;
        }
    }

}