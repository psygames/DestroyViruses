using UnityEngine;
using System;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class ConfigGameVirusWaveAsset : ScriptableObject
    {
		[SerializeField]
        public ConfigGameVirusWave[] dataArray;

        [NonSerialized]
        private Dictionary<string, ConfigGameVirusWave> mDict = null;

        private static ConfigGameVirusWaveAsset _ins = null;
        public static ConfigGameVirusWaveAsset Instance
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
            mDict = new Dictionary<string, ConfigGameVirusWave>();
            foreach (var data in dataArray)
            {
                mDict.Add(data.id, data);
            }
        }

		public ConfigGameVirusWave Get(string id)
        {
            ConfigGameVirusWave data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public ConfigGameVirusWave Get(Func<ConfigGameVirusWave, bool> predicate)
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
            _ins = Resources.Load<ConfigGameVirusWaveAsset>("Config/ConfigGameVirusWave");
            _ins.InitDict();
        }
    }

    [System.Serializable]
    public class ConfigGameVirusWave
    {
		/// <summary>
		/// ID
		/// </summary>
		public string id;

		/// <summary>
		/// 波数标识
		/// </summary>
		public int wave;

		/// <summary>
		/// 产生病毒种类
		/// </summary>
		public string[] virus;

		/// <summary>
		/// <para>产生病毒种类概率,</para><para>对应virus列索引</para>
		/// </summary>
		public float[] virusProb;

		/// <summary>
		/// <para>产生病毒种尺寸（范围1-4）,</para><para>对应virus列索引</para>
		/// </summary>
		public int[] virusSize;

		/// <summary>
		/// 病毒产生时间间隔
		/// </summary>
		public float spawnInterval;

		/// <summary>
		/// 病毒总数量
		/// </summary>
		public int spawnCount;

		/// <summary>
		/// 病毒血量范围
		/// </summary>
		public Vector2 virusHpRange;

		/// <summary>
		/// 病毒速度范围
		/// </summary>
		public Vector2 virusSpeedRange;


		public static ConfigGameVirusWave Get(string id)
		{
			return ConfigGameVirusWaveAsset.Instance.Get(id);
		}
		
		public static ConfigGameVirusWave Get(Func<ConfigGameVirusWave, bool> predicate)
        {
			return ConfigGameVirusWaveAsset.Instance.Get(predicate);
		}
    }
}