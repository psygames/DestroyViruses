using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnibusEvent;
using UniRx;

namespace DestroyViruses
{
    public class VirusBase : EntityBase
    {
        public const float baseRadius = 125f;

        public Text hpText;
        public Animator animator;

        public new Collider2D collider2D { get; private set; }
        public ParticleSystem cureEffect { get; private set; }
        public ParticleSystem stunEffect { get; private set; }

        public int id { get; protected set; }
        public TableVirus table { get; protected set; }
        public float cd { get; protected set; }
        public float hp { get; protected set; }
        public bool isHpFull { get { return hp >= hpTotal; } }
        public float hpTotal { get; protected set; }
        public Vector2 hpRange { get; protected set; }
        public int size { get; protected set; }
        public float speed { get; protected set; }
        public Vector2 direction { get; protected set; }
        public Vector2 position { get; protected set; }
        public bool isInvincible { get; protected set; }
        public bool isStun { get { return mStunCD > 0; } }
        public bool isMatrix { get; protected set; }
        public float radius { get; private set; }
        public float scale { get; private set; }
        public int colorIndex { get; private set; }
        protected virtual float speedMul { get; set; } = 1f;


        private int mLastColorIndex = -1;
        private float mLastHp = -1;
        private float mKnockback = 0;
        private float mShakeScale = 1;
        private float mStunCD = 0;

        protected virtual void Awake()
        {
            collider2D = GetComponent<Collider2D>();
            cureEffect = transform.Find("virus_base/cure_buff").GetComponent<ParticleSystem>();
            cureEffect.Stop(true);
            stunEffect = transform.Find("virus_base/stun_effect").GetComponent<ParticleSystem>();
            stunEffect.Stop(true);
        }

        protected virtual void OnEnable()
        {
            this.BindUntilDisable<EventBullet>(OnEventBullet);
        }

        public virtual void Reset(int id, float hp, int size, float speed, Vector2 pos, Vector2 direction, Vector2 hpRange, bool isMatrix)
        {
            this.id = id;
            table = TableVirus.Get(id);
            cd = table.skillCD;
            hpTotal = hp;
            this.hp = hp;
            this.size = size;
            this.speed = speed;
            this.hpRange = hpRange;
            this.direction = direction;
            this.isMatrix = isMatrix;
            isAlive = true;
            position = pos;
            isInvincible = false;
            rectTransform.anchoredPosition = pos;
            scale = GetSizeScale(size);
            radius = baseRadius * scale;
            rectTransform.localScale = Vector3.one * scale;

            mLastColorIndex = -1;
            mLastHp = -1;
            mLastScale = 1f;
            mLastHitSlowdownTime = 0;
            mStunCD = 0;

            cureEffect.Stop(true);
            stunEffect.Stop(true);
        }

