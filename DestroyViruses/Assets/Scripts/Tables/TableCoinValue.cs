using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableCoinValueCollection
    {
        private Dictionary<int, TableCoinValue> mDict = null;

        [NonSerialized]
        private static TableCoinValueCollection _ins = null;
        public static TableCoinValueCollection Instance
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

		public TableCoinValue Get(int id)
        {
            TableCoinValue data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableCoinValue Get(Func<TableCoinValue, bool> predicate)
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

        public ICollection<TableCoinValue> GetAll()
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
            _ins = (TableCoinValueCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableCoinValue")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableCoinValue"));
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
    public class TableCoinValue
    {
		/// <summary>
		/// ID（等级）
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 金币价值
		/// </summary>
		private float _value;
		public float value { get { return _value; } private set { _value = value; } }

		/// <summary>
		/// 升级消耗
		/// </summary>
		private float _upcost;
		public float upcost { get { return _upcost; } private set { _upcost = value; } }


		public static TableCoinValue Get(int id)
		{
			return TableCoinValueCollection.Instance.Get(id);
		}
		
		public static TableCoinValue Get(Func<TableCoinValue, bool> predicate)
        {
			return TableCoinValueCollection.Instance.Get(predicate);
		}

        public static ICollection<TableCoinValue> GetAll()
        {
            return TableCoinValueCollection.Instance.GetAll();
        }
    }
}