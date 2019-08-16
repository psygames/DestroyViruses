using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniDataBind
{
    public class DataBindManager : Singleton<DataBindManager>
    {
        [Serializable]
        public struct PrepareBindScript
        {
            public string name;
            public MonoBehaviour script;
        }

        public PrepareBindScript[] prepareBind;

        Dictionary<string, object> mDataDict = new Dictionary<string, object>();

        private void Awake()
        {
            foreach (var prepare in prepareBind)
            {
                AddData(prepare.name, prepare.script);
            }
        }

        public void AddData(string name, object data)
        {
            if (!mDataDict.ContainsKey(name))
            {
                mDataDict.Add(name, data);
            }

        }

        public void RemoveData(string name)
        {
            if (mDataDict.ContainsKey(name))
            {
                mDataDict.Remove(name);
            }
        }

        public void Clear()
        {
            mDataDict.Clear();
        }

        public object Get(string name)
        {
            if (mDataDict.ContainsKey(name))
            {
                return mDataDict[name];
            }
            return null;
        }
    }
}