//
//  RunTimeController.cs
//  MeTACAST
//
//  Copyright (c) 2022, 2023 Lixiang Zhao. All rights reserved.
//


using System;
using System.Collections.Generic;
using UnityEngine;
using static LixaingZhao.MeTACAST.Enum;


namespace LixaingZhao.MeTACAST{
public class RunTimeController : MonoBehaviour
{


[SerializeField, SetProperty("DATASET")]
    private Dataset dataset=Dataset.nbody2;

    public Dataset DATASET
        {
            get { return dataset; }
            set {
                     dataset=value;
                     SwitchDatasetFromFile(dataset.ToString());
                
                  
            }
        }


[SerializeField, SetProperty("SELECTIONTECH")]
   private SelectionTech selectionTech=SelectionTech.Point;
   public  SelectionTech SELECTIONTECH
    {
            get { return selectionTech; }
            set
            {   
                 selectionTech=value;
                 SwitchSelectionTech(selectionTech);

            }
    }

    private int gridNum=64;
    [SerializeField, SetProperty("GRIDNUM")]
    private GRIDNum gRIDNum;
    public GRIDNum GRIDNUM
    {
          get {return gRIDNum;}
            set
            {      
                gRIDNum=value;
        if (value == GRIDNum.grid100)
            {gridNum = 100;}
        if (value == GRIDNum.grid64)
            gridNum = 64;
        if (value == GRIDNum.grid200)
            gridNum = 200;
             SwitchDatasetFromFile(dataset.ToString());
                
            }
    }
public void SetGRIDNum(int g)
{
gridNum=g;
}
    public string LoadFlagName;
    public string StoreFlagName;
    public bool LoadFlag;
    public string StoreName;


    public ComputeShader kde_shader;

    private RenderDataRunTime RD;
    private MarchingCubeGPU MCgpu;
    private Selection sl;
 
    private List<string> dataset_generator = new List<string> { "random_sphere" };
    private void Start()
    {

        RD = this.gameObject.transform.parent.GetComponentInChildren<RenderDataRunTime>();
        MCgpu = this.gameObject.transform.parent.GetComponentInChildren<MarchingCubeGPU>();
        sl = this.gameObject.transform.parent.GetComponentInChildren<Selection>();
        SwitchDatasetFromFile(dataset.ToString());
        SwitchSelectionTech(selectionTech);
    }


    public void SwitchDatasetFromFile(string name)
    {
        DataMemory.StacksInitialize();
        if (!dataset_generator.Contains(name))
        {
            DataMemory.LoadDataByByte(name);
        }
        else
        {
            DataMemory.LoadDataByVec3s(DataGenerator.Generate(name), name);
        }
        DataMemory.CreateDensityField(gridNum);
        GPUKDECsHelper.StartGpuKDE(DataMemory.allParticle, DataMemory.densityField, kde_shader);
        if (LoadFlag)
            DataMemory.LoadFlagsToStack(LoadFlagName);
        RD.GenerateMesh();
        MCgpu.Init();
    }

    public void SwitchSelectionTech(SelectionTech s)
    {
        sl.Init(s);
        MCgpu.Init();
    }


    [ContextMenu("StoreFlags")]
    public void StoreFlages()
    {
        DataMemory.StoreFlags(StoreName);
    }
    

        [ContextMenu("SaveSelectedAsNewData")]
    public void SaveSelectedAsNewData()
    {
        DataMemory.SaveSelectedAsNewData(StoreName);
    }

            [ContextMenu("SaveTargetAsNewData")]
    public void SaveTargetAsNewData()
    {
        DataMemory.SaveTargetAsNewData(StoreName);
    }

                [ContextMenu("SaveDataAsNewData")]
    public void SaveDataAsNewData()
    {
        DataMemory.SaveDataAsNewData(StoreName);
    }



}



}