using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Text;

public class QuickHotUpdateTablesEditorWindow : EditorWindow
{
    [MenuItem("Tools/表格快速热更工具")]
    static void GenConfigClass()
    {
        var window = GetWindow(typeof(QuickHotUpdateTablesEditorWindow));
        window.titleContent = new GUIContent("表格快速热更工具");
    }

    private bool isRunning = false;
    private string checkRoot = "";
    private string tableRoot = "Assets/Tables/";
    private string tableOutRoot = "Assets/AssetBundles/Tables/";
    private string quickHotUpdateRoot = "QuickHotUpdate";
    private string triggerFileName = "ready";
    private string logFileName = "finish";
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
                tableOutRoot = PathField("生成后表格路径", tableOutRoot);
                quickHotUpdateRoot = PathField("热更新文件路径", quickHotUpdateRoot);
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
        if (!File.Exists(triggerFile))
            return;

        File.Delete(triggerFile);

        // change log
        var logFile = Path.Combine(checkRoot, logFileName);
        var log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":\n";

        // process
        try
        {
            // check change
            var changeList = new List<string>();

            foreach (var f in Directory.GetFiles(checkRoot))
            {
                var fileName = Path.GetFileName(f);
                var ext = Path.GetExtension(f);
                if (ext == ".xls" || ext == ".xlsx")
                {
                    var dest = Path.Combine(tableRoot, fileName);
                    if (File.Exists(dest))
                    {
                        File.Delete(dest);
                        File.Copy(f, dest);
                        changeList.Add(fileName);
                    }
                }
            }

            // backup
            var backupDir = Path.Combine(checkRoot, "backup", DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss_ms"));
            foreach (var f in changeList)
            {
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);
                FileUtil.MoveFileOrDirectory(Path.Combine(checkRoot, f), Path.Combine(backupDir, f));
            }

            // change process
            if (changeList.Count > 0)
            {
                // table generate
                if (changeList.Any(a => a.Contains(".xls")))
                {
                    var window = GetWindow(typeof(TableToolEditorWindow)) as TableToolEditorWindow;
                    window.titleContent = new GUIContent("配置表生成工具");
                    window.GeneAllAssets();
                    var errMsg = window.GenAssetsErrorMessage;
                    window.Close();

                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        log += "ERRPR:\n" + errMsg;
                    }

                    if (!Directory.Exists(quickHotUpdateRoot))
                        Directory.CreateDirectory(quickHotUpdateRoot);
                    foreach (var fi in changeList)
                    {
                        var fn = Path.GetFileNameWithoutExtension(fi) + ".bytes";
                        var src = Path.Combine(tableOutRoot, fn);
                        var dest = Path.Combine(quickHotUpdateRoot, fn);
                        if (File.Exists(dest))
                            File.Delete(dest);
                        FileUtil.CopyFileOrDirectory(src, dest);
                    }
                }

                log += "Changes:\n";
                foreach (var c in changeList)
                {
                    log += c + "\n";
                }
            }
        }
        catch (Exception e)
        {
            log += "Error:\n" + e.Message + "\n";
        }
        finally
        {
            globalLog += log + "----------------------------\n";
            if (!File.Exists(logFile))
                File.Create(logFile).Close();
            File.WriteAllText(logFile, log, Encoding.UTF8);
        }
    }
}
