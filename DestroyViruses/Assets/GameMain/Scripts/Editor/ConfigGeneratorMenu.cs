using GameFramework;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor.DataTableTools;

namespace DestroyViruses.Editor
{
    public sealed class ConfigGeneratorMenu
    {
        private const string ConfigPath = "Assets/GameMain/Configs";

        [MenuItem("Destroy Viruses/Generate Config Bytes")]
        private static void GenerateDataTables()
        {
            GenerateConfigDataFile("DefaultConfig");
            AssetDatabase.Refresh();
        }


        private static bool GenerateConfigDataFile(string configName)
        {
            var filePath = Utility.Path.GetCombinePath(ConfigPath, configName + ".bytes");
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                using (BinaryWriter stream = new BinaryWriter(fileStream, Encoding.GetEncoding("GB2312")))
                {
                    int startPosition = (int)stream.BaseStream.Position;
                    stream.BaseStream.Position += sizeof(int);
                    int endPosition = (int)stream.BaseStream.Position;
                    int length = endPosition - startPosition - sizeof(int);
                    stream.BaseStream.Position = startPosition;
                    stream.Write(length);
                    stream.BaseStream.Position = endPosition;
                }
            }
            return true;
        }
    }
}
