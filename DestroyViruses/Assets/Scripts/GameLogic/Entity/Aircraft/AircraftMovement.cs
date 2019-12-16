using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public class AircraftMovement : MonoBehaviour
    {
        private RectTransform mRectTransform;
        private Vector2 mTargetDelta = Vector2.zero;

        public float baseMoveSpeed { get; set; } = 100000;
        public float moveSpeedRatio { get; set; } = 1;
        public Vector2 position { get { return mRectTransform.anchoredPosition; } }

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

            var moveSpeed = baseMoveSpeed;
            var scale = moveSpeedRatio * ProxyManager.GetProxy<BuffProxy>().Effect_MoveLimitation;
            if (!Mathf.Approximately(scale, 1f))
                moveSpeed = 1000 * scale;

            Vector2 climpDelta = Vector2.ClampMagnitude(mTargetDelta, moveSpeed * Time.deltaTime);
            mTargetDelta = Vector2.zero;
            var targetPos = mRectTransform.anchoredPosition + climpDelta;
            targetPos.x = Mathf.Clamp(targetPos.x, 0, UIUtil.width);
            targetPos.y = Mathf.Clamp(targetPos.y, 0, UIUtil.height);
            mRectTransform.anchoredPosition = targetPos;
        }

    }
}