using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using ExcelDataReader;
using System.Data;

public class ConfigToolEditor : Editor
{
    [MenuItem("Tools/Config/Genenrate Class")]
    static void GenConfigClass()
    {
        GenerateClassFile(@"D:\workspace\DestroyViruses\DestroyViruses\Assets\Configs\Aircraft.xlsx");
    }

    [MenuItem("Tools/Config/Genenrate Asset")]
    static void GenConfigAsset()
    {
        GenerateAssetFile(@"D:\workspace\DestroyViruses\DestroyViruses\Assets\Configs\Aircraft.xlsx");
    }

    class PropertyData
    {
        public string name;
        public string type;
        public string description;
    }

    private static void GenerateClassFile(string excelPath)
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

        File.WriteAllText($"Assets/Scripts/GameLogic/Config/{className}.cs", classTemplate);
    }

    private static void GenerateAssetFile(string excelPath)
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
        ScriptableObject.CreateInstance(Type.)
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

        File.WriteAllText($"Assets/Scripts/GameLogic/Config/{className}.cs", classTemplate);
    }

    private static List<string> GetExcelList()
    {
        return new List<string>() { };
    }
}
