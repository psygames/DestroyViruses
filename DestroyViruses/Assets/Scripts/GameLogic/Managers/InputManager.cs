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
        private Queue<InputData> mInputStack = new Queue<InputData>();

        public void Enqueue(InputData inputData)
        {
            mInputStack.Enqueue(inputData);
        }

        public InputData Peek()
        {
            return mInputStack.Peek();
        }

        public bool IsEmpty()
        {
            return mInputStack.Count <= 0;
        }

        public InputData Dequeue()
        {
            return mInputStack.Dequeue();
        }

        public void Clear()
        {
            mInputStack.Clear();
        }
    }
}