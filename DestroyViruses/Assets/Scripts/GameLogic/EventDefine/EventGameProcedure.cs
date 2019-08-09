using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestroyViruses
{
    public class EventGameProcedure
    {
        public enum ActionType
        {
            GameBegin,
            FinalWave,
            GameEndWin,
            GameEndLose,
        }

        public ActionType action { get; private set; }

        private static EventGameProcedure sIns;
        public static EventGameProcedure Get(ActionType action)
        {
            if (sIns == null)
                sIns = new EventGameProcedure();
            sIns.action = action;
            return sIns;
        }
    }
}
