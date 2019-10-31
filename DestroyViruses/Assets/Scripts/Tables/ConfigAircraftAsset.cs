using UnityEngine;
using System;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class ConfigAircraftAsset : ScriptableObject
    {
		[SerializeField]
        public ConfigAircraft[] dataArray;

        [NonSerialized]
        private Dictionary<string, ConfigAircraft> mDict = null;

        private static ConfigAircraftAsset _ins = null;
        public static ConfigAircraftAsset Instance
        {
            get
            {
                if (_ins == null)
                {
                    Load();
                }
                return _ins;
            }
        }

        private void InitDict()
        {
            mDict = new Dictionary<string, ConfigAircraft>();
            foreach (var data in dataArray)
            {
                mDict.Add(data.id, data);
            }
        }

		public ConfigAircraft Get(string id)
        {
            ConfigAircraft data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public ConfigAircraft Get(Func<ConfigAircraft, bool> predicate)
        {
            foreach (var item in _ins.mDict)
            {
                if (predicate(item.Value))
                {
                    return item.Value;
                }
            }
            return null;
        }

        private static void Load()
        {
            _ins = Resources.Load<ConfigAircraftAsset>("Config/ConfigAircraft");
            _ins.InitDict();
        }
    }

    [System.Serializable]
    public class ConfigAircraft
    {
		/// <summary>
		/// ID
		/// </summary>
		public string id;

		/// <summary>
		/// 等级
		/// </summary>
		public int level;


		public static ConfigAircraft Get(string id)
		{
			return ConfigAircraftAsset.Instance.Get(id);
		}
		
		public static ConfigAircraft Get(Func<ConfigAircraft, bool> predicate)
        {
			return ConfigAircraftAsset.Instance.Get(predicate);
		}
    }
}