using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusSwallow : VirusBase
    {
        public Image triangleImage1;
        public Image triangleImage2;

        private float mCheckCD = 0;
        private float mSwallowCD = 0;
        private float mSwallowVirusSpeed = 0;

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);

            triangleImage1.SetSprite($"virus_shape_swallow_{index}");
            triangleImage2.SetSprite($"virus_shape_swallow_{index}");
        }

        protected override void OnSkillTrigger()
        {
            base.OnSkillTrigger();
            if (mCheckCD > 0 || mSwallowCD > 0)
                return;

            mCheckCD = 0.2f;
        }

        protected override void UpdatePosition()
        {
            if (mSwallowCD > 0)
            {
            }
            else
            {
                base.UpdatePosition();
            }
        }

        protected override void Update()
        {
            base.Update();

            mCheckCD = this.UpdateCD(mCheckCD);
            var _lastSCD = mSwallowCD;
            mSwallowCD = this.UpdateCD(mSwallowCD);

            if (_lastSCD > 0 && mSwallowCD <= 0)
            {
                var newVirus = EntityManager.Create<VirusSwallow>();
                newVirus.Reset(id, hp, size, mSwallowVirusSpeed, position, -direction, hpRange);
            }

            SetInvincible(mSwallowCD > 0);
            collider2D.enabled = mSwallowCD <= 0;
            hpText.gameObject.SetActive(!isInvincible);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (mCheckCD <= 0)
                return;
            if (collision.tag == TagUtil.Virus)
            {
                var virus = collision.GetComponent<VirusBase>();
                if (virus != null && virus.isAlive && !virus.isInvincible
                    && virus.GetType() != typeof(VirusSwallow)
                    && virus.size < size)
                {
                    mCheckCD = 0;
                    mSwallowCD = table.effect1;
                    mSwallowVirusSpeed = virus.speed;
                    virus.ForceRecycle();
                }
            }
        }
    }
}