using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableVirusCollection
    {
        private Dictionary<int, TableVirus> mDict = null;

        [NonSerialized]
        private static TableVirusCollection _ins = null;
        public static TableVirusCollection Instance
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

		public TableVirus Get(int id)
        {
            TableVirus data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableVirus Get(Func<TableVirus, bool> predicate)
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

        public ICollection<TableVirus> GetAll()
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
            _ins = (TableVirusCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableVirus")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableVirus"));
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
    public class TableVirus
    {
		/// <summary>
		/// ID
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// virus type（程序使用）
		/// </summary>
		private string _type;
		public string type { get { return _type; } private set { _type = value; } }

		/// <summary>
		/// 多语言
		/// </summary>
		private string _nameID;
		public string nameID { get { return _nameID; } private set { _nameID = value; } }

		/// <summary>
		/// 多语言
		/// </summary>
		private string _descriptionID;
		public string descriptionID { get { return _descriptionID; } private set { _descriptionID = value; } }

		/// <summary>
		/// 多语言
		/// </summary>
		private string _tipsID;
		public string tipsID { get { return _tipsID; } private set { _tipsID = value; } }

		/// <summary>
		/// skill触发间隔
		/// </summary>
		private float _skillCD;
		public float skillCD { get { return _skillCD; } private set { _skillCD = value; } }

		/// <summary>
		/// 技能效果是数值1
		/// </summary>
		private float _effect1;
		public float effect1 { get { return _effect1; } private set { _effect1 = value; } }

		/// <summary>
		/// 技能效果数值2
		/// </summary>
		private float _effect2;
		public float effect2 { get { return _effect2; } private set { _effect2 = value; } }


		public static TableVirus Get(int id)
		{
			return TableVirusCollection.Instance.Get(id);
		}
		
		public static TableVirus Get(Func<TableVirus, bool> predicate)
        {
			return TableVirusCollection.Instance.Get(predicate);
		}

        public static ICollection<TableVirus> GetAll()
        {
            return TableVirusCollection.Instance.GetAll();
        }
    }
}