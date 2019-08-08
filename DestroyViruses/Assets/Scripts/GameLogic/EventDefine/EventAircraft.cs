using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestroyViruses
{
    public class EventAircraft
    {
        public enum ActionType
        {
            Crash,
            Reborn,
        }

        public ActionType action { get; private set; }
        private static EventAircraft sIns;
        public static EventAircraft Get(ActionType action)
        {
            if (sIns == null)
                sIns = new EventAircraft();
            sIns.action = action;
            return sIns;
        }
    }
}
