using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableWeaponCollection
    {
        private Dictionary<int, TableWeapon> mDict = null;

        [NonSerialized]
        private static TableWeaponCollection _ins = null;
        public static TableWeaponCollection Instance
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

		public TableWeapon Get(int id)
        {
            TableWeapon data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableWeapon Get(Func<TableWeapon, bool> predicate)
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

        public ICollection<TableWeapon> GetAll()
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
            _ins = (TableWeaponCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableWeapon")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableWeapon"));
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
    public class TableWeapon
    {
		/// <summary>
		/// ID
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 武器图标
		/// </summary>
		private string _icon;
		public string icon { get { return _icon; } private set { _icon = value; } }

		/// <summary>
		/// 技能效果数值1
		/// </summary>
		private float _effect1;
		public float effect1 { get { return _effect1; } private set { _effect1 = value; } }

		/// <summary>
		/// 技能效果数值2
		/// </summary>
		private float _effect2;
		public float effect2 { get { return _effect2; } private set { _effect2 = value; } }

		/// <summary>
		/// 技能效果数值3
		/// </summary>
		private float _effect3;
		public float effect3 { get { return _effect3; } private set { _effect3 = value; } }


		public static TableWeapon Get(int id)
		{
			return TableWeaponCollection.Instance.Get(id);
		}
		
		public static TableWeapon Get(Func<TableWeapon, bool> predicate)
        {
			return TableWeaponCollection.Instance.Get(predicate);
		}

        public static ICollection<TableWeapon> GetAll()
        {
            return TableWeaponCollection.Instance.GetAll();
        }
    }
}