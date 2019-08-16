using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestroyViruses
{
    public class EventBullet
    {
        public enum Action
        {
            HIT,
        }

        public Action action { get; private set; }
        public VirusBase target { get; private set; }
        public float damage { get; private set; }

        private static EventBullet sIns;
        public static EventBullet Get(Action action, VirusBase target, float damage)
        {
            if (sIns == null)
                sIns = new EventBullet();
            sIns.action = action;
            sIns.target = target;
            sIns.damage = damage;
            return sIns;
        }
    }
}
