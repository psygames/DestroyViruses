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
        private float mSpawnCountFix;
        private TableGameLevel mTableGameLevel;

        public float getCoin { get; private set; }
        public float progress { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();

            mTableGameLevel = TableGameLevel.Get(D.I.gameLevel);
            mSpawnCountFix = FormulaUtil.Expresso(ConstTable.table.formulaArgsVirusSpawnCount);

            mWaveModule.Init();
            mBuffGenModule.Init();
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
            Analytics.Event.GameBegin(D.I.gameLevel);
            getCoin = 0;
            progress = 0;
            mLastWaveIndex = -1;
            D.I.reviveCount = 1;
            D.I.CostEnergy(ConstTable.table.energyBattleCost);
            Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.GameBegin));
            mWaveModule.Start();
        }

        protected override void OnEnd(bool isWin)
        {
            base.OnEnd(isWin);
            Analytics.Event.GameEnd(D.I.gameLevel, isWin, progress);
            mWaveModule.Stop();
            D.I.BattleEnd(isWin);
            if (isWin)
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.GameEndWin));
            else
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.GameEndLose));
        }


        protected override void OnUpdate(float deltaTime)
        {
            deltaTime *= GlobalData.slowDownFactor;
            base.OnUpdate(deltaTime);
            if (isBegin)
            {
                mBuffGenModule.Update(progress);
                mWaveModule.Update(deltaTime);
            }
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
            if (!isRunning)
                return;

            if (mLastWaveIndex != mWaveModule.waveIndex && mWaveModule.isBossWave)
            {
                Unibus.Dispatch(EventGameProcedure.Get(EventGameProcedure.Action.BossWave));
            }
            mLastWaveIndex = mWaveModule.waveIndex;

            if (mWaveModule.isFinalWave
                && mWaveModule.isStart
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
            for (int i = 0; i < mTableGameLevel.waveID.Length; i++)
            {
                var configWave = TableGameWave.Get(mTableGameLevel.waveID[i]);
                var waveCount = (int)(configWave.spawnCount * mSpawnCountFix * mTableGameLevel.spawnCountFactor);
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

        public void DoRevive()
        {
            Aircraft.ins.Revive();
            GameModeManager.Instance.Resume();
        }

        public void GiveUpRevive()
        {
            GameModeManager.Instance.Resume();
            GameModeManager.Instance.End(false);
        }

        private void OnEventAircraft(EventAircraft evt)
        {
            if (!isInit || !isBegin || !isRunning)
                return;

            if (evt.action == EventAircraft.Action.Crash)
            {
                if (D.I.reviveCount > 0)
                {
                    D.I.reviveCount -= 1;
                    UIManager.Open<GameReviveView>(UILayer.Top);
                    GameModeManager.Instance.Pause();
                }
                else
                {
                    GameModeManager.Instance.End(false);
                }
            }
        }

        private float mAddCoinCount = 0;
        private void OnEventVirus(EventVirus evt)
        {
            if (evt.action == EventVirus.Action.DEAD)
            {
                // add coin
                getCoin += FormulaUtil.CoinConvert(evt.virus.size, mTableGameLevel.coinValueFactor, D.I.coinValue);
                mAddCoinCount += evt.virus.size * 0.1f;
                if (Random.value > ConstTable.table.coinAddProb[evt.virus.size - 1])
                {
                    var pos = UIUtil.GetUIPos(evt.virus.rectTransform);
                    int coinCount = Mathf.Clamp(Mathf.CeilToInt(mAddCoinCount), 1, 15);
                    Unibus.Dispatch(EventBattle.Get(EventBattle.Action.GET_COIN, coinCount, pos));
                    mAddCoinCount = 0;
                }

                // virus kills 4 buff
                D.I.kills4Buff += 1;

                // book     
                D.I.BookAddCollectCount(evt.virus.id);
            }
            else if (evt.action == EventVirus.Action.BE_HIT)
            {
                if (BuffProxy.Ins.Has_Effect_Coin)
                {
                    var buff = BuffProxy.Ins.GetBuff("coin");
                    if (buff != null && Random.value <= buff.param2)
                    {
                        getCoin += FormulaUtil.CoinConvert(evt.virus.size, mTableGameLevel.coinValueFactor * buff.param1, D.I.coinValue);
                        var pos = UIUtil.GetUIPos(evt.virus.rectTransform);
                        Unibus.Dispatch(EventBattle.Get(EventBattle.Action.GET_COIN, 1, pos));
                    }
                }
            }
        }

    }
}