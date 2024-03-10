//
//  RunTimeController.cs
//  MeTACAST
//
//  Copyright (c) 2022, 2023 Lixiang Zhao. All rights reserved.
//


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EnumVariables;




public class RunTimeController : MonoBehaviour
{
    #region variables

    [SerializeField, SetProperty("DATASET")]
    protected EnumVariables.Dataset dataset;
    public EnumVariables.Dataset DATASET
    {
        get { return dataset; }
        set
        {
            dataset = value;
            if (Application.isPlaying)
                SwitchDatasetFromFile(dataset.ToString());

        }
    }
    protected List<string> dataset_generator = new List<string> { "random_sphere" };
    protected List<string> dataset_ply = new List<string> { "dragon_vrip" };

    public bool LoadFlag;
    [SerializeField]
    public List<FlagNamesCollection> LoadFlagNames;
    public string StoreFlagName;



    public bool CalculateDensity;
    public ComputeShader kde_shader;
    private int gridNum = 64;
    [SerializeField, SetProperty("GRIDNUM")]
    private EnumVariables.GRIDNum gRIDNum;
    public EnumVariables.GRIDNum GRIDNUM
    {
        get { return gRIDNum; }
        set
        {
            gRIDNum = value;

            if (value == GRIDNum.grid64)
                gridNum = 64;
            if (value == GRIDNum.grid128)
            { gridNum = 128; }
            if (value == GRIDNum.grid256)
                gridNum = 256;
            if (value == GRIDNum.grid512)
                gridNum = 512;
            if (Application.isPlaying)
                SwitchDatasetFromFile(dataset.ToString());

        }
    }
    public UnityAction myAction;
    public UnityEvent myEvent;
    public String storeName;
    #endregion

   

    private void Start()
    {
        SwitchDatasetFromFile(dataset.ToString());
    }


    public void SwitchDatasetFromFile(string name)
    {
        DataMemory.StacksInitialize();

        if (dataset_generator.Contains(name))
        {
            DataMemory.LoadDataByVec3s(DataGenerator.Generate(name), name);
        }
        else if (dataset_ply.Contains(name))
        {
            DataMemory.LoadDataByPly(name);
        }
        else
        {
            DataMemory.LoadDataByByte(name);
        }

        if (LoadFlagNames.Count != 0 && LoadFlag)
            DataMemory.LoadFlagsToStack(LoadFlagNames);

        if (CalculateDensity)
        {
            DataMemory.CreateDensityField(gridNum);
            GPUKDECsHelper.StartGpuKDE(DataMemory.allParticle, DataMemory.densityField, kde_shader);
            this.transform.parent.GetComponentInChildren<MarchingCubeGPU>().enabled = true;
            this.transform.parent.GetComponentInChildren<MarchingCubeGPU>().Init();
        }
        else
        {
            this.transform.parent.GetComponentInChildren<MarchingCubeGPU>().enabled = false;
        }
        RenderDataRunTime.GenerateMesh();
        myEvent?.Invoke();

    }
    public void SetGRIDNum(int g)
    {
        gridNum = g;
    }

    [ContextMenu("StoreFlags")]
    public void StoreFlages()
    {
        DataMemory.StoreFlags(StoreFlagName);
    }
    

        [ContextMenu("SaveSelectedAsNewData")]
    public void SaveSelectedAsNewData()
    {
        DataMemory.SaveSelectedAsNewData(storeName);
    }

            [ContextMenu("SaveTargetAsNewData")]
    public void SaveTargetAsNewData()
    {
        DataMemory.SaveTargetAsNewData(storeName);
    }

                [ContextMenu("SaveDataAsNewData")]
    public void SaveDataAsNewData()
    {
        DataMemory.SaveDataAsNewData(storeName);
    }



}



