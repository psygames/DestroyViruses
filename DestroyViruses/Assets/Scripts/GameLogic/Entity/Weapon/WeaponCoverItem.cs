using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnibusEvent;
using UniRx;
using System;

namespace DestroyViruses
{
    public class WeaponCoverItem : MonoBehaviour
    {
        public Action<int, VirusBase> onCollider;
        public Vector2 position => rectTransform.GetUIPos();

        private new Collider2D collider2D;
        private RectTransform rectTransform;
        private bool isReady = false;
        private int index = -1;

        private void Awake()
        {
            collider2D = GetComponent<Collider2D>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetData(int index, float area)
        {
            this.index = index;
            float angle = (index % 9) / 9f * 360;
            float dist = (index / 9) * 20 + area * 0.5f;
            rectTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rectTransform.anchoredPosition = rectTransform.localRotation * Vector3.up * dist;
        }

        public void SetFill(float fill)
        {
            rectTransform.localScale = new Vector3(fill, fill, 1);
        }

        public void SetReady(bool isReady)
        {
            this.isReady = isReady;
            collider2D.enabled = isReady;
        }

        private void OnTriggerEnter2D(Collider2D _collider)
        {
            if (!isReady)
                return;

            if (_collider.tag == TagUtil.Virus)
            {
                var virus = _collider.GetComponent<VirusBase>();
                if (virus != null && virus.isAlive)
                {
                    onCollider?.Invoke(index, virus);
                }
            }
        }

    }
}
