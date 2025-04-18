//
//  DataMemory.cs
//  MeTACAST
//
//  Copyright (c) 2022, 2023 Lixiang Zhao. All rights reserved.
//
using System.Collections.Generic;
using UnityEngine;


public class DataMemory : MonoBehaviour
{

    #region Operation Stack and DisplayInfo

    private static Stack<List<int>> pStack;
    private static Stack<List<int>> pOperateStack;



    public static void StacksInitialize()
    {
        pStack = new Stack<List<int>>();
        pOperateStack=new Stack<List<int>>();

    }
    public static void LoadFlagsToStack(List<FlagNamesCollection> names)
    {
        foreach(var name in names)
        {
          for(int n=0;n<name.FlagNames.Length;n++)
        {
        int[] flags = LoadDataBybyte.StartLoadFlags(Application.dataPath + "/MeTACAST/data/flags/" + allParticle.name+"_"+ name.FlagNames[n]);
        for (int i = 0; i < flags.Length; i++)
        {

            DataMemory.allParticle.SetTarget(flags[i], true, names.IndexOf(name)); 
        }
        }
        }
            
    }

    public static void AddParticles(List<int> l)  // previous+new
    {
        if (pStack.Count == 0)
            pStack.Push(l);
        else
        {
            List<int> newl = new List<int>();
            newl.AddRange( pStack.Peek());
            newl.AddRange(l);
            pStack.Push(newl);
        }
    }

    public static void AddParticlesDirectly(List<int> l)  //only add new, previous is not considered
    {
        pStack.Push(l);
    }


    public static List<int> GetpStack()
    {
        if(pStack.Count>0)
        return pStack.Peek();
        else
            return new List<int>();
    }

    public static void Return() 
    {
        if (pStack.Count == 0)
            return;
          pOperateStack.Push(  pStack.Pop());
    }
    public static void Forward()
    {
        if (pOperateStack.Count == 0)
            return;
       pStack.Push(pOperateStack.Pop());
    }
    public static void ReleaseOperatorStack() 
    {

        pOperateStack = new Stack<List<int>>();
    }




    public static void DisplayAllParticle(bool loadFlag, List<FlagNamesCollection>  LoadFlagNames)
    {
        StacksInitialize();
            LoadFlagsToStack(LoadFlagNames);
        DisplayParticles.DisplayMesh(GameObject.Find("PointCloudMesh"), allParticle);
    }
    #endregion

    #region ParticleInfo
    [SerializeField]
    [HideInInspector] public List<Vector3> particleflow_dest;
    [SerializeField]
    static public ParticleGroup allParticle = new ParticleGroup();
    static public void LoadDataByPly(string loadFileName)
    {

        allParticle.LoadDatasetsByPly(Application.dataPath + "/PointCloud-Visualization-Tool/data/data/", loadFileName);

        Debug.Log("Load success" + " " + loadFileName + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength());

    }
    public static void LoadDataByByte(string loadFileName)
    {

        allParticle.LoadDatasetByByte(Application.dataPath + "/MeTACAST/data/data/" + loadFileName,loadFileName);
        Debug.Log("Load success" + " " + loadFileName + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength().x + " " + allParticle.GetSmoothLength().y + " " + allParticle.GetSmoothLength().z);
    }
    static public void LoadDataByCsv(string loadFileName)
    {

        allParticle.LoadDatasetByCsv(Application.dataPath + "/MeTACAST/data/data/" + loadFileName, loadFileName);
        Debug.Log("Load success" + " " + loadFileName + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength());
    }
   
    static public void LoadDataByVec3s(Vector3[] v, string dataname, bool forSimulation = false)
    {


        allParticle.LoadDatasetByVec3s(v, dataname, forSimulation);

        Debug.Log("Load success" + " " + dataname + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength());

    }

   
    public static void ClearParticleMemory()
    {
        allParticle = new ParticleGroup();
    }

    
    #endregion

    #region DensityFieldInfo
    [SerializeField]
  
    static public DensityField densityField = new DensityField();
    [SerializeField]
    
    static public DensityField densityField2 = new DensityField();

    static public void CreateDensityField(int gridNum)
    {

        float xstep = (allParticle.XMAX - allParticle.XMIN) / gridNum;
        float ystep = (allParticle.YMAX - allParticle.YMIN) / gridNum;
        float zstep = (allParticle.ZMAX - allParticle.ZMIN) / gridNum;
        allParticle.XMAX += xstep;
        allParticle.XMIN -= xstep;
        allParticle.YMAX += ystep;
        allParticle.YMIN -= ystep;
        allParticle.ZMAX += zstep;
        allParticle.ZMIN -= zstep;
        densityField.InitializeDensityFieldByGapDis(allParticle.name, allParticle.XMIN, allParticle.XMAX, gridNum, allParticle.YMIN, allParticle.YMAX, gridNum, allParticle.ZMIN, allParticle.ZMAX, gridNum);
        densityField2.InitializeDensityFieldByGapDis(allParticle.name, allParticle.XMIN, allParticle.XMAX, gridNum, allParticle.YMIN, allParticle.YMAX, gridNum, allParticle.ZMIN, allParticle.ZMAX, gridNum);
        Debug.Log("Create density field success");

    }
    public static void ClearDensityMemory()
    {
        densityField = new DensityField(); densityField2 = new DensityField();
    }
    #endregion

     public static void StoreFlags(string ExtendstoreFileName)
    {
        allParticle.StoreFlags(Application.dataPath + "/MeTACAST/data/" + "/Flags/" + allParticle.name+"_"+ ExtendstoreFileName);

    }

        public static void SaveSelectedAsNewData(string ExtendstoreFileName)
    {
        allParticle.SaveSelectedAsNewData(Application.dataPath + "/MeTACAST/data/" + "/data/" + allParticle.name+"_"+ ExtendstoreFileName);

    }

            public static void SaveTargetAsNewData(string ExtendstoreFileName)
    {
        allParticle.SaveTargetAsNewData(Application.dataPath + "/MeTACAST/data/" + "/data/" + allParticle.name+"_"+ ExtendstoreFileName);

    }
              public static void SaveDataAsNewData(string ExtendstoreFileName)
    {
        allParticle.SaveDataAsNewData(Application.dataPath + "/MeTACAST/data/" + "/data/" + allParticle.name+"_"+ ExtendstoreFileName);

    }

}
