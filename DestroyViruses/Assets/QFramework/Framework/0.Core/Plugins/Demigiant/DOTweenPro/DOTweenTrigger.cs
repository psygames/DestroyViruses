// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/03/12 15:55

using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening
{
    public class DOTweenTrigger : MonoBehaviour
    {
        public bool onEnableTrigger = true;
        public bool getCompsAlways = false;

        private DOTweenAnimation[] animations;

        private void Awake()
        {
            if (!getCompsAlways)
                animations = GetComponentsInChildren<DOTweenAnimation>();
        }

        private void GetAnimations()
        {
            if (!getCompsAlways)
                return;
            animations = GetComponentsInChildren<DOTweenAnimation>();
        }

        public void PlayAll()
        {
            GetAnimations();
            if (animations == null)
                return;
            foreach (var ani in animations)
            {
                ani.DORestart();
            }
        }

        private void OnEnable()
        {
            if (onEnableTrigger)
            {
                PlayAll();
            }
        }
    }
}
