using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableConstCollection
    {
        private Dictionary<int, TableConst> mDict = null;

        [NonSerialized]
        private static TableConstCollection _ins = null;
        public static TableConstCollection Instance
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

		public TableConst Get(int id)
        {
            TableConst data = null;
			_ins.mDict.TryGetValue(id, out data);
            return data;
        }

		public TableConst Get(Func<TableConst, bool> predicate)
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

        public ICollection<TableConst> GetAll()
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
            _ins = (TableConstCollection)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void Load()
        {
            var bytes = ResourceUtil.Load<TextAsset>(PathUtil.Table("TableConst")).bytes;
            Load(bytes);
        }

		private static byte[] AesDecrypt(byte[] bytes)
        {
			byte[] original = null;
			Rijndael Aes = Rijndael.Create();
			using (var Memory = new System.IO.MemoryStream(bytes))
			{
				var transform = Aes.CreateDecryptor(AesKey("TABLE_SECURITY"), AesKey("TableConst"));
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
    public class TableConst
    {
		/// <summary>
		/// <para>ID（正在使用 - 1）</para><para>可配置多套数据，通过修改id替换不同数据方案。</para>
		/// </summary>
		private int _id;
		public int id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 病毒尺寸1~5
		/// </summary>
		private float[] _virusSize;
		public float[] virusSize { get { return _virusSize; } private set { _virusSize = value; } }

		/// <summary>
		/// 病毒产生间隔随机范围
		/// </summary>
		private TRange _spawnVirusInterval;
		public TRange spawnVirusInterval { get { return _spawnVirusInterval; } private set { _spawnVirusInterval = value; } }

		/// <summary>
		/// 病毒产出方向范围(以下方向为0)
		/// </summary>
		private TRange _spawnVirusDirection;
		public TRange spawnVirusDirection { get { return _spawnVirusDirection; } private set { _spawnVirusDirection = value; } }

		/// <summary>
		/// <para>分裂病毒方向范围（以上方向为0）</para><para>最多3个分裂方向</para>
		/// </summary>
		private TRange[] _divideVirusDirection;
		public TRange[] divideVirusDirection { get { return _divideVirusDirection; } private set { _divideVirusDirection = value; } }

		/// <summary>
		/// 病毒血量随机范围
		/// </summary>
		private TRange _hpRandomRange;
		public TRange hpRandomRange { get { return _hpRandomRange; } private set { _hpRandomRange = value; } }

		/// <summary>
		/// 病毒速度随机范围
		/// </summary>
		private TRange _speedRandomRange;
		public TRange speedRandomRange { get { return _speedRandomRange; } private set { _speedRandomRange = value; } }

		/// <summary>
		/// 当前波怪物清空数量，（例如配置为2，当场上只剩下2个病毒时，则认为该波病毒已清空。）
		/// </summary>
		private int _waveClearVirusCount;
		public int waveClearVirusCount { get { return _waveClearVirusCount; } private set { _waveClearVirusCount = value; } }

		/// <summary>
		/// 当没有点击屏幕时，病毒减速，为1时不触发减速。
		/// </summary>
		private float _noTouchSlowDown;
		public float noTouchSlowDown { get { return _noTouchSlowDown; } private set { _noTouchSlowDown = value; } }

		/// <summary>
		/// 战斗中，击杀[1-5]等级病毒触发金币特效概率。（其实击杀每个病毒都有增加金币，但是为了不过于频繁，需要间歇触发。）
		/// </summary>
		private float[] _coinAddProb;
		public float[] coinAddProb { get { return _coinAddProb; } private set { _coinAddProb = value; } }

		/// <summary>
		/// 击杀[1-5]等级病毒金币价值
		/// </summary>
		private int[] _coinValue;
		public int[] coinValue { get { return _coinValue; } private set { _coinValue = value; } }

		/// <summary>
		/// 击杀震动时长
		/// </summary>
		private float _vibrateDuration;
		public float vibrateDuration { get { return _vibrateDuration; } private set { _vibrateDuration = value; } }

		/// <summary>
		/// 击杀震动最小间隔
		/// </summary>
		private float _vibrateInterval;
		public float vibrateInterval { get { return _vibrateInterval; } private set { _vibrateInterval = value; } }

		/// <summary>
		/// 击杀爆炸音效最小播放间隔
		/// </summary>
		private float _explosionSfxInterval;
		public float explosionSfxInterval { get { return _explosionSfxInterval; } private set { _explosionSfxInterval = value; } }

		/// <summary>
		/// 金币音效最小间隔
		/// </summary>
		private float _coinSfxInterval;
		public float coinSfxInterval { get { return _coinSfxInterval; } private set { _coinSfxInterval = value; } }


		public static TableConst Get(int id)
		{
			return TableConstCollection.Instance.Get(id);
		}
		
		public static TableConst Get(Func<TableConst, bool> predicate)
        {
			return TableConstCollection.Instance.Get(predicate);
		}

        public static ICollection<TableConst> GetAll()
        {
            return TableConstCollection.Instance.GetAll();
        }
    }
}