using UnityEngine;
using System.Collections;
using UniRx;
using System;

namespace DestroyViruses
{
    // 关卡模式
    public class LevelMode : GameMode
    {
        private ConfigGameLevel mConfig = null;
        private int mWave = 0;
        private int mWaveSpawnIndex = 0;

        protected override void OnInit()
        {
            mConfig = ConfigGameLevel.Get(_ => _.level == GameLocalData.Instance.gameLevel);
            Aircraft.Create();
            OnBegin();
        }

        IDisposable virusCreator = null;
        protected override void OnBegin()
        {
            mWave = 1;
            virusCreator = Observable.Interval(TimeSpan.FromSeconds(mConfig.spawnInterval)).Do((ticks) =>
            {
                var virus = VirusBase.Create();
                virus.Reset(new Vector2(UIUtil.width * 0.5f, UIUtil.height));
            }).Subscribe();
        }

        protected override void OnEnd()
        {
            virusCreator.Dispose();
        }

        protected override void OnQuit()
        {

        }

        protected override void OnUpdate(float deltaTime)
        {

        }
    }
}