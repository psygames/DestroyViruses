using UnityEngine;
using UniRx;
using System;

namespace DestroyViruses
{
    // 关卡模式
    public class LevelMode : GameMode
    {
        private ConfigGameLevel mConfig = null;
        private bool mIsPause = false;
        private int mWave = 0;
        private int mSpawnIndex = 0;

        protected override void OnInit()
        {
            mConfig = ConfigGameLevel.Get(_ => _.level == GameLocalData.Instance.gameLevel);
            Aircraft.Create();
            OnBegin();
        }

        IDisposable virusCreator = null;
        protected override void OnBegin()
        {
            mSpawnIndex = 0;
            mWave = 0;
            Wave1();
        }

        private void Wave1()
        {
            mWave = 1;
            float inteval = FormulaUtil.WaveSpawnInterval(mWave, mConfig.spawnInterval);
            virusCreator = Observable.Interval(inteval).Do(_ =>
            {
                if (mSpawnIndex >= FormulaUtil.WaveVirusCount(mWave, mConfig.spawnCount))
                {
                    virusCreator.Dispose();
                    Wave2Wait();
                    return;
                }
                CreateVirus();
                mSpawnIndex++;
            }).Subscribe();
        }

        private void Wave2()
        {
            mWave = 2;
            float inteval = FormulaUtil.WaveSpawnInterval(mWave, mConfig.spawnInterval);
            virusCreator = Observable.Interval(inteval).Do(_ =>
            {
                if (mSpawnIndex >= mConfig.spawnCount)
                {
                    virusCreator.Dispose();
                    EndCheck();
                    return;
                }
                CreateVirus();
                mSpawnIndex++;
            }).Subscribe();
        }

        private void Wave2Wait()
        {
            virusCreator = Observable.EveryUpdate().Do(_ =>
            {
                if (EntityManager.Count<VirusBase>() <= 0)
                {
                    virusCreator.Dispose();
                    Wave2();
                }
            }).Subscribe();
        }

        private void EndCheck()
        {
            virusCreator = Observable.EveryUpdate().Do(_ =>
            {
                if (EntityManager.Count<VirusBase>() <= 0)
                {
                    virusCreator.Dispose();
                    Win();
                }
            }).Subscribe();
        }

        private void Win()
        {
            Debug.LogError("WIN");
        }

        private void Lose()
        {
            Debug.LogError("LOSE");
        }

        private void CreateVirus()
        {
            var virus = VirusBase.Create();
            var direction = Quaternion.AngleAxis(UnityEngine.Random.Range(-80f, 80f), Vector3.forward) * Vector2.down;
            var pos = new Vector2(UIUtil.width * 0.5f, UIUtil.height + VirusBase.radius);
            var hp = FormulaUtil.RandomInRanage(mConfig.virusHpRange);
            var size = FormulaUtil.RandomInProbArray(mConfig.virusSizeProbability) + 1;
            var speed = FormulaUtil.RandomInRanage(mConfig.virusSpeedRange);
            virus.Reset(hp, size, speed, pos, direction);
        }

        protected override void OnEnd()
        {
            virusCreator.Dispose();
        }

        protected override void OnQuit()
        {
            virusCreator.Dispose();
        }

        protected override void OnUpdate(float deltaTime)
        {

        }

        protected override void OnPause()
        {
            mIsPause = true;
        }

        protected override void OnResume()
        {
            mIsPause = false;
        }
    }
}