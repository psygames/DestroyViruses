using UnityEngine;

namespace DestroyViruses
{
    public class GameLocalData : LocalData<GameLocalData>
    {
        // 默认数据
        public GameLocalData()
        {
            firePowerLevel = 1;
            fireSpeedLevel = 1;
            gameLevel = 1;
            unlockedGameLevel = 1;
            coin = 0;
            diamond = 0;
            streak = 0;
        }

        public int firePowerLevel;  //火力
        public int fireSpeedLevel;  //射速
        public int gameLevel;       //关卡
        public int unlockedGameLevel;//已解锁关卡
        public long coin;
        public long diamond;
        public int streak;          //连胜/败
    }
}
