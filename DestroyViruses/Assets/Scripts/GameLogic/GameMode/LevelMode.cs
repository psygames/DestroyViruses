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
        private int mSpawnIndex = 0;

        protected override void OnInit()
        {
            mConfig = ConfigGameLevel.Get(_ => _.level == GameLocalData.Instance.gameLevel);
            Aircraft.Create();
        }

        IDisposable virusCreator = null;
        protected override void OnBegin()
        {
            mWave = 1;
            virusCreator = Observable.Interval(TimeSpan.FromSeconds(mConfig.spawnInterval)).Do((ticks) =>
            {
                
            }).Subscribe();
        }

        private void CreateVirus()
        {
            var virus = VirusBase.Create();
            var direction = Quaternion.AngleAxis(UnityEngine.Random.Range(-80f, 80f), Vector3.forward) * Vector2.down;
            var pos = new Vector2(UIUtil.width * 0.5f, UIUtil.height + VirusBase.radius);
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

        protected override void OnPause()
        {
        }

        protected override void OnResume()
        {
        }
    }
}