using UnityEngine;
using UniRx;
using UnibusEvent;

namespace DestroyViruses
{
    // 关卡模式
    public class LevelMode : GameMode
    {
        private WaveModule mWaveModule = new WaveModule();
        public float getCoin { get; private set; }
        public float progress { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();
            mWaveModule.Init(GDM.ins.gameLevel);
            Unibus.Subscribe<EventAircraft>(OnEventAircraft);
            Unibus.Subscribe<EventVirus>(OnEventVirus);
        }

        protected override void OnQuit()
        {
            base.OnQuit();
            Unibus.Unsubscribe<EventAircraft>(OnEventAircraft);
            Unibus.Unsubscribe<EventVirus>(OnEventVirus);
        }

        protected override void OnBegin()
        {
            base.OnBegin();
            getCoin = 0;
            progress = 0;
            mLastWaveIndex = -1;
            Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.GameBegin));
            mWaveModule.Start();
        }

        protected override void OnEnd(bool isWin)
        {
            base.OnEnd(isWin);
            if (isWin)
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.GameEndWin));
            else
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.GameEndLose));

            mWaveModule.Stop();

            // 解锁新关卡
            if (isWin && GDM.ins.gameLevel >= GDM.ins.unlockedGameLevel)
            {
                GDM.ins.UnlockNewLevel();
            }
        }


        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            mWaveModule.Update(deltaTime);
            CheckGameState();
            UpdateProgress();
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


        private int mLastWaveIndex = -1;
        private void CheckGameState()
        {
            if (mLastWaveIndex != mWaveModule.waveIndex && mWaveModule.isBossWave)
            {
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.BossWave));
            }
            mLastWaveIndex = mWaveModule.waveIndex;

            if (mWaveModule.isFinalWave && mWaveModule.isStart
                && EntityManager.Count<VirusBase>() <= 0
                && mWaveModule.isSpawnOver)
            {
                GameModeManager.Instance.End(true);
            }
        }

        private void UpdateProgress()
        {
            int total = 0;
            int spawnedTotal = mWaveModule.spawnIndex;
            for (int i = 0; i < mWaveModule.configLevel.waveID.Length; i++)
            {
                var configWave = TableGameWave.Get(mWaveModule.configLevel.waveID[i]);
                var waveCount = (int)(configWave.spawnCount * mWaveModule.configLevel.spawnCountFactor);
                total += waveCount;
                if (mWaveModule.waveIndex > i)
                {
                    spawnedTotal += waveCount;
                }
            }
            int killed = spawnedTotal - EntityManager.Count<VirusBase>(a => a.isMatrix);
            float _p = Mathf.Clamp01(1f * killed / total);
            if (_p > progress)
            {
                progress = _p;
            }
        }

        private void OnEventAircraft(EventAircraft evt)
        {
            if (evt.action == EventAircraft.Action.Crash)
            {
                GameModeManager.Instance.End(false);
            }
        }

        private float mAddCoinCount = 0;
        private void OnEventVirus(EventVirus evt)
        {
            if (evt.action == EventVirus.Action.DEAD)
            {
                // add coin
                getCoin += FormulaUtil.CoinConvert(evt.virus.size, mWaveModule.configLevel.coinValueFactor);
                mAddCoinCount += evt.virus.size * 0.1f;
                if (Random.value > ConstTable.table.coinAddProb[evt.virus.size - 1])
                {
                    var pos = UIUtil.GetUIPos(evt.virus.rectTransform);
                    int coinCount = Mathf.Clamp(Mathf.CeilToInt(mAddCoinCount), 1, 15);
                    Unibus.Dispatch(EventBattle.Get(EventBattle.Action.GET_COIN, coinCount, pos));
                    mAddCoinCount = 0;
                }
            }
        }

        public class WaveModule
        {
            public TableGameLevel configLevel { get; private set; }
            public TableGameWave configWave { get; private set; }
            public int waveIndex { get; private set; }
            public bool isBossWave { get { return configWave.bossWave; } }
            public bool isFinalWave { get { return waveIndex == configLevel.waveID.Length - 1; } }
            public bool isSpawnOver { get { return spawnIndex >= spawnCount; } }
            public bool needClear { get { return configWave.needClear; } }
            public int spawnCount { get { return (int)(configWave.spawnCount * configLevel.spawnCountFactor); } }
            public float spawnInterval { get { return (configWave.spawnInterval * configLevel.spawnIntervalFactor); } }
            public int spawnIndex { get; private set; }

            public bool isStart { get; private set; }
            public bool isPause { get; private set; }

            private float mSpawnCD = 0;

            public void Init(int gameLevel)
            {
                Stop();
                configLevel = TableGameLevel.Get(gameLevel);
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
                this.spawnIndex = 0;
                configWave = TableGameWave.Get(configLevel.waveID[waveIndex]);
            }

            public void Update(float deltaTime)
            {
                if (!isStart || isPause)
                    return;

                if (!isSpawnOver) // 产生病毒
                {
                    mSpawnCD = Mathf.Max(0, mSpawnCD - deltaTime);
                    if (mSpawnCD <= 0)
                    {
                        SpawnVirus();
                        //随机CD
                        mSpawnCD = spawnInterval * ConstTable.table.spawnVirusInterval.random;
                        spawnIndex++;
                    }
                }
                else // 等待当前波结束结束
                {
                    // 非最终波
                    if (!isFinalWave && (!needClear || EntityManager.Count<VirusBase>() <= ConstTable.table.waveClearVirusCount))
                    {
                        SetWave(waveIndex + 1);
                    }
                }
            }


            private void SpawnVirus()
            {
                var direction = Quaternion.AngleAxis(ConstTable.table.spawnVirusDirection.random, Vector3.forward) * Vector2.down;
                var pos = new Vector2(Random.Range(VirusBase.baseRadius, UIUtil.width - VirusBase.baseRadius), UIUtil.height + VirusBase.baseRadius);

                var virusIndex = FormulaUtil.RandomIndexInProbArray(configWave.virusProb);
                var virusTable = TableVirus.Get(configWave.virus[virusIndex]);
                var virusType = "DestroyViruses." + virusTable.type;
                var virus = (VirusBase)EntityManager.Create(System.Type.GetType(virusType));

                var hpRange = new Vector2(configLevel.hpRange.min, configLevel.hpRange.max);
                var hp = configWave.virusHp[virusIndex].random * configLevel.virusHpFactor * ConstTable.table.hpRandomRange.random;
                var speed = configWave.virusSpeed[virusIndex].random * configLevel.virusSpeedFactor * ConstTable.table.speedRandomRange.random;
                var size = configWave.virusSize[virusIndex].random;

                virus.Reset(virusTable.id, hp, size, speed, pos, direction, hpRange, true);
            }
        }
    }
}