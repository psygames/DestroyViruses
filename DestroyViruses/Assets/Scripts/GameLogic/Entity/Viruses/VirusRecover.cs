using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class VirusRecover : VirusBase
    {
        protected override void OnSkillTrigger()
        {
            base.OnSkillTrigger();

            SetHp(hp + table.effect1 * hpTotal);
            PlayCure();
        }
    }
}