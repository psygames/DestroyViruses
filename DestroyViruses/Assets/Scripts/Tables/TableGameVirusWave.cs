using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableGameVirusWaveCollection
    {
        private Dictionary<string, TableGameVirusWave> mDict = null;

        [NonSerialized]
        private static TableGameVirusWaveCollection _ins = null;
        public static TableGameVirusWaveCollection Instance
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

		public TableGameVirusWave Get(string id)
        {
            TableGameVirusWave data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableGameVirusWave Get(Func<TableGameVirusWave, bool> predicate)
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

        public ICollection<TableGameVirusWave> GetAll()
        {
            return mDict.Values;
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableGameVirusWave")).bytes;
            if(true)
			{
				bytes = AesDecrypt(bytes);
			}
            var stream = new System.IO.MemoryStream(bytes);
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            _ins = (TableGameVirusWaveCollection)formatter.Deserialize(stream);
            stream.Close();
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY1"), AesKey("TableGameVirusWave"));
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
    public class TableGameVirusWave
    {
		/// <summary>
		/// ID
		/// </summary>
		private string _id;
		public string id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 波数标识
		/// </summary>
		private int _wave;
		public int wave { get { return _wave; } private set { _wave = value; } }

		/// <summary>
		/// 产生病毒种类
		/// </summary>
		private string[] _virus;
		public string[] virus { get { return _virus; } private set { _virus = value; } }

		/// <summary>
		/// <para>产生病毒种类概率,</para><para>对应virus列索引</para>
		/// </summary>
		private float[] _virusProb;
		public float[] virusProb { get { return _virusProb; } private set { _virusProb = value; } }

		/// <summary>
		/// <para>产生病毒种尺寸（范围1-4）,</para><para>对应virus列索引</para>
		/// </summary>
		private int[] _virusSize;
		public int[] virusSize { get { return _virusSize; } private set { _virusSize = value; } }

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
		/// 病毒血量范围
		/// </summary>
		private TVector2 _virusHpRange;
		public TVector2 virusHpRange { get { return _virusHpRange; } private set { _virusHpRange = value; } }

		/// <summary>
		/// 病毒速度范围
		/// </summary>
		private TVector2 _virusSpeedRange;
		public TVector2 virusSpeedRange { get { return _virusSpeedRange; } private set { _virusSpeedRange = value; } }


		public static TableGameVirusWave Get(string id)
		{
			return TableGameVirusWaveCollection.Instance.Get(id);
		}
		
		public static TableGameVirusWave Get(Func<TableGameVirusWave, bool> predicate)
        {
			return TableGameVirusWaveCollection.Instance.Get(predicate);
		}

        public static ICollection<TableGameVirusWave> GetAll()
        {
            return TableGameVirusWaveCollection.Instance.GetAll();
        }
    }
}