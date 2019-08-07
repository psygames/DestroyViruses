using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestroyViruses
{
    public class EventVirus
    {
        public enum ActionType
        {
            HIT,
        }

        public ActionType action;
        public float value = 0;

        public EventVirus(ActionType action, float value)
        {
            this.action = action;
            this.value = value;
        }
    }
}
