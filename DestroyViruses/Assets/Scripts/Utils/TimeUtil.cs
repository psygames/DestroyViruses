using System;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public static class TimeUtil
    {
        private static Dictionary<string, float> sCheckIntervalDict = new Dictionary<string, float>();
        public static bool CheckInterval(string key, float interval)
        {
            if (sCheckIntervalDict.TryGetValue(key, out float lastTime) && lastTime + interval > Time.time)
            {
                return false;
            }
            sCheckIntervalDict[key] = Time.time;
            return true;
        }
    }
}
