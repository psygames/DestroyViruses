using UnityEngine;

namespace DestroyViruses
{
    public class ConfigAircraftCollection : ScriptableObject
    {
        public ConfigAircraft[] dataArray;
    }

    [System.Serializable]
    public class ConfigAircraft
    {
		/// <summary>
		/// ID
		/// </summary>
		public int id { get; private set; }
		/// <summary>
		/// 等级
		/// </summary>
		public int level { get; private set; }
		/// <summary>
		/// 血量
		/// </summary>
		public int hp { get; private set; }

    }
}