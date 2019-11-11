using System.IO;
using UnityEditor;
using UnityEngine;

namespace ClearTools
{
    public static class ClearMenu
    {
        private const string clearPersistent = "Tools/清除 PersistentData";

        [MenuItem(clearPersistent)]
        private static void ClearPersistent()
        {
            if (Directory.Exists(Application.persistentDataPath))
            {
                Directory.Delete(Application.persistentDataPath, true);
            }
        }
    }
}