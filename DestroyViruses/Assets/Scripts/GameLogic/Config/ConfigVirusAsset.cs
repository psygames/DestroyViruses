using UnityEngine;
using System;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class ConfigVirusAsset : ScriptableObject
    {
		[SerializeField]
        public ConfigVirus[] dataArray;

        [NonSerialized]
        private Dictionary<string, ConfigVirus> mDict = null;

        private static ConfigVirusAsset _ins = null;
        public static ConfigVirusAsset Instance
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
            mDict = new Dictionary<string, ConfigVirus>();
            foreach (var data in dataArray)
            {
                mDict.Add(data.id, data);
            }
        }

		public ConfigVirus Get(string id)
        {
            ConfigVirus data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public ConfigVirus Get(Func<ConfigVirus, bool> predicate)
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
            _ins = Resources.Load<ConfigVirusAsset>("Config/ConfigVirus");
            _ins.InitDict();
        }
    }

    [System.Serializable]
    public class ConfigVirus
    {
		/// <summary>
		/// ID
		/// </summary>
		public string id;

		/// <summary>
		/// 等级
		/// </summary>
		public int level;

		/// <summary>
		/// virus type
		/// </summary>
		public string type;

		/// <summary>
		/// 血量
		/// </summary>
		public float hp;

		/// <summary>
		/// 速度
		/// </summary>
		public float speed;


		public static ConfigVirus Get(string id)
		{
			return ConfigVirusAsset.Instance.Get(id);
		}
		
		public static ConfigVirus Get(Func<ConfigVirus, bool> predicate)
        {
			return ConfigVirusAsset.Instance.Get(predicate);
		}
    }
}