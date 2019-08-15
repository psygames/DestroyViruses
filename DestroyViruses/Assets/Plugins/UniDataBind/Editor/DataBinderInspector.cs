using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniDataBind;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

namespace UniDataBindEditor
{
    [CustomEditor(typeof(DataBinder))]
    public class DataBinderInspector : Editor
    {
        private Assembly assembly;

        public override void OnInspectorGUI()
        {
            var binder = target as DataBinder;

            EditorGUILayout.Space();

            if (assembly == null)
                assembly = Assembly.Load("Assembly-CSharp");

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
            binder.bindName = Popup(typeNameArray, binder.bindName, aliasArray);
            var type = assembly.GetType(binder.bindName);
            if (type == null)
            {
                EditorGUILayout.LabelField($"无效数据类型: {binder.bindName}");
            }
            else
            {
                binder.bindPath = PathDraw(type, binder.bindPath);
            }

            EditorGUILayout.EndHorizontal();

            binder.format = EditorGUILayout.TextField("格式化", binder.format);

            EditorGUI.BeginDisabledGroup(true);
            binder.bindPath = EditorGUILayout.TextField(binder.bindPath);
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }

        private string PathDraw(Type type, string path)
        {
            string newPath = "";
            if (!string.IsNullOrEmpty(path))
            {
                var fields = path.Split('.').ToList();
                for (int i = 0; i < fields.Count; i++)
                {
                    string lst = fields[i];
                    fields[i] = Popup(GetFields(type), fields[i]);
                    type = GetFieldType(type, fields[i]);
                    if (type == null)
                        return path;
                    newPath += fields[i] + ".";
                    if (lst != fields[i] || type.IsPrimitive || type == typeof(string))
                    {
                        break;
                    }
                }
            }
            if (type != null && !type.IsPrimitive && type != typeof(string))
            {
                var end = Popup(GetFields(type), GetFields(type)[0]);
                type = GetFieldType(type, end);
                if (type != null)
                    newPath += end;
            }
            return newPath.TrimEnd('.');
        }

        private BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
        private string[] GetFields(Type type)
        {
            List<string> fields = new List<string>();
            fields.Add("--请选择--");
            foreach (var f in type.GetFields(bindingFlags))
            {
                fields.Add(f.Name);
            }
            foreach (var p in type.GetProperties(bindingFlags))
            {
                fields.Add(p.Name);
            }
            return fields.ToArray();
        }

        private Type GetFieldType(Type type, string fieldName)
        {
            if (type.GetProperty(fieldName, bindingFlags) != null)
                return type.GetProperty(fieldName, bindingFlags).PropertyType;
            if (type.GetField(fieldName, bindingFlags) != null)
                return type.GetField(fieldName, bindingFlags).FieldType;
            return null;
        }


        private string Popup(string[] array, string selected = "", string[] aliasArray = null)
        {
            var index = array.ToList().IndexOf(selected);
            index = index < 0 ? 0 : index;
            index = EditorGUILayout.Popup(index, aliasArray ?? array);
            return index >= 0 && index < array.Length ? array[index] : selected;
        }
    }
}
