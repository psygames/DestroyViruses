using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public partial class AircraftMovement : MonoBehaviour
    {
        private RectTransform mRectTransform = null;
        public float moveSpeed = 10000;
        private Vector2 mTargetDelta = Vector2.zero;

        private void Awake()
        {
            mRectTransform = GetComponent<RectTransform>();
        }

        public void Move(Vector2 delta)
        {
            mTargetDelta += delta;
        }

        private void Update()
        {
            if (mTargetDelta == Vector2.zero)
                return;

            Vector2 climpDelta = Vector2.ClampMagnitude(mTargetDelta, moveSpeed * Time.deltaTime);
            mTargetDelta -= climpDelta;
            mRectTransform.anchoredPosition += climpDelta;
        }

    }
}