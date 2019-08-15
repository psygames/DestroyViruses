using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniDataBind;
using UnityEditor;
using System;
using System.Linq;

namespace UniDataBindEditor
{
    [CustomEditor(typeof(DataBinder))]
    public class DataBinderInspector : Editor
    {
        private int mSelectedIndex = 0;

        public override void OnInspectorGUI()
        {
            var binder = target as DataBinder;

            EditorGUILayout.Space();

            string[] nameArray = new string[DataBindSetting.Ins.bindTypes.Length];
            string[] typeNameArray = new string[DataBindSetting.Ins.bindTypes.Length];
            string[] aliasArray = new string[DataBindSetting.Ins.bindTypes.Length];
            for (int i = 0; i < DataBindSetting.Ins.bindTypes.Length; i++)
            {
                var bind = DataBindSetting.Ins.bindTypes[i];
                nameArray[i] = bind.name;
                typeNameArray[i] = bind.typeName;
                aliasArray[i] = bind.alias;
            }

            EditorGUILayout.BeginHorizontal();
            binder.bindName = Popup(aliasArray, binder.bindName);
            var type = Type.GetType(typeNameArray[mSelectedIndex]);
            if (type == null)
            {
                EditorGUILayout.LabelField($"无效数据类型: {typeNameArray[mSelectedIndex]}");
            }
            else
            {
                PathDraw(type, binder.bindPath, binder.format);
            }

            EditorGUILayout.EndHorizontal();

            binder.format = EditorGUILayout.TextField("格式化", binder.format);
            serializedObject.ApplyModifiedProperties();
        }

        private void PathDraw(Type type, string path, string format)
        {
            var fields = path.Split('.');
            foreach (var field in fields)
            {
                type = GetFieldType(type, field);
                if (type.IsPrimitive || type == typeof(string))
                {
                    if (string.IsNullOrEmpty())
                }
            }
        }

        private Type GetFieldType(Type type, string fieldName)
        {

        }


        private string Popup(string[] array, string selected = "")
        {
            var index = array.ToList().IndexOf(selected);
            index = index < 0 ? 0 : index;
            index = EditorGUILayout.Popup(index, array);
            return index >= 0 && index < array.Length ? array[index] : selected;
        }
    }
}
