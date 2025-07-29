using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using System.ComponentModel;
public class DataLoader : MonoBehaviour
{

    [HideInInspector] public Particles particles;
    public UnityEvent eventAfterLoadData;
    private string dataPath;

    public void LoadData(int datasetIndex, int customIndex, bool custom)
    {
        // DataStorage.StacksInitialize();

        if (!custom)
            LoadDataset(datasetIndex); //load data file
        else
            LoadCustomGenerator(customIndex); //load data by function


        transform.parent.GetComponentInChildren<PointRenderer>().Init();
        eventAfterLoadData?.Invoke(); //actions after data load
    }

    public void LoadDataset(int index)
    {
        dataPath = Application.dataPath + "/PointCloud-Visualization-Tool/data/data/";
        var n = index * 2; //exclude .meta file
        try
        {
            var files = Directory.GetFiles(dataPath).ToArray();

            if (n >= 0 && n < files.Length)
            {
                var nthFileName = Path.GetFileNameWithoutExtension(files[n]);
                var nthFileExtention = Path.GetExtension(files[n]);
                if (nthFileExtention == ".bin")
                    particles.LoadBin(dataPath, nthFileName);
                else if (nthFileExtention == ".ply")
                    particles.LoadPly(dataPath, nthFileName);
                else if (nthFileExtention == ".pcd")
                    particles.LoadPcd(dataPath, nthFileName);
                else if (nthFileExtention == ".txt")
                    particles.LoadTxt(dataPath, nthFileName);
                else if (nthFileExtention == ".csv") particles.LoadCsv(dataPath, nthFileName);

                transform.parent.gameObject.name = nthFileName;
                transform.parent.GetComponentInChildren<PointRenderer>().Init(); // set rendering parameters
            }
            else
            {
                Console.WriteLine("exceed index. Total {0} files.", files.Length);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("error: " + e.Message);
        }
    }

    public void LoadCustomGenerator(int index)
    {
        try
        {
            var mi = typeof(DataGenerator).GetMethods(BindingFlags.Public | BindingFlags.Instance |
                                                      BindingFlags.Static | BindingFlags.DeclaredOnly);
            particles.LoadVec3s((Vector3[])mi[index].Invoke(new DataGenerator(), null), mi[index].Name);
            transform.parent.gameObject.name = "Custom_" + mi[index].Name;
            transform.parent.GetComponentInChildren<PointRenderer>().Init(); // set rendering parameters
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    


    private void OnApplicationQuit()
    {
        Destroy(this);
    }

    public void SaveAsBin(string location)
    {
        particles.SaveAsBin(location);
    }

    public void SaveAsPly(string location)
    {
        particles.SaveAsPly(location);
    }

    public void SaveAsTxt(string location)
    {
        particles.SaveAsTxt(location);
    }

    public void SaveAsPcd(string location)
    {
        particles.SaveAsPcd(location);
    }
    public void SaveFlag(string ExtendstoreFileName)
    {
        particles.StoreFlags(Application.dataPath + "/PointCloud-Visualization-Tool/data/" + "/Flags/" + particles.name+"_"+ ExtendstoreFileName);

    }
    public void SaveSelectedAsNewData(string ExtendstoreFileName)
    {
        particles.SaveSelectedAsNewData(Application.dataPath + "/PointCloud-Visualization-Tool/data/" + "/data/" + particles.name+"_"+ ExtendstoreFileName+".bin");

    }

    public void SaveTargetAsNewData(string ExtendstoreFileName)
    {
        particles.SaveTargetAsNewData(Application.dataPath + "/PointCloud-Visualization-Tool/data/" + "/data/" + particles.name+"_"+ ExtendstoreFileName+".bin");

    }
}