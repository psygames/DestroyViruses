using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableAdsCollection
    {
        private Dictionary<string, TableAds> mDict = null;

        [NonSerialized]
        private static TableAdsCollection _ins = null;
        public static TableAdsCollection Instance
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

		public TableAds Get(string id)
        {
            TableAds data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableAds Get(Func<TableAds, bool> predicate)
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

        public ICollection<TableAds> GetAll()
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
            _ins = (TableAdsCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableAds")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableAds"));
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
    public class TableAds
    {
		/// <summary>
		/// ID
		/// </summary>
		private string _id;
		public string id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 程序用，副武器类型
		/// </summary>
		private string _desc;
		public string desc { get { return _desc; } private set { _desc = value; } }

		/// <summary>
		/// 名称
		/// </summary>
		private string _unitID;
		public string unitID { get { return _unitID; } private set { _unitID = value; } }

		/// <summary>
		/// <para>类型， Banner, Interstitial, RewardedVideo</para><para></para>
		/// </summary>
		private string _type;
		public string type { get { return _type; } private set { _type = value; } }

		/// <summary>
		/// <para>播放优先级</para><para>0 不加载</para>
		/// </summary>
		private float _priority;
		public float priority { get { return _priority; } private set { _priority = value; } }


		public static TableAds Get(string id)
		{
			return TableAdsCollection.Instance.Get(id);
		}
		
		public static TableAds Get(Func<TableAds, bool> predicate)
        {
			return TableAdsCollection.Instance.Get(predicate);
		}

        public static ICollection<TableAds> GetAll()
        {
            return TableAdsCollection.Instance.GetAll();
        }
    }
}