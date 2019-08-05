using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using ExcelDataReader;
using System.Data;
using System;
using System.Reflection;

public class ConfigToolEditorWindow : EditorWindow
{
    [MenuItem("Tools/Config Genenrate")]
    static void GenConfigClass()
    {
        var window = GetWindow(typeof(ConfigToolEditorWindow));
        window.titleContent = new GUIContent("配置表生成工具");
    }


    private string EditorPrefsStringField(string prefName, string title, string defaultValue = "")
    {
        var prefStr = EditorPrefs.GetString(prefName, defaultValue);
        var str = (EditorGUILayout.TextField(title, prefStr));
        if (str != prefStr)
        {
            EditorPrefs.SetString(prefName, str);
        }
        return str;
    }

    Vector2 scrollPos = Vector2.zero;
    private void OnGUI()
    {
        try
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorPrefsStringField("ConfigExcelDir", "Excel目录");
            EditorPrefsStringField("ConfigClassOutputDir", "Class生成目录");
            EditorPrefsStringField("ConfigAssetOutputDir", "Asset生成目录");
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            if (GUILayout.Button("生成Class"))
            {
                GeneAllClasses();
            }
            if (GUILayout.Button("生成Asset"))
            {
                GeneAllAssets();
            }
            EditorGUILayout.Space();
        }
        catch (Exception e)
        {
            Debug.LogError("生成失败: " + e.Message + "\n" + e.StackTrace);
            EditorUtility.ClearProgressBar();
        }
    }

    private void GeneAllAssets()
    {
        var files = GetExcelList();
        int i = 0;
        foreach (var excelPath in files)
        {
            var excelName = Path.GetFileName(excelPath);
            bool cancel = EditorUtility.DisplayCancelableProgressBar($"生成Asset文件中...", excelName, 1f * i / files.Count);
            if (cancel)
            {
                EditorUtility.ClearProgressBar();
                break;
            }
            GenerateAssetFile(excelPath);
            i++;
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
        Debug.Log("生成Asset完成");
    }

    private void GeneAllClasses()
    {
        var files = GetExcelList();
        int i = 0;
        foreach (var excelPath in files)
        {
            var excelName = Path.GetFileName(excelPath);
            bool cancel = EditorUtility.DisplayCancelableProgressBar($"生成Class文件中...", excelName, 1f * i / files.Count);
            if (cancel)
            {
                EditorUtility.ClearProgressBar();
                break;
            }
            GenerateClassFile(excelPath);
            i++;
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
        Debug.Log("生成Class完成");
    }

    private void GenerateClassFile(string excelPath)
    {
        // 第一行属性名
        // 第二行属性类型
        // 第三行属性描述
        // 第四行以及之后数据航

        string classTemplate = File.ReadAllText(@"Assets/Editor/ConfigTool/ConfigClassTemplate.txt");
        string className = "Config" + Path.GetFileNameWithoutExtension(excelPath);
        string _namespace = "DestroyViruses";
        List<PropertyData> properties = new List<PropertyData>();
        int _headRowColumnCount = -1;

        using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                int _row = 0;
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    if (_row == 0)
                    {
                        _headRowColumnCount = row.ItemArray.Length;
                    }
                    else if (row.ItemArray.Length < _headRowColumnCount)
                    {
                        throw new System.Exception($"{excelPath} 第{_row}行列数小于首行。");
                    }
                    int _column = 0;
                    foreach (var item in row.ItemArray)
                    {
                        if (_column >= _headRowColumnCount)
                            break;

                        if (_row == 0)
                        {
                            var p = new PropertyData();
                            p.name = item.ToString();
                            properties.Add(p);
                        }
                        else if (_row == 1)
                        {
                            properties[_column].type = item.ToString();
                        }
                        else if (_row == 2)
                        {
                            properties[_column].description = item.ToString();
                        }
                        _column++;
                    }
                    _row++;
                }
            }
        }

        var _propertiesStr = "";
        var propertyTemplate = File.ReadAllText(@"Assets/Editor/ConfigTool/ConfigPropertyTemplate.txt");
        foreach (var p in properties)
        {
            var _tempStr = propertyTemplate;
            _tempStr = _tempStr.Replace("{name}", p.name);
            _tempStr = _tempStr.Replace("{type}", p.type);
            _tempStr = _tempStr.Replace("{description}", p.description);
            _propertiesStr += _tempStr;
        }

        classTemplate = classTemplate.Replace("{namespace}", _namespace);
        classTemplate = classTemplate.Replace("{className}", className);
        classTemplate = classTemplate.Replace("{properties}", _propertiesStr);

        var dir = EditorPrefs.GetString("ConfigClassOutputDir", "");
        File.WriteAllText(Path.Combine(dir, $"{className}Asset.cs"), classTemplate);
    }

    public object ParseData(string data, string type)
    {
        if (type == "int")
        {
            return int.Parse(data);
        }

        return data;
    }

    private void GenerateAssetFile(string excelPath)
    {
        // 第一行属性名
        // 第二行属性类型
        // 第三行属性描述
        // 第四行以及之后数据航

        string classTemplate = File.ReadAllText(@"Assets/Editor/ConfigTool/ConfigClassTemplate.txt");
        string className = "Config" + Path.GetFileNameWithoutExtension(excelPath);
        string assetClassName = className + "Asset";
        string _namespace = "DestroyViruses";
        Type classType = Assembly.Load("Assembly-CSharp").GetType(_namespace + "." + className);
        Type assetClassType = Assembly.Load("Assembly-CSharp").GetType(_namespace + "." + assetClassName);
        List<object> dataList = new List<object>();
        List<PropertyData> properties = new List<PropertyData>();
        int _headRowColumnCount = -1;
        using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                int _row = 0;
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    if (_row == 0)
                    {
                        _headRowColumnCount = row.ItemArray.Length;
                    }

                    int _column = 0;
                    object data = null;
                    if (_row >= 3)
                    {
                        data = Activator.CreateInstance(classType);
                    }
                    foreach (var item in row.ItemArray)
                    {
                        if (_column >= _headRowColumnCount)
                            break;

                        if (_row == 0)
                        {
                            var p = new PropertyData();
                            p.name = item.ToString();
                            properties.Add(p);
                        }
                        else if (_row == 1)
                        {
                            properties[_column].type = item.ToString();
                        }
                        else if (_row >= 3)
                        {
                            var cellData = ParseData(item.ToString(), properties[_column].type);
                            data.GetType().GetField(properties[_column].name).SetValue(data, cellData);
                        }
                        _column++;
                    }
                    if (_row >= 3)
                    {
                        dataList.Add(data);
                    }
                    _row++;
                }
            }
        }

        var so = CreateInstance(assetClassType);
        var array = Array.CreateInstance(classType, dataList.Count);
        for (int i = 0; i < dataList.Count; i++)
        {
            array.SetValue(dataList[i], i);
        }
        so.GetType().GetField("dataArray").SetValue(so, array);
        var dir = EditorPrefs.GetString("ConfigAssetOutputDir", "");
        AssetDatabase.CreateAsset(so, Path.Combine(dir, $"{className}.asset"));
        AssetDatabase.SaveAssets();
    }

    private static List<string> GetExcelList()
    {
        var files = Directory.GetFiles(EditorPrefs.GetString("ConfigExcelDir", ""), "*.xlsx", SearchOption.TopDirectoryOnly);
        return new List<string>(files);
    }


    class PropertyData
    {
        public string name;
        public string type;
        public string description;
    }
}
