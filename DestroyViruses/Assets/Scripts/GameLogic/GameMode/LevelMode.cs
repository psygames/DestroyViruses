using UnityEngine;
using UniRx;
using UnibusEvent;
using System.Collections.Generic;

namespace DestroyViruses
{
    // 关卡模式
    public class LevelMode : GameMode
    {
        private WaveModule mWaveModule = new WaveModule();
        private BuffGenModule mBuffGenModule = new BuffGenModule();
        public float getCoin { get; private set; }
        public float progress { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();
            mWaveModule.Init(GDM.ins.gameLevel, GDM.ins.firePower);
            mBuffGenModule.Init(GDM.ins.gameLevel, GDM.ins.streak);
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
            Analytics.Event.GameBegin(GDM.ins.gameLevel);
            getCoin = 0;
            progress = 0;
            mLastWaveIndex = -1;
            GDM.ins.adRevive = false;
            Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.GameBegin));
            mWaveModule.Start();
        }

        protected override void OnEnd(bool isWin)
        {
            base.OnEnd(isWin);
            Analytics.Event.GameEnd(GDM.ins.gameLevel, isWin, progress);
            mWaveModule.Stop();
            GDM.ins.BattleEnd(isWin);
            if (isWin)
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.GameEndWin));
            else
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.GameEndLose));
        }


        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            mBuffGenModule.Update(progress);
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
            for (int i = 0; i < mWaveModule.tableGameLevel.waveID.Length; i++)
            {
                var configWave = TableGameWave.Get(mWaveModule.tableGameLevel.waveID[i]);
                var waveCount = (int)(configWave.spawnCount * mWaveModule.spawnCountFixFactor * mWaveModule.tableGameLevel.spawnCountFactor);
                total += waveCount;
                if (mWaveModule.waveIndex > i)
                {
                    spawnedTotal += waveCount;
                }
            }
            float killed = spawnedTotal - EntityManager.Count<VirusBase>(a => a.isMatrix) - EntityManager.Count<VirusBase>(a => !a.isMatrix) * 0.5f;
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
                var factor = mWaveModule.tableGameLevel.coinValueFactor + ProxyManager.GetProxy<BuffProxy>().Effect_Coin;
                getCoin += FormulaUtil.CoinConvert(evt.virus.size, factor);
                mAddCoinCount += evt.virus.size * 0.1f;
                if (Random.value > ConstTable.table.coinAddProb[evt.virus.size - 1]
                    || ProxyManager.GetProxy<BuffProxy>().Has_Effect_Coin)
                {
                    var pos = UIUtil.GetUIPos(evt.virus.rectTransform);
                    int coinCount = Mathf.Clamp(Mathf.CeilToInt(mAddCoinCount), 1, 15);
                    Unibus.Dispatch(EventBattle.Get(EventBattle.Action.GET_COIN, coinCount, pos));
                    mAddCoinCount = 0;
                }

                // virus kills 4 buff
                GDM.ins.kills4Buff += 1;
            }
        }

        public class WaveModule
        {
            public TableGameLevel tableGameLevel { get; private set; }
            public TableGameWave tableGameWave { get; private set; }

            public int waveIndex { get; private set; }
            public int spawnIndex { get; private set; }

            public bool isStart { get; private set; }
            public bool isPause { get; private set; }

            public bool isBossWave { get { return tableGameWave.bossWave; } }
            public bool isFinalWave { get { return waveIndex == tableGameLevel.waveID.Length - 1; } }
            public bool isSpawnOver { get { return spawnIndex >= spawnCount; } }
            public bool needClear { get { return tableGameWave.needClear; } }
            public int spawnCount { get { return (int)(tableGameWave.spawnCount * tableGameLevel.spawnCountFactor * mSpawnCountFixFactor); } }
            public float spawnInterval { get { return (tableGameWave.spawnInterval * tableGameLevel.spawnIntervalFactor); } }

            private float mFirePower = 0f;
            private float mSpawnCD = 0;

            private float mSpawnCountFixFactor = 1;
            private float mHpFixFactor = 1;
            private float mSpeedFixFactor = 1;

            public float spawnCountFixFactor { get { return mSpawnCountFixFactor; } }

            public void Init(int gameLevel, float firePower)
            {
                mFirePower = firePower;
                tableGameLevel = TableGameLevel.Get(gameLevel);
                Stop();
                ResetFixFactor();
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
                tableGameWave = TableGameWave.Get(tableGameLevel.waveID[waveIndex]);
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

                var virusIndex = FormulaUtil.RandomIndexInProbArray(tableGameWave.virusProb);
                var virusTable = TableVirus.Get(tableGameWave.virus[virusIndex]);
                var virusType = "DestroyViruses." + virusTable.type;
                var virus = (VirusBase)EntityManager.Create(System.Type.GetType(virusType));

                var hpRange = new Vector2(tableGameLevel.hpRange.min, tableGameLevel.hpRange.max);
                var hp = tableGameWave.virusHp[virusIndex].random * tableGameLevel.virusHpFactor * mHpFixFactor * ConstTable.table.hpRandomRange.random;
                var speed = tableGameWave.virusSpeed[virusIndex].random * tableGameLevel.virusSpeedFactor * mSpeedFixFactor * ConstTable.table.speedRandomRange.random;
                var size = tableGameWave.virusSize[virusIndex].random;

                virus.Reset(virusTable.id, hp, size, speed, pos, direction, hpRange, true);
            }

            private void ResetFixFactor()
            {
                if (mFirePower <= tableGameLevel.firePowerLimitation)
                {
                    mHpFixFactor = 1;
                    mSpeedFixFactor = 1;
                    mSpawnCountFixFactor = 1;
                }
                else
                {
                    var r = mFirePower / tableGameLevel.firePowerLimitation;
                    var r1 = Mathf.Sqrt(r);
                    mHpFixFactor = r;
                    mSpawnCountFixFactor = r1;
                    mSpeedFixFactor = 1;
                }
            }
        }


        public class BuffGenModule
        {
            private List<float> mGenProgress = new List<float>();
            private TableBuffAutoGen mTable;
            private int mGameLevel;
            private int mStreak;

            public void Init(int gameLevel, int streak)
            {
                mGameLevel = gameLevel;
                mStreak = streak;
                mGenProgress.Clear();
                mTable = TableBuffAutoGen.Get(a => a.gameLevel.Contains(mGameLevel) && a.streak == mStreak);
                var _count = mTable.buffCount.random;
                for (int i = 0; i < _count; i++)
                {
                    mGenProgress.Add(Random.value);
                }
            }

            public void Update(float progress)
            {
                for (int i = mGenProgress.Count - 1; i >= 0; i--)
                {
                    if (mGenProgress[i] < progress)
                    {
                        mGenProgress.RemoveAt(i);
                        GenBuff();
                    }
                }
            }

            private void GenBuff()
            {
                var buffID = FormulaUtil.RandomInProbDict(mTable.buffTypePriority);
                var _speed = ConstTable.table.buffSpeedRange.random;
                var pos = new Vector2(Random.Range(60, UIUtil.width - 60), UIUtil.height);
                var dir = Quaternion.AngleAxis(ConstTable.table.buffSpawnDirection.random, Vector3.forward) * Vector2.down;
                Buff.Create().Reset(buffID, pos, dir, _speed);
            }
        }
    }
}