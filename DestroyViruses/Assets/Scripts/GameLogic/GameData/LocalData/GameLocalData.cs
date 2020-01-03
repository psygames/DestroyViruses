using System;
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
            signDays = 1;
            weaponId = 0;
            lastSignDateTicks = 0;
            coinValueLevel = 1;
            coinIncomeLevel = 1;
            lastTakeIncomeTicks = 0;
        }

        public int firePowerLevel;  //火力
        public int fireSpeedLevel;  //射速
        public int gameLevel;       //关卡
        public int unlockedGameLevel; //已解锁关卡
        public float coin;   
        public float diamond;
        public int streak;          //连胜/败
        public int signDays;        //签到天数
        public int weaponId;        //选择的副武器，0表示没有
        public long lastSignDateTicks; //上次签到时间
        public int coinValueLevel;
        public int coinIncomeLevel;
        public long lastTakeIncomeTicks;
    }
}
