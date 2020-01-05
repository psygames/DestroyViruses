using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DestroyViruses
{
    public static class GameUtil
    {
        public static bool isInHome { get { return StateManager.Instance.currentState.GetType() == typeof(MainState); } }
        public static bool isInBattle { get { return StateManager.Instance.currentState.GetType() == typeof(BattleState); } }
        public static bool isFrozen
        {
            get
            {
                return GameModeManager.Instance.currentMode == null
                    || !GameModeManager.Instance.currentMode.isInit
                    || !GameModeManager.Instance.currentMode.isBegin
                    || !GameModeManager.Instance.currentMode.isRunning;
            }
        }

        public static float runningTime
        {
            get
            {
                if (GameModeManager.Instance.currentMode == null)
                    return 0;
                return GameModeManager.Instance.currentMode.runningTime;
            }
        }
    }
}
