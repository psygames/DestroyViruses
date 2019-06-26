using UnityEngine;
using UnityEditor;

namespace DestroyViruses
{
    public static class Event
    {
        public class Input
        {
            public const string KEY = "Event.Input";
            public enum Type
            {
                DOWN,
                UP,
                MOVE,
            }

            private Input() { }
            public Type type { get; private set; }
            public Vector2 value { get; private set; }
            public static Input Get(Type type, Vector2 value)
            {
                return new Input() { type = type, value = value };
            }
        }
    }
}