using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DestroyViruses
{
    public class InputView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private bool isInHome { get { return StateManager.Instance.currentState.GetType() == typeof(MainState); } }
        private bool isInBattle { get { return StateManager.Instance.currentState.GetType() == typeof(BattleState); } }
        private Aircraft aircraft { get { return Aircraft.ins; } }

        // private
        private Vector2 mTotalDrag = Vector2.zero;
        private float mHoldTime;
        private bool mIsDown { get { foreach (var d in mDowns) if (d.Value) return true; return false; } }


        // battle begin
        private float mDragBeginThreshold = 20f;
        private float mHoldBeginThreshold = 0.5f;

        private Dictionary<int, bool> mDowns = new Dictionary<int, bool>();


        public void OnPointerDown(PointerEventData eventData)
        {
            if (aircraft == null)
                return;

            mTotalDrag = Vector2.zero;
            mDowns[eventData.pointerId] = true;
            mHoldTime = 0;

            if (isInHome)
            {
                aircraft.anima.StopStandBy();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (aircraft == null)
                return;

            mTotalDrag = Vector2.zero;
            mDowns[eventData.pointerId] = false;
            mHoldTime = 0;
            if (isInHome)
            {
                aircraft.anima.PlayStandby();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 delta = UIUtil.FormatToVirtual(eventData.delta);
            if (aircraft == null || !mIsDown)
                return;

            if (isInHome)
            {
                if (mTotalDrag.magnitude > mDragBeginThreshold)
                {
                    BattleStart();
                }
                if (aircraft != null)
                {
                    aircraft.rectTransform.anchoredPosition += delta;
                }
                mTotalDrag += delta;
            }
            else if (isInBattle)
            {
                BattleDrag(delta);
            }
        }

        private bool mLastBattleDown = false;
        private void Update()
        {
            if (aircraft == null)
                return;

            if (mIsDown)
                mHoldTime += Time.deltaTime;

            if (isInHome && mIsDown && mHoldTime >= mHoldBeginThreshold)
            {
                BattleStart();
            }

            if (isInBattle)
            {
                GlobalData.isBattleTouchOn = mIsDown;
                if (mLastBattleDown != mIsDown)
                {
                    if (mIsDown)
                    {
                        InputManager.Instance.Enqueue(new InputData(InputType.Down, Vector2.zero));
                    }
                    else
                    {
                        InputManager.Instance.Enqueue(new InputData(InputType.Up, Vector2.zero));
                    }

                    mLastBattleDown = mIsDown;
                }
            }
        }

        private void BattleStart()
        {
            mLastBattleDown = false;
            StateManager.ChangeState<BattleState>();
        }

        private void BattleDrag(Vector2 delta)
        {
            InputManager.Instance.Enqueue(new InputData(InputType.Drag, delta));
        }
    }
}