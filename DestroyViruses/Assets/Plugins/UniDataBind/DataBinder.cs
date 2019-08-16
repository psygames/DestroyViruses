using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UniDataBind
{
    public class DataBinder : MonoBehaviour
    {
        [SerializeField]
        private string mBindName;
        [SerializeField]
        private string mBindPath;
        [SerializeField]
        private string mFormat;
        [SerializeField]
        private ValidateType mValidateType = ValidateType.LateUpdate;
        [SerializeField]
        private BindType mBindType = BindType.Text;

        public string bindName { get { return mBindName; } set { mBindName = value; } }
        public string bindPath { get { return mBindPath; } set { mBindPath = value; } }
        public string format { get { return mFormat; } set { mFormat = value; } }
        public ValidateType validateType { get { return mValidateType; } set { mValidateType = value; } }
        public BindType bindType { get { return mBindType; } set { mBindType = value; } }

        private void Start()
        {
            if (mValidateType == ValidateType.Start)
                Validate();
        }

        private void OnEnable()
        {
            if (mValidateType == ValidateType.OnEnable)
                Validate();
        }

        private void Update()
        {
            if (mValidateType == ValidateType.Update)
                Validate();
        }

        private void LateUpdate()
        {
            if (mValidateType == ValidateType.LateUpdate)
                Validate();
        }

        private void SetValue(object value)
        {
            if (!string.IsNullOrEmpty(format))
                GetComponent<Text>().text = string.Format(format, value);
            else
                GetComponent<Text>().text = value.ToString();
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(mBindName) || string.IsNullOrEmpty(mBindPath))
                return;
            var obj = DataBindManager.Instance.Get(mBindName);
            if (obj == null)
            {
                Debug.LogError($"绑定数据不存在！ {mBindName}");
                return;
            }

            Type type = obj.GetType();
            var fields = mBindPath.Split('.');
            for (int i = 0; i < fields.Length; i++)
            {
                GetFieldType(type, obj, fields[i], out type, out obj);
                if (type == null)
                {
                    Debug.LogError($"绑定路径错误！ {mBindPath}({fields[i]})");
                    return;
                }
                if (obj == null)
                {
                    Debug.LogError($"绑定数据为空！ {mBindPath}({fields[i]})");
                    return;
                }
            }

            if (!type.IsPrimitive && type != typeof(string))
            {
                Debug.LogError($"绑定数据类型错误！ {mBindPath}({fields[fields.Length - 1]})");
                return;
            }

            SetValue(obj);
        }

        private static BindingFlags sBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
        private void GetFieldType(Type type, object obj, string fieldName, out Type outType, out object outObj)
        {
            outType = null;
            outObj = null;

            var p = type.GetProperty(fieldName, sBindingFlags);
            if (p != null)
            {
                outType = p.PropertyType;
                outObj = p.GetValue(obj);
                return;
            }
            var f = type.GetField(fieldName, sBindingFlags);
            if (f != null)
            {
                outType = f.FieldType;
                outObj = f.GetValue(obj);
            }
        }

        public enum ValidateType
        {
            Start,
            OnEnable,
            Update,
            LateUpdate,
        }

        public enum BindType
        {
            Text,
            Image,
        }
    }
}