using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class GameStart : MonoBehaviour
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
            LoadRes();
        }

        private void LoadRes()
        {
            // UIRoot
            // ResUtil.LoadSync<GameObject>("Resources/UI/UIRoot").Instantiate().Name("UIRoot");
            // MainPanel
            // UIMgr.OpenPanel<MainPanel>(prefabName: "Resources/UI/MainPanel");
            // MainPlayer
            // MainPlayer.Allocate();
        }
    }
}