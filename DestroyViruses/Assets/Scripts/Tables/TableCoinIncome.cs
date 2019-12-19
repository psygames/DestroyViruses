using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableCoinIncomeCollection
    {
        private Dictionary<int, TableCoinIncome> mDict = null;

        [NonSerialized]
        private static TableCoinIncomeCollection _ins = null;
        public static TableCoinIncomeCollection Instance
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

		public TableCoinIncome Get(int id)
        {
            TableCoinIncome data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableCoinIncome Get(Func<TableCoinIncome, bool> predicate)
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

        public ICollection<TableCoinIncome> GetAll()
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
            _ins = (TableCoinIncomeCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableCoinIncome")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableCoinIncome"));
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
    public class TableCoinIncome
    {
		/// <summary>
		/// ID（等级）
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 日常收益 每秒
		/// </summary>
		private float _income;
		public float income { get { return _income; } private set { _income = value; } }

		/// <summary>
		/// 需要金币
		/// </summary>
		private float _upcost;
		public float upcost { get { return _upcost; } private set { _upcost = value; } }


		public static TableCoinIncome Get(int id)
		{
			return TableCoinIncomeCollection.Instance.Get(id);
		}
		
		public static TableCoinIncome Get(Func<TableCoinIncome, bool> predicate)
        {
			return TableCoinIncomeCollection.Instance.Get(predicate);
		}

        public static ICollection<TableCoinIncome> GetAll()
        {
            return TableCoinIncomeCollection.Instance.GetAll();
        }
    }
}