        // FOR HOME ONLY
        public void SetColor(int colorIndex)
        {
            OnColorChanged(colorIndex);
            hpText.text = "";
        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        public void SetHp(float hp)
        {
            this.hp = Mathf.Clamp(hp, 0, hpTotal);
        }

        public void SetInvincible(bool value)
        {
            this.isInvincible = value;
        }

        public void ForceRecycle()
        {
            hp = 0;
            isAlive = false;
            Recycle();
        }

        private float GetSizeScale(int _size)
        {
            var index = Mathf.Clamp(_size - 1, 0, CT.table.virusSize.Length - 1);
            return CT.table.virusSize[index];
        }

        private void OnEventBullet(EventBullet evt)
        {
            if (evt.target != this)
                return;

            if (evt.action == EventBullet.Action.HIT)
            {
                BeHit(evt.damage);
                hp = Mathf.Max(0f, hp - evt.damage);
                if (Mathf.Approximately(hp, 0))
                {
                    BeDead();
                }
            }
        }

        private void Knockback()
        {
            mKnockback = radius * ProxyManager.GetProxy<BuffProxy>().Effect_Knockback;
        }

        private void BeHit(float damage)
        {
            PlayHurt();
            Knockback();
            BeHitSlowDown();
            Unibus.Dispatch(EventVirus.Get(EventVirus.Action.BE_HIT, this, damage));
        }


        private void PlayHurt()
        {
            if (animator != null
                && TimeUtil.CheckInterval("virus_" + uid + "_hurt", 0.2f)
                )
            {
                animator.SetTrigger("hurt");
            }

            if (TimeUtil.CheckInterval("virus_" + uid + "_hurt_shake", CT.table.hitVirusShakeCD))
            {
                DOTween.Shake(() => Vector3.one * mShakeScale, x => mShakeScale = x.x
                , CT.table.hitVirusShakeCD, Vector3.one * CT.table.hitVirusShakeScale
                , CT.table.hitVirusShakeTimes);
            }
        }

        private void BeHitSlowDown()
        {
            isHitSlowdown = true;
            mLastHitSlowdownTime = GameUtil.runningTime;
        }

        bool isHitSlowdown = false;
        float mLastHitSlowdownTime = 0;
        private void UpdateHitSlowdown()
        {
            var _cd = CT.table.hitVirusSlowdownCD;
            if (isHitSlowdown && GameUtil.runningTime - mLastHitSlowdownTime > _cd)
            {
                isHitSlowdown = false;
            }
        }

        private void BeDead()
        {
            isAlive = false;
            Unibus.Dispatch(EventVirus.Get(EventVirus.Action.DEAD, this, hpTotal));
            Recycle();
            PlayDead();
            Divide();
            GenBuff();
        }

        private void PlayDead()
        {
            ExplosionVirus.Create().Reset(rectTransform.anchoredPosition, table.id, scale);
        }

        private void GenBuff()
        {
            bool isGen = false;
            var _tab = TableBuffKillGen.Get(a => a.gameLevel.Contains(D.I.gameLevel) && a.streak == D.I.streak);
            var _prob = D.I.IsVip() ? _tab.vipProbability : _tab.probability;
            float ratio = _prob * D.I.kills4Buff;
            if (Random.value <= ratio)
            {
                isGen = true;
                var _typeProb = D.I.IsVip() ? _tab.vipBuffTypePriority : _tab.buffTypePriority;
                var buffID = FormulaUtil.RandomInProbDict(_typeProb);
                var _speed = CT.table.buffSpeedRange.random;
                var dir = Quaternion.AngleAxis(CT.table.buffSpawnDirection.random, Vector3.forward) * Vector2.down;
                Buff.Create().Reset(buffID, position, dir, _speed);
            }

            if (isGen)
            {
                D.I.kills4Buff = 0;
            }
        }

        private void Divide()
        {
            if (this.size <= 1)
                return;

            var _hp = Mathf.Ceil(hpTotal * 0.5f);
            var _size = size - 1;
            var pos = transform.GetUIPos();

            Vector2 dirA = Quaternion.AngleAxis(CT.table.divideVirusDirection[0].random, Vector3.forward) * Vector2.up;
            Create().Reset(id, _hp, _size, speed, pos, dirA, hpRange, false);

            Vector2 dirB = Quaternion.AngleAxis(CT.table.divideVirusDirection[1].random, Vector3.forward) * Vector2.up;
            Create().Reset(id, _hp, _size, speed, pos, dirB, hpRange, false);
        }

        protected VirusBase Create()
        {
            return EntityManager.Create(GetType()) as VirusBase;
        }

        protected virtual void UpdateHp()
        {
            if (!Mathf.Approximately(mLastHp, hp))
            {
                hpText.text = hp.KMB();
                mLastHp = hp;
            }
        }

        protected virtual void UpdatePosition()
        {
            if (!isAlive || isStun)
                return;

            var uiPos = UIUtil.GetUIPos(rectTransform);
            if (uiPos.y < -baseRadius)
            {
                position = new Vector2(rectTransform.anchoredPosition.x, UIUtil.height);
            }
            else if (uiPos.y > UIUtil.height - baseRadius)
            {
                direction = new Vector2(direction.x, -Mathf.Abs(direction.y));
            }
            if (uiPos.x < baseRadius)
            {
                direction = new Vector2(Mathf.Abs(direction.x), direction.y);
            }
            else if (uiPos.x > UIUtil.width - baseRadius)
            {
                direction = new Vector2(-Mathf.Abs(direction.x), direction.y);
            }

            float speedScale = GlobalData.slowDownFactor * BuffSpeedMul() * HitSlowdownSpeedMul() * speedMul;
            position += direction * speed * speedScale * Time.deltaTime + Vector2.up * mKnockback;
            rectTransform.anchoredPosition = position;

            mKnockback = 0;
        }

        protected float HitSlowdownSpeedMul()
        {
            if (isHitSlowdown)
                return CT.table.hitVirusSlowdown;
            return 1;
        }

        protected float BuffSpeedMul()
        {
            var proxy = ProxyManager.GetProxy<BuffProxy>();
            return proxy.Effect_Slowdown * proxy.Effect_LiveUpVirus;
        }

        protected float mLastScale = 1f;
        protected virtual void UpdateScale()
        {
            var proxy = ProxyManager.GetProxy<BuffProxy>();
            var _scale = GetSizeScale((int)proxy.Effect_BoostVirus + size) * mShakeScale;
            if (!Mathf.Approximately(mLastScale, _scale))
            {
                mLastScale = proxy.Effect_BoostVirus;
                rectTransform.localScale = new Vector3(_scale, _scale, 1);
            }
        }

        protected virtual void UpdateColor()
        {
            colorIndex = FormulaUtil.GetHpColorIndex(hpRange, hp);
            if (mLastColorIndex != colorIndex)
            {
                OnColorChanged(colorIndex);
                mLastColorIndex = colorIndex;
            }
        }

        protected virtual void OnSkillTrigger()
        {

        }

        protected virtual void UpdateCD()
        {
            var lastStunCD = mStunCD;
            mStunCD = this.UpdateCD(mStunCD, GlobalData.slowDownFactor);

            if (mStunCD <= 0 && lastStunCD > 0)
            {
                stunEffect.Stop(true);
            }

            if (table.skillCD > 0 && !isStun)
            {
                cd = this.UpdateCD(cd, GlobalData.slowDownFactor);
                if (cd <= 0)
                {
                    cd = table.skillCD;
                    OnSkillTrigger();
                }
            }
        }

        protected virtual void OnColorChanged(int index)
        {
            if (animator != null)
            {
                animator.GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture
                    = ResourceUtil.Load<Texture2D>(PathUtil.Texture($"{GetType().Name}Texture_{index + 1}"));
            }
        }

        protected virtual void Update()
        {
            if (GameUtil.isFrozen)
            {
                return;
            }

            UpdateHp();
            UpdateHitSlowdown();
            UpdatePosition();
            UpdateScale();
            UpdateColor();
            UpdateCD();
        }

        protected float GetDist(VirusBase virus)
        {
            return Mathf.Max(0, (position - virus.position).magnitude - radius - virus.radius);
        }

        public void PlayCure()
        {
            cureEffect.Play(true);
        }

        public void Stun(float duration)
        {
            mStunCD = duration;
            stunEffect.Play(true);
        }
    }
}