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
		/// LV.
		/// </summary>
		private string _LEVEL_DOT;
		public string LEVEL_DOT { get { return _LEVEL_DOT; } private set { _LEVEL_DOT = value; } }

		/// <summary>
		/// 升级
		/// </summary>
		private string _UPGRADE;
		public string UPGRADE { get { return _UPGRADE; } private set { _UPGRADE = value; } }

		/// <summary>
		/// 满级
		/// </summary>
		private string _LEVEL_MAX;
		public string LEVEL_MAX { get { return _LEVEL_MAX; } private set { _LEVEL_MAX = value; } }

		/// <summary>
		/// 热更资源：
		/// </summary>
		private string _UPDATE_RESOURCE;
		public string UPDATE_RESOURCE { get { return _UPDATE_RESOURCE; } private set { _UPDATE_RESOURCE = value; } }

		/// <summary>
		/// 快速热更：
		/// </summary>
		private string _QUICK_UPDATE_RESOURCE;
		public string QUICK_UPDATE_RESOURCE { get { return _QUICK_UPDATE_RESOURCE; } private set { _QUICK_UPDATE_RESOURCE = value; } }

		/// <summary>
		/// 检查热更资源...
		/// </summary>
		private string _CHECK_UPDATE_RESOURCE;
		public string CHECK_UPDATE_RESOURCE { get { return _CHECK_UPDATE_RESOURCE; } private set { _CHECK_UPDATE_RESOURCE = value; } }

		/// <summary>
		/// 准备热更资源...
		/// </summary>
		private string _READY_UPDATE_RESOURCE;
		public string READY_UPDATE_RESOURCE { get { return _READY_UPDATE_RESOURCE; } private set { _READY_UPDATE_RESOURCE = value; } }

		/// <summary>
		/// 恭喜过关
		/// </summary>
		private string _GAME_END_WIN;
		public string GAME_END_WIN { get { return _GAME_END_WIN; } private set { _GAME_END_WIN = value; } }

		/// <summary>
		/// 游戏结束
		/// </summary>
		private string _GAME_END_LOSE;
		public string GAME_END_LOSE { get { return _GAME_END_LOSE; } private set { _GAME_END_LOSE = value; } }

		/// <summary>
		/// 剩余病毒:
		/// </summary>
		private string _REMAIN_VIRUS_COUNT;
		public string REMAIN_VIRUS_COUNT { get { return _REMAIN_VIRUS_COUNT; } private set { _REMAIN_VIRUS_COUNT = value; } }

		/// <summary>
		/// 滑动屏幕开始游戏
		/// </summary>
		private string _SLIDE_TO_START;
		public string SLIDE_TO_START { get { return _SLIDE_TO_START; } private set { _SLIDE_TO_START = value; } }

		/// <summary>
		/// 火力
		/// </summary>
		private string _FIRE_POWER;
		public string FIRE_POWER { get { return _FIRE_POWER; } private set { _FIRE_POWER = value; } }

		/// <summary>
		/// 射速
		/// </summary>
		private string _FIRE_SPEED;
		public string FIRE_SPEED { get { return _FIRE_SPEED; } private set { _FIRE_SPEED = value; } }

		/// <summary>
		/// 功能尚未解锁
		/// </summary>
		private string _FUNCTION_LOCKED;
		public string FUNCTION_LOCKED { get { return _FUNCTION_LOCKED; } private set { _FUNCTION_LOCKED = value; } }

		/// <summary>
		/// 点击领取
		/// </summary>
		private string _CLICK_TO_RECEIVE;
		public string CLICK_TO_RECEIVE { get { return _CLICK_TO_RECEIVE; } private set { _CLICK_TO_RECEIVE = value; } }


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