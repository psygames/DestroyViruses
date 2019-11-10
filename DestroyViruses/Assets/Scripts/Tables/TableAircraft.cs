using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableAircraftCollection
    {
        private Dictionary<int, TableAircraft> mDict = null;

        [NonSerialized]
        private static TableAircraftCollection _ins = null;
        public static TableAircraftCollection Instance
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

		public TableAircraft Get(int id)
        {
            TableAircraft data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableAircraft Get(Func<TableAircraft, bool> predicate)
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

        public ICollection<TableAircraft> GetAll()
        {
            return mDict.Values;
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableAircraft")).bytes;
            if(true)
			{
				bytes = AesDecrypt(bytes);
			}
            var stream = new System.IO.MemoryStream(bytes);
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            _ins = (TableAircraftCollection)formatter.Deserialize(stream);
            stream.Close();
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableAircraft"));
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
    public class TableAircraft
    {
		/// <summary>
		/// ID
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 程序用，副武器类型
		/// </summary>
		private string _tag;
		public string tag { get { return _tag; } private set { _tag = value; } }

		/// <summary>
		/// 名称
		/// </summary>
		private string _name;
		public string name { get { return _name; } private set { _name = value; } }

		/// <summary>
		/// 解锁等级
		/// </summary>
		private int _unlockGameLevel;
		public int unlockGameLevel { get { return _unlockGameLevel; } private set { _unlockGameLevel = value; } }

		/// <summary>
		/// 火力
		/// </summary>
		private float _power;
		public float power { get { return _power; } private set { _power = value; } }

		/// <summary>
		/// 强度（暂时不清楚什么逻辑，好像是减少冷却时间？）
		/// </summary>
		private float _strength;
		public float strength { get { return _strength; } private set { _strength = value; } }

		/// <summary>
		/// 冷却时间
		/// </summary>
		private float _cd;
		public float cd { get { return _cd; } private set { _cd = value; } }

		/// <summary>
		/// 图标
		/// </summary>
		private string _icon;
		public string icon { get { return _icon; } private set { _icon = value; } }


		public static TableAircraft Get(int id)
		{
			return TableAircraftCollection.Instance.Get(id);
		}
		
		public static TableAircraft Get(Func<TableAircraft, bool> predicate)
        {
			return TableAircraftCollection.Instance.Get(predicate);
		}

        public static ICollection<TableAircraft> GetAll()
        {
            return TableAircraftCollection.Instance.GetAll();
        }
    }
}