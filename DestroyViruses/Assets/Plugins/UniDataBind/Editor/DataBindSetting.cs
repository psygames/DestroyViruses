using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UniDataBindEditor
{
    //[CreateAssetMenu(fileName = "DataBinTypes", menuName = "Asset/DataBindSetting")]
    [System.Serializable]
    public class DataBindSetting : ScriptableObject
    {
        public DataBindType[] bindTypes;

        [System.Serializable]
        public struct DataBindType
        {
            public string name;
            public string typeName;
            public string alias;
        }

        private static DataBindSetting Load()
        {
            return Resources.Load<DataBindSetting>("DataBindSetting");
        }

        private static DataBindSetting sIns = null;
        public static DataBindSetting Ins
        {
            get
            {
                if (sIns == null)
                    sIns = Load();
                return sIns;
            }
        }
    }
}

