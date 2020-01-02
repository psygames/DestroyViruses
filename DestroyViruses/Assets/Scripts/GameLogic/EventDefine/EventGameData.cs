using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestroyViruses
{
    public class EventGameData
    {
        public enum Action
        {
            ChangeWeapon,
            UnlockNewLevel,
            DataChange,
            Error,
        }

        public Action action { get; private set; }
        public string errorMsg { get; private set; }
        private static EventGameData sIns;
        public static EventGameData Get(Action action, string errorMsg = "")
        {
            if (sIns == null)
                sIns = new EventGameData();
            sIns.action = action;
            sIns.errorMsg = errorMsg;
            return sIns;
        }
    }
}
