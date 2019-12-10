using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableBuffCollection
    {
        private Dictionary<int, TableBuff> mDict = null;

        [NonSerialized]
        private static TableBuffCollection _ins = null;
        public static TableBuffCollection Instance
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

		public TableBuff Get(int id)
        {
            TableBuff data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableBuff Get(Func<TableBuff, bool> predicate)
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

        public ICollection<TableBuff> GetAll()
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
            _ins = (TableBuffCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableBuff")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableBuff"));
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
    public class TableBuff
    {
		/// <summary>
		/// ID
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 多语言KEY
		/// </summary>
		private string _nameID;
		public string nameID { get { return _nameID; } private set { _nameID = value; } }

		/// <summary>
		/// <para>1-增益buff</para><para>2-减益buff</para>
		/// </summary>
		private int _type;
		public int type { get { return _type; } private set { _type = value; } }

		/// <summary>
		/// 图标
		/// </summary>
		private string _icon;
		public string icon { get { return _icon; } private set { _icon = value; } }

		/// <summary>
		/// 出现时间
		/// </summary>
		private float _appear;
		public float appear { get { return _appear; } private set { _appear = value; } }

		/// <summary>
		/// 效果
		/// </summary>
		private string _effect;
		public string effect { get { return _effect; } private set { _effect = value; } }

		/// <summary>
		/// 效果时间
		/// </summary>
		private float _effectDuration;
		public float effectDuration { get { return _effectDuration; } private set { _effectDuration = value; } }

		/// <summary>
		/// 参数1
		/// </summary>
		private float _param1;
		public float param1 { get { return _param1; } private set { _param1 = value; } }

		/// <summary>
		/// 
		/// </summary>
		private TVector2 _param2;
		public TVector2 param2 { get { return _param2; } private set { _param2 = value; } }


		public static TableBuff Get(int id)
		{
			return TableBuffCollection.Instance.Get(id);
		}
		
		public static TableBuff Get(Func<TableBuff, bool> predicate)
        {
			return TableBuffCollection.Instance.Get(predicate);
		}

        public static ICollection<TableBuff> GetAll()
        {
            return TableBuffCollection.Instance.GetAll();
        }
    }
}