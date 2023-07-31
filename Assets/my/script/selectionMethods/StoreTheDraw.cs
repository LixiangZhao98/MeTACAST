using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StoreTheDraw : MonoBehaviour
{
    // Start is called before the first frame update
    private List<float> timeBuffer;
    private List<Vector3> posBuffer;
    private Vector3 camPosBuffer;
    StreamWriter writer;
    string _txtPath;
    public GameObject particelinhand;

    // Update is called once per frame
    private void Start()
    {
        posBuffer = new List<Vector3>();
        timeBuffer = new List<float>();
    }

  public  void SaveText_Userstudy(string s)
    {
            FileInfo file = new FileInfo(s);
                writer = file.CreateText();
                WriteToTxT();
                writer.Flush();
                writer.Dispose();
                writer.Close();
                Debug.Log("Store success:"+s);
        }
    void SaveText()
    {
        _txtPath= Application.dataPath + "/Userstudy/user";
        for (int i=0;i<100;i++)
        {
            FileInfo file = new FileInfo(_txtPath+i.ToString()+".txt");
            if (!file.Exists)
            {
                writer = file.CreateText();
                WriteToTxT();
                writer.Flush();
                writer.Dispose();
                writer.Close();
                Debug.Log("Store success");
                break;
            }
        }
    }
    void WriteToTxT()
    {
        for(int i=0;i<posBuffer.Count;i++)
        {
            writer.WriteLine(posBuffer[i].x+" "+ posBuffer[i].y+" "+ posBuffer[i].z + " " +timeBuffer[i]);
        }
        writer.WriteLine(camPosBuffer.x + " " + camPosBuffer.y + " " + camPosBuffer.z + " " + 0f);
    }
    
  public  void SetPosBuffer(Vector3[] vs)
    {
        posBuffer.Clear();
        foreach (var v in vs)
            posBuffer.Add(v);
    }
    public void SetTimeBuffer(float[] vs)
    {
        timeBuffer.Clear();
        foreach (var v in vs)
            timeBuffer.Add(v);
    }
    public void SetCamPosBuffer(Vector3 vs)
    {
        camPosBuffer = vs;
    }

    public List<Vector3> GetPosBuffer()
    {
        return posBuffer;
    }


}
