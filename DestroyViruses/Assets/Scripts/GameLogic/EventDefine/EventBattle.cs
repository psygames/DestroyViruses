using System;
using UnityEngine;

namespace DestroyViruses
{
    public class EventBattle
    {
        public enum Action
        {
            GET_COIN,
        }

        public Action action { get; private set; }
        public int count { get; private set; }
        public Vector2 position { get; private set; }

        private static EventBattle sIns;
        public static EventBattle Get(Action action, int count, Vector2 position)
        {
            if (sIns == null)
                sIns = new EventBattle();
            sIns.action = action;
            sIns.count = count;
            sIns.position = position;
            return sIns;
        }
    }
}
