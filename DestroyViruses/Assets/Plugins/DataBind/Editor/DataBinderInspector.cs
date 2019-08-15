using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniDataBind;
using UnityEditor;
using System;

namespace UniDataBindEditor
{
    [CustomEditor(typeof(DataBinder))]
    public class DataBinderInspector : Editor
    {
        private int mSelectedIndex = 0;

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

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
                if (bind.name == binder.bindName)
                    mSelectedIndex = i;
            }

            EditorGUILayout.BeginHorizontal();
            mSelectedIndex = EditorGUILayout.Popup(mSelectedIndex, aliasArray);
            var type = Type.GetType(typeNameArray[mSelectedIndex]);
            if (type == null)
            {
                EditorGUILayout.LabelField($"无效数据类型: {typeNameArray[mSelectedIndex]}");
            }
            else
            {
                LoopDraw(type, binder.bindPath);
            }

            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }

        private void LoopDraw(Type type, string path)
        {

        }
    }
}
