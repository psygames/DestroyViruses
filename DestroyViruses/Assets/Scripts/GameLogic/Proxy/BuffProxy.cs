using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DestroyViruses
{
    public class BuffProxy : ProxyBase<BuffProxy>
    {
        public Dictionary<int, BuffData> buffs = new Dictionary<int, BuffData>();

        private Dictionary<string, BuffData> mEffectValues = new Dictionary<string, BuffData>();

        public void AddBuff(int id)
        {
            var buff = GetBuff(id);
            if (buff == null)
            {
                buff = new BuffData();
                buffs.Add(id, buff);
            }
            buff.SetData(id, GameUtil.runningTime);
        }

        public BuffData GetBuff(int id)
        {
            buffs.TryGetValue(id, out var buff);
            return buff;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var key in buffs.Keys.ToList())
            {
                if (buffs[key].progress >= 1f || Mathf.Approximately(buffs[key].progress, 1))
                {
                    buffs.Remove(key);
                }
            }

            mEffectValues.Clear();
            foreach (var kv in buffs)
            {
                mEffectValues[kv.Value.effect] = kv.Value;
            }
        }

        protected override void OnDestroy()
        {
            buffs.Clear();
            base.OnDestroy();
        }

        public BuffData GetBuff(string buffType)
        {
            mEffectValues.TryGetValue(buffType, out var val);
            return val;
        }

        public float GetEffectValue(string effect, float defaultVal = 1f)
        {
            if (mEffectValues.TryGetValue(effect, out var val))
                return val.param1;
            return defaultVal;
        }

        public bool HasEffect(string effect)
        {
            return mEffectValues.TryGetValue(effect, out _);
        }

        public float Effect_FireSpeed { get { return GetEffectValue("fire_speed"); } }
        public float Effect_FirePower { get { return GetEffectValue("fire_power"); } }
        public float Effect_Coin { get { return GetEffectValue("coin"); } }
        public float Effect_Knockback { get { return GetEffectValue("knockback", 0f); } }
        public float Effect_Support { get { return GetEffectValue("support", 0f); } }
        public float Effect_Slowdown { get { return GetEffectValue("slowdown"); } }
        public float Effect_WeaponUp { get { return GetEffectValue("weapon_up"); } }
        public float Effect_BoostVirus { get { return GetEffectValue("boost_virus", 0); } }
        public float Effect_MoveLimitation { get { return GetEffectValue("move_limitation"); } }
        public float Effect_LiveUpVirus { get { return GetEffectValue("live_up_virus"); } }


        public bool Has_Effect_FireSpeed { get { return HasEffect("fire_speed"); } }
        public bool Has_Effect_FirePower { get { return HasEffect("fire_power"); } }
        public bool Has_Effect_Coin { get { return HasEffect("coin"); } }
        public bool Has_Effect_Knockback { get { return HasEffect("knockback"); } }
        public bool Has_Effect_Support { get { return HasEffect("support"); } }
        public bool Has_Effect_Slowdown { get { return HasEffect("slowdown"); } }
        public bool Has_Effect_WeaponUp { get { return HasEffect("weapon_up"); } }
        public bool Has_Effect_BoostVirus { get { return HasEffect("boost_virus"); } }
        public bool Has_Effect_MoveLimitation { get { return HasEffect("move_limitation"); } }
        public bool Has_Effect_LiveUpVirus { get { return HasEffect("live_up_virus"); } }
    }

    public class BuffData
    {
        public int id { get; private set; }
        public string name { get; private set; }
        public float startTime { get; private set; }
        public TableBuff table { get; private set; }
        public float progress { get { return Mathf.Clamp01((GameUtil.runningTime - startTime) / table.effectDuration); } }
        public string effect { get { return table.effect; } }
        public float param1 { get { return table.param1; } }
        public Vector2 param2 { get { return table.param2.value; } }
        public string icon { get { return table.icon; } }
        public string uiIcon { get { return "ui_" + table.icon; } }

        public void SetData(int id, float time)
        {
            this.id = id;
            this.startTime = time;
            this.table = TableBuff.Get(id);
            this.name = LT.Get(table.nameID);
        }
    }
}