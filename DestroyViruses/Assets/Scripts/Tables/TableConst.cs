using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DestroyViruses
{
	[Serializable]
    public class TableConstCollection
    {
        private Dictionary<string, TableConst> mDict = null;

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

		public TableConst Get(string id)
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
		/// 配置组名
		/// </summary>
		private string _id;
		public string id { get { return _id; } private set { _id = value; } }

		/// <summary>
		/// 游戏帧数，-1表示不限制。
		/// </summary>
		private int _frameRate;
		public int frameRate { get { return _frameRate; } private set { _frameRate = value; } }

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

		/// <summary>
		/// 命中病毒减速系数
		/// </summary>
		private float _hitVirusSlowdown;
		public float hitVirusSlowdown { get { return _hitVirusSlowdown; } private set { _hitVirusSlowdown = value; } }

		/// <summary>
		/// 命中病毒减速作用时间
		/// </summary>
		private float _hitVirusSlowdownCD;
		public float hitVirusSlowdownCD { get { return _hitVirusSlowdownCD; } private set { _hitVirusSlowdownCD = value; } }

		/// <summary>
		/// 病毒爆炸震屏系数
		/// </summary>
		private float _virusBombShakeScreenRate;
		public float virusBombShakeScreenRate { get { return _virusBombShakeScreenRate; } private set { _virusBombShakeScreenRate = value; } }

		/// <summary>
		/// 命中病毒震动缩放倍数
		/// </summary>
		private float _hitVirusShakeScale;
		public float hitVirusShakeScale { get { return _hitVirusShakeScale; } private set { _hitVirusShakeScale = value; } }

		/// <summary>
		/// 命中病毒震动次数
		/// </summary>
		private int _hitVirusShakeTimes;
		public int hitVirusShakeTimes { get { return _hitVirusShakeTimes; } private set { _hitVirusShakeTimes = value; } }

		/// <summary>
		/// 命中病毒震动时间
		/// </summary>
		private float _hitVirusShakeCD;
		public float hitVirusShakeCD { get { return _hitVirusShakeCD; } private set { _hitVirusShakeCD = value; } }

		/// <summary>
		/// 每日签到解锁关卡
		/// </summary>
		private int _dailySignUnlockLevel;
		public int dailySignUnlockLevel { get { return _dailySignUnlockLevel; } private set { _dailySignUnlockLevel = value; } }

		/// <summary>
		/// 武器系统解锁关卡
		/// </summary>
		private int _weaponUnlockLevel;
		public int weaponUnlockLevel { get { return _weaponUnlockLevel; } private set { _weaponUnlockLevel = value; } }

		/// <summary>
		/// 图鉴系统解锁关卡
		/// </summary>
		private int _bookUnlockLevel;
		public int bookUnlockLevel { get { return _bookUnlockLevel; } private set { _bookUnlockLevel = value; } }

		/// <summary>
		/// 大厅飞船待机动画移动距离
		/// </summary>
		private float _aircraftHomeAnimaDist;
		public float aircraftHomeAnimaDist { get { return _aircraftHomeAnimaDist; } private set { _aircraftHomeAnimaDist = value; } }

		/// <summary>
		/// 大厅飞船位置（距底边）
		/// </summary>
		private float _aircraftHomePosY;
		public float aircraftHomePosY { get { return _aircraftHomePosY; } private set { _aircraftHomePosY = value; } }

		/// <summary>
		/// 大厅飞船缩放
		/// </summary>
		private float _aircraftHomeScale;
		public float aircraftHomeScale { get { return _aircraftHomeScale; } private set { _aircraftHomeScale = value; } }

		/// <summary>
		/// 累计杀n个病毒可收集图鉴奖励
		/// </summary>
		private int[] _bookVirusCollectKillCount;
		public int[] bookVirusCollectKillCount { get { return _bookVirusCollectKillCount; } private set { _bookVirusCollectKillCount = value; } }

		/// <summary>
		/// 累计杀n个病毒可收集图鉴奖励对应钻石数量
		/// </summary>
		private int[] _bookVirusCollectRewardDiamond;
		public int[] bookVirusCollectRewardDiamond { get { return _bookVirusCollectRewardDiamond; } private set { _bookVirusCollectRewardDiamond = value; } }

		/// <summary>
		/// 最大武器满级试用次数（每日）
		/// </summary>
		private int _maxWeaponTrialCount;
		public int maxWeaponTrialCount { get { return _maxWeaponTrialCount; } private set { _maxWeaponTrialCount = value; } }

		/// <summary>
		/// 每1个能量（体力值）恢复间隔时间（秒）
		/// </summary>
		private int _energyRecoverInterval;
		public int energyRecoverInterval { get { return _energyRecoverInterval; } private set { _energyRecoverInterval = value; } }

		/// <summary>
		/// 获胜获得体力值数量
		/// </summary>
		private int _energyRecoverWin;
		public int energyRecoverWin { get { return _energyRecoverWin; } private set { _energyRecoverWin = value; } }

		/// <summary>
		/// 每次战斗消耗体力值数量
		/// </summary>
		private int _energyBattleCost;
		public int energyBattleCost { get { return _energyBattleCost; } private set { _energyBattleCost = value; } }

		/// <summary>
		/// 体力值上限
		/// </summary>
		private int _energyMax;
		public int energyMax { get { return _energyMax; } private set { _energyMax = value; } }

		/// <summary>
		/// 体力值兑换（1钻石=x体力值）
		/// </summary>
		private int _energyExchange;
		public int energyExchange { get { return _energyExchange; } private set { _energyExchange = value; } }

		/// <summary>
		/// 子弹横向距离
		/// </summary>
		private float _bulletHDist;
		public float bulletHDist { get { return _bulletHDist; } private set { _bulletHDist = value; } }

		/// <summary>
		/// 子弹纵向距离
		/// </summary>
		private float _bulletVDist;
		public float bulletVDist { get { return _bulletVDist; } private set { _bulletVDist = value; } }

		/// <summary>
		/// 最大开火速度
		/// </summary>
		private float _maxFireSpeed;
		public float maxFireSpeed { get { return _maxFireSpeed; } private set { _maxFireSpeed = value; } }

		/// <summary>
		/// buff速度范围
		/// </summary>
		private TRange _buffSpeedRange;
		public TRange buffSpeedRange { get { return _buffSpeedRange; } private set { _buffSpeedRange = value; } }

		/// <summary>
		/// buff产生时方向范围(以下方向为0)
		/// </summary>
		private TRange _buffSpawnDirection;
		public TRange buffSpawnDirection { get { return _buffSpawnDirection; } private set { _buffSpawnDirection = value; } }

		/// <summary>
		/// 复活倒计时
		/// </summary>
		private int _reviveCountDown;
		public int reviveCountDown { get { return _reviveCountDown; } private set { _reviveCountDown = value; } }

		/// <summary>
		/// 复活后无敌时间
		/// </summary>
		private float _reviveInvincibleCD;
		public float reviveInvincibleCD { get { return _reviveInvincibleCD; } private set { _reviveInvincibleCD = value; } }

		/// <summary>
		/// 金币收益刷新CD
		/// </summary>
		private float _coinIncomeRefreshCD;
		public float coinIncomeRefreshCD { get { return _coinIncomeRefreshCD; } private set { _coinIncomeRefreshCD = value; } }

		/// <summary>
		/// 金币收益最长间隔时间
		/// </summary>
		private float _coinIncomeMaxDuration;
		public float coinIncomeMaxDuration { get { return _coinIncomeMaxDuration; } private set { _coinIncomeMaxDuration = value; } }

		/// <summary>
		/// 病毒血量受关卡火力值影响数值
		/// </summary>
		private string _formulaArgsVirusHp;
		public string formulaArgsVirusHp { get { return _formulaArgsVirusHp; } private set { _formulaArgsVirusHp = value; } }

		/// <summary>
		/// 病毒产生数量受关卡火力值影响数值
		/// </summary>
		private string _formulaArgsVirusSpawnCount;
		public string formulaArgsVirusSpawnCount { get { return _formulaArgsVirusSpawnCount; } private set { _formulaArgsVirusSpawnCount = value; } }

		/// <summary>
		/// 兑换金币受金币价值影响数值
		/// </summary>
		private string _formulaArgsCoinExchange;
		public string formulaArgsCoinExchange { get { return _formulaArgsCoinExchange; } private set { _formulaArgsCoinExchange = value; } }

		/// <summary>
		/// 签到金币受金币价值影响
		/// </summary>
		private string _formulaArgsDailySignCoin;
		public string formulaArgsDailySignCoin { get { return _formulaArgsDailySignCoin; } private set { _formulaArgsDailySignCoin = value; } }


		public static TableConst Get(string id)
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