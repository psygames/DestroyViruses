using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableDailySignCollection
    {
        private Dictionary<int, TableDailySign> mDict = null;

        [NonSerialized]
        private static TableDailySignCollection _ins = null;
        public static TableDailySignCollection Instance
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

		public TableDailySign Get(int id)
        {
            TableDailySign data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableDailySign Get(Func<TableDailySign, bool> predicate)
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

        public ICollection<TableDailySign> GetAll()
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
            _ins = (TableDailySignCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableDailySign")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableDailySign"));
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
    public class TableDailySign
    {
		/// <summary>
		/// DAYS
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 多语言KEY
		/// </summary>
		private string _nameID;
		public string nameID { get { return _nameID; } private set { _nameID = value; } }

		/// <summary>
		/// <para>1 - 钻石</para><para>2 - 金币</para>
		/// </summary>
		private int _type;
		public int type { get { return _type; } private set { _type = value; } }

		/// <summary>
		/// 图
		/// </summary>
		private string _icon;
		public string icon { get { return _icon; } private set { _icon = value; } }

		/// <summary>
		/// 数量
		/// </summary>
		private int _count;
		public int count { get { return _count; } private set { _count = value; } }


		public static TableDailySign Get(int id)
		{
			return TableDailySignCollection.Instance.Get(id);
		}
		
		public static TableDailySign Get(Func<TableDailySign, bool> predicate)
        {
			return TableDailySignCollection.Instance.Get(predicate);
		}

        public static ICollection<TableDailySign> GetAll()
        {
            return TableDailySignCollection.Instance.GetAll();
        }
    }
}