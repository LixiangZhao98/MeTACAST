
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using System.Linq;
    
    [InitializeOnLoad]
    public static class DatasetEnumUpdater
    {
        private static string enumName = "Dataset";
        private static string dataPath = Application.dataPath+ "/PointCloud-Visualization-Tool/data/data";
        private static string enumFilePath = Application.dataPath+ "/PointCloud-Visualization-Tool/script/dataprocessing/"+enumName+".cs";
    
        static DatasetEnumUpdater()
        {
            EditorApplication.update += UpdateEnum;
        }
    
        private static void UpdateEnum()
        {
            if (string.IsNullOrEmpty(enumName))
            {
                Debug.LogError("enum name can not be null");
                return;
            }
    
            if (!Directory.Exists(dataPath))
            {
                Debug.LogWarning("No path:" + dataPath);
                return;
            }
    
            var files = Directory.GetFiles(dataPath)
                .Where(f => !f.EndsWith(".meta"))
                .Select(f => Path.GetFileNameWithoutExtension(f) + "_" + Path.GetExtension(f))
                .Distinct()
                .ToList();
    
            var currentEnumContent = File.Exists(enumFilePath) ? File.ReadAllText(enumFilePath) : string.Empty;
    
            var newEnumContent = GenerateEnumContent(files);
    
            if (currentEnumContent != newEnumContent)
            {
                File.WriteAllText(enumFilePath, newEnumContent);
                AssetDatabase.Refresh();
                Debug.Log(enumName + " updated");
            }
            
    
        }
    
        private static string GenerateEnumContent(System.Collections.Generic.List<string> values)
        {
            var enumContent = "public enum " + enumName + "\n{\n";
            enumContent += string.Join(",\n", values.Select(v => "    " + v.Replace(".", "_")));
            enumContent += "\n}\n";
            return enumContent;
        }
    }

