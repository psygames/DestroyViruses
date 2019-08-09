using UnityEngine;
using UniRx;
using UnibusEvent;

namespace DestroyViruses
{
    // 关卡模式
    public class LevelMode : GameMode
    {
        private WaveModule mWaveModule = new WaveModule();

        protected override void OnInit()
        {
            base.OnInit();
            mWaveModule.Init(GameLocalData.Instance.gameLevel);
            Unibus.Subscribe<EventAircraft>(OnEventAircraft);
        }

        protected override void OnQuit()
        {
            base.OnQuit();
            Unibus.Unsubscribe<EventAircraft>(OnEventAircraft);
        }

        protected override void OnBegin()
        {
            base.OnBegin();
            mLastIsFinalWave = false;
            Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.ActionType.GameBegin));
            mWaveModule.Start();
        }

        protected override void OnEnd(bool isWin)
        {
            base.OnEnd(isWin);
            if (isWin)
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.ActionType.GameEndWin));
            else
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.ActionType.GameEndLose));
            mWaveModule.Stop();
        }


        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            mWaveModule.Update(deltaTime);
            CheckGameState();
        }

        protected override void OnPause()
        {
            base.OnPause();
            mWaveModule.Pause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            mWaveModule.Resume();
        }


        private bool mLastIsFinalWave = false;
        private void CheckGameState()
        {
            if (mLastIsFinalWave && mWaveModule.isFinalWave)
            {
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.ActionType.FinalWave));
            }
            mLastIsFinalWave = mWaveModule.isFinalWave;

            if (mWaveModule.isFinalWave && mWaveModule.isStart
                && EntityManager.Count<VirusBase>() <= 0)
            {
                GameModeManager.Instance.End(true);
            }
        }

        private void OnEventAircraft(EventAircraft evt)
        {
            if (evt.action == EventAircraft.ActionType.Crash)
            {
                GameModeManager.Instance.End(false);
            }
        }


        public class WaveModule
        {
            public const int waveClearVirusCount = 3;

            public ConfigGameLevel configLevel { get; private set; }
            public ConfigGameVirusWave configWave { get; private set; }
            public int waveIndex { get; private set; }
            public bool isFinalWave { get { return waveIndex == configLevel.waveID.Length - 1; } }
            public int spawnCount { get { return (int)(configWave.spawnCount * configLevel.spawnCountFactor); } }
            public float spawnInterval { get { return (configWave.spawnInterval * configLevel.spawnIntervalFactor); } }

            public bool isStart { get; private set; }
            public bool isPause { get; private set; }

            private float mSpawnCD = 0;
            private int mSpawnIndex = 0;

            public void Init(int gameLevel)
            {
                Stop();
                configLevel = ConfigGameLevel.Get(_ => _.level == gameLevel);
            }

            public void Start()
            {
                SetWave(0);
                Resume();
                isStart = true;
            }

            public void Stop()
            {
                isStart = false;
            }

            public void Resume()
            {
                isPause = false;
            }

            public void Pause()
            {
                isPause = true;
            }

            public void SetWave(int waveIndex)
            {
                this.waveIndex = waveIndex;
                this.mSpawnCD = 0;
                this.mSpawnIndex = 0;
                configWave = ConfigGameVirusWave.Get(configLevel.waveID[waveIndex]);
            }

            public void Update(float deltaTime)
            {
                if (!isStart || isPause)
                    return;

                if (mSpawnIndex < spawnCount) // 产生病毒
                {
                    mSpawnCD = Mathf.Max(0, mSpawnCD - deltaTime);
                    if (mSpawnCD <= 0)
                    {
                        SpawnVirus();
                        //随机CD
                        mSpawnCD = Random.Range(spawnInterval / 0.8f, spawnInterval * 1.25f);
                        mSpawnIndex++;
                    }
                }
                else // 等待当前波结束结束
                {
                    // 非最终波
                    if (!isFinalWave && EntityManager.Count<VirusBase>() <= waveClearVirusCount)
                    {
                        SetWave(waveIndex + 1);
                    }
                }
            }


            private void SpawnVirus()
            {
                var direction = Quaternion.AngleAxis(Random.Range(-80f, 80f), Vector3.forward) * Vector2.down;
                var pos = new Vector2(Random.Range(VirusBase.radius, UIUtil.width - VirusBase.radius), UIUtil.height + VirusBase.radius);

                var hp = FormulaUtil.RandomInRanage(configWave.virusHpRange) * configLevel.virusHpFactor;
                var speed = FormulaUtil.RandomInRanage(configWave.virusSpeedRange) * configLevel.virusSpeedFactor;

                var virusIndex = FormulaUtil.RandomIndexInProbArray(configWave.virusProb);
                var virusType = "DestroyViruses.VirusTriangle";//ConfigVirus.Get(configWave.virus[virusIndex]).type;//TODO:VirusType
                var virus = (VirusBase)EntityManager.Create(System.Type.GetType(virusType));
                var size = configWave.virusSize[virusIndex];

                virus.Reset(hp, size, speed, pos, direction, configWave.virusHpRange);
            }

        }
    }
}