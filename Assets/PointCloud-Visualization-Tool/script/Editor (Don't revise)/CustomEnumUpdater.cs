
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System;
    [InitializeOnLoad]
    public static class CustomEnumUpdater
    { 
        private static string enumName = "CustomGeneratorEnum";
        private static string enumFilePath = Application.dataPath+ "/PointCloud-Visualization-Tool/script/dataprocessing/"+enumName+".cs";
    
        static CustomEnumUpdater()
        {
            EditorApplication.update += UpdateCustomEnum;
        }
    
        private static void UpdateCustomEnum()
        {
            if (string.IsNullOrEmpty(enumName))
            {
                Debug.LogError("enum name can not be null");
                return;
            }
    
    
            var methodNames = GetMethodNamesFromFile();
            
            var currentEnumContent = File.Exists(enumFilePath) ? File.ReadAllText(enumFilePath) : string.Empty;
    
            var newEnumContent = GenerateEnumContent(methodNames);
    
    
            File.WriteAllText(enumFilePath, newEnumContent);
    
            
        } 
    
        private static string[] GetMethodNamesFromFile()
        {
            Type t = typeof(DataGenerator);
            MethodInfo[] mi= t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            
            string[] outString = new string[mi.Length];
            for (int i = 0; i < mi.Length; i++)
            {
                outString[i] = mi[i].Name;
            }
            return outString;
        }
    
        private static string GenerateEnumContent(string[] values)
        {
            var enumContent = "public enum " + enumName + "\n{\n";
            enumContent += string.Join(",\n", values.Select(v => "    " + v));
            enumContent += "\n}\n";
            return enumContent;
        }
    }

