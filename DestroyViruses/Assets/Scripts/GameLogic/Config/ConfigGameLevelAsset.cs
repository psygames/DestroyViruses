using UnityEngine;
using System;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class ConfigGameLevelAsset : ScriptableObject
    {
		[SerializeField]
        public ConfigGameLevel[] dataArray;

        [NonSerialized]
        private Dictionary<string, ConfigGameLevel> mDict = null;

        private static ConfigGameLevelAsset _ins = null;
        public static ConfigGameLevelAsset Instance
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
            mDict = new Dictionary<string, ConfigGameLevel>();
            foreach (var data in dataArray)
            {
                mDict.Add(data.id, data);
            }
        }

		public ConfigGameLevel Get(string id)
        {
            ConfigGameLevel data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public ConfigGameLevel Get(Func<ConfigGameLevel, bool> predicate)
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
            _ins = Resources.Load<ConfigGameLevelAsset>("Config/ConfigGameLevel");
            _ins.InitDict();
        }
    }

    [System.Serializable]
    public class ConfigGameLevel
    {
		/// <summary>
		/// ID
		/// </summary>
		public string id;

		/// <summary>
		/// 关卡
		/// </summary>
		public int level;

		/// <summary>
		/// virus(ConfigVirus) id array
		/// </summary>
		public string[] wave1;

		/// <summary>
		/// virus(ConfigVirus) id array
		/// </summary>
		public string[] wave2;

		/// <summary>
		/// virus(ConfigVirus) id array
		/// </summary>
		public string[] wave3;

		/// <summary>
		/// 每波产生病毒间隔
		/// </summary>
		public float[] waveSpawnInterval;

		/// <summary>
		/// 每波产生病毒最大数（当前存活）
		/// </summary>
		public int[] waveSpawnMax;


		public static ConfigGameLevel Get(string id)
		{
			return ConfigGameLevelAsset.Instance.Get(id);
		}
		
		public static ConfigGameLevel Get(Func<ConfigGameLevel, bool> predicate)
        {
			return ConfigGameLevelAsset.Instance.Get(predicate);
		}
    }
}