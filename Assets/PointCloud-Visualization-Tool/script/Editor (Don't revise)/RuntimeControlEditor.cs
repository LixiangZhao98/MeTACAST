// using UnityEditor;
// using UnityEngine;
// using System;
// using System.Text.RegularExpressions;
//
// [CustomEditor(typeof(PointCloudRuntime))]
// public class RunTimeControlEditor : Editor
// {
//     String s = "";
//     PointCloudFileType pcf;
//
//     public override void OnInspectorGUI()
//     {
//         PointCloudRuntime runTimeControl = (PointCloudRuntime)target;
//
//         runTimeControl.custom = EditorGUILayout.Toggle("Use_Dataset_Defined_Yourself", runTimeControl.custom);
//         if (runTimeControl.custom)
//         {
//             EditorGUILayout.BeginHorizontal();
//             runTimeControl.customGenerator =
//                 (CustomGeneratorEnum)EditorGUILayout.EnumPopup("Customized Dataset", runTimeControl.customGenerator);
//             if (GUILayout.Button("Add customized dataset", GUILayout.Width(200)))
//             {
//                 OpenDataGeneratorScript();
//             }
//
//             EditorGUILayout.EndHorizontal();
//         }
//         else
//         {
//             runTimeControl.dataset = (Dataset)EditorGUILayout.EnumPopup("Dataset in Project", runTimeControl.dataset);
//         }
//
//         GUILayout.Space(20);
//         EditorGUILayout.BeginHorizontal();
//
//         s = EditorGUILayout.TextField("Save pointcloud as ", s);
//         pcf = (PointCloudFileType)EditorGUILayout.EnumPopup("", pcf, GUILayout.Width(100));
//         EditorGUILayout.EndHorizontal();
//         EditorGUILayout.BeginHorizontal();
//         if (GUILayout.Button("Save"))
//         {
//             if (s == "")
//             {
//                 if (pcf == PointCloudFileType.bin)
//                     runTimeControl.GetComponentInChildren<DataLoader>().SaveAsBin(Application.dataPath +
//                         "/PointCloud-Visualization-Tool/data/" + "/data/" + "CustomizedDataset.bin");
//                 if (pcf == PointCloudFileType.ply)
//                     runTimeControl.GetComponentInChildren<DataLoader>().SaveAsPly(Application.dataPath +
//                         "/PointCloud-Visualization-Tool/data/" + "/data/" + "CustomizedDataset.ply");
//                 if (pcf == PointCloudFileType.pcd)
//                     runTimeControl.GetComponentInChildren<DataLoader>().SaveAsPcd(Application.dataPath +
//                         "/PointCloud-Visualization-Tool/data/" + "/data/" + "CustomizedDataset.pcd");
//                 if (pcf == PointCloudFileType.txt)
//                     runTimeControl.GetComponentInChildren<DataLoader>().SaveAsTxt(Application.dataPath +
//                         "/PointCloud-Visualization-Tool/data/" + "/data/" + "CustomizedDataset.txt");
//             }
//             else
//             {
//                 string pattern = @"[^a-zA-Z0-9_]";
//                 s = Regex.Replace(s, pattern, "");
//
//                 if (pcf == PointCloudFileType.bin)
//                     runTimeControl.GetComponentInChildren<DataLoader>().SaveAsBin(Application.dataPath +
//                         "/PointCloud-Visualization-Tool/data/" + "/data/" + s + ".bin");
//                 if (pcf == PointCloudFileType.ply)
//                     runTimeControl.GetComponentInChildren<DataLoader>().SaveAsPly(Application.dataPath +
//                         "/PointCloud-Visualization-Tool/data/" + "/data/" + s + ".ply");
//                 if (pcf == PointCloudFileType.pcd)
//                     runTimeControl.GetComponentInChildren<DataLoader>().SaveAsPcd(Application.dataPath +
//                         "/PointCloud-Visualization-Tool/data/" + "/data/" + s + ".pcd");
//                 if (pcf == PointCloudFileType.txt)
//                     runTimeControl.GetComponentInChildren<DataLoader>().SaveAsTxt(Application.dataPath +
//                         "/PointCloud-Visualization-Tool/data/" + "/data/" + s + ".txt");
//             }
//         }
//
//         EditorGUILayout.EndHorizontal();
//     }
//
//     private void OpenDataGeneratorScript()
//     {
//         string scriptPath = "Assets/PointCloud-Visualization-Tool/script/dataprocessing/DataGenerator.cs";
//
//         EditorUtility.OpenWithDefaultApp(scriptPath);
//     }
// }




using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

[CustomEditor(typeof(PointCloudRuntime))]
public class RunTimeControlEditor : Editor
{
    string s1 = "";
    string s2 = "";
    string s3 = "";
    string s4 = "";
    
    PointCloudFileType pcf;

    public override void OnInspectorGUI()
    {
        PointCloudRuntime runTimeControl = (PointCloudRuntime)target;
        
        runTimeControl.custom = EditorGUILayout.Toggle("Use_Dataset_Defined_Yourself", runTimeControl.custom);
        if (runTimeControl.custom)
        {
            EditorGUILayout.BeginHorizontal();
            runTimeControl.customGenerator =
                (CustomGeneratorEnum)EditorGUILayout.EnumPopup("Customized Dataset", runTimeControl.customGenerator);
            if (GUILayout.Button("Add customized dataset", GUILayout.Width(200)))
            {
                OpenDataGeneratorScript();
            }

            EditorGUILayout.EndHorizontal();
        }
        else
        {
            runTimeControl.dataset = (Dataset)EditorGUILayout.EnumPopup("Dataset in Project", runTimeControl.dataset);
        }

        GUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();

        s1 = EditorGUILayout.TextField("Save pointcloud ", s1);
        pcf = (PointCloudFileType)EditorGUILayout.EnumPopup("", pcf, GUILayout.Width(100));

        if (GUILayout.Button("Save",GUILayout.Width(100)))
        {
            if (s1 == "")
            {
                if (pcf == PointCloudFileType.bin)
                    runTimeControl.GetComponentInChildren<DataLoader>().SaveAsBin(Application.dataPath +
                        "/PointCloud-Visualization-Tool/data/" + "/data/" + "CustomizedDataset.bin");
                if (pcf == PointCloudFileType.ply)
                    runTimeControl.GetComponentInChildren<DataLoader>().SaveAsPly(Application.dataPath +
                        "/PointCloud-Visualization-Tool/data/" + "/data/" + "CustomizedDataset.ply");
                if (pcf == PointCloudFileType.pcd)
                    runTimeControl.GetComponentInChildren<DataLoader>().SaveAsPcd(Application.dataPath +
                        "/PointCloud-Visualization-Tool/data/" + "/data/" + "CustomizedDataset.pcd");
                if (pcf == PointCloudFileType.txt)
                    runTimeControl.GetComponentInChildren<DataLoader>().SaveAsTxt(Application.dataPath +
                        "/PointCloud-Visualization-Tool/data/" + "/data/" + "CustomizedDataset.txt");
            }
            else
            {
                string pattern = @"[^a-zA-Z0-9_]";
                s1 = Regex.Replace(s1, pattern, "");

                if (pcf == PointCloudFileType.bin)
                    runTimeControl.GetComponentInChildren<DataLoader>().SaveAsBin(Application.dataPath +
                        "/PointCloud-Visualization-Tool/data/" + "/data/" + s1 + ".bin");
                if (pcf == PointCloudFileType.ply)
                    runTimeControl.GetComponentInChildren<DataLoader>().SaveAsPly(Application.dataPath +
                        "/PointCloud-Visualization-Tool/data/" + "/data/" + s1 + ".ply");
                if (pcf == PointCloudFileType.pcd)
                    runTimeControl.GetComponentInChildren<DataLoader>().SaveAsPcd(Application.dataPath +
                        "/PointCloud-Visualization-Tool/data/" + "/data/" + s1 + ".pcd");
                if (pcf == PointCloudFileType.txt)
                    runTimeControl.GetComponentInChildren<DataLoader>().SaveAsTxt(Application.dataPath +
                        "/PointCloud-Visualization-Tool/data/" + "/data/" + s1 + ".txt");
            }
        }
        EditorGUILayout.EndHorizontal();
        
        
        
        EditorGUILayout.BeginHorizontal();
        s4 = EditorGUILayout.TextField("Save target as new .bin data ", s4);

        if (GUILayout.Button("Save",GUILayout.Width(100)))
        {
            if (s4 == "")
            {
                runTimeControl.GetComponentInChildren<DataLoader>(). SaveTargetAsNewData("CustomizedTargetData");
            }
            else
            {
                string pattern = @"[^a-zA-Z0-9_]";
                s4 = Regex.Replace(s4, pattern, "");
                runTimeControl.GetComponentInChildren<DataLoader>(). SaveTargetAsNewData(s4);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        
        
        
        EditorGUILayout.BeginHorizontal();
        s2 = EditorGUILayout.TextField("Save selected flag file", s2);

        if (GUILayout.Button("Save",GUILayout.Width(100)))
        {
            if (s2 == "")
            {
                runTimeControl.GetComponentInChildren<DataLoader>(). SaveFlag("CustomizedFlag");
            }
            else
            {
                string pattern = @"[^a-zA-Z0-9_]";
                s2 = Regex.Replace(s2, pattern, "");
                runTimeControl.GetComponentInChildren<DataLoader>(). SaveFlag(s2);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        
        EditorGUILayout.BeginHorizontal();
        s3 = EditorGUILayout.TextField("Save selected as new .bin data ", s3);

        if (GUILayout.Button("Save",GUILayout.Width(100)))
        {
            if (s3 == "")
            {
                runTimeControl.GetComponentInChildren<DataLoader>(). SaveSelectedAsNewData("CustomizedSelectedData");
            }
            else
            {
                string pattern = @"[^a-zA-Z0-9_]";
                s3 = Regex.Replace(s3, pattern, "");
                runTimeControl.GetComponentInChildren<DataLoader>(). SaveSelectedAsNewData(s3);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        
        
        
        

    }

    private void OpenDataGeneratorScript()
    {
        string scriptPath = "Assets/PointCloud-Visualization-Tool/script/dataprocessing/DataGenerator.cs";

        EditorUtility.OpenWithDefaultApp(scriptPath);
    }
}