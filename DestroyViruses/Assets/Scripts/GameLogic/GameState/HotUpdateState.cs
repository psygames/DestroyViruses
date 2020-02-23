﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DestroyViruses
{
    public class HotUpdateState : StateBase
    {
        private AssetsUpdate updater = new AssetsUpdate();

        public float progress { get { return 0; } private set { LoadingView.SetProgress(value); } }
        public string message { get { return ""; } private set { LoadingView.SetMessage(value); } }

#if !UNITY_EDITOR && PUBLISH_BUILD
        public const bool ENABLE_QUICK_HOT_UPDATE = false;
#else
        public const bool ENABLE_QUICK_HOT_UPDATE = true;
#endif

        private const string sQuickHotUpdateUrl = "http://39.105.150.229:8741/QuickHotUpdate/";
        private bool mQuickHotUpdateFinished = false;
        private int mQuickHotUpdateIndex = 0;
        private float mQuickHotUpdateProgress = 0;
        private UnityWebRequest mQuickHotUpdateRequest = null;
        private List<string> mQuickHotUpdateList = new List<string>();

        private void InitQuickHotUpdate()
        {
            mQuickHotUpdateRequest = null;
            mQuickHotUpdateFinished = false;
            mQuickHotUpdateIndex = 0;
            mQuickHotUpdateProgress = 0;
            mQuickHotUpdateList.Clear();
            foreach (var t in TableList.GetAll())
            {
                mQuickHotUpdateList.Add(t.id);
            }
        }

        public override void OnEnter()
        {
            UIManager.Open<LoadingView>();
            progress = 0;
            message = "";
            updater.Init();
            InitQuickHotUpdate();
            base.OnEnter();
        }

        private void UpdateMessage()
        {
            if (updater.state == AssetsUpdate.State.Error)
            {
                message = $"{"ERROR: "}{updater.message}";
                progress = 0.1f;
            }
            else if (updater.state == AssetsUpdate.State.Wait)
            {
                message = LT.Get("READY_UPDATE_RESOURCE");
                progress = 0.05f;
            }
            else if (updater.state == AssetsUpdate.State.Checking)
            {
                message = LT.Get("CHECK_UPDATE_RESOURCE");
                progress = 0.1f;
            }
            else if (updater.state == AssetsUpdate.State.Downloading)
            {
                message = $"{LT.Get("UPDATE_RESOURCE")}[{updater.downloadIndex}/{updater.downloadCount}]";
                progress = updater.progress * 0.8f + 0.1f;
            }
            else if (updater.state == AssetsUpdate.State.Completed && !mQuickHotUpdateFinished)
            {
                message = $"{LT.Get("QUICK_UPDATE_RESOURCE")}[{mQuickHotUpdateIndex}/{mQuickHotUpdateList.Count}]";
                progress = (1f * mQuickHotUpdateIndex / mQuickHotUpdateList.Count + mQuickHotUpdateProgress) * 0.1f + 0.9f;
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            updater.Update();
            UpdateMessage();

            if (updater.state == AssetsUpdate.State.Wait)
            {
                updater.Check();
            }
            if (updater.state == AssetsUpdate.State.Error
                && (updater.message == "Cannot connect to destination host"
                || updater.message.Contains("404")))
            {
                updater.state = AssetsUpdate.State.Completed;
            }
            else if (updater.state == AssetsUpdate.State.Completed)
            {
                if (!ENABLE_QUICK_HOT_UPDATE || mQuickHotUpdateFinished)
                {
                    StateManager.ChangeState<LoadingState>();
                }
                else
                {
                    UpdateQuickHotUpdate();
                }
            }
        }

        private void UpdateQuickHotUpdate()
        {
            if (!Plugins.XAsset.Utility.assetBundleMode)
            {
                mQuickHotUpdateFinished = true;
                return;
            }

            if (mQuickHotUpdateIndex >= mQuickHotUpdateList.Count)
            {
                mQuickHotUpdateFinished = true;
                return;
            }

            if (mQuickHotUpdateRequest == null)
            {
                string url = sQuickHotUpdateUrl + mQuickHotUpdateList[mQuickHotUpdateIndex] + ".bytes";
                mQuickHotUpdateRequest = UnityWebRequest.Get(url);
                mQuickHotUpdateRequest.SendWebRequest();
                mQuickHotUpdateProgress = 0;
            }
            else
            {
                if (mQuickHotUpdateRequest.isHttpError || mQuickHotUpdateRequest.isNetworkError)
                {
                    mQuickHotUpdateRequest.Dispose();
                    mQuickHotUpdateRequest = null;
                    mQuickHotUpdateIndex++;
                    return;
                }

                if (!mQuickHotUpdateRequest.isDone)
                {
                    mQuickHotUpdateProgress = mQuickHotUpdateRequest.downloadProgress;
                }
                else
                {
                    var bytes = mQuickHotUpdateRequest.downloadHandler.data;
                    LoadToTable(bytes);
                    mQuickHotUpdateRequest.Dispose();
                    mQuickHotUpdateRequest = null;
                    mQuickHotUpdateIndex++;
                }
            }
        }

        private void LoadToTable(byte[] bytes)
        {
            var type = Type.GetType(mQuickHotUpdateList[mQuickHotUpdateIndex] + "Collection");
            type.GetMethod("Load", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Invoke(null, new object[] { bytes });
        }

        public override void OnExit()
        {
            UIManager.Close<LoadingView>();
            base.OnExit();
        }
    }
}
