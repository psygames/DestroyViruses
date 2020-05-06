using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class BuildProcess : MonoBehaviour
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget BuildTarget, string path)
    {
        if (BuildTarget == BuildTarget.iOS)
        {
            Debug.Log("XCodePostProcess: Starting to perform post build tasks for iOS platform.");
            XcodeProcess(path);
        }
    }

    private static void XcodeProcess(string path)
    {
        /*======== project ========*/
        string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string target = proj.ProjectGuid();

        // ENABLE_BITCODE=False
        proj.SetBuildProperty(target, "ENABLE_BITCODE", "false");

        // add extra framework(s)
        // proj.AddFrameworkToProject(target, "Security.framework", false);
        // proj.AddFrameworkToProject(target, "CoreTelephony.framework", true);
        // proj.AddFrameworkToProject(target, "libz.tbd", true);

        // rewrite to file
        File.WriteAllText(projPath, proj.WriteToString());


        /*======== Info.plist ========*/
        string plistPath = path + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));

        // Get root
        PlistElementDict rootDict = plist.root;

        /* ipad 关闭分屏 */
        // rootDict.SetBoolean("UIRequiresFullScreen", true);

        /* 设置Build值 */
        // var now = System.DateTime.Now;
        // string time = string.Format("{0}_{1}_{2} {3}:{4}:{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        // rootDict.SetString("CFBundleVersion", string.Format("{0}({1})", GlobalVars.VERSION, time));

        /* iOS9所有的app对外http协议默认要求改成https */
        // Add value of NSAppTransportSecurity in Xcode plist
        // var atsKey = "NSAppTransportSecurity";
        // PlistElementDict dictTmp = rootDict.CreateDict(atsKey);
        // dictTmp.SetBoolean("NSAllowsArbitraryLoads", true);

        /* Google AD */
        rootDict.SetString("GADApplicationIdentifier", "ca-app-pub-6997766984294588~7770804300");

        // location native development region 
        // rootDict.SetString("CFBundleDevelopmentRegion", "zh_CN");

        // for share sdk 截屏
        // rootDict.SetString("NSPhotoLibraryUsageDescription", "We need use photo library usage");

        // Write to file
        File.WriteAllText(plistPath, plist.WriteToString());
    }
}
