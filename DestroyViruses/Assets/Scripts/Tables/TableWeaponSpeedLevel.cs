using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableWeaponSpeedLevelCollection
    {
        private Dictionary<int, TableWeaponSpeedLevel> mDict = null;

        [NonSerialized]
        private static TableWeaponSpeedLevelCollection _ins = null;
        public static TableWeaponSpeedLevelCollection Instance
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

		public TableWeaponSpeedLevel Get(int id)
        {
            TableWeaponSpeedLevel data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableWeaponSpeedLevel Get(Func<TableWeaponSpeedLevel, bool> predicate)
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

        public ICollection<TableWeaponSpeedLevel> GetAll()
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
            _ins = (TableWeaponSpeedLevelCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableWeaponSpeedLevel")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableWeaponSpeedLevel"));
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
    public class TableWeaponSpeedLevel
    {
		/// <summary>
		/// ID没用，不重复即可
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 武器ID
		/// </summary>
		private int _weaponId;
		public int weaponId { get { return _weaponId; } private set { _weaponId = value; } }

		/// <summary>
		/// 武器等级
		/// </summary>
		private int _level;
		public int level { get { return _level; } private set { _level = value; } }

		/// <summary>
		/// 充能时间
		/// </summary>
		private float _recharge;
		public float recharge { get { return _recharge; } private set { _recharge = value; } }

		/// <summary>
		/// 影响效果数值
		/// </summary>
		private float _effectFactor1;
		public float effectFactor1 { get { return _effectFactor1; } private set { _effectFactor1 = value; } }

		/// <summary>
		/// 影响效果数值
		/// </summary>
		private float _effectFactor2;
		public float effectFactor2 { get { return _effectFactor2; } private set { _effectFactor2 = value; } }

		/// <summary>
		/// 影响效果数值
		/// </summary>
		private float _effectFactor3;
		public float effectFactor3 { get { return _effectFactor3; } private set { _effectFactor3 = value; } }

		/// <summary>
		/// 影响效果数值
		/// </summary>
		private float _effectFactor4;
		public float effectFactor4 { get { return _effectFactor4; } private set { _effectFactor4 = value; } }

		/// <summary>
		/// 影响效果数值
		/// </summary>
		private float _effectFactor5;
		public float effectFactor5 { get { return _effectFactor5; } private set { _effectFactor5 = value; } }

		/// <summary>
		/// 升级消耗
		/// </summary>
		private float _upCost;
		public float upCost { get { return _upCost; } private set { _upCost = value; } }


		public static TableWeaponSpeedLevel Get(int id)
		{
			return TableWeaponSpeedLevelCollection.Instance.Get(id);
		}
		
		public static TableWeaponSpeedLevel Get(Func<TableWeaponSpeedLevel, bool> predicate)
        {
			return TableWeaponSpeedLevelCollection.Instance.Get(predicate);
		}

        public static ICollection<TableWeaponSpeedLevel> GetAll()
        {
            return TableWeaponSpeedLevelCollection.Instance.GetAll();
        }
    }
}