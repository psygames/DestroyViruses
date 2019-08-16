using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniDataBind;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine.UI;

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

            int _index = nameArray.ToList().IndexOf(binder.bindName);
            var bindType = _index < 0 ? "" : typeNameArray[_index];

            EditorUtil.DisableGroup(() =>
            {
                if (string.IsNullOrEmpty(bindType))
                {
                    EditorUtil.Label("无效绑定类型", Color.red);
                }
                else
                {
                    var type = assembly.GetType(bindType);
                    if (ValidatePath(type, binder.bindPath))
                    {
                        EditorUtil.Label("有效绑定路径", binder.bindPath, Color.green);
                    }
                    else
                    {
                        EditorUtil.Label("无效绑定路径", binder.bindPath, Color.red);
                    }
                }
            });

            EditorUtil.Horizontal(() =>
            {
                binder.bindName = EditorUtil.Popup(binder.bindName, nameArray, aliasArray);
                _index = nameArray.ToList().IndexOf(binder.bindName);
                bindType = _index < 0 ? "" : typeNameArray[_index];
                var type = assembly.GetType(bindType);
                if (type == null)
                {
                    EditorUtil.Label($"无效数据类型: {bindType}", Color.red);
                }
                else
                {
                    binder.bindPath = PathDraw(type, binder.bindPath);
                }
            });

            binder.validateType = (DataBinder.ValidateType)EditorUtil.Enum("数据更新方式", binder.validateType);
            binder.bindType = (DataBinder.BindType)EditorUtil.Enum("绑定至", binder.bindType);
            if (binder.bindType == DataBinder.BindType.Text)
            {
                if (binder.GetComponent<Text>() == null)
                {
                    EditorUtil.Label($"没找到绑定的脚本: {binder.bindType}", Color.red);
                }
                else
                {
                    binder.format = EditorUtil.Text("格式化字符串", binder.format);
                }
            }
            else if (binder.bindType == DataBinder.BindType.Image)
            {
                if (binder.GetComponent<Image>() == null)
                {
                    EditorUtil.Label($"没找到绑定的脚本: {binder.bindType}", Color.red);
                }
                else
                {
                    binder.format = EditorUtil.Text("格式化字符串", binder.format);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private bool ValidatePath(Type type, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var fields = path.Split('.').ToList();
                for (int i = 0; i < fields.Count; i++)
                {
                    type = GetFieldType(type, fields[i]);
                    if (type == null)
                        return false;
                }
                if (type.IsPrimitive || type == typeof(string))
                {
                    return true;
                }
            }
            return false;
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
                    fields[i] = EditorUtil.Popup(fields[i], GetFields(type));
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
                var end = EditorUtil.Popup(GetFields(type)[0], GetFields(type));
                type = GetFieldType(type, end);
                if (type != null)
                    newPath += end;
            }
            return newPath.TrimEnd('.');
        }

        private bool ValidateField(Type type, string fieldName)
        {
            if (typeof(Component).IsAssignableFrom(type) || typeof(GameObject).IsAssignableFrom(type))
            {
                List<string> except = new List<string>()
                {
                    "runInEditMode", "rigidbody", "camera", "light", "animation", "constantForce",
                    "renderer", "audio", "guiText", "networkView", "guiElement", "guiTexture",
                    "collider", "collider2D", "hingeJoint", "particleSystem", "hideFlags", "rigidbody2D",
                    "scene", "active", "useGUILayout",
                };
                return !except.Contains(fieldName);
            }
            return true;
        }

        private string[] GetFields(Type type)
        {
            List<string> fields = new List<string>();
            fields.Add("--请选择--");
            foreach (var f in type.GetFields(bindingFlags))
            {
                if (ValidateField(type, f.Name))
                    fields.Add(f.Name);
            }
            foreach (var p in type.GetProperties(bindingFlags))
            {
                if (ValidateField(type, p.Name))
                    fields.Add(p.Name);
            }
            return fields.ToArray();
        }

        private BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
        private Type GetFieldType(Type type, string fieldName)
        {
            if (type.GetProperty(fieldName, bindingFlags) != null)
                return type.GetProperty(fieldName, bindingFlags).PropertyType;
            if (type.GetField(fieldName, bindingFlags) != null)
                return type.GetField(fieldName, bindingFlags).FieldType;
            return null;
        }
    }
}
