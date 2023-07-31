using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    public string loadFileName;
    public string loadFlagName;
    public string StoreFlagsName;
    public bool LoadFlag;
    public int gridNum = 64;
    public GameObject PointCloudMeshinHand;
 
    
    // Start is called before the first frame update
    [ContextMenu("LoadCsv and CreateField")]

    public void LoadCSVandCreateField()
    {


        DataMemory.LoadDataByCsv(loadFileName);
        DataMemory.CreateDensityField(gridNum);
        DataMemory.DisplayAllParticle(LoadFlag, loadFlagName);

    }
    [ContextMenu("LoadByte and CreateField")]
    public void LoadByteandCreateField()
    {

        DataMemory.LoadDataByByte(loadFileName);
        DataMemory.CreateDensityField(gridNum);
        DataMemory. DisplayAllParticle(LoadFlag, loadFlagName);

    }

    [ContextMenu("Clear memory")]
    public void ClearMemory()
    {

        DataMemory.ClearDensityMemory();
        DataMemory.ClearParticleMemory();
        DataMemory.StacksInitialize();


    }




}
