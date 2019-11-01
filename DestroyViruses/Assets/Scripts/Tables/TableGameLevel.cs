using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableGameLevelCollection
    {
        private Dictionary<string, TableGameLevel> mDict = null;

        [NonSerialized]
        private static TableGameLevelCollection _ins = null;
        public static TableGameLevelCollection Instance
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

		public TableGameLevel Get(string id)
        {
            TableGameLevel data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableGameLevel Get(Func<TableGameLevel, bool> predicate)
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

        public ICollection<TableGameLevel> GetAll()
        {
            return mDict.Values;
        }

        private static void Load()
        {
            var bytes = Resources.Load<TextAsset>("Tables/TableGameLevel").bytes;
            if(false)
			{
				bytes = AesDecrypt(bytes);
			}
            var stream = new System.IO.MemoryStream(bytes);
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            _ins = (TableGameLevelCollection)formatter.Deserialize(stream);
            stream.Close();
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey(""), AesKey("TableGameLevel"));
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
    public class TableGameLevel
    {
		/// <summary>
		/// ID
		/// </summary>
		private string _id;
		public string id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 关卡
		/// </summary>
		private int _level;
		public int level { get { return _level; } private set { _level = value; } }

		/// <summary>
		/// 病毒进攻波，对应表ConfigGameVirusWave
		/// </summary>
		private string[] _waveID;
		public string[] waveID { get { return _waveID; } private set { _waveID = value; } }

		/// <summary>
		/// 病毒产生间隔时间调节因素
		/// </summary>
		private float _spawnIntervalFactor;
		public float spawnIntervalFactor { get { return _spawnIntervalFactor; } private set { _spawnIntervalFactor = value; } }

		/// <summary>
		/// 病毒产生数量调节因素
		/// </summary>
		private float _spawnCountFactor;
		public float spawnCountFactor { get { return _spawnCountFactor; } private set { _spawnCountFactor = value; } }

		/// <summary>
		/// 病毒速度调节因素
		/// </summary>
		private float _virusSpeedFactor;
		public float virusSpeedFactor { get { return _virusSpeedFactor; } private set { _virusSpeedFactor = value; } }

		/// <summary>
		/// 病毒血量调节因素
		/// </summary>
		private float _virusHpFactor;
		public float virusHpFactor { get { return _virusHpFactor; } private set { _virusHpFactor = value; } }

		/// <summary>
		/// BOSS关卡
		/// </summary>
		private bool _isBoss;
		public bool isBoss { get { return _isBoss; } private set { _isBoss = value; } }


		public static TableGameLevel Get(string id)
		{
			return TableGameLevelCollection.Instance.Get(id);
		}
		
		public static TableGameLevel Get(Func<TableGameLevel, bool> predicate)
        {
			return TableGameLevelCollection.Instance.Get(predicate);
		}

        public static ICollection<TableGameLevel> GetAll()
        {
            return TableGameLevelCollection.Instance.GetAll();
        }
    }
}