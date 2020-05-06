using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnibusEvent;
using System;

namespace DestroyViruses
{
    public class WeaponCoverItem : MonoBehaviour
    {
        public Action<int, VirusBase> onCollider;
        public Vector2 position => rectTransform.GetUIPos();
        public int shape { get; private set; }

        private new Collider2D collider2D;
        private RectTransform rectTransform;
        private bool isReady = false;
        private int index = -1;
        private float fill = 0;
        private float area = 0;

        private void Awake()
        {
            collider2D = GetComponent<Collider2D>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetData(int index, float area)
        {
            this.index = index;
            this.area = area;
        }

        public void SetFill(float fill)
        {
            this.fill = fill;
        }


        private Vector2 tarPos = Vector2.zero;
        private Quaternion tarRot = Quaternion.identity;
        public void SetShape(int shape)
        {
            this.shape = shape;
            var groupDist = 20f;

            if (shape == 0)
            {
                float angle = (index % 9) / 9f * 360;
                float dist = (index / 9) * groupDist + area * 0.5f;
                tarRot = Quaternion.AngleAxis(angle, Vector3.forward);
                tarPos = tarRot * Vector3.up * dist;
            }
            else
            {
                if (index < 8)
                {
                    float angle = (index % 4) / 3f * 100 - 50;
                    float dist = (index / 4) * groupDist + area * 0.5f;
                    tarRot = Quaternion.AngleAxis(angle, Vector3.forward);
                    tarPos = tarRot * Vector3.up * dist;
                }
                else
                {
                    float angle = ((index - 8) % 5) / 4f * 100 - 50;
                    float dist = ((index - 8) / 5) * groupDist + area * 0.75f;
                    tarRot = Quaternion.AngleAxis(angle, Vector3.forward);
                    tarPos = tarRot * Vector3.up * dist;
                }
            }
        }

        private void Update()
        {
            rectTransform.localScale = Vector3.one * fill * fill;
            rectTransform.localRotation = Quaternion.Slerp(rectTransform.localRotation, tarRot, Time.deltaTime * 15);
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, tarPos * Mathf.Sqrt(fill), Time.deltaTime * 15);
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
