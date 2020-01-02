using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Text;

public class HotUpdateEditorWindow : EditorWindow
{
    [MenuItem("Tools/热更检测工具")]
    static void GenConfigClass()
    {
        var window = GetWindow(typeof(HotUpdateEditorWindow));
        window.titleContent = new GUIContent("热更检测工具");
    }

    private bool isRunning = false;
    private string checkRoot = "";
    private string tableRoot = "Assets/Tables/";
    private string triggerFileName = "删除此文件后，开始热更资源。";
    private string logFileName = "热更日志.txt";
    private string globalLog = "";

    private void OnEnable()
    {
        isRunning = false;
        EditorApplication.update -= EditorUpdate;
    }

    private string PathField(string title, string path)
    {
        if (!File.Exists(path) && !Directory.Exists(path))
        {
            EditorUtil.Label($"{title} 不存在：{path}", Color.red);
            return EditorUtil.Text(title, path);
        }
        else
        {
            EditorUtil.Label($"{title} 路径正确", Color.green);
            return EditorUtil.Text(title, path);
        }
    }

    private void OnGUI()
    {
        try
        {
            EditorGUILayout.Space();
            EditorUtil.Label("服务器自动热更检测程序");
            EditorGUILayout.Space();


            EditorUtil.Box("配置信息：", () =>
            {
                checkRoot = PathField("资源检测路径", checkRoot);
                tableRoot = PathField("表格文件路径", tableRoot);
            });

            EditorGUILayout.Space();

            EditorUtil.Box("日志：", () =>
            {
                EditorUtil.ScrollView(() =>
                {
                    EditorUtil.DisableGroup(() =>
                    {
                        EditorGUILayout.TextArea(globalLog);
                    });
                });
            });

            EditorGUILayout.Space();
            if (isRunning)
            {
                if (GUILayout.Button("停止"))
                {
                    isRunning = false;
                    EditorApplication.update -= EditorUpdate;
                }
            }
            else
            {
                if (GUILayout.Button("开始"))
                {
                    isRunning = true;
                    EditorApplication.update += EditorUpdate;
                }
            }
            EditorGUILayout.Space();
        }
        catch (Exception e)
        {
            Debug.LogError("生成失败: " + e.Message + "\n" + e.StackTrace);
            EditorUtility.ClearProgressBar();
        }
    }

    private void EditorUpdate()
    {
        if (!isRunning)
            return;

        var triggerFile = Path.Combine(checkRoot, triggerFileName);
        if (File.Exists(triggerFile))
            return;

        // backup
        var backupDir = Path.Combine(checkRoot, "backup", DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss_ms"));
        foreach (var f in Directory.GetFileSystemEntries(checkRoot))
        {
            if (Path.GetFileName(f) == "backup")
                continue;

            if (!Directory.Exists(backupDir))
                Directory.CreateDirectory(backupDir);

            FileUtil.MoveFileOrDirectory(f, Path.Combine(backupDir, Path.GetFileName(f)));
        }

        // table genes
        var window = GetWindow(typeof(TableToolEditorWindow)) as TableToolEditorWindow;
        window.titleContent = new GUIContent("配置表生成工具");
        window.GeneAllAssets();
        window.Close();

        // build asset bundle
        Plugins.XAsset.Editor.BuildScript.BuildManifest();
        Plugins.XAsset.Editor.BuildScript.BuildAssetBundles();

        // change log
        var logFile = Path.Combine(checkRoot, logFileName);
        var log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":\n";
        globalLog += log + "----------------------------\n";
        if (!File.Exists(logFile))
            File.Create(logFile).Close();
        File.WriteAllText(logFile, log, Encoding.UTF8);

        // add trigger
        if (!File.Exists(triggerFile))
            File.Create(triggerFile).Close();
    }
}
