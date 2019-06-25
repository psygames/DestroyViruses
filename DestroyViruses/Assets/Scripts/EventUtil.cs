using UnityEngine;
using UnityEditor;
using QFramework;
using System;

namespace DestroyViruses
{
    public static class EventUtil
    {
        private static QEventSystem s_EventSystem = NonPublicObjectPool<QEventSystem>.Instance.Allocate();

        public static void RegisterEvent<T>(T msg, OnEvent process) where T : IConvertible
        {
            s_EventSystem.Register(msg, process);
        }

        public static void SendEvent<T>(T msg) where T : IConvertible
        {
            s_EventSystem.Send(msg);
        }

        public static void UnRegistEvent<T>(T msgEvent, OnEvent process) where T : IConvertible
        {
            s_EventSystem.UnRegister(msgEvent, process);
        }

    }
}