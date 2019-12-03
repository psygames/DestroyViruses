using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using ExcelDataReader;
using System.Data;
using System;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

public class TableToolEditorWindow : EditorWindow
{
    [MenuItem("Tools/配置表生成工具")]
    static void GenConfigClass()
    {
        var window = GetWindow(typeof(TableToolEditorWindow));
        window.titleContent = new GUIContent("配置表生成工具");
    }


    private TableToolSettings settings = null;
    private Assembly assembly = null;
    private string classTemplate;
    private string propertyTemplate;
    private string propertyDictionaryTemplate;

    public string GenAssetsErrorMessage { get; private set; }

    private void OnEnable()
    {
        settings = AssetDatabase.LoadAssetAtPath<TableToolSettings>("Assets/Plugins/TableTool/Editor/TableToolSettings.asset");
        assembly = Assembly.Load(settings.assemblyName);
        classTemplate = File.ReadAllText(settings.classTemplatePath);
        propertyTemplate = File.ReadAllText(settings.propertyTemplatePath);
        propertyDictionaryTemplate = File.ReadAllText(settings.propertyDictionaryTemplatePath);
    }

    private void PathField(string _title, string path)
    {
        if (!File.Exists(path) && !Directory.Exists(path))
        {
            EditorUtil.Label($"{_title} 不存在：{path}", Color.red);
        }
        else
        {
            EditorUtil.Text(_title, path);
        }
    }

