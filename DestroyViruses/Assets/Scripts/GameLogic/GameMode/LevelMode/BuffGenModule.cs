using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class BuffGenModule
    {
        private List<float> mGenProgress = new List<float>();
        private TableBuffAutoGen mTable;
        private int mGameLevel;
        private int mStreak;

        private TRangeInt buffCountRange
        {
            get
            {
                if (D.I.IsVip())
                    return mTable.vipBuffCount;
                return mTable.buffCount;
            }
        }

        private Dictionary<int,float> buffTypePriority
        {
            get
            {
                if (D.I.IsVip())
                    return mTable.vipBuffTypePriority;
                return mTable.buffTypePriority;
            }
        }

        public void Init()
        {
            mGameLevel = D.I.gameLevel;
            mStreak = D.I.streak;
            mGenProgress.Clear();
            mTable = TableBuffAutoGen.Get(a => a.gameLevel.Contains(mGameLevel) && a.streak == mStreak);
            var _count = buffCountRange.random;
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
            var buffID = FormulaUtil.RandomInProbDict(buffTypePriority);
            var _speed = ConstTable.table.buffSpeedRange.random;
            var pos = new Vector2(Random.Range(60, UIUtil.width - 60), UIUtil.height);
            var dir = Quaternion.AngleAxis(ConstTable.table.buffSpawnDirection.random, Vector3.forward) * Vector2.down;
            Buff.Create().Reset(buffID, pos, dir, _speed);
        }
    }
}