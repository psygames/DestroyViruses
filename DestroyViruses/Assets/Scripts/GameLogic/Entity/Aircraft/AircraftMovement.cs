using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public class AircraftMovement : MonoBehaviour
    {
        private RectTransform headRoot;

        private RectTransform mRectTransform;
        private Vector2 mTargetDelta = Vector2.zero;

        public float baseMoveSpeed { get; set; } = 100000;
        public float moveSpeedRatio { get; set; } = 1;
        public Vector2 position { get { return mRectTransform.anchoredPosition; } }
        public Vector2 headPosition { get { return mRectTransform.anchoredPosition + headRoot.anchoredPosition; } }

        private void Awake()
        {
            mRectTransform = GetComponent<RectTransform>();

            if (headRoot == null)
                headRoot = transform.Find("headRoot")?.GetComponent<RectTransform>();
            if (headRoot == null)
                Debug.LogError("headRoot is null, please take care of this!");
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