//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using ParticleProperty;
//using ScalarField;


//public class DataMemory : MonoBehaviour
//{

//    #region Operation Stack and DisplayInfo

//    private static Stack<int> pStack;
//    private static Stack<int> pNumStack;


//    public static void StacksInitialize()
//    {
//        pStack = new Stack<int>();
//        pNumStack = new Stack<int>();


//    }
//    public static void LoadFlagsToStack(string name)
//    {
//        int[] flags = LoadDataBybyte.StartLoadFlags(Application.dataPath + "/my/data/flags/" + name);
//        foreach (var f in flags)
//            pStack.Push(f);
//        pNumStack.Push(flags.Length);
//    }

//    public static void AddTopStacks(int index)
//    {
//        pStack.Push(index);
//    }
//    public static void AddTopNumStacks(int num)
//    {
//        pNumStack.Push(num);
//    }

//    public static Stack<int> GetpStack()
//    {
//        return pStack;
//    }
//    public static Stack<int> GetpNumStack()
//    {
//        return pNumStack;
//    }


//    public static void DeleteElementsInpStack(bool delSeed = false) //true :delete seed on the top of seedstack
//    {
//        if (pNumStack.Count == 0)
//            return;
//        int numToBePoped = pNumStack.Pop();
//        for (int i = 0; i < numToBePoped; i++)
//        {
//            pStack.Pop();
//        }


//    }


//    public static void DisplayAllParticle(bool loadFlag,string LoadFlagName)
//    {
//        StacksInitialize();
//         if(loadFlag)
//            LoadFlagsToStack(LoadFlagName);
//        DisplayParticles.DisplayMesh(GameObject.Find("PointCloudMesh"),allParticle);
//    }
//    #endregion

//    #region ParticleInfo
//    [SerializeField]
//    [HideInInspector] public List<Vector3> particleflow_dest;
//    [SerializeField]
//    static public ParticleGroup allParticle = new ParticleGroup();
//    [SerializeField]
//    static public Mesh unselected_mesh = new Mesh();
//    [SerializeField]
//    static public Mesh selected_mesh = new Mesh();
//    public static void LoadDataByByte(string loadFileName)
//    {

//        allParticle.LoadDatasetByByte(Application.dataPath + "/my/data/data/" + loadFileName);
//        Debug.Log("Load success" + " " + loadFileName + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength().x+" "+ allParticle.GetSmoothLength().y+" "+ allParticle.GetSmoothLength().z);
//    }
//    static public void LoadDataByCsv(string loadFileName)
//    {

//        allParticle.LoadDatasetByCsv(Application.dataPath + "/my/data/data/" + loadFileName);
//        Debug.Log("Load success" + " " + loadFileName + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength());
//    }
//    static public void LoadDataByTxt(string[] dataname)
//    {

//        // string[] dataname = { "data1.txt", "data2.txt" };
//        allParticle.LoadDatasetsByTxt(dataname);

//        Debug.Log("Load success" + " " + dataname[0] + " " + dataname[1] + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength());

//    }
//    static public void LoadDataByVec3s(Vector3[] v, string dataname,bool forSimulation=false)
//    {


//        allParticle.LoadDatasetByVec3s (v,dataname,forSimulation);

//        Debug.Log("Load success" + " " + dataname+ " with " + allParticle.GetParticlenum() + " particles."+" SmoothLength: "+allParticle.GetSmoothLength());

//    }

//    static public void StoreDataFlags(string storeFileName)
//    {
//        allParticle.StoreFlags(Application.dataPath + "/my/data/" + "Flags/" + storeFileName);
//    }
//    public static void ClearParticleMemory()
//    {
//        allParticle = new ParticleGroup();
//     }
//    #endregion

//    #region DensityFieldInfo
//    [SerializeField]
//    //[HideInInspector]
//    static public DensityField densityField = new DensityField();
//    [SerializeField]
//    //[HideInInspector]
//    static public DensityField densityField2 = new DensityField();

//    static public void CreateDensityField(int gridNum)
//    {

//        float step = (allParticle.XMAX - allParticle.XMIN) / gridNum ;
//        allParticle.XMAX+=step;
//        allParticle.XMIN -= step;
//        allParticle.YMAX += step;
//        allParticle.YMIN -= step;
//        allParticle.ZMAX += step;
//        allParticle.ZMIN -= step;
//        densityField.InitializeDensityFieldByGapDis(allParticle.name, allParticle.XMIN , allParticle.XMAX , gridNum, allParticle.YMIN , allParticle.YMAX , gridNum, allParticle.ZMIN , allParticle.ZMAX, gridNum);
//        densityField2.InitializeDensityFieldByGapDis(allParticle.name, allParticle.XMIN , allParticle.XMAX , gridNum, allParticle.YMIN , allParticle.YMAX , gridNum, allParticle.ZMIN , allParticle.ZMAX , gridNum);
//        Debug.Log("Create density field success" + " with " + densityField.GetNodeNum() + " nodes.");

