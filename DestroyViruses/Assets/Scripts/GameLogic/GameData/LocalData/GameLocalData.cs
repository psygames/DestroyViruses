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
            lastEnergyTicks = 0;
            lastTrialTicks = 0;
            trialCount = 0;
            trialWeaponID = 0;
            isInTrial = false;
            lastVipTicks = 0;
            lastVipRewardDays = -1;
            lastConstGroup = "default";
            minVersion = "0.1.0";
            latestVersion = "0.1.0";
        }

        public int firePowerLevel;      //火力
        public int fireSpeedLevel;      //射速
        public int gameLevel;           //关卡
        public int unlockedGameLevel;   //已解锁关卡
        public float coin;
        public float diamond;
        public int streak;              //连胜/败
        public int signDays;            //签到天数
        public int weaponId;            //选择的副武器，0表示没有
        public long lastSignDateTicks;  //上次签到时间
        public int coinValueLevel;
        public int coinIncomeLevel;
        public long lastTakeIncomeTicks;    //上次领取收益时间
        public int energy;                  //体力值
        public long lastEnergyTicks;        //上次体力值变化时间
        public long lastTrialTicks;         //上次武器试用时间
        public int trialCount;              //武器试用次数
        public int trialWeaponID;           //试用武器ID
        public bool isInTrial;              //是否试用中
        public long lastVipTicks;           //上次购买VIP时间
        public int lastVipRewardDays;       //上次VIP奖励领取时间
        public string lastConstGroup;       //上次配置组ID
        public string minVersion;           //最小版本号
        public string latestVersion;        //最新版本号
    }
}
