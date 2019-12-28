using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DestroyViruses
{
    public class HotUpdateState : StateBase
    {
        private AssetsUpdate updater = new AssetsUpdate();

        public float progress { get; private set; }
        public string message { get; private set; }

        private const string sQuickHotUpdateUrl = "http://39.105.150.229:8741/QuickHotUpdate/";
        private bool mQuickHotUpdateFinished = false;
        private int mQuickHotUpdateIndex = 0;
        private float mQuickHotUpdateProgress = 0;
        private UnityWebRequest mQuickHotUpdateRequest = null;
        private List<Type> mQuickHotUpdateList = new List<Type>();

        private void InitQuickHotUpdate()
        {
            mQuickHotUpdateRequest = null;
            mQuickHotUpdateFinished = false;
            mQuickHotUpdateIndex = 0;
            mQuickHotUpdateProgress = 0;
            mQuickHotUpdateList.Add(typeof(TableAdsCollection));
            mQuickHotUpdateList.Add(typeof(TableAircraftCollection));
            mQuickHotUpdateList.Add(typeof(TableBuffCollection));
            mQuickHotUpdateList.Add(typeof(TableBuffAutoGenCollection));
            mQuickHotUpdateList.Add(typeof(TableBuffKillGenCollection));
            mQuickHotUpdateList.Add(typeof(TableCoinIncomeCollection));
            mQuickHotUpdateList.Add(typeof(TableCoinValueCollection));
            mQuickHotUpdateList.Add(typeof(TableConstCollection));
            mQuickHotUpdateList.Add(typeof(TableDailySignCollection));
            mQuickHotUpdateList.Add(typeof(TableFirePowerCollection));
            mQuickHotUpdateList.Add(typeof(TableFireSpeedCollection));
            mQuickHotUpdateList.Add(typeof(TableGameLevelCollection));
            mQuickHotUpdateList.Add(typeof(TableGameWaveCollection));
            mQuickHotUpdateList.Add(typeof(TableLanguageCollection));
            mQuickHotUpdateList.Add(typeof(TableVirusCollection));
            mQuickHotUpdateList.Add(typeof(TableWeaponCollection));
            mQuickHotUpdateList.Add(typeof(TableWeaponPowerLevelCollection));
            mQuickHotUpdateList.Add(typeof(TableWeaponSpeedLevelCollection));
        }

        public override void OnEnter()
        {
            progress = 0;
            UIManager.Open<LoadingView>();
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
                message = LT.table.READY_UPDATE_RESOURCE;
                progress = 0.05f;
            }
            else if (updater.state == AssetsUpdate.State.Checking)
            {
                message = LT.table.CHECK_UPDATE_RESOURCE;
                progress = 0.1f;
            }
            else if (updater.state == AssetsUpdate.State.Downloading)
            {
                message = $"{LT.table.UPDATE_RESOURCE}[{updater.downloadIndex}/{updater.downloadCount}]";
                progress = updater.progress * 0.8f + 0.1f;
            }
            else if (updater.state == AssetsUpdate.State.Completed && !mQuickHotUpdateFinished)
            {
                message = $"{LT.table.QUICK_UPDATE_RESOURCE}[{mQuickHotUpdateIndex}/{mQuickHotUpdateList.Count}]";
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
                if (mQuickHotUpdateFinished)
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
                string url = sQuickHotUpdateUrl + mQuickHotUpdateList[mQuickHotUpdateIndex].Name.Replace("Collection", "") + ".bytes";
                mQuickHotUpdateRequest = UnityWebRequest.Get(url);
                mQuickHotUpdateRequest.SendWebRequest();
                mQuickHotUpdateProgress = 0;
            }
            else
            {
                if (mQuickHotUpdateRequest.isHttpError || mQuickHotUpdateRequest.isNetworkError)
                {
                    // Debug.Log("qucik hot update failed table: " + mQuickHotUpdateList[mQuickHotUpdateIndex]);
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
            var type = mQuickHotUpdateList[mQuickHotUpdateIndex];
            type.GetMethod("Load", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Invoke(null, new object[] { bytes });
            //Debug.Log("qucik hot update success table: " + mQuickHotUpdateList[mQuickHotUpdateIndex]);
        }

        public override void OnExit()
        {
            base.OnExit();
            UIManager.Close<LoadingView>();
        }
    }
}
