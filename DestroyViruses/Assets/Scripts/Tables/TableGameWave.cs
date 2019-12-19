using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableGameWaveCollection
    {
        private Dictionary<int, TableGameWave> mDict = null;

        [NonSerialized]
        private static TableGameWaveCollection _ins = null;
        public static TableGameWaveCollection Instance
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

		public TableGameWave Get(int id)
        {
            TableGameWave data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableGameWave Get(Func<TableGameWave, bool> predicate)
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

        public ICollection<TableGameWave> GetAll()
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
            _ins = (TableGameWaveCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableGameWave")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableGameWave"));
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
    public class TableGameWave
    {
		/// <summary>
		/// ID
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 波数标识
		/// </summary>
		private int _wave;
		public int wave { get { return _wave; } private set { _wave = value; } }

		/// <summary>
		/// 产生病毒种类
		/// </summary>
		private int[] _virus;
		public int[] virus { get { return _virus; } private set { _virus = value; } }

		/// <summary>
		/// <para>产生病毒种类概率,</para><para>对应virus列索引</para>
		/// </summary>
		private float[] _virusProb;
		public float[] virusProb { get { return _virusProb; } private set { _virusProb = value; } }

		/// <summary>
		/// <para>产生病毒种尺寸（范围1-4）,</para><para>对应virus列索引</para>
		/// </summary>
		private TRangeInt[] _virusSize;
		public TRangeInt[] virusSize { get { return _virusSize; } private set { _virusSize = value; } }

		/// <summary>
		/// 产生病毒的血量
		/// </summary>
		private TRange[] _virusHp;
		public TRange[] virusHp { get { return _virusHp; } private set { _virusHp = value; } }

		/// <summary>
		/// 病毒移动速度
		/// </summary>
		private TRange[] _virusSpeed;
		public TRange[] virusSpeed { get { return _virusSpeed; } private set { _virusSpeed = value; } }

		/// <summary>
		/// 病毒产生时间间隔
		/// </summary>
		private float _spawnInterval;
		public float spawnInterval { get { return _spawnInterval; } private set { _spawnInterval = value; } }

		/// <summary>
		/// 病毒总数量
		/// </summary>
		private int _spawnCount;
		public int spawnCount { get { return _spawnCount; } private set { _spawnCount = value; } }

		/// <summary>
		/// 是否需要等待清除病毒后，才进入下一关
		/// </summary>
		private bool _needClear;
		public bool needClear { get { return _needClear; } private set { _needClear = value; } }

		/// <summary>
		/// 是否弹出“病毒入侵”警告
		/// </summary>
		private bool _bossWave;
		public bool bossWave { get { return _bossWave; } private set { _bossWave = value; } }


		public static TableGameWave Get(int id)
		{
			return TableGameWaveCollection.Instance.Get(id);
		}
		
		public static TableGameWave Get(Func<TableGameWave, bool> predicate)
        {
			return TableGameWaveCollection.Instance.Get(predicate);
		}

        public static ICollection<TableGameWave> GetAll()
        {
            return TableGameWaveCollection.Instance.GetAll();
        }
    }
}