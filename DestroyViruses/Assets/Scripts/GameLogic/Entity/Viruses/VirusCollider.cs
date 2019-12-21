using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusCollider : VirusBase
    {
        protected override void OnColorChanged(int index)
        {
            base.OnColorChanged(index);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == TagUtil.Virus)
            {
                var virus = collision.GetComponent<VirusBase>();
                if (virus != null && virus.isAlive && !virus.isInvincible)
                {
                    Vector2 cDir = virus.position - position;
                    Vector2 dir = (cDir + virus.direction).normalized;
                    virus.SetDirection(dir);
                }
            }
        }
    }
}