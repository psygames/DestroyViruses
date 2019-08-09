using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestroyViruses
{
    public static class GameUtil
    {
        public static bool isFrozen
        {
            get
            {
                return GameModeManager.Instance.currentMode == null
                    || !GameModeManager.Instance.currentMode.isInit
                    || !GameModeManager.Instance.currentMode.isBegin
                    || GameModeManager.Instance.currentMode.isPause;
            }
        }
    }
}
