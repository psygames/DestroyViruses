using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusSwallow : VirusBase
    {
        private float mCheckCD = 0;
        private float mSwallowCD = 0;
        private float mSwallowVirusSpeed = 0;

        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);
        }

        protected override void OnSkillTrigger()
        {
            base.OnSkillTrigger();
            if (mCheckCD > 0 || mSwallowCD > 0)
                return;

            mCheckCD = 0.2f;
        }

        protected override void Update()
        {
            base.Update();

            mCheckCD = this.UpdateCD(mCheckCD);
            var _lastSCD = mSwallowCD;
            mSwallowCD = this.UpdateCD(mSwallowCD, GlobalData.slowDownFactor);

            if (_lastSCD > 0 && mSwallowCD <= 0)
            {
                var newVirus = EntityManager.Create<VirusSwallow>();
                newVirus.Reset(id, hp, size, mSwallowVirusSpeed, position, -direction, hpRange, false);
            }

            SetInvincible(mSwallowCD > 0);
            collider2D.enabled = mSwallowCD <= 0;
            speedMul = mSwallowCD > 0 ? 0 : 1f;
            hpText.gameObject.SetActive(!isInvincible);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!GameUtil.isInBattle)
                return;
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