//        Debug.Log(densityField.XNUM+" "+ densityField.YNUM+ " " + densityField.ZNUM);
//    }
//    public static void ClearDensityMemory()
//    {
//        densityField = new DensityField(); densityField2 = new DensityField();
//    }
//    #endregion








//}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticleProperty;
using ScalarField;
using System.Linq;

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
    public static void LoadFlagsToStack(string name)
    {
        int[] flags = LoadDataBybyte.StartLoadFlags(Application.dataPath + "/my/data/flags/" + allParticle.name+"_"+ name);
        //pStack.Push(flags.ToList());
        for (int i = 0; i < flags.Length; i++)
        {
           
            DataMemory.allParticle.SetTarget(flags[i], true); }
            
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




    public static void DisplayAllParticle(bool loadFlag, string LoadFlagName)
    {
        StacksInitialize();
        if (loadFlag)
            LoadFlagsToStack(LoadFlagName);
        DisplayParticles.DisplayMesh(GameObject.Find("PointCloudMesh"), allParticle);
    }
    #endregion

    #region ParticleInfo
    [SerializeField]
    [HideInInspector] public List<Vector3> particleflow_dest;
    [SerializeField]
    static public ParticleGroup allParticle = new ParticleGroup();

    public static void LoadDataByByte(string loadFileName)
    {

        allParticle.LoadDatasetByByte(Application.dataPath + "/my/data/data/" + loadFileName,loadFileName);
        Debug.Log("Load success" + " " + loadFileName + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength().x + " " + allParticle.GetSmoothLength().y + " " + allParticle.GetSmoothLength().z);
    }
    static public void LoadDataByCsv(string loadFileName)
    {

        allParticle.LoadDatasetByCsv(Application.dataPath + "/my/data/data/" + loadFileName, loadFileName);
        Debug.Log("Load success" + " " + loadFileName + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength());
    }
    static public void LoadDataByTxt(string[] dataname)
    {

        // string[] dataname = { "data1.txt", "data2.txt" };
        allParticle.LoadDatasetsByTxt(dataname);

        Debug.Log("Load success" + " " + dataname[0] + " " + dataname[1] + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength());

    }
    static public void LoadDataByVec3s(Vector3[] v, string dataname, bool forSimulation = false)
    {


        allParticle.LoadDatasetByVec3s(v, dataname, forSimulation);

        Debug.Log("Load success" + " " + dataname + " with " + allParticle.GetParticlenum() + " particles." + " SmoothLength: " + allParticle.GetSmoothLength());

    }

    static public void StoreDataFlags(string storeFileName)
    {
        allParticle.StoreFlags(Application.dataPath + "/my/data/" + "Flags/" + storeFileName);
    }
    public static void ClearParticleMemory()
    {
        allParticle = new ParticleGroup();
    }

    public static void StoreFlags(string ExtendstoreFileName)
    {
        allParticle.StoreFlags(Application.dataPath + "/my/data/" + "/Flags/" + allParticle.name+"_"+ ExtendstoreFileName);

    }
    #endregion

    #region DensityFieldInfo
    [SerializeField]
    //[HideInInspector]
    static public DensityField densityField = new DensityField();
    [SerializeField]
    //[HideInInspector]
    static public DensityField densityField2 = new DensityField();

    static public void CreateDensityField(int gridNum)
    {

        float step = (allParticle.XMAX - allParticle.XMIN) / gridNum;
        allParticle.XMAX += step;
        allParticle.XMIN -= step;
        allParticle.YMAX += step;
        allParticle.YMIN -= step;
        allParticle.ZMAX += step;
        allParticle.ZMIN -= step;
        densityField.InitializeDensityFieldByGapDis(allParticle.name, allParticle.XMIN, allParticle.XMAX, gridNum, allParticle.YMIN, allParticle.YMAX, gridNum, allParticle.ZMIN, allParticle.ZMAX, gridNum);
        densityField2.InitializeDensityFieldByGapDis(allParticle.name, allParticle.XMIN, allParticle.XMAX, gridNum, allParticle.YMIN, allParticle.YMAX, gridNum, allParticle.ZMIN, allParticle.ZMAX, gridNum);
        Debug.Log("Create density field success" + " with " + densityField.GetNodeNum() + " nodes.");

        Debug.Log(densityField.XNUM + " " + densityField.YNUM + " " + densityField.ZNUM);
    }
    public static void ClearDensityMemory()
    {
        densityField = new DensityField(); densityField2 = new DensityField();
    }
    #endregion








}
