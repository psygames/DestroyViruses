using GameFramework;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor.DataTableTools;

namespace DestroyViruses.Editor
{
    public sealed class DataTableGeneratorMenu
    {
        [MenuItem("Destroy Viruses/Generate DataTables")]
        private static void GenerateDataTables()
        {
            foreach (string dataTableName in ProcedurePreload.DataTableNames)
            {
                if (!GenerateDataTable(dataTableName))
                    break;
            }


            AssetDatabase.Refresh();
        }

        private static bool GenerateDataTable(string dataTableName,bool generateCode = true)
        {
            DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
            if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
            {
                Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                return false;
            }

            DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
            DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            return true;
        }
    }
}