    private void OnGUI()
    {
        try
        {
            EditorGUILayout.Space();
            EditorUtil.Label("提示！！！ 若需要修改配置文件请转至 TableToolConfig.asset 中修改。");
            EditorGUILayout.Space();


            EditorUtil.Box("配置信息：", () =>
            {
                EditorUtil.DisableGroup(() =>
                {
                    EditorUtil.Text("Assembly 名字", settings.assemblyName);
                    EditorUtil.Text("命名空间", settings._namespace);
                    PathField("类模版 文件路径", settings.classTemplatePath);
                    PathField("属性模版 文件路径", settings.propertyTemplatePath);
                    PathField("字典类模版 文件路径", settings.propertyDictionaryTemplatePath);
                    PathField("内置类型 文件路径", settings.internalClassesPath);
                    PathField("Excel表格 目录路径", settings.excelFolderPath);
                    PathField("生成数据类 目录路径", settings.generateClassFolderPath);
                    PathField("生成数据文件 目录路径", settings.generateAssetFolderPath);
                    EditorUtil.Text("数据加密开关", settings.encrypt ? "打开" : "关闭");
                    EditorUtil.Text("数据加密Key", settings.encryptKey);
                });
            });

            EditorGUILayout.Space();

            EditorUtil.Box("全部表格：", () =>
            {
                if (!Directory.Exists(settings.excelFolderPath))
                {
                    EditorUtil.Label($"Excel表格路径不存在：{settings.excelFolderPath}", Color.red);
                }
                else
                {
                    EditorUtil.ScrollView(() =>
                    {
                        foreach (var fi in GetExcelList())
                        {
                            EditorUtil.Label("    " + Path.GetFileName(fi));
                        }
                    });
                }
            });

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

    public void GeneAllAssets()
    {
        GenAssetsErrorMessage = "";
        try
        {
            var files = GetExcelList();
            int i = 0;
            foreach (var excelPath in files)
            {
                var excelName = Path.GetFileName(excelPath);
                bool cancel = EditorUtility.DisplayCancelableProgressBar($"生成数据文件中...", excelName, 1f * i / files.Count);
                if (cancel)
                {
                    EditorUtility.ClearProgressBar();
                    break;
                }
                GenerateAssetFile(excelPath);
                i++;
            }
        }
        catch (Exception e)
        {
            GenAssetsErrorMessage += e.Message + "\n";
            Debug.LogError(e.Message + "\n" + e.StackTrace);
        }
        finally
        {
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }
        Debug.Log("生成数据文件完成");
    }

    public void GeneAllClasses()
    {
        var internalClasses = File.ReadAllText(settings.internalClassesPath);
        var icName = Path.GetFileNameWithoutExtension(settings.internalClassesPath);
        File.WriteAllText(Path.Combine(settings.generateClassFolderPath, $"{icName}.cs"), internalClasses);

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

    private List<List<string>> ReadTable(string excelPath)
    {
        var datas = new List<List<string>>();
        Action<int, int, string> add = (i, j, d) =>
        {
            if (datas.Count <= i)
                datas.Add(new List<string>());
            if (datas[i].Count <= j)
                datas[i].Add(d);
            else
                datas[i][j] = d;
        };
        using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var table = result.Tables[0];
                var vertical = table.Rows[0].ItemArray[0].ToString().EndsWith("(vertical)", StringComparison.OrdinalIgnoreCase);
                int _row = 0;
                int _col = 0;
                foreach (DataRow row in table.Rows)
                {
                    _col = 0;
                    foreach (var item in row.ItemArray)
                    {
                        var _val = item.ToString();
                        if (vertical)
                        {
                            if (_row == 0 && _col == 0)
                            {
                                _val = _val.Substring(0, _val.Length - "(vertical)".Length);
                            }
                            add(_col, _row, _val);
                        }
                        else
                        {
                            add(_row, _col, _val);
                        }
                        _col++;
                    }
                    _row++;
                }
            }
        }
        return datas;
    }

    private void GenerateClassFile(string excelPath)
    {
        // 第一行属性名
        // 第二行属性类型
        // 第三行属性描述
        // 第四行以及之后数据航

        string className = Path.GetFileNameWithoutExtension(excelPath);
        string _namespace = settings._namespace;
        List<PropertyData> properties = new List<PropertyData>();
        int _headRowColumnCount = -1;

        var datas = ReadTable(excelPath);
        int _row = 0;
        foreach (var row in datas)
        {
            if (_row == 0)
            {
                _headRowColumnCount = row.Count;
            }
            else if (row.Count < _headRowColumnCount)
            {
                throw new Exception($"{excelPath} 第{_row}行列数小于首行。");
            }
            if (_row > 3)
            {
                break;
            }
            int _column = 0;
            foreach (var item in row)
            {
                if (_column >= _headRowColumnCount)
                    break;
                switch (_row)
                {
                    case 0:
                        var p = new PropertyData
                        {
                            name = item.ToString()
                        };
                        properties.Add(p);
                        break;
                    case 1:
                        properties[_column].type = item.ToString();
                        break;
                    case 2:
                        properties[_column].description = item.ToString();
                        break;
                }

                _column++;
            }
            _row++;
        }

        var _classStr = classTemplate;
        var _idType = properties[0].type;
        var _propertiesStr = "";
        foreach (var p in properties)
        {
            if (IsIgnoreColumn(p))
                continue;

            var _tempStr = propertyTemplate;
            if (p.type.StartsWith("Dictionary", StringComparison.Ordinal))
            {
                _tempStr = propertyDictionaryTemplate;
                var kvType = GetDictionaryKVType(p.type);
                _tempStr = _tempStr.Replace("{keyType}", kvType.Item1);
                _tempStr = _tempStr.Replace("{valueType}", kvType.Item2);
            }

            _tempStr = _tempStr.Replace("{type}", p.type);
            _tempStr = _tempStr.Replace("{name}", p.name);
            if (p.description.Contains("\n"))
            {
                p.description = p.description.Replace("\r", "");
                var paraDesc = "";
                foreach (var para in p.description.Split('\n'))
                {
                    paraDesc += $"<para>{para}</para>";
                }
                p.description = paraDesc;
            }
            _tempStr = _tempStr.Replace("{description}", p.description);
            _propertiesStr += _tempStr;
        }

        _classStr = _classStr.Replace("{namespace}", _namespace);
        _classStr = _classStr.Replace("{className}", className);
        _classStr = _classStr.Replace("{idType}", _idType);
        _classStr = _classStr.Replace("{properties}", _propertiesStr);
        _classStr = _classStr.Replace("{encrypt}", settings.encrypt.ToString().ToLower());
        _classStr = _classStr.Replace("{encryptKey}", settings.encryptKey);
        _classStr = _classStr.Replace("{encryptIv}", className);

        var dir = settings.generateClassFolderPath;
        if (!Directory.Exists(settings.generateClassFolderPath))
        {
            Debug.LogError("未能找到Class生成目录路径：" + settings.generateClassFolderPath);
            return;
        }
        File.WriteAllText(Path.Combine(dir, $"{className}.cs"), _classStr);
    }

    private Type GetType(string stype)
    {
        switch (stype)
        {
            case "string":
                return typeof(string);
            case "int":
                return typeof(int);
            case "long":
                return typeof(long);
            case "float":
                return typeof(float);
            case "bool":
                return typeof(bool);
            case "short":
                return typeof(short);
            case "char":
                return typeof(char);
            case "double":
                return typeof(double);
            default:
                return assembly.GetType(stype);
        }
    }

    private Tuple<string, string> GetDictionaryKVType(string stype)
    {
        var _st = stype.IndexOf("<", StringComparison.Ordinal) + 1;
        var keyType = stype.Substring(_st, stype.IndexOf(",", StringComparison.Ordinal) - _st);
        _st = stype.IndexOf(",", StringComparison.Ordinal) + 1;
        var valueType = stype.Substring(_st, stype.IndexOf(">", StringComparison.Ordinal) - _st);
        return Tuple.Create(keyType, valueType);
    }

    private Type GetDictionaryType(string stype)
    {
        var kv = GetDictionaryKVType(stype);
        var kt = GetType(kv.Item1);
        var vt = GetType(kv.Item2);
        Type generic = typeof(Dictionary<,>);
        return generic.MakeGenericType(kt, vt);
    }

    private string TempCollectionText(string rawText)
    {
        int bracketsCount = 0;
        var chars = rawText.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '(')
            {
                bracketsCount++;
            }

            if (chars[i] == ')')
            {
                bracketsCount--;
            }

            if (bracketsCount > 0 && chars[i] == ',')
            {
                chars[i] = '_';
            }
        }
        var data = new string(chars);
        return data;
    }

    public object ParseData(string rawData, string stype)
    {
        if (stype == "string")
            return rawData;

        if (stype.EndsWith("[]", StringComparison.Ordinal))
        {
            var data = TempCollectionText(rawData);
            data = data.Trim('[', ']');
            var items = data.Split(',');
            int length = items[0] == "" ? 0 : items.Length;
            var itemType = stype.Substring(0, stype.Length - 2);
            Array array = Array.CreateInstance(GetType(itemType), length);
            if (array == null)
            {
                throw new Exception($"invalid array type : {stype}");
            }
            for (int i = 0; i < length; i++)
            {
                array.SetValue(ParseData(items[i].Replace("_", ","), itemType), i);
            }
            return array;
        }

        if (stype.StartsWith("Dictionary", StringComparison.Ordinal))
        {
            var data = TempCollectionText(rawData);
            data = data.Trim('[', ']');
            var items = data.Split(',');
            int length = items[0] == "" ? 0 : items.Length;
            var dictType = GetDictionaryType(stype);
            var kvType = GetDictionaryKVType(stype);
            var dict = Activator.CreateInstance(dictType);
            var addMethod = dictType.GetMethod("Add");
            for (int i = 0; i < length; i++)
            {
                var ii = items[i].Trim('(', ')').Split('_');
                var key = ParseData(ii[0], kvType.Item1);
                var value = ParseData(ii[1], kvType.Item2);
                addMethod.Invoke(dict, new object[] { key, value });
            }
            return dict;
        }

        // 自定义类型 or 基础类型
        var type = GetType(stype);
        if (type != null)
        {
            var parseMethod = type.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(string) }, null);
            if (parseMethod == null)
                throw new Exception($"类型 {stype} 不存在静态 Parse 方法");
            if (rawData == "")
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            return parseMethod.Invoke(null, new object[] { rawData });
        }

        throw new Exception($"invalid data {stype}: {rawData}");
    }

    private bool IsIgnoreColumn(PropertyData p)
    {
        return p.name == "" || p.type == "";
    }

    private void GenerateAssetFile(string excelPath)
    {
        // 第一行属性名
        // 第二行属性类型
        // 第三行属性描述
        // 第四行以及之后数据航

        string className = Path.GetFileNameWithoutExtension(excelPath);
        string collectionClassName = className + "Collection";
        string _namespace = settings._namespace;
        Type classType = GetType(_namespace + "." + className);
        Type collectionClassType = GetType(_namespace + "." + collectionClassName);
        List<object> dataList = new List<object>();
        List<PropertyData> properties = new List<PropertyData>();
        int _headRowColumnCount = -1;
        var datas = ReadTable(excelPath);
        int _row = 0;
        foreach (var row in datas)
        {
            if (_row == 0)
            {
                _headRowColumnCount = row.Count;
            }

            int _column = 0;
            object data = null;
            if (_row >= 3)
            {
                data = Activator.CreateInstance(classType);
            }
            foreach (var item in row)
            {
                if (_column >= _headRowColumnCount)
                    break;

                if (_row == 0)
                {
                    var p = new PropertyData
                    {
                        name = item
                    };
                    properties.Add(p);
                }
                else if (_row == 1)
                {
                    properties[_column].type = item;
                }
                else if (_row >= 3 && !IsIgnoreColumn(properties[_column]) && row[0] != "")
                {
                    try
                    {
                        var cellData = ParseData(item, properties[_column].type);
                        data.GetType().GetProperty(properties[_column].name).SetValue(data, cellData);
                    }
                    catch (Exception e)
                    {
                        var errMsg = $"{className} ParseData Error at cell {_row + 1}:{_column + 1}, type: {properties[_column].type}, value: {item.ToString()}\n{e.Message}\n{e.StackTrace}";
                        GenAssetsErrorMessage += errMsg + "\n";
                        Debug.LogError(errMsg);
                    }
                }
                _column++;
            }
            if (_row >= 3)
            {
                dataList.Add(data);
            }
            _row++;
        }

        var obj = assembly.CreateInstance(collectionClassType.FullName);
        var _idType = properties[0].type;
        var dict = Activator.CreateInstance(GetDictionaryType($"Dictionary<{_idType},{_namespace}.{className}>"));
        var flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        var keys = new List<object>();
        for (int i = 0; i < dataList.Count; i++)
        {
            object id = dataList[i].GetType().GetProperty("id", flag).GetValue(dataList[i]);
            if (string.IsNullOrEmpty(id.ToString()))
            {
                continue;
            }
            if (keys.Any(_ => _.ToString() == id.ToString()))
            {
                var errMsg = $"{className} Already has the same id {id}";
                GenAssetsErrorMessage += errMsg + "\n";
                Debug.LogError(errMsg);
                continue;
            }
            dict.GetType().GetMethod("Add", flag).Invoke(dict, new object[] { id, dataList[i] });
            keys.Add(id);
        }
        obj.GetType().GetField("mDict", flag).SetValue(obj, dict);
        var savePath = Path.Combine(settings.generateAssetFolderPath, $"{className}.bytes");
        SaveTableObj(obj, savePath, className);
    }

    private void SaveTableObj(object obj, string path, string className)
    {
        var stream = new MemoryStream();
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, obj);
        var bytes = stream.ToArray();
        stream.Close();
        if (settings.encrypt)
        {
            bytes = AesEncrypt(bytes, settings.encryptKey, className);
        }
        Stream fStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
        fStream.Write(bytes, 0, bytes.Length);
        fStream.Close();
    }

    private byte[] AesEncrypt(byte[] bytes, string key, string iv)
    {
        byte[] cryptograph = null;
        Rijndael Aes = Rijndael.Create();
        using (MemoryStream Memory = new MemoryStream())
        {
            var transform = Aes.CreateEncryptor(AesKey(key), AesKey(iv));
            using (CryptoStream Encryptor = new CryptoStream(Memory, transform, CryptoStreamMode.Write))
            {
                Encryptor.Write(bytes, 0, bytes.Length);
                Encryptor.FlushFinalBlock();
                cryptograph = Memory.ToArray();
            }
            transform.Dispose();
        }
        return cryptograph;
    }

    private byte[] AesKey(string key)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        for (int i = 0; i < bytes.Length; i++)
        {
            keyBytes[i % 16] = (byte)(keyBytes[i % 16] ^ bytes[i]);
        }
        return keyBytes;
    }

    private List<string> GetExcelList()
    {
        var files = Directory.GetFiles(settings.excelFolderPath)
            .Where(a => { return a.EndsWith(".xls", StringComparison.Ordinal) || a.EndsWith(".xlsx", StringComparison.Ordinal); })
            .ToList();
        return files;
    }


    class PropertyData
    {
        public string name;
        public string type;
        public string description;
    }
}
