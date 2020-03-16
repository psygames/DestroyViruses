using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DestroyViruses
{
    public class InputView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private Aircraft aircraft { get { return Aircraft.ins; } }

        // private
        private Vector2 mTotalDrag = Vector2.zero;
        private float mHoldTime;
        private bool mIsDown { get { foreach (var d in mDowns) if (d.Value) return true; return false; } }


        // battle begin
        private float mDragBeginThreshold = 20f;
        private float mHoldBeginThreshold = 0.5f;

        private Dictionary<int, bool> mDowns = new Dictionary<int, bool>();

        private bool IsSystemNoTouches()
        {
            return Input.touchCount <= 0
                && !Input.GetMouseButton(0)
                && !Input.GetMouseButton(1)
                && !Input.GetMouseButton(2)
                ;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (aircraft == null)
                return;

            if (!mIsDown)
            {
                mIsTriggerBattleStateOnDown = false;
            }

            mTotalDrag = Vector2.zero;
            mDowns[eventData.pointerId] = true;
            mHoldTime = 0;

            if (GameUtil.isInHome)
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
            if (GameUtil.isInHome)
            {
                aircraft.anima.PlayStandby();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 delta = UIUtil.FormatToVirtual(eventData.delta);
            if (aircraft == null || !mIsDown)
                return;

            if (GameUtil.isInHome)
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
            else if (GameUtil.isInBattle)
            {
                BattleDrag(delta);
            }
        }

        private bool mLastBattleDown = false;
        private void Update()
        {
            if (aircraft == null)
                return;

            if (IsSystemNoTouches() && mDowns.Count > 0)
                mDowns.Clear();

            if (mIsDown)
                mHoldTime += Time.deltaTime;

            if (GameUtil.isInHome && mIsDown && mHoldTime >= mHoldBeginThreshold)
            {
                BattleStart();
            }

            if (GameUtil.isInBattle)
            {
                GlobalData.isBattleTouchOn = mIsDown && !GameUtil.isFrozen;
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

        private bool mIsTriggerBattleStateOnDown = false;
        private void BattleStart()
        {
            if (mIsTriggerBattleStateOnDown)
                return;
            mIsTriggerBattleStateOnDown = true;
            if (D.I.energy >= CT.table.energyBattleCost)
            {
                mLastBattleDown = false;
                StateManager.ChangeState<BattleState>();
            }
            else
            {
                Toast.Show(LTKey.ENERGY_NOT_ENOUGH.LT());
            }
        }

        private void BattleDrag(Vector2 delta)
        {
            InputManager.Instance.Enqueue(new InputData(InputType.Drag, delta));
        }
    }
}