using UnityEngine;
using System.Collections;
using System;

namespace DestroyViruses
{
    public class AircraftInputHandle : MonoBehaviour
    {
        private bool mHasFireAction;
        private bool mFireActionValue;
        private bool mHasMoveAction;
        private Vector2 moveActionValue = Vector2.zero;

        public Action<Vector2> onMove;
        public Action onFire;
        public Action onHoldFire;


        private void Update()
        {
            mHasFireAction = false;
            mFireActionValue = false;
            mHasMoveAction = false;
            moveActionValue = Vector2.zero;
            while (!InputManager.Instance.IsEmpty())
            {
                HandleInput(InputManager.Instance.Pop());
            }
        }

        private void HandleInput(InputData data)
        {
            if (data.type == InputType.Down)
            {
                mHasFireAction = true;
                mFireActionValue = true;
            }
            else if (data.type == InputType.Up)
            {
                mHasFireAction = true;
                mFireActionValue = false;
            }
            else if (data.type == InputType.Drag)
            {
                mHasMoveAction = true;
                moveActionValue += data.value;
            }

            if (mHasFireAction)
            {
                if (mFireActionValue) onFire?.Invoke();
                else onHoldFire?.Invoke();
            }
            if (mHasMoveAction)
            {
                onMove?.Invoke(moveActionValue);
            }
        }
    }
}