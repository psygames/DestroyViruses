using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "TableToolConfig", menuName = "Creat TableToolConfig")]
public class TableToolSettings : ScriptableObject
{
    [Header("Assembly 名字")]
    public string assemblyName = "Assembly-CSharp";

    [Header("命名空间")]
    public string _namespace = "Table";

    [Header("类模版 文件路径")]
    public string classTemplatePath = "Assets/Plugins/TableTool/Editor/TableClassTemplate.txt";
    [Header("属性模版 文件路径")]
    public string propertyTemplatePath = "Assets/Plugins/TableTool/Editor/TablePropertyTemplate.txt";
    [Header("字典属性模版 文件路径")]
    public string propertyDictionaryTemplatePath = "Assets/Plugins/TableTool/Editor/TablePropertyDictionaryTemplate.txt";
    [Header("内置类型 文件路径")]
    public string internalClassesPath = "Assets/Plugins/TableTool/Editor/TableInternalClasses.txt";
    [Header("Excel表格 目录路径")]
    public string excelFolderPath = "Assets/Tables/";

    [Header("生成数据类 目录路径")]
    public string generateClassFolderPath = "Assets/Scripts/Tables/";
    [Header("生成数据Asset 目录路径")]
    public string generateAssetFolderPath = "Assets/Resources/Tables/";

    [Header("是否开启数据加密")]
    public bool encrypt = true;
    [Header("数据加密KEY")]
    public string encryptKey = "";
}
