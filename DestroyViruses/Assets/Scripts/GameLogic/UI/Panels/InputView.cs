using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class InputView : MonoBehaviour
    {
        public UIEventListener inputListener;

        private bool isInHome { get { return StateManager.Instance.currentState.GetType() == typeof(MainState); } }
        private bool isInBattle { get { return StateManager.Instance.currentState.GetType() == typeof(BattleState); } }
        private Aircraft aircraft { get { return EntityManager.GetAll<Aircraft>().First() as Aircraft; } }

        // private
        private Vector2 mTotalDrag = Vector2.zero;
        private float mHoldTime;
        private bool mIsDown;


        // battle begin
        private float mDragBeginThreshold = 100;
        private float mHoldBeginThreshold = 1f;


        private void Awake()
        {
            inputListener.onDrag.AddListener(OnDrag);
            inputListener.onDown.AddListener(OnDown);
            inputListener.onUp.AddListener(OnUp);
        }

        private void OnDestroy()
        {
            inputListener.onDrag.RemoveAllListeners();
            inputListener.onDown.RemoveAllListeners();
            inputListener.onUp.RemoveAllListeners();
        }


        private void OnDown(Vector2 pos)
        {
            mTotalDrag = Vector2.zero;
            mIsDown = true;
            mHoldTime = 0;

            if (isInBattle)
            {
                BattleTouchDown(pos);
            }
            else if (isInHome && aircraft != null)
            {
                aircraft.anima.StopAll();
            }
        }

        private void OnUp(Vector2 pos)
        {
            mTotalDrag = Vector2.zero;
            mIsDown = false;
            mHoldTime = 0;

            if (isInBattle)
            {
                BattleTouchUp(pos);
            }
            else if (isInHome && aircraft != null)
            {
                aircraft.Reset();
                aircraft.anima.PlayStandby();
            }
        }

        private void OnDrag(Vector2 delta)
        {
            if (isInHome)
            {
                if (mTotalDrag.magnitude > mDragBeginThreshold)
                {
                    BattleStart();
                }
                if (aircraft != null)
                {
                    aircraft.rectTransform.anchoredPosition += UIUtil.FormatToVirtual(delta);
                }
            }
            else if (isInBattle)
            {
                BattleDrag(delta);
            }

            mTotalDrag += UIUtil.FormatToVirtual(delta);
        }

        private void Update()
        {
            if (mIsDown)
                mHoldTime += Time.deltaTime;

            if (isInHome && mIsDown && mHoldTime >= mHoldBeginThreshold)
            {
                BattleStart();
            }

            if (isInBattle)
            {
                GlobalData.isBattleTouchOn = mIsDown;
            }
        }

        private void BattleStart()
        {
            StateManager.ChangeState<BattleState>();

            InputManager.Instance.Push(new InputData(InputType.Down, UIUtil.FormatToVirtual(Vector2.zero)));
        }

        private void BattleDrag(Vector2 delta)
        {
            InputManager.Instance.Push(new InputData(InputType.Drag, UIUtil.FormatToVirtual(delta)));
        }

        private void BattleTouchDown(Vector2 pos)
        {

            InputManager.Instance.Push(new InputData(InputType.Down, UIUtil.FormatToVirtual(pos)));
        }

        private void BattleTouchUp(Vector2 pos)
        {
            InputManager.Instance.Push(new InputData(InputType.Up, UIUtil.FormatToVirtual(pos)));
        }
    }
}