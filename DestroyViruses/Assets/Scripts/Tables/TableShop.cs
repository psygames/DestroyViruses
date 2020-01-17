using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableShopCollection
    {
        private Dictionary<string, TableShop> mDict = null;

        [NonSerialized]
        private static TableShopCollection _ins = null;
        public static TableShopCollection Instance
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

		public TableShop Get(string id)
        {
            TableShop data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableShop Get(Func<TableShop, bool> predicate)
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

        public ICollection<TableShop> GetAll()
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
            _ins = (TableShopCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableShop")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableShop"));
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
    public class TableShop
    {
		/// <summary>
		/// ID
		/// </summary>
		private string _id;
		public string id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// <para>描述</para><para>（策划用）</para>
		/// </summary>
		private string _desc;
		public string desc { get { return _desc; } private set { _desc = value; } }

		/// <summary>
		/// 名称
		/// </summary>
		private string _productID;
		public string productID { get { return _productID; } private set { _productID = value; } }

		/// <summary>
		/// 图片
		/// </summary>
		private string _image;
		public string image { get { return _image; } private set { _image = value; } }

		/// <summary>
		/// 名称多语言
		/// </summary>
		private string _nameID;
		public string nameID { get { return _nameID; } private set { _nameID = value; } }

		/// <summary>
		/// 描述多语言
		/// </summary>
		private string _descriptionID;
		public string descriptionID { get { return _descriptionID; } private set { _descriptionID = value; } }

		/// <summary>
		/// <para>Consumable = 0</para><para>NonConsumable = 1</para><para>Subscription = 2</para>
		/// </summary>
		private int _type;
		public int type { get { return _type; } private set { _type = value; } }

		/// <summary>
		/// 钻石数量（对应到Store奖励数量）
		/// </summary>
		private int _diamonds;
		public int diamonds { get { return _diamonds; } private set { _diamonds = value; } }

		/// <summary>
		/// 额外获得（游戏内额外给予）
		/// </summary>
		private int _extra;
		public int extra { get { return _extra; } private set { _extra = value; } }


		public static TableShop Get(string id)
		{
			return TableShopCollection.Instance.Get(id);
		}
		
		public static TableShop Get(Func<TableShop, bool> predicate)
        {
			return TableShopCollection.Instance.Get(predicate);
		}

        public static ICollection<TableShop> GetAll()
        {
            return TableShopCollection.Instance.GetAll();
        }
    }
}