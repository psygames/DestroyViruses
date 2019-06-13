using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace DestroyViruses
{
    public class GameManager : MonoSingleton<GameManager>, IManager
    {
        private void Start()
        {
            Init();
        }
        
        public void Init()
        {
            GameStart();
        }

        public void RegisterEvent<T>(T msgId, OnEvent process) where T : IConvertible
        {
        }

        public void SendEvent<T>(T eventId) where T : IConvertible
        {
        }

        public void SendMsg(QMsg msg)
        {
        }

        public void UnRegistEvent<T>(T msgEvent, OnEvent process) where T : IConvertible
        {
        }

        private void GameStart()
        {
            UIMgr.OpenPanel<MainPanel>(prefabName: "Resources/UI/MainPanel");
        }
    }
}