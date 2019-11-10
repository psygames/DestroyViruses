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
        public float radius { get; private set; }
        public float scale { get; private set; }

        private int mLastColorIndex = -1;
        private float mLastHp = -1;

        private void OnEnable()
        {
            this.BindUntilDisable<EventBullet>(OnEventBullet);
        }

        public virtual void Reset(int id, float hp, int size, float speed, Vector2 pos, Vector2 direction, Vector2 hpRange)
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

        private float GetSizeScale(int size)
        {
            return 1 / Mathf.Sqrt(6 - Mathf.Clamp(size, 0, 5));
        }

        private void OnEventBullet(EventBullet evt)
        {
            if (evt.target != this)
                return;

            if (evt.action == EventBullet.Action.HIT)
            {
                BeHit(evt.damage);
                hp = Mathf.Max(0, hp - evt.damage);
                if (hp <= 0)
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
            // isShaked = true;
        }

        bool isShaked = false;
        private void UpdateShake()
        {
            if (isShaked)
            {
                shakeOffset = Random.insideUnitSphere * 5;
                isShaked = false;
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
            int colorCount = 4;
            int type = (int)((hpTotal - hpRange.x) / (hpRange.y - hpRange.x) * colorCount);
            type = colorCount - Mathf.Clamp(type, 0, colorCount - 1);
            ExplosionVirus.Create().Reset(rectTransform.anchoredPosition, type);
        }

        private void Divide()
        {
            if (this.size <= 1)
                return;

            // TODO: 血量较少的不产生分裂
            // if ((hpTotal - hpRange.x) < (hpRange.y - hpRange.x) * 0.2f)
            //    return;

            var hp = hpTotal * 0.5f;
            var size = this.size - 1;
            var pos = transform.GetUIPos();

            Vector2 dirA = Quaternion.AngleAxis(Random.Range(-60, -80), Vector3.forward) * Vector2.up;
            Create().Reset(id, hp, size, speed, pos + dirA * baseRadius * GetSizeScale(size), dirA, hpRange);

            Vector2 dirB = Quaternion.AngleAxis(Random.Range(60, 80), Vector3.forward) * Vector2.up;
            Create().Reset(id, hp, size, speed, pos + dirB * baseRadius * GetSizeScale(size), dirB, hpRange);
        }

        protected VirusBase Create()
        {
            return EntityManager.Create(GetType()) as VirusBase;
        }

        protected virtual void UpdateHp()
        {
            if (mLastHp != hp)
            {
                hpText.text = hp.KMB();
                mLastHp = hp;
            }
        }

        protected virtual void UpdatePosition()
        {
            if (!isAlive || isInvincible)
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
            position += direction * speed * Time.deltaTime;
            rectTransform.anchoredPosition = shakeOffset + position;
        }

        protected virtual void UpdateColor()
        {
            int colorCount = 9;
            int index = (int)((hp - hpRange.x) / (hpRange.y - hpRange.x) * colorCount);
            index = colorCount - Mathf.Clamp(index, 0, colorCount - 1) - 1;
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