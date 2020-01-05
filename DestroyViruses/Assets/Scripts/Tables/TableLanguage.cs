using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableLanguageCollection
    {
        private Dictionary<string, TableLanguage> mDict = null;

        [NonSerialized]
        private static TableLanguageCollection _ins = null;
        public static TableLanguageCollection Instance
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

		public TableLanguage Get(string id)
        {
            TableLanguage data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableLanguage Get(Func<TableLanguage, bool> predicate)
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

        public ICollection<TableLanguage> GetAll()
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
            _ins = (TableLanguageCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableLanguage")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableLanguage"));
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
    public class TableLanguage
    {
		/// <summary>
		/// 描述
		/// </summary>
		private string _id;
		public string id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 中文（中华人民共和国）
		/// </summary>
		private string _cn;
		public string cn { get { return _cn; } private set { _cn = value; } }

		/// <summary>
		/// 英语
		/// </summary>
		private string _en;
		public string en { get { return _en; } private set { _en = value; } }

		/// <summary>
		/// 法语（标准）
		/// </summary>
		private string _fr;
		public string fr { get { return _fr; } private set { _fr = value; } }

		/// <summary>
		/// 西班牙语
		/// </summary>
		private string _sp;
		public string sp { get { return _sp; } private set { _sp = value; } }

		/// <summary>
		/// 德语（标准）
		/// </summary>
		private string _de;
		public string de { get { return _de; } private set { _de = value; } }

		/// <summary>
		/// 俄语
		/// </summary>
		private string _ru;
		public string ru { get { return _ru; } private set { _ru = value; } }

		/// <summary>
		/// 日语
		/// </summary>
		private string _ja;
		public string ja { get { return _ja; } private set { _ja = value; } }

		/// <summary>
		/// 韩语
		/// </summary>
		private string _ko;
		public string ko { get { return _ko; } private set { _ko = value; } }


		public static TableLanguage Get(string id)
		{
			return TableLanguageCollection.Instance.Get(id);
		}
		
		public static TableLanguage Get(Func<TableLanguage, bool> predicate)
        {
			return TableLanguageCollection.Instance.Get(predicate);
		}

        public static ICollection<TableLanguage> GetAll()
        {
            return TableLanguageCollection.Instance.GetAll();
        }
    }
}