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
        public Image glowImage;

        public new Collider2D collider2D { get; private set; }

        public int id { get; protected set; }
        public TableVirus table { get; protected set; }
        public float cd { get; protected set; }
        public float hp { get; protected set; }
        public float hpTotal { get; protected set; }
        public Vector2 hpRange { get; protected set; }
        public int size { get; protected set; }
        public float speed { get; protected set; }
        public Vector2 direction { get; protected set; }
        public Vector2 position { get; protected set; }
        public Vector2 shakeOffset { get; protected set; }
        public bool isInvincible { get; protected set; }
        public bool isMatrix { get; protected set; }
        public float radius { get; private set; }
        public float scale { get; private set; }
        public float slowDownFactor
        {
            get
            {
                if (!GlobalData.isBattleTouchOn)
                    return ConstTable.table.noTouchSlowDown;
                return 1f;
            }
        }
        protected virtual float speedMul { get; set; } = 1f;

        private int mLastColorIndex = -1;
        private float mLastHp = -1;

        protected virtual void Awake()
        {
            collider2D = GetComponent<Collider2D>();
        }

        protected virtual void OnEnable()
        {
            this.BindUntilDisable<EventBullet>(OnEventBullet);
        }

        public virtual void Reset(int id, float hp, int size, float speed, Vector2 pos, Vector2 direction, Vector2 hpRange,bool isMatrix)
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
            shakeOffset = Vector2.zero;
            scale = GetSizeScale(size);
            radius = baseRadius * scale;
            rectTransform.localScale = Vector3.one * scale;

            mLastColorIndex = -1;
            mLastHp = -1;
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
            return ConstTable.table.virusSize[_size - 1];
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

        private void BeHit(float damage)
        {
            Unibus.Dispatch(EventVirus.Get(EventVirus.Action.BE_HIT, this, damage));
            PlayHit();
        }


        private void PlayHit()
        {
            // TODO: OPEN SHAKE?
            isShaked = true;
        }

        bool isShaked = false;
        float shakeInterval = 0.1f;
        float mLastShakeTime = 0;
        float shakeDist = 5;
        private void UpdateShake()
        {
            if (isShaked && Time.time - mLastShakeTime > shakeInterval)
            {
                shakeOffset = Random.insideUnitSphere * shakeDist;
                isShaked = false;
                mLastShakeTime = Time.time;
            }
        }

        private void BeDead()
        {
            isAlive = false;
            Unibus.Dispatch(EventVirus.Get(EventVirus.Action.DEAD, this, hpTotal));
            Recycle();
            PlayDead();
            Divide();
        }

        private void PlayDead()
        {
            //var index = FormulaUtil.GetHpColorIndex(hpRange, hpTotal, 4);
            var index = Random.Range(0, 4);
            ExplosionVirus.Create().Reset(rectTransform.anchoredPosition, index + 1, scale * 2);
        }

        private void Divide()
        {
            if (this.size <= 1)
                return;

            // TODO: 血量较少的不产生分裂
            // if ((hpTotal - hpRange.x) < (hpRange.y - hpRange.x) * 0.2f)
            //    return;

            var _hp = Mathf.Ceil(hpTotal * 0.5f);
            var _size = size - 1;
            var pos = transform.GetUIPos();

            Vector2 dirA = Quaternion.AngleAxis(Random.Range(-60, -80), Vector3.forward) * Vector2.up;
            Create().Reset(id, _hp, _size, speed, pos + dirA * baseRadius * GetSizeScale(_size), dirA, hpRange,false);

            Vector2 dirB = Quaternion.AngleAxis(Random.Range(60, 80), Vector3.forward) * Vector2.up;
            Create().Reset(id, _hp, _size, speed, pos + dirB * baseRadius * GetSizeScale(_size), dirB, hpRange,false);
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
            if (!isAlive)
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
            position += direction * speed * slowDownFactor * speedMul * Time.deltaTime;
            rectTransform.anchoredPosition = shakeOffset + position;
        }

        protected virtual void UpdateColor()
        {
            var index = FormulaUtil.GetHpColorIndex(hpRange, hp);
            if (mLastColorIndex != index)
            {
                OnColorChanged(index);
                mLastColorIndex = index;
            }
        }

        protected virtual void OnSkillTrigger()
        {

        }

        protected virtual void UpdateCD()
        {
            if (table.skillCD <= 0)
                return;

            cd = this.UpdateCD(cd);
            if (cd <= 0)
            {
                cd = table.skillCD;
                OnSkillTrigger();
            }
        }

        protected virtual void OnColorChanged(int index)
        {
            glowImage.SetSprite($"virus_glow_circle_{index}");
        }

        protected virtual void Update()
        {
            if (GameUtil.isFrozen)
            {
                return;
            }

            UpdateHp();
            UpdateShake();
            UpdatePosition();
            UpdateColor();
            UpdateCD();
        }

        protected float GetDist(VirusBase virus)
        {
            return Mathf.Max(0, (position - virus.position).magnitude - radius - virus.radius);
        }
    }
}