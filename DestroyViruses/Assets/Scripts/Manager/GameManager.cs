using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace DestroyViruses
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            GameStart();
        }

        private void GameStart()
        {
            LoadRes();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void LoadRes()
        {
            // UIRoot
            ResUtil.LoadSync<GameObject>("Resources/UI/UIRoot").Instantiate().Name("UIRoot");
            // MainPanel
            UIMgr.OpenPanel<MainPanel>(prefabName: "Resources/UI/MainPanel");
            // MainPlayer
            MainPlayer.Allocate();
        }
    }
}