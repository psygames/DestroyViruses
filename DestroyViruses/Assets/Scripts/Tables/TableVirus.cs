using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableVirusCollection
    {
        private Dictionary<string, TableVirus> mDict = null;

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

		public TableVirus Get(string id)
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

        private static void Load()
        {
            var bytes = Resources.Load<TextAsset>("Tables/TableVirus").bytes;
            if(false)
			{
				bytes = AesDecrypt(bytes);
			}
            var stream = new System.IO.MemoryStream(bytes);
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            _ins = (TableVirusCollection)formatter.Deserialize(stream);
            stream.Close();
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey(""), AesKey("TableVirus"));
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
		private string _id;
		public string id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// virus type
		/// </summary>
		private string _type;
		public string type { get { return _type; } private set { _type = value; } }


		public static TableVirus Get(string id)
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