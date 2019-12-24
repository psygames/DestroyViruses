using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
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


        public void Init()
        {
            mFirePower = D.I.firePower;
            tableGameLevel = TableGameLevel.Get(D.I.gameLevel);
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
                mHpFixFactor = FormulaUtil.Expresso(ConstTable.table.formulaArgsVirusHp);
                mSpawnCountFixFactor = FormulaUtil.Expresso(ConstTable.table.formulaArgsVirusSpawnCount);
                mSpeedFixFactor = 1;
            }
        }
    }


}
