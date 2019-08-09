using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public enum InputType
    {
        Down = 0,
        Up = 1,
        Drag = 2,
    }

    public struct InputData
    {
        public InputType type;
        public Vector2 value;

        public InputData(InputType type, Vector2 data)
        {
            this.type = type;
            this.value = data;
        }
    }

    public class InputManager : Singleton<InputManager>
    {
        private Stack<InputData> mInputStack = new Stack<InputData>();

        public void Push(InputData inputData)
        {
            mInputStack.Push(inputData);
        }

        public InputData Peek()
        {
            return mInputStack.Peek();
        }

        public bool IsEmpty()
        {
            return mInputStack.Count <= 0;
        }

        public InputData Pop()
        {
            return mInputStack.Pop();
        }

        public void Clear()
        {
            mInputStack.Clear();
        }
    }
}