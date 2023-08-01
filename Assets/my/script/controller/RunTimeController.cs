using PavelKouril.MarchingCubesGPU;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RunTimeController : MonoBehaviour
{


    public Dataset dataset;
    private Dataset old_dataset;
    private List<string> dataset_generator = new List<string> { "random_sphere" };

    public SelectionTech selectionTech;
    private SelectionTech old_selectionTech;

    public GRIDNum gRIDNum;




    public string LoadFlagName;
    public string StoreFlagName;
    public bool LoadFlag;

    public ComputeShader kde_shader;

    private int gridNum;

    private RenderDataRunTime RD;
    private MarchingCubeGPU MCgpu;
    private Selection sl;

    private void Start()
    {

        RD = this.gameObject.transform.parent.GetComponentInChildren<RenderDataRunTime>();
        MCgpu = this.gameObject.transform.parent.GetComponentInChildren<MarchingCubeGPU>();
        sl = this.gameObject.transform.parent.GetComponentInChildren<Selection>();
        old_dataset = dataset;
        old_selectionTech = selectionTech;
        SwitchDatasetFromFile(dataset.ToString());
        SwitchSelectionTech(selectionTech);
    }
    private void Update()
    {
        if (old_dataset != dataset)
        {
            old_dataset = dataset;
            SwitchDatasetFromFile(dataset.ToString());

        }

        if (old_selectionTech != selectionTech)
        {
            old_selectionTech = selectionTech;
            SwitchSelectionTech(selectionTech);

        }


    }

    public void SwitchDatasetFromFile(string name)
    {
        GC.Collect();
        if (gRIDNum == GRIDNum.grid100)
            gridNum = 99;
        if (gRIDNum == GRIDNum.grid64)
            gridNum = 64;
        if (gRIDNum == GRIDNum.grid200)
            gridNum = 200;


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
        GC.Collect();
        sl.Init(s);
        MCgpu.Init();

    }


}
[Serializable]
public enum SelectionTech { Point, Brush, Paint, BaseLine };
[Serializable]
public enum Dataset { disk, uniform_Lines, ball_hemisphere, ununiform_Lines, Flocculentcube1, strings, Flocculentcube2, Flocculentcube3, galaxy, nbody1, nbody2, training_torus, random_sphere, three_rings, multiEllipsolds, fiveellipsolds, stringf, stringf1, snap_C02_200_127_animation };
[Serializable]
public enum GRIDNum { grid64, grid100, grid200 };


