using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableBuffKillGenCollection
    {
        private Dictionary<int, TableBuffKillGen> mDict = null;

        [NonSerialized]
        private static TableBuffKillGenCollection _ins = null;
        public static TableBuffKillGenCollection Instance
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

		public TableBuffKillGen Get(int id)
        {
            TableBuffKillGen data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableBuffKillGen Get(Func<TableBuffKillGen, bool> predicate)
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

        public ICollection<TableBuffKillGen> GetAll()
        {
            return mDict.Values;
        }

        public static void Load(byte[] bytes)
        {
            if(true)
			{
				bytes = AesDecrypt(bytes);
			}
            var stream = new System.IO.MemoryStream(bytes);
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            _ins = (TableBuffKillGenCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableBuffKillGen")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableBuffKillGen"));
				using (CryptoStream Decryptor = new CryptoStream(Memory, transform, CryptoStreamMode.Read))
				{
					using (var originalMemory = new System.IO.MemoryStream())
					{
						byte[] Buffer = new byte[1024];
						int readBytes = 0;
						while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
						{
							originalMemory.Write(Buffer, 0, readBytes);
						}
						original = originalMemory.ToArray();
					}
				}
                transform.Dispose();
			}
			return original;
		}

        private static byte[] AesKey(string key)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(key);
			byte[] keyBytes = new byte[16];
			for (int i = 0; i < bytes.Length; i++)
			{
				keyBytes[i % 16] = (byte)(keyBytes[i % 16] ^ bytes[i]);
			}
			return keyBytes;
        }
    }

    [Serializable]
    public class TableBuffKillGen
    {
		/// <summary>
		/// ID
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 关卡范围
		/// </summary>
		private TRangeInt _gameLevel;
		public TRangeInt gameLevel { get { return _gameLevel; } private set { _gameLevel = value; } }

		/// <summary>
		/// <para>连胜/败次数</para><para>（负数表示连败）</para>
		/// </summary>
		private int _streak;
		public int streak { get { return _streak; } private set { _streak = value; } }

		/// <summary>
        /// buff类型权重
        /// </summary>
        [SerializeField]
        private int[] _buffTypePriority_dc_key;
        [SerializeField]
        private float[] _buffTypePriority_dc_value;
        private Dictionary<int, float> _buffTypePriority;
        public Dictionary<int, float> buffTypePriority
        {
            get
            {
                if (_buffTypePriority == null)
                {
                    _buffTypePriority = new Dictionary<int, float>();
                    for (int i = 0; i < _buffTypePriority_dc_key.Length; i++)
                    {
                        _buffTypePriority.Add(_buffTypePriority_dc_key[i], _buffTypePriority_dc_value[i]);
                    }
                }
                return _buffTypePriority;
            }
            private set
            {
                _buffTypePriority = value;
                _buffTypePriority_dc_key = new int[value.Keys.Count];
                _buffTypePriority_dc_value = new float[value.Values.Count];
				int i = 0;
                foreach (var kv in value)
                {
                    _buffTypePriority_dc_key[i] = kv.Key;
                    _buffTypePriority_dc_value[i] = kv.Value;
                    i++;
                }
            }
        }

		/// <summary>
		/// <para>击杀生成buff概率</para><para>（连续未生成，则累加概率）</para>
		/// </summary>
		private float _probability;
		public float probability { get { return _probability; } private set { _probability = value; } }


		public static TableBuffKillGen Get(int id)
		{
			return TableBuffKillGenCollection.Instance.Get(id);
		}
		
		public static TableBuffKillGen Get(Func<TableBuffKillGen, bool> predicate)
        {
			return TableBuffKillGenCollection.Instance.Get(predicate);
		}

        public static ICollection<TableBuffKillGen> GetAll()
        {
            return TableBuffKillGenCollection.Instance.GetAll();
        }
    }
}