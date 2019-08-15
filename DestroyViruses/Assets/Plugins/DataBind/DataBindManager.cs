using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniDataBind
{
    public class DataBindManager : Singleton<DataBindManager>
    {
        Dictionary<string, object> mDataDict = new Dictionary<string, object>();

        public void AddData(string name, object data)
        {
            if (!mDataDict.ContainsKey(name))
            {
                mDataDict.Add(name, data);
            }

        }

        public void RemoveData(string name)
        {

        }
    }
}