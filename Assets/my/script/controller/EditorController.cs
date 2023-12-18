//
//  EditorController.cs
//  MeTACAST
//
//  Copyright (c) 2022, 2023 Lixiang Zhao. All rights reserved.
//
using System.Collections.Generic;
using UnityEngine;

namespace LixaingZhao.MeTACAST{

public class EditorController : MonoBehaviour
{
    public string loadFileName;
    public List<FlagNamesCollection>  loadFlagNames;
    public bool LoadFlag;
    public int gridNum = 64;
    
    // Start is called before the first frame update
    [ContextMenu("LoadCsv and CreateField")]

    public void LoadCSVandCreateField()
    {


        DataMemory.LoadDataByCsv(loadFileName);
        DataMemory.CreateDensityField(gridNum);
        DataMemory.DisplayAllParticle(LoadFlag, loadFlagNames);

    }
    [ContextMenu("LoadByte and CreateField")]
    public void LoadByteandCreateField()
    {

        DataMemory.LoadDataByByte(loadFileName);
        DataMemory.CreateDensityField(gridNum);
        DataMemory. DisplayAllParticle(LoadFlag, loadFlagNames);

    }

    [ContextMenu("Clear memory")]
    public void ClearMemory()
    {

        DataMemory.ClearDensityMemory();
        DataMemory.ClearParticleMemory();
        DataMemory.StacksInitialize();


    }

}


}
